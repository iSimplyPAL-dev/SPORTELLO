using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using OPENgovSPORTELLO.Models;
using log4net;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Pagina di accesso lato Ufficio Tributi
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class LoginBO : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginBO));
        private BLL.Settings fncMng = new BLL.Settings();
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// Autentica e autorizza l'account al sistema lato BackOffice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="05/2018"><strong>Expire&Strong Password</strong>
        /// Questa opzione non calcola il numero di tentativi di accesso non riusciti per il blocco dell'account
        /// var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: false);
        /// Per abilitare il conteggio degli errori di password e attivare il blocco, impostare shouldLockout: true
        /// Questa opzione calcola il numero di tentativi di accesso non riusciti per il blocco dell'account (manager.MaxFailedAccessAttemptsBeforeLockout = 3;)
        /// var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);
        /// </revision></revisionHistory>
        protected void LogIn(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (IsValid)
                    {
                        // Convalidare la password utente
                        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                        signinManager.UserManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(60);

                        var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);
                        
                        switch (result)
                        {
                            case SignInStatus.Success:
                                string utente = Email.Text;
                                List<UserRole> ListGen = fncMng.LoadUserRole(utente, string.Empty, false, string.Empty, utente);
                                if (ListGen.Count > 0)
                                {
                                    ApplicationUser user = manager.FindByEmail(utente);
                                        if (user.LastPasswordChangedDate.AddDays(ApplicationUserManager.PasswordExpireDays) < DateTime.UtcNow)
                                    {
                                        string code = manager.GeneratePasswordResetToken(user.Id);
                                        string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                                        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                        Response.Redirect(callbackUrl + "&utente=" + user.Email + "&ExpiredPwd=1&hfFrom=BO");
                                    }
                                    else
                                    {
                                        //creo variabili di sessione
                                        MySession.Current.UserLogged = ListGen[0];
                                        if (ListGen[0].IDTipoProfilo == UserRole.PROFILO.UtenteFrontEnd)
                                        {
                                            FailureText.Text = "Tentativo di accesso non valido";
                                            ErrorMessage.Visible = true;
                                            break;
                                        }
                                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultBO, Response);
                                    }
                                }
                                else
                                {
                                    FailureText.Text = "Tentativo di accesso non valido";
                                    ErrorMessage.Visible = true;
                                    break;
                                }
                                break;
                            case SignInStatus.LockedOut:
                                Response.Redirect("/Account/Lockout");
                                break;
                            case SignInStatus.RequiresVerification:
                                Response.Redirect(String.Format("/Account/TwoFactorAuthenticationSignIn?ReturnUrl={0}&RememberMe={1}",
                                                                Request.QueryString["ReturnUrl"],
                                                                RememberMe.Checked),
                                                  true);
                                break;
                            case SignInStatus.Failure:
                            default:
                                FailureText.Text = "Tentativo di accesso non valido";
                                ErrorMessage.Visible = true;
                                break;
                        }
                    }
                }
                catch (EntityDataSourceValidationException ex)
                {
                    Log.Debug("OPENgovSPORTELLO.LoginBO.LogIn::errore::", ex);
                }
            }
            catch (Exception er)
            {
                Log.Debug("OPENgovSPORTELLO.LoginBO.LogIn::errore::", er);
            }
        }
    }
}