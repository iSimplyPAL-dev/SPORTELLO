using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Dichiarazioni.OSAP
{
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.OSAP fncMng = new BLL.OSAP();
        private static SPC_DichOSAP UIOrg;
        private static string MailUser;
        private static Istanza myIstanza = new Istanza();

        /// <summary>
        /// Inizializzazione della pagina con eventuale:
        /// <list type="bullet">
        /// <item>caricamento testi fissi</item>
        /// <item>caricamento help</item>
        /// <item>esposizione oggetti</item>
        /// </list>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.OSAP, MySession.Current.Ente.IDEnte);

                sScript += "$('#divDatiIstanza').hide();";
                sScript += "$('.BottoneCounter').hide();$('.BottoneWork').hide();$('.BottoneStop').hide();$('.BottoneAccept').hide();";
                sScript += "$('p#Protocolla').hide();$('p#InCarico').hide();$('p#Respingi').hide();$('p#Valida').hide();";
                sScript += "$('.lead_header').text('Dati Immobile');";
                sScript += "$('.lead_header').removeClass('col-md-2');";
                sScript += "$('.lead_header').addClass('col-md-5');";
                sScript += "$('.BottoneDiv').hide();";
                if (MySession.Current.Scope == "BO" && MySession.Current.Ente.DatiVerticali.TipoBancaDati == "I")
                    sScript += "$('.BottoneSort').show();$('p#Ribalta').show();";
                else
                    sScript += "$('.BottoneSort').hide();$('p#Ribalta').hide();";
                RegisterScript(sScript, this.GetType());

                hfTypeProtocollo.Value = MySettings.GetConfig("TypeProtocollo");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;

                if (MySession.Current.IsInitDich)
                    RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                if (!Page.IsPostBack)
                {
                    General fncGen = new General();
                    List<GenericCategory> ListGenTipo = new List<GenericCategory>();
                    List<GenericCategory> ListGenRichiedente = new List<GenericCategory>();
                    List<GenericCategory> ListGenTributo = new List<GenericCategory>();
                    List<GenericCategory> ListGenTipoDurata = new List<GenericCategory>();
                    List<GenericCategory> ListGenOccupazione = new List<GenericCategory>();
                    List<GenericCategory> ListGenClasse = new List<GenericCategory>();
                    List<GenericCategory> ListGenTipoConsistenza = new List<GenericCategory>();
                    List<GenericCategory> ListGenAgevolazioni = new List<GenericCategory>();
                    List<GenericCategory> ListGenMotivazioni = new List<GenericCategory>();
                    SPC_DichOSAP myUIDich = new SPC_DichOSAP();
                    ListGenRichiedente = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_Richiedente, string.Empty, string.Empty);
                    ListGenOccupazione = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_Occupazione, string.Empty, string.Empty);
                    ListGenClasse = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_Categoria, string.Empty, string.Empty);
                    ListGenAgevolazioni = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_Agevolazioni, string.Empty, string.Empty);
                    ListGenTipo = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_TipoDichiarazione, string.Empty, string.Empty);
                    ListGenTributo = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_Tributo, string.Empty, string.Empty);
                    ListGenTipoDurata = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_TipoDurata, string.Empty, string.Empty);
                    ListGenTipoConsistenza = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_TipoConsistenza, string.Empty, string.Empty);
                    ListGenMotivazioni = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.OSAP_Motivazioni, string.Empty, string.Empty);

                    fncGen.LoadCombo(ddlTipoAtto, ListGenTipo, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlRichiedente, ListGenRichiedente, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlTributo, ListGenTributo, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlTipoDurata, ListGenTipoDurata, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlCat, ListGenClasse, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlOccupazione, ListGenOccupazione, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlMisuraCons, ListGenTipoConsistenza, "CODICE", "DESCRIZIONE");
                    GrdAgevolazioni.DataSource = ListGenAgevolazioni;
                    GrdAgevolazioni.DataBind();

                    GrdMotivazioni.DataSource = ListGenMotivazioni;
                    GrdMotivazioni.DataBind();
                    if (MySession.Current.IdRifCalcolo > 0)
                    {
                        if (!fncMng.LoadDich(MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, -1, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {

                            lblDataInizioORG.InnerText = new FunctionGrd().FormattaDataGrd(myUIDich.DataInizio);
                            LoadForm(myUIDich);
                            LockControl();
                            MySession.Current.UIDichOld = myUIDich;
                        }
                    }
                    else if (MySession.Current.IdIstanza > 0)
                    {
                        LoadIstanza();
                        if (!fncMng.LoadDich(Istanza.TIPO.ConsultaDich, MySession.Current.Ente.IDEnte, MySession.Current.myAnag.COD_CONTRIBUENTE, MySession.Current.IdRifCalcolo, MySession.Current.IdIstanza, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {
                            lblDataInizioORG.InnerText = new FunctionGrd().FormattaDataGrd(myUIDich.DataInizio);
                            LoadForm(myUIDich);
                            LockControl();
                        }
                        if (myIstanza.DescrTipo == "Pagamento")
                        {
                            RegisterScript("$('#divDatiUI').hide()", this.GetType());
                        }
                    }
                    else
                    {
                        if (MySession.Current.IdIstanza > 0)
                            LoadIstanza();
                        UIOrg = new SPC_DichOSAP();
                        ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
                    }
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("SearchStradario", false);
                    if (MySession.Current.IdIstanza <= 0)
                    {
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                    }
                    else {
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Page_Load", "ingresso pagina", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                    }
                }
                ShowHide("FileToUpload", false);
                ShowHide("divRiepilogo", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                if (MySession.Current.Scope == "FO")
                    RegisterScript("$('#FAQ').addClass('HelpFOOSAP');", this.GetType());
                else
                    RegisterScript("$('#FAQ').addClass('HelpBOOSAP');", this.GetType());
            }
        }
        #region "Bottoni"
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            if (((System.Web.UI.WebControls.Button)sender).ID == "CmdBackSearchStradario")
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita stadario", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                ShowHide("SearchStradario", false);
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                if (MySession.Current.Scope == "BO")
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeGen, Response);
                else
                    if (MySession.Current.IsBackToTributi)
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                else
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFO_IstanzeGen, Response);
            }
            else
            {
                MySession.Current.TipoIstanza = string.Empty;
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoOSAP, Response);
            }
        }
        /// <summary>
        /// Bottone per il caricamento degli allegati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GestAllegati(object sender, EventArgs e)
        {
            try
            {
                if (chkAllegati.Checked)
                {
                    ShowHide("FileToUpload", true);
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "GestAllegati", "chiesto inserimento allegati", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                }
                else
                    ShowHide("FileToUpload", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.GestAllegati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la ricerca da stradario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchStradario(object sender, EventArgs e)
        {
            try
            {
                LoadSearch();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.SearchStradario::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        #region "Bottoni Front Office"
        /// <summary>
        /// Bottone per la richiesta di istanza di chiusura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Close(object sender, ImageClickEventArgs e)
        {
            try
            {
                MySession.Current.TipoIstanza = Istanza.TIPO.Cessazione;
                LockControl();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaClose", "chiesto inserimento istanza chiusura", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Close::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per la richiesta di istanza di variazione o inserimento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                SPC_DichOSAP myDich = new SPC_DichOSAP();
                string sMyErr = string.Empty;
                string sScriptDich = string.Empty;
                DateTime DataInizioORG = DateTime.MinValue;
                DateTime.TryParse(lblDataInizioORG.InnerText, out DataInizioORG);
                myIstanza = ReadFormIstanza();
                myDich = ReadFormDich();
                if (!GetTipoIstanza(myDich, ref myIstanza, ref sMyErr))
                {
                    sMyErr += "$('#lblErrorFO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorFO').show();";
                    RegisterScript(sMyErr, this.GetType());
                }
                else {
                    if (fncMng.FieldValidator(MySession.Current.TipoIstanza, myIstanza, myDich, DataInizioORG, out sMyErr))
                    {
                        if (myIstanza != null && myDich != null)
                        {
                            if (MySession.Current.IDDichiarazioneIstanze <= 0)
                            {
                                myIstanza.NDichiarazione = new BLL.Dichiarazioni(new Istanza() { IDEnte = MySession.Current.Ente.IDEnte }).GetNewDichiarazione();
                                MySession.Current.IDDichiarazioneIstanze = myIstanza.NDichiarazione;
                            }
                            else
                                myIstanza.NDichiarazione = MySession.Current.IDDichiarazioneIstanze;

                            if (((SPC_DichOSAP)MySession.Current.UIDichOld) != null)
                            {
                                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).SetIstanzaAnnullaPrec(((SPC_DichOSAP)MySession.Current.UIDichOld).IDIstanza);
                            }

                            if (new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Save())
                            {
                                if (new BLL.Dichiarazioni(myIstanza).SaveDichiarazione() <= 0)
                                {
                                    sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    return;
                                }
                                myDich.IDIstanza = myIstanza.IDIstanza;
                                myDich.IDEnte = myIstanza.IDEnte;
                                myDich.IDContribuente = myIstanza.IDContribuente;
                                myDich.IDRifOrg = MySession.Current.IdRifCalcolo;
                                myDich.ID = -1;
                                if (new BLL.BLLSPC_DichOSAP(myDich).Save(myIstanza, ref sScriptDich))
                                {
                                    MySession.Current.IsInitDich = true;
                                    RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                                    if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                                    {
                                        foreach (GridViewRow myItem in GrdAgevolazioni.Rows)
                                        {
                                            ((CheckBox)myItem.Cells[0].FindControl("chkSel")).Checked = false;
                                        }
                                        foreach (GridViewRow myItem in GrdMotivazioni.Rows)
                                        {
                                            ((CheckBox)myItem.Cells[0].FindControl("chkSel")).Checked = false;
                                        }
                                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza con ulteriore inserimento", General.TRIBUTO.OSAP, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                        MySession.Current.HasNewDich = 1;
                                        sScript = "$('#lblDescrConfirm').text('Si vuole inserire un altro immobile?');";
                                        sScript += "$('#divConfirm').show();";
                                        RegisterScript(sScript, this.GetType());
                                    }
                                    else
                                    {
                                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza", General.TRIBUTO.OSAP, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                        MySession.Current.HasNewDich = 2;
                                        sScript = "$('#lblErrorFO').text('Salvataggio effettuato con successo!');$('#lblErrorFO').show();$(location).attr('href', '" + UrlHelper.GetGestRiepilogoOSAP + "');";
                                        RegisterScript(sScript, this.GetType());
                                    }
                                }
                                else {
                                    sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                }
                            }
                            else
                            {
                                sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                RegisterScript(sScript, this.GetType());
                            }
                        }
                        else {
                            sScript = "$('#lblErrorFO').text('Errore nei dati!');$('#lblErrorFO').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                    else
                    {
                        sMyErr += "$('#lblErrorFO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorFO').show();";
                        RegisterScript(sMyErr, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Save::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #region "Bottoni Back Office"
        /// <summary>
        /// Bottone per la protocollazione dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Protocolla(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Protocolla(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in Protocollazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Protocollazione effettuata con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Protocolla::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la presa in carico dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Work(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).InCarico(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in Presa in carico!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                     RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Presa in carico con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Work::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la validazione dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Accept(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Valida(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in Validazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                     RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Validazione effettuata con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                     sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Accept::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il rigetto dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Stop(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Respingi(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in rifiuto istanza!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                     RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Istanza Respinta con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.Stop::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la richiesta di integrazione dati via mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MailBox(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (MySession.Current.Scope == "FO")
                {
                    MailUser = MySession.Current.Ente.Mail.BackOffice;
                }
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Integrazione(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in invio comunicazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Comunicazione inviata con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '";
                    if (MySession.Current.Scope == "FO")
                    {
                        sScript += UrlHelper.GetFO_IstanzeGen;
                    }
                    else {
                        sScript += UrlHelper.GetBO_IstanzeGen;
                    }
                    sScript += "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.MailBox::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        #endregion
        #endregion
        #region "Griglie"  
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdStradarioRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDSetting = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "AttachRow":
                        foreach (GridViewRow myCfg in GrdStradario.Rows)
                        {
                            if (((HiddenField)myCfg.Cells[0].FindControl("hdIdStradario")).Value == IDSetting.ToString())
                            {
                                txtVia.Text = ((Label)myCfg.Cells[0].FindControl("lblStradario")).Text;
                                hfIdVia.Value = IDSetting.ToString();
                                break;
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "AttachRow", "agganciato strada", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Funzione di gestione cambio pagina della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdStradarioPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdAllegatiRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtn($(this).attr('id'));");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.GrdAllegatiRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdAllegatiRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string Allegati, sScript;
                Allegati = sScript = string.Empty;
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "RowDownload":
                        List<IstanzaAllegato> myAll = new BLL.IstanzaAllegati(new IstanzaAllegato()).LoadAllegati(-1, IDRow);
                        if (myAll.Count > 0)
                        {
                            try
                            {
                                Response.ContentType = myAll[0].FileMIMEType;
                                Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", myAll[0].FileName));
                                Response.BinaryWrite(myAll[0].PostedFile);
                                Response.End();
                            }
                            catch (Exception err)
                            {
                                if (err.Message != "Thread was being aborted.")
                                {
                                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.GrdAllegatiRowCommand::errore::", err);
                                }
                            }
                        }
                        else
                        {
                            sScript = "$('#lblErrorFO').text('Allegato non disponibile!');$('#lblErrorFO').show();";
                        }
                        break;
                    default:
                        break;
                }
                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).LoadFormData(out sScript, ref GrdStatiIstanza, ref GrdAllegati);
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.GrdStatiIstanzaRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            }
        }
        #endregion
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">SPC_DichOSAP oggetto da caricare</param>
        private void LoadForm(SPC_DichOSAP myItem)
        {
            try
            {
                ddlTipoAtto.SelectedValue = myItem.IDTipoAtto.ToString();
                txtDataAtto.Text = new FunctionGrd().FormattaDataGrd(myItem.DataAtto);
                txtNumAtto.Text = myItem.NAtto;
                ddlRichiedente.SelectedValue = myItem.IDRichiedente.ToString();
                ddlTributo.SelectedValue = myItem.IDTributo.ToString();
                hfIdVia.Value = myItem.IDVia.ToString();
                txtVia.Text = myItem.Via;
                if (myItem.Civico == string.Empty)
                    txtCivico.Text = "SNC";
                else
                    txtCivico.Text = myItem.Civico;
                txtDataInizio.Text = new FunctionGrd().FormattaDataGrd(myItem.DataInizio);
                txtDataFine.Text = new FunctionGrd().FormattaDataGrd(myItem.DataFine);
                ddlTipoDurata.SelectedValue = myItem.IDTipoDurata.ToString();
                txtDurata.Text = myItem.Durata.ToString();
                ddlCat.SelectedValue = myItem.IDCategoria.ToString();
                ddlOccupazione.SelectedValue = myItem.IDOccupazione.ToString();
                ddlMisuraCons.SelectedValue = myItem.IDConsistenza.ToString();
                chkAttrazione.Checked = myItem.IsAttrazione;
                txtCons.Text = myItem.Consistenza.ToString();
                txtPercMagg.Text = myItem.PercMagg.ToString();
                txtImpDetraz.Text = myItem.ImpDetraz.ToString();
                foreach (GenericCategory myAg in myItem.ListAgevolazioni)
                {
                    foreach (GridViewRow myRow in GrdAgevolazioni.Rows)
                    {
                        if (myAg.ID == int.Parse(((HiddenField)myRow.FindControl("hfIdAgevolazione")).Value))
                        {
                            ((CheckBox)myRow.FindControl("chkSel")).Checked = true;
                            break;
                        }
                    }
                }
                UIOrg = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.LoadForm::errore::", ex);
                throw new Exception();
            }
        }
        /// <summary>
        /// Funzione di caricamento dati istanza
        /// </summary>
        private void LoadIstanza()
        {
            try
            {
                List<Istanza> listDati = new List<Istanza>();
                string sScript = string.Empty;

                if (MySession.Current.IdIstanza > 0)
                {
                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze("", 0, DateTime.MaxValue, "", "", "", "", MySession.Current.IdIstanza, -1, true, out listDati))
                        RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina.');$('#lblErrorFO').show();", this.GetType());
                    else {
                        foreach (Istanza myItem in listDati)
                        {
                            myIstanza = myItem;
                            myIstanza.DescrTipoIstanza = myIstanza.DescrTipo;
                            myIstanza.ListDatiDich = UIOrg;
                            if (MySession.Current.myAnag == null)
                            {
                                MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(myItem.IDContribuente, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                                List<UserRole> ListGen = new BLL.Settings().LoadUserRole("", MySession.Current.myAnag.CodiceFiscale, true, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser);
                                if (ListGen.Count > 0)
                                {
                                    MailUser = ListGen[0].NameUser;
                                }
                                else
                                {
                                    sScript += "$('#lblErrorFO').text('Errore in caricamento pagina.');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    break;
                                }
                            }
                            else if (MySession.Current.myAnag.COD_CONTRIBUENTE != myItem.IDContribuente)
                            {
                                MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(myItem.IDContribuente, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                            }

                            new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).LoadFormData(out sScript, ref GrdStatiIstanza, ref GrdAllegati);
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.LoadIstanza::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione per l'abilitazione dei controlli
        /// </summary>
        private void LockControl()
        {
            if (MySession.Current.TipoIstanza == Istanza.TIPO.Modifica)
            {
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipoDurata.Enabled = false;
                txtDurata.Enabled = false;
                ddlCat.Enabled = false;
                ddlOccupazione.Enabled = false;
                ddlMisuraCons.Enabled = false;
                chkAttrazione.Enabled = false;
                txtCons.Enabled = false;
                txtPercMagg.Enabled = false;
                txtImpDetraz.Enabled = false;
                GrdAgevolazioni.Enabled = false;
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
            {
                ddlTipoAtto.Enabled = false;
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
                ddlRichiedente.Enabled = false;
                ddlTributo.Enabled = false;
                txtVia.Enabled = false;
                txtCivico.Enabled = false;
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Cessazione)
            {
                ddlTipoAtto.Enabled = false;
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
                ddlRichiedente.Enabled = false;
                ddlTributo.Enabled = false;
                txtVia.Enabled = false;
                txtCivico.Enabled = false;
                txtDataInizio.Enabled = false;
                ddlTipoDurata.Enabled = false;
                txtDurata.Enabled = false;
                ddlCat.Enabled = false;
                ddlOccupazione.Enabled = false;
                ddlMisuraCons.Enabled = false;
                chkAttrazione.Enabled = false;
                txtCons.Enabled = false;
                txtPercMagg.Enabled = false;
                txtImpDetraz.Enabled = false;
                GrdAgevolazioni.Enabled = false;
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                ddlTipoAtto.Enabled = false;
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
                ddlRichiedente.Enabled = false;
                ddlTributo.Enabled = false;
                txtVia.Enabled = false;
                txtCivico.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipoDurata.Enabled = false;
                txtDurata.Enabled = false;
                ddlCat.Enabled = false;
                ddlOccupazione.Enabled = false;
                ddlMisuraCons.Enabled = false;
                chkAttrazione.Enabled = false;
                txtCons.Enabled = false;
                txtPercMagg.Enabled = false;
                txtImpDetraz.Enabled = false;
                GrdAgevolazioni.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdMotivazioni", false);
            }
            else
            {
                ddlTipoAtto.Enabled = false;
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
                ddlRichiedente.Enabled = false;
                ddlTributo.Enabled = false;
                txtVia.Enabled = false;
                txtCivico.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipoDurata.Enabled = false;
                txtDurata.Enabled = false;
                ddlCat.Enabled = false;
                ddlOccupazione.Enabled = false;
                ddlMisuraCons.Enabled = false;
                chkAttrazione.Enabled = false;
                txtCons.Enabled = false;
                txtPercMagg.Enabled = false;
                txtImpDetraz.Enabled = false;
                GrdAgevolazioni.Enabled = false;
                ShowHide("divMotivazione", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", false);
            }

            ManageBottoniera(General.TRIBUTO.OSAP, UIOrg.Stato);
            if (MySession.Current.UserLogged.IDDelegato > 0)
            {
                RegisterScript("$('.BottoneDiv').hide();$('.BottoneDivIstanza').hide();$('.BottoneSave').hide();", this.GetType());
            }
        }
        #endregion
        #region "Read Data From Form"
        /// <summary>
        /// Funzione per il caricamento dell'istanza dalla pagina
        /// </summary>
        /// <returns>Istanza oggetto istanza</returns>
        private Istanza ReadFormIstanza()
        {
            Istanza myItem = new Istanza();
            try
            {
                myItem.DataPresentazione = DateTime.Now;
                myItem.DataInCarico = DateTime.MaxValue;
                myItem.DataRespinta = DateTime.MaxValue;
                myItem.DataValidata = DateTime.MaxValue;
                myItem.IDContribuente = MySession.Current.UserLogged.IDContribToWork;
                myItem.IDDelegato = (MySession.Current.UserLogged.IDContribToWork != MySession.Current.UserLogged.IDContribLogged) ? MySession.Current.UserLogged.IDContribLogged : -1;
                myItem.IDEnte = MySession.Current.Ente.IDEnte;
                myItem.IDIstanza = -1;
                myItem.IDStato = Istanza.STATO.Presentata;
                myItem.IDTributo = General.TRIBUTO.OSAP;
                myItem.Note = "";

                foreach (GridViewRow myRow in GrdMotivazioni.Rows)
                {
                    if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                    {
                        IstanzaMotivazione myMotiv = new IstanzaMotivazione();
                        myMotiv.IDTipo = int.Parse(((HiddenField)myRow.FindControl("hfIdMotivazione")).Value);
                        myMotiv.Note = myRow.Cells[1].Text;
                        myItem.ListMotivazioni.Add(myMotiv);
                    }
                }
                if (txtMotivazione.Text.Trim() != string.Empty)
                {
                    IstanzaMotivazione myMotiv = new IstanzaMotivazione();
                    myMotiv.IDTipo = -1;
                    myMotiv.Note = txtMotivazione.Text;
                    myItem.ListMotivazioni.Add(myMotiv);
                }
                if (chkAllegati.Checked)
                {
                    HttpFileCollection ListFiles = Request.Files;
                    List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
                    List<IstanzaAllegato> ListDichAttach = myItem.ListAllegati;
                    if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Istanza, ref ListMailAttachments, ref ListDichAttach))
                    {
                        string sScript = "$('#lblErrorFO').text('Errore in lettura allegati!');$('#lblErrorFO').show();";
                        RegisterScript(sScript, this.GetType());
                        return null;
                    }
                }

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.ReadFormIstanza::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// Funzione per la lettura dei dati dell'immobile dalla pagina
        /// </summary>
        /// <returns>SPC_DichOSAP oggetto immobile</returns>
        private SPC_DichOSAP ReadFormDich()
        {
            SPC_DichOSAP NewItem = new SPC_DichOSAP();
            try
            {
                NewItem.ID = -1;
                NewItem.IDIstanza = -1;
                NewItem.IDRifOrg = MySession.Current.IdRifCalcolo;
                NewItem.IDTipoAtto = int.Parse(ddlTipoAtto.SelectedValue);
                NewItem.DescrTipoAtto = ddlTipoAtto.SelectedItem.Text;
                if (txtDataAtto.Text.Trim() != string.Empty)
                    NewItem.DataAtto = DateTime.Parse(txtDataAtto.Text);
                NewItem.NAtto = txtNumAtto.Text;
                NewItem.IDRichiedente = int.Parse(ddlRichiedente.SelectedValue);
                NewItem.DescrRichiedente = ddlRichiedente.SelectedItem.Text;
                NewItem.IDTributo = int.Parse(ddlTributo.SelectedValue);
                NewItem.DescrTributo = ddlTributo.SelectedItem.Text;
                NewItem.IDVia = int.Parse(hfIdVia.Value);
                NewItem.Via = txtVia.Text;
                NewItem.Civico = txtCivico.Text;
                NewItem.DataInizio = DateTime.Parse(txtDataInizio.Text);
                if (txtDataFine.Text.Trim() != string.Empty)
                    NewItem.DataFine = DateTime.Parse(txtDataFine.Text);
                else
                    NewItem.DataFine = DateTime.MaxValue;
                NewItem.IDTipoDurata = int.Parse(ddlTipoDurata.SelectedValue);
                NewItem.DescrTipoDurata = ddlTipoDurata.SelectedItem.Text;
                NewItem.Durata = int.Parse(txtDurata.Text);
                NewItem.IDCategoria = int.Parse(ddlCat.SelectedValue);
                NewItem.DescrCategoria = ddlCat.SelectedItem.Text;
                NewItem.IDOccupazione = int.Parse(ddlOccupazione.SelectedValue);
                NewItem.DescrOccupazione = ddlOccupazione.SelectedItem.Text;
                NewItem.IDConsistenza = int.Parse(ddlMisuraCons.SelectedValue);
                NewItem.DescrConsistenza = ddlMisuraCons.SelectedItem.Text;
                NewItem.IsAttrazione = chkAttrazione.Checked;
                NewItem.Consistenza = decimal.Parse(txtCons.Text.Replace(".", ","));
                if (txtPercMagg.Text != string.Empty)
                    NewItem.PercMagg = decimal.Parse(txtPercMagg.Text.Replace(".", ","));
                if (txtImpDetraz.Text != string.Empty)
                    NewItem.ImpDetraz = decimal.Parse(txtImpDetraz.Text.Replace(".", ","));
                List<GenericCategory> ListMyAgevolaz = new List<GenericCategory>();
                foreach (GridViewRow myRow in GrdAgevolazioni.Rows)
                {
                    if (((CheckBox)myRow.Cells[0].FindControl("chkSel")).Checked)
                    {
                        GenericCategory myItem = new GenericCategory();
                        myItem = LoadAgevolazFromGrd(myRow);
                        ListMyAgevolaz.Add(myItem);
                    }
                }
                NewItem.ListAgevolazioni = ListMyAgevolaz;

                return NewItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.ReadFormDich::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <returns></returns>
        private GenericCategory LoadAgevolazFromGrd(GridViewRow myRow)
        {
            GenericCategory myItem = new GenericCategory();
            try
            {
                myItem.ID = int.Parse(((HiddenField)myRow.Cells[0].FindControl("hfIdAgevolazione")).Value.ToString());
                myItem.Codice = ((HiddenField)myRow.Cells[0].FindControl("hfIdAgevolazione")).Value;
                myItem.Descrizione = myRow.Cells[1].Text;
                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.OSAP.LoadAgevolazFromGrd::errore::", ex);
                return myItem;
            }
        }
        #endregion
        /// <summary>
        /// Funzione di caricamento dati in griglia
        /// </summary>
        /// <param name="page">int numero di pagina su cui posizionare la visualizzazione</param>
        private void LoadSearch(int? page = 0)
        {
            BLL.Settings fncGen = new BLL.Settings();
            try
            {
                GrdStradario.DataSource = fncGen.LoadStradario(MySession.Current.Ente.IDEnte, txtSearch.Text);
                if (page.HasValue)
                    GrdStradario.PageIndex = page.Value;
                GrdStradario.DataBind();
                ShowHide("SearchStradario", true);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "LoadSearch", "cercato strada", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.LoadSearch::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione che determina il tipo di istanza che si sta creando
        /// </summary>
        /// <param name="UIDichCurrent">SPC_DichOSAP oggetto immobile</param>
        /// <param name="myItem">ref Istanza oggetto istanza che si sta creando</param>
        /// <param name="sErr">ref string errore</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        private bool GetTipoIstanza(SPC_DichOSAP UIDichCurrent, ref Istanza myItem, ref string sErr)
        {
            try
            {
                if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                {
                    if (((SPC_DichOSAP)MySession.Current.UIDichOld).ListAgevolazioni != UIDichCurrent.ListAgevolazioni
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).Consistenza != UIDichCurrent.Consistenza
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IDCategoria != UIDichCurrent.IDCategoria
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).Durata != UIDichCurrent.Durata
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IDConsistenza != UIDichCurrent.IDConsistenza
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IDTipoDurata != UIDichCurrent.IDTipoDurata
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IDTributo != UIDichCurrent.IDTributo
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).ImpDetraz != UIDichCurrent.ImpDetraz
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IsAttrazione != UIDichCurrent.IsAttrazione
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).PercMagg != UIDichCurrent.PercMagg
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).DataInizio.ToShortDateString() != UIDichCurrent.DataInizio.ToShortDateString()
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).DataFine.ToShortDateString() != UIDichCurrent.DataFine.ToShortDateString()
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaAlter", "chiesto inserimento istanza variazione", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                    }
                    else if (((SPC_DichOSAP)MySession.Current.UIDichOld).Via != UIDichCurrent.Via
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).DataAtto != UIDichCurrent.DataAtto
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IDRichiedente != UIDichCurrent.IDRichiedente
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).IDTipoAtto != UIDichCurrent.IDTipoAtto
                            || ((SPC_DichOSAP)MySession.Current.UIDichOld).NAtto != UIDichCurrent.NAtto
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Modifica;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaUpdate", "chiesto inserimento istanza correzione", General.TRIBUTO.OSAP, "", MySession.Current.Ente.IDEnte);
                    }
                    else
                    {
                        sErr = "$('#lblErrorFO').text('Non è stata effettuata nessuna modifica! Impossibile salvare i dati!');$('#lblErrorFO').show();";
                        return false;
                    }
                }
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                ListMyData = new BLL.Settings().LoadTipoIstanze(General.TRIBUTO.OSAP, MySession.Current.TipoIstanza, false);
                foreach (GenericCategory myTipo in ListMyData)
                {
                    myItem.IDTipo = myTipo.ID;
                }
                myItem.DescrTipoIstanza = MySession.Current.TipoIstanza;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich.GetTipoIstanza::errore::", ex);
                sErr = "$('#lblErrorFO').text('Errore nei dati!');$('#lblErrorFO').show();";
                return false;
            }
        }
    }
}