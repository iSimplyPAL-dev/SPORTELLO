using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Pagina per il cambio password
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class ResetPassword : GeneralPage
    {
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var EmailUtentePassata = Request.Params["utente"];
            Email.Text = EmailUtentePassata;
            if (Request.Params["ExpiredPwd"] != null)
            {
                if (Request.Params["ExpiredPwd"] == "1")
                {                   
                    Startup.CountScript += 1;
                    string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                    string sScript = "<script language='javascript'>";
                    sScript += "$('#TitleH4').text('Password scaduta. E’ necessario reimpostarla.');";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                }
            }
            if (Request.Params["hfFrom"] != null)
            {
                if (Request.Params["hfFrom"].ToString() == "BO")
                {
                    hfFrom.Value = Request.Params["hfFrom"].ToString();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string StatusMessage
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Reset_Click(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            if (code != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

                var user = manager.FindByName(Email.Text);
                if (user == null)
                {
                    ErrorMessage.Text = "Nessun utente trovato.";
                    return;
                }
                var result = manager.ResetPassword(user.Id, code, Password.Text);
                if (result.Succeeded)
                {
                    user.LastPasswordChangedDate = DateTime.UtcNow;
                    manager.Update(user);
                    if(hfFrom.Value=="BO")
                        Response.Redirect("~/Account/LoginBO");
                    else
                    Response.Redirect("~/Account/ResetPasswordConfirmation");
                    return;
                }
                if (result.Errors.FirstOrDefault().ToLower().IndexOf("passwords must") >= 0)
                {
                    ErrorMessage.Text = "Password non corretta.";
                }
                else {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
                return;
            }
            ErrorMessage.Text = "Si è verificato un errore.";
        }
    }
}