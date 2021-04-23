using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using Microsoft.AspNet.Identity;

namespace OPENgovSPORTELLO.Dichiarazioni.TARSU
{
    /// <summary>
    /// La selezione di un articolo + bottone “visualizza dettaglio articolo”.
    /// La funzione di inserimento nuova dichiarazione può essere eseguita da qualsiasi contribuente (nuovo o vecchio).
    /// Le informazioni che saranno richieste saranno in parte controllate nella sostanza, in parte solo formalmente.  
    /// Al completamento dell’inserimento dei dati, il sistema richiede conferma.
    /// La dichiarazione non è più modificabile, eventuali anomalie potranno essere corrette esclusivamente con una nuova dichiarazione di variazione, oppure con una comunicazione via PEC al CUC.
    /// Eseguite tutte le variazioni, il sistema produrrà la dichiarazione(su modello normativo) il cittadino avrà possibilità di invio.Il portale invia la dichiarazione al BO che visualizzerà la presenza di istanze da validare, tramite funzione visualizzerà documento ed eseguirà protocollazione automatica, post protocollazione il sistema invierà mail al cittadino per informare che la sua dichiarazione è stata ricevuta ed ha assunto il numero di protocollo xx
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.TARSU fncMng = new BLL.TARSU();
        private static SPC_DichTARSU UIOrg;
        private static string MailUser;
        private static Istanza myIstanza = new Istanza();
        private static List<System.Web.Mail.MailAttachment> ListIstanzaMailAttachments = new List<System.Web.Mail.MailAttachment>();
        private static List<IstanzaAllegato> ListIstanzaDichAttach = new List<IstanzaAllegato>();
        private string UrlGoToGIS = string.Empty;

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
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.TARSU, MySession.Current.Ente.IDEnte);

                sScript += "$('#divDatiIstanza').hide();";
                sScript += "$('#divOccupanti').hide();";
                sScript += "$('.BottoneCounter').hide();$('.BottoneWork').hide();$('.BottoneStop').hide();$('.BottoneAccept').hide();";
                sScript += "$('#lblSIT').hide();$('.BottoneMap').hide();";
                sScript += "$('p#Protocolla').hide();$('p#InCarico').hide();$('p#Respingi').hide();$('p#Valida').hide();";
                sScript += "$('.lead_header').text('Dati Immobile');";
                sScript += "$('.lead_header').removeClass('col-md-2');";
                sScript += "$('.lead_header').addClass('col-md-5');";
                sScript += "$('.BottoneDiv').hide();";
                if (MySession.Current.Scope == "BO" && MySession.Current.Ente.DatiVerticali.TipoBancaDati == "I")
                    sScript += "$('.BottoneSort').show();$('p#Ribalta').show();";
                else
                    sScript += "$('.BottoneSort').hide();$('p#Ribalta').hide();";
                sScript += new BLL.GestForm().GetHelp("HelpFODichTARSU", MySession.Current.Ente.UrlWiki);
                RegisterScript(sScript, this.GetType());

                hfTypeProtocollo.Value = MySettings.GetConfig("TypeProtocollo");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Page_Init::errore::", ex);
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
                    List<GenericCategory> ListGenSO = new List<GenericCategory>();
                    List<GenericCategory> ListGenCat = new List<GenericCategory>();
                    List<GenericCategory> ListGenRid = new List<GenericCategory>();
                    List<GenericCategory> ListGenEse = new List<GenericCategory>();
                    List<GenericCategory> ListGenMotivazioni = new List<GenericCategory>();
                    SPC_DichTARSU myUIDich = new SPC_DichTARSU();
                    ListGenSO = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_StatoOccupazione, string.Empty, string.Empty);
                    ListGenCat = new BLL.Settings().LoadTARSUCat(MySession.Current.Ente.IDEnte, "D");
                    ListGenRid = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_Riduzioni, string.Empty, string.Empty);
                    ListGenEse = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_Esenzioni, string.Empty, string.Empty);

                    ListGenMotivazioni = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_Motivazioni, string.Empty, string.Empty);

                    fncGen.LoadCombo(ddlStatoOccupazione, ListGenSO, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlRid, ListGenRid, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlEse, ListGenEse, "CODICE", "DESCRIZIONE");

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

                            sScript = string.Empty;
                            sScript += new BLL.Istanze(new Istanza(), string.Empty).GetLinkGIS(new SPC_DichICI(), (MySession.Current.Ente.SIT.IsActive == 1 ? true : false), MySession.Current.Ente.SIT.Url, MySession.Current.Ente.SIT.Token, txtFoglio.Text, txtNumero.Text, txtSub.Text,ref UrlGoToGIS) ;
                            RegisterScript(sScript, this.GetType());
                        }

                        sScript = string.Empty;
                        sScript += "$('#lblForewordUI').html('Per fare dichiarazioni di Inagibilità, Inutilizzabilità e Cessazione, clicca sul bottone corrispondente a destra.<br />Se devi fare una variazione agisci direttamente sul campo da variare.');";
                        RegisterScript(sScript, this.GetType());
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
                    }
                    else
                    {
                        if (myUIDich.ListVani == null)
                        {
                            myUIDich.ListVani = new List<SPC_DichTARSUVani>();
                            myUIDich.ListVani.Add(new SPC_DichTARSUVani() { ScopeCat = "D" });
                        }
                        GrdVani.DataSource = myUIDich.ListVani;
                        GrdVani.DataBind();
                        if (myUIDich.ListOccupanti.Count == 0)
                        {
                            myUIDich.ListOccupanti.Add(new SPC_DichTARSUOccupanti());
                        }
                        GrdOccupanti.DataSource = myUIDich.ListOccupanti;
                        GrdOccupanti.DataBind();
                        ShowHide("divOccupanti", false);

                        if (MySession.Current.IdIstanza > 0)
                            LoadIstanza();
                        UIOrg = new SPC_DichTARSU();
                        ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
                    }
                    if (MySession.Current.Scope == "FO")
                    {
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_UrlAuthGIS').val('"+MySession.Current.Ente.SIT.UrlAuth+"');";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_TokenAuthGIS').val('"+MySession.Current.Ente.SIT.Token+"');";
                        sScript += "autoSubmit();";
                        RegisterScript(sScript, this.GetType());
                    }
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("SearchStradario", false);

                    if (MySession.Current.IdIstanza <= 0)
                    { new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte); }
                    else { new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Page_Load", "ingresso pagina", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte); }
                }
                LoadTotMQ();
                ShowHide("FileToUpload", false);
                ShowHide("divRiepilogo", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                if (MySession.Current.Scope == "FO")
                    RegisterScript("$('#FAQ').addClass('HelpFOTARSU');", this.GetType());
                else
                    RegisterScript("$('#FAQ').addClass('HelpBOTARSU');", this.GetType());
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
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita stadario", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                ShowHide("SearchStradario", false);
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                if (MySession.Current.TipoStorico != string.Empty)
                {
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetStoricoTARSU, Response);
                }
                else {
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoTARSU, Response);
                }
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "GestAllegati", "chiesto inserimento allegati", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                }

                else
                {
                    ShowHide("FileToUpload", false);
                }

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GestAllegati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.SearchStradario::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
            }
        }
        #region "Bottoni Front Office"
        /// <summary>
        /// Bottone per la richiesta di istanza d'inagibilità
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Inagibile(object sender, ImageClickEventArgs e)
        {
            try
            {
                MySession.Current.TipoIstanza = Istanza.TIPO.Inagibilità;
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaInagibile", "chiesto inserimento istanza inagibile", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Inagibile::errore::", ex);
                LoadException(ex);
            }
        }
        protected void Inutilizzo(object sender, ImageClickEventArgs e)
        {
            try
            {
                MySession.Current.TipoIstanza = Istanza.TIPO.Inutilizzabilità;
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Inutilizzo", "chiesto inserimento istanza Inutilizzo", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Inutilizzo::errore::", ex);
                LoadException(ex);
            }
        }
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaClose", "chiesto inserimento istanza chiusura", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Close::errore::", ex);
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
                bool IsRifACatasto = false;
                decimal MQCatasto, MQ;
                MQCatasto = MQ = 0;

                if ((MySession.Current.Ente.SIT.IsActive == 1 ? true : false))
                {
                    try
                    {
                        string requestUriParam = String.Format("?cf_iva={0}&foglio={1}&mappale={2}&subalterno={3}&cod_ente={4}&eff={5}"
                                , string.Empty
                                , txtFoglio.Text
                                , txtNumero.Text
                                , txtSub.Text
                                , MySession.Current.Ente.CodCatastale
                                , ""
                            );
                        string sErr = string.Empty;
                        var ListUICatasto = string.Empty;
                         new BLL.RestService().MakeRequestByRifCat<string>(MySession.Current.Ente.SIT.Url
                            , requestUriParam
                            , result => ListUICatasto = result
                            , error => sErr = error.Message
                            /*, "Basic " + authInfo*/
                            , "Token: " + MySession.Current.Ente.SIT.Token
                            );
                        if (sErr != string.Empty)
                        {
                            Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.consultacatasto::errore::" + sErr);
                        }
                        else
                        {
                            var myJObject = Newtonsoft.Json.Linq.JObject.Parse(ListUICatasto);
                            foreach (object myInt in myJObject["fabbricati"]["unita_immobiliari"])
                            {
                                var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                if (((myDetIntest["subalterno"].ToString() != string.Empty) ? myDetIntest["subalterno"].ToString() : string.Empty) == txtSub.Text)
                                {
                                    IsRifACatasto = true;
                                    MQCatasto += decimal.Parse((myDetIntest["superficie_tarsu"].ToString() != string.Empty ? myDetIntest["superficie_tarsu"].ToString() : "0"));
                                }
                            }
                        }
                    }
                    catch (HttpException ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.consultacatasto::errore::", ex);
                    }

                    foreach (GridViewRow myRow in GrdVani.Rows)
                    {
                        SPC_DichTARSUVani myItem = new SPC_DichTARSUVani();
                        myItem = fncMng.LoadVanoFromGrd(myRow, MySession.Current.IdRifDBOrg,false);
                        MQ += myItem.MQ;
                    }
                    if (chkAllegati.Checked)
                    {
                        HttpFileCollection ListFiles = Request.Files;
                        if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Istanza, ref ListIstanzaMailAttachments, ref ListIstanzaDichAttach))
                        {
                            sScript = "$('#lblErrorFO').text('Errore in lettura allegati!');$('#lblErrorFO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                    }
                    if (!IsRifACatasto)
                    {
                         sScript = "$('#lblDescrConfirm').text('Attenzione! Riferimenti Catastali non presenti. Si vuole proseguire?');";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKCatasto').show();$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKNew').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_KOBack').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_KORest').show();$('#divConfirm').show();";
                    }
                    else
                    {   
                        if (MQ < (MQCatasto * (decimal.Parse((80.00 / 100.00).ToString()))))
                        {
                            sScript += "if (confirm('Attenzione! La superficie catastale risulta essere maggiore (" + MQCatasto.ToString() + " mq). Vuoi proseguire?'))";
                        }
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_CmdSaveIstanza').click();";
                    }
                }
                else
                {
                    if (chkAllegati.Checked)
                    {
                        HttpFileCollection ListFiles = Request.Files;
                        if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Istanza, ref ListIstanzaMailAttachments, ref ListIstanzaDichAttach))
                        {
                            sScript = "$('#lblErrorFO').text('Errore in lettura allegati!');$('#lblErrorFO').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_CmdSaveIstanza').click();";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il salvataggio dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveIstanza(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                SPC_DichTARSU myDich = new SPC_DichTARSU();
                string sMyErr = string.Empty;
                string sScriptDich = string.Empty;
                string NoteVariazione = string.Empty;
                DateTime DataInizioORG = DateTime.MinValue;
                DateTime.TryParse(lblDataInizioORG.InnerText, out DataInizioORG);
                myIstanza = ReadFormIstanza();
                myIstanza.ListAllegati = ListIstanzaDichAttach;
                myDich = ReadFormDich();
                if (!GetTipoIstanza(myDich, ref myIstanza, ref NoteVariazione, ref sMyErr))
                {
                    RegisterScript(sMyErr, this.GetType());
                }
                else {
                    myDich.Note = NoteVariazione;
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

                            if (((SPC_DichTARSU)MySession.Current.UIDichOld) != null)
                            {
                                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).SetIstanzaAnnullaPrec(((SPC_DichTARSU)MySession.Current.UIDichOld).IDIstanza);
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
                                bool retSave = false;
                                if (MySession.Current.TipoIstanza == Istanza.TIPO.Inagibilità)
                                {
                                    retSave = new BLL.BLLSPC_DichTARSU(myDich).SaveInagibile(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich);
                                }
                                else if (MySession.Current.TipoIstanza == Istanza.TIPO.Inutilizzabilità)
                                {
                                    retSave = new BLL.BLLSPC_DichTARSU(myDich).SaveInutilizzato(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich);
                                }
                                else if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                                {
                                    retSave = new BLL.BLLSPC_DichTARSU(myDich).SaveVariazione(DateTime.Now, myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich);
                                }
                                else
                                {
                                    retSave = new BLL.BLLSPC_DichTARSU(myDich).SaveVariazione(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich);
                                }
                                 if (retSave)
                                {
                                    MySession.Current.IsInitDich = true;
                                    RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                                    if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                                    {
                                        List<SPC_DichTARSUVani> listVani = new List<SPC_DichTARSUVani>();
                                        listVani.Add(new SPC_DichTARSUVani() { ID = -1, IDDichTARSU = -1, CodCategoria = "D", IDCatTIA = -1, ScopeCat = "D", MQ = 0, NComponenti = 0, NComponentiPV = 0, IsEsente = 0 });
                                        GrdVani.DataSource = listVani;
                                        GrdVani.DataBind();
                                        foreach (GridViewRow myItem in GrdMotivazioni.Rows)
                                        {
                                            ((CheckBox)myItem.Cells[0].FindControl("chkSel")).Checked = false;
                                        }
                                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza con ulteriore inserimento", General.TRIBUTO.TARSU, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                        MySession.Current.HasNewDich = 1;
                                        sScript = "$('#lblDescrConfirm').text('Si vuole inserire un altro immobile?');";
                                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKCatasto').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKNew').show();$('#" + BLL.GestForm.PlaceHolderName.Body + "_KOBack').show();$('#" + BLL.GestForm.PlaceHolderName.Body + "_KORest').hide();$('#divConfirm').show();";
                                        RegisterScript(sScript, this.GetType());
                                    }
                                    else
                                    {
                                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza", General.TRIBUTO.TARSU, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                        MySession.Current.HasNewDich = 2;
                                        sScript = "$('#lblErrorFO').text('Salvataggio effettuato con successo!');$('#lblErrorFO').show();$(location).attr('href', '" + UrlHelper.GetGestRiepilogoTARSU + "');";
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.SaveIstanza::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Protocolla::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Work::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Accept::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Stop::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.MailBox::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il ribaltamento automatico dell'istanza nel verticale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ribalta(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeRibalta, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.Ribalta::errore::", ex);
                LoadException(ex);
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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "AttachRow", "agganciato strada", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
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
            RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdVaniRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ChangeScopeCat(((SPC_DichTARSUVani)(e.Row.DataItem)).ScopeCat, (DropDownList)e.Row.FindControl("ddlCatND"), ((SPC_DichTARSUVani)e.Row.DataItem).IDCatTIA.ToString(), (TextBox)e.Row.FindControl("txtNC"));

                    DropDownList ddlRow = (DropDownList)e.Row.FindControl("ddlVani");
                    List<GenericCategory> ListMyData = new List<GenericCategory>();
                    ListMyData = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_Vani, string.Empty, string.Empty);
                    new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                    ddlRow.SelectedValue = ((SPC_DichTARSUVani)e.Row.DataItem).IDTipoVano.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdVaniRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdVaniRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                int IDRow = int.Parse(e.CommandArgument.ToString());
                List<SPC_DichTARSUVani> ListMyData = new List<SPC_DichTARSUVani>();

                foreach (GridViewRow myRow in GrdVani.Rows)
                {
                    SPC_DichTARSUVani myItem = new SPC_DichTARSUVani();
                    myItem = fncMng.LoadVanoFromGrd(myRow, MySession.Current.IdRifDBOrg,true);
                    ListMyData.Add(myItem);
                }
                SPC_DichTARSUVani mySetting = new SPC_DichTARSUVani { ID = IDRow };
                switch (e.CommandName)
                {
                    case "DeleteRow":
                        foreach (SPC_DichTARSUVani myItem in ListMyData)
                        {
                            if (myItem.ID == IDRow)
                            {
                                ListMyData.Remove(myItem);
                                break;
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "DeleteRow", "eliminato vano", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        ListMyData.Add(new SPC_DichTARSUVani());
                        ListMyData = ListMyData.OrderBy(o => o.MQ).ThenBy(o => o.ID).ThenBy(o => o.ScopeCat).ThenBy(o => o.CodCategoria).ToList();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "default", "nuovo vano", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        break;
                }
                GrdVani.DataSource = ListMyData;
                GrdVani.DataBind();
                LoadTotMQ();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdVaniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdOccupantiRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    DropDownList ddlRow = (DropDownList)e.Row.FindControl("ddlParentela");
                    List<GenericCategory> ListMyData = new List<GenericCategory>();
                    ListMyData = new BLL.Settings().LoadConfigForDDL(string.Empty, 0, GenericCategory.TIPO.LegameParentela, string.Empty, string.Empty);
                    new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                    ddlRow.SelectedValue = ((SPC_DichTARSUOccupanti)e.Row.DataItem).IDParentela.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdOccupantiRowDataBound::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide("divOccupanti", true);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdOccupantiRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                int IDRow = int.Parse(e.CommandArgument.ToString());
                List<SPC_DichTARSUOccupanti> ListMyData = new List<SPC_DichTARSUOccupanti>();

                foreach (GridViewRow myRow in GrdOccupanti.Rows)
                {
                    SPC_DichTARSUOccupanti myItem = new SPC_DichTARSUOccupanti();
                    myItem = fncMng.LoadOccupantiFromGrd(myRow, MySession.Current.IdRifDBOrg);
                    ListMyData.Add(myItem);
                }
                SPC_DichTARSUOccupanti mySetting = new SPC_DichTARSUOccupanti { ID = IDRow };
                switch (e.CommandName)
                {
                    case "DeleteRow":
                        foreach (SPC_DichTARSUOccupanti myItem in ListMyData)
                        {
                            if (myItem.ID == IDRow)
                            {
                                ListMyData.Remove(myItem);
                                break;
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "DeleteRow", "eliminato Occupante", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        int MaxOccupanti = 0;
                        foreach (GridViewRow myRow in GrdVani.Rows)
                        {
                            SPC_DichTARSUVani myItem = new SPC_DichTARSUVani();
                            myItem = fncMng.LoadVanoFromGrd(myRow, MySession.Current.IdRifDBOrg,true);
                            if (MaxOccupanti < myItem.NComponenti)
                                MaxOccupanti = myItem.NComponenti;
                        }
                        if (ListMyData.Count < MaxOccupanti)
                        {
                            ListMyData.Add(new SPC_DichTARSUOccupanti());
                            ListMyData = ListMyData.OrderBy(o => o.Nominativo).ToList();
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "default", "nuovo Occupante", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        }
                        else
                        {
                            string sMyErr = "$('#lblErrorFO').text('Numero massimo di occupanti raggiunto');$('#lblErrorFO').show();";
                            RegisterScript(sMyErr, this.GetType());
                        }
                        break;
                }
                GrdOccupanti.DataSource = ListMyData;
                GrdOccupanti.DataBind();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdOccupantiRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdAllegatiRowDataBound::errore::", ex);
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
                                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdAllegatiRowCommand::errore::", err);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GrdStatiIstanzaRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
            }
        }
        #endregion
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">SPC_DichTARSU oggetto da caricare</param>
        private void LoadForm(SPC_DichTARSU myItem)
        {
            try
            {
                hfIdVia.Value = myItem.IDVia.ToString();
                txtVia.Text = myItem.Via;
                txtCivico.Text = myItem.Civico;
                txtFoglio.Text = myItem.Foglio;
                txtNumero.Text = myItem.Numero;
                txtSub.Text = myItem.Sub;
                txtDataInizio.Text = new FunctionGrd().FormattaDataGrd(myItem.DataInizio);
                txtDataFine.Text = new FunctionGrd().FormattaDataGrd(myItem.DataFine);
                ddlStatoOccupazione.SelectedValue = myItem.IDStatoOccupazione;
                GrdVani.DataSource = myItem.ListVani;
                GrdVani.DataBind();
                if (myItem.ListOccupanti.Count == 0)
                    myItem.ListOccupanti.Add(new SPC_DichTARSUOccupanti());
                GrdOccupanti.DataSource = myItem.ListOccupanti;
                GrdOccupanti.DataBind();
                foreach (SPC_DichTARSUVani myRow in myItem.ListVani)
                {
                    if (myRow.NComponenti > 0)
                    {
                        ShowHide("divOccupanti", true);
                        break;
                    }
                }
                foreach (SPC_DichTARSURidEse myRidEse in myItem.ListRid)
                {
                    ddlRid.SelectedValue = myRidEse.ID.ToString();
                }
                foreach (SPC_DichTARSURidEse myRidEse in myItem.ListEse)
                {
                    ddlEse.SelectedValue = myRidEse.ID.ToString();
                }
                UIOrg = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.LoadForm::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.LoadIstanza::errore::", ex);
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
                txtDataFine.Enabled = true;
                ddlStatoOccupazione.Enabled = false;
                GrdVani.Enabled = false;
                GrdOccupanti.Enabled = false;
                ddlRid.Enabled = false;
                ddlEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Inagibilità)
            {
                List<GenericCategory> myItem = new List<GenericCategory>();
                myItem = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_StatoOccupazione, Istanza.TIPO.Inutilizzabilità, string.Empty);
                if (myItem.Count > 0)
                    if (myItem[0].Codice != string.Empty)
                        ddlStatoOccupazione.SelectedValue = myItem[0].Codice;
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Text = string.Empty;
                GrdVani.Enabled = false;
                GrdOccupanti.Enabled = false;
                ddlRid.Enabled = false;
                ddlEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Inutilizzabilità)
            {
                List<GenericCategory> myItem = new List<GenericCategory>();
                myItem = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.TARSU_StatoOccupazione, Istanza.TIPO.Inutilizzabilità, string.Empty);
                if (myItem.Count > 0)
                    if (myItem[0].Codice != string.Empty)
                        ddlStatoOccupazione.SelectedValue = ((myItem[0].Codice != string.Empty) ? myItem[0].Codice : "-1");
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Text = string.Empty;
                GrdVani.Enabled = false;
                GrdOccupanti.Enabled = false;
                ddlRid.Enabled = false;
                ddlEse.Enabled = false;
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
                GrdVani.Enabled = false;
                GrdOccupanti.Enabled = false;
                ddlRid.Enabled = false;
                ddlEse.Enabled = false;
                ShowHide("divMotivazione", false);
            }
            else if (MySession.Current.IdIstanza > 0 || UIOrg.Stato == "ML")
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlStatoOccupazione.Enabled = false;
                GrdVani.Enabled = false;
                GrdOccupanti.Enabled = false;
                ddlRid.Enabled = false;
                ddlEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdMotivazioni", false);
                if (UIOrg.Stato == "ML" && MySession.Current.Scope == "FO")
                    RegisterScript("$('#divSuggestFromCatasto').html('<label class=\"text-danger\">La posizione è in lavorazione dal comune. Impossibile modificarla.</label>')", this.GetType());
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
                txtDataFine.Enabled = false;
                GrdVani.Enabled = true;
                GrdOccupanti.Enabled = true;
                ddlRid.Enabled = true;
                ddlEse.Enabled = true;
                ShowHide("divMotivazione", true);
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
            if (MySession.Current.UserLogged.IDDelegato > 0)
            {
                RegisterScript("$('.BottoneDiv').hide();$('.BottoneDivIstanza').hide();$('.BottoneSave').hide();", this.GetType());
            }
        }
        #endregion
        #region "Read Data From Form"
        /// <summary>
        /// 
        /// </summary>
        private void LoadTotMQ()
        {
            try
            {
                decimal MQ, Tassabili;
                MQ = Tassabili = 0;
                foreach (GridViewRow myRow in GrdVani.Rows)
                {
                    SPC_DichTARSUVani myItem = new SPC_DichTARSUVani();
                    myItem = fncMng.LoadVanoFromGrd(myRow, MySession.Current.IdRifDBOrg,true);
                    MQ += myItem.MQ;
                    if (myItem.IsEsente == 0)
                        Tassabili += myItem.MQ;
                }
                string sScript = string.Empty;
                if (MQ > 0)
                {
                    sScript += "document.getElementById('lblTotMQ').innerText='Tot MQ:" + MQ.ToString("#,##0") + "';";
                    sScript += "document.getElementById('lblTotTassabili').innerText='Tot. MQ Tassabili:" + Tassabili.ToString("#,##0") + "';";
                }
                else
                {
                    sScript += "document.getElementById('lblTotMQ').innerText='';";
                    sScript += "document.getElementById('lblTotTassabili').innerText='';";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.LoadTotMQ::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.TARSU, UIOrg.Stato);
            }
        }
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
                myItem.IDTributo = General.TRIBUTO.TARSU;
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

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.ReadFormIstanza::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// Funzione per la lettura dei dati dell'immobile dalla pagina
        /// </summary>
        /// <returns>SPC_DichTARSU oggetto immobile</returns>
        private SPC_DichTARSU ReadFormDich()
        {
            SPC_DichTARSU NewItem = new SPC_DichTARSU();
            try
            {
                NewItem.ID = default(int);
                NewItem.IDIstanza = default(int);
                NewItem.IDRifOrg = MySession.Current.IdRifDBOrg;
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
                NewItem.IDStatoOccupazione = ddlStatoOccupazione.SelectedValue;
                NewItem.StatoOccupazione = ddlStatoOccupazione.SelectedItem.Text;
                NewItem.IDTipoVano = 1;
                NewItem.NVani = 1;
                List<SPC_DichTARSUVani> ListMyData = new List<SPC_DichTARSUVani>();
                List<SPC_DichTARSUOccupanti> ListOccupanti = new List<SPC_DichTARSUOccupanti>();
                bool OccupantiAllowed = false;
                decimal MQ = 0;
                foreach (GridViewRow myRow in GrdVani.Rows)
                {
                    SPC_DichTARSUVani myItem = new SPC_DichTARSUVani();
                    myItem = fncMng.LoadVanoFromGrd(myRow, MySession.Current.IdRifDBOrg,false);
                    if (myItem.NComponenti > 0)
                        OccupantiAllowed = true;
                    if(myItem.MQ>0)
                    ListMyData.Add(myItem);
                    MQ += myItem.MQ;
                }
                NewItem.ListVani = ListMyData;
                NewItem.MQ = MQ;
                if (OccupantiAllowed)
                {
                    foreach (GridViewRow myRow in GrdOccupanti.Rows)
                    {
                        SPC_DichTARSUOccupanti myItem = new SPC_DichTARSUOccupanti();
                        myItem = fncMng.LoadOccupantiFromGrd(myRow, MySession.Current.IdRifDBOrg);
                        if(myItem.Nominativo!=string.Empty)
                        ListOccupanti.Add(myItem);
                    }
                }
                NewItem.ListOccupanti = ListOccupanti;
                NewItem.Note = "";
                if (ddlRid.SelectedValue != string.Empty && ddlRid.SelectedValue != "-1")
                {
                    SPC_DichTARSURidEse myRid = new SPC_DichTARSURidEse();
                    myRid.IDDichTARSU = NewItem.ID;
                    myRid.IDType = SPC_DichTARSURidEse.TYPE.Riduzione;
                    myRid.Codice = ddlRid.SelectedValue;
                    myRid.Descrizione = ddlRid.SelectedItem.Text;
                    NewItem.ListRid.Add(myRid);
                }
                if (ddlEse.SelectedValue != string.Empty && ddlEse.SelectedValue!="-1")
                {
                    SPC_DichTARSURidEse myEse = new SPC_DichTARSURidEse();
                    myEse.IDDichTARSU = NewItem.ID;
                    myEse.IDType = SPC_DichTARSURidEse.TYPE.Esenzione;
                    myEse.Codice = ddlEse.SelectedValue;
                    myEse.Descrizione = ddlRid.SelectedItem.Text;
                    NewItem.ListEse.Add(myEse);
                }
                return NewItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.ReadFormDich::errore::", ex);
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "LoadSearch", "cercato strada", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.LoadSearch::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione che determina il tipo di istanza che si sta creando
        /// </summary>
        /// <param name="UIDichCurrent">SPC_DichTARSU oggetto immobile</param>
        /// <param name="myItem">ref Istanza oggetto istanza che si sta creando</param>
        /// <param name="sErr">ref string errore</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        private bool GetTipoIstanza(SPC_DichTARSU UIDichCurrent, ref Istanza myItem, ref string NoteVariazione, ref string sErr)
        {
            try
            {
                if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                {
                    if ((((SPC_DichTARSU)MySession.Current.UIDichOld).ListVani.Count != UIDichCurrent.ListVani.Count || ((SPC_DichTARSU)MySession.Current.UIDichOld).MQ != UIDichCurrent.MQ)
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).ListRid.Count != UIDichCurrent.ListRid.Count
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).ListEse.Count != UIDichCurrent.ListEse.Count
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).DataInizio.ToShortDateString() != UIDichCurrent.DataInizio.ToShortDateString()
                            || ((((SPC_DichTARSU)MySession.Current.UIDichOld).DataFine.Year==1)?DateTime.MaxValue.ToShortDateString():((SPC_DichTARSU)MySession.Current.UIDichOld).DataFine.ToShortDateString()) != UIDichCurrent.DataFine.ToShortDateString()
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                        if (((SPC_DichTARSU)MySession.Current.UIDichOld).ListVani.Count != UIDichCurrent.ListVani.Count || ((SPC_DichTARSU)MySession.Current.UIDichOld).MQ != UIDichCurrent.MQ)
                            NoteVariazione += " Variazione Vani";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).ListRid.Count != UIDichCurrent.ListRid.Count)
                            NoteVariazione += " Variazione Riduzioni";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).ListEse.Count != UIDichCurrent.ListEse.Count)
                            NoteVariazione += " Variazione Esenzioni";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).DataInizio.ToShortDateString() != UIDichCurrent.DataInizio.ToShortDateString())
                            NoteVariazione += " Variazione Data inizio";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).DataFine.ToShortDateString() != UIDichCurrent.DataFine.ToShortDateString())
                            NoteVariazione += " Variazione Data fine";

                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaAlter", "chiesto inserimento istanza variazione", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                    }
                    else if (((SPC_DichTARSU)MySession.Current.UIDichOld).Via != UIDichCurrent.Via
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).Civico != UIDichCurrent.Civico
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).Foglio != UIDichCurrent.Foglio
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).Numero != UIDichCurrent.Numero
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).Sub != UIDichCurrent.Sub
                            || ((SPC_DichTARSU)MySession.Current.UIDichOld).IDStatoOccupazione != UIDichCurrent.IDStatoOccupazione
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Modifica;
                        if (((SPC_DichTARSU)MySession.Current.UIDichOld).Via != UIDichCurrent.Via)
                            NoteVariazione += " Variazione Via";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).Civico != UIDichCurrent.Civico)
                            NoteVariazione += " Variazione Civico";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).Foglio != UIDichCurrent.Foglio)
                            NoteVariazione += " Variazione Foglio";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).Numero != UIDichCurrent.Numero)
                            NoteVariazione += " Variazione Numero";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).Sub != UIDichCurrent.Sub)
                            NoteVariazione += " Variazione Subalterno";
                        else if (((SPC_DichTARSU)MySession.Current.UIDichOld).IDStatoOccupazione != UIDichCurrent.IDStatoOccupazione)
                            NoteVariazione += " Variazione Stato Occupazione";
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaUpdate", "chiesto inserimento istanza correzione", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                    }
                    else
                    {
                        sErr = "$('#lblErrorFO').text('Non è stata effettuata nessuna modifica! Impossibile salvare i dati!');$('#lblErrorFO').show();";
                        return false;
                    }
                }
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                ListMyData = new BLL.Settings().LoadTipoIstanze(General.TRIBUTO.TARSU, MySession.Current.TipoIstanza, false);
                foreach (GenericCategory myTipo in ListMyData)
                {
                    myItem.IDTipo = myTipo.ID;
                }
                myItem.DescrTipoIstanza = MySession.Current.TipoIstanza;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GetTipoIstanza::errore::", ex);
                sErr = "$('#lblErrorFO').text('Errore nei dati!');$('#lblErrorFO').show();";
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myScopeCat"></param>
        /// <param name="ddlRow"></param>
        /// <param name="IdCat"></param>
        /// <param name="txtNC"></param>
        private void ChangeScopeCat(string myScopeCat, DropDownList ddlRow, string IdCat, TextBox txtNC)
        {
            try
            {
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                if (myScopeCat == "N")
                {
                    ListMyData = new BLL.Settings().LoadTARSUCat(MySession.Current.Ente.IDEnte, "N");
                    ddlRow.Visible = true; txtNC.Visible = false;
                }
                else
                {
                    ListMyData = new BLL.Settings().LoadTARSUCat(MySession.Current.Ente.IDEnte, "D");
                    txtNC.Visible = true; ddlRow.Visible = false;
                }
                new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                if (IdCat != string.Empty)
                    ddlRow.SelectedValue = IdCat;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.CatChanged::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di caricamento dropdownlist
        /// </summary>
        /// <param name="TypeCombo">string dropdown in lavorazione</param>
        /// <returns></returns>
        protected List<GenericCategory> LoadPageCombo(string TypeCombo)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                if (TypeCombo == "UTILIZZO")
                    ListMyData = new BLL.Settings().LoadTARSUScopeCat();
                else if (TypeCombo == "CATTIA")
                    ListMyData = new BLL.Settings().LoadTARSUCat(MySession.Current.Ente.IDEnte, "A");

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.LoadPageCombo::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ControlSelectedChanged(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    DropDownList ddlRowTributo = (DropDownList)sender;
                    GridViewRow myRow = (GridViewRow)ddlRowTributo.NamingContainer;
                    ChangeScopeCat(ddlRowTributo.SelectedValue, (DropDownList)myRow.Cells[1].FindControl("ddlCatND"), string.Empty, (TextBox)myRow.Cells[1].FindControl("txtNC"));
                }
                catch
                {
                    LoadTotMQ();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
        }
        /// <summary>
        /// Bottone per il caricamento del link di catasto in base ai riferimenti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RifCatChanged(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                sScript +=  new BLL.Istanze(new Istanza(), string.Empty).GetLinkGIS(new SPC_DichICI(), (MySession.Current.Ente.SIT.IsActive == 1 ? true : false), MySession.Current.Ente.SIT.Url, MySession.Current.Ente.SIT.Token, txtFoglio.Text, txtNumero.Text, txtSub.Text,ref UrlGoToGIS) ;
                RegisterScript(sScript, this.GetType());
                if (((TextBox)sender).ID == "txtFoglio")
                {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_txtNumero').focus();";
                }
                else if (((TextBox)sender).ID == "txtNumero")
                {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_txtSub').focus();";
                }
                else if (((TextBox)sender).ID == "txtSub")
                {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_ddlCat').focus();";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.RifCatChanged::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
    }
}
