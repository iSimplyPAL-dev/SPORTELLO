using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace OPENgovSPORTELLO.Help
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FO_FAQ : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_FAQ));

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
                new General().ClearSession();
                string sScript = new BLL.GestForm().GetLabel("FO_FAQ", "");
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_FAQ.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "F.A.Q.", "FO", "Back", "uscita pagina", "", "", string.Empty);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_FAQ.Back::errore::", ex);
                LoadException(ex);
            }
        }
    }
}