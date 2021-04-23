using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public partial class BO_SettingsBase : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_SettingsBase));

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
                ShowHide("divICI", false);
                ShowHide("divTASI", false);
                ShowHide("divTARSU", false);
                ShowHide("divOSAP", false);
                ShowHide("divICP", false);
                ShowHide("CfgSistema", false);
                ShowHide("CfgICI", false);
                ShowHide("CfgTASI", false);
                ShowHide("CfgTARSU", false);
                ShowHide("CfgOSAP", false);
                ShowHide("CfgICP", false);

                lnk01.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.Enti });
                lnk02.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.Stradario });
                lnk03.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.Operatori });
                lnk04.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.Documenti });
                lnk60.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Caratteristica });
                lnk61.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Utilizzo });
                lnk62.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Possesso });
                lnk63.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Categorie });
                lnk64.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Zone });
                lnk65.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Motivazioni });
                lnk66.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Vincoli });
                lnk67.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICI_Aliquote });
                lnk50.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TARSU_Categorie });
                lnk51.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TARSU_StatoOccupazione });
                lnk52.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TARSU_Riduzioni });
                lnk53.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TARSU_Esenzioni });
                lnk54.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TARSU_Motivazioni });
                lnk55.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TARSU_Vani });
                lnk74.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.OSAP_Richiedente });
                lnk75.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.OSAP_Categoria });
                lnk76.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.OSAP_Occupazione });
                lnk77.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.OSAP_Agevolazioni });
                lnk78.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.OSAP_Motivazioni });
                lnk92.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TASI_Agevolazioni });
                lnk93.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TASI_Aliquote });
                lnk94.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.TASI_Motivazioni });
                lnk80.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICP_Tipologia });
                lnk81.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICP_Caratteristica });
                lnk83.HRef = GetRouteUrl("Configurazioni", new { TOCONF = GenericCategory.TIPO.ICP_Motivazioni });
            }
            catch (Exception ex)
            {
                Log.Debug("BO_SettingsBase::Page_Init::errore::", ex);
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
                List<string> ListTribEnti = new List<string>();
                if (MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.Amministratore &&
                    MySession.Current.UserLogged.IDTipoProfilo != UserRole.PROFILO.ResponsabileEnte)

                    ShowHide("divSistema", false);
                else
                    ShowHide("divSistema", true);
                ListTribEnti = new BLL.Settings().LoadTributiGestiti(MySession.Current.UserLogged.NameUser);
                foreach (string myItem in ListTribEnti)
                {
                    if (myItem == General.TRIBUTO.ICI)
                        ShowHide("divICI", true);
                    else if (myItem == General.TRIBUTO.TASI)
                        ShowHide("divTASI", true);
                    else if (myItem == General.TRIBUTO.TARSU)
                        ShowHide("divTARSU", true);
                    else if (myItem == General.TRIBUTO.OSAP)
                        ShowHide("divOSAP", true);
                    else if (myItem == General.TRIBUTO.ICP)
                        ShowHide("divICP", true);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("BO_SettingsBase::Page_Load::errore::", ex);
                LoadException(ex);
            }
        }
    }
}