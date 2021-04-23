using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using System.Configuration;
using log4net;
using System.Collections.Specialized;

namespace OPENgovSPORTELLO
{    /// <summary>
     /// Classe per la gestione dei reindirizzamenti.
     /// </summary>
     /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
     /// <revisionHistory>
     /// <revision date="06/03/2019">
     /// Cancellazione registrazioni parziali
     /// </revision>
     /// </revisionHistory>
    public static class RouteConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routes"></param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapPageRoute("Configurazioni", "Configurazioni/{TOCONF}/", "~/Settings/MngSettings.aspx");
            routes.MapPageRoute("Comunicazioni", "Comunicazioni", "~/Settings/MngMessages.aspx");
            routes.MapPageRoute("GestUser", "GestUser/{TOGEST}/", "~/Cruscotto/BO_GestPWD.aspx");

            routes.MapPageRoute("Tributo"+General.TRIBUTO.ICI, "Tributo" + General.TRIBUTO.ICI, "~/Dichiarazioni/ICI/Riepilogo.aspx");
            routes.MapPageRoute("Tributo" + General.TRIBUTO.TASI, "Tributo" + General.TRIBUTO.TASI, "~/Dichiarazioni/TASI/Riepilogo.aspx");
            routes.MapPageRoute("Tributo"+General.TRIBUTO.TARSU, "Tributo" + General.TRIBUTO.TARSU, "~/Dichiarazioni/TARSU/Riepilogo.aspx");
            routes.MapPageRoute("Tributo" + General.TRIBUTO.OSAP, "Tributo" + General.TRIBUTO.OSAP, "~/Dichiarazioni/OSAP/Riepilogo.aspx");
            routes.MapPageRoute("Tributo" + General.TRIBUTO.ICP, "Tributo" + General.TRIBUTO.ICP, "~/Dichiarazioni/ICP/Riepilogo.aspx");
            routes.MapPageRoute("Tributo" + General.TRIBUTO.TESSERE, "Tributo" + General.TRIBUTO.TESSERE, "~/Dichiarazioni/TESSERE/Riepilogo.aspx");
            routes.MapPageRoute("Tributo" + General.TRIBUTO.PROVVEDIMENTI, "Tributo" + General.TRIBUTO.PROVVEDIMENTI, "~/Dichiarazioni/PROVVEDIMENTI/Riepilogo.aspx");

            routes.MapPageRoute("Immobile" + General.TRIBUTO.ICI, "Immobile" + General.TRIBUTO.ICI, "~/Dichiarazioni/ICI/Dich.aspx");
            routes.MapPageRoute("Immobile" + General.TRIBUTO.TASI, "Immobile" + General.TRIBUTO.TASI, "~/Dichiarazioni/TASI/Dich.aspx");
            routes.MapPageRoute("Immobile" + General.TRIBUTO.TARSU, "Immobile" + General.TRIBUTO.TARSU, "~/Dichiarazioni/TARSU/Dich.aspx");
            routes.MapPageRoute("Immobile" + General.TRIBUTO.OSAP, "Immobile" + General.TRIBUTO.OSAP, "~/Dichiarazioni/OSAP/Dich.aspx");
            routes.MapPageRoute("Immobile" + General.TRIBUTO.ICP, "Immobile" + General.TRIBUTO.ICP, "~/Dichiarazioni/ICP/Dich.aspx");
            routes.MapPageRoute("Immobile" + General.TRIBUTO.TESSERE, "Immobile" + General.TRIBUTO.TESSERE, "~/Dichiarazioni/TESSERE/Dich.aspx");
            routes.MapPageRoute("Immobile" + General.TRIBUTO.PROVVEDIMENTI, "Immobile" + General.TRIBUTO.PROVVEDIMENTI, "~/Dichiarazioni/PROVVEDIMENTI/Dettaglio.aspx");
        }
        /// <summary>
        /// 
        /// </summary>
        public static string TypeDB
        {
            get
            {
                if (ConfigurationManager.AppSettings["TypeDB"] != null)
                    return ConfigurationManager.AppSettings["TypeDB"].ToString();
                else
                    return "SQL";
            }
        }
        #region "Stringhe di connessione"
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionAnagrafica
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["AnagraficaContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["AnagraficaContext"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionStradario
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["StradarioContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["StradarioContext"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionICI
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["ICIContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["ICIContext"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string StringConnectionTARSU
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings["TARSUContext"] != null)
                {
                    return ConfigurationManager.ConnectionStrings["TARSUContext"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione Helper
    /// </summary>
    public class UrlHelper
    {
        #region "Param INI"
        /// <summary>
        /// 
        /// </summary>
        public static string GetPortSite
        {
            get
            {
                if (ConfigurationManager.AppSettings["portsite"] != null)
                {
                    return ConfigurationManager.AppSettings["portsite"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPathSite
        {
            get
            {
                if (ConfigurationManager.AppSettings["pathsite"] != null)
                {
                    return ConfigurationManager.AppSettings["pathsite"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPathDichiarazioni
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathDichiarazioni"] != null)
                {
                    return ConfigurationManager.AppSettings["PathDichiarazioni"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPathWebDichiarazioni
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathWebDichiarazioni"] != null)
                {
                    return ConfigurationManager.AppSettings["PathWebDichiarazioni"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPathAttachments
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathAttachments"] != null)
                {
                    return ConfigurationManager.AppSettings["PathAttachments"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPathLogo
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathLogo"] != null)
                {
                    return ConfigurationManager.AppSettings["PathLogo"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPathFlussiCarico
        {
            get
            {
                if (ConfigurationManager.AppSettings["PathFlussiCarico"] != null)
                {
                    return ConfigurationManager.AppSettings["PathFlussiCarico"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static bool GetVisualCatasto
        {
            get
            {
                if (ConfigurationManager.AppSettings["VisualCatasto"] != null)
                {
                    return bool.Parse(ConfigurationManager.AppSettings["VisualCatasto"].ToString());
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static int PasswordExpireDays
        {
            get
            {
                if (ConfigurationManager.AppSettings["PasswordExpireDays"] != null)
                {
                    return int.Parse(ConfigurationManager.AppSettings["PasswordExpireDays"]);
                }
                else
                {
                    return 90;
                }
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        public static string GetFOTributi
        {
            get { return GetPathSite + "/Dichiarazioni/FO_Base.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoTARSU
        {
            get { return GetPathSite + "/Dichiarazioni/TARSU/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoICI
        {
            get { return GetPathSite + "/Dichiarazioni/ICI/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoTASI
        {
            get { return GetPathSite + "/Dichiarazioni/TASI/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoOSAP
        {
            get { return GetPathSite + "/Dichiarazioni/OSAP/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoICP
        {
            get { return GetPathSite + "/Dichiarazioni/ICP/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoTESSERE
        {
            get { return GetPathSite + "/Dichiarazioni/TESSERE/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestRiepilogoPROVVEDIMENTI
        {
            get { return GetPathSite + "/Dichiarazioni/PROVVEDIMENTI/Riepilogo.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetGestDichiarazione
        {
            get { return GetPathSite + "/Istanze/FO_GestDich.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetDefaultBO
        {
            get { return GetPathSite + "/DefaultBO.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetDefaultFO
        {
            get { return GetPathSite + "/DefaultFO.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetEmailConfirmation
        {
            get { return GetPathSite + "/Account/EmailConfirmation.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetLoginFO
        {
            get { return GetPathSite + "/Account/LoginFO.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetLoginBO
        {
            get { return GetPathSite + "/Account/LoginBO.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetRegister
        {
            get { return GetPathSite + "/Account/RegisterFO.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetProfiloFO
        {
            get { return GetPathSite + "/Account/ProfiloFO.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetBO_IstanzeRibalta
        {
            get { return GetPathSite + "/Istanze/BO_IstanzeDet.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetBO_IstanzeGen
        {
            get { return GetPathSite + "/Istanze/BO_IstanzeGen.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetFO_IstanzeGen
        {
            get { return GetPathSite + "/Istanze/FO_IstanzeGen.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetRepositoryPDF
        {
            get
            {
                if (ConfigurationManager.AppSettings["pathRepositoryPDF"] != null)
                {
                    return ConfigurationManager.AppSettings["pathRepositoryPDF"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetRepositoryPDFUrl
        {
            get
            {
                if (ConfigurationManager.AppSettings["pathRepositoryPDFUrl"] != null)
                {
                    return ConfigurationManager.AppSettings["pathRepositoryPDFUrl"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetBOSettingsBase
        {
            get { return GetPathSite + "/Settings/BO_SettingsBase.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetStoricoICI
        {
            get { return GetPathSite + "/Dichiarazioni/ICI/RiepilogoStorico.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetStoricoTARSU
        {
            get { return GetPathSite + "/Dichiarazioni/TARSU/RiepilogoStorico.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetBO_ReportGen
        {
            get { return GetPathSite + "/Cruscotto/BO_ReportGen.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetFO_ReportGen
        {
            get { return GetPathSite + "/Cruscotto/FO_ReportGen.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetBO_AnalisiEventi
        {
            get { return GetPathSite + "/Cruscotto/Analisi/BO_AnalisiEventi.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetSelDelegante
        {
            get { return GetPathSite + "/Account/SelDelegante.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetRavvedimento
        {
            get { return GetPathSite + "/Dichiarazioni/ICI/Ravvedimento.aspx"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetUrlVerticale
        {
            get
            {
                if (ConfigurationManager.AppSettings["UrlVerticale"] != null)
                {
                    return ConfigurationManager.AppSettings["UrlVerticale"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetUrlVerticaleICI
        {
            get
            {
                if (ConfigurationManager.AppSettings["UrlVerticaleICI"] != null)
                {
                    return ConfigurationManager.AppSettings["UrlVerticaleICI"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetUrlVerticaleTARSU
        {
            get
            {
                if (ConfigurationManager.AppSettings["UrlVerticaleTARSU"] != null)
                {
                    return ConfigurationManager.AppSettings["UrlVerticaleTARSU"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string GetPaga
        {
            get { return GetPathSite + "/Paga/FO_PayGen.aspx"; }
        }
    }
    /// <summary>
    /// Classe di gestione parametri report
    /// </summary>
    public class MySettingsReport : ConfigurationSection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MySettingsReport));
        /// <summary>
        /// 
        /// </summary>
        public class EstensioneReport
        {
            public static string HTML = "HTML";
            public static string PDF = "PDF";
            public static string Excel = "XLS";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MySettingsReport GetConfig()
        {
            return (MySettingsReport)ConfigurationManager.GetSection("MySettingsReport");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public static string GetConfig(string Param)
        {
            try
            {
                NameValueCollection myKey = ConfigurationManager.GetSection("MySettingsReport") as NameValueCollection;
                return myKey[Param].ToString();
            }
            catch (Exception ex)
            {
                Log.Debug("MySettingsReport.GetConfig(" + Param + ")::errore::", ex);
                return string.Empty;
            }
        }
    }
    /// <summary>
    /// Classe di gestione config
    /// </summary>
    public class MySettings : ConfigurationSection
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MySettings));
        /// <summary>
        /// 
        /// </summary>
        public class EstensioneReport
        {
            public static string HTML = "HTML";
            public static string PDF = "PDF";
            public static string Excel = "XLS";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MySettings GetConfig()
        {
            return (MySettings)ConfigurationManager.GetSection("appSettings");

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public static string GetConfig(string Param)
        {
            try
            {
                NameValueCollection myKey = ConfigurationManager.GetSection("appSettings") as NameValueCollection;
                return myKey[Param].ToString();
            }
            catch (Exception ex)
            {
                Log.Debug("MySettings.GetConfig(" + Param + ")::errore::", ex);
                return string.Empty;
            }
        }
    }
}
