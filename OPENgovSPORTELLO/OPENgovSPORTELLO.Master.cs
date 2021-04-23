using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using log4net;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OPENgovSPORTELLO : System.Web.UI.MasterPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(OPENgovSPORTELLO));
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

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
            // Il codice seguente facilita la protezione da attacchi XSRF
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Utilizzare il token Anti-XSRF dal cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generare un nuovo token Anti-XSRF e salvarlo nel cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void master_Page_PreLoad(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Impostare il token Anti-XSRF
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? string.Empty;
            }
            else
            {
                try
                {
                    // Convalidare il token Anti-XSRF
                    if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                        || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? string.Empty))
                    {
                        throw new InvalidOperationException("Convalida del token Anti-XSRF non riuscita.");
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("master_Page_PreLoad.errore::" + ex.Message);
                    if (MySession.Current.Scope == "FO")
                        Response.Redirect("~/Account/EmailConfirmation.aspx");
                }
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            HttpContext.Current.Session["__MySession__"] = null;
            Startup.CountScript += 1;
            string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
            string sScript = "<script language='javascript'>";
            sScript += "$('#hdDescrEnte').val('');";
            sScript += "</script>";
            ScriptManager.RegisterClientScriptBlock(HeadTitleContent, this.GetType(), uniqueId, sScript, true);
            if (MySession.Current.Scope == "BO")
                Response.Redirect(UrlHelper.GetDefaultBO);
            else
                Response.Redirect(UrlHelper.GetLoginFO);
        }
    }
}