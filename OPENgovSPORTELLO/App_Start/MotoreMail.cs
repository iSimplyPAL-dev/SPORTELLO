using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace My
{
    /// <summary>
    /// Classe di gestione parametri configurazione da file
    /// </summary>
    sealed class MySettings : System.Configuration.ApplicationSettingsBase
    {
        private static MySettings defaultInstance = ((MySettings)(System.Configuration.ApplicationSettingsBase.Synchronized(new MySettings())));
        /// <summary>
        /// 
        /// </summary>
        public static MySettings Default
        {
            get
            {
                return defaultInstance;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string pathfileconflog4net
        {
            get
            {
                if (ConfigurationManager.AppSettings["pathfileconflog4net"] != null)
                {
                    return ConfigurationManager.AppSettings["pathfileconflog4net"].ToString();
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
        [System.Configuration.ApplicationScopedSettingAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Configuration.DefaultSettingValueAttribute("60")]
        public int CheckDelayTime
        {
            get
            {
                return ((int)(this["CheckDelayTime"]));
            }
        }

        [System.Configuration.ApplicationScopedSettingAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Configuration.DefaultSettingValueAttribute("4")]
        public int SendDay
        {
            get
            {
                return ((int)(this["SendDay"]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DataDB
        {
            get
            {
                return ((string)(this["DataDB"]));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [System.Configuration.ApplicationScopedSettingAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Configuration.DefaultSettingValueAttribute("smtp.gmail.com")]
        public static string mailServer
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailServer"] != null)
                {
                    return ConfigurationManager.AppSettings["mailServer"].ToString();
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
        public static string mailServerPort
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailServerPort"] != null)
                {
                    return ConfigurationManager.AppSettings["mailServerPort"].ToString();
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
        public static string mailSender
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailSender"] != null)
                {
                    return ConfigurationManager.AppSettings["mailSender"].ToString();
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
        public static string mailSenderName
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailSenderName"] != null)
                {
                    return ConfigurationManager.AppSettings["mailSenderName"].ToString();
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
        public static string mailUser
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailUser"] != null)
                {
                    return ConfigurationManager.AppSettings["mailUser"].ToString();
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
        public static string mailPassword
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailPassword"] != null)
                {
                    return ConfigurationManager.AppSettings["mailPassword"].ToString();
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
        public static bool mailSSL
        {


            get
            {

                if (ConfigurationManager.AppSettings["mailSSL"] != null)
                {
                    return bool.Parse(ConfigurationManager.AppSettings["mailSSL"].ToString());
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
        public static string mailCC
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailCC"] != null)
                {
                    return ConfigurationManager.AppSettings["mailCC"].ToString();
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
        public static string MailWarningMessage
        {
            get
            {
                if (ConfigurationManager.AppSettings["MailWarningMessage"] != null)
                {
                    return ConfigurationManager.AppSettings["MailWarningMessage"].ToString();
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
        public static string MailWarningSubject
        {
            get
            {
                if (ConfigurationManager.AppSettings["MailWarningSubject"] != null)
                {
                    return ConfigurationManager.AppSettings["MailWarningSubject"].ToString();
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
        public static string MailWarningRecipient
        {
            get
            {
                if (ConfigurationManager.AppSettings["MailWarningRecipient"] != null)
                {
                    return ConfigurationManager.AppSettings["MailWarningRecipient"].ToString();
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
        public static string MailSendErrorMessage
        {
            get
            {
                if (ConfigurationManager.AppSettings["MailSendErrorMessage"] != null)
                {
                    return ConfigurationManager.AppSettings["MailSendErrorMessage"].ToString();
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
        public static string MailBackOffice
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailBackOffice"] != null)
                {
                    return ConfigurationManager.AppSettings["mailBackOffice"].ToString();
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
        public static string MailArchive
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailArchive"] != null)
                {
                    return ConfigurationManager.AppSettings["mailArchive"].ToString();
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
        public static string MailProtocollo
        {
            get
            {
                if (ConfigurationManager.AppSettings["mailProtocollo"] != null)
                {
                    return ConfigurationManager.AppSettings["mailProtocollo"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}

namespace My
{
    /// <summary>
    /// Classe di gestione proprietà configurazione
    /// </summary>
    [System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class MySettingsProperty
    {

        [System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")]
        internal My.MySettings Settings
        {
            get
            {
                return My.MySettings.Default;
            }
        }
    }
}