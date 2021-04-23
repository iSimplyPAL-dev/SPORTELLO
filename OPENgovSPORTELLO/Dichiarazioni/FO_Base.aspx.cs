using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.Data.SqlClient;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Dichiarazioni
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FO_Base : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_Base));
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
                if (MySession.Current.Ente == null)
                {
                    string sScript = "alert('Selezionare un’ente prima di poter accedere!');";
                    sScript += "window.location='" + UrlHelper.GetDefaultFO + "'";
                    RegisterScript(sScript, this.GetType());
                }
                else { 
                lnk8852Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICI, null);
                lnk8852Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICI, null);
                lnkTASIBtn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TASI, null);
                lnkTASIDescr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TASI, null);
                lnk0434Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TARSU, null);
                lnk0434Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TARSU, null);
                lnkTESSBtn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TESSERE, null);
                lnkTESSDescr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TESSERE, null);
                lnk0453Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.OSAP, null);
                lnk0453Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.OSAP, null);
                lnk9763Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICP, null);
                lnk9763Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICP, null);
                lnk9999Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.PROVVEDIMENTI, null);
                lnk9999Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.PROVVEDIMENTI, null);
                string sScript = new BLL.GestForm().GetHelp("HelpFOTributi", MySession.Current.Ente.UrlWiki);
                RegisterScript(sScript, this.GetType());}
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.Page_Init::errore::", ex);
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
                new General().ClearSession();
                if (MySession.Current.Ente == null)
                {
                    string sScript = "alert('Selezionare un’ente prima di poter accedere!');";
                    sScript += "window.location='" + UrlHelper.GetDefaultFO + "'";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    lnk8852Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICI, null);
                    lnk8852Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICI, null);
                    lnkTASIBtn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TASI, null);
                    lnkTASIDescr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TASI, null);
                    lnk0434Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TARSU, null);
                    lnk0434Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TARSU, null);
                    lnkTESSBtn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TESSERE, null);
                    lnkTESSDescr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.TESSERE, null);
                    lnk0453Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.OSAP, null);
                    lnk0453Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.OSAP, null);
                    lnk9763Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICP, null);
                    lnk9763Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.ICP, null);
                    lnk9999Btn.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.PROVVEDIMENTI, null);
                    lnk9999Descr.HRef = GetRouteUrl("Tributo" + General.TRIBUTO.PROVVEDIMENTI, null);
                    string sScript = new BLL.GestForm().GetHelp("HelpFOTributi", MySession.Current.Ente.UrlWiki);
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.Page_Init::errore::", ex);
            }
            try
            {
                if (!Page.IsPostBack)
                {
                    List<Istanza> ListIstanze = new List<Istanza>();
                    RiepilogoDebito myRiepDeb = new RiepilogoDebito();
                    List<GenericCategory> ListComunicazioni = new List<GenericCategory>();
                    Log.Debug("Page_Load FO_Base");
                    Log.Debug("LoadRiepilogoDovuto::IDEnte_" + MySession.Current.Ente.IDEnte.ToString() + " IDUserLogged_" + MySession.Current.UserLogged.ID.ToString() +" IDContribToWork_" + MySession.Current.UserLogged.IDContribToWork.ToString());
                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadRiepilogoDovuto(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListIstanze, out myRiepDeb, out ListComunicazioni))
                    {
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                        Log.Debug("Errore in caricamento pagina");
                    }
                    else {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.Page_Load::ListIstanze::lunga:_" + ListIstanze.Count.ToString());
                        if (ListIstanze.Count > 0)
                        {
                            Log.Debug("Ordino lista istanze");
                            ListIstanze = ListIstanze.OrderBy(o => o.DataPresentazione).ToList();
                            ListIstanze.Reverse();
                            GrdDichiarazioni.DataSource = ListIstanze.Take(5).ToList();
                            GrdDichiarazioni.DataBind();
                        }
                        else
                        {
                            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDichiarazioni", false);
                        }
                        string sScriptNews = string.Empty;
                         foreach (GenericCategory myNews in ListComunicazioni)
                        {
                                  List<string> ListVal = myNews.Descrizione.Split(char.Parse("|")).ToList();//0->TIPO (Istanza/Comunicazione), 1->TRIBUTO, 2->ID Istanza/Comunicazione, 3->Testo
                             sScriptNews += "<label class=\"text-white " + ((myNews.Codice == "NEW") ? "blink_me" : string.Empty) + "\" id=\"lblNews" + myNews.ID + "\"";
                            sScriptNews += ">" + ListVal[3].Replace("\r\n", "").Replace("'", "&rsquo;") + "</label>";
                            sScriptNews += "<hr></hr>";
                        }
                        RegisterScript("$('#News').html('" + sScriptNews + "');", this.GetType());
                        string sScriptOnclickNews = string.Empty;
                        foreach (GenericCategory myNews in ListComunicazioni)
                        {
                            List<string> ListVal = myNews.Descrizione.Split(char.Parse("|")).ToList();
                             sScriptOnclickNews += "$('#lblNews" + myNews.ID + "').on(\"click\",function(){";
                            sScriptOnclickNews += "$('#MainContent_hfIdNews').val('" + myNews.ID + "');";
                            sScriptOnclickNews += "$('#MainContent_hfIdGenNews').val('" + ListVal[2] + "');";
                            sScriptOnclickNews += "$('#MainContent_hfTributoNews').val('" + ListVal[1] + "');";
                            sScriptOnclickNews += "$('#MainContent_CmdReadNews').click();";
                            sScriptOnclickNews += "});";
                        }
                        RegisterScript(sScriptOnclickNews, this.GetType());

                        if (MySession.Current.UserLogged.IDContribLogged != MySession.Current.UserLogged.IDContribToWork)
                        {
                            ShowHide("ICI", false);
                            ShowHide("TASI", false);
                            ShowHide("TARSU", false);
                            ShowHide("OSAP", false);
                            ShowHide("ICP", false);
                            ShowHide("TESSERE", false);
                            ShowHide("PROVVEDIMENTI", false);
                            List<string> ListDeleg = MySession.Current.UserLogged.ListDeleganti.Split(char.Parse(",")).ToList();

                            foreach (string myItem in ListDeleg)
                            {
                                List<string> ListDetails = myItem.Split(char.Parse("|")).ToList();
                                if (ListDetails[0] == MySession.Current.UserLogged.IDContribToWork.ToString())
                                {
                                    foreach (string myTrib in ListDetails[2].Split(char.Parse("-")).ToList())
                                    {
                                        if (myTrib == General.TRIBUTO.ICI)
                                            ShowHide("ICI", true);
                                        if (myTrib == General.TRIBUTO.TASI)
                                            ShowHide("TASI", true);
                                        if (myTrib == General.TRIBUTO.TARSU)
                                            ShowHide("TARSU", true);
                                        if (myTrib == General.TRIBUTO.OSAP)
                                            ShowHide("OSAP", true);
                                        if (myTrib == General.TRIBUTO.ICP)
                                            ShowHide("ICP", true);
                                        if (myTrib == General.TRIBUTO.TESSERE)
                                            ShowHide("TESSERE", true);
                                        if (myTrib == General.TRIBUTO.PROVVEDIMENTI)
                                            ShowHide("PROVVEDIMENTI", true);
                                    }
                                }
                            }
                        }
                        else {
                            ShowHide("ICI", false);
                            ShowHide("TASI", false);
                            ShowHide("TARSU", false);
                            ShowHide("OSAP", false);
                            ShowHide("ICP", false);
                            ShowHide("TESSERE", false);
                            ShowHide("PROVVEDIMENTI", false);
                            foreach (GenericCategory myTrib in MySession.Current.Ente.ListTributi)
                            {
                                if (myTrib.IDTributo == General.TRIBUTO.ICI && myTrib.IsActive==1)
                                    ShowHide("ICI", true);
                                if (myTrib.IDTributo == General.TRIBUTO.TASI && myTrib.IsActive == 1)
                                    ShowHide("TASI", true);
                                if (myTrib.IDTributo == General.TRIBUTO.TARSU && myTrib.IsActive == 1)
                                    ShowHide("TARSU", true);
                                if (myTrib.IDTributo == General.TRIBUTO.OSAP && myTrib.IsActive == 1)
                                    ShowHide("OSAP", true);
                                if (myTrib.IDTributo == General.TRIBUTO.ICP && myTrib.IsActive == 1)
                                    ShowHide("ICP", true);
                                if (myTrib.IDTributo == General.TRIBUTO.TESSERE && myTrib.IsActive == 1)
                                    ShowHide("TESSERE", true);
                                if (myTrib.IDTributo == General.TRIBUTO.PROVVEDIMENTI && myTrib.IsActive == 1)
                                    ShowHide("PROVVEDIMENTI", true);
                            }
                        }
                        RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                        lblnomeente.Text = MySession.Current.Ente.Descrizione;

                        LblImpDovuto.Text = myRiepDeb.Dovuto.ToString("#,##0.00").PadLeft(12, char.Parse(" "));
                        LblImpPagato.Text = myRiepDeb.Pagato.ToString("#,##0.00").PadLeft(12, char.Parse(" "));
                        LblImpInsoluto.Text = myRiepDeb.Insoluto.ToString("#,##0.00").PadLeft(12, char.Parse(" "));
                        LoadChart(double.Parse(myRiepDeb.Pagato.ToString()), double.Parse(myRiepDeb.Insoluto.ToString()));
                    }
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "", "Page_Load", "ingresso pagina", "", "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.Page_Load::errore::", ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                RegisterScript("$('#FAQ').addClass('HelpFOTributi');", this.GetType());
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "", "Back", "uscita pagina", "", "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.Back::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ReadNews(object sender, EventArgs e)
        {
            try
            {
                MySession.Current.IsBackToTributi = true;
                if (!new BLL.Messages(new Message()).SetReading(int.Parse(hfIdNews.Value), ((hfTributoNews.Value == string.Empty) ? "COMUNICAZIONE" : "ISTANZA")))
                {
                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.ReadNews.lettura comunicazione::errore::");
                }
                else {
                    if (hfTributoNews.Value == string.Empty)
                    {
                        MySession.Current.IdMessage = int.Parse(hfIdGenNews.Value);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Comunicazioni", null), Response);
                    }
                    else
                    {
                        MySession.Current.IdIstanza = int.Parse(hfIdGenNews.Value);

                        if (hfTributoNews.Value == General.TRIBUTO.ICI)
                        {
                            IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
                        }
                        else if (hfTributoNews.Value == General.TRIBUTO.TARSU)
                        {
                            IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TARSU, null), Response);
                        }
                        else if (hfTributoNews.Value == General.TRIBUTO.TASI)
                        {
                            IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TASI, null), Response);
                        }
                        else if (hfTributoNews.Value == General.TRIBUTO.OSAP)
                        {
                            IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.OSAP, null), Response);
                        }
                        else if (hfTributoNews.Value == General.TRIBUTO.ICP)
                        {
                            IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICP, null), Response);
                        }
                        else if (hfTributoNews.Value == General.TRIBUTO.PROVVEDIMENTI)
                        {
                            IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.PROVVEDIMENTI, null), Response);
                        }
                        else if (hfTributoNews.Value == "ANAG")
                        {
                            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetProfiloFO, Response);
                        }
                        else
                        {
                            RegisterScript("$('#OnlyNumber_error').text('Funzionalità al momento non disponibile');$('#OnlyNumber_error').show();", this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.ReadNews::errore::", ex);
                LoadException(ex);
            }
        }
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDichiarazioniRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.GrdDichiarazioniRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDichiarazioniRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "UIOpen":
                        MySession.Current.IDDichiarazioneIstanze = IDRow;
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestDichiarazione, Response);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "", "UIOpen", "chiesto consultazione dichiarazioni", "", "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.FO_Base.GrdDichiarazioniRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        private void LoadChart(double Pagato, double Insoluto)
        {
            
        }

    }
}