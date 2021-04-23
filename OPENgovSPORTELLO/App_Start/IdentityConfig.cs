using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using OPENgovSPORTELLO.Models;

using System.Net;
using System.Configuration;
using System.Diagnostics;
using log4net;
using System.Web.Mail;
using System.Collections.Generic;
using AnagInterface;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Classe di gestione INVIO MAIL
    /// </summary>
    public class EmailService : IIdentityMessageService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EmailService));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendAsync(IdentityMessage message)
        {
            await SendEmail(message,new BaseMail() , new List<string>() { message.Destination }, new List<string>(), new List<string>(), new List<MailAttachment>());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="myMail"></param>
        /// <param name="ListTO"></param>
        /// <param name="ListCC"></param>
        /// <param name="ListBCC"></param>
        /// <param name="ListAttachment"></param>
        /// <returns></returns>
        public async Task SendAsync(IdentityMessage message, BaseMail myMail, List<string> ListTO, List<string> ListCC, List<string> ListBCC, List<MailAttachment> ListAttachment)
        {
            await SendEmail(message, myMail, ListTO, ListCC, ListBCC, ListAttachment);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="myMail"></param>
        /// <param name="ListTO"></param>
        /// <param name="ListCC"></param>
        /// <param name="ListBCC"></param>
        /// <param name="ListAttachment"></param>
        /// <returns></returns>
        private async Task SendEmail(IdentityMessage message, BaseMail myMail, List<string> ListTO, List<string> ListCC, List<string> ListBCC, List<MailAttachment> ListAttachment)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Server);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.Sender);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);

                mail.From = myMail.Sender;
                string mailTo = string.Empty;
                foreach (string myRecipient in ListTO)
                {
                    mailTo = (mailTo != string.Empty ? ";": "")+myRecipient;
                }
                mail.To = mailTo;
                mailTo = string.Empty;
                foreach (string myRecipient in ListCC)
                {
                    mailTo = (mailTo != string.Empty ? ";" : "") + myRecipient;
                }
                mail.Cc = mailTo;
                string mailToBCC = string.Empty;
                if (myMail.SSL==0)
                {
                    foreach (string myRecipient in ListBCC)
                    {
                        mailToBCC = (mailTo != string.Empty ? ";" : "") + myRecipient;
                    }
                }
                mail.Bcc = mailToBCC;
                mail.Subject = message.Subject;
                mail.Body = message.Body;
                foreach (MailAttachment myAttach in ListAttachment)
                {
                    mail.Attachments.Add(myAttach);
                }
                
                try
                {
                    Log.Debug("devo inviare mail da myMail.Server->" + myMail.Server);
                    Log.Debug("devo inviare mail da myMail.ServerPort->" + myMail.ServerPort);
                    Log.Debug("devo inviare mail da myMail.Sender->" + myMail.Sender);
                    Log.Debug("devo inviare mail da myMail.Password->" + myMail.Password);
                    Log.Debug("devo inviare mail da myMail.SSL->" + myMail.SSL);
                    Log.Debug("devo inviare mail da myMail.From->" + mail.From);
                    Log.Debug("devo inviare mail da myMail.To->" + mail.To);
                    Log.Debug("devo inviare mail da mail.Subject->" + mail.Subject);
                    Log.Debug("devo inviare mail da mail.Body->" + mail.Body);
                    SmtpMail.Send(mail);
                    if (myMail.SSL==1)
                    {
                        try
                        {
                            mail = new MailMessage();
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Server);
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.Sender);
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);
                            mail.From = myMail.SenderName;
                            mail.To = myMail.Archive;
                            mail.Subject = message.Subject;
                            mail.Body = message.Body;

                            SmtpMail.Send(mail);
                        }
                        catch (Exception mailerr)
                        {
                            Log.Debug("OPENgovSPORTELLO.EmailService.SendEmail.SendSSL::errore::", mailerr);
                            try
                            {
                                mail = new MailMessage();
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Server);
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.Sender);
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                                mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);
                                mail.From = myMail.SenderName;
                                mail.To = myMail.WarningRecipient;
                                mail.Subject = myMail.WarningSubject;
                                mail.Body = (myMail.WarningMessage + " Errore rilevato:" + mailerr.Message + "\nMail inviata a:" + message.Destination + "\nMail:" + message.Subject + "\n" + message.Body);

                                SmtpMail.Send(mail);
                            }
                            catch (Exception err)
                            {
                                Log.Debug("OPENgovSPORTELLO.EmailService.SendEmailWarning.mailerr::errore::", err);
                                throw err;
                            }
                        }
                    }
                }
                catch (Exception mailEx)
                {
                    Log.Debug("OPENgovSPORTELLO.EmailService.SendEmail.Send::errore::", mailEx);
                    try
                    {
                        mail = new MailMessage();
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Server);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.Sender);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                        mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);
                        mail.From =  myMail.SenderName;
                        mail.To=myMail.WarningRecipient;
                        //mail.Bcc=myMail.Archive;
                        mail.Subject = myMail.WarningSubject;
                        mail.Body = (myMail.WarningMessage + " Errore rilevato:" + mailEx.Message + "\nMail inviata a:" + message.Destination + "\nMail:" + message.Subject + "\n" + message.Body);

                        Log.Debug("devo inviare mail da myMail.Server->" + myMail.Server);
                        Log.Debug("devo inviare mail da myMail.ServerPort->" + myMail.ServerPort);
                        Log.Debug("devo inviare mail da myMail.Sender->" + myMail.Sender);
                        Log.Debug("devo inviare mail da myMail.Password->" + myMail.Password);
                        Log.Debug("devo inviare mail da myMail.SSL->" + myMail.SSL);
                        Log.Debug("devo inviare mail da myMail.From->" + mail.From);
                        Log.Debug("devo inviare mail da myMail.To->" + mail.To);
                        Log.Debug("devo inviare mail da mail.Subject->" + mail.Subject);
                        Log.Debug("devo inviare mail da mail.Body->" + mail.Body);
                        SmtpMail.Send(mail);
                    }
                    catch (Exception err)
                    {
                        Log.Debug("OPENgovSPORTELLO.EmailService.SendEmailWarning::errore::", err);
                        throw err;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.EmailService.SendEmail::errore::", ex);
                try
                {
                    Log.Debug("devo inviare mail di errore da myMail.Sender" + myMail.Sender);
                    Log.Debug("devo inviare mail di errore da myMail.ServerPort" + myMail.ServerPort);
                    Log.Debug("devo inviare mail di errore da myMail.SenderName" + myMail.SenderName);
                    Log.Debug("devo inviare mail di errore da myMail.Password" + myMail.Password);
                    Log.Debug("devo inviare mail di errore da myMail.SSL" + myMail.SSL);
                    Log.Debug("devo inviare mail di errore da myMail.WarningRecipient" + myMail.WarningRecipient);
                    Log.Debug("devo inviare mail di errore da myMail.WarningSubject" + myMail.WarningSubject);
                    Log.Debug("devo inviare mail di errore da myMail.Body" + (myMail.WarningMessage + message.Destination));
                    MailMessage mail = new MailMessage();
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", myMail.Sender);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", myMail.ServerPort);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2"); //Send the message using the network (SMTP over the network)
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //YES
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", myMail.SenderName);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", myMail.Password);
                    mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", myMail.SSL);
                    mail.From = myMail.SenderName;
                    mail.To = myMail.WarningRecipient;
                    mail.Bcc = myMail.Archive;
                    mail.Subject = myMail.WarningSubject;
                    mail.Body = (myMail.WarningMessage + message.Destination);

                    SmtpMail.Send(mail);
                }
                catch (Exception err)
                {
                    Log.Debug("OPENgovSPORTELLO.EmailService.SendEmailWarning::errore::", err);
                    throw err;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myMail"></param>
        /// <param name="ListRecipient"></param>
        /// <param name="ListRecipientBCC"></param>
        /// <param name="MailSubject"></param>
        /// <param name="MailBody"></param>
        /// <param name="ListAttachment"></param>
        /// <param name="sErr"></param>
        public void CreateMail(BaseMail myMail,List<string> ListRecipient, List<string> ListRecipientBCC, string MailSubject, string MailBody, List<MailAttachment> ListAttachment, out string sErr)
        {
            sErr = string.Empty;
            try
            {
                IdentityMessage myMessage = new IdentityMessage();
                myMessage.Body = MailBody;
                myMessage.Subject = MailSubject;
                new EmailService().SendAsync(myMessage,myMail, ListRecipient, new List<string>(), ListRecipientBCC, ListAttachment);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.EmailService.CreateMail::errore::", ex);
                sErr = ex.Message;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Inserire qui la parte di codice del servizio SMS per l'invio di un SMS.
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Configurare la gestione utenti dell'applicazione utilizzata in questa applicazione. UserManager viene definito in ASP.NET Identity e viene utilizzato dall'applicazione.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public static readonly int PasswordExpireDays = UrlHelper.PasswordExpireDays;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            ApplicationUserManager manager; 
            if (RouteConfig.TypeDB == "MySQL")
                manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContextMySQL>()));
            else
                manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContextSQL>()));
            // Configurare la logica di convalida per i nomi utente
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            //Configurare la logica di convalida per le password
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Registrare i provider di autenticazione a due fattori. Questa applicazione usa il numero di telefono e gli indirizzi e-mail come metodi per ricevere un codice di verifica dell'utente
            // Si può scrivere un provider personalizzato e inserirlo qui.
            manager.RegisterTwoFactorProvider("Codice telefono", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Il codice di sicurezza è {0}"
            });
            manager.RegisterTwoFactorProvider("Codice e-mail", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Codice di sicurezza",
                BodyFormat = "Il codice di sicurezza è {0}"
            });

            // Configurare le impostazioni predefinite per il blocco dell'utente
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(60);
            manager.MaxFailedAccessAttemptsBeforeLockout = 3;

            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="authenticationManager"></param>
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager) { }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

    }
    /// <summary>
    /// Classe di gestione login
    /// </summary>
    public class LoginManager
    {
         private static readonly ILog Log = LogManager.GetLogger(typeof(LoginManager));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEmail"></param>
        /// <param name="mySpIDCode"></param>
        /// <param name="FailureText"></param>
        /// <returns></returns>
        /// <revisionHistory><revision date="11/03/2020"><strong>SPID</strong>Le regole SPID vogliono che il pulsante di accesso sia subito visibile; bisogna quindi spostare la selezione dell'ente dopo l'autenticazione</revision></revisionHistory>
        public string ManageLogin(string myEmail, string mySpIDCode, out string FailureText)
        {
            UserRole myUser = new UserRole();
            FailureText = string.Empty;
            string myEnte = string.Empty;
            try
            {
                if (MySession.Current.Ente != null)
                {
                    myEnte = MySession.Current.Ente.IDEnte;
                }
                new General().LogActionEvent(DateTime.Now, myEmail, "", "Login", "Login", "LogIn", "login a sistema", mySpIDCode, "Accesso", myEnte);
                List<UserRole> ListGen = new BLL.Settings().LoadUserRole(myEmail, string.Empty, true, myEnte, myEmail);
                if (ListGen.Count > 0)
                {
                    myUser = ListGen[0];
                    string cfpivatemp = myUser.CFPIVA;

                    DettaglioAnagrafica myAnag = new DettaglioAnagrafica();
                    if (cfpivatemp.Length > 11)
                        myAnag.CodiceFiscale = cfpivatemp;
                    else
                        myAnag.PartitaIva = cfpivatemp;
                    Log.Debug("prelevo anagrafica per " + cfpivatemp);
                    DettaglioAnagraficaReturn myAnagTmp = new Anagrafica.DLL.GestioneAnagrafica().GestisciAnagrafica(myAnag, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, true, false);

                    //creo variabili di sessione
                    if (myUser.IDTipoProfilo != UserRole.PROFILO.UtenteFrontEnd)
                    {
                        FailureText = "Tentativo di accesso non valido";
                        return "Errore";
                    }
                    else {
                        MySession.Current.UserLogged = ListGen[0];
                        Log.Debug("cod contribuente->" + myAnagTmp.COD_CONTRIBUENTE);
                        MySession.Current.UserLogged.IDContribLogged = int.Parse(myAnagTmp.COD_CONTRIBUENTE);
                        MySession.Current.UserLogged.Nominativo = myAnagTmp.NOMINATIVO;
                        if (myAnagTmp.NOMINATIVO == "")
                        {
                            if (MySession.Current.Ente != null)
                            {
                                if (MySession.Current.Ente != null)
                                {
                                    if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E")
                                    {
                                        if (MySession.Current.myAnag == null)
                                        {
                                            MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribLogged, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                                        }
                                        Log.Debug("ho anagrafica di idcontrib->" + MySession.Current.myAnag.COD_CONTRIBUENTE.ToString());
                                        string IdSoggetto = string.Empty;
                                        if ((MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale).Trim() != string.Empty)
                                        {
                                            Log.Debug("ManageLogin.ReadDatiVerticale per->"+ (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale));
                                            new BLLImport().ReadDatiVerticale(myEnte, (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale), ref IdSoggetto);
                                            new BLLImport().SetDatiVerticaleRead(myEnte, IdSoggetto);
                                        }
                                        else
                                        {
                                            Log.Debug("ManageLogin.NO ReadDatiVerticale perché non ho riferimento");
                                        }
                                    }
                                }
                            }
                            if (myAnagTmp.NOMINATIVO == "")
                            {
                                MySession.Current.UserLogged.IDContribToWork = MySession.Current.UserLogged.IDContribLogged;
                                return "GetProfiloFO";
                            }
                            else
                            {
                                return "GetDefaultFO";
                            }
                        }
                    }
                }
                else
                {
                    return "GetmyEmailConfirmation";
                }
                if (MySession.Current.UserLogged.ListDeleganti != string.Empty)
                {
                    return "GetSelDelegante";
                }
                else {
                    MySession.Current.UserLogged.IDContribToWork = MySession.Current.UserLogged.IDContribLogged;
                    if (MySession.Current.Ente != null)
                    {
                        if (MySession.Current.Ente != null)
                        {
                            if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E")
                            {
                                if (MySession.Current.myAnag == null)
                                {
                                    MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                                }
                                string IdSoggetto = string.Empty;
                                if ((MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale).Trim() != string.Empty)
                                {
                                    Log.Debug("ManageLogin.ReadDatiVerticale per->" + (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale));
                                    new BLLImport().ReadDatiVerticale(myEnte, (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale), ref IdSoggetto);
                                    new BLLImport().SetDatiVerticaleRead(myEnte, IdSoggetto);
                                }
                                else
                                {
                                    Log.Debug("ManageLogin.NO ReadDatiVerticale perché non ho riferimento");
                                }
                            }
                        }
                    }
                    return "GetDefaultFO";
                }
            }
            catch (Exception ex)
            {
                FailureText = ex.Message;
                return "Errore";
            }
        }
    }
}
