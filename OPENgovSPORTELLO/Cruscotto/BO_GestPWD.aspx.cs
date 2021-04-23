using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using System.IO;

namespace OPENgovSPORTELLO.Cruscotto
{
    /// <summary>
    /// Pagina lato BackOffice per la stampa password da inviare.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    /// <revisionHistory>
    /// <revision date="06/03/2019">
    /// <strong>Cancellazione registrazioni parziali</strong>
    /// Sarà aggiunta la nuova funzione UTENTI NON CONFERMATI.
    /// La videata permetterà di visualizzare ed estrarre in Excel l’elenco degli utenti registrati ma non confermati.
    /// Per ogni utente saranno visualizzati:
    /// <list type="bullet">
    /// <item>Mail</item>
    /// <item>Codice Fiscale</item>
    /// <item>Data registrazione</item>
    /// </list>	
    /// Per ogni singolo utente sarà disponibile il pulsante per la cancellazione della registrazione.
    /// </revision>
    /// </revisionHistory>
    public partial class BO_GestPWD : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_GestPWD));
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
                string sScript = "";
                sScript += "$('#divDownloadXLS').hide();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            int myType = (int)BLL.Analisi.TypeGestUser.GestPWD;
            try
            {
                myType = (RouteData.Values["TOGEST"] != null) ? int.Parse(RouteData.Values["TOGEST"].ToString()) : (int)BLL.Analisi.TypeGestUser.GestPWD;
                if (!Page.IsPostBack)
                {
                    if (myType == (int)BLL.Analisi.TypeGestUser.UtentiNonConfermati)
                    {
                        LoadUserNoConfirmed();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Utenti non confermati", "Page_Load", "ingresso pagina", "", "", "");
                    }
                    else
                    {
                        LoadPWD();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Password da inviare", "Page_Load", "ingresso pagina", "", "", "");
                    }
                }
                ManageShowHide(myType);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.Page_Load::errore::", ex);
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
            if (((RouteData.Values["TOGEST"] != null) ? int.Parse(RouteData.Values["TOGEST"].ToString()) : (int)BLL.Analisi.TypeGestUser.GestPWD) == (int)BLL.Analisi.TypeGestUser.UtentiNonConfermati)
            {
                MySession.Current.UserNoConfirmed = new List<UserRole>();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Utenti non confermati", "Back", "uscita pagina", "", "", "");
            }
            else {
                MySession.Current.GestPWD = new List<GestPWD>();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Password da inviare", "Back", "uscita pagina", "", "", "");
            }
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }
        #region Stampa
        /// <summary>
        /// Bottone per l'esportazione dei risultati in excel.
        /// La stampa sarà disponibile solo a fronte della ricerca analitica e per ogni tipo di Istanza saranno stampati i seguenti dati:
        /// •	Nominativo
        /// •	Codice Fiscale
        /// •	Tipo e Data Istanza
        /// Il file viene creato sul server e reso disponibile per il download tramite apposito pulsante.
        /// Viene utilizzata la .NET Library ClosedXML.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
        /// <revisionHistory>
        /// <revision date="05/04/2019">
        /// <strong>Sportello_GestioneRegistrazioni.docx</strong>
        /// </revision>
        /// </revisionHistory>
        protected void Print(object sender, EventArgs e)
        {
            string sScript = "";
            string PathNameFile = "";
            string UrlNameFile = "";
            try
            {
                List<object> ListToPrint = new List<object>();
                try
                {
                    var wb = new ClosedXML.Excel.XLWorkbook();
                    var ws = wb.Worksheets.Add("Foglio1");
                    if (((RouteData.Values["TOGEST"] != null) ? int.Parse(RouteData.Values["TOGEST"].ToString()) : (int)BLL.Analisi.TypeGestUser.GestPWD) == (int)BLL.Analisi.TypeGestUser.UtentiNonConfermati)
                    {
                        if (new BLL.Analisi().LoadUserNoConfirmed(new UserRoleStampa(), out ListToPrint))
                        {
                            System.Reflection.PropertyInfo[] properties = ListToPrint.First().GetType().GetProperties();
                            List<string> headerNames = properties.Select(prop => prop.Name).ToList();
                            for (int i = 0; i < headerNames.Count; i++)
                            {
                                ws.Cell(1, i + 1).Value = new BLL.Analisi().GetHeaderXLS(headerNames[i]);
                            }
                            ws.Cell(2, 1).InsertData(ListToPrint);
                            PathNameFile = "ElencoUtentiNonConfermati" + DateTime.Now.ToString("yyyyMMdd_mmss") + ".xlsx";
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Utenti non confermati", "Print", "stampa", "", "", "");
                        }
                    }
                    else
                    {
                        if (new BLL.Analisi().LoadGestPWD(new GestPWDStampa(), out ListToPrint))
                        {
                            System.Reflection.PropertyInfo[] properties = ListToPrint.First().GetType().GetProperties();
                            List<string> headerNames = properties.Select(prop => prop.Name).ToList();
                            for (int i = 0; i < headerNames.Count; i++)
                            {
                                ws.Cell(1, i + 1).Value = new BLL.Analisi().GetHeaderXLS(headerNames[i]);
                            }
                            ws.Cell(2, 1).InsertData(ListToPrint);
                            PathNameFile = "ElencoPassword" + DateTime.Now.ToString("yyyyMMdd_mmss") + ".xlsx";
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Password da inviare", "Print", "stampa", "", "", "");
                        }
                    }
                    if (PathNameFile != string.Empty)
                    {
                        UrlNameFile = UrlHelper.GetRepositoryPDFUrl + PathNameFile;
                        PathNameFile = UrlHelper.GetRepositoryPDF + PathNameFile;
                        wb.SaveAs(PathNameFile);
                        sScript += "$('#btnDownload').click(function(){window.open('" + UrlNameFile + "');});";
                        sScript += "$('#divDownloadXLS').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                    else
                    {
                        sScript = "$('#OnlyNumber_error').text('Nessun risultato!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
                catch (Exception ex)
                {
                    Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.Print::errore", ex);
                    LoadException(ex);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.Print::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// Funzione per settare come inviata la password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SetSend(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                foreach (GridViewRow myRow in GrdGestionePWD.Rows)
                {
                    if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                    {
                        if (!new BLL.Analisi().SetSendPWD(((HiddenField)myRow.FindControl("hfIdToSend")).Value))
                        {
                            sScript = "$('#OnlyNumber_error').text('Errore inserimento invio mail per :" + myRow.Cells[0].Text + "!');$('#OnlyNumber_error').show();";
                            RegisterScript(sScript, this.GetType());
                            return;
                        }
                    }
                }
                sScript = "$('#OnlyNumber_error').text('Salvataggio effettuato con successo!');$('#OnlyNumber_error').show();";
                RegisterScript(sScript, this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Gestione PWD", "SetSend", "uscita pagina", "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.SetSend::errore", ex);
                LoadException(ex);
            }
        }
        #region"Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUserNoConfirmedRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.GrdUserNoConfirmedRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUserNoConfirmedRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDRow;
                IDRow = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "RowDel":
                        if (new BLL.Analisi().DeleteUserNoConfirmed(IDRow) > 0)
                        {
                            LoadUserNoConfirmed();
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Utenti non Confermati", "RowDel", "cancellazione utente", "", IDRow, "");
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_GestPWD.GrdUserNoConfirmedRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione dell'ordinamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdGestPWDSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<GestPWD> ListSorted = MySession.Current.GestPWD;
                switch (e.SortExpression)
                {
                    case "Nominativo":
                        ListSorted = ListSorted.OrderBy(order => order.Nominativo).ToList();
                        break;
                    case "CodFiscalePIVA":
                        ListSorted = ListSorted.OrderBy(order => order.CodFiscalePIVA).ToList();
                        break;
                    case "Indirizzo":
                        ListSorted = ListSorted.OrderBy(order => order.Indirizzo).ToList();
                        break;
                }
                if (MySession.Current.SortDirection == SortDirection.Descending)
                    MySession.Current.SortDirection = SortDirection.Ascending;
                else
                    MySession.Current.SortDirection = SortDirection.Descending;

                if (MySession.Current.SortDirection == SortDirection.Descending)
                    ListSorted.Reverse();
                GrdGestionePWD.DataSource = ListSorted;
                GrdGestionePWD.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.GrdResultRowSorting::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione dell'ordinamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdUserNoConfirmedSorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<UserRole> ListSorted = MySession.Current.UserNoConfirmed;
                switch (e.SortExpression)
                {
                    case "Username":
                        ListSorted = ListSorted.OrderBy(order => order.NameUser).ToList();
                        break;
                    case "CodFiscalePIVA":
                        ListSorted = ListSorted.OrderBy(order => order.CFPIVA).ToList();
                        break;
                    case "LastPasswordChangedDate":
                        ListSorted = ListSorted.OrderBy(order => order.LastPasswordChangedDate).ToList();
                        break;
                }
                if (MySession.Current.SortDirection == SortDirection.Descending)
                    MySession.Current.SortDirection = SortDirection.Ascending;
                else
                    MySession.Current.SortDirection = SortDirection.Descending;

                if (MySession.Current.SortDirection == SortDirection.Descending)
                    ListSorted.Reverse();
                GrdGestionePWD.DataSource = ListSorted;
                GrdGestionePWD.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.GrdResultRowSorting::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// Funzione per la gestione degli oggetti da vedere/nascondere in base all'operazione
        /// </summary>
        /// <param name="myType">intero tipo di analisi</param>
        private void ManageShowHide(int myType)
        {
            try
            {
                string sScript = "";
                sScript += "document.getElementById('TitlePage').setAttribute('href', '/Cruscotto/BO_ReportGen.aspx');";
                if (myType == (int)BLL.Analisi.TypeGestUser.UtentiNonConfermati)
                {
                    sScript += "document.getElementById('TitlePage').innerHTML='Utenti non Confermati';";
                    sScript += "$('.BottoneAccept').hide();";
                    ShowHide("divUserNoConfirmed", true);
                    ShowHide("divGestPWD", false);
                }
                else {
                    sScript += "document.getElementById('TitlePage').innerHTML='Password da inviare';";
                    ShowHide("divGestPWD", true);
                    ShowHide("divUserNoConfirmed", false);
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.ManageShowHide::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void LoadPWD()
        {
            try
            {
                MySession.Current.SortDirection = SortDirection.Descending;
                List<object> ListMyData = new List<object>();
                if (new BLL.Analisi().LoadGestPWD(new GestPWD(), out ListMyData))
                {
                    MySession.Current.GestPWD = (ListMyData as IEnumerable<object>).Cast<GestPWD>().ToList();
                    GrdGestionePWD.DataSource = MySession.Current.GestPWD;
                    GrdGestionePWD.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadPWD::errore::", ex);
            }
        }
        /// <summary>
        /// Funzione di caricamento Utenti non Confermati
        /// </summary>
        private void LoadUserNoConfirmed()
        {
            try
            {
                MySession.Current.SortDirection = SortDirection.Descending;
                List<object> ListMyData = new List<object>();
                if (new BLL.Analisi().LoadUserNoConfirmed(new UserRole(), out ListMyData))
                {
                    MySession.Current.UserNoConfirmed = (ListMyData as IEnumerable<object>).Cast<UserRole>().ToList();
                    GrdUserNoConfirmed.DataSource = MySession.Current.UserNoConfirmed;
                    GrdUserNoConfirmed.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngSettings.LoadUserNoConfirmed::errore::", ex);
            }
        }
    }
}