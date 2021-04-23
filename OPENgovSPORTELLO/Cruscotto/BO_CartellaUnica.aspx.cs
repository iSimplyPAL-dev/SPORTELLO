using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Cruscotto
{
    /// <summary>
    /// La cartella Unica è il documento che riepiloga la situazione tributaria del contribuente.
    /// Può essere richiamata e stampata sia dal contribuente, che dal back office.
    /// I criteri di estrazione per il solo back office, sono:
    /// <list type="bullet">
    /// <item>Ente</item>
    /// <item>Codice Fiscale</item>
    /// <item>Nominativo(in alternativa al codice fiscale)</item>
    /// </list>
    /// E’ obbligatoria la selezione di Ente e contribuente attraverso  cod.fiscale e/o nominativo.
    /// Per ogni tributo saranno dettagliati tutti gli articoli attivi nel periodo selezionato, i pagamenti  e la situazione riscontrata dal CUC ai fini IMU.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_CartellaUnica : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_CartellaUnica));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private General fncGen = new General();

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
                ShowHide("div8852", false);
                ShowHide("divTASI", false);
                ShowHide("div0434", false);
                ShowHide("div0453", false);
                ShowHide("div9999", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.Page_Init::errore::", ex);
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
                if (Page.IsPostBack == false)
                {
                    List<GenericCategory> ListUserEnti = new BLL.User(new UserRole() { NameUser = MySession.Current.UserLogged.NameUser, IDTipoProfilo = MySession.Current.UserLogged.IDTipoProfilo }).LoadUserEnti(string.Empty, MySession.Current.UserLogged.NameUser);
                    fncGen.LoadCombo(ddlEnte, ListUserEnti, "CODICE", "DESCRIZIONE");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.Page_Load::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                if (ddlEnte.SelectedValue == string.Empty || (txtNominativo.Text == string.Empty && txtCFPIVA.Text == string.Empty))
                {
                    sScript = "$('#OnlyNumber_error').text('Inserire tutti i parametri di ricerca');$('#OnlyNumber_error').show();";
                }
                else {
                    UserRole myUtente = new BLL.User(new UserRole()).LoadUser(txtNominativo.Text, txtCFPIVA.Text, String.Empty, -1, ddlEnte.SelectedValue);
                    if (myUtente != null)
                    {
                        AnagInterface.DettaglioAnagrafica myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(myUtente.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                        RegisterScript(new BLL.Profilo().LoadJumbotron(myAnag, myUtente.IDContribToWork), this.GetType());
                        EntiInLavorazione myEnte= new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                        List<GenericCategory> ListTributi = new BLL.EntiSistema(new EntiInLavorazione()).LoadEntiTributi(ddlEnte.SelectedValue, -1, 0);
                        foreach (GenericCategory myTrib in ListTributi)
                        {
                            if (myTrib.IDTributo == General.TRIBUTO.ICI && myTrib.IsActive == 1)
                                Load8852(myUtente, ddlEnte.SelectedValue);
                            if (myTrib.IDTributo == General.TRIBUTO.TASI && myTrib.IsActive == 1)
                                LoadTASI(myUtente, ddlEnte.SelectedValue);
                            if (myTrib.IDTributo == General.TRIBUTO.TARSU && myTrib.IsActive == 1)
                                Load0434(myUtente, ddlEnte.SelectedValue);
                            if (myTrib.IDTributo == General.TRIBUTO.OSAP && myTrib.IsActive == 1)
                                LoadOSAP(myUtente, ddlEnte.SelectedValue);
                            if (myTrib.IDTributo == General.TRIBUTO.PROVVEDIMENTI && myTrib.IsActive == 1)
                                LoadProvvedimenti(myUtente, ddlEnte.SelectedValue);
                        }
                    }
                    else
                    {
                        sScript += "$('#OnlyNumber_error').text('Utente non presente');$('#OnlyNumber_error').show();";
                    }
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "CartellaUnica", "Search", "Ricerca", "", "", "");
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.Search::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void stampaReport(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                UserRole myUtente = new BLL.User(new UserRole()).LoadUser(txtNominativo.Text, txtCFPIVA.Text, String.Empty, -1, ddlEnte.SelectedValue);
                if (myUtente != null)
                {
                    sScript += "window.open('" + MySettingsReport.GetConfig("PathServer") + MySettingsReport.EstensioneReport.PDF + MySettingsReport.GetConfig("CartellaUnica") + "." + MySettingsReport.EstensioneReport.PDF.ToLower();
                    sScript += "?j_username=" + MySettingsReport.GetConfig("User") + "&j_password=" + MySettingsReport.GetConfig("Pwd");
                    sScript += "&varIDENTE=" + ddlEnte.SelectedValue;
                    sScript += "&varIDCONTRIBUENTE=" + new BLL.User(new UserRole()).LoadUser(txtNominativo.Text, txtCFPIVA.Text, String.Empty, -1, ddlEnte.SelectedValue).IDContribToWork;
                    sScript += "');";
                }
                else
                {
                    sScript += "$('#OnlyNumber_error').text('Utente non presente');$('#OnlyNumber_error').show();";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.CartellaUnica.stampaReport::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "CartellaUnica", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUtente"></param>
        /// <param name="IDEnte"></param>
        private void Load8852(UserRole myUtente, string IDEnte)
        {
            try
            {
                List<object> ListDich = new List<object>();
                List<object> ListDovuto = new List<object>();
                List<object> ListPag = new List<object>();
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.ICI, "ReportDichiarato", new ReportDich8852(), IDEnte, myUtente.IDContribToWork, out ListDich);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.ICI, "ReportDovuto", new ReportDovuto8852(), IDEnte, myUtente.IDContribToWork, out ListDovuto);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.ICI, "ReportPagato", new ReportPagato8852(), IDEnte, myUtente.IDContribToWork, out ListPag);
                if (ListDich.Count == 0 && ListDovuto.Count == 0 && ListPag.Count == 0)
                    RegisterScript("$('#div8852').hide();", this.GetType());
                else
                {
                    GrdDich8852.DataSource = ListDich;
                    GrdDich8852.DataBind();
                    GrdDovuto8852.DataSource = ListDovuto;
                    GrdDovuto8852.DataBind();
                    GrdPagato8852.DataSource = ListPag;
                    GrdPagato8852.DataBind();
                    RegisterScript("$('#div8852').show();", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.Load8852::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUtente"></param>
        /// <param name="IDEnte"></param>
        private void LoadTASI(UserRole myUtente, string IDEnte)
        {
            try
            {
                List<object> ListDich = new List<object>();
                List<object> ListDovuto = new List<object>();
                List<object> ListPag = new List<object>();
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.TASI, "ReportDichiarato", new ReportDich8852(), IDEnte, myUtente.IDContribToWork, out ListDich);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.TASI, "ReportDovuto", new ReportDovuto8852(), IDEnte, myUtente.IDContribToWork, out ListDovuto);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.TASI, "ReportPagato", new ReportPagato8852(), IDEnte, myUtente.IDContribToWork, out ListPag);
                if (ListDich.Count == 0 && ListDovuto.Count == 0 && ListPag.Count == 0)
                    RegisterScript("$('#divTASI').hide();", this.GetType());
                else
                {
                    GrdDichTASI.DataSource = ListDich;
                    GrdDichTASI.DataBind();
                    GrdDovutoTASI.DataSource = ListDovuto;
                    GrdDovutoTASI.DataBind();
                    GrdPagatoTASI.DataSource = ListPag;
                    GrdPagatoTASI.DataBind();
                    RegisterScript("$('#divTASI').show();", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.LoadTASI::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUtente"></param>
        /// <param name="IDEnte"></param>
        private void Load0434(UserRole myUtente, string IDEnte)
        {
            try
            {
                List<object> ListDich = new List<object>();
                List<object> ListDovuto = new List<object>();
                List<object> ListPag = new List<object>();
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.TARSU, "ReportDichiarato", new ReportDich0434(), IDEnte, myUtente.IDContribToWork, out ListDich);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.TARSU, "ReportDovuto", new ReportDovuto0434(), IDEnte, myUtente.IDContribToWork, out ListDovuto);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.TARSU, "ReportPagato", new ReportPagato0434(), IDEnte, myUtente.IDContribToWork, out ListPag);
                if (ListDich.Count == 0 && ListDovuto.Count == 0 && ListPag.Count == 0)
                    RegisterScript("$('#div0434').hide();", this.GetType());
                else
                {
                    GrdDich0434.DataSource = ListDich;
                    GrdDich0434.DataBind();
                    GrdDovuto0434.DataSource = ListDovuto;
                    GrdDovuto0434.DataBind();
                    GrdPagato0434.DataSource = ListPag;
                    GrdPagato0434.DataBind();
                    RegisterScript("$('#div0434').show();", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.Load0434::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUtente"></param>
        /// <param name="IDEnte"></param>
        private void LoadOSAP(UserRole myUtente, string IDEnte)
        {
            try
            {
                List<object> ListDich = new List<object>();
                List<object> ListDovuto = new List<object>();
                List<object> ListPag = new List<object>();
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.OSAP, "ReportDichiarato", new ReportDich0453(), IDEnte, myUtente.IDContribToWork, out ListDich);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.OSAP, "ReportDovuto", new ReportDovuto0434(), IDEnte, myUtente.IDContribToWork, out ListDovuto);
                new BLL.Analisi().LoadCartellaUnica(General.TRIBUTO.OSAP, "ReportPagato", new ReportPagato0434(), IDEnte, myUtente.IDContribToWork, out ListPag);
                if (ListDich.Count == 0 && ListDovuto.Count == 0 && ListPag.Count == 0)
                    RegisterScript("$('#div0453').hide();", this.GetType());
                else
                {
                    GrdDich0453.DataSource = ListDich;
                    GrdDich0453.DataBind();
                    GrdDovuto0453.DataSource = ListDovuto;
                    GrdDovuto0453.DataBind();
                    GrdPagato0453.DataSource = ListPag;
                    GrdPagato0453.DataBind();
                    RegisterScript("$('#div0453').show();", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.LoadOSAP::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUtente"></param>
        /// <param name="IDEnte"></param>
        private void LoadProvvedimenti(UserRole myUtente, string IDEnte)
        {
            try
            {
                List<SPC_Provvedimento> ListProvvedimento = new List<SPC_Provvedimento>();
                if (new BLL.PROVVEDIMENTI().LoadListProvvedimenti(IDEnte, myUtente.IDContribToWork, out ListProvvedimento))
                {
                    if (ListProvvedimento.Count > 0)
                    {
                        GrdDich9999.DataSource = ListProvvedimento;
                        GrdDich9999.DataBind();
                        ShowHide("div9999", true);
                    }
                    else
                    {
                        ShowHide("div9999", false);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica.LoadProvvedimenti::errore", ex);
                LoadException(ex);
            }
        }
    }
}