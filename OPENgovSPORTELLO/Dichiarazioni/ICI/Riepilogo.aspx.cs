using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;

namespace OPENgovSPORTELLO.Dichiarazioni.ICI
{
    /// <summary>
    /// La selezione della voce IMU consente la visualizzazione degli immobili attivi o comunque chiusi dopo il 01-01 AA-1 .
    /// Le  informazioni da visualizzare sono:
    /// <list type="bullet">
    /// <item>dati anagrafici del contribuente</item>
    /// <item>Situazione calcolo - Elenco immobili banca dati comunale aggiornata al xxxx su cui apportare le modifiche per l’anno in corso e ovviamente le modifiche fatte.</item> 
    /// <item>Situazione dichiarata - Elenco immobili presenti nella banca dati del Comune aggiornata al xxxx</item>
    /// <item>Situazione catastale - Elenco immobili presenti a catasto alla data di interrogazione</item>
    /// </list>
    /// La sezione per il calcolo, la prima volta riporta gli stessi dati presenti nel verticale.
    /// Al termine delle dichiarazioni, il contribuente può  richiedere al sistema il calcolo del dovuto e stampa F24.
    /// Il back office recepisce le dichiarazione, ma non valida.Nel momento in cui il back office conferma l’acquisizione, il sistema protocolla e invia mail di conferma ricezione.
    /// Per il back office l’istanza è lavorata, ma si mantiene aperta la possibilità di effettuare comunicazioni al contribuente in momenti futuri, tutte le istanze, validate, rifiutate, accettate potranno essere oggetto di successive comunicazioni da parte del back office.
    /// Nella sezione catasto aggiungere una colonna 	“Corrispondenza con dati calcolo”, il sistema evidenzierà le differenze tra il dati di calcolo ed il dato catastale. Non si effettua la verifica dei presenti in calcolo e non in catasto.
    /// Nelle tre sezioni saranno visualizzati tutti gli immobili attivi, ovvero, quelli per i quali è stato chiuso il periodo di possesso nell’ultimo anno d’imposta. L’utente avrà la possibilità di richiamare gli immobili chiusi mediante la pressione del bottone [STORICO]. Il bottone sarà attivo per ogni sezione.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
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
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetHelp("HelpFORiepICI",MySession.Current.Ente.UrlWiki);
                RegisterScript(sScript, this.GetType());
                MySession.Current.TipoStorico = string.Empty;
                MySession.Current.UINewSuggest.IDRifOrg = -1;
                MySession.Current.IdRifCalcolo = -1;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.Page_Init::errore::", ex);
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
                    sScript += "$('#hfInitDich').val('1');$('.BottoneInbox').show();$('p#PrintDich').show();$('.BottoneF24').hide();$('p#F24').hide();$('.BottoneEdit').hide();$('p#Ravvedi').hide();";
                }
                else
                {
                    sScript += "$('.BottoneInbox').hide();$('p#PrintDich').hide();$('.BottoneF24').show();$('p#F24').show();$('.BottoneEdit').show();$('p#Ravvedi').show();";
                }

                if (!Page.IsPostBack)
                {
                   List<RiepilogoUI> ListUICalcolo = new List<RiepilogoUI>();
                    List<RiepilogoUI> ListUIDich = new List<RiepilogoUI>();
                    List<RiepilogoUI> ListUICatasto= new List<RiepilogoUI>();
                    List<SPC_DichICI> ListDichUICatasto = new List<SPC_DichICI>();
                    List<RiepilogoDovuto> ListDovuto = new List<RiepilogoDovuto>();

                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadICIRiepilogo(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, out ListUICalcolo, out ListUIDich, out ListUICatasto, out ListDovuto))
                        RegisterScript("$('#OnlyNumber_error').text('Errore in caricamento pagina!');$('#OnlyNumber_error').show();", this.GetType());
                    else {
                        GrdUI.DataSource = ListUIDich;
                        GrdUI.DataBind();
                        GrdCalcolo.DataSource = ListUICalcolo;
                        GrdCalcolo.DataBind();

                       if ((MySession.Current.Ente.SIT.IsActive == 1 ? true : false))
                        {
                            try
                            {
                                string requestUriParam = String.Format("?cf_iva={0}&foglio={1}&mappale={2}&subalterno={3}&cod_ente={4}&eff={5}"
                                        , MySession.Current.myAnag.CodiceFiscale
                                        , string.Empty
                                        , string.Empty
                                        , string.Empty
                                        , MySession.Current.Ente.CodCatastale
                                        , ""
                                    );
                                string sErr = string.Empty;
                                ListUICatasto = new List<RiepilogoUI>();
                                Log.Debug("Richiamo catasto Url->"+ MySession.Current.Ente.SIT.Url + " :: Param->" + requestUriParam);
                                new BLL.RestService().MakeRequestBySoggetto<List<RiepilogoUI>>(MySession.Current.Ente.SIT.Url 
                                    , requestUriParam
                                    , result => ListUICatasto = result
                                    , result => ListDichUICatasto = result
                                    , error => sErr = error.Message
                                    , "Token: " + MySession.Current.Ente.SIT.Token
                                    );
                                if (sErr != string.Empty)
                                {
                                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.consultacatasto::errore::" + sErr);
                                    ListUICatasto = new List<RiepilogoUI>();
                                }
                                else
                                {
                                    foreach (RiepilogoUI myCat in ListUICatasto)
                                    {
                                        foreach (RiepilogoUI myCalc in ListUICalcolo)
                                        {
                                            if (myCat.Foglio == myCalc.Foglio && myCat.Numero == myCalc.Numero && myCat.Sub == myCalc.Sub)
                                            {
                                                string CatCatasto = myCat.CodCategoria;
                                                if (myCat.CodCategoria == myCalc.CodCategoria 
                                                    && myCat.PercPossesso == myCalc.PercPossesso && myCat.RenditaValore == myCalc.RenditaValore)
                                                {
                                                    myCat.Stato = "S";
                                                }
                                                else
                                                {
                                                    myCat.Stato = "D";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (HttpException ex)
                            {
                                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.consultacatasto::errore::", ex);
                                ListUICatasto = new List<RiepilogoUI>();
                            }

                            GrdCatasto.DataSource = ListUICatasto;
                            GrdCatasto.DataBind();
                            MySession.Current.ListRiepUICatasto = ListUICatasto;
                            MySession.Current.ListDichUICatasto = ListDichUICatasto;
                            ShowHide("divCatasto", true);
                        }

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
                        sScript += LoadForewordCalc(OnlyDich);
                        new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadBaseStato(ListUIDich, imgStato);
                    }
                }
                sScript += "$('#lblForewordDich').text('Elenco immobili presenti nella banca dati del Comune aggiornata al ');";
                sScript += "$('#lblForewordDichAgg').text('" + MySession.Current.Ente.DatiVerticali.AnnoVerticaleICI.ToString() + "');";
                sScript += "$('#lblForewordCalc').show();$('#lblForewordDich').show();$('#lblForewordDichAgg').show();";
                RegisterScript(sScript, this.GetType());
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoni(false);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Page_Load", "ingresso pagina", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                RegisterScript("$('#FAQ').addClass('HelpFORiepICI');", this.GetType());
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
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "IstanzaNew", "chiesto inserimento nuova istanza", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.NewIstanza::errore::", ex);
                LoadException(ex);
            }
        }
        protected void PrintDichiarazione(object sender, EventArgs e)
        {
            try
            {
                string sScriptDich = string.Empty;
                List<Istanza> ListIstRif = new List<Istanza>();
                ListIstRif.Add(new Istanza { IDTributo = General.TRIBUTO.ICI });
                if (!new BLL.Dichiarazioni(new Istanza()).StampaDichiarazione(MySession.Current.IDDichiarazioneIstanze, General.TRIBUTO.ICI, MySession.Current.Ente, MySession.Current.myAnag, out sScriptDich))
                {
                    string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.PrintDichiarazione::" + MySession.Current.IDDichiarazioneIstanze.ToString() + " - " + General.TRIBUTO.ICI.ToString() + " - " + MySession.Current.Ente.IDEnte.ToString() + " - " + MySession.Current.myAnag.ToString());
                    divRiepBody.InnerHtml = sScriptDich;
                    RegisterScript("$('#hfInitDich').val('0');", this.GetType());
                    ShowHide("divRiepilogo", true); ShowHide("divBase", false); ShowHide("divF24", false);
                    ManageBottoni(true);
                    string sNameHTMLtoPDF = "DICH" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                    string sNamePDF = "DICH_IMU_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf";
                    try {
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "stampa dichiarazione", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);

                    SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                    SelectPdf.PdfDocument doc = converter.ConvertUrl(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                    // save pdf document
                    doc.Save(UrlHelper.GetPathDichiarazioni + sNamePDF);
                    // close pdf document
                    doc.Close();

                    File.Copy(UrlHelper.GetPathDichiarazioni + sNamePDF
                            , UrlHelper.GetRepositoryPDF + sNamePDF);
                    File.Delete(UrlHelper.GetRepositoryPDF + sNamePDF);
                    RegisterScript("$('#myEmbedPDF').attr('src','" + UrlHelper.GetPathWebDichiarazioni + "DICH_IMU_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf');", this.GetType());

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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintDichiarazione", "invio mail dichiarazione da protocollare", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    }
                    catch (Exception ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.PrintDichiarazione.SendDichiarazione.Mail::errore::", ex);
                    }
                 }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.PrintDichiarazione::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.PrintF24::errore::", ex);
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
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "PrintPDF", "creazione pdf " + TypePDF, General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                }
                catch (Exception err)
                {
                    if (err.Message != "Thread was being aborted.")
                    {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.PrintPDF::errore::", err);
                        string sScript = "$('#OnlyNumber_error').text('Errore in stampa!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.PrintPDF::errore::", ex);
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
                        TotDovuto += decimal.Parse( myRow.Cells[12].Text);                        
                    }
                    string sScript = "$('#lblTotDovuto').text('Tot. Dovuto " + TotDovuto.ToString("#,##0.00") + " €');";
                    bool OnlyDich = true;
                    foreach (GridViewRow myRow in GrdCalcolo.Rows)
                    {
                        if (myRow.Cells[1].Text == "NO")
                        {
                            OnlyDich = false;
                            break;
                        }
                    }
                    sScript += LoadForewordCalc(OnlyDich);
                    RegisterScript(sScript, this.GetType());

                    ShowHide("divBase", true); ShowHide("divF24", false); ShowHide("divRiepilogo", false);
                    ManageBottoni(false);
                    hdTypePDF.Value = "";
                    File.Delete(UrlHelper.GetRepositoryPDF + "DICH_IMU_" + MySession.Current.myAnag.Cognome + "_" + MySession.Current.myAnag.Nome + "_" + ((MySession.Current.myAnag.PartitaIva != string.Empty) ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale) + ".pdf");
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita stampa pdf", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                }
                else
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Back", "uscita pagina", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                }
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
        protected void Storico(object sender, EventArgs e)
        {
            try
            {
                if (((System.Web.UI.WebControls.Button)sender).ID == "StoricoCat")
                {
                    MySession.Current.TipoStorico = "CAT";
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Storico", "consulta storico catasto", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetStoricoICI, Response);
                }
                else
                {
                    MySession.Current.TipoStorico = "DICH";
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Storico", "consulta storico dichiarazione", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetStoricoICI, Response);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.Storico::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ravvedimento(object sender, EventArgs e)
        {
            try
            {
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "Ravvedimento", "consulta Ravvedimento", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetRavvedimento, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.Ravvedimento::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GrdCalcoloRowDataBound::errore::", ex);
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
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GrdCalcoloRowCommand::errore::", ex);
                LoadException(ex);
            }
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
                    if (((HiddenField)e.Row.FindControl("hfIdRifOrg")).Value == "-1")
                    {
                        ((CheckBox)e.Row.FindControl("chkSel")).Enabled = false;
                    }
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtn($(this).attr('id'));");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GrdUIRowDataBound::errore::", ex);
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
                        MySession.Current.TipoIstanza = Istanza.TIPO.ConsultaDich;
                        MySession.Current.IdRifCalcolo = IDRow;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui dichiarazione", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GrdUIRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdCatastoRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GrdCatastoRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdCatastoRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "UIOpen":
                        MySession.Current.IdIstanza = -1;
                        MySession.Current.TipoIstanza = Istanza.TIPO.ConsultaCatasto;
                        MySession.Current.IdRifCalcolo = IDRow;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "UIOpen", "chiesto consultazione ui catasto", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response);
                        break;
                    case "RowNew":
                        MySession.Current.IdIstanza = -1;
                        MySession.Current.TipoIstanza = Istanza.TIPO.NuovaDichiarazione;
                        MySession.Current.IdRifCalcolo = -1;
                        foreach (RiepilogoUI myItem in MySession.Current.ListRiepUICatasto)
                        {
                            if (myItem.IDRifOrg == IDRow)
                            {
                                MySession.Current.UINewSuggest = myItem;                                
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Riepilogo", "RowNew", "chiesto inserimento nuova istanza da catasto", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                        IdentityHelper.RedirectToReturnUrl(GetRouteUrl("Immobile" + General.TRIBUTO.ICI, null), Response); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GrdCatastoRowCommand::errore::", ex);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.TipoF24CheckedChanged::errore::", ex);
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
                    RegisterScript("$('.BottoneF24').hide();$('p#F24').hide();$('.BottoneEdit').hide();$('p#Ravvedi').hide();", this.GetType());
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
                    RegisterScript("$('.BottoneF24').show();$('p#F24').show();$('.BottoneEdit').show();$('p#Ravvedi').show();", this.GetType());
                    if (MySession.Current.HasNewDich ==2)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.ManageBottoni::errore::", ex);
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

            try {
            if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadF24(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, General.TRIBUTO.ICI, DateTime.Now.Year, ((optUS.Checked) ? "U" : ((optSaldo.Checked) ? "S" : "A")), out ListUICalcolo))
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
                        hdTypePDF.Value = "F24";
                        new General().LogActionEvent(DateTime.Now, MySession.Current.Scope, MySession.Current.UserLogged.NameUser, "Tributi", "Riepilogo", "PrintF24", "stampa F24", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.LoadF24::errore::", ex);
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
                    if (myRow.uivar>0)
                        sScriptForPDF += "X";
                    sScriptForPDF += "'></div>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='acc' class='mgs1' value='";
                    if (myRow.acc>0)
                        sScriptForPDF += "X";
                    sScriptForPDF += "'></div>";
                    sScriptForPDF += "<div class='d_sem_ch'><input type='text' id='sal' class='mgs1' value='";
                    if (myRow.sal>0)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.GetPageF24::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OnlyDich"></param>
        /// <returns></returns>
        protected string LoadForewordCalc(bool OnlyDich)
        {
            string sScript = "";
            try {
                sScript += "$('#lblForewordCalc').html('Elenco immobili";
                if (OnlyDich)
                {
                    sScript += " banca dati comunale aggiornata al " + MySession.Current.Ente.DatiVerticali.AnnoVerticaleICI.ToString();
                }
                sScript += " su cui potrai apportare le modifiche per l’anno in corso.<br />";
                sScript += "<label class=\"text-11 text-italic\">Per inserire una nuova dichiarazione clicca sul tasto verde a destra. ";
                sScript += "Per fare una variazione clicca sulla casella posta alla sinistra del fabbricato da variare (colonna &lsquo;Sel.&rsquo;).</label>";
                sScript += "');";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.LoadForewordCalc::errore::", ex);
                sScript = string.Empty;
            }
            return sScript;
        }
    }
}