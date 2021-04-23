using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using OPENgovSPORTELLO.Models;
using AnagInterface;
using System.Text;
using System.Collections.Generic;
using log4net;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Pagina di registrazione
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class RegisterFO : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RegisterFO));
        private BLL.Settings fncMng = new BLL.Settings();

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
            string uniqueId = "";
            string sScript = "";
            try
            {
                Startup.CountScript += 1;
                uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                sScript = "<script language='javascript'>";
                sScript += new BLL.GestForm().GetLabel("RegisterFO", "");
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.RegisterFO.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="11/03/2020"><strong>SPID</strong>Le regole SPID vogliono che il pulsante di accesso sia subito visibile; bisogna quindi spostare la selezione dell'ente dopo l'autenticazione</revision></revisionHistory>
        /// <revisionHistory><revision date="14/04/2021"><strong>SPID</strong>Se mi sono autenticato con SPID ma non sono registrato precompilo i campi</revision></revisionHistory>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    List<GenericCategory> ListGenEnti = new BLL.Settings().LoadEnti(string.Empty, MySession.Current.ComuneSito);
                    if (ListGenEnti.Count == 1)
                        ListGenEnti = new BLL.Settings().LoadEnti(string.Empty, string.Empty);
                    new General().LoadCombo(ddlEnte, ListGenEnti, "CODICE", "DESCRIZIONE");
                    if (ListGenEnti.Count == 2)
                    {
                        ddlEnte.SelectedValue = ListGenEnti[1].Codice;
                        ddlEnte.Enabled = false;
                        MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                        Startup.CountScript += 1;
                        string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                        string sScript = "<script language='javascript'>";
                        sScript += "$('#hdDescrEnte').val('" + MySession.Current.Ente.Descrizione.Replace("'", "’") + " " + MySession.Current.Ente.Ambiente + "');";
                        sScript += "$('.divLogo').css('background-image', 'none');";
                        sScript += "</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                    }
                    if (MySession.Current.SPIDAuthn != null)
                    {
                        infoSPID.Text = "Controllare e completare il form per proseguire con la registrazione.";
                        Email.Text = MySession.Current.SPIDAuthn.email;
                        if (MySession.Current.SPIDAuthn.fiscalNumber != string.Empty)
                            TextBoxCodiceFiscale.Text = MySession.Current.SPIDAuthn.fiscalNumber;
                        else if (MySession.Current.SPIDAuthn.ivaCode!=string.Empty)
                            TextBoxCodiceFiscale.Text = MySession.Current.SPIDAuthn.ivaCode;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.RegisterFO.Page_Load::errore::", ex);
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            try
            {
                string utente = Email.Text;
                string CodFiscPIVA = TextBoxCodiceFiscale.Text;
                if (MySession.Current.Ente != null)
                {
                    if (utente != string.Empty)
                    {
                        List<UserRole> ListGen = fncMng.LoadUserRole(utente, string.Empty, true, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser);
                        if (ListGen.Count > 0)
                        {
                            if (ListGen[0].NameUser == utente)
                            {
                                ErrorMessage.Text = "L\'indirizzo mail è già presente.";
                            }
                            if (ListGen[0].CFPIVA == CodFiscPIVA)
                            {
                                ErrorMessage.Text = "Codice Fiscale/P.IVA già presente.";
                            }
                            else {
                                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();

                                //genero password random
                                string RandomPassword = new ApplicationUser().CreatePassword(8);

                                var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text, IDROLE = UserRole.PROFILO.UtenteFrontEnd, CodiceFiscale = TextBoxCodiceFiscale.Text, PWDToSend = RandomPassword.Substring(RandomPassword.Length / 2, RandomPassword.Length / 2), LastPasswordChangedDate = DateTime.Now };
                                IdentityResult result = manager.Create(user, RandomPassword);

                                if (result.Succeeded)
                                {
                                    DettaglioAnagrafica cfpiva = new DettaglioAnagrafica();
                                    cfpiva.CodEnte = MySession.Current.Ente.IDEnte;
                                    if (TextBoxCodiceFiscale.Text.Length > 11)
                                        cfpiva.CodiceFiscale = TextBoxCodiceFiscale.Text;
                                    else
                                        cfpiva.PartitaIva = TextBoxCodiceFiscale.Text;

                                    DettaglioAnagraficaReturn cfpivareturn = new Anagrafica.DLL.GestioneAnagrafica().GestisciAnagrafica(cfpiva, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, true, false);

                                    string code = manager.GenerateEmailConfirmationToken(user.Id);

                                    string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);

                                    string sErr = string.Empty;
                                    string TestoMail = "Buongiorno,";
                                    TestoMail += "\nper confermare l'account, aprire il link: " + callbackUrl;
                                    if (MySession.Current.Ente.SplitPWD == 1)
                                    {
                                        TestoMail += "\n\nLa prima parte della password è: " + RandomPassword.Substring(0, RandomPassword.Length / 2);
                                        TestoMail += ", la seconda parte Le sarà inviata a mezzo posta.";
                                    }
                                    else {
                                        TestoMail += "\n\nLa password è: " + RandomPassword;
                                    }
                                    TestoMail += "\nCordiali Saluti.";
                                    new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { user.Email }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello Contribuente - Conferma account", TestoMail, new List<System.Web.Mail.MailAttachment>(), out sErr);
                                    new General().LogActionEvent(DateTime.Now, user.UserName, MySession.Current.Scope, "Register", "", "CreateUser", "registrazione nuovo account", "", Istanza.TIPO.Registrazione, MySession.Current.Ente.IDEnte);
                                    Response.Redirect(UrlHelper.GetEmailConfirmation);
                                }
                                else
                                {
                                    ErrorMessage.Text = "Tentativo di registrazione non valido.";
                                }
                            }
                        }
                        else {
                            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                            var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();

                            //genero password random
                            string RandomPassword = new ApplicationUser().CreatePassword(8);

                            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text, IDROLE = UserRole.PROFILO.UtenteFrontEnd, CodiceFiscale = TextBoxCodiceFiscale.Text, LastPasswordChangedDate = DateTime.Now };
                            IdentityResult result = manager.Create(user, RandomPassword);

                            if (result.Succeeded)
                            {
                                string code = manager.GenerateEmailConfirmationToken(user.Id);

                                string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);

                                string sErr = string.Empty;
                                string TestoMail = "Buongiorno,";
                                TestoMail += "\nper confermare l'account, aprire il link: " + callbackUrl;
                                if (MySession.Current.Ente.SplitPWD == 1)
                                {
                                    TestoMail += "\n\nLa prima parte della password è: " + RandomPassword.Substring(0, RandomPassword.Length / 2);
                                    TestoMail += ", la seconda parte Le sarà inviata a mezzo posta.";
                                }
                                else {
                                    TestoMail += "\n\nLa password è: " + RandomPassword;
                                }
                                TestoMail += "\nCordiali Saluti.";
                                new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { user.Email }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello Contribuente - Conferma account", TestoMail, new List<System.Web.Mail.MailAttachment>(), out sErr);

                                new General().LogActionEvent(DateTime.Now, user.UserName, MySession.Current.Scope, "Register", "", "CreateUser", "registrazione nuovo account", "", Istanza.TIPO.Registrazione, MySession.Current.Ente.IDEnte);
                                Response.Redirect(UrlHelper.GetEmailConfirmation);
                            }
                            else
                            {
                                ErrorMessage.Text = "Tentativo di registrazione nuovo utente non valido.";
                            }
                        }
                    }
                    else
                    {
                        ErrorMessage.Text = "Tentativo di registrazione non valido.<br>Inserire l\'indirizzo mail.";
                    }
                }
                else {
                    ErrorMessage.Text = "Tentativo di registrazione non valido.<br>Selezionare un’ente prima di poter accedere!";
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.RegisterFO.CreateUser_Click::errore::", ex);
            }
        }
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
                Log.Debug("OPENgovSPORTELLO.RegisterFO.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
        }
    }
}