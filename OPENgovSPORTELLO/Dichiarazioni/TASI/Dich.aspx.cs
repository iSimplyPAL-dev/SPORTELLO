using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Dichiarazioni.TASI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.TASI fncMng = new BLL.TASI();
        private static SPC_DichTASI UIOrg;
        private static string MailUser;
        private static Istanza myIstanza = new Istanza();
        private static string TipoDichiarante;

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
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.TASI, MySession.Current.Ente.IDEnte);

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
                sScript += new BLL.GestForm().GetHelp("HelpFODichTASI", MySession.Current.Ente.UrlWiki);
                RegisterScript(sScript, this.GetType());
                TipoDichiarante = "I";
                hfTypeProtocollo.Value = MySettings.GetConfig("TypeProtocollo");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Page_Init::errore::", ex);
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
                    List<GenericCategory> ListGenCat = new List<GenericCategory>();
                    List<GenericCategory> ListGenNaturaTitolo = new List<GenericCategory>();
                    List<GenericCategory> ListGenCaratteristica = new List<GenericCategory>();
                    List<GenericCategory> ListGenAgevolazioni = new List<GenericCategory>();
                    List<GenericCategory> ListGenMotivazioni = new List<GenericCategory>();
                    SPC_DichTASI myUIDich = new SPC_DichTASI();
                    ListGenTipo = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Caratteristica, string.Empty, General.TRIBUTO.TASI);
                    ListGenCat = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Categorie, string.Empty, string.Empty);
                    ListGenNaturaTitolo = new BLL.Settings().LoadConfigForDDL(string.Empty, 0, GenericCategory.TIPO.TASI_NaturaTitolo, string.Empty, string.Empty);
                    ListGenCaratteristica = new BLL.Settings().LoadConfigForDDL(string.Empty, 0, GenericCategory.TIPO.TASI_Caratteristica, string.Empty, string.Empty);
                    ListGenAgevolazioni = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TASI_Agevolazioni, string.Empty, string.Empty);

                    ListGenMotivazioni = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TASI_Motivazioni, string.Empty, string.Empty);

                    fncGen.LoadCombo(ddlTipologia, ListGenTipo, "CODICE", "DESCRIZIONE");
                    ListGenCat = ListGenCat.OrderBy(o => o.Codice).ToList();
                    fncGen.LoadCombo(ddlCat, ListGenCat, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlCaratteristica, ListGenCaratteristica, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlNaturaTitolo, ListGenNaturaTitolo, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlAgevolazioni, ListGenAgevolazioni, "CODICE", "DESCRIZIONE");

                    GrdMotivazioni.DataSource = ListGenMotivazioni;
                    GrdMotivazioni.DataBind();

                    if (MySession.Current.IdRifCalcolo > 0)
                    {
                        if (!fncMng.LoadDich(MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, -1, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {
                            lblDataInizioORG.InnerText = new FunctionGrd().FormattaDataGrd(myUIDich.DataInizio);
                            hfIDTipologiaORG.Value = myUIDich.IDTipologia.ToString();
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
                            hfIDTipologiaORG.Value = myUIDich.IDTipologia.ToString();
                            LoadForm(myUIDich);
                            LockControl();
                        }
                    }
                    else
                    {
                        if (MySession.Current.IdIstanza > 0)
                            LoadIstanza();
                        UIOrg = new SPC_DichTASI();
                        RegisterScript("$('#divTipoDichiarante').html('<p class=\"text-danger\">In qualità di " + ((UIOrg.TipoDichiarante == "P") ? "Proprietario" : "Inquilino") + "</label>');", this.GetType());
                        txtPercPos.Enabled = false;
                        if (UIOrg.TipoDichiarante != "P")
                        {
                            ShowHide("divQuotaCalcolo", false);
                        }
                        ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
                    }
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("SearchStradario", false);
                    if (MySession.Current.IdIstanza <= 0)
                    { new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte); }
                    else {
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Page_Load", "ingresso pagina", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    }
                }
                ShowHide("FileToUpload", false);
                ShowHide("divRiepilogo", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Page_Load::errore::", ex);
                LoadException(ex);
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
            try
            {
                if (((System.Web.UI.WebControls.Button)sender).ID == "CmdBackSearchStradario")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita stadario", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    ShowHide("SearchStradario", false);
                }
                else if (MySession.Current.IdIstanza > 0)
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoTASI, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Back::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "GestAllegati", "chiesto inserimento allegati", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                }
                else
                    ShowHide("FileToUpload", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GestAllegati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.SearchStradario::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaClose", "chiesto inserimento istanza chiusura", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Close::errore::", ex);
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
                SPC_DichTASI myDich = new SPC_DichTASI();
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

                            if (new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Save())
                            {
                                if (new BLL.Dichiarazioni(myIstanza).SaveDichiarazione() <= 0)
                                {
                                    sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    return;
                                }
                                myDich.IDIstanza = myIstanza.IDIstanza;
                                if (new BLL.BLLSPC_DichTASI(myDich).Save())
                                {
                                    myDich.ID = MySession.Current.IdRifCalcolo;
                                    myDich.IDEnte = myIstanza.IDEnte;
                                    myDich.IDContribuente = myIstanza.IDContribuente;
                                    myDich.IDRifOrg = -1;
                                    bool retSave = false;
                                    if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                                    {
                                        retSave = new BLL.BLLSPC_DichTASI(myDich).SaveVariazione(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich);
                                    }
                                    else
                                    {
                                        retSave = new BLL.BLLSPC_DichTASI(myDich).SaveCalcolo(myIstanza, ref sScriptDich);
                                    }
                                    if (retSave)
                                    {
                                        MySession.Current.IsInitDich = true;
                                        RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                                        if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                                        {
                                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza con ulteriore inserimento", General.TRIBUTO.TASI, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                            MySession.Current.HasNewDich = 1;
                                            sScript = "$('#lblDescrConfirm').text('Si vuole inserire un altro immobile?');";
                                            sScript += "$('#divConfirm').show();";
                                            RegisterScript(sScript, this.GetType());
                                        }
                                        else
                                        {
                                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza", General.TRIBUTO.TASI, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                            MySession.Current.HasNewDich = 2;
                                            sScript = "$('#lblErrorFO').text('Salvataggio effettuato con successo!');$('#lblErrorFO').show();$(location).attr('href', '" + UrlHelper.GetGestRiepilogoTASI + "');";
                                            RegisterScript(sScript, this.GetType());
                                        }
                                    }
                                    else {
                                        sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Protocolla::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Work::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Accept::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.Stop::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.MailBox::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "AttachRow", "agganciato strada", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GrdAllegatiRowDataBound::errore::", ex);
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
                                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GrdAllegatiRowCommand::errore::", err);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GrdStatiIstanzaRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
            }
        }
        #endregion
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">SPC_DichTASI oggetto da caricare</param>
        private void LoadForm(SPC_DichTASI myItem)
        {
            try
            {
                hfIdVia.Value = myItem.IDVia.ToString();
                TipoDichiarante = myItem.TipoDichiarante;
                RegisterScript("$('#divTipoDichiarante').html('<p class=\"text-danger\">In qualità di " + ((myItem.TipoDichiarante == "P") ? "Proprietario" : "Inquilino") + "</label>');", this.GetType());
                txtVia.Text = myItem.Via;
                txtCivico.Text = myItem.Civico;
                txtFoglio.Text = myItem.Foglio;
                txtNumero.Text = myItem.Numero;
                txtSub.Text = myItem.Sub;
                txtDataInizio.Text = new FunctionGrd().FormattaDataGrd(myItem.DataInizio);
                txtDataFine.Text = new FunctionGrd().FormattaDataGrd(myItem.DataFine);
                ddlTipologia.SelectedValue = myItem.IDTipologia.ToString();
                ddlCat.SelectedValue = myItem.IDCategoria.ToString();
                txtPercPos.Text = myItem.PercPossesso.ToString();
                ddlNaturaTitolo.SelectedValue = myItem.IDNaturaTitolo.ToString();
                txtAgEntrateContrattoAffitto.Text = myItem.AgEntrateContrattoAffitto;
                txtEstremiContrattoAffitto.Text = myItem.EstremiContrattoAffitto;
                ddlCaratteristica.SelectedValue = myItem.IDCaratteristica.ToString();
                txtRendita.Text = myItem.RenditaValore.ToString();
                ddlAgevolazioni.SelectedValue = myItem.IDAgevolazione.ToString();
                optQuotaTotale.Text = "Quota proprietario Totale (100%)";
                optQuotaRegolamento.Text = "Quota proprietario da Regolamento (" + GetQuotaProprietario() + ")";
                if (myItem.TypeQuotaCalcolo == SPC_DichTASI.TipoQuota.CalcoloDaRegolamento)
                    optQuotaRegolamento.Checked = true;
                else
                    optQuotaTotale.Checked = true;
                if (myItem.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                UIOrg = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.LoadForm::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.LoadIstanza::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione per l'abilitazione dei controlli
        /// </summary>
        private void LockControl()
        {
            if (MySession.Current.TipoIstanza == Istanza.TIPO.Cessazione)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                ddlTipologia.Enabled = false;
                optQuotaTotale.Enabled = false; optQuotaRegolamento.Enabled = false;
                 ddlCat.Enabled = false;
                txtPercPos.Enabled = false;
                ddlNaturaTitolo.Enabled = false;
                txtAgEntrateContrattoAffitto.Enabled = false;
                txtEstremiContrattoAffitto.Enabled = false;
                ddlCaratteristica.Enabled = false;
                txtRendita.Enabled = false;
                ddlAgevolazioni.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.ConsultaDich || MySession.Current.TipoIstanza == Istanza.TIPO.ConsultaCatasto)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();$('#lblForewordUI').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipologia.Enabled = false;
                optQuotaTotale.Enabled = false; optQuotaRegolamento.Enabled = false;
                ddlCat.Enabled = false;
                txtPercPos.Enabled = false;
                ddlNaturaTitolo.Enabled = false;
                txtAgEntrateContrattoAffitto.Enabled = false;
                txtEstremiContrattoAffitto.Enabled = false;
                ddlCaratteristica.Enabled = false;
                txtRendita.Enabled = false;
                ddlAgevolazioni.Enabled = false;
                ShowHide("divMotivazione", false);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Modifica || MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
            {
                txtVia.Enabled = true;
                RegisterScript("$('.BottoneMapMarker').show();", this.GetType());
                txtCivico.Enabled = true;
                txtFoglio.Enabled = true;
                txtNumero.Enabled = true;
                txtSub.Enabled = true;
                txtDataInizio.Enabled = true;
                txtDataFine.Enabled = true;
                ddlTipologia.Enabled = true;
                optQuotaTotale.Enabled = true; optQuotaRegolamento.Enabled = true;
                ddlCat.Enabled = true;
                if (TipoDichiarante == "P")
                    txtPercPos.Enabled = true;
                else
                    txtPercPos.Enabled = false;
                ddlNaturaTitolo.Enabled = true;
                txtAgEntrateContrattoAffitto.Enabled = true;
                txtEstremiContrattoAffitto.Enabled = true;
                ddlCaratteristica.Enabled = true;
                txtRendita.Enabled = true;
                ddlAgevolazioni.Enabled = true;
                ShowHide("divMotivazione", true);
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipologia.Enabled = false;
                optQuotaTotale.Enabled = false; optQuotaRegolamento.Enabled = false;
                ddlCat.Enabled = false;
                txtPercPos.Enabled = false;
                ddlNaturaTitolo.Enabled = false;
                txtAgEntrateContrattoAffitto.Enabled = false;
                txtEstremiContrattoAffitto.Enabled = false;
                ddlCaratteristica.Enabled = false;
                txtRendita.Enabled = false;
                ddlAgevolazioni.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdMotivazioni", false);
            }
            if (UIOrg.TipoDichiarante != "P")
            {
                ShowHide("divQuotaCalcolo", false);
            }
            ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
            if (MySession.Current.UserLogged.IDContribLogged != MySession.Current.UserLogged.IDContribToWork && MySession.Current.UserLogged.IDContribToWork > 0)
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
                myItem.IDTributo = General.TRIBUTO.TASI;
                myItem.Note = "";
                foreach (GridViewRow myRow in GrdMotivazioni.Rows)
                {
                    if (((CheckBox)myRow.Cells[0].FindControl("chkSel")).Checked)
                    {
                        IstanzaMotivazione myMotiv = new IstanzaMotivazione();
                        myMotiv.IDTipo = int.Parse(((HiddenField)myRow.Cells[0].FindControl("hfIdMotivazione")).Value);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.ReadFormIstanza::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// Funzione per la lettura dei dati dell'immobile dalla pagina
        /// </summary>
        /// <returns>SPC_DichTASI oggetto immobile</returns>
        private SPC_DichTASI ReadFormDich()
        {
            SPC_DichTASI NewItem = new SPC_DichTASI();
            try
            {
                NewItem.ID = default(int);
                NewItem.IDIstanza = default(int);
                NewItem.IDRifOrg = MySession.Current.IdRifCalcolo;
                NewItem.IDVia = int.Parse(hfIdVia.Value);
                NewItem.Via = txtVia.Text;
                NewItem.Civico = txtCivico.Text;
                NewItem.Esponente = "";
                NewItem.Interno = "";
                NewItem.Scala = "";
                NewItem.Foglio = txtFoglio.Text;
                NewItem.Numero = txtNumero.Text;
                NewItem.Sub = txtSub.Text;
                NewItem.DataInizio = DateTime.Parse(txtDataInizio.Text);
                if (txtDataFine.Text.Trim() != string.Empty)
                    NewItem.DataFine = DateTime.Parse(txtDataFine.Text);
                else
                    NewItem.DataFine = DateTime.MaxValue;
                NewItem.Note = "";
                NewItem.IDTipologia = int.Parse(ddlTipologia.SelectedValue);
                NewItem.DescrTipologia = ddlTipologia.SelectedItem.Text;
                if (ddlCat.SelectedValue != "")
                {
                    NewItem.IDCategoria = int.Parse(ddlCat.SelectedValue);
                    NewItem.DescrCategoria = ddlCat.SelectedItem.Text;
                }
                if (txtPercPos.Text != string.Empty)
                    NewItem.PercPossesso = decimal.Parse(txtPercPos.Text.Replace(".", ","));
                NewItem.IDNaturaTitolo = int.Parse(ddlNaturaTitolo.SelectedValue);
                NewItem.AgEntrateContrattoAffitto = txtAgEntrateContrattoAffitto.Text;
                NewItem.EstremiContrattoAffitto = txtEstremiContrattoAffitto.Text;
                NewItem.IDCaratteristica = int.Parse(ddlCaratteristica.SelectedValue);
                NewItem.RenditaValore = decimal.Parse(txtRendita.Text.Replace(".", ","));
                if (ddlAgevolazioni.SelectedValue != string.Empty)
                    NewItem.IDAgevolazione = int.Parse(ddlAgevolazioni.SelectedValue);
                 if ( optQuotaRegolamento.Checked)
                   NewItem.TypeQuotaCalcolo = SPC_DichTASI.TipoQuota.CalcoloDaRegolamento;
                else
                    NewItem.TypeQuotaCalcolo = SPC_DichTASI.TipoQuota.CalcoloTotale;
                 return NewItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.ReadFormDich::errore::", ex);
                return null;
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "LoadSearch", "cercato strada", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.LoadSearch::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione che determina il tipo di istanza che si sta creando
        /// </summary>
        /// <param name="UIDichCurrent">SPC_DichTASI oggetto immobile</param>
        /// <param name="myItem">ref Istanza oggetto istanza che si sta creando</param>
        /// <param name="sErr">ref string errore</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        private bool GetTipoIstanza(SPC_DichTASI UIDichCurrent, ref Istanza myItem, ref string sErr)
        {
            try
            {
                if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                {
                    if (((SPC_DichTASI)MySession.Current.UIDichOld).IDNaturaTitolo != UIDichCurrent.IDNaturaTitolo
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).EstremiContrattoAffitto != UIDichCurrent.EstremiContrattoAffitto
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).IDTipologia != UIDichCurrent.IDTipologia
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).TypeQuotaCalcolo!=UIDichCurrent.TypeQuotaCalcolo
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).IDCategoria != UIDichCurrent.IDCategoria
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).AgEntrateContrattoAffitto != UIDichCurrent.AgEntrateContrattoAffitto
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).IDCaratteristica != UIDichCurrent.IDCaratteristica
                            || (((SPC_DichTASI)MySession.Current.UIDichOld).IDAgevolazione == 0 ? -1 : ((SPC_DichTASI)MySession.Current.UIDichOld).IDAgevolazione) != UIDichCurrent.IDAgevolazione
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).DataInizio.ToShortDateString() != UIDichCurrent.DataInizio.ToShortDateString()
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).DataFine.ToShortDateString() != UIDichCurrent.DataFine.ToShortDateString()
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaAlter", "chiesto inserimento istanza variazione", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    }
                    else if (((SPC_DichTASI)MySession.Current.UIDichOld).Via != UIDichCurrent.Via
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).Civico != UIDichCurrent.Civico
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).Foglio != UIDichCurrent.Foglio
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).Numero != UIDichCurrent.Numero
                            || ((SPC_DichTASI)MySession.Current.UIDichOld).Sub != UIDichCurrent.Sub
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Modifica;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaUpdate", "chiesto inserimento istanza correzione", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    }
                    else
                    {
                        sErr = "$('#lblErrorFO').text('Non è stata effettuata nessuna modifica! Impossibile salvare i dati!');$('#lblErrorFO').show();";
                        return false;
                    }
                }
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                ListMyData = new BLL.Settings().LoadTipoIstanze(General.TRIBUTO.TASI, MySession.Current.TipoIstanza, false);
                foreach (GenericCategory myTipo in ListMyData)
                {
                    myItem.IDTipo = myTipo.ID;
                }
                myItem.DescrTipoIstanza = MySession.Current.TipoIstanza;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GetTipoIstanza::errore::", ex);
                sErr = "$('#lblErrorFO').text('Errore nei dati!');$('#lblErrorFO').show();";
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTipologiaIndexChanged(object sender, EventArgs e)
        {
            try
            {
                optQuotaTotale.Text = "Quota proprietario Totale (100%)";
                optQuotaRegolamento.Text = "Quota proprietario da Regolamento (" + GetQuotaProprietario() + ")";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.ddlTipologiaIndexChanged::errore::", ex);
            }
            finally
            {
                if (UIOrg.TipoDichiarante != "P")
                {
                    ShowHide("divQuotaCalcolo", false);
                }
                ManageBottoniera(General.TRIBUTO.TASI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetQuotaProprietario()
        {
            string Quota = string.Empty;
            try
            {
                List<GenericCategoryWithRate> ListItem = new BLL.Settings().LoadTariffe(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.TASI_Aliquote, ddlTipologia.SelectedItem.Text, string.Empty);
                foreach (GenericCategoryWithRate myItem in ListItem)
                {
                    Quota = myItem.PercProprietario.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Dich.GetQuotaProprietario::errore::", ex);
                Quota = string.Empty;
            }
            return Quota;
        }
    }
}