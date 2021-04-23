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
    /// Il responsabile di back-office avrà  a disposizione un cruscotto con il quale potrà verificare lo stato di tutte le pratiche attive e di quelle archiviate, potrà effettuare delle statistiche e delle analisi ad hoc.
    /// Il Cruscotto attività produrrà anche reports riepilogativi delle attività gestite con l’applicazione.
    /// La videata di apertura consente la selezione dei seguenti criteri:
    /// <list type="bullet">
    /// <item>Ente -  può essere selezionato un Ente, 2Enti(alcuni enti??) o tutti gli Enti</item>
    /// <item>Dal – al</item>
    /// <item>Analitico – sintetico, dove nel caso di analitico il sistema estrae il numero di istanze suddivise per tipo di istanza e sintetico estrae le istanze in base all’esito.</item>
    /// </list>
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_AnalisiIstanzeBO : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_AnalisiIstanzeBO));
        protected FunctionGrd FncGrd = new FunctionGrd();

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
                    List<GenericCategory> ListUserEnti = new BLL.User(new UserRole() { NameUser = MySession.Current.UserLogged.NameUser, IDTipoProfilo = MySession.Current.UserLogged.IDTipoProfilo }).LoadUserEnti(string.Empty, MySession.Current.UserLogged.NameUser);
                    if (ListUserEnti.Count == 1)
                    {
                        ListUserEnti[0].IsActive = 1;
                    }
                    GrdEnti.DataSource =ListUserEnti;
                    GrdEnti.DataBind();
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Attività", "Back", "ingresso pagina", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.Page_Load::errore::", ex);
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
            List<Attivita> ListStati = new List<Attivita>();
            List<TempiMedi> ListTempiMedi = new List<TempiMedi>();
            List<AttivitaAnalitica> ListAttivitaAnalitica = new List<AttivitaAnalitica>();
            DateTime dataDal, dataAl;
            try
            {
            dataDal = dataAl = DateTime.MaxValue;
            if (txtDal.Text != string.Empty)
            {
                dataDal = DateTime.Parse(txtDal.Text);
            }
            if (txtAl.Text != string.Empty)
            {
                dataAl = DateTime.Parse(txtAl.Text);
            }
            string ListEnti = string.Empty;
            foreach (GridViewRow myRow in GrdEnti.Rows)
            {
                if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                    ListEnti += ((ListEnti != string.Empty) ? "," : string.Empty) + ((HiddenField)myRow.FindControl("hfCodice")).Value;
            }
                if (ListEnti == string.Empty)
                {
                    string sScript = "$('#OnlyNumber_error').text('Selezionare almeno un Ente!');$('#OnlyNumber_error').show();";
                    RegisterScript(sScript, this.GetType());                    
                }
                else {
                    if (optRaffronto.Checked && ListEnti.Split(char.Parse(",")).ToList<string>().Count<2 )
                    {
                        string sScript = "$('#OnlyNumber_error').text('Selezionare almeno due Enti!');$('#OnlyNumber_error').show();";
                        RegisterScript(sScript, this.GetType());
                        return;
                    }
                    if (optSintetica.Checked || optRaffronto.Checked)
                    {
                        GrdAnalytic.Visible = false;GrdStati.Visible = true;GrdTempi.Visible = true;
                        ListStati = new BLL.Analisi().LoadAttivita(ListEnti, dataDal, dataAl, optRaffronto.Checked);
                        GrdStati.DataSource = ListStati;
                        GrdStati.DataBind();
                        ListTempiMedi = new BLL.Analisi().LoadTempiMedi(ListEnti, dataDal, dataAl, optRaffronto.Checked);
                        GrdTempi.DataSource = ListTempiMedi;
                        GrdTempi.DataBind();
                        if (optSintetica.Checked)
                        {
                            RegisterScript("$('#divGrdStati').removeClass('col-md-12'); $('#divGrdStati').addClass('col-md-4');", this.GetType());
                            RegisterScript("$('#divGrdTempi').removeClass('col-md-12'); $('#divGrdTempi').addClass('col-md-6');", this.GetType());
                            RegisterScript("$('#piechart_div').show();$('#barchart_div').show();", this.GetType());
                            LoadMultipleChart(ListStati, ListTempiMedi);
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiEventi", "Search", "Ricerca Sintetica", "", "", "");
                        }
                        else
                        {
                            RegisterScript("$('#divGrdStati').removeClass('col-md-4'); $('#divGrdStati').addClass('col-md-12');", this.GetType());
                            RegisterScript("$('#divGrdTempi').removeClass('col-md-6'); $('#divGrdTempi').addClass('col-md-12');", this.GetType());
                            RegisterScript("$('#piechart_div').hide();$('#barchart_div').hide();", this.GetType());
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiEventi", "Search", "Ricerca Raffronto", "", "", "");
                        }
                    }
                    else if (optAnalitica.Checked)
                    {
                        GrdAnalytic.Visible = true; GrdStati.Visible = false; GrdTempi.Visible = false;
                        ShowHide("Synthetic", false);
                        ListAttivitaAnalitica = new BLL.Analisi().LoadAttivitaAnalitica(ListEnti, dataDal, dataAl);
                        GrdAnalytic.DataSource = ListAttivitaAnalitica;
                        GrdAnalytic.DataBind();

                        for (int x = 4; x <= 8; x++)
                            GrdAnalytic.Columns[x].Visible = false;
                        List<string> AllTributi = new BLL.Settings().LoadTributiGestiti(string.Empty);
                        foreach (string myTrib in AllTributi)
                        {
                            if (myTrib == General.TRIBUTO.ICI)
                                GrdAnalytic.Columns[4].Visible = true;
                            else if (myTrib == General.TRIBUTO.TASI)
                                GrdAnalytic.Columns[5].Visible = true;
                            else if (myTrib == General.TRIBUTO.TARSU)
                                GrdAnalytic.Columns[6].Visible = true;
                            else if (myTrib == General.TRIBUTO.OSAP)
                                GrdAnalytic.Columns[7].Visible = true;
                            else if (myTrib == General.TRIBUTO.ICP)
                                GrdAnalytic.Columns[8].Visible = true;
                        }

                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiEventi", "Search", "Ricerca Analitica", "", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.Search::errore", ex);
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
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "Attività", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_ReportGen, Response);
        }

        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdStatiRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try {
                int x = 0;
                if (optRaffronto.Checked)
                {
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                }
                else {

                    GrdStati.Columns[x].Visible =false ;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = true;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                    GrdStati.Columns[x].Visible = false;
                    x++;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.GrdStatiRowDataBound::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdTempiRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                int x = 0;
                if (optRaffronto.Checked)
                {
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                    GrdTempi.Columns[x].Visible = false;
                    x++;
                    GrdTempi.Columns[x].Visible = false;
                    x++;
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                }
                else {

                    GrdTempi.Columns[x].Visible = false;
                    x++;
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                    GrdTempi.Columns[x].Visible = true;
                    x++;
                    GrdTempi.Columns[x].Visible = false;
                    x++;
                    GrdTempi.Columns[x].Visible = false;
                    x++;
                    GrdTempi.Columns[x].Visible = false;
                    x++;
                    GrdTempi.Columns[x].Visible = false;
                    x++;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.GrdTempiRowDataBound::errore", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListStati"></param>
        /// <param name="ListTempiMedi"></param>
        private void LoadMultipleChart(List<Attivita> ListStati, List<TempiMedi> ListTempiMedi)
        {
            string sScript = "";
            try
            {
                sScript = "<script language='javascript' type='text/javascript'>";
                // Load Charts and the corechart and barchart packages.
                sScript += "google.charts.load('current', {'packages':['corechart']});";
                // Draw the pie chart and bar chart when Charts is loaded.
                sScript += "google.charts.setOnLoadCallback(drawChart);";
                sScript += "function drawChart() {";
                sScript += LoadPieChart((ListStati as IEnumerable<object>).Cast<object>().ToList());
                sScript += LoadBarChart((ListTempiMedi as IEnumerable<object>).Cast<object>().ToList());
                sScript += "}";
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "drawchart" + DateTime.Now.ToLongDateString(), sScript);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.LoadMultipleChart::errore", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListDati"></param>
        /// <returns></returns>
        private string LoadPieChart(List<object> ListDati)
        {
            string sScript, sSliches;
            sScript = sSliches = "";
            try
            {
                sScript += "var data = new google.visualization.DataTable();";
                sScript += "data.addColumn('string', 'Stati');";
                sScript += "data.addColumn('number', 'N.Istanze');";
                sScript += "data.addRows([";
                foreach (object value in ListDati)
                {
                    sSliches += ((sSliches != string.Empty) ? "," : string.Empty) + "['" + ((Attivita)value).Descrizione.Replace("'", " ") + "', " + ((Attivita)value).NIstanze + "]";
                }
                sScript += sSliches;
                sScript += "]);";

                sScript += "var piechart_options = {title:''";
                sScript += ",backgroundColor: 'transparent'";
                sScript += ",is3D: true";
                sScript += ",width:600";
                sScript += ",height:250";
                sScript += "};";
                sScript += "var piechart = new google.visualization.PieChart(document.getElementById('piechart_div'));";
                sScript += "piechart.draw(data, piechart_options);";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.LoadPieChart::errore", ex);
                sScript = string.Empty;
            }
            return sScript;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListDati"></param>
        /// <returns></returns>
        private string LoadBarChart(List<object> ListDati)
        {
            string sScript, sSliches;
            sScript = sSliches = "";
            try
            {
                sScript += "var data = new google.visualization.DataTable();";
                sScript += "data.addColumn('string', '');";
                sScript += "data.addColumn('number', 'Tempi Medi');";
                sScript += "data.addColumn({type:'string', role:'annotation'});"; // annotation role col.
                sScript += "data.addRows([";
                foreach (object value in ListDati)
                {
                    sSliches += ((sSliches != string.Empty) ? "," : string.Empty) + "['', " + ((TempiMedi)value).GG.ToString().Replace(",",".") + ",'" + ((TempiMedi)value).Descrizione.Replace("'", "\'") + "']";
                }
                sScript += sSliches;
                sScript += "]);";

                sScript += "var barchart_options = {title:''";
                sScript += ",backgroundColor: 'transparent'";
                sScript += ",legend: 'none'";
                sScript += "};";
                sScript += "var barchart = new google.visualization.BarChart(document.getElementById('barchart_div'));";
                sScript += "barchart.draw(data, barchart_options);";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO.LoadBarChart::errore", ex);
                sScript = string.Empty;
            }
            return sScript;
        }
    }
}