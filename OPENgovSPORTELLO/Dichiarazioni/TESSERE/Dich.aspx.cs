using log4net;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OPENgovSPORTELLO.Dichiarazioni.TESSERE
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.TESSERE fncMng = new BLL.TESSERE();
        private static SPC_DichTESSERE UIOrg;

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
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.TESSERE, MySession.Current.Ente.IDEnte);

                sScript += "$('#lblLnkConf').hide();$('.BottoneTessera').hide();";
                sScript += "$('.lead_header').text('Dati Conferimenti');";
                sScript += "$('.lead_header').removeClass('col-md-2');";
                sScript += "$('.lead_header').addClass('col-md-5');";
                sScript += new BLL.GestForm().GetHelp("HelpFODichTESSERE", MySession.Current.Ente.UrlWiki);
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich.Page_Init::errore::", ex);
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

                if (!Page.IsPostBack)
                {
                    General fncGen = new General();
                    SPC_DichTESSERE myUIDich = new SPC_DichTESSERE();
                    List<GenericCategory> ListPeriodo = new BLL.Analisi().LoadPeriodo();
                    fncGen.LoadCombo(ddlPeriodo, ListPeriodo, "CODICE", "DESCRIZIONE");
                    ListPeriodo = new BLL.TESSERE().LoadTipoTessera();
                    fncGen.LoadCombo(ddlTipoTessera, ListPeriodo, "CODICE", "DESCRIZIONE");
                    if (MySession.Current.IdRifCalcolo > 0)
                    {
                        if (!fncMng.LoadDich(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, DateTime.Now.AddYears(-1), DateTime.Now, 3, 1, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {
                            LoadForm(myUIDich);
                            LoadColumnChart((myUIDich.ListConferimenti as IEnumerable<object>).Cast<object>().ToList());
                            LockControl();
                            MySession.Current.UIDichOld = myUIDich;

                            sScript = string.Empty;
                            sScript += GetLinkConferimenti();
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.TESSERE, "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (MySession.Current.Scope == "FO")
                    RegisterScript("$('#FAQ').addClass('HelpFOTESSERE');", this.GetType());
                else
                    RegisterScript("$('#FAQ').addClass('HelpBOTESSERE');", this.GetType());
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
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoTESSERE, Response);
        }
        /// <summary>
        /// 
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

                if (txtDal.Text == string.Empty || txtAl.Text == string.Empty || int.Parse(ddlPeriodo.SelectedValue) <= 0 || TxtNPeriodo.Text == string.Empty)
                {
                    string sScript = "$('#lblErrorFO').text('Inserire tutti i parametri per poter eseguire il confronto!');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else {
                    List<EventiRaffronto> ListEventi = new List<EventiRaffronto>();
                    ListEventi = fncMng.LoadTESSEREConferimenti(MySession.Current.Ente.IDEnte, MySession.Current.IdRifCalcolo, dataDal, dataAl, int.Parse(ddlPeriodo.SelectedValue), int.Parse(TxtNPeriodo.Text));
                    GrdConf.DataSource = ListEventi;
                    GrdConf.DataBind();
                    ListAnalisi = (ListEventi as IEnumerable<object>).Cast<object>().ToList();
                    LoadColumnChart(ListAnalisi);
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Cruscotto", "AnalisiIstanze", "Search", "Ricerca Raffronto", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich.Search::errore", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                RegisterScript(GetLinkConferimenti(), this.GetType());
                ManageBottoniera(General.TRIBUTO.TESSERE, string.Empty);
            }
        }
        #endregion
        #region "Link a Cartografia"
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetLinkConferimenti()
        {
            string myLink = string.Empty;
            try
            {
                if (MySession.Current.Ente.UrlConferimenti != string.Empty)
                {
                    myLink += "$('.BottoneTessera').on(\"click\",function(){";
                    myLink += "window.open('" + MySession.Current.Ente.UrlConferimenti + "', '_blank');";
                    myLink += "});";
                }
                else
                {
                    myLink += "$('.BottoneTessera').on(\"click\",function(){";
                    myLink += "$('#lblErrorFO').text('Sito Conferimenti non trovato! Impossibile aprire!');$('#lblErrorFO').show();";
                    myLink += "});";
                }
                string sScript = "$('#lblLnkConf').show();$('.BottoneTessera').show();";
                RegisterScript(sScript, this.GetType());
            }
            catch (HttpException ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich.GetLinkConferimenti::errore::", ex);
            }
            return myLink;
        }
        #endregion
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">SPC_DichTESSERE oggetto da caricare</param>
        private void LoadForm(SPC_DichTESSERE myItem)
        {
            try
            {
                txtNTessera.Text = myItem.NTessera;
                ddlTipoTessera.SelectedValue = myItem.IDTipoTessera.ToString();
                txtDataInizio.Text = new FunctionGrd().FormattaDataGrd(myItem.DataInizio);
                txtDataFine.Text = new FunctionGrd().FormattaDataGrd(myItem.DataFine);
                txtDal.Text = new FunctionGrd().FormattaDataGrd(DateTime.Now.AddYears(-1));
                txtAl.Text = new FunctionGrd().FormattaDataGrd(DateTime.Now);
                ddlPeriodo.SelectedValue = "3";
                TxtNPeriodo.Text = "1";
                GrdConf.DataSource = myItem.ListConferimenti;
                GrdConf.DataBind();
                UIOrg = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich.LoadForm::errore::", ex);
                throw new Exception();
            }
        }
        /// <summary>
        /// Funzione per l'abilitazione dei controlli
        /// </summary>
        private void LockControl()
        {
            txtNTessera.Enabled = false;
            ddlTipoTessera.Enabled = false;
            txtDataInizio.Enabled = false;
            txtDataFine.Enabled = false;
            ManageBottoniera(General.TRIBUTO.TESSERE, string.Empty);
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
                sScript += "[\"Periodo\", \"Litri\"]";
                foreach (object value in ListDati)
                {
                    string tipo = FncGrd.FormattaDataGrd(((EventiRaffronto)value).Dal) + "-" + FncGrd.FormattaDataGrd(((EventiRaffronto)value).Al);
                    decimal valore = ((EventiRaffronto)value).Valore;
                    sScript += ", ['" + tipo + "', " + valore.ToString().Replace(",", ".") + "]";
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich.Grafico::errore", ex);
                LoadException(ex);
            }
        }
    }
}
