using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe generale di gestione form
    /// </summary>
    public class GestForm
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GestForm));
        /// <summary>
        /// 
        /// </summary>
        public static class PlaceHolderName
        {
            public static string Home = "HeadHomeContent";
            public static string Title = "HeadTitleContent";
            public static string Menu = "LeftMenuContent";
            public static string Body = "MainContent";
        }
        /// <summary>
        /// 
        /// </summary>
        public static class FormName
        {
            public static string UIDettaglio = "Dich";
            public static string Comunicazioni = "MngMessages";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NameForm"></param>
        /// <param name="IDEnte"></param>
        /// <returns></returns>
        public string GetLabel(string NameForm, string IDEnte)
        {
            List<Models.ManageForm> ListLbl = new List<Models.ManageForm>();
            string sScript = string.Empty;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetManageForm", "FORM", "IDENTE");
                    ListLbl = ctx.ContextDB.Database.SqlQuery<ManageForm>(sSQL, ctx.GetParam("FORM", NameForm)
                        , ctx.GetParam("IDENTE", IDEnte)).ToList<ManageForm>();
                    ctx.Dispose();
                }
                if (ListLbl.Count > 0)
                {
                    foreach (ManageForm myItem in ListLbl)
                    {
                        sScript += "$('#" + myItem.NomeControllo + "').html('" + myItem.Testo.Replace("\r\n", "").Replace("'", "&rsquo;") + "');";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GestForm.GetLabel::errore::", ex);
                sScript = string.Empty;
            }
            return sScript;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myPage"></param>
        /// <param name="UrlWiki"></param>
        /// <returns></returns>
        public string GetHelp(string myPage, string UrlWiki)
        {
            string sScript = string.Empty;
            try
            {
                foreach (string myKey in System.Configuration.ConfigurationManager.AppSettings)
                {
                    if (myKey.StartsWith(myPage) && UrlWiki!=string.Empty)
                    {
                        sScript += "$('a." + myKey + "').attr('href', '" + UrlWiki + System.Configuration.ConfigurationManager.AppSettings[myKey] + "');";
                        sScript += "$('iframe." + myKey + "').attr('src', '" + UrlWiki + System.Configuration.ConfigurationManager.AppSettings[myKey] + "');";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GestForm.GetHelp::errore::", ex);
                sScript = string.Empty;
            }
            return sScript;
        }
        /// <summary>
        /// 
        /// </summary>
        public void KillSleepProcess()
        {
            List<Models.ManageForm> ListLbl = new List<Models.ManageForm>();
            string sScript = string.Empty;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_KillSleepProcess");
                    int nRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL).FirstOrDefault<int>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GestForm.KillSleepProcess::errore::", ex);
            }
        }
    }
}