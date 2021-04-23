using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using OPENgovSPORTELLO.Models;
using System.Collections.Generic;
using AnagInterface;
using log4net;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Autentica e autorizza l'account al sistema lato Sportello Contribuente
    /// <strong>Gestione commercialisti/CAF</strong>
    /// L’utente “CAF” dopo l'accesso deve scegliere un contribuente su cui lavorare.
    /// L’utente “delegato” accede come un contribuente classico, se però avrà presentato delle istanze di delega (in qualsiasi stato esse siano) avrà la dicitura “delegato” in altro a sinistra.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class Login : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Login));
        private BLL.Settings fncMng = new BLL.Settings();
        public string UserIP { get; set; }
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
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += "ChooseLogin(this, 'byAccount', 'bySpID');";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.LoginFO.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="11/03/2020"><strong>SPID</strong>Le regole SPID vogliono che il pulsante di accesso sia subito visibile; bisogna quindi spostare la selezione dell'ente dopo l'autenticazione</revision></revisionHistory>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                UserIP = Request.UserHostAddress.ToString();
                UserIP = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                successPanel.Visible = false;
                errorPanel.Visible = false;
                //when user is behind proxy server
                if (UserIP == null)
                {
                    UserIP = Request.ServerVariables["REMOTE_ADDR"];
                }
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                if (!Page.IsPostBack)
                {
                    RegisterHyperLink.NavigateUrl = "RegisterFO";
                    Startup.CountScript += 1;
                    List<GenericCategory> ListGenEnti = new BLL.Settings().LoadEnti(string.Empty, MySession.Current.ComuneSito);
                    if (ListGenEnti.Count == 1)
                        ListGenEnti = new BLL.Settings().LoadEnti(string.Empty, string.Empty);
                    new General().LoadCombo(ddlEnte, ListGenEnti, "CODICE", "DESCRIZIONE");
                    if (ListGenEnti.Count == 2)
                    {
                        ddlEnte.SelectedValue = ListGenEnti[1].Codice;
                        ddlEnte.Enabled = false;
                        MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                        sScript += "$('#hdDescrEnte').val('" + MySession.Current.Ente.Descrizione.Replace("'", "’") + " " + MySession.Current.Ente.Ambiente + "');";
                        sScript += "$('.divLogo').css('background-image', 'none');";
                    }
                    sScript += "ChooseLogin(this, 'byAccount', 'bySpID');";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                    new General().LogActionEvent(DateTime.Now, Email.Text, "", "Login", "Login", "Page_Load", "ingresso pagina", UserIP, "", "");
                    if (IdentityHelper.GetCodeFromRequest(Request) != null)
                    {
                        string code = IdentityHelper.GetCodeFromRequest(Request);
                        string userId = IdentityHelper.GetUserIdFromRequest(Request);
                        if (code != null && userId != null)
                        {
                            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                            var result = manager.ConfirmEmail(userId, code);
                            if (result.Succeeded)
                            {
                                successPanel.Visible = true;
                            }
                            else {
                                errorPanel.Visible = true;
                            }
                        }
                        else {
                            errorPanel.Visible = true;
                        }
                    }
                }
                //ForgotPasswordHyperLink.NavigateUrl = "Forgot";
                var returnUrl = HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
                if (!String.IsNullOrEmpty(returnUrl))
                {
                    RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + returnUrl;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.LoginFO.Page_Load::errore::", ex);
            }
        }
        /// <summary>
        /// Funzione di controllo acceso
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
        /// <revisionHistory><revision date="19/12/2019">al primo accesso (nominativo='') carico i dati dal verticale se nominativo ancora vuoto allora mando alla pagina del profilo</revision></revisionHistory>
        /// <revisionHistory><revision date="30/01/2020">SPID</revision></revisionHistory>
        /// <revisionHistory><revision date="25/03/2021">Tolto selezione ente perché con SPID non riesco a farlo tornare</revision></revisionHistory>
        protected void LogIn(object sender, EventArgs e)
        {
            UserRole myUser = new UserRole();
            try
            {
                if (IsValid)
                {
                    // Convalidare la password utente
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                    signinManager.UserManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(60);

                    if (chkAccept.Checked)
                    {
                        var user = manager.FindByName(Email.Text);
                        if (user == null)
                        {
                            FailureText.Text = "Nessun utente trovato";
                            ErrorMessage.Visible = true;
                            return;
                        }
                        else
                        {
                            user.TwoFactorEnabled = false;
                            manager.Update(user);
                        }
                    }
                    var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);
                    switch (result)
                    {
                        case SignInStatus.Success:
                            var user = manager.FindByEmail(Email.Text);
                            if (user.LastPasswordChangedDate.AddDays(ApplicationUserManager.PasswordExpireDays) < DateTime.UtcNow)
                            {
                                string code = manager.GeneratePasswordResetToken(user.Id);
                                string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
                                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                Response.Redirect(callbackUrl + "&utente=" + user.Email + "&ExpiredPwd=1");
                            }
                            else
                            {
                                string myFailureText = string.Empty;
                                string mySignIn = new LoginManager().ManageLogin(user.Email, "", out myFailureText);
                                switch (mySignIn)
                                {
                                    case "GetProfiloFO":
                                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
                                        break;
                                    case "GetDefaultFO":
                                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
                                        break;
                                    case "GetEmailConfirmation":
                                        MySession.Current.Ente = null;
                                        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetEmailConfirmation, Response);
                                        break;
                                    case "Errore":
                                        FailureText.Text = myFailureText;
                                        ErrorMessage.Visible = true;
                                        break;
                                    case "GetSelDelegante":
                                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetSelDelegante, Response);
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case SignInStatus.LockedOut:
                            FailureText.Text = "Questo account è stato bloccato, riprovare più tardi.";
                            ErrorMessage.Visible = true;
                            break;
                        case SignInStatus.RequiresVerification:
                            FailureText.Text = "Per procedere bisogna accettare l’Informativa sul trattamento dei dati personali.";
                            ErrorMessage.Visible = true;
                            Startup.CountScript += 1;
                            string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                            string sScript = "<script language='javascript'>";
                            sScript += "$('#PrivacyPolicy').removeClass('hidden');";
                            sScript += "</script>";
                            ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                            break;
                        case SignInStatus.Failure:
                        default:
                            FailureText.Text = "Tentativo di accesso non valido. Indirizzo mail o password errati.";
                            ErrorMessage.Visible = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                FailureText.Text = ex.Message;
                ErrorMessage.Visible = true;
            }
            finally
            {
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += "ChooseLogin(this, 'byAccount', 'bySpID');";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
            }
        }
        //protected void LogIn(object sender, EventArgs e)
        //{
        //    UserRole myUser = new UserRole();
        //    try
        //    {
        //        if (IsValid)
        //        {
        //            if (MySession.Current.Ente != null)
        //            {
        //                // Convalidare la password utente
        //                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //                var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
        //                signinManager.UserManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(60);

        //                if (chkAccept.Checked)
        //                {
        //                    var user = manager.FindByName(Email.Text);
        //                    if (user == null)
        //                    {
        //                        FailureText.Text = "Nessun utente trovato";
        //                        ErrorMessage.Visible = true;
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        user.TwoFactorEnabled = false;
        //                        manager.Update(user);
        //                    }
        //                }
        //                var result = signinManager.PasswordSignIn(Email.Text, Password.Text, RememberMe.Checked, shouldLockout: true);
        //                switch (result)
        //                {
        //                    case SignInStatus.Success:
        //                        var user = manager.FindByEmail(Email.Text);
        //                        if (user.LastPasswordChangedDate.AddDays(ApplicationUserManager.PasswordExpireDays) < DateTime.UtcNow)
        //                        {
        //                            string code = manager.GeneratePasswordResetToken(user.Id);
        //                            string callbackUrl = IdentityHelper.GetResetPasswordRedirectUrl(code, Request);
        //                            Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //                            Response.Redirect(callbackUrl + "&utente=" + user.Email + "&ExpiredPwd=1");
        //                        }
        //                        else
        //                        {
        //                            string myFailureText = string.Empty;
        //                            string mySignIn = new LoginManager().ManageLogin(user.Email, "", out myFailureText);
        //                            switch (mySignIn)
        //                            {
        //                                case "GetProfiloFO":
        //                                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
        //                                    break;
        //                                case "GetDefaultFO":
        //                                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
        //                                    break;
        //                                case "GetEmailConfirmation":
        //                                    MySession.Current.Ente = null;
        //                                    Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //                                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetEmailConfirmation, Response);
        //                                    break;
        //                                case "Errore":
        //                                    FailureText.Text = myFailureText;
        //                                    ErrorMessage.Visible = true;
        //                                    break;
        //                                case "GetSelDelegante":
        //                                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetSelDelegante, Response);
        //                                    break;
        //                                default:
        //                                    break;
        //                            }
        //                        }
        //                        break;
        //                    case SignInStatus.LockedOut:
        //                        FailureText.Text = "Questo account è stato bloccato, riprovare più tardi.";
        //                        ErrorMessage.Visible = true;
        //                        break;
        //                    case SignInStatus.RequiresVerification:
        //                        FailureText.Text = "Per procedere bisogna accettare l’Informativa sul trattamento dei dati personali.";
        //                        ErrorMessage.Visible = true;
        //                        Startup.CountScript += 1;
        //                        string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
        //                        string sScript = "<script language='javascript'>";
        //                        sScript += "$('#PrivacyPolicy').removeClass('hidden');";
        //                        sScript += "</script>";
        //                        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
        //                        break;
        //                    case SignInStatus.Failure:
        //                    default:
        //                        FailureText.Text = "Tentativo di accesso non valido. Indirizzo mail o password errati.";
        //                        ErrorMessage.Visible = true;
        //                        break;
        //                }
        //            }
        //            else {
        //                FailureText.Text = "Tentativo di registrazione non valido.<br>Selezionare un’ente prima di poter accedere!";
        //                ErrorMessage.Visible = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        FailureText.Text = ex.Message;
        //        ErrorMessage.Visible = true;
        //    }
        //    finally
        //    {
        //        Startup.CountScript += 1;
        //        string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
        //        string sScript = "<script language='javascript'>";
        //        sScript += "ChooseLogin(this, 'byAccount', 'bySpID');";
        //        sScript += "</script>";
        //        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
        //    }
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ControlSelectedChanged(object sender, EventArgs e)
        {
            try
            {
                MySession.Current.Ente = null;
                if (ddlEnte.SelectedValue != string.Empty)
                {
                    MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, string.Empty);
                    Startup.CountScript += 1;
                    string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                    string sScript = "<script language='javascript'>";
                    sScript += "$('#hdDescrEnte').val('" + MySession.Current.Ente.Descrizione.Replace("'", "’") + " " + MySession.Current.Ente.Ambiente + "');";
                    sScript += "$('.divLogo').css('background-image', 'none');";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.LoginFO.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
        }
    }
}