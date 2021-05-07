using log4net;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OPENgovSPORTELLO.Istanze
{
    /// <summary>
    /// L’operatore di back office deve visionare, validare le istanze presentate dai contribuenti, quindi procedere con l’inserimento manuale dei dati nel verticale dell’Ente.
    /// All’operatore di back-office il sistema presenta un’interfaccia in cui sono riassunte tutte le istanze /dichiarazioni proposte dai contribuenti attraverso il front-end.
    /// Sulla base dei criteri di selezione impostati, il sistema visualizza i dati ordinati per contribuente/data.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_IstanzeGen : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_IstanzeGen));
        private BLL.Settings fncMng = new BLL.Settings();
        General fncGen = new General();
        protected FunctionGrd FncGrd = new FunctionGrd();

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
                if (MySession.Current.UserLogged.IDTipoProfilo == UserRole.PROFILO.Amministratore || MySession.Current.UserLogged.IDTipoProfilo == UserRole.PROFILO.ResponsabileEnte)
                    RegisterScript("$('.BottoneDatabase').show();", this.GetType());
                else
                    RegisterScript("$('.BottoneDatabase').hide();", this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BO_IstanzeGen.Page_Init::errore::", ex);
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
                if (Page.IsPostBack == false)
                {
                    List<GenericCategory> ListGenEnti = new BLL.User(new UserRole() { NameUser = MySession.Current.UserLogged.NameUser }).LoadUserEnti("", MySession.Current.UserLogged.NameUser);
                    fncGen.LoadCombo(ddlEnte, ListGenEnti, "CODICE", "DESCRIZIONE");
                    if (ListGenEnti.Count == 1)
                    {
                        ddlEnte.SelectedValue = ListGenEnti[0].Codice;
                        ddlEnte.Enabled = false;
                        MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                    }
                    List<GenericCategory> ListGenTributi = fncMng.LoadTributi(0);
                    ListGenTributi.Add(new GenericCategory { Codice = "ANAG", Descrizione = "Anagrafica" });
                    ListGenTributi.Add(new GenericCategory { Codice = "DELE", Descrizione = "Delega" });
                    ListGenTributi = ListGenTributi.OrderBy(order => order.Descrizione).ToList();
                    fncGen.LoadCombo(ddlTributo, ListGenTributi, "CODICE", "DESCRIZIONE");

                    List<GenericCategory> ListGenStatoIstanze = fncMng.LoadStatoIstanze();
                    fncGen.LoadCombo(ddlStatoIstanze, ListGenStatoIstanze, "CODICE", "DESCRIZIONE");
                    if (MySession.Current.ParamRicIstanze != null)
                    {
                        List<string> listParam = (List<string>)MySession.Current.ParamRicIstanze;
                        if (listParam.Count > 0)
                        {
                            ddlEnte.SelectedValue = listParam[0];
                            txtDataPresentazione.Text = listParam[1];
                            ddlTributo.SelectedValue = listParam[2];
                            ddlStatoIstanze.SelectedValue = listParam[3];
                            txtNominativo.Text = listParam[4];
                            txtCFPIVA.Text = listParam[5];
                        }
                        List<Istanza> listDati = new List<Istanza>();
                        DateTime data = DateTime.MaxValue;
                        new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze(ddlEnte.SelectedValue, 0, ((txtDataPresentazione.Text != "") ? DateTime.Parse(txtDataPresentazione.Text) : DateTime.MaxValue), ddlTributo.SelectedValue, ddlStatoIstanze.SelectedValue, txtNominativo.Text, txtCFPIVA.Text, -1, -1, true, out listDati);
                        GrdResult.DataSource = listDati;
                        GrdResult.DataBind();
                        string sScript = string.Empty;
                        if (listDati.Count > 0)
                        {
                            sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult').show();";
                            sScript += "$('#lblResult').text('');";
                        }
                        else
                        {
                            sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult').hide();";
                            sScript += "$('#lblResult').text('Nessuna istanza trovata.');";
                        }
                        RegisterScript(sScript, this.GetType());
                    }
                }
                if (ddlEnte.SelectedValue != string.Empty)
                {
                    MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                }

                RegisterScript("$('#hdDescrEnte').val('" + MySession.Current.Ente.Ambiente + "');", typeof(Page));

                RegisterScript("$('.divGrdBtn').hide();", typeof(Page));
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "Page_Load", "ingresso pagina", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeGen.Page_Load::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search(object sender, EventArgs e)
        {
            List<Istanza> listDati = new List<Istanza>();
            DateTime data = DateTime.MaxValue;
            string sScript = string.Empty;

            if (txtDataPresentazione.Text != "")
            {
                data = DateTime.Parse(txtDataPresentazione.Text);
            }

            try
            {
                new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze(ddlEnte.SelectedValue, 0, data, ddlTributo.SelectedValue, ddlStatoIstanze.SelectedValue, txtNominativo.Text, txtCFPIVA.Text, -1, -1, true, out listDati);
                GrdResult.DataSource = listDati;
                GrdResult.DataBind();
                if (listDati.Count > 0)
                {
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult').show();";
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult_GrdResult').show();";
                    sScript += "$('#lblResult').text('');";
                }
                else
                {
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult').hide();";
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult_GrdResult').hide();";
                    sScript += "$('#lblResult').text('Nessuna istanza trovata.');";
                }
                RegisterScript(sScript, this.GetType());
                List<string> listParam = new List<string>();
                listParam.Add(ddlEnte.SelectedValue);
                listParam.Add(txtDataPresentazione.Text);
                listParam.Add(ddlTributo.SelectedValue);
                listParam.Add(ddlStatoIstanze.SelectedValue);
                listParam.Add(txtNominativo.Text);
                listParam.Add(txtCFPIVA.Text);
                MySession.Current.ParamRicIstanze = listParam;
                MySession.Current.GestIstanze = listDati;

                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "Search", "ricerca istanze", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeGen.Search:errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultBO, Response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PagamentiToVerticale(object sender, EventArgs e)
        {
            string sScript = string.Empty;

            try
            {
                if (ddlEnte.SelectedValue != string.Empty)
                {
                    if (new Paga.clsPagoPA().PaymentToVerticale(ddlEnte.SelectedValue))
                    {
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Ribaltamento pagamenti in Verticale", "PagamentiToVerticale", "pagamenti into verticale", "", "", "");
                        sScript += "$('#lblResult').text('Pagamenti ribaltati nel Verticale con successo.');";
                    }
                    else
                    {
                        sScript += "$('#lblErrorBO').text('Errore nel ribaltamento dei pagamenti nel Verticale.');$('#lblErrorBO').show();";
                    }
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult').hide();";
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_GrdResult_GrdResult').hide();";
                }
                else
                {
                    sScript += "$('#lblErrorBO').text('Selezionare un ente.');$('#lblErrorBO').show();";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeGen.PagamentiToVerticale:errore::", ex);
                LoadException(ex);
            }
        }
        #region "Griglia"
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdResultRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "UIOpen":
                        MySession.Current.IdIstanza = IDRow;
                        foreach (GridViewRow myRow in GrdResult.Rows)
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
                                    RegisterScript("$('#lblErrorBO').text('Funzionalità al momento non disponibile');$('#lblErrorBO').show();", this.GetType());
                                }
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "", "RowOpen", "consulta istanza", "", "", "");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeGen.GrdResultRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdResultRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeGen.GrdResultRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione dell'ordinamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdResultRowSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<Istanza> ListSorted = MySession.Current.GestIstanze;
                switch (e.SortExpression)
                {
                    case "Nominativo":
                        ListSorted = ListSorted.OrderBy(order => order.Nominativo).ToList();
                        break;
                    case "CodFiscalePIVA":
                        ListSorted = ListSorted.OrderBy(order => order.CodFiscalePIVA).ToList();
                        break;
                    case "DescrTributo":
                        ListSorted = ListSorted.OrderBy(order => order.DescrTributo).ToList();
                        break;
                }
                if (MySession.Current.SortDirection == SortDirection.Descending)
                    MySession.Current.SortDirection = SortDirection.Ascending;
                else
                    MySession.Current.SortDirection = SortDirection.Descending;

                if (MySession.Current.SortDirection == SortDirection.Descending)
                    ListSorted.Reverse();
                GrdResult.DataSource = ListSorted;
                GrdResult.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeGen.GrdResultRowSorting::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ControlSelectedChanged(object sender, EventArgs e)
        {
            try
            {
                MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.ControlSelectedChanged::errore::", ex);
                LoadException(ex);
            }
        }
    }
}
