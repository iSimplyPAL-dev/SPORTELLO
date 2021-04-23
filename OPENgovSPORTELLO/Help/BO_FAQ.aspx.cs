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
    public partial class BO_FAQ : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_FAQ));
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "F.A.Q.", "BO", "Back", "uscita pagina", "", "", string.Empty);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultBO, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BO_FAQ.Back::errore::", ex);
                LoadException(ex);
            }
        }
    }
}