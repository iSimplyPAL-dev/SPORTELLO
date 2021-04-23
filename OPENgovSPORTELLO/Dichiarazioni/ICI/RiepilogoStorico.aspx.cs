using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using Newtonsoft.Json;

namespace OPENgovSPORTELLO.Dichiarazioni.ICI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RiepilogoStorico : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RiepilogoStorico));
        protected FunctionGrd FncGrd = new FunctionGrd();

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
                    List<RiepilogoUI> ListUIDich = new List<RiepilogoUI>();
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    if (MySession.Current.TipoStorico == "CAT")
                    {
                        ShowHide("divDich", false); ShowHide("divCat", true);
                    }
                    else {
                        if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadICIRiepilogoStorico(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListUIDich))
                            RegisterScript("Errore in caricamento pagina", this.GetType());
                        else {
                            GrdUI.DataSource = ListUIDich;
                            GrdUI.DataBind();
                            RegisterScript("document.getElementById('lblAggVertICI').innerText='" + MySession.Current.Ente.DatiVerticali.AnnoVerticaleICI.ToString() + "';", this.GetType());
                        }
                        ShowHide("divDich", true); ShowHide("divCat", false);
                    }
                }
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "RiepilogoStorico", "Page_Load", "ingresso pagina per " + MySession.Current.TipoStorico, General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.RiepilogoStorico.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            MySession.Current.TipoStorico = null;
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "RiepilogoStorico", "Back", "uscita pagina per " + MySession.Current.TipoStorico, General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoICI, Response);
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUIRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtn($(this).attr('id'));");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.RiepilogoStorico.GrdUIRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUIRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "UIOpen":
                        MySession.Current.TipoIstanza = Istanza.TIPO.ConsultaDich+" "+ MySession.Current.TipoStorico;
                        MySession.Current.IdRifCalcolo = IDRow;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "RiepilogoStorico", "UIOpen", "chiesto consultazione ui dichiarazione", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.RiepilogoStorico.GrdUIRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
    }
}