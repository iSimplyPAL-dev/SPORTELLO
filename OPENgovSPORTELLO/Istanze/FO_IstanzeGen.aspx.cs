using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Istanze
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class FO_IstanzeGen : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_IstanzeGen));
        protected FunctionGrd FncGrd = new FunctionGrd();
        /// <summary>
        ///  Inizializzazione della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                if (MySession.Current.Ente == null)
                {
                    sScript = "alert('Selezionare un’ente prima di poter accedere!');";
                    sScript += "window.location='" + UrlHelper.GetDefaultFO + "'";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_IstanzeGen.Page_Init::errore::", ex);
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
                if (!Page.IsPostBack)
                {
                    new General().ClearSession();
                    List<Istanza> ListIstanze = new List<Istanza>();
                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, DateTime.MaxValue, string.Empty, string.Empty, string.Empty, string.Empty, -1, -1, true, out ListIstanze))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                    else {
                        GrdIstanze.DataSource = ListIstanze;
                        GrdIstanze.DataBind();
                        RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    }
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "Page_Load", "ingresso pagina", "", "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_IstanzeGen::Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide("divRic", true); ShowHide("divDet", false);
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                RegisterScript("$('#FAQ').addClass('HelpFOIstanze');", this.GetType());
                if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E")
                {
                    ShowHide("bandieraGialla", false);
                    ShowHide("testoI", false);
                    RegisterScript("$('#testoI').removeClass('blink_slow');", this.GetType());
                }
                else
                {
                    ShowHide("testoE", false);
                    RegisterScript("$('#testoE').removeClass('blink_slow');", this.GetType());
                }
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            try
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "Back", "uscita pagina", "", "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.OSAP.Riepilogo.Back::errore::", ex);
                LoadException(ex);
            }
        }
        #region "Griglia"
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdIstanzeRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "RowOpen":                        
                        MySession.Current.IdIstanza = IDRow;
                        Log.Debug("apro istanza->" + IDRow.ToString());
                         foreach (GridViewRow myRow in GrdIstanze.Rows)
                        {
                            if (((HiddenField)myRow.FindControl("hfIDIstanza")).Value == IDRow.ToString())
                            {
                                if (((HiddenField)myRow.FindControl("hfTributo")).Value == General.TRIBUTO.ICI)
                                {
                                     IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
                                    break;
                                }
                                else if (((HiddenField)myRow.FindControl("hfTributo")).Value == General.TRIBUTO.TARSU)
                                {
                                     IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TARSU, null), Response);
                                    break;
                                }
                                else if (((HiddenField)myRow.FindControl("hfTributo")).Value == General.TRIBUTO.TASI)
                                {
                                    IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TASI, null), Response);
                                    break;
                                }
                                else if (((HiddenField)myRow.FindControl("hfTributo")).Value == General.TRIBUTO.OSAP)
                                {
                                    IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.OSAP, null), Response);
                                    break;
                                }
                                else if (((HiddenField)myRow.FindControl("hfTributo")).Value == General.TRIBUTO.ICP)
                                {
                                    IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICP, null), Response);
                                    break;
                                }
                                else if (((HiddenField)myRow.FindControl("hfTributo")).Value == "ANAG")
                                {
                                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
                                    break;
                                }
                                else if (((HiddenField)myRow.FindControl("hfTributo")).Value == "DELE")
                                {
                                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
                                    break;
                                }
                                else
                                {
                                    RegisterScript("$('#OnlyNumber_error').text('Funzionalità al momento non disponibile');$('#OnlyNumber_error').show();", this.GetType());
                                }
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "RowOpen", "consultazione istanza", "", "",MySession.Current.Ente.IDEnte);
                        break;
                    default:
                       break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.IstanzeFO.GrdIstanzeRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdIstanzeRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (MySettings.GetConfig("TypeProtocollo") != "E")
                    GrdIstanze.Columns[5].Visible = false;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtn($(this).attr('id'));");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.IstanzeFO.GrdIstanzeRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
     }
}