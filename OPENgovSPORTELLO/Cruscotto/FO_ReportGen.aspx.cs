using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace OPENgovSPORTELLO.Cruscotto
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FO_ReportGen : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_ReportGen));

        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Report", "", "Page_Load", "ingresso pagina", "", "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("FO_ReportGen::Page_Load::errore::", ex);
            }
            finally
            {
                RegisterScript("$('#FAQ').addClass('HelpFOReport');", this.GetType());
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Report", "", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
        }

    }
}