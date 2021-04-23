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
    /// Il raffronto dovuto/versato possibile solo sulle posizioni dove il contribuente ha confermato/rettificato i dati ed ha eseguito il calcolo con lo sportello. Porta in evidenza le discordanze tra il dovuto calcolato e l’effettivo versato che risulta a sistema.
    /// Il raffronto può essere eseguito su più anni contemporaneamente dove il risultato è ordinato per contribuente/anno.
    /// Attenzione, i versamenti estratti devono essere solo quelli relativi ai contribuenti per i quali è presente il dovuto su sportello.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_DovutoVersato : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_DovutoVersato));        
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Raffronto Dovuto Versato", "Page_Load", "ingresso pagina", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_DovutoVersato.Page_Load::errore::", ex);
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
                     List<DovutoVSVersato> ListEventi = new BLL.Analisi().LoadRaffrontoDovutoVersato(ddlEnte.SelectedValue,((txtAnno.Text==string.Empty)?-1:int.Parse( txtAnno.Text)));
                    GrdResult.DataSource = ListEventi;
                    GrdResult.DataBind();
                     new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Raffronto Dovuto Versato", "Search", "Ricerca", "", "", "");
                            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_DovutoVersato.Search::errore", ex);
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
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Raffronto Dovuto Versato", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }
    }
}