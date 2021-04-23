using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using Microsoft.AspNet.Identity;
using SelectPdf;
using System.IO;

namespace OPENgovSPORTELLO.Istanze
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FO_GestDich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FO_GestDich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private static Istanza myDichiarazione;
        private static List<Istanza> ListIstanze;

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
                    if (!new BLL.Dichiarazioni(new Istanza()).LoadDichiarazione(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IDDichiarazioneIstanze, out ListIstanze))
                        RegisterScript("Errore in caricamento pagina", this.GetType());
                    else {
                        if (ListIstanze.Count <= 0)
                            RegisterScript("Errore in caricamento pagina", this.GetType());
                        else {
                            myDichiarazione = ListIstanze[0];
                            if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, DateTime.MaxValue, string.Empty, string.Empty, string.Empty, string.Empty, -1, MySession.Current.IDDichiarazioneIstanze, false, out ListIstanze))
                                RegisterScript("Errore in caricamento pagina", this.GetType());
                            else {
                                LoadForm();
                                LockControl();
                                GrdIstanze.DataSource = ListIstanze;
                                GrdIstanze.DataBind();
                                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                            }
                        }
                    }
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "GestDich", "Page_Load", "ingresso pagina", "", "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();$('.BottonePDF').hide();", this.GetType());
                ShowHide("divRiepilogo", false);
                ManageBottoni();
            }
        }
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdIstanzeRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    ((CheckBox)e.Row.FindControl("chkSel")).Attributes.Add("onclick", "ShowHideGrdBtn($(this).attr('id'));");
                }
                GrdIstanze.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.GrdUIRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
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
                         break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.GrdIstanzeRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #region "Bottoni"
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
                if (!new BLL.Dichiarazioni(new Istanza()).StampaDichiarazione(MySession.Current.IDDichiarazioneIstanze, ListIstanze[0].IDTributo, MySession.Current.Ente, MySession.Current.myAnag, out sScriptDich))
                {
                    string sScript = "$('#lblErrorFO').text('Errore in stampa!');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    string sNameHTMLtoPDF = "DICH" + Request.Cookies["__AntiXsrfToken"].Value + ".html";
                    using (StreamWriter writetext = new StreamWriter(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF))
                    {
                        writetext.WriteLine(sScriptDich);
                        writetext.Close();
                    }
                    try
                    {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dichiarazione", "PrintDichiarazione", "stampa dichiarazione", "", "", MySession.Current.Ente.IDEnte);
                        SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();
                         SelectPdf.PdfDocument doc = converter.ConvertUrl(UrlHelper.GetRepositoryPDF + sNameHTMLtoPDF);
                        // save pdf document
                        doc.Save(Response, false, "DICH.pdf");
                        // close pdf document
                        doc.Close();
                    }
                    catch (Exception err)
                    {
                        if (err.Message != "Thread was being aborted.")
                        {
                            Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.PrintPDF::errore::", err);
                            string sScript = "$('#lblErrorFO').text('Errore in stampa!');$('#lblErrorFO').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.PrintDichiarazione::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SendDichiarazione(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string FilesToAttach = string.Empty;
            List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
            try
            {
                HttpFileCollection ListFiles = Request.Files;
                List<IstanzaAllegato> ListDichAttach = myDichiarazione.ListAllegati;
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Dichiarazione, ref ListMailAttachments, ref ListDichAttach))
                {
                    sScript = "$('#lblErrorFO').text('Errore in lettura allegati!');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    myDichiarazione.ListAllegati = ListDichAttach;
                    if (myDichiarazione.ListAllegati.Count <= 0)
                    {
                        sScript = "$('#lblErrorFO').text('Inserire gli allegati!');$('#lblErrorFO').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                    else {
                        if (MySettings.GetConfig("TypeProtocollo") == "E")
                        {
                            try
                            {
                                string sErr = string.Empty;
                                List<string> ListRecipient = new List<string>();
                                ListRecipient.Add(MySession.Current.Ente.MailEnte);
                                string TestoMail = "Per il comune di " + MySession.Current.Ente.Descrizione + ", è stata inserita la dichiarazione N."+ MySession.Current.Ente.IDEnte+"/"+MySession.Current.IDDichiarazioneIstanze.ToString() + ", in allegato, da protocollare.";
                                new EmailService().CreateMail(MySession.Current.Ente.Mail, ListRecipient, new List<string>() { MySession.Current.Ente.Mail.Archive }
                                    , "Sportello - Dichiarazione in attesa di protocollo per il contribuente "
                                        +(MySession.Current.myAnag.Cognome+" "+MySession.Current.myAnag.Nome).Trim()
                                        +" Cod.Fiscale/P.IVA: "
                                        + (MySession.Current.myAnag.PartitaIva==string.Empty?MySession.Current.myAnag.CodiceFiscale:MySession.Current.myAnag.PartitaIva)
                                    , TestoMail, ListMailAttachments, out sErr);
                            }
                            catch (Exception ex)
                            {
                                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.SendDichiarazione.Mail::errore::", ex);
                            }
                        }
                        foreach (IstanzaAllegato myAttach in myDichiarazione.ListAllegati)
                        {
                            myAttach.IDIstanza = myDichiarazione.NDichiarazione;
                            if (!new BLL.IstanzaAllegati(myAttach).Save(MySession.Current.Ente.IDEnte))
                            {
                                sScript += "$('#lblErrorFO').text('Errore in salvataggio allegati!');$('#lblErrorFO').show();";
                                RegisterScript(sScript, this.GetType());
                                break;
                            }
                        }
                        foreach (Istanza myIst in ListIstanze)
                        {
                            myIst.DataInvioDichiarazione = DateTime.Now;                          
                            myIst.NDichiarazione = myDichiarazione.NDichiarazione;
                            if (txtNote.Text != string.Empty)
                                myIst.ListComunicazioni.Add(new IstanzaComunicazione() { IDIstanza = myIst.IDIstanza, IDTipo = Istanza.STATO.Inviata, Data = DateTime.Now, DataLettura=DateTime.Now, Testo = txtNote.Text, ListAllegati = ListDichAttach });
                            if (!new BLL.Istanze(myIst, MySession.Current.UserLogged.ID).Save())
                            {
                                sScript += "$('#lblErrorFO').text('Errore in invio dichiarazione!');$('#lblErrorFO').show();";
                                RegisterScript(sScript, this.GetType());
                                break;
                            }
                        }
                        sScript += "$('#lblErrorFO').text('Dichiarazione inviata con successo!');$('#lblErrorFO').show();";
                        RegisterScript(sScript, this.GetType());
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message != "Thread was being aborted.")
                {
                    Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.SendDichiarazione::errore::", ex);
                    LoadException(ex);
                }
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
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
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.Back::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        protected void ManageBottoni()
        {
            try
            {
                RegisterScript("$('.BottoneMailBox').show();", this.GetType());
                RegisterScript("$('.BottonePrint').show();", this.GetType());
                RegisterScript("$('.BottoneBack').show();", this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.ManageBottoni::errore::", ex);
            }
        }
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        private void LoadForm()
        {
            try
            {
                string sScript = "";
                sScript += "document.getElementById('lblDescrTributo').innerText='Tributo: " + myDichiarazione.DescrTributo + "';";
                sScript += "document.getElementById('lblDataPresentazione').innerText='Data Registrazione: " + new FunctionGrd().FormattaDataGrd(myDichiarazione.DataPresentazione) + "';";                
                RegisterScript(sScript, this.GetType());

                sScript = string.Empty;
                foreach (IstanzaAllegato myAttach in myDichiarazione.ListAllegati)
                {
                    sScript += "<p>" + myAttach.FileName + "</p>";
                }
                divAllegati.InnerHtml = sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.FO_GestDich.LoadForm::errore::", ex);
                throw new Exception();
            }
        }
        /// <summary>
        /// Funzione per l'abilitazione dei controlli
        /// </summary>
        private void LockControl()
        {
            if (myDichiarazione.DataInvioDichiarazione.Date != DateTime.MaxValue.Date)
                RegisterScript("$('.BottoneInbox').hide();", this.GetType());
        }
        #endregion
    }
}