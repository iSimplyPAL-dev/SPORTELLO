using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using log4net;
using OPENgovSPORTELLO.Models;
using System.Data.SqlClient;
using Microsoft.AspNet.SignalR;
using System.Web;
using Microsoft.AspNet.Identity;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Classe di gestione pagina generale
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class GeneralPage : Page
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GeneralPage));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                Log.Debug("OPENgovSPORTELLO.GeneralPage::Scope=" + MySession.Current.Scope + " - Ente= " + ((MySession.Current.Ente == null) ? "nessuno" : MySession.Current.Ente.IDEnte + " -> " + MySession.Current.Ente.Descrizione));
                List<string> myList = Request.Url.Authority.Split(char.Parse(".")).ToList();
                if (myList != null)
                    MySession.Current.ComuneSito = myList[0].ToLower().Replace(":", "").Replace(".", "").Replace("utd", "").Replace("std", "");
                else
                    MySession.Current.ComuneSito = string.Empty;

                string sScript = "<script language='javascript'>";
                sScript += "LoadCSS('" + MySession.Current.ComuneSito + "');";
                Log.Debug("OPENgovSPORTELLO.GeneralPage.OnInit::LoadCSS->" + MySession.Current.ComuneSito);
                sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_hfFrom').val('" + MySession.Current.Scope + "');";
                if (MySession.Current.Ente != null)
                {
                    sScript += "$('#hdDescrEnte').val('" + MySession.Current.Ente.Descrizione.Replace("'", "’") + " " + MySession.Current.Ente.Ambiente + "');";
                    if (MySession.Current.Ente.Logo.PostedFile != null && MySession.Current.Ente.Logo.NameLogo != string.Empty)
                    {
                        try
                        {
                            System.IO.File.WriteAllBytes(UrlHelper.GetPathLogo + MySession.Current.Ente.Logo.NameLogo, MySession.Current.Ente.Logo.PostedFile);
                        }
                        catch(Exception ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.GeneralPage.OnInit::errore download logo->" + UrlHelper.GetPathAttachments + MySession.Current.Ente.Logo.NameLogo, ex);
                        }
                    }
                    try {
                        sScript += "$('.divLogo').css('background-image', 'url(\"../Images/" + MySession.Current.Ente.Logo.NameLogo + "\")');";
                    }
                    catch(Exception ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.GeneralPage.OnInit::errore caricamento logo->" + UrlHelper.GetPathAttachments + MySession.Current.Ente.Logo.NameLogo, ex);
                    }
                }
                else {
                    sScript += "$('#hdDescrEnte').val('');";
                    sScript += "$('.divLogo').removeClass();$('.divLogo').css('background-image', 'none');";
                    sScript += "$('.divLogo').addClass('divLogo');";
                }
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                base.OnInit(e);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.GeneralPage.OnInit::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        public void LoadException(Exception exception)
        {
            Server.ClearError();
            if (!(exception is System.Threading.ThreadAbortException) && exception.Message.IndexOf("to the path") <= 0 && exception.Message!= "Thread was being aborted.")
            {
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                string sScript = "<script language='javascript'>";
                sScript += "$('#divException').show();";
                sScript += "$('#lblDescrErr').text('" + exception.Message.Replace("'", "").Replace("\r\n"," ") + "');";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
            }
        }
    }
    /// <summary>
    /// Classe di gestione pagina base
    /// </summary>
    public class BasePage : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GeneralPage));
        public string BreadCrumb { get; set; }
        public List<Models.ManageForm> ListLbl = new List<Models.ManageForm>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                if (!Request.IsAuthenticated)
                {
                    if (MySession.Current.Scope == "FO")
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetLoginFO, Response);
                    else
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetLoginBO, Response);
                }
                else
                {
                    Log.Debug("non ho idtipoprofilo o sono in fo senza idcontribtowork");
                    if (MySession.Current.UserLogged.IDTipoProfilo <= 0 || (MySession.Current.Scope == "FO" && MySession.Current.UserLogged.IDContribToWork <= 0))
                    {
                        MySession.Current.Ente = null;
                        Context.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        if (MySession.Current.Scope == "FO")
                            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetLoginFO, Response);
                        else
                            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetLoginBO, Response);
                    }
                }
                if (MySession.Current.UserLogged.IDContribToWork > 0)
                {
                    Log.Debug("carico angrafica");
                    AnagInterface.DettaglioAnagrafica Anag = new AnagInterface.DettaglioAnagrafica();
                    if (MySession.Current.myAnag == null)
                    {
                        Anag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                    }
                    else {
                        Anag = MySession.Current.myAnag;
                        if (Anag.COD_CONTRIBUENTE != MySession.Current.UserLogged.IDContribToWork)
                        {
                            Anag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                        }
                    }
                    MySession.Current.myAnag = Anag;
                }
                else
                {
                    MySession.Current.myAnag = null;
                }
                Log.Debug("OPENgovSPORTELLO.BasePage::UserName=" + MySession.Current.UserLogged.NameUser + " - IdContribuente= " + MySession.Current.UserLogged.IDContribToWork.ToString() + " - IdRole= " + MySession.Current.UserLogged.IDTipoProfilo.ToString());
                base.OnInit(e);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BasePage.OnInit::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="IsVisible"></param>
        public void ShowHide(string objId, bool IsVisible)
        {
            if (IsVisible)
                RegisterScript(string.Format(@"Scopri('{0}');", objId), typeof(Page));
            else
                RegisterScript(string.Format(@"Nascondi('{0}');", objId), typeof(Page));
        }
        public void ShowHideRibalta(string objId, string objControllo)
        {
            RegisterScript(string.Format(@"ShowHideRibalta('{0}','{1}');", objId, objControllo), typeof(Page));
        }
        /// <summary>
        /// Registers client script to Type
        /// </summary>
        /// <param name="script">The script to register</param>
        /// <param name="uniqueId">The unique Key to register the script</param>
        /// <param name="type">The Type of Control where to register script.</param>
        protected void RegisterScript(string script, Type type)
        {
            Startup.CountScript += 1;
            string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
            string sScript = "<script language='javascript'>";
            sScript += script;
            sScript += "</script>";
            ClientScript.RegisterStartupScript(type, uniqueId, sScript);
        }
    }
}