using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Dichiarazioni.ICP
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.ICP fncMng = new BLL.ICP();
        private static List<SPC_DichICP> UIOrg;
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
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.ICP, MySession.Current.Ente.IDEnte);

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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Page_Init::errore::", ex);
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
                    List<GenericCategory> ListGenMotivazioni = new List<GenericCategory>();
                    List<SPC_DichICP> myUIDich = new List<SPC_DichICP>();
                    ListGenMotivazioni = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICP_Motivazioni, string.Empty, string.Empty);
                    GrdMotivazioni.DataSource = ListGenMotivazioni;
                    GrdMotivazioni.DataBind();

                    if (MySession.Current.IdRifCalcolo > 0)
                    {
                        if (!fncMng.LoadDich(MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, -1, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {
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
                            LoadForm(myUIDich);
                            LockControl();
                        }
                    }
                    else
                    {
                        if (myUIDich.Count == 0)
                            myUIDich.Add(new SPC_DichICP());
                        GrdUI.DataSource = myUIDich;
                        GrdUI.DataBind();
                        if (MySession.Current.IdIstanza > 0)
                            LoadIstanza();
                        UIOrg = new List<SPC_DichICP>();
                        ManageBottoniera(General.TRIBUTO.ICP, "");
                    }
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("SearchStradario", false);
                    if (MySession.Current.IdIstanza <= 0)
                    { new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte); }
                    else { new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Page_Load", "ingresso pagina", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte); }
                }
                ShowHide("FileToUpload", false);
                ShowHide("divRiepilogo", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                if (MySession.Current.Scope == "FO")
                    RegisterScript("$('#FAQ').addClass('HelpFOICP');", this.GetType());
                else
                    RegisterScript("$('#FAQ').addClass('HelpBOICP');", this.GetType());
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita stadario", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                ShowHide("SearchStradario", false);
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoICP, Response);
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "GestAllegati", "chiesto inserimento allegati", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                }

                else
                {
                    ShowHide("FileToUpload", false);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GestAllegati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.ICP, UIOrg[0].Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.SearchStradario::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICP, "");
            }
        }
        #region "Bottoni Front Office"
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
                List<SPC_DichICP> myDich = new List<SPC_DichICP>();
                string sMyErr = string.Empty;
                string sScriptDich = string.Empty;
                myIstanza = ReadFormIstanza();
                myDich = ReadFormDich();
                if (!GetTipoIstanza(myDich, ref myIstanza, ref sMyErr))
                {
                    sMyErr += "$('#lblErrorFO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorFO').show();";
                    RegisterScript(sMyErr, this.GetType());
                }
                else {
                    if (fncMng.FieldValidator(MySession.Current.TipoIstanza, myIstanza, myDich, out sMyErr))
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

                            if (((SPC_DichICP)MySession.Current.UIDichOld) != null)
                            {
                                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).SetIstanzaAnnullaPrec(((SPC_DichICP)MySession.Current.UIDichOld).IDIstanza);
                            }

                            if (new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Save())
                            {
                                if (new BLL.Dichiarazioni(myIstanza).SaveDichiarazione() <= 0)
                                {
                                    sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    return;
                                }
                                foreach (SPC_DichICP myItem in myDich)
                                {
                                    myItem.IDIstanza = myIstanza.IDIstanza;
                                    myItem.IDEnte = myIstanza.IDEnte;
                                    myItem.IDContribuente = myIstanza.IDContribuente;
                                    myItem.IDRifOrg = MySession.Current.IdRifCalcolo;
                                    myItem.ID = -1;
                                    if (new BLL.BLLSPC_DichICP(myItem).Save(myIstanza, ref sScriptDich))
                                    {
                                    }
                                    else {
                                        sScript = "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                        RegisterScript(sScript, this.GetType());
                                        return;
                                    }
                                }
                                MySession.Current.IsInitDich = true;
                                RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                                if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                                {
                                    myDich = new List<SPC_DichICP>();
                                    myDich.Add(new SPC_DichICP());
                                    GrdUI.DataSource = myDich;
                                    GrdUI.DataBind();
                                    foreach (GridViewRow myItem in GrdMotivazioni.Rows)
                                    {
                                        ((CheckBox)myItem.Cells[0].FindControl("chkSel")).Checked = false;
                                    }
                                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza con ulteriore inserimento", General.TRIBUTO.ICP, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                    MySession.Current.HasNewDich = 1;
                                    sScript = "if (confirm('Si vuole inserire un’altra dichiarazione?')){Azzera();} else {$(location).attr('href', '" + UrlHelper.GetGestRiepilogoICP + "');}";
                                    RegisterScript(sScript, this.GetType());
                                }
                                else
                                {
                                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza", General.TRIBUTO.ICP, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                    MySession.Current.HasNewDich = 2;
                                    sScript = "$('#lblErrorFO').text('Salvataggio effettuato con successo!');$('#lblErrorFO').show();$(location).attr('href', '" + UrlHelper.GetGestRiepilogoICP + "');";
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Save::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Protocolla::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICP, "");
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Work::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICP, "");
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
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Valida(Request.Files, txtMotivazione.Text,MailUser, out sMyErr))
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Accept::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICP, "");
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.Stop::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICP, "");
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.MailBox::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICP, "");
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
                List<SPC_DichICP> ListGen = new List<SPC_DichICP>();
                switch (e.CommandName)
                {
                    case "AttachRow":
                        foreach (GridViewRow myRow in GrdUI.Rows)
                        {
                            SPC_DichICP myItem = new SPC_DichICP();
                            myItem = fncMng.LoadUIFromGrd(myRow, ((txtDataAtto.Text != string.Empty) ? DateTime.Parse(txtDataAtto.Text) : DateTime.MaxValue), txtNumAtto.Text);
                            if (myItem != null)
                            {
                                if (myItem.ID.ToString() == hfIdRow.Value)
                                {
                                    foreach (GridViewRow myVia in GrdStradario.Rows)
                                    {
                                        if (((HiddenField)myVia.FindControl("hdIdStradario")).Value == IDSetting.ToString())
                                        {
                                            myItem.Via = ((Label)myVia.FindControl("lblStradario")).Text;
                                            myItem.IDVia = int.Parse(IDSetting.ToString());
                                            break;
                                        }
                                    }
                                }
                                ListGen.Add(myItem);
                            }
                            else
                            {
                                return;
                            }
                        }
                        GrdUI.DataSource = ListGen;
                        GrdUI.DataBind();

                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "AttachRow", "agganciato strada", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICP, "");
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GrdAllegatiRowDataBound::errore::", ex);
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
                                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GrdAllegatiRowCommand::errore::", err);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GrdStatiIstanzaRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICP, UIOrg[0].Stato);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUIRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlRow = (DropDownList)e.Row.FindControl("ddlTipologia");
                    List<GenericCategory> ListMyData = new List<GenericCategory>();
                    ListMyData = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICP_Tipologia, string.Empty, string.Empty);
                    new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                    ddlRow.SelectedValue = ((SPC_DichICP)e.Row.DataItem).IDTipologia.ToString();

                    ddlRow = (DropDownList)e.Row.FindControl("ddlCaratteristica");
                    ListMyData = new List<GenericCategory>();
                    ListMyData = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICP_Caratteristica, string.Empty, string.Empty);
                    new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                    ddlRow.SelectedValue = ((SPC_DichICP)e.Row.DataItem).IDCaratteristica.ToString();

                    ddlRow = (DropDownList)e.Row.FindControl("ddlTipoDurata");
                    ListMyData = new List<GenericCategory>();
                    ListMyData = new BLL.Settings().LoadConfigForDDL("", 0, GenericCategory.TIPO.ICP_TipoDurata, string.Empty, string.Empty);
                    new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                    ddlRow.SelectedValue = ((SPC_DichICP)e.Row.DataItem).IDTipoDurata.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GrdUIRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUIRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                int IDRow = int.Parse(e.CommandArgument.ToString());
                List<SPC_DichICP> ListMyData = new List<SPC_DichICP>();

                foreach (GridViewRow myRow in GrdUI.Rows)
                {
                    SPC_DichICP myItem = new SPC_DichICP();
                    myItem = fncMng.LoadUIFromGrd(myRow, ((txtDataAtto.Text != string.Empty) ? DateTime.Parse(txtDataAtto.Text) : DateTime.MaxValue), txtNumAtto.Text);
                    ListMyData.Add(myItem);
                }
                SPC_DichICP mySetting = new SPC_DichICP { ID = IDRow };
                switch (e.CommandName)
                {
                    case "FindRow":
                        if (IDRow > 0)
                        {
                            sScript = "$('#lblErrorFO').text('Impossibile cambiare Ente');$('#lblErrorFO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                        else {
                            hfIdRow.Value = IDRow.ToString();
                            ShowHide("SearchStradario", true);
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "FindRow", "ricerca stradario", "", "", "");
                        }
                        break;
                    case "DeleteRow":
                        foreach (SPC_DichICP myItem in ListMyData)
                        {
                            if (myItem.ID == IDRow)
                            {
                                ListMyData.Remove(myItem);
                                break;
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "DeleteRow", "eliminato vano", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        ListMyData.Add(new SPC_DichICP());
                        ListMyData = ListMyData.OrderBy(o => o.Qta).ThenBy(o => o.ID).ToList();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "default", "nuovo vano", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                        break;
                }
                GrdUI.DataSource = ListMyData;
                GrdUI.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GrdUIRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICP, "");
            }
        }
        #endregion
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">List<SPC_DichICP> oggetto da caricare</param>
        private void LoadForm(List<SPC_DichICP> myItem)
        {
            try
            {
                txtDataAtto.Text = new FunctionGrd().FormattaDataGrd(myItem[0].DataAtto);
                txtNumAtto.Text = myItem[0].NAtto;
                if (myItem.Count == 0)
                    myItem.Add(new SPC_DichICP());
                GrdUI.DataSource = myItem;
                GrdUI.DataBind();
                UIOrg = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.LoadForm::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.LoadIstanza::errore::", ex);
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
                GrdUI.Enabled = false;
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
            {
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Cessazione)
            {
                foreach (GridViewRow myRow in GrdUI.Rows)
                {
                    ((TextBox)myRow.FindControl("txtVia")).Enabled = false;
                    ((TextBox)myRow.FindControl("txtCivico")).Enabled = false;
                    ((DropDownList)myRow.FindControl("ddlTipologia")).Enabled = false;
                    ((DropDownList)myRow.FindControl("ddlCaratteristica")).Enabled = false;
                    ((DropDownList)myRow.FindControl("ddlTipoDurata")).Enabled = false;
                    ((TextBox)myRow.FindControl("txtMezzo")).Enabled = false;
                    ((TextBox)myRow.FindControl("txtDataInizio")).Enabled = false;
                    ((TextBox)myRow.FindControl("txtQta")).Enabled = false;
                    ((ImageButton)myRow.FindControl("imgDelete")).Enabled = false;
                }
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
                GrdUI.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdMotivazioni", false);
            }
            else
            {
                txtDataAtto.Enabled = false;
                txtNumAtto.Enabled = false;
                GrdUI.Enabled = false;
            }
            ManageBottoniera(General.TRIBUTO.ICP, "");
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
                myItem.IDTributo = General.TRIBUTO.ICP;
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.ReadFormIstanza::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// Funzione per la lettura dei dati dell'immobile dalla pagina
        /// </summary>
        /// <returns>List<SPC_DichICP> array oggetto immobile</returns>
        private List<SPC_DichICP> ReadFormDich()
        {
            try
            {
                List<SPC_DichICP> ListMyData = new List<SPC_DichICP>();
                foreach (GridViewRow myRow in GrdUI.Rows)
                {
                    SPC_DichICP myItem = new SPC_DichICP();
                    myItem = fncMng.LoadUIFromGrd(myRow, ((txtDataAtto.Text != string.Empty) ? DateTime.Parse(txtDataAtto.Text) : DateTime.MaxValue), txtNumAtto.Text);
                    ListMyData.Add(myItem);
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.ReadFormDich::errore::", ex);
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "LoadSearch", "cercato strada", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.LoadSearch::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione che determina il tipo di istanza che si sta creando
        /// </summary>
        /// <param name="UIDichCurrent">List<SPC_DichICP> array oggetto immobile</param>
        /// <param name="myItem">ref Istanza oggetto istanza che si sta creando</param>
        /// <param name="sErr">ref string errore</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        private bool GetTipoIstanza(List<SPC_DichICP> UIDichCurrent, ref Istanza myItem, ref string sErr)
        {
            try
            {
                foreach (SPC_DichICP myUI in UIDichCurrent)
                {
                    if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                    {
                        if (((SPC_DichICP)MySession.Current.UIDichOld).IDCaratteristica != myUI.IDCaratteristica
                                || ((SPC_DichICP)MySession.Current.UIDichOld).IDTipoDurata != myUI.IDTipoDurata
                                || ((SPC_DichICP)MySession.Current.UIDichOld).IDTipologia != myUI.IDTipologia
                                || ((SPC_DichICP)MySession.Current.UIDichOld).Mezzo != myUI.Mezzo
                                || ((SPC_DichICP)MySession.Current.UIDichOld).Qta != myUI.Qta
                                || ((SPC_DichICP)MySession.Current.UIDichOld).DataInizio.ToShortDateString() != myUI.DataInizio.ToShortDateString()
                                || ((SPC_DichICP)MySession.Current.UIDichOld).DataFine.ToShortDateString() != myUI.DataFine.ToShortDateString()
                            )
                        {
                            MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaAlter", "chiesto inserimento istanza variazione", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                            break;
                        }
                        else if (((SPC_DichICP)MySession.Current.UIDichOld).Via != myUI.Via
                                || ((SPC_DichICP)MySession.Current.UIDichOld).Civico != myUI.Civico
                                || ((SPC_DichICP)MySession.Current.UIDichOld).DataAtto != myUI.DataAtto
                                || ((SPC_DichICP)MySession.Current.UIDichOld).NAtto != myUI.NAtto
                            )
                        {
                            MySession.Current.TipoIstanza = Istanza.TIPO.Modifica;
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaUpdate", "chiesto inserimento istanza correzione", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                        }
                        else
                        {
                            sErr = "$('#lblErrorFO').text('Non è stata effettuata nessuna modifica! Impossibile salvare i dati!');$('#lblErrorFO').show();";
                        }
                    }
                }
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                ListMyData = new BLL.Settings().LoadTipoIstanze(General.TRIBUTO.ICP, MySession.Current.TipoIstanza, false);
                foreach (GenericCategory myTipo in ListMyData)
                {
                    myItem.IDTipo = myTipo.ID;
                }
                myItem.DescrTipoIstanza = MySession.Current.TipoIstanza;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GetTipoIstanza::errore::", ex);
                sErr = "alert('Errore nei dati!');";
                return false;
            }
        }
    }
}