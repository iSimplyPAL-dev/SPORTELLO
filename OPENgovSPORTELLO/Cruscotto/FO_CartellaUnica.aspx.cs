using log4net;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OPENgovSPORTELLO.Cruscotto
{
    /// <summary>
    /// Mediante il bottone [CARTELLA UNICA], il contribuente avrà la possibilità di generare il report contenente tutte le informazioni relative alla sua posizione contributiva ai fini dei tributi.
    /// La funzione deve estrarre la cartella già descritta per il back office, senza criteri di selezione in quanto estrae i dati del contribuente registrato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class FO_CartellaUnica : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_CartellaUnica));
        protected FunctionGrd FncGrd = new FunctionGrd();

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
                Log.Debug("OPENgovSPORTELLO.Cruscotto.CartellaUnica.Page_Init::errore::", ex);
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
                if (!Page.IsPostBack)
                {
                    new General().ClearSession();
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());

                    List<RiepilogoUI> ListDich = new List<RiepilogoUI>();
                    List<RiepilogoUI> ListUI = new List<RiepilogoUI>();
                    List<RiepilogoDovuto> ListDovuto = new List<RiepilogoDovuto>();
                    foreach (GenericCategory myTrib in MySession.Current.Ente.ListTributi)
                    {
                        if (myTrib.IDTributo == General.TRIBUTO.ICI && myTrib.IsActive == 1)
                        {
                            if (new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadICIRiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListDich, out ListUI, out ListUI, out ListDovuto))
                            {
                                if (ListDich.Count > 0)
                                {
                                    ShowHide("div8852", true);
                                    GrdDich8852.DataSource = ListDich;
                                    GrdDich8852.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich8852", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich8852", false);
                                }
                                if (ListDovuto.Count > 0)
                                {
                                    ShowHide("div8852", true);
                                    GrdPag8852.DataSource = ListDovuto;
                                    GrdPag8852.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPag8852", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPag8852", false);
                                }
                            }
                        }
                        if (myTrib.IDTributo == General.TRIBUTO.TASI && myTrib.IsActive == 1)
                        {
                            ListDich = new List<RiepilogoUI>();
                            ListDovuto = new List<RiepilogoDovuto>();
                            if (new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadTASIRiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListDich, out ListDovuto))
                            {
                                if (ListDich.Count > 0)
                                {
                                    ShowHide("divTASI", true);
                                    GrdDichTASI.DataSource = ListDich;
                                    GrdDichTASI.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDichTASI", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDichTASI", false);
                                }
                                if (ListDovuto.Count > 0)
                                {
                                    ShowHide("divTASI", true);
                                    GrdPagTASI.DataSource = ListDovuto;
                                    GrdPagTASI.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPagTASI", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPagTASI", false);
                                }
                            }
                        }
                        if (myTrib.IDTributo == General.TRIBUTO.TARSU && myTrib.IsActive == 1)
                        {
                            ListDich = new List<RiepilogoUI>();
                            ListDovuto = new List<RiepilogoDovuto>();
                            List<CategorieTARSU> ListCat = new List<CategorieTARSU>();
                            List<RidEseTARSU> ListUIRidEse = new List<RidEseTARSU>();
                            if (new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadTARSURiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListUI, out ListDich, out ListCat, out ListUIRidEse, out ListUIRidEse, out ListDovuto))
                            {
                                if (ListDich.Count > 0)
                                {
                                    ShowHide("div0434", true);
                                    GrdDich0434.DataSource = ListDich;
                                    GrdDich0434.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich0434", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich0434", false);
                                }
                                if (ListDovuto.Count > 0)
                                {
                                    ShowHide("div0434", true);
                                    GrdPag0434.DataSource = ListDovuto;
                                    GrdPag0434.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPag0434", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPag0434", false);
                                }
                            }
                        }
                        if (myTrib.IDTributo == General.TRIBUTO.OSAP && myTrib.IsActive == 1)
                        {
                            ListDich = new List<RiepilogoUI>();
                            ListDovuto = new List<RiepilogoDovuto>();
                            if (new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadOSAPRiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListDich, out ListUI, out ListDovuto))
                            {
                                if (ListUI.Count > 0)
                                {
                                    ShowHide("div0453", true);
                                    GrdDich0453.DataSource = ListUI;
                                    GrdDich0453.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich0453", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich0453", false);
                                }
                                if (ListDovuto.Count > 0)
                                {
                                    ShowHide("div0453", true);
                                    GrdPag0453.DataSource = ListDovuto;
                                    GrdPag0453.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPag0453", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdPag0453", false);
                                }
                            }
                        }
                        if (myTrib.IDTributo == General.TRIBUTO.PROVVEDIMENTI && myTrib.IsActive == 1)
                        {
                            List<SPC_Provvedimento> ListProvvedimento = new List<SPC_Provvedimento>();
                            if (new BLL.PROVVEDIMENTI().LoadListProvvedimenti(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListProvvedimento))
                            {
                                if (ListProvvedimento.Count > 0)
                                {
                                    ShowHide("div9999", true);
                                    GrdDich9999.DataSource = ListProvvedimento;
                                    GrdDich9999.DataBind();
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich9999", true);
                                }
                                else
                                {
                                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich9999", false);
                                }
                            }
                        }
                    }

                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Cartella Unica", "Page_Load", "ingresso pagina", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.CartellaUnica.Page_Load::errore::", ex);
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
                sScript += "window.open('" + MySettingsReport.GetConfig("PathServer") + MySettingsReport.EstensioneReport.PDF + MySettingsReport.GetConfig("CartellaUnica") + "." + MySettingsReport.EstensioneReport.PDF.ToLower() + "?j_username=" + MySettingsReport.GetConfig("User") + "&j_password=" + MySettingsReport.GetConfig("Pwd") + "&varIDENTE=" + MySession.Current.Ente.IDEnte + "&varIDCONTRIBUENTE=" + MySession.Current.UserLogged.IDContribToWork + "');";
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
            try
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Report", "Cartella Unica", "Back", "uscita pagina", "", "", "");
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFO_ReportGen, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.CartellaUnica.Back::errore::", ex);
                LoadException(ex);
            }
        }
    }
}