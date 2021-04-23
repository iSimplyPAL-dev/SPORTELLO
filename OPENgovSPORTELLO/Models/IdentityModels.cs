using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using OPENgovSPORTELLO.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace OPENgovSPORTELLO.Models
{
    
    /// <summary>
    /// È possibile aggiungere dati utente specificando altre proprietà per la classe utente. Per ulteriori informazioni visitare http://go.microsoft.com/fwlink/?LinkID=317594.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            // Tenere presente che il valore di authenticationType deve corrispondere a quello definito in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Aggiungere qui i reclami utente personalizzati
            return userIdentity;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }
        //codice campi personalizzati utente
        [Required]
        public int IDROLE { get; set; }
        [Required]
        public string CodiceFiscale { get; set; }
        public string PWDToSend { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }

        /// <summary>
        /// creazione randomica della password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@#$%^&+=!._-";
            string res = string.Empty;
            Random rnd = new Random();
            for (int x = 0; x < length; x++)
            {
                res += valid[rnd.Next(valid.Length)];
            }
            if (!ValidatePassword(res))
                res = CreatePassword(length);
            return res;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myPwd"></param>
        /// <returns></returns>
        public bool ValidatePassword(string myPwd)
        {
            bool IsValid = true;
            try
            {
                if (!new Regex(@"[a-z]+").IsMatch(myPwd))
                    IsValid = false;
                if (!new Regex(@"[A-Z]+").IsMatch(myPwd))
                    IsValid = false;
                if (!new Regex(@"[@#$%^&+=!._-]").IsMatch(myPwd))
                    IsValid = false;
                if (!new Regex(@"[0-9]+").IsMatch(myPwd))
                    IsValid = false;
                if (!new Regex(@".{8,50}").IsMatch(myPwd))
                    IsValid = false;
                if (myPwd.IndexOf(" ") >= 0)
                    IsValid = false;
            }
            catch 
            {
                IsValid = false;
            }
            return IsValid;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationDbContextSQL : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContextSQL()
            : base("SportelloContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContextSQL Create()
        {
            return new ApplicationDbContextSQL();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    [System.Data.Entity.DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class ApplicationDbContextMySQL : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContextMySQL()
            : base("SportelloContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContextMySQL Create()
        {
            return new ApplicationDbContextMySQL();
        }
    }
}

#region Helper
namespace OPENgovSPORTELLO
{
    /// <summary>
    /// 
    /// </summary>
    public static class IdentityHelper
    {
        // Utilizzati per XSRF durante il collegamento degli account di accesso esterni
        public const string XsrfKey = "XsrfId";

        public const string ProviderNameKey = "providerName";
        public static string GetProviderNameFromRequest(HttpRequest request)
        {
            return request.QueryString[ProviderNameKey];
        }

        public const string CodeKey = "code";
        public static string GetCodeFromRequest(HttpRequest request)
        {
            return request.QueryString[CodeKey];
        }

        public const string UserIdKey = "userId";
        public static string GetUserIdFromRequest(HttpRequest request)
        {
            return HttpUtility.UrlDecode(request.QueryString[UserIdKey]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetResetPasswordRedirectUrl(string code, HttpRequest request)
        {
            try
            {
                if (MySettings.GetConfig("PortSite") != string.Empty)
                {
                    var absoluteUri = UrlHelper.GetPathSite + "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
                    return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString().Replace("/std", ":81/std");
                }
                else if (MySettings.GetConfig("UrlSite") != string.Empty)
                {
                    var absoluteUri = MySettings.GetConfig("UrlSite") + "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
                    return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
                }
                else
                {
                    var absoluteUri = "/Account/ResetPassword?" + CodeKey + "=" + HttpUtility.UrlEncode(code);
                    return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <revisionHistory><revision date="29/10/2020">per SPID è stata cambiata la prima pagina da Default a Login quindi devo atterrare sulla nuova pagina</revision></revisionHistory>
        public static string GetUserConfirmationRedirectUrl(string code, string userId, HttpRequest request)
        {
            if (MySettings.GetConfig("PortSite") != string.Empty)
            {
                var absoluteUri =":81/std/"+ UrlHelper.GetLoginFO + "?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
                return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
            }
            else if (MySettings.GetConfig("UrlSite") != string.Empty)
            {
                var absoluteUri = MySettings.GetConfig("UrlSite")+ UrlHelper.GetLoginFO+"?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
                return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
            }
            else
            {
                var absoluteUri = UrlHelper.GetLoginFO+"?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
                return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
            }
        }
        /*public static string GetUserConfirmationRedirectUrl(string code, string userId, HttpRequest request)
        {
            if (MySettings.GetConfig("PortSite") != string.Empty)
            {
                var absoluteUri = ":81/std/DefaultFO?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
                return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString().Replace("/std/Account/", "");
            }
            else if (MySettings.GetConfig("UrlSite") != string.Empty)
            {
                var absoluteUri = MySettings.GetConfig("UrlSite") + "/DefaultFO?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
                return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
            }
            else
            {
                var absoluteUri = "/DefaultFO?" + CodeKey + "=" + HttpUtility.UrlEncode(code) + "&" + UserIdKey + "=" + HttpUtility.UrlEncode(userId);
                return new Uri(request.Url, absoluteUri).AbsoluteUri.ToString();
            }
        }*/
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool IsLocalUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) || (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="response"></param>
        public static void RedirectToReturnUrl(string returnUrl, HttpResponse response)
        {
            if (!String.IsNullOrEmpty(returnUrl) && IsLocalUrl(returnUrl))
            {
                response.Redirect(returnUrl);
            }
            else
            {
                response.Redirect("~/");
            }
        }
     }
}
#endregion
