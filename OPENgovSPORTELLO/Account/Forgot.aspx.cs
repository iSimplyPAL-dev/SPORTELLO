using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using OPENgovSPORTELLO.Models;
using log4net;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Pagina per il reset della password.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ForgotPassword : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ForgotPassword));
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.Scope, "", "Login", "Forgot", "Page_Load", "ingresso pagina", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.ForgotPassword.Page_Load::errore::", ex);
                LoadException(ex);
            }
        }
         /// <summary>
        /// Controlla che la mail inserita appartenga ad un account validato.
        /// Invia un messaggio di posta elettronica con il codice e il reindirizzamento alla pagina di reimpostazione della password.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Forgot(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Convalidare l'indirizzo e-mail dell'utente
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.FindByName(Email.Text);
                if (user == null || !manager.IsEmailConfirmed(user.Id))
                {
                    FailureText.Text = "L'utente non esiste o non è confermato.";
                    ErrorMessage.Visible = true;
                    return;
                }
                // Per ulteriori informazioni su come abilitare la conferma dell'account e la reimpostazione della password, visitare http://go.microsoft.com/fwlink/?LinkID=320771
                // Inviare un messaggio di posta elettronica con il codice e il reindirizzamento alla pagina di reimpostazione della password
                string code = manager.GeneratePasswordResetToken(user.Id);
                string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                callbackUrl = callbackUrl + "&utente=" + Email.Text;
                manager.SendEmail(user.Id, "Reimposta password", "Per reimpostare la password, fare clic sul seguente link: " + callbackUrl );
                loginForm.Visible = false;
                DisplayEmail.Visible = true;
                new General().LogActionEvent(DateTime.Now, MySession.Current.Scope, "", "Login", "Forgot", "Forgot", "richiesto nuova password", "", "", "");
            }
        }
    }
}