using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using AnagInterface;using Anagrafica.DLL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OPENgovSPORTELLO.Models;
using System.IO;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Nella sezione Anagrafica, il contribuente deve avere la possibilità di variare i propri dati anagrafici (l’indirizzo di residenza, inserire/modificare gli indirizzi di spedizione, i recapiti telefoni e di mail e PEC).
    /// Ogni variazione di indirizzo/recapito dovrà essere accompagnato da una data di variazione, in particolare per gli indirizzi mail/PEC.
    /// Gestione del domicilio elettronico(indirizzo mail del commercialista)
    /// In questa sezione, il contribuente avrà la possibilità di richiedere l’invio degli avvisi di pagamento tramite mail anziché via posta.L’opzione di invio avvisi di pagamento tramite mail deve poi essere gestita dal verticale (opzione prevista dal verticale iSimply).
    /// L’aggiornamento dell’anagrafica sui verticali comunali sarà a cura dell’operatore del back office.
    /// <strong>Gestione commercialisti/CAF</strong>
    /// Nella pagina del profilo si visualizza il pulsante “deleghe”. Cliccando sul pulsante si nascondono tutti i dati del profilo, con relativi pulsanti, e si visualizza pannello con griglia dei contribuenti per i quali ho la delega.
    /// In griglia ho:
    /// Colonna di gestione - Nominativo - Codice fiscale/partita iva - Tributo - Stato
    /// Oltre alla griglia ho i pulsanti di:
    /// <list type="bullet">
    /// <item>Salva: se i controlli formali sono andati a buon fine, aggiorna la situazione deleganti del contribuente, nasconde pannello e ritorna a dati del profilo con relativi pulsanti.</item>
    /// <item>Indietro: nasconde pannello e ritorna a dati del profilo con relativi pulsanti.</item>
    /// </list>	
    /// L’inserimento di una nuova delega (inteso anche come subentro) genera un’istanza che il back-office deve lavorare.
    /// La rimozione di una delega ha subito effetto senza generare istanze.Eventuali istanze non ancora validate, o comunque quelle fatte dal precedente delegato dovranno essere visibili dal delegante.L’eventuale nuovo delegato dovrà vedere le istanze che presenterà, mentre vedrà la totalità delle dichiarazioni.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ProfiloFO : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ProfiloFO));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.Profilo fncMng = new BLL.Profilo();
        private static Istanza myIstanza = new Istanza();
        private static string MailUser;

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
                ShowHide("FileToUpload", false);
                ShowHide("divDatiIstanza", false);
                ShowHide("divMotivazione", false);

                hfFrom.Value = MySession.Current.Scope;
                string sScript = string.Empty;
                if (MySession.Current.Ente == null && hfFrom.Value == "FO")
                {
                    sScript = "alert('Selezionare un’ente prima di poter accedere!');";
                    sScript += "window.location='" + UrlHelper.GetDefaultFO + "'";
                    RegisterScript(sScript, this.GetType());
                }
                else { 
                sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();";
                sScript += "$('.divGrdBtn').hide();";
                sScript += new BLL.GestForm().GetHelp("HelpFOAccount", MySession.Current.Ente.UrlWiki);
                sScript += new BLL.GestForm().GetLabel("Profilo", "");

                sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_GrdAllegati').hide();";

                sScript += "$('#Deleghe').hide();$('#Profilo').hide();";
                sScript += "$('.BottoneDelegate').hide();$('.BottoneKey').hide();$('.BottoneSave').hide();$('.BottoneBack').hide();";
                sScript += "$('.textheader').text('Profilo');";
                sScript += "$('#divDelegaAttach').hide();";

                RegisterScript(sScript, this.GetType());}
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Page_Init::errore::", ex);
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
                CmdSaveSpedizione.Attributes.Add("OnClick", "return VerificaCampiObbligatoriSpedizione();");
                CmdSave.Attributes.Add("OnClick", "return VerificaCampiObbligatori();");
                CmdDelete.Attributes.Add("OnClick", "return Conferma();");
                if (!Page.IsPostBack)
                {
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                    ShowHide("SearchEnti", false);

                    if (MySession.Current.IdIstanza > 0)
                    {
                        LoadIstanza();
                    }
                    else {
                        if (!fncMng.Load(MySession.Current.UserLogged.IDContribToWork, ddlTributo, ddlTipoContatto, MySession.Current.myAnag))
                            RegisterScript("Errore in caricamento pagina", this.GetType());
                        else {
                            LoadAnagrafica(MySession.Current.myAnag,null);
                        }
                        ManageShowHideDelega(false);
                    }
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "Page_Load", "ingresso pagina", "", "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('#FAQ').addClass('HelpFOAccount');", this.GetType());
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        #region "Bottoni"
        /// <summary>
        /// Bottone per il reset della password.
        /// Richiama la stessa funzione disponibile al Login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangePassword(object sender, EventArgs e)
        {
            try
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.FindByName(User.Identity.Name);
                string code = manager.GeneratePasswordResetToken(user.Id);
                string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                Response.Redirect(callbackUrl + "&utente=" + user.Email);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "ChangePassword", "chiesto cambio password", "", "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.ChangePassword::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per il salvataggio dei dati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;

                DettaglioAnagrafica myItem = LoadFromForm();
                bool IsValidCFPIVA = true;
                if (myItem.CodiceFiscale.Length <= 0 && myItem.PartitaIva.Length <= 0)
                    IsValidCFPIVA = false;
                if (myItem.CodiceFiscale.Length > 0)
                    IsValidCFPIVA = new General().ValidateCodiceFiscale(myItem.CodiceFiscale);
                else
                    IsValidCFPIVA = new General().ValidatePartitaIva(myItem.PartitaIva);
                if (IsValidCFPIVA)
                {
                    if (fncMng.Save(myItem, MySession.Current.UserLogged.ID, out sScript) <= 0)
                    {
                        string sMyErr = "$('#lblErrorBO').text(" + sScript.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorBO').show();";
                        RegisterScript(sMyErr, this.GetType());
                    }
                    else
                    {
                        MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(myItem.COD_CONTRIBUENTE, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "Save", "salvataggio istanza", "ANAG", Istanza.TIPO.Anagrafica, MySession.Current.Ente.IDEnte);
                        string IdSoggetto = string.Empty;
                        new BLLImport().ReadDatiVerticale(MySession.Current.Ente.IDEnte, (myItem.PartitaIva != string.Empty ? myItem.PartitaIva : myItem.CodiceFiscale), ref IdSoggetto);
                        new BLLImport().SetDatiVerticaleRead(MySession.Current.Ente.IDEnte, IdSoggetto);
                        Response.Redirect(UrlHelper.GetDefaultFO);
                    }
                }
                else
                {
                    string sMyErr = "$('#lblErrorBO').text('Codice Fiscale non corretto');$('#lblErrorBO').show();";
                    RegisterScript(sMyErr, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            try
            {
                if (((System.Web.UI.WebControls.Button)sender).ID == "CmdBackSearchEnti")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "Back", "uscita ricerca enti", "", "", MySession.Current.Ente.IDEnte);
                    ManageShowHideDelega(false);
                    ShowHide("SearchEnti", false);
                }
                else if (((System.Web.UI.WebControls.Button)sender).ID == "CmdDelegaBack")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "Back", "uscita delega", "", "", MySession.Current.Ente.IDEnte);
                    ManageShowHideDelega(false);
                }
                else if (((System.Web.UI.WebControls.Button)sender).ID == "CmdDelegaAttachBack")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "Back", "uscita delega", "", "", MySession.Current.Ente.IDEnte);
                    string sScript = "$('#divDelegaAttach').hide();";
                    RegisterScript(sScript, this.GetType());
                    ManageShowHideDelega(true);
                }
                else if (((System.Web.UI.WebControls.Button)sender).ID == "CmdBackMngInvio")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "Back", "uscita gestione spedizione", "", "", MySession.Current.Ente.IDEnte);
                    ManageShowHideDelega(false);
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                }
                else if (MySession.Current.IdIstanza > 0)
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "Back", "uscita pagina", string.Empty, "", MySession.Current.Ente.IDEnte);
                    if (MySession.Current.Scope == "BO")
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeGen, Response);
                    else
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFO_IstanzeGen, Response);
                }
                else {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "", "Back", "uscita pagina", "", "", MySession.Current.Ente.IDEnte);
                    if (MySession.Current.Scope == "BO")
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeGen, Response);
                    else
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Back::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Ricerca dei comuni
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchEnti(object sender, EventArgs e)
        {
            LoadSearch();
            ManageShowHideDelega(false);
            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
            ShowHide("divCmdCO", false);
            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
        }
        #region "Codice Fiscale"
        /// <summary>
        /// Bottone per il calcolo del codice fiscale dai dati di nascita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CalcoloCFFromDati(object sender, EventArgs e)
        {
            ControlliCFPI fncCF = new ControlliCFPI();
            string sErr = "";
            try
            {
                if (txtDataNascita.Text != string.Empty)
                    txtCodiceFiscale.Text = fncCF.Calcolo_Codice_Fiscale(txtCognome.Text.ToUpper(), txtNome.Text.ToUpper(), txtDataNascita.Text, ddlSesso.SelectedItem.Value.ToUpper(), (txtLuogoNascita.Text + txtPVNascita.Text).ToUpper(), ref sErr, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::CalcoloCFFromDati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Bottone per il calcolo dei dati di nascita dal codice fiscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CalcoloDatiFromCF(object sender, EventArgs e)
        {
            ControlliCFPI fncCF = new ControlliCFPI();
            string sErr, PVNascita;
            sErr = PVNascita = "";
            try
            {
                txtDataNascita.Text = fncCF.Data_Nascita_da_CodFiscale(txtCodiceFiscale.Text.ToUpper(), false, ref sErr);
                txtLuogoNascita.Text = fncCF.Luogo_Nascita_da_CodFiscale(txtCodiceFiscale.Text.ToUpper(), false, ref sErr, ref sErr, ref PVNascita, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica);
                txtPVNascita.Text = PVNascita;
                ddlSesso.SelectedValue = fncCF.Sesso_da_CodFiscale(txtCodiceFiscale.Text.ToUpper(), false, ref sErr);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::CalcoloDatiFromCF::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        #endregion
        #region "Deleghe"
        /// <summary>
        /// Bottone per la gestione delle deleghe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GestDeleghe(object sender, EventArgs e)
        {
            try
            {
                hfIdIstanzaDelega.Value = "-1";
                List<Delega> myList = new List<Delega>();
                myList = new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).LoadDeleghe(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribLogged, -1);
                if (myList.Count == 0)
                    myList.Add(new Delega() { IDStato = Delega.TipoStato.Nuova, Stato = "Nuova Delega" });
                GrdDeleghe.DataSource = myList;
                GrdDeleghe.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GestDeleghe::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                string sScript = "$('#divDelegaAttach').hide();";
                RegisterScript(sScript, this.GetType());
                ManageShowHideDelega(true);
            }
        }
        /// <summary>
        /// Bottone per il salvataggio delle deleghe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveDelega(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            Istanza myIstanza = new Istanza();
            List<Delega> ListDeleghe = new List<Delega>();
            try
            {
                foreach (GridViewRow myRow in GrdDeleghe.Rows)
                {
                    Delega myItem = new Delega();
                    int Takeover = int.Parse(hfTakeover.Value);
                    myItem = new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).LoadDataFromGrd(myRow, ref Takeover);
                    bool IsValidCFPIVA = true;
                    if (myItem.CodFiscalePIVA.Length <= 0)
                        IsValidCFPIVA = false;
                    if (myItem.CodFiscalePIVA.Length > 11)
                        IsValidCFPIVA = new General().ValidateCodiceFiscale(myItem.CodFiscalePIVA);
                    else
                        IsValidCFPIVA = new General().ValidatePartitaIva(myItem.CodFiscalePIVA);
                    if (IsValidCFPIVA)
                    {
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').show();";
                        RegisterScript(sScript, this.GetType());
                        hfTakeover.Value = Takeover.ToString();
                        if (!new BLL.Deleghe(myItem, MySession.Current.Ente.IDEnte).FieldValidator(out sMyErr))
                        {
                            sMyErr += "$('#lblErrorBO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorBO').show();";
                            sMyErr += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').hide();";
                            RegisterScript(sMyErr, this.GetType());
                            return;
                        }
                        if (myItem.Subentro != "OK")
                        {
                            sScript += "if (confirm('Vuoi fare un subentro di delega per il contribuente " + myItem.Nominativo + " sul tributo " + myItem.DescrTributo + "?'))$('#" + BLL.GestForm.PlaceHolderName.Body + "_hfTakeover').val('1');";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                        ListDeleghe.Add(myItem);
                    }
                    else
                    {
                        sMyErr += "$('#lblErrorBO').text('Il codice fiscale non è formalmente corretto.');$('#lblErrorBO').show();";
                        sMyErr += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').hide();";
                        RegisterScript(sMyErr, this.GetType());
                        return;
                    }
                }
                foreach (Delega myRow in ListDeleghe)
                {
                    if (myRow.IDIstanza <= 0)
                    {
                        myIstanza.DataPresentazione = DateTime.Now;
                        myIstanza.DataInvioDichiarazione = DateTime.Now;
                        myIstanza.DataInCarico = DateTime.MaxValue;
                        myIstanza.DataRespinta = DateTime.MaxValue;
                        myIstanza.DataValidata = DateTime.MaxValue;
                        myIstanza.IDContribuente = MySession.Current.UserLogged.IDContribLogged;
                        myIstanza.IDDelegato = 0;
                        myIstanza.IDEnte = MySession.Current.Ente.IDEnte;
                        myIstanza.IDIstanza = -1;
                        myIstanza.IDStato = Istanza.STATO.Presentata;
                        myIstanza.IDTributo = string.Empty;
                        myIstanza.Note = "";
                        foreach (GenericCategory myTipo in new BLL.Settings().LoadTipoIstanze(string.Empty, Istanza.TIPO.Delega, false))
                        {
                            myIstanza.IDTipo = myTipo.ID;
                        }
                        if (new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Save())
                        {
                            myRow.IDIstanza = myIstanza.IDIstanza;
                            if (!new BLL.Deleghe(myRow, MySession.Current.Ente.IDEnte).Save())
                            {
                                sMyErr += "$('#lblErrorBO').text('Errore in salvataggio!');$('#lblErrorBO').show();";
                                RegisterScript(sMyErr, this.GetType());
                                break;
                            }
                        }
                    }
                }
                GrdDeleghe.DataSource = new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).LoadDeleghe(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribLogged, -1);
                GrdDeleghe.DataBind();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Deleghe", "Save", "salvataggio delega", "", Istanza.TIPO.Delega, MySession.Current.Ente.IDEnte);
                sMyErr += "$('#lblErrorBO').text('Salvataggio effettuato con successo!');$('#lblErrorBO').show();";
                RegisterScript(sMyErr, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.SaveDelega::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(true);
            }
        }
        /// <summary>
        /// Bottone per la stampa della delega
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrintDelega(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                ManageShowHideDelega(true);
                PrintDelega(int.Parse(hfIdIstanzaDelega.Value));
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.PrintDelega::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(true);
            }
        }
        /// <summary>
        /// Bottone per il salvataggio degli allegati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveDelegaAttach(object sender, EventArgs e)
        {
            try
            {
                HttpFileCollection ListFiles = Request.Files;
                List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
                List<IstanzaAllegato> ListDichAttach = new List<IstanzaAllegato>();
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Istanza, ref ListMailAttachments, ref ListDichAttach))
                {
                    string sScript = "$('#lblErrorBO').text('Errore in lettura allegati!');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                foreach (IstanzaAllegato myItem in ListDichAttach)
                {
                    if (myItem.PostedFile != null)
                    {
                        myItem.IDIstanza = int.Parse(hfIdIstanzaDelega.Value);
                        if (!new BLL.IstanzaAllegati(myItem).Save(MySession.Current.Ente.IDEnte))
                        {
                            string sScript = "$('#lblErrorBO').text('Errore in lettura allegati!');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            break;
                        }
                        else
                        {
                            if (myItem.IDAllegato <= 0)
                            {
                                string sScript = "$('#lblErrorBO').text('Errore in lettura allegati!');$('#lblErrorBO').show();";
                                RegisterScript(sScript, this.GetType());
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.SaveDelegaAttach::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                string sScript = "$('#divDelegaAttach').hide();";
                RegisterScript(sScript, this.GetType());
                ManageShowHideDelega(true);
            }
        }
        #endregion
        #region "Spedizione"
        /// <summary>
        /// Bottone per il salvataggio dei dati di spedizione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveSpedizione(object sender, EventArgs e)
        {
            ObjIndirizziSpedizione mySped = new ObjIndirizziSpedizione();
            List<ObjIndirizziSpedizione> ListSpedizione = new List<ObjIndirizziSpedizione>();
            bool bTrovato = false;
            string sScript = string.Empty;
            try
            {
                mySped = LoadSpedFromForm();
                if (mySped.ID_DATA_SPEDIZIONE <= 0)
                {
                    mySped.ID_DATA_SPEDIZIONE = (MySession.Current.IndirizziSpedizione).ToList<ObjIndirizziSpedizione>().Count * -100;
                }
                if (((System.Web.UI.WebControls.Button)sender).ID == "CmdSaveSpedizione")
                {
                    foreach (ObjIndirizziSpedizione oSped in (MySession.Current.IndirizziSpedizione))
                    {
                        if ((oSped.ID_DATA_SPEDIZIONE == mySped.ID_DATA_SPEDIZIONE))
                        {
                            ListSpedizione.Add(mySped);
                            bTrovato = true;
                        }
                        else
                            if (oSped.ID_DATA_SPEDIZIONE != -1 && oSped.ID_DATA_SPEDIZIONE != 0)
                            ListSpedizione.Add(oSped);
                    }
                }
                else {
                    foreach (ListItem myItem in ddlTributo.Items)
                    {
                        if ((mySped.CodTributo != myItem.Value) && (myItem.Value != "-1"))
                        {
                            ObjIndirizziSpedizione oSped = new ObjIndirizziSpedizione();
                            oSped = LoadSpedFromForm();
                            oSped.ID_DATA_SPEDIZIONE = ((ListSpedizione.Count * -100) + 1);
                            oSped.CodTributo = myItem.Value;
                            oSped.DescrTributo = myItem.Text;
                            ListSpedizione.Add(oSped);
                        }
                    }
                }
                if (bTrovato == false)
                {
                    ListSpedizione.Add(mySped);
                }

                ListSpedizione.Add(new ObjIndirizziSpedizione());
                // controllo che non ci sia un doppio tributo
                foreach (ObjIndirizziSpedizione oSped in ListSpedizione)
                {
                    if ((oSped.CodTributo.PadLeft(4, char.Parse("0")) == mySped.CodTributo) && (oSped.ID_DATA_SPEDIZIONE != mySped.ID_DATA_SPEDIZIONE))
                    {
                        SvuotaIndSped();
                        sScript = "$('#lblErrorBO').text('Indirizzo per Tributo già presente!\nImpossibile inserirlo come nuovo!');$('#lblErrorBO').show();";
                        RegisterScript(sScript, this.GetType());
                        return;
                    }
                }

                // ricarico la griglia
                MySession.Current.IndirizziSpedizione = ListSpedizione;
                GrdInvio.DataSource = ListSpedizione;
                GrdInvio.DataBind();
                SvuotaIndSped();
                sScript = "$('#lblErrorBO').text('Si ricorda che le modifiche saranno salvate effettivamente solo al salvataggio generale!');$('#lblErrorBO').show();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.SaveSpedizione::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
            }
        }
        /// <summary>
        /// Bottone per la cancellazione di un indirizzo di spedizione
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteRecapito(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                sScript = "AzzeraIndInvio();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.DeleteRecapito::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        #endregion
        #region "Contatto"
        /// <summary>
        /// Bottone per il salvataggio dei contatti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveContatto(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                GestioneAnagrafica oAnagrafica = new GestioneAnagrafica();

                System.Data.DataSet ds = oAnagrafica.SetContatti(MySession.Current.DataSetContatti, ddlTipoContatto.SelectedIndex, txtDatiRiferimento.Text, txtDataInizioInvio.Text, int.Parse(hdIdContatto.Value), ddlTipoContatto.SelectedItem.Text);
                MySession.Current.DataSetContatti = (dsContatti)ds;
                hdIdContatto.Value = "-1";
                ddlTipoContatto.SelectedIndex = -1;
                txtDatiRiferimento.Text = "";
                chkInvioInformativeViaMail.Checked = false; txtDataInizioInvio.Text = "";
                GrdContatti.DataSource = ds;
                GrdContatti.DataBind();

                sScript = "$('#lblErrorBO').text('Si ricorda che le modifiche saranno salvate effettivamente solo al salvataggio generale!');$('#lblErrorBO').show();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.SaveContatto::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Bottoner per la cancellazione di un contatto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteContatto(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                GestioneAnagrafica oAnagrafica = new GestioneAnagrafica();

                System.Data.DataSet ds = oAnagrafica.DeleteContatti(MySession.Current.DataSetContatti, int.Parse(hdIdContatto.Value));
                MySession.Current.DataSetContatti = (dsContatti)ds;
                hdIdContatto.Value = "-1";
                ddlTipoContatto.SelectedIndex = -1;
                txtDatiRiferimento.Text = "";
                chkInvioInformativeViaMail.Checked = false; txtDataInizioInvio.Text = "";
                GrdContatti.DataSource = ds;
                GrdContatti.DataBind();

                sScript = "$('#lblErrorBO').text('Si ricorda che le modifiche saranno salvate effettivamente solo al salvataggio generale!');$('#lblErrorBO').show();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.DeleteContatto::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Bottone per la pulizia dei dati di contatto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void EraseContatto(object sender, EventArgs e)
        {
            try
            {
                hdIdContatto.Value = "-1";
                ddlTipoContatto.SelectedIndex = -1;
                txtDatiRiferimento.Text = "";
                chkInvioInformativeViaMail.Checked = false; txtDataInizioInvio.Text = "";
                            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.EraseContatto::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        #endregion
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
                    sScript = "$('#lblErrorBO').text('Errore in Protocollazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('Protocollazione effettuata con successo!');$('#lblErrorBO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Protocolla::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
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
                    sScript = "$('#lblErrorBO').text('Errore in Presa in carico!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('Presa in carico effettuata con successo!');$('#lblErrorBO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Work::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
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
                    sScript = "$('#lblErrorBO').text('Errore in Validazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('Validazione effettuata con successo!');$('#lblErrorBO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Accept::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
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
                    sScript = "$('#lblErrorBO').text('Errore in rifiuto istanza!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('Istanza Respinta con successo!');$('#lblErrorBO').show();";

                    if (MySession.Current.Scope == "BO")
                        sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    else
                        sScript += "$(location).attr('href', '" + UrlHelper.GetDefaultFO + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Stop::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
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
                    sScript = "$('#lblErrorBO').text('Errore in invio comunicazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('Comunicazione inviata con successo!');$('#lblErrorBO').show();";
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
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.MailBox::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
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
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GestAllegati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                LoadIstanza();
            }
        }
        #endregion
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdContattiDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                Label lblSID, lblDescrizione, lblIDRIFERIMENTO, lblDataInizioInvio;
                string strArgumentsID, strArgumentsDESC, strArgumentsIDRIFERIMENTO, strArgumentsDataInizioInvio;

                switch (e.Item.ItemType)
                {
                    case ListItemType.Item:
                    case ListItemType.AlternatingItem:
                        lblSID = ((Label)(e.Item.FindControl("lblTipoRiferimento")));
                        lblIDRIFERIMENTO = ((Label)(e.Item.FindControl("lblIDRIFERIMENTO")));
                        lblDescrizione = ((Label)(e.Item.FindControl("DatiRiferimento")));
                        lblDataInizioInvio = ((Label)(e.Item.FindControl("DataInizioInvio")));
                        strArgumentsID = ("\'"
                                    + (lblSID.Text + "\'"));
                        strArgumentsDESC = ("\'"
                                    + (lblDescrizione.Text + "\'"));
                        strArgumentsIDRIFERIMENTO = ("\'"
                                    + (lblIDRIFERIMENTO.Text + "\'"));
                        strArgumentsDataInizioInvio = ("\'"
                                    + (lblDataInizioInvio.Text.Replace("Data validità invio: ", "") + "\'"));
                        e.Item.Attributes.Add("OnClick", "ModificaContatti(" + strArgumentsID + "," + strArgumentsDESC + "," + strArgumentsIDRIFERIMENTO + "," + strArgumentsDataInizioInvio + ");");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GrdContattiDataBound.errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdComuniRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                TextBox txtComune, txtCAP, txtPV, txtCatastale, txtIdEnte;
                txtComune = txtCAP = txtPV = txtCatastale = txtIdEnte = new TextBox();

                if (hdTypeComune.Value == "NAS")
                {
                    txtComune = txtLuogoNascita;
                    txtPV = txtPVNascita;
                    txtCatastale = txtCodComuneNascita;
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                }
                else if (hdTypeComune.Value == "RES")
                {
                    txtComune = txtComuneRes;
                    txtCAP = txtCAPRes;
                    txtPV = txtProvinciaRes;
                    txtCatastale = txtCodComuneResidenza;
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                }
                else if (hdTypeComune.Value == "SPE")
                {
                    txtComune = txtComuneCO;
                    txtCAP = txtCAPCO;
                    txtPV = txtProvinciaCO;
                    txtCatastale = txtCodComuneSpedizione;
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
                }

                string IDSetting = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "AttachRow":
                        foreach (GridViewRow myEnte in GrdComuni.Rows)
                        {
                            if (myEnte.Cells[0].Text == IDSetting.ToString())
                            {
                                txtIdEnte.Text = myEnte.Cells[0].Text;
                                txtComune.Text = myEnte.Cells[1].Text;
                                txtCAP.Text = myEnte.Cells[2].Text;
                                txtPV.Text = myEnte.Cells[3].Text;
                                txtCatastale.Text = myEnte.Cells[4].Text;
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Handler.Comuni.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Funzione di gestione cambio pagina della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdComuniPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
            ManageShowHideDelega(false);
            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
            ShowHide("divCmdCO", false);
            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdInvioRowDataBound(object sender, GridViewRowEventArgs e)
        {
            DropDownList ddlMyDati = new DropDownList();
            ImageButton MyBtn;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if ((((ObjIndirizziSpedizione)(e.Row.DataItem)).ID_DATA_SPEDIZIONE == -1))
                    {
                        MyBtn = ((ImageButton)(e.Row.FindControl("imgOpen")));
                        MyBtn.CssClass = "SubmitBtn Bottone BottoneNewGrd";
                        e.Row.Cells[5].Enabled = false;
                        e.Row.Cells[4].Visible = false; e.Row.Cells[5].Visible = false;
                    }
                    else {
                        MyBtn = ((ImageButton)(e.Row.FindControl("imgOpen")));
                        MyBtn.CssClass = "SubmitBtn Bottone BottoneOpenGrd";
                        e.Row.Cells[5].Enabled = true;
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_CmdNewSped", false);
                    }
                }
                GrdInvio.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::GrdInvioRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdInvioRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow = int.Parse(e.CommandArgument.ToString());
                switch (e.CommandName)
                {
                    case "OpenRow":
                        if (MySession.Current.IndirizziSpedizione != null)
                        {
                            foreach (ObjIndirizziSpedizione mySped in ((List<ObjIndirizziSpedizione>)MySession.Current.IndirizziSpedizione))
                            {
                                if ((mySped.ID_DATA_SPEDIZIONE == IDRow))
                                {
                                    LoadSpedizione(mySped);
                                    break;
                                }
                            }
                        }
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_CmdNewSped", false);
                        break;
                    case "DeleteRow":
                        List<ObjIndirizziSpedizione> ListSpedizione = new List<ObjIndirizziSpedizione>();
                        foreach (ObjIndirizziSpedizione mySped in ((List<ObjIndirizziSpedizione>)MySession.Current.IndirizziSpedizione))
                        {
                            if ((mySped.ID_DATA_SPEDIZIONE != IDRow))
                            {
                                ListSpedizione.Add(mySped);
                            }
                        }
                        SvuotaIndSped();
                        MySession.Current.IndirizziSpedizione = ListSpedizione;
                        GrdInvio.DataSource = ListSpedizione;
                        GrdInvio.DataBind();
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_CmdNewSped", true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Handler.Comuni.GrdInvioRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
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
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GrdAllegatiRowDataBound::errore::", ex);
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
                                    Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GrdAllegatiRowCommand::errore::", err);
                                }
                            }
                        }
                        else
                        {
                            sScript = "$('#lblErrorBO').text('Allegato non disponibile!');$('#lblErrorBO').show();";
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
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GrdStatiIstanzaRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(false);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDelegheRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtnDeleghe($(this).attr('id'));");
                    if ((((Delega)(e.Row.DataItem)).IDStato == 2))
                    {
                        e.Row.Cells[1].Enabled = false;
                        e.Row.Cells[2].Enabled = false;
                        e.Row.Cells[3].Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.Profilo.GrdDelegheRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDelegheRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow = int.Parse(e.CommandArgument.ToString());
                List<Delega> ListGen = new List<Delega>();
                string sScript = string.Empty;

                foreach (GridViewRow myRow in GrdDeleghe.Rows)
                {
                    Delega myItem = new Delega();
                    int Takeover = int.Parse(hfTakeover.Value);
                    myItem = new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).LoadDataFromGrd(myRow, ref Takeover);
                    hfTakeover.Value = Takeover.ToString();
                    ListGen.Add(myItem);
                }
                switch (e.CommandName)
                {
                    case "RowPrint":
                        foreach (Delega myItem in ListGen)
                        {
                            if (myItem.ID == IDRow)
                            {
                                hfIdIstanzaDelega.Value = myItem.IDIstanza.ToString();
                                sScript = "$('.divGrdBtn').hide();";
                                RegisterScript(sScript, this.GetType());
                                ManageShowHideDelega(true);
                                PrintDelega(myItem.IDIstanza);
                                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "RowPrint", "stampa delega", myItem.IdTributo, string.Empty, MySession.Current.Ente.IDEnte);
                                break;
                            }
                        }
                        break;
                    case "RowAttach":
                        foreach (Delega myItem in ListGen)
                        {
                            if (myItem.ID == IDRow)
                            {
                                hfIdIstanzaDelega.Value = myItem.IDIstanza.ToString();
                                GrdDelegaAttach.DataSource = new BLL.IstanzaAllegati(new IstanzaAllegato()).LoadAllegati(myItem.IDIstanza, -1);
                                GrdDelegaAttach.DataBind();
                                sScript = "$('#divDelegaAttach').show();";
                                RegisterScript(sScript, this.GetType());
                                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "RowAttach", "consulta allegati delega", myItem.IdTributo, string.Empty, MySession.Current.Ente.IDEnte);
                                break;
                            }
                        }
                        break;
                    case "RowDelete":
                        foreach (Delega myItem in ListGen)
                        {
                            if (myItem.ID == IDRow)
                            {
                                hfIdIstanzaDelega.Value = myItem.IDIstanza.ToString();
                                myItem.IDOrigineCessazione = Delega.OrigineCessazione.Delegato;
                                if (new BLL.Deleghe(myItem, MySession.Current.Ente.IDEnte).Delete())
                                {
                                    ListGen = new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).LoadDeleghe(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribLogged, -1);
                                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "RowDelete", "eliminato delega", myItem.IdTributo, string.Empty, MySession.Current.Ente.IDEnte);
                                }
                                else
                                {
                                    sScript = "$('#lblErrorBO').text('Impossibile eliminare la delega');$('#lblErrorBO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    return;
                                }
                                break;
                            }
                        }
                        break;
                    default:
                        ListGen.Add(new Delega() { IDStato = Delega.TipoStato.Nuova, Stato = "Nuova Delega" });
                        ListGen = ListGen.OrderBy(o => o.ID).ThenBy(o => o.Nominativo).ToList();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "RowNew", "nuova delega", "", "", MySession.Current.Ente.IDEnte);
                        break;
                }
                GrdDeleghe.DataSource = ListGen;
                GrdDeleghe.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GrdDelegheRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                string sScript = "$('.divGrdBtn').hide();";
                RegisterScript(sScript, this.GetType());
                ManageShowHideDelega(true);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDelegaAttachRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Account.Profilo.GrdDelegaAttachRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDelegaAttachRowCommand(object sender, GridViewCommandEventArgs e)
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
                                    Log.Debug("OPENgovSPORTELLO.Account.Profilo.GrdDelegaAttachRowCommand::errore::", err);
                                }
                            }
                        }
                        else
                        {
                            sScript = "$('#lblErrorBO').text('Allegato non disponibile!');$('#lblErrorBO').show();";
                        }
                        break;
                    default:
                        break;
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.Profilo.GrdDelegaAttachRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHideDelega(true);
            }
        }
        #endregion
        /// <summary>
        /// Funzione di caricamento dati nella pagina
        /// </summary>
        /// <param name="myItem">DettaglioAnagrafica oggetto anagrafica</param>
        /// <param name="myContatti">ContattoAnag oggetto contatti</param>
        private void LoadAnagrafica(DettaglioAnagrafica myItem, ContattoAnag myContatti)
        {
            try
            {
                if (!(myItem == null))
                {
                    hdIdContribuente.Value = myItem.COD_CONTRIBUENTE.ToString();
                    hdIdAnagrafica.Value = myItem.ID_DATA_ANAGRAFICA.ToString();
                    hdCodRappresentanteLegale.Value = myItem.CodContribuenteRappLegale;
                    txtCodiceFiscale.Text = myItem.CodiceFiscale;
                    if (myItem.PartitaIva != "00000000000")
                        txtPartitaIva.Text = myItem.PartitaIva;
                    txtCognome.Text = myItem.Cognome;
                    txtNome.Text = myItem.Nome;
                    ddlSesso.SelectedValue = myItem.Sesso;
                    txtLuogoNascita.Text = myItem.ComuneNascita;
                    txtPVNascita.Text = myItem.ProvinciaNascita;
                    txtDataNascita.Text = myItem.DataNascita;
                    txtCodComuneNascita.Text = myItem.CodiceComuneNascita;
                    txtCodComuneResidenza.Text = myItem.CodiceComuneResidenza;
                    hdCodViaResidenza.Value = myItem.CodViaResidenza;
                    txtComuneRes.Text = myItem.ComuneResidenza;
                    txtCAPRes.Text = myItem.CapResidenza;
                    txtProvinciaRes.Text = myItem.ProvinciaResidenza;
                    txtViaRes.Text = myItem.ViaResidenza;
                    txtCivicoRes.Text = myItem.CivicoResidenza;
                    txtEsponenteCivicoRes.Text = myItem.EsponenteCivicoResidenza;
                    txtScalaRes.Text = myItem.ScalaCivicoResidenza;
                    txtInternoRes.Text = myItem.InternoCivicoResidenza;
                    txtFrazioneRes.Text = myItem.FrazioneResidenza;
                    if (myItem.ListSpedizioni.Count == 0)
                        myItem.ListSpedizioni.Add(new ObjIndirizziSpedizione());
                    foreach (ObjIndirizziSpedizione myCO in myItem.ListSpedizioni)
                    {
                        LoadSpedizione(myCO);
                    }
                    GrdInvio.DataSource = myItem.ListSpedizioni;
                    GrdInvio.DataBind();
                    if (myContatti != null)
                    {
                        GrdContatti.DataSource =new List<ContattoAnag> { myContatti };
                    }
                    else
                        GrdContatti.DataSource = myItem.dsContatti;
                    GrdContatti.DataBind();
                    MySession.Current.DataSetContatti = (Anagrafica.DLL.dsContatti)myItem.dsContatti;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::LoadToForm::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di caricamento dato spedizione nella pagina
        /// </summary>
        /// <param name="myItem">ObjIndirizziSpedizione oggetto spedizione</param>
        private void LoadSpedizione(ObjIndirizziSpedizione myItem)
        {
            try
            {
                if (!(myItem == null))
                {
                    hdIdCO.Value = myItem.ID_DATA_SPEDIZIONE.ToString();
                    if ((myItem.CodTributo != ""))
                    {
                        ddlTributo.SelectedValue = myItem.CodTributo.PadLeft(4, char.Parse("0"));
                    }
                    else
                    {
                        ddlTributo.SelectedValue = General.TRIBUTO.ICI;
                    }
                    txtCognomeCO.Text = myItem.CognomeInvio;
                    txtNomeCO.Text = myItem.NomeInvio;
                    txtComuneCO.Text = myItem.ComuneRCP;
                    txtCAPCO.Text = myItem.CapRCP;
                    txtProvinciaCO.Text = myItem.ProvinciaRCP;
                    txtIndirizzoCO.Text = myItem.ViaRCP;
                    txtNumeroCivicoCO.Text = myItem.CivicoRCP;
                    txtEsponenteCO.Text = myItem.EsponenteCivicoRCP;
                    txtScalaCO.Text = myItem.ScalaCivicoRCP;
                    txtInternoCO.Text = myItem.InternoCivicoRCP;
                    txtFrazioneCO.Text = myItem.FrazioneRCP;
                    txtCodComuneSpedizione.Text = myItem.CodComuneRCP;
                    hdCodViaSpedizione.Value = myItem.CodViaRCP;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::LoadSpedizione::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        /// <summary>
        /// Funzione di caricamento dati dalla pagina all'oggetto specifico
        /// </summary>
        /// <returns>DettaglioAnagrafica oggetto anagrafica</returns>
        private DettaglioAnagrafica LoadFromForm()
        {
            DettaglioAnagrafica myItem = new DettaglioAnagrafica();
            try
            {
                myItem.COD_CONTRIBUENTE = int.Parse(hdIdContribuente.Value);
                myItem.ID_DATA_ANAGRAFICA = int.Parse(hdIdAnagrafica.Value);
                myItem.CodiceFiscale = txtCodiceFiscale.Text;
                myItem.PartitaIva = txtPartitaIva.Text;
                myItem.Cognome = txtCognome.Text;
                myItem.Nome = txtNome.Text;
                if (ddlSesso.SelectedItem.Value == "-1")
                {
                    myItem.Sesso = "";
                }
                else {
                    myItem.Sesso = ddlSesso.SelectedItem.Value;
                }

                myItem.ComuneNascita = txtLuogoNascita.Text;
                myItem.CodiceComuneNascita = txtCodComuneNascita.Text;
                myItem.ProvinciaNascita = txtPVNascita.Text;
                myItem.DataNascita = txtDataNascita.Text;
                myItem.CodContribuenteRappLegale = hdCodRappresentanteLegale.Value;
                myItem.ComuneResidenza = txtComuneRes.Text;
                myItem.CodiceComuneResidenza = txtCodComuneResidenza.Text;
                myItem.CapResidenza = txtCAPRes.Text;
                myItem.ProvinciaResidenza = txtProvinciaRes.Text;
                myItem.ViaResidenza = txtViaRes.Text;
                myItem.Operatore = User.Identity.GetUserName();
                myItem.CodViaResidenza = hdCodViaResidenza.Value;
                myItem.CivicoResidenza = txtCivicoRes.Text;
                myItem.EsponenteCivicoResidenza = txtEsponenteCivicoRes.Text;
                myItem.ScalaCivicoResidenza = txtScalaRes.Text;
                myItem.InternoCivicoResidenza = txtInternoRes.Text;
                myItem.FrazioneResidenza = txtFrazioneRes.Text;
                myItem.CodEnte = MySession.Current.Ente.IDEnte;
                myItem.Concurrency = DateTime.Now;
                List<ObjIndirizziSpedizione> ListCO = new List<ObjIndirizziSpedizione>();
                ObjIndirizziSpedizione myCO = new ObjIndirizziSpedizione();
                myCO = LoadSpedFromForm();
                ListCO.Add(myCO);
                myItem.ListSpedizioni = ListCO;
                myItem.dsContatti = (dsContatti)MySession.Current.DataSetContatti;
                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::LoadFromForm::errore::", ex);
                return myItem;
            }
        }
        /// <summary>
        /// Funzione di caricamento dati in griglia
        /// </summary>
        /// <param name="page">int numero di pagina su cui posizionare la visualizzazione</param>
        private void LoadSearch(int? page = 0)
        {
            BLL.Settings fncGen = new BLL.Settings();
            try
            {
                GrdComuni.DataSource = fncGen.LoadComuni(txtEnteSearch.Text);
                if (page.HasValue)
                    GrdComuni.PageIndex = page.Value;
                GrdComuni.DataBind();
                ShowHide("SearchEnti", true);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.LoadSearch::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdInvio", false);
                ShowHide("divCmdCO", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", true);
            }
        }
        #region "Spedizione"
        /// <summary>
        /// Funzione di caricamento dato spedizione dalla pagina
        /// </summary>
        /// <returns>ObjIndirizziSpedizione oggetto spedizione</returns>
        private ObjIndirizziSpedizione LoadSpedFromForm()
        {
            ObjIndirizziSpedizione mySped = new ObjIndirizziSpedizione();
            try
            {
                mySped.ID_DATA_SPEDIZIONE = int.Parse(hdIdCO.Value);
                mySped.CodTributo = ddlTributo.SelectedValue;
                if (mySped.CodTributo == string.Empty && txtCognomeCO.Text.Trim() != string.Empty)
                    mySped.CodTributo = General.TRIBUTO.ICI;
                mySped.DescrTributo = ddlTributo.SelectedItem.Text;
                mySped.CognomeInvio = txtCognomeCO.Text;
                mySped.NomeInvio = txtNomeCO.Text;
                mySped.CodComuneRCP = txtCodComuneSpedizione.Text;
                mySped.ComuneRCP = txtComuneCO.Text;
                mySped.CapRCP = txtCAPCO.Text;
                mySped.ProvinciaRCP = txtProvinciaCO.Text;
                mySped.CodViaRCP = hdCodViaSpedizione.Value;
                mySped.ViaRCP = txtIndirizzoCO.Text;
                mySped.PosizioneCivicoRCP = "";
                mySped.CivicoRCP = txtNumeroCivicoCO.Text;
                mySped.EsponenteCivicoRCP = txtEsponenteCO.Text;
                mySped.ScalaCivicoRCP = txtScalaCO.Text;
                mySped.InternoCivicoRCP = txtInternoCO.Text;
                mySped.FrazioneRCP = txtFrazioneCO.Text;
                mySped.OperatoreSpedizione = User.Identity.GetUserName();
                return mySped;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.LoadSpedFromForm::errore::", ex);
                throw new Exception("LoadSpedFromForm::errore::", ex);
            }
        }
        /// <summary>
        /// Funzione di pulizia controlli spedizione
        /// </summary>
        private void SvuotaIndSped()
        {
            try
            {
                hdIdCO.Value = "-1";
                ddlTributo.SelectedValue = General.TRIBUTO.ICI;
                txtCognomeCO.Text = string.Empty;
                txtNomeCO.Text = string.Empty;
                txtComuneCO.Text = string.Empty;
                txtCAPCO.Text = string.Empty;
                txtProvinciaCO.Text = string.Empty;
                txtIndirizzoCO.Text = string.Empty;
                txtNumeroCivicoCO.Text = string.Empty;
                txtEsponenteCO.Text = string.Empty;
                txtScalaCO.Text = string.Empty;
                txtInternoCO.Text = string.Empty;
                txtFrazioneCO.Text = string.Empty;
                txtCodComuneSpedizione.Text = string.Empty;
                hdCodViaSpedizione.Value = "-1";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.SvuotaIndSped::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// Funzione di caricamento dati istanza
        /// </summary>
        private void LoadIstanza()
        {
            try
            {
                List<Istanza> listDati = new List<Istanza>();
                DettaglioAnagrafica myAnagrafica = new DettaglioAnagrafica();
                string sScript = string.Empty;

                if (MySession.Current.IdIstanza > 0)
                {
                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze("", 0, DateTime.MaxValue, "", "", "", "", MySession.Current.IdIstanza, -1, true, out listDati))
                        RegisterScript("$('#lblErrorBO').text('Errore in caricamento pagina.');$('#lblErrorBO').show();", this.GetType());
                    else {
                        foreach (Istanza myItem in listDati)
                        {
                            if (myItem.IDTributo == "DELE")
                            {
                                myIstanza = myItem;
                                myIstanza.DescrTipoIstanza = myIstanza.DescrTipo;
                                if (myIstanza.DescrTipo == Istanza.TIPO.Delega)
                                {
                                    List<Delega> myList = new List<Delega>();
                                    myList = new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).LoadDeleghe(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribLogged, MySession.Current.IdIstanza);
                                    GrdDeleghe.DataSource = myList;
                                    GrdDeleghe.DataBind();

                                    MailUser =new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).GetMailUser(MySession.Current.IdIstanza);

                                    sScript = "$('#divDelegaAttach').hide();$('#lblInstructionsDeleghe').hide();";
                                    RegisterScript(sScript, this.GetType());
                                }
                                else {
                                    ContattoAnag myContatti = new ContattoAnag();
                                    myAnagrafica = new BLL.Profilo().LoadAnagrafica(MySession.Current.IdIstanza, ref myContatti);
                                    List<UserRole> ListGen = new BLL.Settings().LoadUserRole("", myAnagrafica.CodiceFiscale, true, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser);
                                    if (ListGen.Count > 0)
                                    {
                                        MailUser = ListGen[0].NameUser;
                                    }
                                    else
                                    {
                                        sScript += "$('#lblErrorBO').text('Errore in caricamento pagina.');$('#lblErrorBO').show();";
                                        RegisterScript(sScript, this.GetType());
                                        break;
                                    }
                                    LoadAnagrafica(myAnagrafica, myContatti);
                                }
                                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).LoadFormData(out sScript, ref GrdStatiIstanza, ref GrdAllegati);
                                if (MySession.Current.Scope == "BO")
                                {
                                    sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();$('#lblMotivazione').hide();";
                                }
                                else
                                {
                                    sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();$('#lblMotivazione').show();";
                                    sScript += "$('.BottoneAccept').hide();$('p#Valida').hide();";
                                    sScript += "$('.BottoneStop').hide();$('p#Respingi').hide();";
                                }
                                sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_GrdAllegati').show();";
                                RegisterScript(sScript, this.GetType());
                                ShowHide("divDatiIstanza", true);
                                ShowHide("divMotivazione", true);
                                ShowHide("lblDescrTributo", false);
                                sScript = "$('#divProfilo :input').attr('disabled', true);";
                                sScript += "$('.BottoneMapMarker').hide();";
                                sScript += "$('.BottoneKey').hide();$('.BottoneSave').hide();$('.BottoneDelete').hide();";
                                sScript += "$('.BottoneDelegate').hide();$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaBack').hide();";
                                sScript += "$('.BottoneTesseraSanitaria').hide();$('.BottoneRecycleCF').hide();";
                                sScript += "$('p#Elimina').hide();$('p#Salva').hide();";
                                sScript += "$('#lblHeadMotivazione').text('Comunicazioni');";
                                RegisterScript(sScript, this.GetType());
                                ShowHide("BottoniIstanza", true);
                                if (myIstanza.DescrTipo == Istanza.TIPO.Delega)
                                {
                                    ManageShowHideDelega(true);
                                }
                                else
                                {
                                    ManageShowHideDelega(false);
                                }
                            }
                            else
                            {
                                myIstanza = myItem;
                                myIstanza.DescrTipoIstanza = myIstanza.DescrTipo;
                                ContattoAnag myContatti = new ContattoAnag();
                                myAnagrafica = new BLL.Profilo().LoadAnagrafica(MySession.Current.IdIstanza, ref myContatti);                                
                                List<UserRole> ListGen = new BLL.Settings().LoadUserRole("", myAnagrafica.CodiceFiscale, true, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser);
                                if (ListGen.Count > 0)
                                {
                                    MailUser = ListGen[0].NameUser;
                                }
                                else
                                {
                                    sScript += "$('#lblErrorBO').text('Errore in caricamento pagina.');$('#lblErrorBO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    break;
                                }
                                if (!fncMng.Load(MySession.Current.UserLogged.IDContribToWork, ddlTributo, ddlTipoContatto, myAnagrafica))
                                    RegisterScript("Errore in caricamento pagina", this.GetType());
                                else {
                                    LoadAnagrafica(myAnagrafica,myContatti);
                                }
                                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).LoadFormData(out sScript, ref GrdStatiIstanza, ref GrdAllegati);
                                if (MySession.Current.Scope == "BO")
                                {
                                    sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();$('#lblMotivazione').hide();";
                                }
                                else
                                {
                                    sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();$('#lblMotivazione').show();";
                                    sScript += "$('.BottoneAccept').hide();$('p#Valida').hide();";
                                    sScript += "$('.BottoneStop').hide();$('p#Respingi').hide();";
                                }
                                sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_GrdAllegati').show();";
                                RegisterScript(sScript, this.GetType());
                                ShowHide("divDatiIstanza", true);
                                ShowHide("divMotivazione", true);
                                sScript = "$('#divProfilo :input').attr('disabled', true);";
                                sScript += "$('.BottoneMapMarker').hide();";
                                sScript += "$('.BottoneKey').hide();$('.BottoneSave').hide();$('.BottoneDelete').hide();";
                                sScript += "$('.BottoneDelegate').hide();$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaBack').hide();";
                                sScript += "$('.BottoneTesseraSanitaria').hide();$('.BottoneRecycleCF').hide();";
                                sScript += "$('p#Elimina').hide();$('p#Salva').hide();";
                                sScript += "$('#lblHeadMotivazione').text('Comunicazioni');";
                                RegisterScript(sScript, this.GetType());
                                ShowHide("BottoniIstanza", true);
                                ManageShowHideDelega(false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.LoadIstanza::errore::", ex);
                LoadException(ex);
            }
        }
        #region "Deleghe"
        /// <summary>
        /// Funzione per la gestione degli oggetti da vedere/nascondere in base all'operazione
        /// </summary>
        /// <param name="IsDelega">bool indica se si è su una delega</param>
        protected void ManageShowHideDelega(bool IsDelega)
        {
            string sScript = string.Empty;
            try
            {
                if (IsDelega)
                {
                    if (MySession.Current.IdIstanza > 0)
                    {
                        sScript += "$('#Deleghe').show(25);$('#Profilo').show();";
                        sScript += "$('.BottoneBack').show();$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaBack').hide();";
                        sScript += "$('.BottoneDelegate').hide();$('.BottoneKey').hide();$('.BottoneSave').hide();";
                        sScript += "$('.textheader').text('Deleghe');";
                        sScript += "$('.BottoneNewGrd').hide();";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_GrdAllegati').show();";
                        GrdDeleghe.Enabled = false;
                        ShowHide("divDatiIstanza", true);
                        ShowHide("divProfilo", false);
                        ShowHide("divMotivazione", true);
                    }
                    else
                    {
                        sScript += "$('#Deleghe').show(25);$('#Profilo').hide();";
                        sScript += "$('.BottoneDelegate').hide();$('.BottoneKey').hide();$('.BottoneSave').hide();$('.BottoneBack').hide();";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').show();$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaBack').show();";
                        sScript += "$('.textheader').text('Deleghe');";
                        sScript += "if ($('#divDelegaAttach').is(':visible')){";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_CmdSaveDelegaAttach').show();$('#" + BLL.GestForm.PlaceHolderName.Body + "_CmdDelegaAttachBack').show();}";
                        sScript += "if ($('#lblErrorBO').is(':visible')) {$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').hide();}else {$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').show();}";
                    }
                }
                else {
                    if (MySession.Current.IdIstanza > 0)
                    {
                        sScript += "$('#Deleghe').hide();$('#Profilo').show(25);";
                        sScript += "$('.BottoneBack').show();$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaBack').hide();";
                        sScript += "$('.textheader').text('Profilo');";
                        sScript += "$('.BottoneStop').hide();$('p#Respingi').hide();";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_GrdAllegati').show();";
                        sScript += "$('#lblTipoContatto').hide();";
                        sScript += "$('#lblDatiRiferimento').hide();";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_ddlTipoContatto').hide();";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_txtDatiRiferimento').hide();";
                        sScript += "$('.BottoneHelpMini').hide();$('.BottoneErase').hide();$('#Pulisci').hide();";
                        ShowHide("divDatiIstanza", true);
                        ShowHide("divMotivazione", true);
                    }
                    else
                    {
                        sScript += "$('#Deleghe').hide();$('#Profilo').show(25);";
                        sScript += "$('.BottoneKey').show();$('.BottoneSave').show();$('.BottoneBack').show();";
                        if (MySession.Current.UserLogged.IDContribLogged == MySession.Current.UserLogged.IDContribToWork && MySession.Current.UserLogged.IDDelegato <= 0)
                            sScript += "$('.BottoneDelegate').show();";
                        else
                            sScript += "$('.BottoneDelegate').hide();";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaSave').hide();$('#" + BLL.GestForm.PlaceHolderName.Title + "_CmdDelegaBack').hide();";
                        sScript += "$('.textheader').text('Profilo');";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Title + "_GrdAllegati').hide();";

                        ShowHide("divDatiIstanza", false);
                        ShowHide("divMotivazione", false);
                    }
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.ManageShowHideDelega::errore::", ex);
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
                if (TypeCombo == "TRIBUTO")
                    ListMyData = new BLL.EntiSistema(new EntiInLavorazione()).LoadEntiTributi(MySession.Current.Ente.IDEnte, -1, 1);

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.LoadPageCombo::errore::", ex);
                return ListMyData; 
            }
        }
        /// <summary>
        /// Funzione per la stampa della delega
        /// </summary>
        /// <param name="IdItem">int identificativo da stampare</param>
        protected void PrintDelega(int IdItem)
        {
            try
            {
                string sScriptDich = string.Empty;
                if (!new BLL.Deleghe(new Delega(), MySession.Current.Ente.IDEnte).GetDocPrint(IdItem, MySession.Current.Ente, out sScriptDich))
                {
                    string sScript = "$('#lblErrorBO').text('Errore in stampa!');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    string sNameHTMLtoPDF = "DELEGA" + Request.Cookies["__AntiXsrfToken"].Value + "_" + MySession.Current.UserLogged.IDContribToWork + ".html";
                    using (StreamWriter writetext = new StreamWriter(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF))
                    {
                        writetext.WriteLine(sScriptDich);
                        writetext.Close();
                    }
                    try
                    {
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Profilo", "Delega", "PrintDelega", "stampa delega", "", "", MySession.Current.Ente.IDEnte);
                        SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                        SelectPdf.PdfDocument doc = converter.ConvertUrl(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                        doc.Save(Response, false, "DELEGA.pdf");
                        doc.Close();
                    }
                    catch (Exception err)
                    {
                        if (err.Message != "Thread was being aborted.")
                        {
                            Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.PrintPDF::errore::", err);
                            string sScript = "$('#lblErrorBO').text('Errore in stampa!');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.PrintDelega::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
    }
}