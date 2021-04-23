using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.IO;


namespace OPENgovSPORTELLO.Settings
{
    /// <summary>
    /// Le configurazioni previste a sistema, hanno principalmente lo scopo di controllo sui dati inseriti dai contribuenti, fatta eccezione per l’IMU, in quanto sullo sportello deve essere possibile fare il calcolo del dovuto sulla base di una dichiarazione non ancora presente nel verticale.
    /// Sono previste funzioni di controllo che eviteranno perdite di dati già inseriti; si veda ad esempio il controllo di allert qualora un operatore tenti di eliminare un ente per il quale sono già stati configurati ulteriori tabelle(es. in configurazione comuni tento di eliminare il comune A per il quale è già stato configurato lo stradario)
    /// Le configurazioni: 
    ///<list type = "bullet" >
    ///<item> Elenco dei Comuni</item>
    ///<item>Stradario</item>
    ///<item>Operatori/Comuni con gestione delle autorizzazioni</item>
    ///<item>Aliquote IMU e TASI (*)</item>
    ///<item>Parametri IMU(*) – Caratteristica, Tipo utilizzo, Tipo Possesso, zone PRG, vincoli PRG, Categorie catastali, grado di parentela, concessione uso gratuito</item>
    ///<item>Categorie TARI (*)</item>
    ///<item>Parametri TARI(*)- Riduzioni, Esenzioni, Tipo utilizzo</item>
    ///<item>Giustificazioni</item>
    ///</list>
    ///(*) è possibile data Entry + ribaltamento AA a AA+1 o da Comune ad altro Comune.I ribaltamenti obbligano una successiva validazione da parte da operatore.
    ///<strong>Importazione dati da verticali</strong>
    ///Viene fatta ogni giorno, o a scadenze predefinite, eliminando e ripopolando la banca dati da nuovo.
    ///I flussi vengono appoggiati ad un percorso predefinito; l’importazione avverrà tramite servizio di windows.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class MngSettings : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MngSettings));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private string TypeConfig, IdTributo, DescrConfig;
        private string sScript = string.Empty;
        private BLL.Settings fncMng = new BLL.Settings();
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
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_btnUpload", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_btnCopyTo", false);
                ShowHide("lblAnno", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnno", false);
                string sScript = string.Empty;
                if (MySession.Current.Ente != null)
                {
                    sScript += new BLL.GestForm().GetHelp("HelpBOSettingsEnte", MySession.Current.Ente.UrlWiki);
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.Page_Init::errore::", ex);
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
                LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                if (!Page.IsPostBack)
                {
                    ShowHide("SearchEnti", false);
                    ShowHide("UserDetail", false);

                    int Anno = 0;
                    int.TryParse(txtAnno.Text, out Anno);
                    Ribes.OPENgov.WebControls.RibesGridView myGrd;
                    if (TypeConfig == GenericCategory.TIPO.Enti)
                        myGrd = GrdEnti;
                    else if (TypeConfig == GenericCategory.TIPO.Operatori)
                        myGrd = GrdOperatori;
                    else if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote || TypeConfig == GenericCategory.TIPO.ICI_Zone || TypeConfig == GenericCategory.TIPO.ICI_Vincoli
                        || TypeConfig == GenericCategory.TIPO.TASI_Aliquote || TypeConfig == GenericCategory.TIPO.TASI_Agevolazioni)
                        myGrd = GrdAliquote;
                    else if (TypeConfig == GenericCategory.TIPO.Documenti)
                        myGrd = GrdDoc;
                    else
                        myGrd = GrdConfig;
                    if (!LoadData(ddlEnte.SelectedValue, Anno, TypeConfig, true, ddlEnte, ddlEnteDest, myGrd, GrdComuni))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "Page_Load", "ingresso pagina", "", "", "");
                }
                ManageShowHide();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetLabel("MngSettings_" + TypeConfig, "");
                RegisterScript(sScript, this.GetType());
            }
        }
        #region "Bottoni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search(object sender, EventArgs e)
        {
            try
            {
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                int Anno = 0;
                int.TryParse(txtAnno.Text, out Anno);
                try
                {
                    DropDownList ddlRowTributo = (DropDownList)sender;
                    GridViewRow myRow = (GridViewRow)ddlRowTributo.NamingContainer;

                    ListMyData = fncMng.LoadTipoIstanze(ddlRowTributo.SelectedValue, string.Empty, false);
                    DropDownList ddlRowIstanze = (DropDownList)myRow.Cells[1].FindControl("ddlIstanze");
                    new General().LoadCombo(ddlRowIstanze, ListMyData, "Codice", "Descrizione");// DataSource='<%# LoadCombo("TIPOISTANZE") %>' DataTextField="Descrizione" DataValueField="Codice" SelectedValue='<%# Eval("IDTipoIstanza") %>'
                }
                catch
                {
                    Ribes.OPENgov.WebControls.RibesGridView myGrd;
                    if (TypeConfig == GenericCategory.TIPO.Documenti)
                        myGrd = GrdDoc;
                    else if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote || TypeConfig == GenericCategory.TIPO.ICI_Zone || TypeConfig == GenericCategory.TIPO.ICI_Vincoli
                        || TypeConfig == GenericCategory.TIPO.TASI_Aliquote || TypeConfig == GenericCategory.TIPO.TASI_Agevolazioni)
                        myGrd = GrdAliquote;
                    else
                        myGrd = GrdConfig;
                    if (!LoadData(ddlEnte.SelectedValue, Anno, TypeConfig, false, ddlEnte, ddlEnteDest, myGrd, GrdComuni))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.Search::errore::", ex);
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
            bool IsCopy = false;
            Ribes.OPENgov.WebControls.RibesGridView myGrd;

            try
            {
                string sMyErr = string.Empty;

                LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                if (fncMng.FieldValidator(ddlEnte.SelectedValue, txtAnno.Text, hfIsCopyTo.Value, ddlEnteDest.SelectedValue, txtAnnoDest.Text, TypeConfig, out sMyErr))
                {
                    if (TypeConfig == GenericCategory.TIPO.Enti)
                        myGrd = GrdEnti;
                    else if (TypeConfig == GenericCategory.TIPO.Operatori)
                        myGrd = GrdOperatori;
                    else if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote || TypeConfig == GenericCategory.TIPO.ICI_Zone || TypeConfig == GenericCategory.TIPO.ICI_Vincoli
                        || TypeConfig == GenericCategory.TIPO.TASI_Aliquote || TypeConfig == GenericCategory.TIPO.TASI_Agevolazioni)
                        myGrd = GrdAliquote;
                    else if (TypeConfig == GenericCategory.TIPO.Documenti)
                        myGrd = GrdDoc;
                    else
                        myGrd = GrdConfig;
                    if (hfIsCopyTo.Value != "0")
                        IsCopy = true;
                    int myAnnoFrom, myAnnoTo;
                    int.TryParse(txtAnno.Text, out myAnnoFrom);
                    int.TryParse(txtAnnoDest.Text, out myAnnoTo);

                    if (!fncMng.Save(myGrd, MySession.Current.ListEnti, ddlEnte.SelectedValue, TypeConfig, IdTributo, myAnnoFrom, IsCopy, ddlEnteDest.SelectedValue, myAnnoTo, out sMyErr))
                    {
                        sMyErr = "$('#lblErrorBO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorBO').show();";
                        RegisterScript(sMyErr, this.GetType());
                    }
                    else
                    {
                        if (!LoadData(ddlEnte.SelectedValue, myAnnoFrom, TypeConfig, false, ddlEnte, ddlEnteDest, myGrd, GrdComuni))
                        {
                            sMyErr += "$('#lblErrorBO').text('Errore in caricamento pagina');$('#lblErrorBO').show();";
                            RegisterScript(sMyErr, this.GetType());
                        }
                        else {
                            sMyErr += "$('#lblErrorBO').text('Salvataggio effettuato con successo!');$('#lblErrorBO').show();";
                            RegisterScript(sMyErr, this.GetType());
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "Save", "salvataggio configurazioni", "", "", "");
                        }
                        hfIsCopyTo.Value = "0";
                        txtAnnoDest.Text = "";
                        ShowHideRibalta("divDest", BLL.GestForm.PlaceHolderName.Body + "_hfIsCopyTo");
                    }
                    if (TypeConfig == GenericCategory.TIPO.Stradario || TypeConfig == GenericCategory.TIPO.Documenti
                        || TypeConfig == GenericCategory.TIPO.TARSU_Motivazioni || TypeConfig == GenericCategory.TIPO.TARSU_Vani
                        || TypeConfig == GenericCategory.TIPO.ICI_Motivazioni || TypeConfig == GenericCategory.TIPO.ICI_Categorie || TypeConfig == GenericCategory.TIPO.ICI_Caratteristica
                        || TypeConfig == GenericCategory.TIPO.OSAP_Richiedente || TypeConfig == GenericCategory.TIPO.OSAP_Motivazioni
                        || TypeConfig == GenericCategory.TIPO.ICP_Motivazioni)
                    {
                        ShowHide("lblAnnoDest", false);
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnnoDest", false);
                    }
                }
                else
                {
                    sMyErr = "$('#lblErrorBO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorBO').show();";
                    RegisterScript(sMyErr, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHide();
            }
        }

        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            if (((System.Web.UI.WebControls.Button)sender).ID == "btnBackSearchEnti")
            {
                ShowHide("SearchEnti", false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "Back", "uscita ricerca enti", "", "", "");
            }
            else if (((System.Web.UI.WebControls.Button)sender).ID == "btnBackUserDetail"
                || ((System.Web.UI.WebControls.Button)sender).ID == "btnBackEnteMailDetail"
                || ((System.Web.UI.WebControls.Button)sender).ID == "btnBackEnteCartografiaDetail"
                || ((System.Web.UI.WebControls.Button)sender).ID == "btnBackEnteVerticaliDetail"
                || ((System.Web.UI.WebControls.Button)sender).ID == "btnBackEnteLogoDetail"
                || ((System.Web.UI.WebControls.Button)sender).ID == "btnBackPWDOperatore"
            )
            {
                ShowHide("UserDetail", false); ShowHide("EnteMailDetail", false); ShowHide("EnteCartografiaDetail", false); ShowHide("EnteVerticaliDetail", false); ShowHide("EntePagoPADetail", false); ShowHide("EnteLogoDetail", false);
                ShowHide("PWDOperatore", false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "Back", "uscita dettaglio utente", "", "", "");
            }
            else
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "Page_Load", "uscita pagina", "", "", "");
                Response.Redirect(UrlHelper.GetBOSettingsBase);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CopyTo(object sender, EventArgs e)
        {
            try
            {
                hfIsCopyTo.Value = "1";
                ShowHideRibalta("divDest", BLL.GestForm.PlaceHolderName.Body + "_hfIsCopyTo");
                if (TypeConfig == GenericCategory.TIPO.Stradario || TypeConfig == GenericCategory.TIPO.Documenti
                        || TypeConfig == GenericCategory.TIPO.TARSU_Motivazioni || TypeConfig == GenericCategory.TIPO.TARSU_Vani
                        || TypeConfig == GenericCategory.TIPO.ICI_Motivazioni || TypeConfig == GenericCategory.TIPO.ICI_Categorie || TypeConfig == GenericCategory.TIPO.ICI_Caratteristica || TypeConfig == GenericCategory.TIPO.ICI_Possesso
                        || TypeConfig == GenericCategory.TIPO.OSAP_Richiedente || TypeConfig == GenericCategory.TIPO.OSAP_Motivazioni
                        || TypeConfig == GenericCategory.TIPO.ICP_Motivazioni)
                {
                    ShowHide("lblAnnoDest", false);
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnnoDest", false);
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "CopyTo", "ribaltato configurazione", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.CopyTo::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                int Anno;
                int.TryParse(txtAnno.Text, out Anno);
                if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote || TypeConfig == GenericCategory.TIPO.ICI_Vincoli
                    || TypeConfig == GenericCategory.TIPO.TASI_Aliquote || TypeConfig == GenericCategory.TIPO.TASI_Agevolazioni
                )
                {
                    List<GenericCategoryWithRate> ListGen = new List<GenericCategoryWithRate>();
                    LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                    foreach (GridViewRow myRow in GrdAliquote.Rows)
                    {
                        GenericCategoryWithRate myItem = new GenericCategoryWithRate();
                        
                        myItem = fncMng.LoadGenericCatWithRateFromGrd(myRow, ddlEnte.SelectedValue, TypeConfig, IdTributo, Anno, true);
                        ListGen.Add(myItem);
                    }
                    GrdAliquote.DataSource = ListGen;
                    GrdAliquote.DataBind();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Upload(object sender, EventArgs e)
        {
            try
            {
                hfTypeUserDetail.Value = "FlussiCarico";
                sScript = "$('#lblEnteFileAttach').text('Flussi Carico');$('#divDescrFlussiCarico').show();$('#lblEsitoUploadFlussi').text('');";
                RegisterScript(sScript, this.GetType());
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdLogo", false);
                ShowHide("EnteLogoDetail", true);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.Upload::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchEnti(object sender, EventArgs e)
        {
            try
            {
                LoadSearchComuni();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "SearchEnti", "ricerca comuni", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.SearchEnti::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachUserDetail(object sender, EventArgs e)
        {
            try
            {
                List<string> ListMyData = new List<string>();

                foreach (GridViewRow myRow in GrdUserDetail.Rows)
                {
                    if (((CheckBox)myRow.Cells[1].FindControl("chkSel")).Checked)
                        ListMyData.Add(((HiddenField)myRow.Cells[1].FindControl("hfCodice")).Value);
                }
                if (ListMyData.Count > 0)
                {
                    if (GrdOperatori.Rows.Count > 0)
                    {
                        List<UserRole> ListUsers = new List<UserRole>();
                        foreach (GridViewRow myRow in GrdOperatori.Rows)
                        {
                            UserRole myUser = fncMng.LoadUserFromGrd(myRow);
                            if (myUser.ID.ToString() == hfIdRow.Value)
                            {
                                if (hfTypeUserDetail.Value == "Enti")
                                {
                                    if (myUser.IDTipoProfilo == UserRole.PROFILO.OperatoreEnte && ListMyData.Count > 1)
                                    {
                                        ShowHide("UserDetail", false);
                                        RegisterScript("$('#lblErrorBO').text('Selezionare una voce soltanto!');$('#lblErrorBO').show();", this.GetType());
                                        return;
                                    }
                                    myUser.Enti = ListMyData;
                                }
                                else
                                    myUser.Tributi = ListMyData;
                            }
                            ListUsers.Add(myUser);
                        }
                        GrdOperatori.DataSource = ListUsers;
                        GrdOperatori.DataBind();
                        ShowHide("UserDetail", false);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachUserDetail", "inserito dettagli su operatore", "", "", "");
                    }
                    else if (GrdEnti.Rows.Count > 0)
                    {
                        List<GenericCategory> ListTrib = new List<GenericCategory>();
                        foreach (string myStr in ListMyData)
                        {
                            ListTrib.Add(new GenericCategory() { Codice = myStr, IDTributo = myStr, IsActive = 1 });
                        }
                        List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                        foreach (EntiInLavorazione myEnte in MySession.Current.ListEnti)
                        {
                            if (myEnte.ID.ToString() == hfIdRow.Value)
                            {
                                myEnte.ListTributi = ListTrib;
                            }
                            ListEnti.Add(myEnte);
                        }
                        GrdEnti.DataSource = ListEnti;
                        GrdEnti.DataBind();
                        MySession.Current.ListEnti = ListEnti;
                        ShowHide("UserDetail", false);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachUserDetail", "inserito tributi su ente", "", "", "");
                    }
                }
                else
                {
                    RegisterScript("Selezionare almeno una voce", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachUserDetail::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachEnteMailDetail(object sender, EventArgs e)
        {
            try
            {
                BaseMail myData = new BaseMail();

                if (txtMailSender.Text != String.Empty)
                {
                    myData.Sender = txtMailSender.Text;
                    myData.SenderName = txtMailSenderName.Text;
                    if (optYes.Checked)
                        myData.SSL = 1;
                    else
                        myData.SSL = 0;
                    myData.Server = txtMailServer.Text;
                    myData.ServerPort = txtMailServerPort.Text;
                    myData.Password = txtMailPassword.Text;
                    myData.Ente = txtMailEnte.Text;
                    myData.BackOffice = txtMailBackOffice.Text;
                    myData.Archive = txtMailArchive.Text;
                    myData.Protocollo = txtMailProtocollo.Text;
                    myData.WarningRecipient = txtMailWarningRecipient.Text;
                    myData.WarningSubject = txtMailWarningSubject.Text;
                    myData.WarningMessage = txtMailWarningMessage.Text;
                    myData.SendErrorMessage = txtMailSendErrorMessage.Text;
                }
                List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                foreach (EntiInLavorazione myEnte in MySession.Current.ListEnti)
                {
                    if (myEnte.ID.ToString() == hfIdRow.Value)
                    {
                        myEnte.MailEnte = txtMailEnte.Text;
                        myEnte.Mail = myData;
                    }
                    ListEnti.Add(myEnte);
                }
                GrdEnti.DataSource = ListEnti;
                GrdEnti.DataBind();
                MySession.Current.ListEnti = ListEnti;
                ShowHide("EnteMailDetail", false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachEnteMailDetail", "inserito BaseMail su ente", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteMailDetail::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachEnteCartografiaDetail(object sender, EventArgs e)
        {
            try
            {
                BaseCartografia myData = new BaseCartografia();

                myData.Url = txtCartoUrl.Text;
                myData.UrlAuth = txtCartoUrlAuth.Text;
                myData.Token = txtCartoToken.Text;
                if (optCartoYes.Checked)
                    myData.IsActive = 1;
                else
                    myData.IsActive = 0;

                List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                foreach (EntiInLavorazione myEnte in MySession.Current.ListEnti)
                {
                    if (myEnte.ID.ToString() == hfIdRow.Value)
                    {
                        myEnte.SIT = myData;
                    }
                    ListEnti.Add(myEnte);
                }
                GrdEnti.DataSource = ListEnti;
                GrdEnti.DataBind();
                MySession.Current.ListEnti = ListEnti;
                ShowHide("EnteCartografiaDetail", false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachEnteCartografiaDetail", "inserito BaseSIT su ente", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteCartografiaDetail::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachEnteVerticaliDetail(object sender, EventArgs e)
        {
            try
            {
                BaseVerticali myData = new BaseVerticali();

                if (txtAnnoVertICI.Text != string.Empty)
                    myData.AnnoVerticaleICI = int.Parse(txtAnnoVertICI.Text);
                myData.AnniUsoGratuito = txtAnniUsoGrat.Text;
                if (optVertInt.Checked)
                {
                    myData.TipoBancaDati = "I";
                }
                else
                {
                    myData.TipoBancaDati = "E";
                    myData.TipoFornitore = ddlFornitore.SelectedValue;
                }
                    
                    
                myData.TipoFornitore = ddlFornitore.SelectedValue;

                List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                foreach (EntiInLavorazione myEnte in MySession.Current.ListEnti)
                {
                    if (myEnte.ID.ToString() == hfIdRow.Value)
                    {
                        myEnte.DatiVerticali = myData;
                    }
                    ListEnti.Add(myEnte);
                }
                GrdEnti.DataSource = ListEnti;
                GrdEnti.DataBind();
                MySession.Current.ListEnti = ListEnti;
                ShowHide("EnteVerticaliDetail", false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachEnteVerticaliDetail", "inserito DatiVerticali su ente", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteVerticaliDetail::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachEntePagoPADetail(object sender, EventArgs e)
        {
            try
            {
                BasePagoPA myData = new BasePagoPA();

                myData.CARTId = txtCARTId.Text;
                myData.CARTSys = txtCARTSys.Text;
                myData.IBAN = txtIBAN.Text;
                myData.DescrIBAN = txtDescrIBAN.Text;
                myData.IdRiscossore = txtIdRiscossore.Text;
                myData.DescrRiscossore = txtDescrRiscossore.Text;

                List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                foreach (EntiInLavorazione myEnte in MySession.Current.ListEnti)
                {
                    if (myEnte.ID.ToString() == hfIdRow.Value)
                    {
                        myEnte.DatiPagoPA = myData;
                    }
                    ListEnti.Add(myEnte);
                }
                GrdEnti.DataSource = ListEnti;
                GrdEnti.DataBind();
                MySession.Current.ListEnti = ListEnti;
                ShowHide("EntePagoPADetail", false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachEntePagoPADetail", "inserito BasePagoPA su ente", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEntePagoPADetail::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AttachEnteLogoDetail(object sender, EventArgs e)
        {
            try
            {
                string EsitoUpload = string.Empty;
                int nUpload = 0;
                if (hfTypeUserDetail.Value == "FlussiCarico")
                {
                    HttpFileCollection ListFiles = Request.Files;
                    if (ListFiles != null)
                    {
                        for (int i = 0; i < ListFiles.Count; i++)
                        {
                            HttpPostedFile PostedFile = ListFiles[i];
                            try
                            {
                                if (PostedFile.ContentLength > 0)
                                {
                                    if (PostedFile.FileName.Length > 6)
                                    {
                                        nUpload += 1;
                                        //copio file in directory
                                        string fileName = Path.GetFileName(PostedFile.FileName);
                                        string saveAsPath = Path.Combine(UrlHelper.GetPathFlussiCarico, fileName);
                                        Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteLogoDetail.UploadFlussiCarico::FILENAME::" + fileName);
                                        Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteLogoDetail.UploadFlussiCarico::SAVEASPATH::" + saveAsPath);
                                        Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteLogoDetail.UploadFlussiCarico::URLHELPER.GETPATHFLUSSICARICO::" + UrlHelper.GetPathFlussiCarico);

                                        PostedFile.SaveAs(saveAsPath);
                                        //richiamo conversione

                                        //prelevo TUTTI i dati utili dell'ente, ho scomposto la procedura
                                        EntiInLavorazione EnteInLavorazione = new EntiInLavorazione();
                                        EnteInLavorazione = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(PostedFile.FileName.Substring(0, 6), MySession.Current.UserLogged.NameUser);

                                        //prelevo il codice del fornitore e lo assegno ad una variabile temporanea
                                        List<GenericCategory> VariabileAppoggio = new List<GenericCategory>();
                                        VariabileAppoggio = fncMng.LoadFornitori(EnteInLavorazione.IDEnte);

                                        
                                        //assegno il valore prelevato all'oggetto che verrà passato per l'elaborazione
                                        EnteInLavorazione.DatiVerticali.TipoFornitore = VariabileAppoggio[0].Codice;

                                        //in sostituzione all'originale
                                        new BLLImport().UploadFlussiCarico(EnteInLavorazione, PostedFile.FileName, out EsitoUpload);
                                    }
                                }
                            }
                            catch (Exception Ex)
                            {
                                EsitoUpload = "Errore in upload file";
                                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteLogoDetail.UploadFlussiCarico::errore::", Ex);
                            }
                        }
                    }
                    if (nUpload <= 0)
                    {
                        EsitoUpload = "Selezionare un file";
                    }
                    sScript = "$('#lblEsitoUploadFlussi').text('" + EsitoUpload + "');";
                    if (EsitoUpload != "Carico effettuato con successo")
                    {
                        sScript += "$('#lblEsitoUploadFlussi').removeClass('text-success');$('#lblEsitoUploadFlussi').addClass('text-danger');";
                    }
                    else
                    {
                        sScript += "$('#lblEsitoUploadFlussi').removeClass('text-danger');$('#lblEsitoUploadFlussi').addClass('text-success');";
                    }
                    sScript += "$('#lblEnteFileAttach').text('Flussi Carico');$('#divDescrFlussiCarico').show();";
                    RegisterScript(sScript, this.GetType());
                    hfTypeUserDetail.Value = "FlussiCarico";
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdLogo", false);
                    ShowHide("EnteLogoDetail", true);

                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "UploadFlussiCarico", "caricato flussi", "", "", "");
                }
                else {
                    BaseLogo myData = new BaseLogo();

                    HttpFileCollection ListFiles = Request.Files;
                    if (ListFiles != null)
                    {
                        for (int i = 0; i < ListFiles.Count; i++)
                        {
                            HttpPostedFile PostedFile = ListFiles[i];
                            try
                            {
                                if (PostedFile.ContentLength > 0)
                                {
                                    byte[] data = new Byte[PostedFile.ContentLength];
                                    PostedFile.InputStream.Read(data, 0, PostedFile.ContentLength);
                                    myData.PostedFile = data;
                                    myData.NameLogo = PostedFile.FileName;
                                    myData.FileMIMEType = PostedFile.ContentType;
                                }
                            }
                            catch (Exception Ex)
                            {
                                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.UploadAttachments::errore::", Ex);
                                myData = new BaseLogo();
                            }
                        }
                    }

                    List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                    foreach (EntiInLavorazione myEnte in MySession.Current.ListEnti)
                    {
                        if (myEnte.ID.ToString() == hfIdRow.Value)
                        {
                            myEnte.Logo = myData;
                        }
                        ListEnti.Add(myEnte);
                    }
                    GrdEnti.DataSource = ListEnti;
                    GrdEnti.DataBind();
                    MySession.Current.ListEnti = ListEnti;

                    ShowHide("EnteLogoDetail", false);
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachEnteLogoDetail", "inserito logo su ente", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.AttachEnteVerticaliDetail::errore::", ex);
                LoadException(ex);
            }
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PasswordHash"></param>
        /// <returns></returns>
        protected bool ChangePassword(string ID, string PasswordHash)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_ChangePassword", "ID", "PASSWORDHASH");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("ID", ID)
                        , ctx.GetParam("PASSWORDHASH", PasswordHash)).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.ChangePassword::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PWDOperatore(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var managerCreate = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = manager.FindByName(txtNameUser.Text);
                if (user == null)
                {
                    var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                    //genero password
                    string RandomPassword = new ApplicationUser().CreatePassword(8);
                    var NewUser = new ApplicationUser() { UserName = txtNameUser.Text, Email = txtNameUser.Text, IDROLE = UserRole.PROFILO.OperatoreEnte, CodiceFiscale = ".", PWDToSend = string.Empty, EmailConfirmed=true,LastPasswordChangedDate=DateTime.Now };
                    IdentityResult result = managerCreate.Create(NewUser, RandomPassword);
                    if (!result.Succeeded)
                    {
                        sScript += "$('#lblErrorBO').text('Errore in salvataggio!');$('#lblErrorBO').show();";
                        RegisterScript(sScript, this.GetType());
                        return;
                    }
                    user = manager.FindByName(txtNameUser.Text);
                }

                if (new ApplicationUser().ValidatePassword(Password.Text))
                {
                    string myToken=manager.GeneratePasswordResetToken(user.Id);
                    var result = manager.ResetPassword(user.Id, myToken, Password.Text);
                    if (result.Succeeded)
                    {
                        user.LastPasswordChangedDate = DateTime.UtcNow;
                        manager.Update(user);
                        List<UserRole> ListGen = fncMng.LoadUserRole(string.Empty, string.Empty, false, string.Empty, MySession.Current.UserLogged.NameUser);
                        GrdOperatori.DataSource = ListGen;
                        GrdOperatori.DataBind();
                        ShowHide("PWDOperatore", false);
                        sScript += "$('#lblErrorBO').text('Salvataggio password effettuato con successo!');$('#lblErrorBO').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                    else
                    {
                        sScript += "$('#lblErrorBO').text('Errore in salvataggio!');$('#lblErrorBO').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('La password non rispetta i criteri di formattazione! NON DEVE contenere spazi, DEVE essere lunga almeno 8 caratteri, avere almeno un numero(0-9), avere almeno una lettera minuscola(a-z),avere almeno una lettera maiuscola(A-Z),avere almeno un carattere speciale<br />(@ ! . _ - & + = # $ % ^)');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.PWDOperatore::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageShowHide();
            }
        }
        #endregion
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdConfigRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "I")
                    {
                        if (TypeConfig != GenericCategory.TIPO.ICI_Motivazioni && TypeConfig != GenericCategory.TIPO.ICI_Zone && TypeConfig != GenericCategory.TIPO.ICI_Vincoli
                              && TypeConfig != GenericCategory.TIPO.TASI_Aliquote && TypeConfig != GenericCategory.TIPO.TASI_Motivazioni
                              && TypeConfig != GenericCategory.TIPO.TARSU_Motivazioni
                              && TypeConfig != GenericCategory.TIPO.OSAP_Motivazioni
                              && TypeConfig != GenericCategory.TIPO.ICP_Motivazioni
                          )
                        {
                            GrdConfig.Enabled = false;
                            RegisterScript("$('.BottoneRecycle').hide();$('.BottoneSave').hide();$('.BottoneNewGrd').hide();", this.GetType());
                        }
                    }
                }
                if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore)
                    GrdConfig.Columns[3].Visible = false;
                else
                    GrdConfig.Columns[3].Visible = true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdConfigRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdConfigRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow, Anno;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                int.TryParse(txtAnno.Text, out Anno);
                if (ddlEnte.SelectedValue == string.Empty)
                {
                    sScript += "$('#lblErrorBO').text('Selezionare un’ente');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    List<GenericCategory> ListGen = new List<GenericCategory>();
                    LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                    foreach (GridViewRow myRow in GrdConfig.Rows)
                    {
                        GenericCategory myItem = new GenericCategory();
                        myItem = fncMng.LoadGenericCatFromGrd(myRow, ddlEnte.SelectedValue, TypeConfig, IdTributo, Anno);
                        ListGen.Add(myItem);
                    }
                    GenericCategory mySetting = new GenericCategory { ID = IDRow };
                    switch (e.CommandName)
                    {
                        case "DeleteConf":
                            if (new BLL.BLLGenericCategory(mySetting).Delete())
                            {
                                foreach (GenericCategory myItem in ListGen)
                                {
                                    if (myItem.ID == IDRow)
                                    {
                                        ListGen.Remove(myItem);
                                        break;
                                    }
                                }
                                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "DeleteConf", "eliminato configurazione", "", "", "");
                            }
                            else
                            {
                                sScript += "$('#lblErrorBO').text('Impossibile eliminare la voce');$('#lblErrorBO').show();";
                                RegisterScript(sScript, this.GetType());
                                return;
                            }
                            break;
                        default:
                            ListGen.Add(new GenericCategory());
                            ListGen = ListGen.OrderBy(o => o.ID).ThenBy(o => o.Descrizione).ToList();
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "NewConf", "nuova configurazione", "", "", "");
                            break;
                    }
                    GrdConfig.DataSource = ListGen;
                    GrdConfig.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdConfigRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdEntiRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore)
                        RegisterScript("$('.BottoneNewGrd').hide()", this.GetType());
                }
                if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore)
                    GrdEnti.Columns[8].Visible = false;
                else
                    GrdEnti.Columns[8].Visible = true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdEntiRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdEntiRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow, Anno;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                int.TryParse(txtAnno.Text, out Anno);
                List<EntiInLavorazione> ListGen = new List<EntiInLavorazione>();
                LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                EntiInLavorazione myEnteGrd = new EntiInLavorazione();
                foreach (EntiInLavorazione myItem in MySession.Current.ListEnti)
                {
                    foreach (GridViewRow myRow in GrdEnti.Rows)
                    {
                        if (((TextBox)myRow.FindControl("txtCodice")).Text == myItem.IDEnte)
                        {
                            myEnteGrd = fncMng.LoadEntiFromGrd(myRow);
                        }
                    }
                    myItem.Descrizione = myEnteGrd.Descrizione;
                    myItem.Ambiente = myEnteGrd.Ambiente;
                    ListGen.Add(myItem);
                }
                EntiInLavorazione mySetting = new EntiInLavorazione { ID = IDRow };
                switch (e.CommandName)
                {
                    case "DeleteRow":
                        if (new BLL.EntiSistema(mySetting).Delete())
                        {
                            foreach (EntiInLavorazione myItem in ListGen)
                            {
                                if (myItem.ID == IDRow)
                                {
                                    ListGen.Remove(myItem);
                                    break;
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "DeleteConf", "eliminato ente", "", "", "");
                        }
                        else
                        {
                            sScript = "$('#lblErrorBO').text('Impossibile eliminare Ente');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                        break;
                    case "FindRow":
                        if (IDRow > 0)
                        {
                            sScript = "$('#lblErrorBO').text('Impossibile cambiare Ente');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                        else {
                            hfIdRow.Value = IDRow.ToString();
                            ShowHide("SearchEnti", true);
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni Enti", "Dettaglio", "FindRow", "ricerca comune", "", "", "");
                        }
                        break;
                    case "AttachRow":
                        GrdLogo.DataSource = new BLL.EntiSistema(new EntiInLavorazione()).LoadBaseLogo(string.Empty, IDRow);
                        GrdLogo.DataBind();
                        hfIdRow.Value = IDRow.ToString();
                        hfTypeUserDetail.Value = "Logo";
                        sScript = "$('#lblEnteFileAttach').text('Logo');$('#divDescrFlussiCarico').hide();";
                        RegisterScript(sScript, this.GetType());
                        ShowHide("EnteLogoDetail", true);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni Enti", "Dettaglio", "AttachRow", "consulta tributi associati", "", "", "");
                        break;
                    case "ObjectRow":
                        List<GenericCategory> ListEntiLav = new List<GenericCategory>();
                        ListEntiLav = new BLL.EntiSistema(new EntiInLavorazione()).LoadEntiTributi(string.Empty, IDRow,0);
                        GrdUserDetail.DataSource = ListEntiLav;
                        if(ListEntiLav.Count()>0)
                        GrdUserDetail.PageSize = ListEntiLav.Count();
                        GrdUserDetail.AllowPaging = false;
                        GrdUserDetail.DataBind();
                        hfIdRow.Value = IDRow.ToString();
                        hfTypeUserDetail.Value = "Tributi";
                        RegisterScript("$('#lblIntestUserDetail').text('Tributi');", this.GetType());
                        ShowHide("UserDetail", true);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni Enti", "Dettaglio", "ObjectRow", "consulta tributi associati", "", "", "");
                        break;
                    case "MailRow":
                        BaseMail myMail = new BLL.EntiSistema(new EntiInLavorazione()).LoadBaseMail(string.Empty, IDRow);
                        txtMailSender.Text = myMail.Sender;
                        txtMailSenderName.Text = myMail.SenderName;
                        if (myMail.SSL==1)
                            optYes.Checked = true;
                        txtMailServer.Text = myMail.Server;
                        txtMailServerPort.Text = myMail.ServerPort;
                        txtMailPassword.Text = myMail.Password;
                        txtMailEnte.Text = myMail.Ente;
                        txtMailBackOffice.Text = myMail.BackOffice;
                        txtMailArchive.Text = myMail.Archive;
                        txtMailProtocollo.Text = myMail.Protocollo;
                        txtMailWarningRecipient.Text = myMail.WarningRecipient;
                        txtMailWarningSubject.Text = myMail.WarningSubject;
                        txtMailWarningMessage.Text = myMail.WarningMessage;
                        txtMailSendErrorMessage.Text = myMail.SendErrorMessage;

                        hfIdRow.Value = IDRow.ToString();
                        hfTypeUserDetail.Value = "Mail";
                        ShowHide("EnteMailDetail", true);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni Enti", "Dettaglio", "MailRow", "consulta enti associati", "", "", "");
                        break;
                    case "CatastoRow":
                        BaseCartografia mySIT = new BLL.EntiSistema(new EntiInLavorazione()).LoadBaseCartografia(string.Empty, IDRow);
                        txtCartoUrl.Text = mySIT.Url;
                        txtCartoUrlAuth.Text = mySIT.UrlAuth;
                        txtCartoToken.Text = mySIT.Token;
                        if (mySIT.IsActive == 1)
                        {
                            optCartoYes.Checked = true;
                            optCartoNo.Checked = false;
                        }
                        else {
                            optCartoYes.Checked = false;
                            optCartoNo.Checked = true;
                        }

                        hfIdRow.Value = IDRow.ToString();
                        hfTypeUserDetail.Value = "SIT";
                        ShowHide("EnteCartografiaDetail", true);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "CatastoRow", "consulta enti associati", "", "", "");
                        break;
                    case "VerticaleRow":
                        new General().LoadCombo(ddlFornitore, fncMng.LoadFornitori(""), "CODICE", "DESCRIZIONE");
                        BaseVerticali myVert = new BLL.EntiSistema(new EntiInLavorazione()).LoadBaseVerticali(string.Empty, IDRow);
                        txtAnnoVertICI.Text = myVert.AnnoVerticaleICI.ToString();
                        txtAnniUsoGrat.Text = myVert.AnniUsoGratuito;
                        txtDataAgg.Text = FncGrd.FormattaDataGrd(myVert.DataAggiornamento);
                        ddlFornitore.SelectedValue = myVert.TipoFornitore;

                        hfIdRow.Value = IDRow.ToString();
                        hfTypeUserDetail.Value = "Dati Verticali";
                        ShowHide("EnteVerticaliDetail", true);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "VerticaleRow", "consulta enti associati", "", "", "");
                        break;
                    case "PagoPARow":
                        BasePagoPA myData = new BLL.EntiSistema(new EntiInLavorazione()).LoadBasePagoPA(string.Empty, IDRow);
                        txtCARTId.Text = myData.CARTId;
                        txtCARTSys.Text = myData.CARTSys;
                        txtIBAN.Text = myData.IBAN;
                        txtDescrIBAN.Text = myData.DescrIBAN;
                        txtIdRiscossore.Text = myData.IdRiscossore;
                        txtDescrRiscossore.Text = myData.DescrRiscossore;

                        hfIdRow.Value = IDRow.ToString();
                        hfTypeUserDetail.Value = "PagoPA";
                        ShowHide("EntePagoPADetail", true);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "PagoPARow", "consulta enti associati", "", "", "");
                        break;
                    default:
                        ListGen.Add(new EntiInLavorazione());
                        ListGen = ListGen.OrderBy(o => o.ID).ThenBy(o => o.Descrizione).ToList();
                        MySession.Current.ListEnti = ListGen;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "NewEnti", "nuovo ente", "", "", "");
                        break;
                }
                GrdEnti.DataSource = ListGen;
                GrdEnti.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdEntiRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                ManageShowHide();
            }
        }
        /// <summary>
        /// Funzione di gestione cambio pagina della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdComuniPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearchComuni(e.NewPageIndex);
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
                string IDSetting = e.CommandArgument.ToString();
                List<EntiInLavorazione> ListGen = new List<EntiInLavorazione>();
                switch (e.CommandName)
                {
                    case "AttachRow":
                        foreach (EntiInLavorazione myItem in MySession.Current.ListEnti)
                        {
                            if (myItem != null)
                            {
                                if (myItem.ID.ToString() == hfIdRow.Value)
                                {
                                    foreach (GridViewRow myEnte in GrdComuni.Rows)
                                    {
                                        if (myEnte.Cells[0].Text == IDSetting.ToString())
                                        {
                                            myItem.IDEnte = myEnte.Cells[0].Text;
                                            myItem.Descrizione = myEnte.Cells[1].Text;
                                            break;
                                        }
                                    }
                                }
                                ListGen.Add(myItem);
                            }
                            else
                            {
                                sScript = "$('#lblErrorBO').text('Ente non valido!');$('#lblErrorBO').show();";
                                RegisterScript(sScript, this.GetType());
                                return;
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "AttachRow", "associato comune", "", "", "");
                        break;
                    default:
                        break;
                }
                GrdEnti.DataSource = ListGen;
                GrdEnti.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdOperatoriRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore)
                    GrdOperatori.Columns[5].Visible = false;
                else
                    GrdOperatori.Columns[5].Visible = true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdOperatoriRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdOperatoriRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                List<UserRole> ListGen = new List<UserRole>();

                    foreach (GridViewRow myRow in GrdOperatori.Rows)
                    {
                        UserRole myItem = new UserRole();
                        myItem = fncMng.LoadUserFromGrd(myRow);
                        ListGen.Add(myItem);
                    }
                if (IDRow == string.Empty && (e.CommandName== "TownRow" || e.CommandName== "ObjectRow"))
                {
                    foreach (UserRole myItem in ListGen)
                    {
                        if (myItem.ID == IDRow)
                        {
                            sScript = "$('#lblErrorBO').text('Inserire la password per l’operatore " + myItem.NameUser + "!');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            break;
                        }
                    }                    
                }
                else {
                    UserRole mySetting = new UserRole { ID = IDRow };
                    switch (e.CommandName)
                    {
                        case "DeleteRow":
                            if (new BLL.User(mySetting).Delete())
                            {
                                foreach (UserRole myItem in ListGen)
                                {
                                    if (myItem.ID == IDRow)
                                    {
                                        ListGen.Remove(myItem);
                                        break;
                                    }
                                }
                                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "DeleteRow", "eliminato operatore", "", "", "");
                            }
                            else
                            {
                                sScript = "$('#lblErrorBO').text('Impossibile eliminare Operatore');$('#lblErrorBO').show();";
                                RegisterScript(sScript, this.GetType());
                                return;
                            }
                            break;
                        case "TownRow":
                            foreach (UserRole myItem in ListGen)
                            {
                                if (myItem.ID == IDRow)
                                {
                                    if (myItem.IDTipoProfilo > 0)
                                    {
                                        LoadSearchEnti(myItem, IDRow);
                                        hfIdRow.Value = IDRow.ToString();
                                        hfTypeUserDetail.Value = "Enti";
                                        ShowHide("UserDetail", true);
                                        RegisterScript("$('#lblIntestUserDetail').text('Enti');", this.GetType());
                                        break;
                                    }
                                    else
                                    {
                                        sScript = "$('#lblErrorBO').text('Selezionare il profilo!');$('#lblErrorBO').show();";
                                        RegisterScript(sScript, this.GetType());
                                        return;
                                    }
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "TownRow", "consulta enti associati", "", "", "");
                            break;
                        case "ObjectRow":
                            foreach (UserRole myItem in ListGen)
                            {
                                if (myItem.ID == IDRow)
                                {
                                    if (myItem.IDTipoProfilo > 0)
                                    {
                                        List<GenericCategory> ListTributi = new List<GenericCategory>();
                                        List<GenericCategory> ListTributiLav = new List<GenericCategory>();
                                        ListTributi = new BLL.User(myItem).LoadUserTributi(IDRow, false);
                                        if (myItem.Tributi.Count > 0)
                                        {
                                            foreach (string SingleTrib in myItem.Tributi)
                                            {
                                                foreach (GenericCategory Trib in ListTributi)
                                                {
                                                    if (SingleTrib == Trib.Codice)
                                                    {
                                                        Trib.IsActive = 1;
                                                    }
                                                    bool bTrovato = false;
                                                    foreach (GenericCategory TribLav in ListTributiLav)
                                                    {
                                                        if (TribLav.Codice == Trib.Codice)
                                                        {
                                                            bTrovato = true;
                                                        }
                                                    }
                                                    if (!bTrovato)
                                                        ListTributiLav.Add(Trib);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            ListTributiLav = ListTributi;
                                        }
                                        GrdUserDetail.DataSource = ListTributiLav;
                                        if(ListTributiLav.Count()>0)
                                        GrdUserDetail.PageSize = ListTributiLav.Count();
                                        GrdUserDetail.AllowPaging = false;
                                        GrdUserDetail.DataBind();
                                        hfIdRow.Value = IDRow.ToString();
                                        hfTypeUserDetail.Value = "Tributi";
                                        ShowHide("UserDetail", true);
                                        RegisterScript("$('#lblIntestUserDetail').text('Tributi');", this.GetType());
                                        break;
                                    }
                                    else
                                    {
                                        sScript = "$('#lblErrorBO').text('Selezionare il profilo!');$('#lblErrorBO').show();";
                                        RegisterScript(sScript, this.GetType());
                                        return;
                                    }
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "ObjectRow", "consulta tributi associati", "", "", "");
                            break;
                        case "ChangePassword":
                            foreach (UserRole myItem in ListGen)
                            {
                                if (myItem.ID == IDRow)
                                {
                                    txtNameUser.Text = myItem.NameUser;
                                    ShowHide("PWDOperatore", true);
                                    RegisterScript("$('#lblIntestPWDOperatore').text('Gestione password');", this.GetType());
                                    return;
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "ObjectRow", "consulta tributi associati", "", "", "");
                            break;
                        default:
                            ListGen.Add(new UserRole());
                            ListGen = ListGen.OrderBy(o => o.ID).ThenBy(o => o.NameUser).ToList();
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "NewOperatori", "nuovo operatore", "", "", "");
                            break;
                    }
                }
                GrdOperatori.DataSource = ListGen;
                GrdOperatori.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdOperatoriRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione cambio pagina della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUserDetailPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                List<UserRole> ListGen = new List<UserRole>();

                foreach (GridViewRow myRow in GrdOperatori.Rows)
                {
                    UserRole myItem = new UserRole();
                    myItem = fncMng.LoadUserFromGrd(myRow);
                    ListGen.Add(myItem);
                }
                foreach (UserRole myItem in ListGen)
                {
                    if (myItem.ID.ToString() == hfIdRow.Value)
                    {
                        if (myItem.IDTipoProfilo > 0)
                        {
                            LoadSearchEnti(myItem, hfIdRow.Value, e.NewPageIndex);
                            hfTypeUserDetail.Value = "Enti";
                            ShowHide("UserDetail", true);
                            RegisterScript("$('#lblIntestUserDetail').text('Enti');", this.GetType());
                            break;
                        }
                        else
                        {
                            sScript = "$('#lblErrorBO').text('Selezionare il profilo!');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                    }
                }            
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdUserDetailPageIndexChanging::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDocRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    List<GenericCategory> ListMyData = new List<GenericCategory>();

                    ListMyData = fncMng.LoadTipoIstanze(((DocToAttach)e.Row.DataItem).IDTributo, string.Empty, false);
                    DropDownList ddlRow = new DropDownList();
                    ddlRow = (DropDownList)e.Row.FindControl("ddlIstanze");
                    new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                    ddlRow.DataSource = ListMyData;
                    ddlRow.DataBind();
                    ddlRow.SelectedValue = ((DocToAttach)e.Row.DataItem).IDTipoIstanza.ToString();
                }
                if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore)
                    GrdDoc.Columns[4].Visible = false;
                else
                    GrdDoc.Columns[4].Visible = true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdDocRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDocRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                List<DocToAttach> ListGen = new List<DocToAttach>();
                foreach (GridViewRow myRow in GrdDoc.Rows)
                {
                    DocToAttach myItem = new DocToAttach();
                    myItem = fncMng.LoadDocFromGrd(myRow, ddlEnte.SelectedValue);
                    ListGen.Add(myItem);
                }
                DocToAttach mySetting = new DocToAttach { ID = IDRow };
                switch (e.CommandName)
                {
                    case "DeleteRow":
                        if (new BLL.BLLDocToAttach(mySetting).Delete())
                        {
                            foreach (DocToAttach myItem in ListGen)
                            {
                                if (myItem.ID == IDRow)
                                {
                                    ListGen.Remove(myItem);
                                    break;
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "DeleteRow", "eliminato documento", "", "", "");
                        }
                        else
                        {
                            sScript = "$('#lblErrorBO').text('Impossibile eliminare Documento');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                        break;
                    default:
                        ListGen.Add(new DocToAttach());
                        ListGen = ListGen.OrderBy(o => o.ID).ThenBy(o => o.Documento).ToList();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "NewDoc", "nuovo documento", "", "", "");

                        break;
                }
                GrdDoc.DataSource = ListGen;
                GrdDoc.DataBind();
                ShowHide("lblAnnoDest", false);
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnnoDest", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdDocRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdAliquoteRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote
                        || TypeConfig == GenericCategory.TIPO.TASI_Aliquote)
                    {
                        List<GenericCategory> ListMyData = fncMng.LoadConfigForDDL(ddlEnte.SelectedValue, 0, GenericCategory.TIPO.ICI_Caratteristica, string.Empty, string.Empty);
                        DropDownList ddlRow = (DropDownList)e.Row.FindControl("ddlTipologia");
                        new General().LoadCombo(ddlRow, ListMyData, "Codice", "Descrizione");
                        ddlRow.SelectedValue = ((GenericCategoryWithRate)e.Row.DataItem).Codice.ToString();
                        RegisterScript("ShowHideGrdConfTariffe(" + TypeConfig + ");", this.GetType());
                        if (TypeConfig == GenericCategory.TIPO.TASI_Aliquote)
                        {
                            TextBox txtVal = (TextBox)e.Row.FindControl("txtValore");
                            txtVal.Attributes.Add(" onblur", "return FieldValidatorTASI();");
                        }
                    ((TextBox)e.Row.FindControl("txtPercProprietario")).Attributes.Add("onblur", "PrecompileInquilinoVSProprietario($(this).attr('id'));");
                        ((TextBox)e.Row.FindControl("txtPercInquilino")).Attributes.Add("onblur", "PrecompileProprietarioVSInquilino($(this).attr('id'));");
                    }
                    else {
                        RegisterScript("ShowHideGrdConfTariffe(" + TypeConfig + ");", this.GetType());
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote)
                    {
                        GrdAliquote.Columns[0].HeaderText = "Tipo Utilizzo";
                        GrdAliquote.Columns[5].HeaderText = "Codice Tributo F24";
                        GrdAliquote.Columns[1].Visible = false;
                        GrdAliquote.Columns[3].Visible = false; GrdAliquote.Columns[4].Visible = false;
                    }
                    else if (TypeConfig == GenericCategory.TIPO.TASI_Aliquote)
                    {
                        GrdAliquote.Columns[0].HeaderText = "Tipo Utilizzo";
                        GrdAliquote.Columns[3].HeaderText = "% Proprietario"; GrdAliquote.Columns[4].HeaderText = "% Inquilino";
                        GrdAliquote.Columns[5].HeaderText = "Codice Tributo F24";
                        GrdAliquote.Columns[1].Visible = false;
                        GrdAliquote.Columns[3].Visible = true; GrdAliquote.Columns[4].Visible = true;
                    }else {
                        GrdAliquote.Columns[0].HeaderText = "Codice";
                        if (TypeConfig == GenericCategory.TIPO.ICI_Vincoli)
                            GrdAliquote.Columns[2].HeaderText = "% Riduzione";
                        else if (TypeConfig == GenericCategory.TIPO.ICI_Zone)
                            GrdAliquote.Columns[2].HeaderText = "Valore/MQ";
                        GrdAliquote.Columns[5].HeaderText = "Codice Banca Dati Esterna";
                        GrdAliquote.Columns[1].Visible = true;
                        GrdAliquote.Columns[3].Visible = false; GrdAliquote.Columns[4].Visible = false;
                    }
                }
                if (MySession.Current.Ente != null)
                {
                    if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "I")
                    {
                        GrdAliquote.Enabled = false;
                        RegisterScript("$('.BottoneRecycle').hide();$('.BottoneSave').hide();$('.BottoneNewGrd').hide();", this.GetType());
                    }
                }
                if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore)
                    GrdAliquote.Columns[6].Visible = false;
                else
                    GrdAliquote.Columns[6].Visible = true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdAliquoteRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdAliquoteRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow, Anno;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                int.TryParse(txtAnno.Text, out Anno);
                List<GenericCategoryWithRate> ListGen = new List<GenericCategoryWithRate>();
                LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                foreach (GridViewRow myRow in GrdAliquote.Rows)
                {
                    GenericCategoryWithRate myItem = new GenericCategoryWithRate();
                    myItem = fncMng.LoadGenericCatWithRateFromGrd(myRow, ddlEnte.SelectedValue, TypeConfig, IdTributo, Anno, false);
                    ListGen.Add(myItem);
                }
                GenericCategoryWithRate mySetting = new GenericCategoryWithRate { ID = IDRow };
                switch (e.CommandName)
                {
                    case "DeleteRow":
                        if (new BLL.BLLGenericCategoryWithRate(mySetting).Delete())
                        {
                            foreach (GenericCategoryWithRate myItem in ListGen)
                            {
                                if (myItem.ID == IDRow)
                                {
                                    ListGen.Remove(myItem);
                                    break;
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "DeleteRow", "eliminato aliquota", "", "", "");
                        }
                        else
                        {
                            sScript += "$('#lblErrorBO').text('Impossibile eliminare la voce');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                        break;
                    default:
                        ListGen.Add(new GenericCategoryWithRate() { PercProprietario = ((TypeConfig == GenericCategory.TIPO.TASI_Aliquote) ? 90 : 100), PercInquilino = ((TypeConfig == GenericCategory.TIPO.TASI_Aliquote) ? 10 : 0) });
                        ListGen = ListGen.OrderBy(o => o.ID).ThenBy(o => o.Descrizione).ToList();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Dettaglio", "NewAliquota", "nuova aliquota", "", "", "");
                        break;
                }
                GrdAliquote.DataSource = ListGen;
                GrdAliquote.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdAliquote::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdLogoRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdLogoRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdLogoRowCommand(object sender, GridViewCommandEventArgs e)
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
                        List<BaseLogo> myLogo = new BLL.EntiSistema(new EntiInLavorazione()).LoadBaseLogo(string.Empty, IDRow);
                        if (myLogo.Count > 0)
                        {
                            try
                            {
                                Response.ContentType = myLogo[0].FileMIMEType;
                                Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", myLogo[0].NameLogo));
                                Response.BinaryWrite(myLogo[0].PostedFile);
                                Response.End();
                            }
                            catch (Exception err)
                            {
                                if (err.Message != "Thread was being aborted.")
                                {
                                    Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdLogoRowCommand::errore::", err);
                                }
                            }
                        }
                        else
                        {
                            sScript = "$('#lblErrorBO').text('Logo non disponibile!');$('#lblErrorBO').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.GrdLogoRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion

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
                    ListMyData = fncMng.LoadTributi(0);
                else if (TypeCombo == "PROFILO")
                    ListMyData = fncMng.LoadProfili();
                else if (TypeCombo == "TIPOLOGIA")
                    ListMyData = fncMng.LoadConfigForDDL(ddlEnte.SelectedValue, 0, GenericCategory.TIPO.ICI_Caratteristica, string.Empty, string.Empty);

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadPageCombo::errore::", ex);
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
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                LoadType(out TypeConfig, out IdTributo, out DescrConfig);
                int Anno = 0;
                int.TryParse(txtAnno.Text, out Anno);
                try
                {
                    DropDownList ddlRowTributo = (DropDownList)sender;
                    GridViewRow myRow = (GridViewRow)ddlRowTributo.NamingContainer;

                    ListMyData = fncMng.LoadTipoIstanze(ddlRowTributo.SelectedValue, string.Empty, false);
                    DropDownList ddlRowIstanze = (DropDownList)myRow.Cells[1].FindControl("ddlIstanze");
                    new General().LoadCombo(ddlRowIstanze, ListMyData, "Codice", "Descrizione");// DataSource='<%# LoadCombo("TIPOISTANZE") %>' DataTextField="Descrizione" DataValueField="Codice" SelectedValue='<%# Eval("IDTipoIstanza") %>'
                }
                catch
                {
                    Ribes.OPENgov.WebControls.RibesGridView myGrd;
                    if (TypeConfig == GenericCategory.TIPO.Documenti)
                        myGrd = GrdDoc;
                    else if (TypeConfig == GenericCategory.TIPO.ICI_Aliquote || TypeConfig == GenericCategory.TIPO.ICI_Zone || TypeConfig == GenericCategory.TIPO.ICI_Vincoli
                        || TypeConfig == GenericCategory.TIPO.TASI_Aliquote || TypeConfig == GenericCategory.TIPO.TASI_Agevolazioni)
                        myGrd = GrdAliquote;
                    else
                        myGrd = GrdConfig;
                    if (!LoadData(ddlEnte.SelectedValue, Anno, TypeConfig, false, ddlEnte, ddlEnteDest, myGrd, GrdComuni))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Tributo"></param>
        /// <param name="Title"></param>
        private void LoadType(out string Type, out string Tributo, out string Title)
        {
            Type = (RouteData.Values["TOCONF"] != null) ? RouteData.Values["TOCONF"].ToString() : string.Empty;
            if (Type == GenericCategory.TIPO.Enti)
            {
                Title = "Enti"; Tributo = string.Empty;
            }
            else if (Type == GenericCategory.TIPO.Stradario)
            {
                Title = "Stradario"; Tributo = string.Empty;
            }
            else if (Type == GenericCategory.TIPO.Operatori)
            {
                Title = "Operatori"; Tributo = string.Empty;
            }
            else if (Type == GenericCategory.TIPO.Documenti)
            {
                Title = "Documenti"; Tributo = string.Empty;
            }
            #region TARSU
            else if (Type == GenericCategory.TIPO.TARSU_Categorie)
            {
                Title = "Categorie"; Tributo = General.TRIBUTO.TARSU;
            }
            else if (Type == GenericCategory.TIPO.TARSU_StatoOccupazione)
            {
                Title = "Stato Occupazione"; Tributo = General.TRIBUTO.TARSU;
            }
            else if (Type == GenericCategory.TIPO.TARSU_Riduzioni)
            {
                Title = "Riduzioni"; Tributo = General.TRIBUTO.TARSU;
            }
            else if (Type == GenericCategory.TIPO.TARSU_Esenzioni)
            {
                Title = "Esenzioni"; Tributo = General.TRIBUTO.TARSU;
            }
            else if (Type == GenericCategory.TIPO.TARSU_Motivazioni)
            {
                Title = "Motivazioni"; Tributo = General.TRIBUTO.TARSU;
            }
            else if (Type == GenericCategory.TIPO.TARSU_Vani)
            {
                Title = "Vani"; Tributo = General.TRIBUTO.TARSU;
            }
            #endregion
            #region ICI
            else if (Type == GenericCategory.TIPO.ICI_Categorie)
            {
                Title = "Categorie"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Utilizzo)
            {
                Title = "Utilizzo"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Caratteristica)
            {
                Title = "Utilizzo"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Motivazioni)
            {
                Title = "Motivazioni"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Possesso)
            {
                Title = "Possesso"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Zone)
            {
                Title = "Zone"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Vincoli)
            {
                Title = "Vincoli"; Tributo = General.TRIBUTO.ICI;
            }
            else if (Type == GenericCategory.TIPO.ICI_Aliquote)
            {
                Title = "Aliquote"; Tributo = General.TRIBUTO.ICI;
            }
            #endregion
            #region TASI
            else if (Type == GenericCategory.TIPO.TASI_Agevolazioni)
            {
                Title = "Agevolazioni"; Tributo = General.TRIBUTO.TASI;
            }
            else if (Type == GenericCategory.TIPO.TASI_Motivazioni)
            {
                Title = "Motivazioni"; Tributo = General.TRIBUTO.TASI;
            }
            else if (Type == GenericCategory.TIPO.TASI_Aliquote)
            {
                Title = "Aliquote"; Tributo = General.TRIBUTO.TASI;
            }
            #endregion
            #region OSAP
            else if (Type == GenericCategory.TIPO.OSAP_Richiedente)
            {
                Title = "Richiedente"; Tributo = General.TRIBUTO.OSAP;
            }
            else if (Type == GenericCategory.TIPO.OSAP_Categoria)
            {
                Title = "Categorie"; Tributo = General.TRIBUTO.OSAP;
            }
            else if (Type == GenericCategory.TIPO.OSAP_Occupazione)
            {
                Title = "Occupazione"; Tributo = General.TRIBUTO.OSAP;
            }
            else if (Type == GenericCategory.TIPO.OSAP_Agevolazioni)
            {
                Title = "Agevolazioni"; Tributo = General.TRIBUTO.OSAP;
            }
            else if (Type == GenericCategory.TIPO.OSAP_Motivazioni)
            {
                Title = "Motivazioni"; Tributo = General.TRIBUTO.OSAP;
            }
            #endregion
            #region ICP
            else if (Type == GenericCategory.TIPO.ICP_Tipologia)
            {
                Title = "Tipologia"; Tributo = General.TRIBUTO.ICP;
            }
            else if (Type == GenericCategory.TIPO.ICP_Caratteristica)
            {
                Title = "Caratteristica"; Tributo = General.TRIBUTO.ICP;
            }
            else if (Type == GenericCategory.TIPO.ICP_Motivazioni)
            {
                Title = "Motivazioni"; Tributo = General.TRIBUTO.ICP;
            }
            else
                Title = Tributo = string.Empty;
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="AnnoSetting"></param>
        /// <param name="TypeSetting"></param>
        /// <param name="bLoadEnti"></param>
        /// <param name="ddlEnte"></param>
        /// <param name="ddlEnteDest"></param>
        /// <param name="GrdSetting"></param>
        /// <param name="GrdSearchEnti"></param>
        /// <returns></returns>
        private bool LoadData(string IdEnte, int AnnoSetting, string TypeSetting, bool bLoadEnti, DropDownList ddlEnte, DropDownList ddlEnteDest, Ribes.OPENgov.WebControls.RibesGridView GrdSetting, Ribes.OPENgov.WebControls.RibesGridView GrdSearchEnti)
        {
            try
            {
                MySession.Current.ListEnti = new List<EntiInLavorazione>();
                General fncGen = new General();
                if (TypeSetting == GenericCategory.TIPO.Enti)
                {
                    List<EntiInLavorazione> ListGen = new BLL.EntiSistema(new EntiInLavorazione()).LoadEntiSistema(IdEnte, MySession.Current.UserLogged.NameUser);
                    List<EntiInLavorazione> ListEnti = new List<EntiInLavorazione>();
                    bool bSitoSingleEnte = false;
                    foreach (EntiInLavorazione myEnte in ListGen)
                    {
                        Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadData::contenuto:comunesito::" + MySession.Current.ComuneSito.ToString());

                        string enteDB = myEnte.Descrizione.ToLower().ToString();
                        string enteDDL = MySession.Current.ComuneSito.ToLower().ToString();

                        Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadData::contenuto:enteDB::" + myEnte.Descrizione.ToLower().ToString());
                        Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadData::contenuto:enteDDL::" + MySession.Current.ComuneSito.ToLower().ToString());

                        if (enteDDL.Contains(enteDB))
                        {
                            bSitoSingleEnte = true;
                            break;
                        }
                    }
                    if (bSitoSingleEnte)
                    {
                        foreach (EntiInLavorazione myEnte in ListGen)
                        {
                            if (myEnte.Descrizione.ToLower() == MySession.Current.ComuneSito.ToLower())
                            {
                                ListEnti.Add(myEnte);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ListEnti = ListGen;
                    }
                    if (ListEnti.Count == 0)
                        ListEnti.Add(new EntiInLavorazione());
                    GrdSetting.DataSource = ListEnti;
                    GrdSetting.DataBind();
                    MySession.Current.ListEnti = ListEnti;
                    GrdSearchEnti.DataSource = fncMng.LoadComuni("NESSUNO");
                    GrdSearchEnti.DataBind();
                }
                else if (TypeSetting == GenericCategory.TIPO.Operatori)
                {
                    List<UserRole> ListGen = fncMng.LoadUserRole(string.Empty, string.Empty, false,string.Empty, MySession.Current.UserLogged.NameUser);
                    GrdSetting.DataSource = ListGen;
                    GrdSetting.DataBind();
                }
                else {
                    if (bLoadEnti)
                    {
                        List<GenericCategory> ListUserEnti = new BLL.User(new UserRole() { NameUser = MySession.Current.UserLogged.NameUser, IDTipoProfilo = MySession.Current.UserLogged.IDTipoProfilo }).LoadUserEnti(string.Empty, MySession.Current.UserLogged.NameUser);
                        fncGen.LoadCombo(ddlEnte, ListUserEnti, "CODICE", "DESCRIZIONE");
                        fncGen.LoadCombo(ddlEnteDest, ListUserEnti, "CODICE", "DESCRIZIONE");
                        if (ListUserEnti.Count == 1)
                        {
                            IdEnte = ListUserEnti[0].Codice;
                            ddlEnte.SelectedValue = ListUserEnti[0].Codice;
                            ddlEnteDest.SelectedValue = ListUserEnti[0].Codice;
                            ddlEnte.Enabled = false; ddlEnteDest.Enabled = false;
                            ControlSelectedChanged((object)ddlEnte, new EventArgs());
                        }
                        MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                    }
                    if (TypeSetting == GenericCategory.TIPO.Documenti)
                    {
                        List<DocToAttach> ListGen = new BLL.BLLDocToAttach(new DocToAttach()).LoadDocToAttach(IdEnte);
                        GrdSetting.DataSource = ListGen;
                        GrdSetting.DataBind();
                    }
                    else if (TypeSetting == GenericCategory.TIPO.ICI_Aliquote || TypeConfig == GenericCategory.TIPO.ICI_Zone || TypeSetting == GenericCategory.TIPO.ICI_Vincoli
                        || TypeConfig == GenericCategory.TIPO.TASI_Aliquote || TypeConfig == GenericCategory.TIPO.TASI_Agevolazioni)
                    {
                        List<GenericCategoryWithRate> ListGen = fncMng.LoadTariffe(IdEnte, AnnoSetting, TypeSetting, string.Empty, string.Empty);
                        GrdSetting.DataSource = ListGen;
                        GrdSetting.DataBind();
                    }
                    else
                    {
                        List<GenericCategory> ListGenEnti = new List<GenericCategory>();
                        MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                        GrdSetting.DataSource = fncMng.LoadConfig(IdEnte, AnnoSetting, TypeSetting, string.Empty, string.Empty);
                        GrdSetting.DataBind();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadData::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearchComuni(int? page = 0)
        {
            try
            {
                GrdComuni.DataSource = fncMng.LoadComuni(txtEnteSearch.Text);
                if (page.HasValue)
                    GrdComuni.PageIndex = page.Value;
                GrdComuni.DataBind();
                ShowHide("SearchEnti", true);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadSearchComuni::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        /// <param name="IdRow"></param>
        /// <param name="page"></param>
        private void LoadSearchEnti(UserRole myItem,string IdRow, int? page = 0)
        {
            try
            {
                List<GenericCategory> ListEnti = new BLL.User(myItem).LoadUserEnti(IdRow, string.Empty);
                Log.Debug("ho trovato " + ListEnti.Count.ToString() + " enti");
                List<GenericCategory> ListEntiLav = new List<GenericCategory>();
                bool bSitoSingleEnte = false;
                foreach (GenericCategory myEnte in ListEnti)
                {
                    if (myEnte.Descrizione.ToLower() == MySession.Current.ComuneSito.ToLower())
                    {
                        bSitoSingleEnte = true;
                        break;
                    }
                }
                if (bSitoSingleEnte)
                {
                    foreach (GenericCategory myEnte in ListEnti)
                    {
                        if (myEnte.Descrizione.ToLower() == MySession.Current.ComuneSito.ToLower())
                        {
                            ListEntiLav.Add(myEnte);
                        }
                    }
                }
                else {
                    if (myItem.Enti.Count > 0)
                    {
                        foreach (string SingleEnte in myItem.Enti)
                        {
                            foreach (GenericCategory Ente in ListEnti)
                            {
                                if (SingleEnte == Ente.Codice)
                                {
                                    Ente.IsActive = 1;
                                }
                                bool bTrovato = false;
                                foreach(GenericCategory EnteLav in ListEntiLav)
                                {
                                    if (EnteLav.Codice == Ente.Codice)
                                    {
                                        bTrovato = true;
                                    }
                                }
                                if (!bTrovato)
                                ListEntiLav.Add(Ente);
                            }
                        }
                    }
                    else
                    {
                        ListEntiLav = ListEnti;
                    }
                }
                Log.Debug("ho caricato " + ListEntiLav.Count.ToString() + " enti");

                GrdUserDetail.DataSource = ListEntiLav;
                if(ListEntiLav.Count()>0)
                GrdUserDetail.PageSize = ListEntiLav.Count();
                GrdUserDetail.AllowPaging = false;
                if (page.HasValue)
                    GrdUserDetail.PageIndex = page.Value;
                GrdUserDetail.DataBind();
                ShowHide("UserDetail", true);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadSearchEnti::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione per la gestione degli oggetti da vedere/nascondere in base all'operazione
        /// </summary>
        private void ManageShowHide()
        {
            try
            {
                sScript = "document.getElementById('TitlePage').innerHTML='Configurazioni';";
                sScript += "document.getElementById('lblTitle').innerText='" + DescrConfig + "';";
                sScript += "document.getElementById('TitlePage').setAttribute('href', 'BO_SettingsBase.aspx');";
                if (TypeConfig == GenericCategory.TIPO.TARSU_StatoOccupazione)
                {
                    ShowHide("HelpUtilizzo", true);
                }
                else {
                    ShowHide("HelpUtilizzo", false);
                }
                RegisterScript(sScript, this.GetType());

                if (TypeConfig == GenericCategory.TIPO.Documenti)
                {
                    ShowHide("ConfigVSEnte", true);
                    ShowHide("DocAllegati", true);
                    ShowHide("GeneralCategory", false);
                    ShowHide("Enti", false);
                    ShowHide("Operatori", false);
                    ShowHide("Vincoli", false);
                }
                else if (TypeConfig == GenericCategory.TIPO.ICI_Vincoli || TypeConfig == GenericCategory.TIPO.ICI_Zone)
                {
                    ShowHide("ConfigVSEnte", true);
                    ShowHide("Vincoli", true);
                    ShowHide("DocAllegati", false);
                    ShowHide("GeneralCategory", false);
                    ShowHide("Enti", false);
                    ShowHide("Operatori", false);
                }
                else if (TypeConfig == GenericCategory.TIPO.Enti)
                {
                    ShowHide("Enti", true);
                    ShowHide("ConfigVSEnte", false);
                    ShowHide("Operatori", false);
                }
                else if (TypeConfig == GenericCategory.TIPO.Operatori)
                {
                    ShowHide("Operatori", true);
                    ShowHide("ConfigVSEnte", false);
                    ShowHide("DocAllegati", false);
                    ShowHide("Vincoli", false);
                    ShowHide("GeneralCategory", false);
                    ShowHide("Enti", false);
                }
                else
                {
                    ShowHide("ConfigVSEnte", true);
                    ShowHide("GeneralCategory", true);
                    ShowHide("DocAllegati", false);
                    ShowHide("Vincoli", false);
                    ShowHide("Enti", false);
                    ShowHide("Operatori", false);
                }

                if (TypeConfig == GenericCategory.TIPO.Enti)
                        {
                            ShowHide(BLL.GestForm.PlaceHolderName.Title + "_btnUpload", true);
                       }
                        else if (TypeConfig != GenericCategory.TIPO.Documenti && TypeConfig != GenericCategory.TIPO.TARSU_Motivazioni && TypeConfig != GenericCategory.TIPO.TARSU_Vani && TypeConfig != GenericCategory.TIPO.TARSU_StatoOccupazione
                    && TypeConfig != GenericCategory.TIPO.ICI_Motivazioni && TypeConfig != GenericCategory.TIPO.ICI_Categorie && TypeConfig != GenericCategory.TIPO.ICI_Caratteristica && TypeConfig != GenericCategory.TIPO.ICI_Possesso
                    && TypeConfig != GenericCategory.TIPO.TASI_Motivazioni
                    && TypeConfig != GenericCategory.TIPO.OSAP_Richiedente && TypeConfig != GenericCategory.TIPO.OSAP_Motivazioni && TypeConfig != GenericCategory.TIPO.OSAP_Categoria && TypeConfig != GenericCategory.TIPO.OSAP_Occupazione
                    && TypeConfig != GenericCategory.TIPO.ICP_Motivazioni)
                {
                    if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E")
                    {
                        if (TypeConfig != GenericCategory.TIPO.Stradario && TypeConfig != GenericCategory.TIPO.Enti && TypeConfig != GenericCategory.TIPO.Operatori)
                        {
                            ShowHide(BLL.GestForm.PlaceHolderName.Title + "_btnCopyTo", true);
                            ShowHide("lblAnno", true);
                            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnno", true);
                        }
                    }
                    else
                    {
                        if (TypeConfig != GenericCategory.TIPO.Stradario)
                        {
                            ShowHide("lblAnno", true);
                            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnno", true);
                        }
                    }
                }
                else
                {
                    if ((MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E") ||
                                (TypeConfig == GenericCategory.TIPO.ICI_Motivazioni || TypeConfig == GenericCategory.TIPO.ICI_Zone || TypeConfig == GenericCategory.TIPO.ICI_Vincoli
                                    || TypeConfig == GenericCategory.TIPO.TASI_Motivazioni
                                    || TypeConfig == GenericCategory.TIPO.TARSU_Motivazioni
                                    || TypeConfig == GenericCategory.TIPO.OSAP_Motivazioni
                                    || TypeConfig == GenericCategory.TIPO.ICP_Motivazioni
                                )
                            )
                    {
                        ShowHide(BLL.GestForm.PlaceHolderName.Title + "_btnCopyTo", true);
                    }
                }
                ShowHideRibalta("divDest", BLL.GestForm.PlaceHolderName.Body + "_hfIsCopyTo");
                if (TypeConfig == GenericCategory.TIPO.Stradario || TypeConfig == GenericCategory.TIPO.Documenti
                    || TypeConfig == GenericCategory.TIPO.TARSU_Motivazioni || TypeConfig == GenericCategory.TIPO.TARSU_Vani
                    || TypeConfig == GenericCategory.TIPO.ICI_Motivazioni || TypeConfig == GenericCategory.TIPO.ICI_Categorie || TypeConfig == GenericCategory.TIPO.ICI_Caratteristica
                    || TypeConfig == GenericCategory.TIPO.TASI_Motivazioni
                    || TypeConfig == GenericCategory.TIPO.OSAP_Richiedente || TypeConfig == GenericCategory.TIPO.OSAP_Motivazioni
                    || TypeConfig == GenericCategory.TIPO.ICP_Motivazioni)
                {
                    ShowHide("lblAnnoDest", false);
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_txtAnnoDest", false);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.ManageShowHide::errore::", ex);
            }
        }
    }
}