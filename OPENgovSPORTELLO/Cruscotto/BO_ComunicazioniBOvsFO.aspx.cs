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
    /// La piattaforma di back office gestirà tutti i log relativi alla interazioni.
    /// Saranno tracciate tutte le comunicazioni fatte dagli utenti presso il back-office e tutte le comunicazioni dal back-office verso gli utenti.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_ComunicazioniBOvsFO : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_ComunicazioniBOvsFO));
        protected FunctionGrd FncGrd = new FunctionGrd();
        General fncGen = new General();

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

                    List<GenericCategory> ListTipoEventi = new BLL.Settings().LoadTipoIstanze(string.Empty, string.Empty, true);
                    fncGen.LoadCombo(ddlTipoIstanze, ListTipoEventi, "CODICE", "DESCRIZIONE");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_ComunicazioniBOvsFO.Page_Load::errore::", ex);
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
                DateTime dataDal, dataAl;
                dataDal = dataAl = DateTime.MaxValue;
                if (txtDal.Text != string.Empty)
                {
                    dataDal = DateTime.Parse(txtDal.Text);
                }
                if (txtAl.Text != string.Empty)
                {
                    dataAl = DateTime.Parse(txtAl.Text);
                }
                MySession.Current.SortDirection = SortDirection.Descending;
                List<ComunicazioniBOvsFO> ListMyData = new BLL.Analisi().LoadComunicazioniBOvsFO(ddlEnte.SelectedValue, dataDal, dataAl, int.Parse(ddlTipoIstanze.SelectedValue), txtOperatore.Text, txtCFPIVA.Text);
                GrdComunicazioni.DataSource = ListMyData;
                GrdComunicazioni.DataBind();
                MySession.Current.GestComunicazioniBOvsFO = ListMyData;
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "ComunicazioniBOvsFO", "Search", "Ricerca", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_ComunicazioniBOvsFO.Search::errore", ex);
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
            MySession.Current.GestComunicazioniBOvsFO = new List<ComunicazioniBOvsFO>();
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Analisi Eventi", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }

        /// <summary>
        /// Funzione di gestione dell'ordinamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdComunicazioniSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<ComunicazioniBOvsFO> ListSorted = MySession.Current.GestComunicazioniBOvsFO;
                switch (e.SortExpression)
                {
                    case "Nominativo":
                        ListSorted = ListSorted.OrderBy(order => order.Nominativo).ToList();
                        break;
                    case "CodFiscalePIVA":
                        ListSorted = ListSorted.OrderBy(order => order.CodFiscalePIVA).ToList();
                        break;
                    case "DescrIstanza":
                        ListSorted = ListSorted.OrderBy(order => order.DescrIstanza).ToList();
                        break;
                    case "Data":
                        ListSorted = ListSorted.OrderBy(order => order.Data).ToList();
                        break;
                    case "Provenienza":
                        ListSorted = ListSorted.OrderBy(order => order.Provenienza).ToList();
                        break;
                    case "Stato":
                        ListSorted = ListSorted.OrderBy(order => order.Stato).ToList();
                        break;
                    case "Operatore":
                        ListSorted = ListSorted.OrderBy(order => order.Operatore).ToList();
                        break;
                }
                if (MySession.Current.SortDirection == SortDirection.Descending)
                    MySession.Current.SortDirection = SortDirection.Ascending;
                else
                    MySession.Current.SortDirection = SortDirection.Descending;

                if (MySession.Current.SortDirection == SortDirection.Descending)
                    ListSorted.Reverse();
                GrdComunicazioni.DataSource = ListSorted;
                GrdComunicazioni.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_ComunicazioniBOvsFO.GrdResultRowSorting::errore::", ex);
                LoadException(ex);
            }
        }
    }
}