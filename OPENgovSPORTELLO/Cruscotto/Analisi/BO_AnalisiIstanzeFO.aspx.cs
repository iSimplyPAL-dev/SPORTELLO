using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Cruscotto.Analisi
{
    /// <summary>
    /// La funzione ad uso del responsabile di back-office, consente il monitoraggio degli accessi lato contribuenti effettuate sul sito del front-office.
    /// Sono abilitati i seguenti criteri di selezione:
    /// <list type="bullet">
    /// <item>Ente</item>
    /// <item>Periodo dal …. al - controllo formale delle date</item>
    /// <item>Analitica – Sintetica - Raffronto Periodo(si intende estrazione sintetica con raggruppamento dei dati estratti per periodo) – selezione tramite check
    /// (Es.Raffronto periodo) accessi del mese di gennaio 2014 raffrontati con accessi mese di febbraio 2014)</item>
    /// <item>Tipo Periodo -  si abilita solo a fronte di selezione “Raffronto Periodo “e possono essere digitati n° mesi o n° GG.
    /// Nell’esempio sopra, saranno inseriti “periodo dal” 01-01-14 “al “28-02-14 e “tipo periodo” mese</item>
    /// <item>Tipo accesso - selezione da tabella</item>
    /// </list>
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_AnalisiIstanzeFO : BasePage
    {

        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_AnalisiIstanzeFO));
        protected FunctionGrd FncGrd = new FunctionGrd();
        General fncGen = new General();

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
                sScript += "$('.ParamRaffronto').hide();";
                sScript += "$('#divDownloadXLS').hide();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO.Page_Init::errore::", ex);
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
                    List<GenericCategory> ListUserEnti = new BLL.User(new UserRole() { NameUser = MySession.Current.UserLogged.NameUser, IDTipoProfilo = MySession.Current.UserLogged.IDTipoProfilo }).LoadUserEnti(string.Empty, MySession.Current.UserLogged.NameUser);
                    fncGen.LoadCombo(ddlEnte, ListUserEnti, "CODICE", "DESCRIZIONE");

                    List<GenericCategory> ListTipoEventi = new BLL.Analisi().LoadTipoEvento();
                    fncGen.LoadCombo(ddlTipoIstanze, ListTipoEventi, "CODICE", "DESCRIZIONE");

                    List<GenericCategory> ListPeriodo = new BLL.Analisi().LoadPeriodo();
                    fncGen.LoadCombo(ddlPeriodo, ListPeriodo, "CODICE", "DESCRIZIONE");

                    RegisterScript("$('.ParamRaffronto').hide();", this.GetType());
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Analisi Eventi", "Back", "ingresso pagina", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO.Page_Load::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per la ricerca
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search(object sender, EventArgs e)
        {
            try
            {
                List<object> ListAnalisi = new List<object>();
                DateTime dataDal, dataAl;
                dataDal = dataAl = DateTime.MaxValue;
                if (txtDal.Text != string.Empty)
                {
                    dataDal = DateTime.Parse(txtDal.Text);
                }
                if (txtAl.Text != string.Empty)
                {
                    dataAl = DateTime.Parse(txtAl.Text);
                }

                if (optSintetica.Checked)
                {
                    GrdSynthetic.Visible = true; GrdSyntheticNoTrib.Visible = true; GrdAnalytic.Visible = false; GrdCompare.Visible = false;
                    List<Eventi> ListEventi = new BLL.Analisi().LoadEventi(ddlEnte.SelectedValue, dataDal, dataAl, int.Parse(ddlTipoIstanze.SelectedValue), false);
                    GrdSyntheticNoTrib.DataSource = ListEventi;
                    GrdSyntheticNoTrib.DataBind();
                    if (ListEventi.Count > 0)
                        GrdSyntheticNoTrib.Visible = true;
                    else
                        GrdSyntheticNoTrib.Visible = false;
                    ListEventi = new BLL.Analisi().LoadEventi(ddlEnte.SelectedValue, dataDal, dataAl, int.Parse(ddlTipoIstanze.SelectedValue), true);
                    GrdSynthetic.DataSource = ListEventi;
                    GrdSynthetic.DataBind();
                    if (ListEventi.Count > 0)
                    {
                        GrdSynthetic.Visible = true;
                        ListAnalisi = (ListEventi as IEnumerable<object>).Cast<object>().ToList();
                        LoadPieChart(ListAnalisi);
                    }
                    else {
                        GrdSynthetic.Visible = false;
                    }
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiIstanze", "Search", "Ricerca Sintetica", "", "", "");
                }
                else if (optAnalitica.Checked)
                {
                    ShowHide("chart_div", false);
                    GrdSynthetic.Visible = false; GrdSyntheticNoTrib.Visible = false; GrdAnalytic.Visible = true; GrdCompare.Visible = false;
                    if (new BLL.Analisi().LoadEventiAnalitica(ddlEnte.SelectedValue, dataDal, dataAl, int.Parse(ddlTipoIstanze.SelectedValue), new EventiAnalitica(), out ListAnalisi))
                    {
                        GrdAnalytic.DataSource = (ListAnalisi as IEnumerable<object>).Cast<EventiAnalitica>().ToList();
                        GrdAnalytic.DataBind();
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiIstanze", "Search", "Ricecac Analitica", "", "", "");
                    }
                }
                else
                {
                    GrdSynthetic.Visible = false; GrdSyntheticNoTrib.Visible = false; GrdAnalytic.Visible = false; GrdCompare.Visible = true;
                    RegisterScript("$('.ParamRaffronto').show();", this.GetType());
                    if (ddlEnte.SelectedValue == string.Empty || txtDal.Text == string.Empty || txtAl.Text == string.Empty || int.Parse(ddlTipoIstanze.SelectedValue) < 0 || int.Parse(ddlPeriodo.SelectedValue) <= 0 || TxtNPeriodo.Text == string.Empty)
                    {
                        string sScript = "$('#OnlyNumber_error').text('Inserire tutti i parametri per poter eseguire il confronto!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                    else {
                        List<EventiRaffronto> ListEventi = new List<EventiRaffronto>();
                        ListEventi = new BLL.Analisi().LoadEventiRaffronto(ddlEnte.SelectedValue, dataDal, dataAl, int.Parse(ddlTipoIstanze.SelectedValue), int.Parse(ddlPeriodo.SelectedValue), int.Parse(TxtNPeriodo.Text));
                        GrdCompare.DataSource = ListEventi;
                        GrdCompare.DataBind();
                        ListAnalisi = (ListEventi as IEnumerable<object>).Cast<object>().ToList();
                        LoadColumnChart(ListAnalisi);
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiIstanze", "Search", "Ricerca Raffronto", "", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO.Search::errore", ex);
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
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Analisi Eventi", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListDati"></param>
        private void LoadPieChart(List<object> ListDati)
        {
            try
            {
                string sScript = "";

                sScript = "<script language='javascript' type='text/javascript'>";
                // Load the Visualization API and the corechart package.
                sScript += "google.charts.load('current', { 'packages': ['corechart']});";

                // Set a callback to run when the Google Visualization API is loaded.
                sScript += "google.charts.setOnLoadCallback(drawChart);";

                // Callback that creates and populates a data table,
                // instantiates the pie chart, passes in the data and
                // draws it.
                sScript += "function drawChart()";
                sScript += "{";
                // Create the data table.
                sScript += "var data = new google.visualization.arrayToDataTable([";
                sScript += "['', '']";
                foreach (object value in ListDati)
                {
                    string tipo = ((Eventi)value).TipoAccesso;
                    int numero = ((Eventi)value).Numero;
                    sScript += ", ['" + tipo + "', " + numero + "]";
                }
                sScript += "]);";
                // Set chart options
                sScript += "var options = { 'title': ''";
                sScript += ",backgroundColor: 'transparent'";
                sScript += ",is3D: true,";
                sScript += "};";

                // Instantiate and draw our chart, passing in some options.
                sScript += "var chart = new google.visualization.PieChart(document.getElementById('chart_div'));";
                sScript += "chart.draw(data, options);";
                sScript += "}";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "drawchart" + DateTime.Now.ToLongDateString(), sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO.Grafico::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListDati"></param>
        private void LoadColumnChart(List<object> ListDati)
        {
            try
            {
                string sScript = "";

                sScript = "<script language='javascript' type='text/javascript'>";
                // Load the Visualization API and the corechart package.
                sScript += "google.charts.load('current', {'packages':['bar']});";

                // Set a callback to run when the Google Visualization API is loaded.
                sScript += "google.charts.setOnLoadCallback(drawChart);";

                // Callback that creates and populates a data table,
                // instantiates the pie chart, passes in the data and
                // draws it.
                sScript += "function drawChart()";
                sScript += "{";
                // Create the data table.
                sScript += "var data = new google.visualization.arrayToDataTable([";
                sScript += "[\"Periodo\", \"N.Istanze\"]";
                foreach (object value in ListDati)
                {
                    string tipo = FncGrd.FormattaDataGrd(((EventiRaffronto)value).Dal) + "-" + FncGrd.FormattaDataGrd(((EventiRaffronto)value).Al);
                    int numero = ((EventiRaffronto)value).Numero;
                    sScript += ", ['" + tipo + "', " + numero + "]";
                }
                sScript += "]);";
                // Set chart options
                sScript += "var options = {";
                sScript += "title: ''";
                sScript += ",backgroundColor: 'transparent'";
                //sScript += ",width: 900";
                sScript += ",legend: { position: 'none' }";
                sScript += ",chart: { subtitle: '' }";
                sScript += ",axes:{x:{0: { side: 'bttom', label: ''} }}";// Top x-axis.
                sScript += ",bar: { groupWidth: \"90 %\" }";
                sScript += "};";

                // Instantiate and draw our chart, passing in some options.
                sScript += "var chart = new google.charts.Bar(document.getElementById('chart_div'));";
                sScript += "chart.draw(data, google.charts.Bar.convertOptions(options));";
                sScript += "}";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "drawchart" + DateTime.Now.ToLongDateString(), sScript);
            }

            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO.Grafico::errore", ex);
                LoadException(ex);
            }
        }
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
        protected void ExportXLS(object sender, EventArgs e)
        {
            try
            {
                if (!optAnalitica.Checked)
                {
                    string sScript = "$('#OnlyNumber_error').text('L\\'estrazione e\\' disponibile solo per la ricerca Analitica!');$('#OnlyNumber_error').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    List<object> ListEventi = new List<object>();
                    DateTime dataDal, dataAl;
                    dataDal = dataAl = DateTime.MaxValue;
                    if (txtDal.Text != string.Empty)
                    {
                        dataDal = DateTime.Parse(txtDal.Text);
                    }
                    if (txtAl.Text != string.Empty)
                    {
                        dataAl = DateTime.Parse(txtAl.Text);
                    }
                    if (new BLL.Analisi().LoadEventiAnalitica(ddlEnte.SelectedValue, dataDal, dataAl, int.Parse(ddlTipoIstanze.SelectedValue), new EventiAnaliticaStampa(), out ListEventi))
                    {
                        var wb = new ClosedXML.Excel.XLWorkbook();
                        var ws = wb.Worksheets.Add("Foglio1");
                        System.Reflection.PropertyInfo[] properties = ListEventi.First().GetType().GetProperties();
                        List<string> headerNames = properties.Select(prop => prop.Name).ToList();
                        for (int i = 0; i < headerNames.Count; i++)
                        {
                            ws.Cell(1, i + 1).Value = new BLL.Analisi().GetHeaderXLS(headerNames[i]);
                        }
                        ws.Cell(2, 1).InsertData(ListEventi);
                        string PathNameFile = "AnalisiIstanze" + DateTime.Now.ToString("yyyyMMdd_mmss") + ".xlsx";
                        string UrlNameFile = UrlHelper.GetRepositoryPDFUrl + PathNameFile;
                        PathNameFile = UrlHelper.GetRepositoryPDF + PathNameFile;
                        wb.SaveAs(PathNameFile);
                        string sScript = "";
                        sScript += "$('#btnDownload').click(function(){window.open('" + UrlNameFile + "');});";
                        sScript += "$('#divDownloadXLS').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                    else
                    {
                        string sScript = "$('#OnlyNumber_error').text('Nessun risultato!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO.ExportXLS::errore::", ex);
                LoadException(ex);
            }
        }
    }
}