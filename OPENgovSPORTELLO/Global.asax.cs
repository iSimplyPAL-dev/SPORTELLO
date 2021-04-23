using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using log4net;
using log4net.Config;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// 
    /// </summary>
    public class Global : HttpApplication
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Global));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Start(object sender, EventArgs e)
        {
            // Codice eseguito all'avvio dell'applicazione
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            string pathfileinfo;
            pathfileinfo = System.Configuration.ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
            System.IO.FileInfo fileconfiglog4net = new System.IO.FileInfo(pathfileinfo);
            XmlConfigurator.ConfigureAndWatch(fileconfiglog4net);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Log.Debug("OPENgovSPORTELLO.Application_Error::sono quì");
            Exception exception = Server.GetLastError();
            // global handling code goes here
            Log.Debug("OPENgovSPORTELLO.Application_Error::errore::", exception);
            Server.ClearError();
            //Response.Redirect("~/Error.html");
        }
    }
}