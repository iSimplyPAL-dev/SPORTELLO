using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Cruscotto
{
    /// <summary>
    /// Pagina lato BackOffice per la scelta dei report.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    /// <revisionHistory>
    /// <revision date="06/03/2019">
    /// Cancellazione registrazioni parziali
    /// </revision>
    /// </revisionHistory>
    public partial class BO_ReportGen : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_ReportGen));

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
                Log.Debug("BO_ReportGen.-");
                lnkComunicazioni.HRef = GetRouteUrl("Comunicazioni", null);
                Log.Debug("BO_ReportGen.-");
                lnkPWDToSend.HRef = GetRouteUrl("GestUser", new { TOGEST= (int)BLL.Analisi.TypeGestUser.GestPWD});
                Log.Debug("BO_ReportGen.-");
                lnkUserNoConfirmed.HRef = GetRouteUrl("GestUser", new { TOGEST = (int)BLL.Analisi.TypeGestUser.UtentiNonConfermati });
                Log.Debug("BO_ReportGen.-");

                List<EntiInLavorazione> ListGen = new BLL.EntiSistema(new EntiInLavorazione()).LoadEntiSistema(string.Empty, MySession.Current.UserLogged.NameUser);
                Log.Debug("BO_ReportGen.-");
                foreach (EntiInLavorazione myEnte in ListGen)
                {
                    Log.Debug("BO_ReportGen.-");
                    if (myEnte.Descrizione.ToLower() == MySession.Current.ComuneSito.ToLower())
                    {
                        Log.Debug("BO_ReportGen.-");
                        MySession.Current.Ente = myEnte;
                        break;
                    }
                }
                if (MySession.Current.Ente != null)
                {
                    Log.Debug("BO_ReportGen.-");
                    if (MySession.Current.Ente.SplitPWD == 0)//if(!bool.Parse(MySettings.GetConfig("SplitPWD")))
                        RegisterScript("$('#CfgPWDToSend').hide();", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("BO_ReportGen::Page_Init::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                MySession.Current.ParamRicIstanze = null;
            }
            catch (Exception ex)
            {
                Log.Debug("BO_ReportGen::Page_Load::errore::", ex);
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultBO, Response);
        }

    }
}