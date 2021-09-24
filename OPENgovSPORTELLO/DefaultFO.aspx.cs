using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Sulla videata di apertura (esempio sopra) sarà visualizzata una situazione riepilogativa del contribuente.
    /// Nella sezione inferiore del video saranno evidenziate le comunicazioni (istanze) intercorse con il back office con il relativo stato/esito.
    /// Nella sezione superiore del video, saranno presenti i bottoni  per accedere alle varie sezioni di lavoro dei tributi.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    //public partial class DefaultFO : GeneralPage
    public partial class DefaultFO : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DefaultFO));

        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="25/03/2021">Nascosto blocco Paga se l'ente non ha PagoPA attivo</revision></revisionHistory>
        protected void Page_Load(object sender, EventArgs e)
        {
            string ScriptToExec = string.Empty;

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
                        ScriptToExec += "$('#hdDescrEnte').val('" + ddlEnte.SelectedItem.Text.Replace("'", "’") + " " + (EntiInLavorazione)MySession.Current.Ente + "');";
                        ScriptToExec += "$('.divLogo').css('background-image', 'none');";
                    }

                    MySession.Current.Scope = "FO";
                    if (MySession.Current.Ente != null)
                    {
                        ScriptToExec += "$('#hdDescrEnte').val('" + MySession.Current.Ente.Descrizione.Replace("'", "’") + " " + MySession.Current.Ente.Ambiente + "');";
                        ScriptToExec += "$('.divLogo').css('background-image', 'none');";                    
                        if (MySession.Current.UserLogged.IDContribToWork > 0)
                        {
                            List<GenericCategory> ListComunicazioni = new List<GenericCategory>();
                        }
                        ddlEnte.SelectedValue = MySession.Current.Ente.IDEnte;
                        ddlEnte.Attributes.Add("disabled", "disabled");

                        if (MySession.Current.Ente.DatiPagoPA.EndpointOTF == string.Empty)
                        {
                            ScriptToExec += "$('#divPaga').hide();";
                        }
                        else
                        {
                            ScriptToExec += "$('#divPaga').show();";
                        }
                    }
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
                                return;
                            }
                        }
                        successPanel.Visible = false;
                        errorPanel.Visible = true;
                        return;
                    }
                }
                successPanel.Visible = false;
                errorPanel.Visible = false;

                new BLL.Messages(new Models.Message()).SendMail();
                new BLL.GestForm().KillSleepProcess();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.DefaultFO.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                GetLabelForm();
                new General().ClearSession();
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += "$('#FAQ').addClass('HelpFO');";
                sScript += ScriptToExec;
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                RegisterScript(new BLL.Profilo().LoadJumbotronFO(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                //RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="25/03/2021">Nascosto blocco Paga se l'ente non ha PagoPA attivo</revision></revisionHistory>
        protected void ControlSelectedChanged(object sender, EventArgs e)
        {
            try
            {
                if (MySession.Current.UserLogged.NameUser == string.Empty)
                {
                    Startup.CountScript += 1;
                    string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                    string sScript = "<script language='javascript'>";
                    sScript += "alert('Autenticarsi prima di selezionare un’ente!');";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                }
                else {
                    if (ddlEnte.SelectedValue != string.Empty)
                    {
                        MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                        Startup.CountScript += 1;
                        string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                        string sScript = "<script language='javascript'>";
                        sScript += "$('#hdDescrEnte').val('" + ddlEnte.SelectedItem.Text.Replace("'", "’") + " " + ((EntiInLavorazione)MySession.Current.Ente).Ambiente + "');";
                        sScript += "$('.divLogo').css('background-image', 'none');";
                        if (MySession.Current.Ente.DatiPagoPA.EndpointOTF == string.Empty)
                        {
                            sScript += "$('#divPaga').hide();";
                        }
                        else
                        {
                            sScript += "$('#divPaga').show();";
                        }
                        sScript += "</script>";
                        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                        ddlEnte.Attributes.Add("disabled", "disabled");
                        string myFailureText = string.Empty;
                        //BD 24/09/2021 Problema con le deleghe
                        //string mySignIn = new LoginManager().ManageLogin(MySession.Current.UserLogged.NameUser, "", out myFailureText);
                        string mySignIn = new LoginManager().ManageLogin(MySession.Current.UserLogged.NameUser,
                                                                            MySession.Current.UserLogged.CFPIVA,
                                                                            "", out myFailureText);
                        //BD 24/09/2021 Problema con le deleghe

                        switch (mySignIn)
                        {
                            case "GetProfiloFO":
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
                                break;
                            case "GetEmailConfirmation":
                                MySession.Current.Ente = null;
                                Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetEmailConfirmation, Response);
                                break;
                            case "Errore":
                                errorPanel.Visible = true;
                                break;
                            case "GetSelDelegante":
                                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetSelDelegante, Response);
                                break;
                            case "GetDefaultFO":
                            default:
                                break;
                        }
                    }
                    else
                        MySession.Current.Ente = null;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.DefaultFO.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected void GetLabelForm()
        {
            try
            {
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += new BLL.GestForm().GetLabel("DefaultFO", "");
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BasePage.GetLabelForm::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GIS(object sender, EventArgs e)
        {
            try
            {
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>autoSubmit();</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Protocolla::errore::", ex);
                LoadException(ex);
            }
        }
    }
}