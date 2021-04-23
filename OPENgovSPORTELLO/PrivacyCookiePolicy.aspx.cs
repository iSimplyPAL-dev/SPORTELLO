using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PrivacyCookiePolicy :GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PrivacyCookiePolicy));

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
                hfFrom.Value = MySession.Current.Scope;
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += new BLL.GestForm().GetLabel("PrivacyCookiePolicy", "");
                if(hfFrom.Value=="FO")
                sScript += "$('.Home').addClass('HomeFO');";
                else
                    sScript += "$('.Home').addClass('HomeBO');";
                sScript += "$('.NewLogin').hide();";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.PrivacyCookiePolicy.Page_Init::errore::", ex);
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
    }
}