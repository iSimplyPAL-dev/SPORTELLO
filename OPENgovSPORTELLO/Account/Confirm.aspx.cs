using System;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using OPENgovSPORTELLO.Models;
using log4net;
using System.Collections.Generic;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// In questa pagina viene confermato l'account registrato. E' richiamata dall'url inviato via mail. 
    /// Se mail e codice di autenticazione sono validi l'account viene confermato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class Confirm : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Confirm));
        /// <summary>
        /// Stringa messaggio di stato
        /// </summary>
        protected string StatusMessage
        {
            get;
            private set;
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            MySession.Current.Scope = "FO";
            if (!Page.IsPostBack)
            {
                List<GenericCategory> ListGenEnti = new BLL.Settings().LoadEnti(string.Empty,string.Empty);
                new General().LoadCombo(ddlEnte, ListGenEnti, "CODICE", "DESCRIZIONE");
                string code = IdentityHelper.GetCodeFromRequest(Request);
                string userId = IdentityHelper.GetUserIdFromRequest(Request);
                if (code != null && userId != null)
                {
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var result = manager.ConfirmEmail(userId, code);
                    if (result.Succeeded)
                    {
                        successPanel.Visible = true;
                        return;
                    }
                }
                successPanel.Visible = false;
                errorPanel.Visible = true;
                login.Enabled = false;
            }
        }
        /// <summary>
        /// Gestione selezione ente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ControlSelectedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlEnte.SelectedValue != string.Empty)
                {
                    login.Enabled = true;
                    MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue,MySession.Current.UserLogged.NameUser);
                    if (MySession.Current.Ente != null)
                    {
                        Startup.CountScript += 1;
                        string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                        string sScript = "<script language='javascript'>";
                         sScript += "$('#hdDescrEnte').val('" + ddlEnte.SelectedItem.Text.Replace("'", "’") + " " + ((EntiInLavorazione)MySession.Current.Ente).Ambiente + "');";
                        sScript += "</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Confirm.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
        }
    }
}