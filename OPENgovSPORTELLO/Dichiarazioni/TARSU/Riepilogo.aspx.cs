using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace OPENgovSPORTELLO.Dichiarazioni.TARSU
{
    /// <summary>
    /// La selezione della voce TARI consente la visualizzazione degli articoli attivi o comunque chiusi dopo il 01-01 AA-1 .
    /// Le  informazioni da visualizzare sono:
    /// <list type="bullet">
    /// <item>dati anagrafici del contribuente</item>
    /// <item>Situazione banca dati Comunale - Elenco immobili attualmente presenti nella banca dati del Comune.</item> 
    /// <item>Situazione dichiarazioni - Elenco dichiarazioni presentate in corso di validazione da parte del Comune.</item>
    /// <item>Situazione Dovuto - Elenco avvisi di pagamento ricevuti negli ultimi anni. Se necessario possibilità di ristampare i modelli di versamento F24 cliccando sulla casella posta alla sinistra dell’avviso (colonna F24).</item>
    /// </list>
    /// Selezionando l’articolo apparirà finestra con i tasti di comando delle varie tipologie di dichiarazione; il contribuente avrà anche a disposizione, ad inizio videata, il tasto per l’inserimento di una nuova dichiarazione.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class Riepilogo : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Riepilogo));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private static string AnnoF24;

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
                ShowHide("divRiepilogo", false);
                ShowHide("divF24", false);
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetHelp("HelpFORiepTARSU", MySession.Current.Ente.UrlWiki);
                sScript += new BLL.GestForm().GetLabel("Riepilogo0434", "");
                RegisterScript(sScript, this.GetType());
                MySession.Current.TipoStorico = string.Empty;
                MySession.Current.IdRifCalcolo = -1;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.Page_Init::errore::", ex);
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
                if (MySession.Current.IsInitDich)
                {
                    RegisterScript("$('#hfInitDich').val('1');$('.BottoneInbox').show();", this.GetType());
                }
                else
                {
                    RegisterScript("$('.BottoneInbox').hide();", this.GetType());
                }

                if (!Page.IsPostBack)
                {
                    List<RiepilogoUI> ListUI = new List<RiepilogoUI>();
                    List<RiepilogoUI> ListDich = new List<RiepilogoUI>();
                    List<CategorieTARSU> ListCat = new List<CategorieTARSU>();
                    List<RidEseTARSU> ListUIRidEse = new List<RidEseTARSU>();
                    List<RidEseTARSU> ListDichRidEse = new List<RidEseTARSU>();
                    List<RiepilogoDovuto> ListDovuto = new List<RiepilogoDovuto>();

                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadTARSURiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListUI, out ListDich, out ListCat, out ListUIRidEse, out ListDichRidEse, out ListDovuto))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                    else {
                        GrdUI.DataSource = ListUI;
                        GrdUI.DataBind();

                        if (ListDich.Count > 0)
                        {
                            GrdDich.DataSource = ListDich;
                            GrdDich.DataBind();

                            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich", true);
                        }
                        else
                        {
                            ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdDich", false);
                        }

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

                        if (ListUIRidEse.Count > 0)
                        {
                            divListUIRidEse.InnerHtml = "<p class='lead_normal'>** Riduzione/Esenzione</p><p>";
                            foreach (RidEseTARSU myItem in ListUIRidEse)
                            {
                                divListUIRidEse.InnerHtml += myItem.Codice + " - " + myItem.Descrizione + "<br />";
                            }
                            divListUIRidEse.InnerHtml += "</p>";
                        }
                        if  ( (ListDichRidEse.Count > 0) && ( ListDich.Count > 0 ) )
                        {
                            divListDichRidEse.InnerHtml = "<p class='lead_normal'>** Riduzione/Esenzione</p><p>";
                            foreach (RidEseTARSU myItem in ListDichRidEse)
                            {
                                divListDichRidEse.InnerHtml += myItem.Codice + " - " + myItem.Descrizione + "<br />";
                            }
                            divListDichRidEse.InnerHtml += "</p>";
                        }
                        new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadBaseStato(ListUI, imgStato);
                    }
                }
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                   ShowHide("divRiepilogo", false);
                ManageBottoni(false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Page_Load", "ingresso pagina", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                RegisterScript("$('#FAQ').addClass('HelpFORiepTARSU');", this.GetType());
            }
        }
        #region "Bottoni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void IstanzaNew(object sender, EventArgs e)
        {
            try
            {
                MySession.Current.IdIstanza = -1;
                MySession.Current.TipoIstanza= Istanza.TIPO.NuovaDichiarazione;
                MySession.Current.IdRifCalcolo = -1;
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "IstanzaNew", "chiesto inserimento nuova istanza", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                 IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TARSU, null), Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.NewIstanza::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrintDichiarazione(object sender, EventArgs e)
        {
            try
            {
                string sScriptDich = string.Empty;
                List<Istanza> ListIstRif = new List<Istanza>();
                ListIstRif.Add(new Istanza { IDTributo = General.TRIBUTO.TARSU });
                if (!new BLL.Dichiarazioni(new Istanza()).StampaDichiarazione(MySession.Current.IDDichiarazioneIstanze, General.TRIBUTO.TARSU, MySession.Current.Ente, MySession.Current.myAnag, out sScriptDich))
                {
                    string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    divRiepBody.InnerHtml = sScriptDich;
                    RegisterScript("$('#hfInitDich').val('0');", this.GetType());
                    ShowHide("divRiepilogo", true); ShowHide("divBase", false); ShowHide("divF24", false);
                    ManageBottoni(true);
                    string sNameHTMLtoPDF = "DICH" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                    string sNamePDF = "DICH_TARI_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf";
                    try
                    {
                        if (File.Exists(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF))
                            File.Delete(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                        if (File.Exists(UrlHelper.GetPathDichiarazioni + sNamePDF))
                            File.Delete(UrlHelper.GetPathDichiarazioni + sNamePDF);
                        if (File.Exists(UrlHelper.GetRepositoryPDF + sNamePDF))
                            File.Delete(UrlHelper.GetRepositoryPDF + sNamePDF);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.PrintDichiarazione.CancellaDoc::errore::", ex);
                    }
                    using (StreamWriter writetext = new StreamWriter(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF))
                    {
                        writetext.WriteLine(sScriptDich);
                        writetext.Close();
                    }
                    hfTypePDF.Value = "DICH";
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "stampa dichiarazione", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);

                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    SelectPdf.PdfDocument doc = converter.ConvertUrl(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                    // save pdf document
                    doc.Save(UrlHelper.GetPathDichiarazioni + sNamePDF);
                    // close pdf document
                    doc.Close();
                    File.Copy(UrlHelper.GetPathDichiarazioni + sNamePDF
                            , UrlHelper.GetRepositoryPDF + sNamePDF);

                    File.Delete(UrlHelper.GetPathDichiarazioni + sNamePDF);

                    RegisterScript("$('#myEmbedPDF').attr('src','" + UrlHelper.GetPathWebDichiarazioni + sNamePDF, this.GetType());

                    MySession.Current.IsInitDich=false;
                MySession.Current.IDDichiarazioneIstanze=-1;
                try
                    {
                        IdentityMessage myMessage = new IdentityMessage();
                        List<string> ListRecipient = new List<string>();
                        List<string> ListRecipientBCC = new List<string>();
                        ListRecipient.Add(MySession.Current.Ente.Mail.BackOffice);
                        ListRecipientBCC.Add(MySession.Current.Ente.Mail.Archive);
                        myMessage.Body = "E' stata inserita, per il comune di " + MySession.Current.Ente.Descrizione + ", una dichiarazione da protocollare dall'utente " +MySession.Current.myAnag.Cognome + " " + MySession.Current.myAnag.Nome + ".";
                        myMessage.Subject = "Sportello - Dichiarazione in attesa di protocollo";
                        new EmailService().SendAsync(myMessage, MySession.Current.Ente.Mail, ListRecipient, new List<string>(), ListRecipientBCC, new List<System.Web.Mail.MailAttachment>());
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "invio mail dichiarazione da protocollare", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.SendDichiarazione.Mail::errore::", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.PrintDichiarazione::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrintPDF(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string TypePDF = hfTypePDF.Value;

                    string sNameHTMLtoPDF = TypePDF + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    if (TypePDF == "F24")
                    {
                        converter.Options.AutoFitWidth = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
                        converter.Options.AutoFitHeight = SelectPdf.HtmlToPdfPageFitMode.NoAdjustment;
                    }// create a new pdf document converting an url
                    SelectPdf.PdfDocument doc = converter.ConvertUrl(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                    // save pdf document
                    doc.Save(Response, false, TypePDF.ToUpper() + ".pdf");
                    // close pdf document
                    doc.Close();
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintPDF", "creazione pdf " + TypePDF, General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                }
                catch (Exception err)
                {
                    if (err.Message != "Thread was being aborted.")
                    {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.PrintPDF::errore::", err);
                        string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.PrintPDF::errore::", ex);
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
                if (hfTypePDF.Value != string.Empty)
                {
                    ShowHide("divBase", true);  ShowHide("divRiepilogo", false); ShowHide("divF24", false);
                    ManageBottoni(false);
                    hfTypePDF.Value = "";
                    File.Delete(UrlHelper.GetRepositoryPDF + "DICH_TARI_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf");
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita stampa pdf", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                }
                else
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita pagina", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.Back::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Storico(object sender, EventArgs e)
        {
            try
            {
                if (((System.Web.UI.WebControls.Button)sender).ID == "StoricoCat")
                {
                    MySession.Current.TipoStorico = "CAT";
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Storico", "consulta storico catasto", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetStoricoTARSU, Response);
                }
                else
                {
                    MySession.Current.TipoStorico = "DICH";
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Storico", "consulta storico dichiarazione", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetStoricoTARSU, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.Storico::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #region "Griglie"
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GrdUIRowDataBound::errore::", ex);
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
                        MySession.Current.IdIstanza = -1;
                        MySession.Current.TipoIstanza= Istanza.TIPO.Variazione;
                        MySession.Current.IdRifCalcolo = IDRow;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TARSU, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GrdUIRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDichRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((HiddenField)e.Row.FindControl("hfIdRifOrg")).Value == "-1")
                    {
                        ((CheckBox)e.Row.FindControl("chkSel")).Enabled = false;
                    }
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtn($(this).attr('id'));");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GrdDichRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdDichRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "UIOpen":
                        MySession.Current.IdIstanza = -1;
                        MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                        MySession.Current.IdRifCalcolo = IDRow;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui dichiarazione", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TARSU, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GrdDichRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GrdDovutoRowDataBound::errore::", ex);
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
                switch (e.CommandName)
                {
                    case "UIOpen":
                        LoadF24(e.CommandArgument.ToString());
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto stampa F24", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GrdDovutoRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsPrint"></param>
        protected void ManageBottoni(bool IsPrint)
        {
            try
            {
                if (IsPrint)
                {
                    RegisterScript("$('.BottonePDF').show();", this.GetType());
                    RegisterScript("$('.BottoneInbox').hide();$('p#PrintDich').hide();", this.GetType());
                    RegisterScript("$('.BottoneNew').hide();", this.GetType());
                    RegisterScript("$('.BottoneBack').show();", this.GetType());
                }
                else
                {
                    RegisterScript("$('.BottonePDF').hide();", this.GetType());
                    if (MySession.Current.IsInitDich)
                    {
                        RegisterScript("$('.BottoneInbox').show();$('p#PrintDich').show();", this.GetType());
                    }
                    else
                    {
                        RegisterScript("$('.BottoneInbox').hide();$('p#PrintDich').hide();", this.GetType());
                    }
                    if (MySession.Current.HasNewDich == 2)
                        RegisterScript("$('.BottoneNewGrd').hide();$('p#NuovaUI').hide();", this.GetType());
                    else
                        RegisterScript("$('.BottoneNewGrd').show();$('p#NuovaUI').show();", this.GetType());
                    RegisterScript("$('.BottoneBack').show();", this.GetType());
                }
                if (MySession.Current.UserLogged.IDDelegato > 0)
                {
                    RegisterScript("$('.BottoneNewGrd').hide();$('p#NuovaUI').hide();", this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.ManageBottoni::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAnno"></param>
        private void LoadF24(string myAnno)
        {
            AnnoF24 = myAnno;
            string sScriptF24, sScriptF24ForPDF;
            sScriptF24 = sScriptF24ForPDF = string.Empty;
            List<DatiF24> ListUICalcolo = new List<DatiF24>();

            try
            {
                List<GenericCategory> ListScadenze = new BLL.Settings().LoadScadenze(MySession.Current.Ente.IDEnte, int.Parse(myAnno), General.TRIBUTO.TARSU);
                sScriptF24 += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt1').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt2').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt3').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt4').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_optU').hide();";
                sScriptF24 += "$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt1\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt2\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt3\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt4\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_optU\"]').hide();";
                foreach (GenericCategory myScad in ListScadenze)
                {
                    if (myScad.Codice != string.Empty)
                    {
                        sScriptF24 += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt" + myScad.Codice + "').show();";
                        sScriptF24 += "$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt" + myScad.Codice + "\"]').show();";
                    }
                }
                if(ListScadenze.Count>1)
                sScriptF24 += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt" + ListScadenze[1].Codice + "').prop('checked', true);";
                RegisterScript(sScriptF24, this.GetType());
                if (ListScadenze.Count > 1)
                    hfF24Rata.Value = ListScadenze[1].Codice;
                sScriptF24 = string.Empty;

                if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadF24(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, General.TRIBUTO.TARSU, int.Parse(myAnno), hfF24Rata.Value, out ListUICalcolo))
                    RegisterScript("Errore in stampa", this.GetType());
                else {
                    if (ListUICalcolo.Count > 0)
                    {
                        if (!GetPageF24(ListUICalcolo, out sScriptF24, out sScriptF24ForPDF))
                        {
                            string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                        else
                        {
                            divBodyF24.InnerHtml = sScriptF24;
                            ShowHide("divF24", true); ShowHide("divBase", false); ShowHide("divRiepilogo", false);
                            ManageBottoni(true);
                            string sNameHTMLtoPDF = "F24" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                            using (StreamWriter writetext = new StreamWriter(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF))
                            {
                                writetext.WriteLine("<html xmlns='http://www.w3.org/1999/xhtml'>	<head>	<link href='../Content/F24.css' rel='stylesheet'>	</head>	<body>");
                                writetext.WriteLine(sScriptF24ForPDF);
                                writetext.WriteLine("</body></html>");
                                writetext.Close();
                            }
                            hfTypePDF.Value = "F24";
                            new General().LogActionEvent(DateTime.Now, MySession.Current.Scope, MySession.Current.UserLogged.NameUser, "Tributi", "Riepilogo", "PrintF24", "stampa F24", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        }
                    }
                    else {
                        string sScript = "$('#OnlyNumber_error').text('Stampa non disponibile!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.LoadF24::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TipoF24CheckedChanged(object sender, EventArgs e)
        {
            string sScriptF24, sScriptF24ForPDF;
            sScriptF24 = sScriptF24ForPDF = string.Empty;
            List<DatiF24> ListUICalcolo = new List<DatiF24>();
            try
            {
                hfF24Rata.Value = ((CheckBox)sender).ID.Substring(((CheckBox)sender).ID.Length-1,1);
                List<GenericCategory> ListScadenze = new BLL.Settings().LoadScadenze(MySession.Current.Ente.IDEnte, int.Parse(AnnoF24), General.TRIBUTO.TARSU);
                sScriptF24 += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt1').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt2').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt3').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt4').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_optU').hide();";
                sScriptF24 += "$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt1\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt2\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt3\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt4\"]').hide();$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_optU\"]').hide();";
                foreach (GenericCategory myScad in ListScadenze)
                {
                     if (myScad.Codice != string.Empty)
                    {
                        sScriptF24 += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_opt" + myScad.Codice + "').show();";
                        sScriptF24 += "$('label[for=\"" + BLL.GestForm.PlaceHolderName.Body + "_opt" + myScad.Codice + "\"]').show();";
                    }
                }

                if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadF24(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, General.TRIBUTO.TARSU, int.Parse(AnnoF24), hfF24Rata.Value, out ListUICalcolo))
                    RegisterScript("Errore in stampa", this.GetType());
                else {

                    if (ListUICalcolo.Count > 0)
                    {
                        if (!GetPageF24(ListUICalcolo, out sScriptF24, out sScriptF24ForPDF))
                        {
                            string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                        else
                        {
                            divBodyF24.InnerHtml = sScriptF24;
                            ShowHide("divF24", true); ShowHide("divBase", false); ShowHide("divRiepilogo", false);
                            ManageBottoni(true);
                            string sNameHTMLtoPDF = "F24" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                            using (StreamWriter writetext = new StreamWriter(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF))
                            {
                                writetext.WriteLine("<html xmlns='http://www.w3.org/1999/xhtml'>	<head>	<link href='../Content/F24.css' rel='stylesheet'>	</head>	<body>");
                                writetext.WriteLine(sScriptF24ForPDF);
                                writetext.WriteLine("</body></html>");
                                writetext.Close();
                            }
                            hfTypePDF.Value = "F24";
                            new General().LogActionEvent(DateTime.Now, MySession.Current.Scope, MySession.Current.UserLogged.NameUser, "Tributi", "Riepilogo", "PrintF24", "stampa F24", General.TRIBUTO.TARSU, "", MySession.Current.Ente.IDEnte);
                        }
                    }
                    else {
                        string sScript = "$('#OnlyNumber_error').text('Stampa non disponibile!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.TipoF24CheckedChanged::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListUICalcolo"></param>
        /// <param name="sScriptForPage"></param>
        /// <param name="sScriptForPDF"></param>
        /// <returns></returns>
        protected bool GetPageF24(List<DatiF24> ListUICalcolo, out string sScriptForPage, out string sScriptForPDF)
        {
            string sScriptStart, sScriptEnd;
            sScriptForPage = sScriptForPDF = sScriptStart = sScriptEnd = string.Empty;
            Decimal totdeb, totcred;
            try
            {
                totdeb = totcred = 0;
                sScriptStart += "<div id='f24home' class='hc_hr group'>";
                sScriptStart += "<div id='f24main'>";
                #region dati anagrafici 
                sScriptForPDF += "<div id='f24_testata'>";
                sScriptForPDF += "<div id='f24_banca'><input type='text' id='banca' value='' size='38' maxlength='60'></div>";
                sScriptForPDF += "<div id='f24_bagenzia'><input type='text' id='bn_agenzia' value='' size='32' maxlength='40'></div>";
                sScriptForPDF += "<div id='f24_bprov'><input type='text' id='bn_prov' value='' maxlength='2'></div>";
                sScriptForPDF += "<div id='f24_cf'><input type='text' class='cfsp' id='cod_fisc' value='" + ListUICalcolo[0].CFPIVA + "' maxlength='16'></div>";
                sScriptForPDF += "<div id='f24_uff'><input type='text' id='codice_ufficio' class='cfsp' value='' maxlength='3'></div>";
                sScriptForPDF += "<div id='f24_atto'><input type='text' id='codice_atto' class='cfsp' value='' maxlength='11'></div>";
                sScriptForPDF += "<div id='f24_cognome'><input type='text' id='cognome' value='" + ListUICalcolo[0].Cognome + "' maxlength='60'></div>";
                sScriptForPDF += "<div id='f24_nome'><input type='text' id='nome' value='" + ListUICalcolo[0].Nome + "' maxlength='40'></div>";
                sScriptForPDF += "<div id='f24_nascita'><input type='text' class='nasp' id='data_nascita' value='" + ListUICalcolo[0].DataNascita.Replace("/", "") + "' size='27' maxlength='8'></div>";
                sScriptForPDF += "<div id='f24_sesso'><input type='text' class='nasp' id='sesso' value='" + ListUICalcolo[0].Sesso + "' size='27' maxlength='8'></div>";
                sScriptForPDF += "<div id='f24_comune'><input type='text' id='comune_nasc' value='" + ListUICalcolo[0].ComuneNasc + "' maxlength='40' ></div>";
                sScriptForPDF += "<div id='f24_prov'><input type='text' class='cfsp' id='prov_nasc' value='" + ListUICalcolo[0].PVNasc + "' maxlength='2'></div>";
                sScriptForPDF += "<div id='f24_cf_coobl'><input type='text' class='cfsp' id='cod_fisc_coob' value='' maxlength='16'></div>";
                sScriptForPDF += "<div id='f24_cod_id'><input type='text' class='cfsp' id='cod_iden' value='' maxlength='2'></div>";
                sScriptForPDF += "</div>";
                #endregion
                #region dettaglio importi 
                sScriptForPDF += "<div id='f24_imu'>";
                sScriptForPDF += "<div class='r_trib2s'><input type='text' id='idoperazione' class='se_id_op' value='" + ListUICalcolo[0].IDOperazione + "' maxlength='18'></div>";
                foreach (DatiF24 myRow in ListUICalcolo)
                {
                    sScriptForPDF += "<div class='r_trib2s'";
                    if (totdeb == 0)
                        sScriptForPDF += " style='margin-top:12px;'";
                    sScriptForPDF += ">";
                    sScriptForPDF += "<div class='d_sem_1'><input type='text' id='sezione' class='se_1' value='" + myRow.Sezione + "' maxlength='2'></div>";
                    sScriptForPDF += "<div class='d_sem_2'><input type='text' id='idtributo' class='se_2' value='" + myRow.idtributo + "' maxlength='4'></div>";
                    sScriptForPDF += "<div class='d_sem_3'><input type='text' id='ente' class='se_3' value='" + myRow.ente + "' maxlength='4'></div>";
                    sScriptForPDF += "<div class='d_sem_4'>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='ravvedimento' class='mgs1' value='";
                    if (myRow.ravvedimento>0)
                        sScriptForPDF += "X";
                    sScriptForPDF += "'></div>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='uivar' class='mgs1' value='";
                    if (myRow.uivar > 0)
                        sScriptForPDF += "X";
                    sScriptForPDF += "'></div>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='acc' class='mgs1' value='";
                    if (myRow.acc > 0)
                        sScriptForPDF += "X";
                    sScriptForPDF += "'></div>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='sal' class='mgs1' value='";
                    if (myRow.sal > 0)
                        sScriptForPDF += "X";
                    sScriptForPDF += "'></div>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='nui' class='cl2_1i1' value='" + myRow.nui + "' maxlength='3'></div>";
                    sScriptForPDF += "</div>";
                    sScriptForPDF += "<div class='d_sem_5'><input type='text' id='rateazione' class='se_5' value='" + myRow.rateazione + "' maxlength='5'></div>";
                    sScriptForPDF += "<div class='d_sem_6'><input type='text' id='anno' class='se_5' value='" + myRow.anno + "' maxlength='4'></div>";
                    sScriptForPDF += "<div class='d_sem_7'><input type='text' id='impdetr' class='se_3_6' value='" + myRow.impdet + "' maxlength='7'></div>";
                    sScriptForPDF += "<div class='d_sem_8'><input type='text' id='impdeb' class='cl_4_4' value='" + myRow.impdeb + "' maxlength='10'></div>";
                    sScriptForPDF += "<div class='d_sem_8'><input type='text' id='impcred' class='cl_5_4' value='" + myRow.impcred + "' maxlength='10'></div>";
                    sScriptForPDF += "</div>";
                    if (myRow.impdeb != string.Empty)
                        totdeb += decimal.Parse(myRow.impdeb);
                    if (myRow.impcred != string.Empty)
                        totcred += decimal.Parse(myRow.impcred);
                }
                for (int x = ListUICalcolo.Count + 1; x <= 10; x++)
                {
                    sScriptForPDF += "<div class='r_trib2s'></div>";
                }
                #endregion
                #region totale 
                sScriptForPDF += "<div class='r_trib1_f' style='margin-top:0px;'>";
                sScriptForPDF += "<div style='width:100px; float:left;'>";
                sScriptForPDF += "<input type='hidden' class='vparz' id='totdeb' value='" + totdeb + "'>";
                sScriptForPDF += "<input type='hidden' class='vparz' id='totcred' value='" + totcred + "'>";
                sScriptForPDF += "</div>";
                sScriptForPDF += "<div class='b1f5'>";
                sScriptForPDF += "<input type='text' id='totale' class='bl_5' value='" + (totdeb + totcred).ToString().Replace(",", "").Replace(".", "") + "' maxlength='10' readonly='' style='color: rgb(0, 0, 0); background-image: none;'></div>";
                sScriptForPDF += "</div>";
                #endregion
                #region chiusura blocco importi 
                sScriptForPDF += "</div>";
                #endregion 
                sScriptEnd += "</div>";
                sScriptEnd += "</div>";
                sScriptEnd += "</div>";
                sScriptForPage = sScriptStart + sScriptForPDF + sScriptEnd;
                sScriptForPDF = sScriptStart + sScriptForPDF + sScriptForPDF + sScriptEnd;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo.GetPageF24::errore::", ex);
                return false;
            }
        }
    }
}