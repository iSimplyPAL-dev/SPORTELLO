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

namespace OPENgovSPORTELLO.Dichiarazioni.TASI
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
                ShowHide("divRiepilogo", false);
                ShowHide("divF24", false);
                ShowHide("divCatasto", UrlHelper.GetVisualCatasto);
                MySession.Current.IdRifCalcolo = -1;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.Page_Init::errore::", ex);
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
                string sScript = string.Empty;
                if (MySession.Current.IsInitDich)
                {
                    sScript += "$('#hfInitDich').val('1');$('.BottoneInbox').show();$('p#PrintDich').show();$('.BottoneF24').hide();$('p#F24').hide();";
                }
                else
                {
                    sScript += "$('.BottoneInbox').hide();$('p#PrintDich').hide();$('.BottoneF24').show();$('p#F24').show();";
                }

                if (!Page.IsPostBack)
                {
                    List<RiepilogoUI> ListUICalcolo = new List<RiepilogoUI>();
                    List<RiepilogoDovuto> ListDovuto = new List<RiepilogoDovuto>();

                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadTASIRiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListUICalcolo, out ListDovuto))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                    else {
                        GrdCalcolo.DataSource = ListUICalcolo;
                        GrdCalcolo.DataBind();
                        decimal TotDovuto = 0;
                        foreach (RiepilogoUI myItem in ListUICalcolo)
                        {
                            TotDovuto += myItem.ImpDovuto;
                        }
                        sScript += "$('#lblTotDovuto').text('Tot. Dovuto " + TotDovuto.ToString("#,##0.00") + " €');";
                        bool OnlyDich = true;
                        foreach (RiepilogoUI myItem in ListUICalcolo)
                        {
                            if (myItem.IsFromDich == "NO")
                            {
                                OnlyDich = false;
                                break;
                            }
                        }
                        sScript += "$('#lblForewordCalc').html('Elenco immobili";
                        if (OnlyDich)
                        {
                            sScript += " banca dati comunale";
                        }
                        sScript += " su cui potrai apportare le modifiche per l’anno in corso.<br />";
                        sScript += "<label class=\"text-11 text-italic\">Per inserire una nuova dichiarazione clicca sul tasto verde a destra. ";
                        sScript += "Per fare una variazione clicca sulla casella posta alla sinistra del fabbricato da variare (colonna &lsquo;Sel.&rsquo;).</label>";
                        sScript += "');";
                    }
                }
                sScript += "$('#lblForewordDich').text('Elenco immobili presenti nella banca dati del Comune');";
                RegisterScript(sScript, this.GetType());
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ShowHide("divRiepilogo", false);
                ManageBottoni(false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Page_Load", "ingresso pagina", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                RegisterScript("$('#FAQ').addClass('HelpFORiepTASI');", this.GetType());
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
                MySession.Current.TipoIstanza = Istanza.TIPO.NuovaDichiarazione;
                MySession.Current.IdRifCalcolo = -1;
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "IstanzaNew", "chiesto inserimento nuova istanza", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TASI, null), Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.NewIstanza::errore::", ex);
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
                ListIstRif.Add(new Istanza { IDTributo = General.TRIBUTO.TASI });
                if (!new BLL.Dichiarazioni(new Istanza()).StampaDichiarazione(MySession.Current.IDDichiarazioneIstanze, General.TRIBUTO.TASI, MySession.Current.Ente, MySession.Current.myAnag, out sScriptDich))
                {
                    string sScript = "$('#lblErrorFO').text('Errore in stampa!');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    divRiepBody.InnerHtml = sScriptDich;
                    RegisterScript("$('#hfInitDich').val('0');", this.GetType());
                    ShowHide("divRiepilogo", true); ShowHide("divBase", false); ShowHide("MainContent_divF24", false);
                    ManageBottoni(true);
                    string sNameHTMLtoPDF = "DICH" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                    string sNamePDF = "DICH_TASI_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf";
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "stampa dichiarazione", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);

                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    SelectPdf.PdfDocument doc = converter.ConvertUrl(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                    // save pdf document
                    doc.Save(UrlHelper.GetPathDichiarazioni + sNamePDF);
                    // close pdf document
                    doc.Close();
                    File.Copy(UrlHelper.GetPathDichiarazioni + sNamePDF
                            , UrlHelper.GetRepositoryPDF + sNamePDF);
                    File.Delete(UrlHelper.GetRepositoryPDF + sNamePDF);
                    RegisterScript("$('#myEmbedPDF').attr('src','"+UrlHelper.GetPathWebDichiarazioni + "DICH_TASI_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf');", this.GetType());
                    
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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "invio mail dichiarazione da protocollare", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.SendDichiarazione.Mail::errore::", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.PrintDichiarazione::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void PrintF24(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.Month > 6)
                    optSaldo.Checked = true;
                else
                    optAcconto.Checked = true;

                LoadF24();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.PrintF24::errore::", ex);
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintPDF", "creazione pdf " + TypePDF, General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                }
                catch (Exception err)
                {
                    if (err.Message != "Thread was being aborted.")
                    {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.PrintPDF::errore::", err);
                        string sScript = "$('#lblErrorFO').text('Errore in stampa!');$('#lblErrorFO').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.PrintPDF::errore::", ex);
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
                    decimal TotDovuto = 0;
                    foreach (GridViewRow myRow in GrdCalcolo.Rows)
                    {
                        TotDovuto += decimal.Parse(myRow.Cells[9].Text);
                    }
                    string sScript = "$('#lblTotDovuto').text('Tot. Dovuto " + TotDovuto.ToString("#,##0.00") + " €');";
                    RegisterScript(sScript, this.GetType());

                    ShowHide("divBase", true); ShowHide("MainContent_divF24", false); ShowHide("divRiepilogo", false);
                    ManageBottoni(false);
                    hdTypePDF.Value = "";
                    File.Delete(UrlHelper.GetRepositoryPDF + "DICH_TASI_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf");
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita stampa pdf", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                }
                else
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita pagina", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.Back::errore::", ex);
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
        protected void GrdCalcoloRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.GrdCalcoloRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdCalcoloRowCommand(object sender, GridViewCommandEventArgs e)
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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.TASI, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.GrdCalcoloRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TipoF24CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                LoadF24();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.TipoF24CheckedChanged::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IsF24"></param>
        protected void ManageBottoni(bool IsF24)
        {
            try
            {
                if (IsF24)
                {
                    RegisterScript("$('.BottonePDF').show();", this.GetType());
                    RegisterScript("$('.BottoneInbox').hide();$('p#PrintDich').hide();", this.GetType());
                    RegisterScript("$('.BottoneF24').hide();$('p#F24').hide();", this.GetType());
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
                    RegisterScript("$('.BottoneF24').show();$('p#F24').show();", this.GetType());
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.ManageBottoni::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadF24()
        {
            string sScriptF24, sScriptF24ForPDF;
            sScriptF24 = sScriptF24ForPDF = string.Empty;
            List<DatiF24> ListUICalcolo = new List<DatiF24>();

            try
            {
                if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadF24(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, General.TRIBUTO.TASI, DateTime.Now.Year, ((optUS.Checked) ? "U" : ((optSaldo.Checked) ? "S" : "A")), out ListUICalcolo))
                    RegisterScript("Errore in stampa", this.GetType());
                else {
                    if (ListUICalcolo.Count > 0)
                    {
                        if (!GetPageF24(ListUICalcolo, out sScriptF24, out sScriptF24ForPDF))
                        {
                            string sScript = "$('#lblErrorFO').text('Errore in stampa!');$('#lblErrorFO').show();";
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
                            hdTypePDF.Value = "F24";
                            new General().LogActionEvent(DateTime.Now, MySession.Current.Scope, MySession.Current.UserLogged.NameUser, "Tributi", "Riepilogo", "PrintF24", "stampa F24", General.TRIBUTO.TASI, "", MySession.Current.Ente.IDEnte);
                        }
                    }
                    else {
                        string sScript = "$('#lblErrorFO').text('Stampa non disponibile!');$('#lblErrorFO').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.LoadF24::errore::", ex);
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
                #region dati anagrafTASI 
                sScriptForPDF += "<div id='f24_testata'>";
                sScriptForPDF += "<div id='f24_banca'><input type='text' id='banca' value='' size='38' maxlength='60'></div>";
                sScriptForPDF += "<div id='f24_bagenzia'><input type='text' id='bn_agenzia' value='' size='32' maxlength='40'></div>";
                sScriptForPDF += "<div id='f24_bprov'><input type='text' id='bn_prov' value='' maxlength='2'></div>";
                sScriptForPDF += "<div id='f24_cf'><input type='text' class='cfsp' id='cod_fisc' value='" + ListUICalcolo[0].CFPIVA + "' maxlength='16'></div>";
                sScriptForPDF += "<div id='f24_uff'><input type='text' id='codice_uffTASIo' class='cfsp' value='' maxlength='3'></div>";
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
                    if (myRow.ravvedimento > 0)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo.GetPageF24::errore::", ex);
                return false;
            }
        }

    }
}