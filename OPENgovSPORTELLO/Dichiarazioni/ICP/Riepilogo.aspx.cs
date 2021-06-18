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

namespace OPENgovSPORTELLO.Dichiarazioni.ICP
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Riepilogo : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Riepilogo));
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
               MySession.Current.IdRifCalcolo = -1;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.Page_Init::errore::", ex);
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
                    List<RiepilogoDovuto> ListDovuto = new List<RiepilogoDovuto>();

                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadICPRiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListUI, out ListDich, out ListDovuto))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                    else {
                        GrdUI.DataSource = ListUI;
                        GrdUI.DataBind();

                        GrdDich.DataSource = ListDich;
                        GrdDich.DataBind();
                        new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadBaseStato(ListUI, imgStato);
                    }
                }
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                string sScript = string.Empty;
                sScript += "$('#lblForewordCalc').text('Elenco dichiarazioni da te presentate e non ancora recepite nella banca dati comunale.');";
                sScript += "$('#lblForewordDich').text('Elenco immobili presenti nella banca dati del Comune.');";
                RegisterScript(sScript, this.GetType());
                ShowHide("divRiepilogo", false);
                ManageBottoni(false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Page_Load", "ingresso pagina", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
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
                MySession.Current.TipoIstanza= Istanza.TIPO.NuovaDichiarazione;
                MySession.Current.IdRifCalcolo = -1;
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "IstanzaNew", "chiesto inserimento nuova istanza", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICP, null), Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.NewIstanza::errore::", ex);
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
                ListIstRif.Add(new Istanza { IDTributo = General.TRIBUTO.ICP });
                if (!new BLL.Dichiarazioni(new Istanza()).StampaDichiarazione(MySession.Current.IDDichiarazioneIstanze, General.TRIBUTO.ICP, MySession.Current.Ente, MySession.Current.myAnag, out sScriptDich))
                {
                    string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    divRiepBody.InnerHtml = sScriptDich;
                    RegisterScript("$('#hfInitDich').val('0');", this.GetType());
                    ShowHide("divRiepilogo", true); ShowHide("divBase", false);
                    ManageBottoni(true);
                    string sNameHTMLtoPDF = "DICH" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                    string sNamePDF = "DICH_ICP_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf";
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
                    hdTypePDF.Value = "DICH";
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "stampa dichiarazione", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);

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

                    MySession.Current.IsInitDich = false;
                    MySession.Current.IDDichiarazioneIstanze = -1;
                    MySession.Current.HasNewDich = 0;
                    try
                    {
                        IdentityMessage myMessage = new IdentityMessage();
                        List<string> ListRecipient = new List<string>();
                        List<string> ListRecipientBCC = new List<string>();
                        ListRecipient.Add(MySession.Current.Ente.Mail.BackOffice);
                        ListRecipientBCC.Add(MySession.Current.Ente.Mail.Archive);
                        myMessage.Body = "E' stata inserita, per il comune di " + MySession.Current.Ente.Descrizione + ", una dichiarazione da protocollare dall'utente " + MySession.Current.myAnag.Cognome + " " + MySession.Current.myAnag.Nome + ".";
                        myMessage.Subject = "Sportello - Dichiarazione in attesa di protocollo";
                        new EmailService().SendAsync(myMessage, MySession.Current.Ente.Mail, ListRecipient, new List<string>(), ListRecipientBCC, new List<System.Web.Mail.MailAttachment>());
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "invio mail dichiarazione da protocollare", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.SendDichiarazione.Mail::errore::", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.PrintDichiarazione::errore::", ex);
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
                    string TypePDF = hdTypePDF.Value;

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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintPDF", "creazione pdf " + TypePDF, General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                }
                catch (Exception err)
                {
                    if (err.Message != "Thread was being aborted.")
                    {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.PrintPDF::errore::", err);
                        string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.PrintPDF::errore::", ex);
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
                if (hdTypePDF.Value != string.Empty)
                {
                    ShowHide("divBase", true); ShowHide("divRiepilogo", false);
                    ManageBottoni(false);
                    hdTypePDF.Value = "";
                    File.Delete(UrlHelper.GetRepositoryPDF + "DICH_ICP_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf");
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita stampa pdf", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                }
                else
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita pagina", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.Back::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #region"Griglie"
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.GrdUIRowDataBound::errore::", ex);
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
                        MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                        MySession.Current.IdRifCalcolo = IDRow;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                        //IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestIstanzaICP, Response);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICP, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.GrdUIRowCommand::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.GrdDichRowDataBound::errore::", ex);
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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui dichiarazione", General.TRIBUTO.ICP, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICP, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.GrdDichRowCommand::errore::", ex);
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
                    RegisterScript("$('.BottoneInbox').hide();", this.GetType());
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Riepilogo.ManageBottoni::errore::", ex);
            }
        }
    }
}