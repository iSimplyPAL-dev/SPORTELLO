using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Cruscotto.Analisi
{
    /// <summary>
    /// L’obiettivo è analizzare in tempi con cui vengono fatti i pagamenti rispetto alle scadenze.
    /// Per il tributo IMU, le scadenze sono quelle ministeriali. Per gli altri tributi(se non abbiamo disponibilità data scadenza da verticale, devono essere richieste)
    /// Il risultato deve essere per Periodo
    /// Il periodo corrisponde a:
    /// <list type="bullet">
    /// <item>entro 30 GG prima</item>
    /// <item>tra 30 e 15 GG prima</item>
    /// <item>tra 15 GG e scadenza</item>
    /// <item>entro 15 GG post scadenza</item>
    /// <item>entro 30 GG post scadenza</item>
    /// <item>entro 60 GG post scadenza</item>
    /// <item>entro 90 GG post scadenza</item>
    /// <item>entro 180 GG post scadenza</item>
    /// <item>entro 1 anno post scadenza</item>
    /// <item>oltre 1 anno</item>
    /// </list>
    /// Nella griglia andranno conteggiati i contribuenti in base alle data di pagamento che si trova in uno delle ipotesi di periodo sopra elencato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_TempiPagamento : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_TempiPagamento));
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
                    fncGen.LoadCombo(ddlTributo, new BLL.Settings().LoadTributi(1), "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlDataEmissione, new BLL.Settings().LoadDataEmissione(ddlTributo.SelectedValue, ddlEnte.SelectedValue), "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlScadenza, new BLL.Settings().LoadScadenzeTempiPagamento(ddlEnte.SelectedValue, DateTime.MaxValue, ddlTributo.SelectedValue), "CODICE", "DESCRIZIONE");
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Tempi di pagamento", "Page_Load", "ingresso pagina", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento.Page_Load::errore::", ex);
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
                List<TempiPagamento> ListResult = new BLL.Analisi().LoadTempiPagamento(ddlEnte.SelectedValue, DateTime.Parse(ddlDataEmissione.SelectedValue), ddlTributo.SelectedValue, ddlScadenza.SelectedValue);
                GrdResult.DataSource = ListResult;
                GrdResult.DataBind();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Tempi di pagamento", "Search", "Ricerca", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento.Search::errore", ex);
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
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Tempi di pagamento", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TempiMedi(object sender, EventArgs e)

        {
            new BLLImport().ReadTempiPagamento(ddlEnte.SelectedValue, ddlTributo.SelectedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                fncGen.LoadCombo(ddlDataEmissione, new BLL.Settings().LoadDataEmissione(ddlTributo.SelectedValue, ddlEnte.SelectedValue), "CODICE", "DESCRIZIONE");
                DateTime myDataEmissione = DateTime.Parse(ddlDataEmissione.SelectedValue);
                fncGen.LoadCombo(ddlScadenza, new BLL.Settings().LoadScadenzeTempiPagamento(ddlEnte.SelectedValue, DateTime.Parse(ddlDataEmissione.SelectedValue), ddlTributo.SelectedValue), "CODICE", "DESCRIZIONE");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento.ddlSelectedIndexChanged::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSelectedIndexChanged01(object sender, EventArgs e)
        {
            try
            {
                fncGen.LoadCombo(ddlDataEmissione, new BLL.Settings().LoadDataEmissione(ddlTributo.SelectedValue, ddlEnte.SelectedValue), "CODICE", "DESCRIZIONE");
                fncGen.LoadCombo(ddlScadenza, new BLL.Settings().LoadScadenzeTempiPagamento(ddlEnte.SelectedValue, DateTime.Parse(ddlDataEmissione.SelectedValue), ddlTributo.SelectedValue), "CODICE", "DESCRIZIONE");
                List<TempiPagamento> ListResult = new List<TempiPagamento>();
                GrdResult.DataSource = ListResult;
                GrdResult.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento.ddlSelectedIndexChanged01::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSelectedIndexChanged02(object sender, EventArgs e)
        {
            try
            {
                fncGen.LoadCombo(ddlScadenza, new BLL.Settings().LoadScadenzeTempiPagamento(ddlEnte.SelectedValue, DateTime.Parse(ddlDataEmissione.SelectedValue), ddlTributo.SelectedValue), "CODICE", "DESCRIZIONE");
                List<TempiPagamento> ListResult = new List<TempiPagamento>();
                GrdResult.DataSource = ListResult;
                GrdResult.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento.ddlSelectedIndexChanged02::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSelectedIndexChanged03(object sender, EventArgs e)
        {
            try
            {
                List<TempiPagamento> ListResult = new List<TempiPagamento>();
                GrdResult.DataSource = ListResult;
                GrdResult.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento.ddlSelectedIndexChanged03::errore::", ex);
                LoadException(ex);
            }
        }
    }
}
