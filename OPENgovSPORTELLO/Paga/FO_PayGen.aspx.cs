using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Paga
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FO_PayGen : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_PayGen));
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
                ShowHide("lblResultDovuto", false);
                string sScript = new BLL.GestForm().GetLabel("FO_PayGen", "");
                RegisterScript(sScript, this.GetType());
                 if (MySession.Current.Ente == null)
                {
                    sScript = "alert('Selezionare un’ente prima di poter accedere!');";
                    sScript += "window.location='" + UrlHelper.GetDefaultFO + "'";
                    RegisterScript(sScript, this.GetType());
                }
                else { 
               RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());}
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string EsitoPagamento = "OK";
            try
            {
                if (!Page.IsPostBack)
                {
                    string TokenAuth = string.Empty;
                    clsPagoPA.TokenValidation ValidToken = new clsPagoPA.TokenValidation();
                    if (Request.QueryString["Token"] != null)
                        TokenAuth = Request.QueryString["Token"];
                    if (TokenAuth != string.Empty)
                        ValidToken = new clsPagoPA().ValidateTokenGWPag(TokenAuth, MySession.Current.Ente.DatiPagoPA.HScadenza, out EsitoPagamento);

                    if (ValidToken.Validated)
                    {
                        PagamentoPendenza myPaymentCart = GetPagamentoPendenza();
                        if (EsitoPagamento == "OK")
                        {
                            if (myPaymentCart.IdPendenza != string.Empty)
                            {
                                string sScript = new clsPagoPA().VerificaStatoPendenza(MySession.Current.Ente, MySession.Current.UserLogged, myPaymentCart);
                                if (sScript != string.Empty)
                                {
                                    RegisterScript(sScript, this.GetType());
                                }
                                else
                                {
                                    sScript = "$('#OnlyNumber_error').text('Errore in verifica pagamento!');$('#OnlyNumber_error').show();";
                                    RegisterScript(sScript, this.GetType());
                                }
                                MySession.Current.TotShoppingCart = 0;
                                MySession.Current.ListShoppingCart = new List<string>();
                                LoadCart();
                            }
                        }
                        else if (EsitoPagamento == "KO")
                        {
                            new BLL.PagoPA(myPaymentCart).ClearTmpPendenze();
                            string sScript = "$('#OnlyNumber_error').text('Pagamento annullato!');$('#OnlyNumber_error').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                        LoadToPay();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "PagoPA", "", "Page_Load", "ingresso pagina", "", "", MySession.Current.Ente.IDEnte);
                    }
                    else
                    {
                        string sScript = "$('#OnlyNumber_error').text('Tentativo di accesso non valido!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetLoginFO, Response);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                RegisterScript("$('#lblTotCart').text('Tot. Carrello " + MySession.Current.TotShoppingCart.ToString("#,##0.00") + " €');", this.GetType());
            }
        }
        #region "Bottoni"
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            try
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "PagoPA", "", "Back", "uscita pagina", "", "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.Back::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Paga(object sender, EventArgs e)
        {
            string myUrlGW = string.Empty;
            string sErr = string.Empty;

            if (MySession.Current.TotShoppingCart == 0)
            {
                string sScript = "$('#OnlyNumber_error').text('Carrello vuoto impossibile proseguire con il pagamento!');$('#OnlyNumber_error').show();";
                RegisterScript(sScript, this.GetType());
            }
            else
            {
                try
                {
                    PagamentoPendenza myPagamento = new PagamentoPendenza();
                    string UrlBack = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + UrlHelper.GetPaga + "?Token=" + new clsPagoPA().GetTokenGWPag(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser, MySession.Current.UserLogged.IDContribLogged.ToString(), MySession.Current.UserLogged.IDContribToWork.ToString(), "OK");
                    string UrlCancel = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + UrlHelper.GetPaga + "?Token=" + new clsPagoPA().GetTokenGWPag(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser, MySession.Current.UserLogged.IDContribLogged.ToString(), MySession.Current.UserLogged.IDContribToWork.ToString(), "KO");
                    if (new clsPagoPA().MakePayment(MySession.Current.Ente, MySession.Current.myAnag, General.TRIBUTO.OSAP, MySession.Current.UserLogged.IDContribToWork, MySession.Current.ListShoppingCart, UrlBack, UrlCancel, out myPagamento, out myUrlGW, out sErr))
                    {
                        string sScript = "$('#OnlyNumber_error').removeClass('text-danger');$('#OnlyNumber_error').addClass('text-success');$('#OnlyNumber_error').text('Pagamento in corso!');$('#OnlyNumber_error').show();";
                        sScript += "location.href='" + myUrlGW + "';";
                        RegisterScript(sScript, this.GetType());
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "PagoPA", "", "Paga", "pagamento in corso", "", "", MySession.Current.Ente.IDEnte);
                    }
                    else
                    {
                        string sScript = string.Empty;
                        if (sErr != string.Empty)
                        {
                            sScript = "$('#OnlyNumber_error').text('" + sErr + "!');$('#OnlyNumber_error').show();";
                        }
                        else
                        {
                            sScript = "$('#OnlyNumber_error').text('Pagamento annullato a causa di errori!');$('#OnlyNumber_error').show();";
                        }
                        RegisterScript(sScript, this.GetType());
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("OPENgovSPORTELLO.FO_PayGen.Back::errore::", ex);
                    LoadException(ex);
                }
                finally
                {
                    LoadToPay();
                }
            }
        }
        #endregion
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDovutoRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.GrdDovutoRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDovutoRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "RowAdd":
                        bool IsToInsert = true;
                        foreach (string myString in MySession.Current.ListShoppingCart)
                        {
                            if (myString == IDRow)
                                IsToInsert = false;
                        }
                        if (IsToInsert)
                        {//posso avere una sola posizione nel carrello
                            MySession.Current.TotShoppingCart = 0;
                            MySession.Current.ListShoppingCart = new List<string>();

                            MySession.Current.ListShoppingCart.Add(IDRow);
                            foreach (GridViewRow myRow in GrdDovuto.Rows)
                            {
                                if (((HiddenField)myRow.FindControl("hfId")).Value == e.CommandArgument.ToString())
                                {
                                    MySession.Current.TotShoppingCart += decimal.Parse(((HiddenField)myRow.FindControl("hfDebito")).Value);
                                }
                            }
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "PagoPA", "", "RowAdd", "aggiungo al carrello", "", "", MySession.Current.Ente.IDEnte);
                        }
                        break;
                    case "RowDel":
                        List<string> ListNew = new List<string>();
                        foreach (string myToPay in MySession.Current.ListShoppingCart)
                        {
                            if (myToPay != IDRow)
                            {
                                ListNew.Add(IDRow);
                                foreach (GridViewRow myRow in GrdDovuto.Rows)
                                {
                                    if (((HiddenField)myRow.FindControl("hfId")).Value == e.CommandArgument.ToString())
                                    {
                                        MySession.Current.TotShoppingCart -= decimal.Parse(((HiddenField)myRow.FindControl("hfDebito")).Value);
                                    }
                                }
                            }
                        }
                        MySession.Current.ListShoppingCart = ListNew;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "PagoPA", "", "RowDel", "tolgo al carrello", "", "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.GrdDovutoRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                LoadCart();
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        private void LoadToPay()
        {
            try
            {
                List<Dovuto> ListDovuto = new List<Dovuto>();

                if (!new BLL.PagoPA(new PagamentoPendenza()).LoadDovuto(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListDovuto))
                    RegisterScript("Errore in caricamento pagina", this.GetType());
                else {
                    if (ListDovuto.Count > 0)
                    {
                        GrdDovuto.DataSource = ListDovuto;
                        GrdDovuto.DataBind();
                        ShowHide("lblResultDovuto", false);
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDovuto", true);
                    }
                    else
                    {
                        ShowHide("lblResultDovuto", true);
                        ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDovuto", false);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.LoadToPay::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadCart()
        {
            try
            {
                string sScript = string.Empty;
                foreach (string myItem in MySession.Current.ListShoppingCart)
                {
                    sScript += "<li>Avviso n." + myItem + "</li>";
                }
                sScript = "<ul>" + sScript + "</ul>";
                RegisterScript("$('#lblListCart').html('" + sScript + "');", this.GetType());
                RegisterScript("$('#lblTotCart').html('Tot. Carrello " + MySession.Current.TotShoppingCart.ToString("#,##0.00") + " €');", this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.LoadCart::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private PagamentoPendenza GetPagamentoPendenza()
        {
            PagamentoPendenza myPagPendenza = new PagamentoPendenza();
            try
            {
                myPagPendenza = new BLL.PagoPA(new PagamentoPendenza()).LoadTmpPendenze(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.LoadPagamentoPendenza::errore::", ex);
                LoadException(ex);
            }
            return myPagPendenza;
        }
    }
}