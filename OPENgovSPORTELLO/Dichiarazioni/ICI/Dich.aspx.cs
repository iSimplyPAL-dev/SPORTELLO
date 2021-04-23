using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;
using Microsoft.AspNet.Identity;

namespace OPENgovSPORTELLO.Dichiarazioni.ICI
{
    /// <summary>
    /// L’iter di inserimento di dichiarazione nuova o di modifica di un immobile è lo stesso. 
    /// Al salvataggio genera report riepilogativo da allegare a mail.
    /// Sulla base dei dati presenti nella sezione di calcolo, il sistema effettua il calcolo del dovuto IMU/TASI.
    /// Visualizza il dettaglio del dovuto, e dopo conferma stampa i modelli F24.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.ICI fncMng = new BLL.ICI();
        private static SPC_DichICI UIOrg;
        private static string MailUser;
        private static Istanza myIstanza = new Istanza();
        private static List<System.Web.Mail.MailAttachment> ListIstanzaMailAttachments = new List<System.Web.Mail.MailAttachment>();
        private static List<IstanzaAllegato> ListIstanzaDichAttach = new List<IstanzaAllegato>();
        private static List<GenericCategoryWithRate> ListVincoliAttr = new List<GenericCategoryWithRate>();
        private string UrlGoToGIS = string.Empty;

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
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.ICI, MySession.Current.Ente.IDEnte);

                sScript += "$('#divDatiIstanza').hide();";
                sScript += "$('.BottoneCounter').hide();$('.BottoneWork').hide();$('.BottoneStop').hide();$('.BottoneAccept').hide();";
                sScript += "$('#lblSIT').hide();$('.BottoneMap').hide();";
                sScript += "$('p#Protocolla').hide();$('p#InCarico').hide();$('p#Respingi').hide();$('p#Valida').hide();";
                sScript += "$('.lead_header').text('Dati Immobile');";
                sScript += "$('.lead_header').removeClass('col-md-2');";
                sScript += "$('.lead_header').addClass('col-md-5');";
                sScript += "$('.BottoneDiv').hide();";

                if (MySession.Current.Scope == "BO" && MySession.Current.Ente.DatiVerticali.TipoBancaDati == "I")
                    sScript += "$('.BottoneSort').show();$('p#Ribalta').show();";
                else
                    sScript += "$('.BottoneSort').hide();$('p#Ribalta').hide();";

                sScript += new BLL.GestForm().GetHelp("HelpFODichICI", MySession.Current.Ente.UrlWiki);

                RegisterScript(sScript, this.GetType());
                hfTypeProtocollo.Value = MySettings.GetConfig("TypeProtocollo");
                ShowHide("divComproprietari", false);
                ddlZona.Enabled = false; GrdVincoli.Enabled = false; ShowHide("divVincoli", false);// ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdVincoli", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Page_Init::errore::", ex);
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
                SPC_DichICI myUIDich = new SPC_DichICI();
                if (MySession.Current.IsInitDich)
                    RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                if (!Page.IsPostBack)
                {
                    string[] keys = Request.Form.AllKeys;
                    var value = "";
                    for (int i = 0; i < keys.Length; i++)
                    {
                        value = Request.Form[keys[i]];
                    }

                    General fncGen = new General();
                    List<GenericCategory> ListGenTipo = new List<GenericCategory>();
                    List<GenericCategory> ListGenCat = new List<GenericCategory>();
                    List<GenericCategory> ListGenZona = new List<GenericCategory>();
                    List<GenericCategory> ListGenClasse = new List<GenericCategory>();
                    List<GenericCategory> ListGenPossesso = new List<GenericCategory>();
                    List<GenericCategory> ListGenMotivazioni = new List<GenericCategory>();

                    ListGenTipo = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Caratteristica, string.Empty, General.TRIBUTO.ICI);
                    ListGenCat = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Categorie, string.Empty, string.Empty);
                    ListGenZona = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Zone, string.Empty, string.Empty);
                    ListGenClasse = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Classe, string.Empty, string.Empty);
                    ListGenPossesso = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Possesso, string.Empty, string.Empty);

                    ListGenMotivazioni = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Motivazioni, string.Empty, string.Empty);

                    fncGen.LoadCombo(ddlTipologia, ListGenTipo, "CODICE", "DESCRIZIONE");
                    ListGenCat = ListGenCat.OrderBy(o => o.Codice).ToList();
                    fncGen.LoadCombo(ddlCat, ListGenCat, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlClasse, ListGenClasse, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlZona, ListGenZona, "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlPossesso, ListGenPossesso, "CODICE", "DESCRIZIONE");

                    GrdMotivazioni.DataSource = ListGenMotivazioni;
                    GrdMotivazioni.DataBind();

                    GrdVincoli.DataSource = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.ICI_Vincoli, string.Empty, string.Empty);
                    GrdVincoli.DataBind();

                    if (MySession.Current.IdRifCalcolo > 0)
                    {
                        if (!fncMng.LoadDich(MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, -1, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {
                            lblDataInizioORG.InnerText = new FunctionGrd().FormattaDataGrd(myUIDich.DataInizio);
                            hfIDTipologiaORG.Value = myUIDich.IDTipologia.ToString();
                            LoadForm(myUIDich);
                            LockControl();
                            MySession.Current.UIDichOld = myUIDich;
                            LoadComproprietari(myUIDich);
                            sScript = string.Empty;
                            sScript += new BLL.Istanze(new Istanza(), string.Empty).GetLinkGIS(myUIDich, (MySession.Current.Ente.SIT.IsActive == 1 ? true : false), MySession.Current.Ente.SIT.Url, MySession.Current.Ente.SIT.Token, txtFoglio.Text, txtNumero.Text, txtSub.Text,ref UrlGoToGIS);
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                    else if (MySession.Current.UINewSuggest.IDRifOrg > 0)
                    {
                        RegisterScript("$('#divSuggestFromCatasto').html('<label class=\"text-danger\">La dichiarazione è stata precompilata con l’immobile presente a catasto da te selezionato.<br>Puoi completare i dati e confermare, oppure sostituire i dati precompilati con quelli di altro immobile.</label>')", this.GetType());
                        foreach (SPC_DichICI myRiep in MySession.Current.ListDichUICatasto)
                        {
                            if (myRiep.IDRifOrg == MySession.Current.UINewSuggest.IDRifOrg)
                            {
                                myUIDich = myRiep;
                                break;
                            }
                        }
                        LoadForm(myUIDich);
                        LockControl();
                        LoadComproprietari(myUIDich);

                        sScript = string.Empty;
                        sScript += new BLL.Istanze(new Istanza(), string.Empty).GetLinkGIS(myUIDich, (MySession.Current.Ente.SIT.IsActive == 1 ? true : false), MySession.Current.Ente.SIT.Url, MySession.Current.Ente.SIT.Token, txtFoglio.Text, txtNumero.Text, txtSub.Text,ref UrlGoToGIS);
                        RegisterScript(sScript, this.GetType());
                    }
                    else if (MySession.Current.IdIstanza > 0)
                    {
                        LoadIstanza();
                        if (!fncMng.LoadDich(Istanza.TIPO.ConsultaDich, MySession.Current.Ente.IDEnte, MySession.Current.myAnag.COD_CONTRIBUENTE, MySession.Current.IdRifCalcolo, MySession.Current.IdIstanza, out myUIDich))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        else {
                            lblDataInizioORG.InnerText = new FunctionGrd().FormattaDataGrd(myUIDich.DataInizio);
                            hfIDTipologiaORG.Value = myUIDich.IDTipologia.ToString();
                            LoadForm(myUIDich);
                            LockControl();
                        }
                    }
                    else
                    {
                        ShowHide("divPertinenza", false);
                        if (MySession.Current.IdIstanza > 0)
                            LoadIstanza();
                        UIOrg = new SPC_DichICI();
                        ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
                    }
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("SearchStradario", false);

                    sScript = string.Empty;
                    if (!MySession.Current.TipoStorico.StartsWith("RAV"))
                    {
                        sScript += "$('#lblForewordUI').html('Per fare dichiarazioni di Inagibilità, Comodato Gratuito e Cessazione, clicca sul bottone corrispondente a destra.<br />Se devi fare una variazione agisci direttamente sul campo da variare.');";
                    }
                    if (MySession.Current.Scope == "FO")
                    { 
                        sScript += "$('#lblInfoRendita').html('Si ricorda che la rendita non è calcolata dal sistema ma deve sempre essere inserita.');";
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_UrlAuthGIS').val('"+MySession.Current.Ente.SIT.UrlAuth+"');";//http://188.219.128.165/wg_atessa_wip/accesso_isimply.php );
                        sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_TokenAuthGIS').val('"+MySession.Current.Ente.SIT.Token+"');";//080042cad6356ad5dc0a720c18b53b8e53d4c274 );
                        sScript += "autoSubmit();";
                    }
                    RegisterScript(sScript, this.GetType());

                    if (MySession.Current.IdIstanza <= 0)
                    { new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte); }
                    else {
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Page_Load", "ingresso pagina", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    }
                }
                else
                {
                    RegisterScript("$('#divComproprietari').html('" + MySession.Current.scriptComproprietari + "')", this.GetType());
                }
                ShowHide("FileToUpload", false);
                ShowHide("divRiepilogo", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Page_Load::errore::", ex);
                LoadException(ex);
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
            try
            {
                if (((System.Web.UI.WebControls.Button)sender).ID == "CmdVincoliBack")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita vincoli", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("divVincoliGrd", false);
                }
                else if (((System.Web.UI.WebControls.Button)sender).ID == "CmdBackSearchStradario")
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita stadario", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    ShowHide("SearchStradario", false);
                }
                else if (MySession.Current.IdIstanza > 0)
                {
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    if (MySession.Current.Scope == "BO")
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeGen, Response);
                    else
                        if (MySession.Current.IsBackToTributi)
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
                    else
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFO_IstanzeGen, Response);
                }
                else
                {
                    MySession.Current.TipoIstanza = string.Empty;
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    if (MySession.Current.TipoStorico.StartsWith("RAV"))
                    {
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetRavvedimento, Response);
                    }
                    else if (MySession.Current.TipoStorico != string.Empty)
                    {
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetStoricoICI, Response);
                    }
                    else {
                        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoICI, Response);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Back::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il caricamento degli allegati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GestAllegati(object sender, EventArgs e)
        {
            try
            {
                if (chkAllegati.Checked)
                {
                    ShowHide("FileToUpload", true);
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "GestAllegati", "chiesto inserimento allegati", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                }
                else
                    ShowHide("FileToUpload", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.GestAllegati::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la ricerca da stradario
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchStradario(object sender, EventArgs e)
        {
            try
            {
                LoadSearch();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.SearchStradario::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        #region "Bottoni Front Office"
        /// <summary>
        /// Bottone per la richiesta di istanza d'inagibilità
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Inagibile(object sender, ImageClickEventArgs e)
        {
            try
            {
                MySession.Current.TipoIstanza = Istanza.TIPO.Inagibilità;
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaInagibile", "chiesto inserimento istanza inagibile", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Inagibile::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per la richiesta di istanza di uso gratuito
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void UsoGratuito(object sender, ImageClickEventArgs e)
        {
            try
            {
                MySession.Current.TipoIstanza = Istanza.TIPO.ComodatoUsoGratuito;
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "UsoGratuito", "chiesto inserimento istanza uso gratuito", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.UsoGratuito::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per la richiesta di istanza di chiusura
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Close(object sender, ImageClickEventArgs e)
        {
            try
            {
                MySession.Current.TipoIstanza = Istanza.TIPO.Cessazione;
                LockControl();
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaClose", "chiesto inserimento istanza chiusura", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Close::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Bottone per la richiesta di istanza di variazione o inserimento
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                bool IsRifACatasto = false;
                ListVincoliAttr = new List<GenericCategoryWithRate>();

                if ((MySession.Current.Ente.SIT.IsActive == 1 ? true : false))
                {
                    try
                    {
                        string requestUriParam = String.Format("?cf_iva={0}&foglio={1}&mappale={2}&subalterno={3}&cod_ente={4}&eff={5}"
                                , string.Empty
                                , txtFoglio.Text
                                , txtNumero.Text
                                , txtSub.Text
                                , MySession.Current.Ente.CodCatastale
                                , ""
                            );
                        string sErr = string.Empty;
                        var ListUICatasto = string.Empty;

                        new BLL.RestService().MakeRequestByRifCat<string>(MySession.Current.Ente.SIT.Url
                            , requestUriParam
                            , result => ListUICatasto = result
                            , error => sErr = error.Message
                            , "Token: " + MySession.Current.Ente.SIT.Token
                            );
                        if (sErr != string.Empty)
                        {
                            Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.consultacatasto::errore::" + sErr);
                        }
                        else
                        {
                            var myJObject = Newtonsoft.Json.Linq.JObject.Parse(ListUICatasto);
                            foreach (object myInt in myJObject["fabbricati"]["unita_immobiliari"])
                            {
                                var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                if (((myDetIntest["subalterno"].ToString() != string.Empty) ? myDetIntest["subalterno"].ToString() : string.Empty) == txtSub.Text)
                                {
                                    IsRifACatasto = true;
                                }
                            }
                            foreach (object myInt in myJObject["terreni"]["item_terreni"])
                            {
                                var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                if (((myDetIntest["subalterno"].ToString() != string.Empty) ? myDetIntest["subalterno"].ToString() : string.Empty) == txtSub.Text)
                                {
                                    IsRifACatasto = true;
                                    foreach (object myUrb in myJObject["terreni"]["strumenti_urbanistici"])
                                    {
                                        var myDetUrb = Newtonsoft.Json.Linq.JObject.Parse(myUrb.ToString());
                                        if ((myDetUrb["ctg_cod"].ToString() != null ? myDetUrb["ctg_cod"].ToString() : string.Empty) == "Zonizzazioni")
                                        {
                                            if ((myDetUrb["zon_descr"].ToString() != null ? myDetUrb["zon_descr"].ToString() : string.Empty).IndexOf("AGRICOL") > 1)
                                            {
                                            }
                                            else
                                            {
                                                List<GenericCategoryWithRate> ListZone = new BLL.Settings().LoadTariffe(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.ICI_Zone, string.Empty, myDetUrb["zon_cod"].ToString());
                                                foreach (GenericCategoryWithRate myZona in ListZone)
                                                {
                                                    ddlZona.SelectedValue = myZona.ID.ToString();
                                                }
                                            }
                                        }
                                        else if ((myDetUrb["ctg_cod"].ToString() != null ? myDetUrb["ctg_cod"].ToString() : string.Empty) == "Vincoli")
                                        {
                                            List<GenericCategoryWithRate> ListVincoli = new BLL.Settings().LoadTariffe(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.ICI_Vincoli, string.Empty, myDetUrb["zon_cod"].ToString());
                                            foreach (GenericCategoryWithRate myVincolo in ListVincoli)
                                            {
                                                myVincolo.IsActive = 1;
                                                if (myVincolo.ID > 0)
                                                {
                                                    ListVincoliAttr.Add(myVincolo);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (HttpException ex)
                    {
                        Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.consultacatasto::errore::", ex);
                    }
                }
                else
                {
                    IsRifACatasto = true;
                }
                if (ListVincoliAttr.Count > 0)
                    ShowHide("divVincoli", true);
                else
                {
                    foreach (GridViewRow myRow in GrdVincoli.Rows)
                    {
                        if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                        {
                            GenericCategoryWithRate myVincolo = new GenericCategoryWithRate();
                            int idVincolo = 0;
                            int.TryParse(((HiddenField)myRow.FindControl("hfCodice")).Value, out idVincolo);
                            myVincolo.ID = idVincolo;
                            myVincolo.Descrizione = myRow.Cells[0].Text;
                            if (myVincolo.ID > 0)
                            {
                                ListVincoliAttr.Add(myVincolo);
                            }
                        }
                    }
                }
                GrdVincoli.DataSource = ListVincoliAttr;
                GrdVincoli.DataBind();
                if (chkAllegati.Checked)
                {
                    HttpFileCollection ListFiles = Request.Files;
                    if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Istanza, ref ListIstanzaMailAttachments, ref ListIstanzaDichAttach))
                    {
                        sScript = "$('#lblErrorFO').text('Errore in lettura allegati!');$('#lblErrorFO').show();";
                        RegisterScript(sScript, this.GetType());
                        return;
                    }
                }
                if (!IsRifACatasto)
                {
                     sScript = "$('#lblDescrConfirm').text('Attenzione! Riferimenti Catastali non presenti. Si vuole proseguire?');";
                    sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKCatasto').show();$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKNew').hide();$('#divConfirm').show();";
                }
                else {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_CmdSaveIstanza').click();";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il salvataggio dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveIstanza(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                SPC_DichICI myDich = new SPC_DichICI();
                string sMyErr = string.Empty;
                string sScriptDich = string.Empty;
                DateTime DataInizioORG = DateTime.MinValue;
                DateTime.TryParse(lblDataInizioORG.InnerText, out DataInizioORG);
                myIstanza = ReadFormIstanza();
                myIstanza.ListAllegati = ListIstanzaDichAttach;
                myDich = ReadFormDich();
                if (!GetTipoIstanza(myDich, ref myIstanza, ref sMyErr))
                {
                    RegisterScript(sMyErr, this.GetType());
                }
                else {
                    if (fncMng.FieldValidator(MySession.Current.TipoIstanza, myIstanza, myDich, DataInizioORG, out sMyErr))
                    {
                        if (myIstanza != null && myDich != null)
                        {
                            if (MySession.Current.IDDichiarazioneIstanze <= 0)
                            {
                                myIstanza.NDichiarazione = new BLL.Dichiarazioni(new Istanza() { IDEnte = MySession.Current.Ente.IDEnte }).GetNewDichiarazione();
                                MySession.Current.IDDichiarazioneIstanze = myIstanza.NDichiarazione;
                            }
                            else
                                myIstanza.NDichiarazione = MySession.Current.IDDichiarazioneIstanze;

                            if (((SPC_DichICI)MySession.Current.UIDichOld) != null)
                            {
                                if (((SPC_DichICI)MySession.Current.UIDichOld).IDIstanza > 0)
                                {
                                    new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).SetIstanzaAnnullaPrec(((SPC_DichICI)MySession.Current.UIDichOld).IDIstanza);
                                }
                            }

                            if (new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Save())
                            {
                                if (new BLL.Dichiarazioni(myIstanza).SaveDichiarazione() <= 0)
                                {
                                    sScript += "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    return;
                                }
                                myDich.IDIstanza = myIstanza.IDIstanza;
                                myDich.ID = MySession.Current.IdRifCalcolo;
                                myDich.IDEnte = myIstanza.IDEnte;
                                myDich.IDContribuente = myIstanza.IDContribuente;
                                myDich.IDRifOrg = UIOrg.IDRifOrg;
                                bool retSave = false;
                                int IDCalcolo = 0;
                                if (MySession.Current.TipoIstanza == Istanza.TIPO.Inagibilità)
                                {
                                    retSave = new BLL.BLLSPC_DichICI(myDich).SaveInagibile(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich, out IDCalcolo);
                                }
                                else if (MySession.Current.TipoIstanza == Istanza.TIPO.ComodatoUsoGratuito)
                                {
                                    retSave = new BLL.BLLSPC_DichICI(myDich).SaveComodatoGratuito(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, int.Parse(hfIDTipologiaORG.Value), myIstanza, UIOrg, ref sScriptDich, out IDCalcolo);
                                }
                                else if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                                {
                                    retSave = new BLL.BLLSPC_DichICI(myDich).SaveVariazione(DateTime.Parse(lblDataInizioORG.InnerText), myDich.DataInizio, myDich.DataFine, myIstanza, UIOrg, ref sScriptDich, out IDCalcolo);
                                }
                                else
                                {
                                    retSave = new BLL.BLLSPC_DichICI(myDich).SaveCalcolo(myIstanza, ref sScriptDich, out IDCalcolo);
                                }
                                if (retSave)
                                {
                                    myDich.ID = -1;
                                    myDich.IDRifOrg = IDCalcolo;
                                    if (new BLL.BLLSPC_DichICI(myDich).Save())
                                    {
                                        MySession.Current.IsInitDich = true;
                                        RegisterScript("$('#hfInitDich').val('1');", this.GetType());
                                        if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                                        {
                                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza con ulteriore inserimento", General.TRIBUTO.ICI, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                            MySession.Current.HasNewDich = 1;
                                            sScript = "$('#lblDescrConfirm').text('Si vuole inserire un altro immobile?');";
                                            sScript += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKCatasto').hide();$('#" + BLL.GestForm.PlaceHolderName.Body + "_OKNew').show();$('#divConfirm').show();";
                                            RegisterScript(sScript, this.GetType());
                                        }
                                        else
                                        {
                                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Save", "salvataggio istanza", General.TRIBUTO.ICI, MySession.Current.TipoIstanza, MySession.Current.Ente.IDEnte);
                                            MySession.Current.HasNewDich = 2;
                                            if (MySession.Current.TipoStorico.StartsWith("RAV"))
                                            {
                                                sScript += "$(location).attr('href', '" + UrlHelper.GetRavvedimento + "');";
                                            }
                                            else
                                            {
                                                sScript += "$(location).attr('href', '" + UrlHelper.GetGestRiepilogoICI + "');";
                                            }
                                            sScript += "$('#lblErrorFO').text('Salvataggio effettuato con successo!');$('#lblErrorFO').show();";
                                            RegisterScript(sScript, this.GetType());
                                        }
                                    }
                                    else {
                                        sScript += "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                        RegisterScript(sScript, this.GetType());
                                    }
                                }
                                else {
                                    sScript += "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                }
                            }
                            else
                            {
                                sScript += "$('#lblErrorFO').text('Errore in salvataggio!');$('#lblErrorFO').show();";
                                RegisterScript(sScript, this.GetType());
                            }
                        }
                        else {
                            sScript += "$('#lblErrorFO').text('Errore nei dati!');$('#lblErrorFO').show();";
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                    else
                    {
                        sMyErr = "$('#lblErrorFO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorFO').show();";
                        if (sMyErr.IndexOf("data inizio") > 0)
                            sMyErr += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_lblDataInizioORG').show();";
                        else
                            sMyErr += "$('#" + BLL.GestForm.PlaceHolderName.Body + "_lblDataInizioORG').hide();";
                        RegisterScript(sMyErr, this.GetType());
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la precompilazione dei dati da catasto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SuggestNew(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;

                ShowHide("divPertinenza", false);
                if (MySession.Current.IdIstanza > 0)
                    LoadIstanza();
                UIOrg = new SPC_DichICI();
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
                if (MySession.Current.UINewSuggest.IDRifOrg > 0)
                {
                    MySession.Current.UINewSuggest.IDRifOrg += 1;
                    SPC_DichICI myUIDich = new SPC_DichICI();
                    RegisterScript("$('#divSuggestFromCatasto').html('<label class=\"text-danger\">La dichiarazione è stata precompilata con l’immobile presente a catasto da te selezionato.<br>Puoi completare i dati e confermare, oppure sostituire i dati precompilati con quelli di altro immobile.</label>')", this.GetType());
                    foreach (SPC_DichICI myRiep in MySession.Current.ListDichUICatasto)
                    {
                        if (myRiep.IDRifOrg == MySession.Current.UINewSuggest.IDRifOrg)
                        {
                            myUIDich = myRiep;
                            break;
                        }
                    }
                    LoadForm(myUIDich);
                    LockControl();
                    LoadComproprietari(myUIDich);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.SuggestNew::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #region "Bottoni Back Office"
        /// <summary>
        /// Bottone per la protocollazione dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Protocolla(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Protocolla(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in Protocollazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Protocollazione effettuata con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Protocolla::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la presa in carico dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Work(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).InCarico(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in Presa in carico!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Presa in carico con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Work::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la validazione dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Accept(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Valida(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in Validazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Validazione effettuata con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Accept::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il rigetto dell'istanza
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Stop(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Respingi(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in rifiuto istanza!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Istanza Respinta con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '" + UrlHelper.GetBO_IstanzeGen + "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Stop::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per la richiesta di integrazione dati via mail
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void MailBox(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                if (MySession.Current.Scope == "FO")
                {
                    MailUser = MySession.Current.Ente.Mail.BackOffice;
                }
                if (!new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).Integrazione(Request.Files, txtMotivazione.Text, MailUser, out sMyErr))
                {
                    sScript += "$('#lblErrorFO').text('Errore in invio comunicazione!" + ((sMyErr != string.Empty) ? "\n" + sMyErr : string.Empty) + "');";
                    sScript += "$('#lblErrorFO').removeClass('text-success');$('#lblErrorFO').addClass('text-danger');$('#lblErrorFO').show();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#lblErrorFO').text('Comunicazione inviata con successo!');";
                    sScript += "$('#lblErrorFO').removeClass('text-danger');$('#lblErrorFO').addClass('text-success');$('#lblErrorFO').show();";
                    sScript += "$(location).attr('href', '";
                    if (MySession.Current.Scope == "FO")
                    {
                        sScript += UrlHelper.GetFO_IstanzeGen;
                    }
                    else {
                        sScript += UrlHelper.GetBO_IstanzeGen;
                    }
                    sScript += "');";
                    RegisterScript(sScript, this.GetType());
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.MailBox::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                LoadIstanza();
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        /// <summary>
        /// Bottone per il ribaltamento automatico dell'istanza nel verticale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Ribalta(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            string sMyErr = string.Empty;
            try
            {
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeRibalta, Response);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.Ribalta::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #endregion
        #region "Griglie"  
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdStradarioRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string IDSetting = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "AttachRow":
                        foreach (GridViewRow myCfg in GrdStradario.Rows)
                        {
                            if (((HiddenField)myCfg.Cells[0].FindControl("hdIdStradario")).Value == IDSetting.ToString())
                            {
                                txtVia.Text = ((Label)myCfg.Cells[0].FindControl("lblStradario")).Text;
                                hfIdVia.Value = IDSetting.ToString();
                                break;
                            }
                        }
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "AttachRow", "agganciato strada", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.GrdComuniRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (MySession.Current.UINewSuggest.IDRifOrg > 0)
                {
                    foreach (SPC_DichICI myRiep in MySession.Current.ListDichUICatasto)
                    {
                        if (myRiep.IDRifOrg == MySession.Current.UINewSuggest.IDRifOrg)
                        {
                            LoadVincoli(myRiep);
                            break;
                        }
                    }
                }
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
            }
        }
        /// <summary>
        /// Funzione di gestione cambio pagina della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdStradarioPageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LoadSearch(e.NewPageIndex);
            RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdAllegatiRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.GrdAllegatiRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdAllegatiRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string Allegati, sScript;
                Allegati = sScript = string.Empty;
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "RowDownload":
                        List<IstanzaAllegato> myAll = new BLL.IstanzaAllegati(new IstanzaAllegato()).LoadAllegati(-1, IDRow);
                        if (myAll.Count > 0)
                        {
                            try
                            {
                                Response.ContentType = myAll[0].FileMIMEType;
                                Response.AddHeader("content-disposition", string.Format("attachment;filename=\"{0}\"", myAll[0].FileName));
                                Response.BinaryWrite(myAll[0].PostedFile);
                                Response.End();
                            }
                            catch (Exception err)
                            {
                                if (err.Message != "Thread was being aborted.")
                                {
                                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.GrdAllegatiRowCommand::errore::", err);
                                }
                            }
                        }
                        else
                        {
                            sScript = "$('#lblErrorFO').text('Allegato non disponibile!');$('#lblErrorFO').show();";
                        }
                        break;
                    default:
                        break;
                }
                new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).LoadFormData(out sScript, ref GrdStatiIstanza, ref GrdAllegati);
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.GrdStatiIstanzaRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        #endregion
        /// <summary>
        /// Bottone per il caricamento del link di catasto in base ai riferimenti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RifCatChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtFoglio.Text != string.Empty && txtNumero.Text != string.Empty && MySession.Current.UINewSuggest.IDRifOrg <= 0)
                {
                    if ((MySession.Current.Ente.SIT.IsActive == 1 ? true : false))
                    {
                        try
                        {
                            string requestUriParam = String.Format("?cf_iva={0}&foglio={1}&mappale={2}&subalterno={3}&cod_ente={4}&eff={5}"
                                    , string.Empty
                                    , txtFoglio.Text
                                    , txtNumero.Text
                                    , txtSub.Text
                                    , MySession.Current.Ente.CodCatastale
                                    , ""
                                );
                            string sErr = string.Empty;
                            var ListUICatasto = string.Empty;

                            new BLL.RestService().MakeRequestByRifCat<string>(MySession.Current.Ente.SIT.Url
                                , requestUriParam
                                , result => ListUICatasto = result
                                , error => sErr = error.Message
                                , "Token: " + MySession.Current.Ente.SIT.Token
                                );
                            if (sErr != string.Empty)
                            {
                                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.consultacatasto::errore::" + sErr);
                            }
                            else
                            {
                                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(ListUICatasto);
                                foreach (object myInt in myJObject["fabbricati"]["unita_immobiliari"])
                                {
                                    var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                    if (((myDetIntest["subalterno"].ToString() != string.Empty) ? myDetIntest["subalterno"].ToString() : string.Empty) == txtSub.Text)
                                    {
                                        List<GenericCategory> ListSubset = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Categorie, string.Empty, myDetIntest["categoria"].ToString());
                                        if (ListSubset.Count > 0)
                                            ddlCat.SelectedValue = ListSubset[0].Codice;
                                        break;
                                    }
                                }
                            }
                        }
                        catch (HttpException ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.RifCatChanged.consultacatasto::errore::", ex);
                        }
                        LoadComproprietari(new SPC_DichICI() { Foglio = txtFoglio.Text, Numero = txtNumero.Text, Sub = txtSub.Text });
                    }

                }
                GrdVincoli.DataSource = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.ICI_Vincoli, string.Empty, string.Empty);
                GrdVincoli.DataBind();
                ddlZona.Enabled = true; GrdVincoli.Enabled = true; ShowHide("divVincoli", true); //ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdVincoli", true);

                string sScript = string.Empty;
                sScript += new BLL.Istanze(new Istanza(), string.Empty).GetLinkGIS(new SPC_DichICI(), (MySession.Current.Ente.SIT.IsActive == 1 ? true : false), MySession.Current.Ente.SIT.Url, MySession.Current.Ente.SIT.Token, txtFoglio.Text, txtNumero.Text, txtSub.Text,ref UrlGoToGIS);
                RegisterScript(sScript, this.GetType());
                if (((TextBox)sender).ID == "txtFoglio")
                {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_txtNumero').focus();";
                }
                else if (((TextBox)sender).ID == "txtNumero")
                {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_txtSub').focus();";
                }
                else if (((TextBox)sender).ID == "txtSub")
                {
                    sScript = "$('#" + BLL.GestForm.PlaceHolderName.Body + "_ddlCat').focus();";
                }
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.RifCatChanged::errore::", ex);
            }
            finally
            {
                RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            }
        }
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">SPC_DichICI oggetto da caricare</param>
        private void LoadForm(SPC_DichICI myItem)
        {
            try
            {
                hfIdVia.Value = myItem.IDVia.ToString();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica hfIdVia:" + myItem.IDVia.ToString());
                txtVia.Text = myItem.Via;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtVia:" + myItem.Via.ToString());
                txtCivico.Text = myItem.Civico;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtCivico:" + myItem.Civico.ToString());
                txtFoglio.Text = myItem.Foglio;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtFoglio:" + myItem.Foglio.ToString());
                txtNumero.Text = myItem.Numero;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtNumero:" + myItem.Numero.ToString());
                txtSub.Text = myItem.Sub;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtSub:" + myItem.Sub.ToString());
                txtDataInizio.Text = new FunctionGrd().FormattaDataGrd(myItem.DataInizio);
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtDataInizio:" + txtDataInizio.Text);
                txtDataFine.Text = new FunctionGrd().FormattaDataGrd(myItem.DataFine);
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtDataFine:" + txtDataFine.Text);
                ddlTipologia.SelectedValue = ((myItem.IDTipologia > 0) ? myItem.IDTipologia.ToString() : "-1");
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica ddlTipologia:" + ddlTipologia.SelectedValue.ToString());
                if (ddlTipologia.SelectedItem.Text.IndexOf("C/2") > 0 || ddlTipologia.SelectedItem.Text.IndexOf("C/6") > 0 || ddlTipologia.SelectedItem.Text.IndexOf("C/7") > 0)
                    ShowHide("divPertinenza", true);
                else
                    ShowHide("divPertinenza", false);
                ddlCat.SelectedValue = myItem.IDCategoria.ToString();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica ddlCat:" + ddlCat.SelectedValue.ToString());
                ddlClasse.SelectedValue = myItem.IDClasse.Trim();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica ddlClasse:" + ddlClasse.SelectedValue.ToString());
                ddlZona.SelectedValue = ((myItem.IDZona > 0) ? myItem.IDZona.ToString() : "-1");
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica ddlZona:" + ddlZona.SelectedValue.ToString());
                txtConsistenza.Text = myItem.Consistenza.ToString();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtConsistenza:" + txtConsistenza.Text);
                chkStorico.Checked = myItem.IsStorico;
                txtRendita.Text = myItem.RenditaValore.ToString();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtRendita:" + txtRendita.Text);
                ddlPossesso.SelectedValue = ((myItem.IDPossesso > 0) ? myItem.IDPossesso.ToString() : "-1");
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica ddlPossesso:" + ddlPossesso.SelectedValue.ToString());
                txtPercPos.Text = myItem.PercPossesso.ToString();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtPercPos:" + txtPercPos.Text);
                if (myItem.PercRiduzione > 0)
                    txtPercRid.Text = myItem.PercRiduzione.ToString();
                if (myItem.PercEsenzione > 0)
                    txtPercEse.Text = myItem.PercEsenzione.ToString();
                txtNUtilizzatori.Text = myItem.NUtilizzatori.ToString();
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtNUtilizzatori:" + txtNUtilizzatori.Text);
                chkStorico.Checked = myItem.IsStorico;
                txtPertFoglio.Text = myItem.PertFoglio;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtPertFoglio:" + txtPertFoglio.Text);
                txtPertNumero.Text = myItem.PertNumero;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtPertNumero:" + txtPertNumero.Text);
                txtPertSub.Text = myItem.PertSub;
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadForm::carica txtPertSub:" + txtPertSub.Text);
                LoadVincoli(myItem);

                UIOrg = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.LoadForm::errore::", ex);
                throw new Exception();
            }
        }
        /// <summary>
        /// Funzione per il caricamento dei possibili vincoli da selezionare
        /// </summary>
        /// <param name="myItem">SPC_DichICI oggetto per il caricamento</param>
        private void LoadVincoli(SPC_DichICI myItem)
        {
            try
            {
                foreach (string myDato in myItem.ListVincoli)
                {
                    foreach (GridViewRow myRow in GrdVincoli.Rows)
                    {
                        if (myDato == ((HiddenField)myRow.FindControl("hfCodice")).Value)
                        {
                            ((CheckBox)myRow.FindControl("chkSel")).Checked = true;
                        }
                    }
                }
                List<GenericCategory> ListVincoli = new List<GenericCategory>();
                foreach (GridViewRow myRow in GrdVincoli.Rows)
                {
                    GenericCategory myVincolo = new GenericCategory();
                    if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                    {
                        myVincolo.ID = int.Parse(((HiddenField)myRow.FindControl("hfCodice")).Value);
                        myVincolo.Descrizione = myRow.Cells[0].Text;
                        myVincolo.IsActive = 1;
                        ListVincoli.Add(myVincolo);
                    }
                    else
                    {
                        myVincolo.ID = int.Parse(((HiddenField)myRow.FindControl("hfCodice")).Value);
                        myVincolo.Descrizione = myRow.Cells[0].Text;
                        myVincolo.IsActive = 0;
                        ListVincoli.Add(myVincolo);
                    }
                }
                if (ListVincoli.Count > 0)
                    ShowHide("divVincoli", true);
                GrdVincoli.Enabled = true;

                GrdVincoli.DataSource = ListVincoli;
                GrdVincoli.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.LoadVincoli::errore::", ex);
            }
        }
        /// <summary>
        /// Funzione di caricamento dati istanza
        /// </summary>
        private void LoadIstanza()
        {
            try
            {
                List<Istanza> listDati = new List<Istanza>();
                string sScript = string.Empty;
                if (MySession.Current.IdIstanza > 0)
                {
                    if (!new BLL.Istanze(new Istanza(), MySession.Current.UserLogged.ID).LoadIstanze("", 0, DateTime.MaxValue, "", "", "", "", MySession.Current.IdIstanza, -1, true, out listDati))
                        RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina.');$('#lblErrorFO').show();", this.GetType());
                    else {
                        foreach (Istanza myItem in listDati)
                        {
                            myIstanza = myItem;
                            myIstanza.DescrTipoIstanza = myIstanza.DescrTipo;
                            myIstanza.ListDatiDich = UIOrg;
                            if (MySession.Current.myAnag == null)
                            {
                                MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(myItem.IDContribuente, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                                List<UserRole> ListGen = new BLL.Settings().LoadUserRole("", MySession.Current.myAnag.CodiceFiscale, true, MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.NameUser);
                                if (ListGen.Count > 0)
                                {
                                    MailUser = ListGen[0].NameUser;
                                }
                                else
                                {
                                    sScript += "$('#lblErrorFO').text('Errore in caricamento pagina.');$('#lblErrorFO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    break;
                                }
                            }
                            else if (MySession.Current.myAnag.COD_CONTRIBUENTE != myItem.IDContribuente)
                            {
                                MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(myItem.IDContribuente, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                            }

                            new BLL.Istanze(myIstanza, MySession.Current.UserLogged.ID).LoadFormData(out sScript, ref GrdStatiIstanza, ref GrdAllegati);
                            RegisterScript(sScript, this.GetType());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.LoadIstanza::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione per l'abilitazione dei controlli
        /// </summary>
        private void LockControl()
        {
            ddlZona.Enabled = false;
            /*GrdVincoli.Enabled = false;*/
            if (MySession.Current.TipoIstanza == Istanza.TIPO.Cessazione)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                ddlTipologia.Enabled = false;
                ddlCat.Enabled = false;
                ddlClasse.Enabled = false;
                ddlZona.Enabled = false;
                GrdVincoli.Enabled = false;
                txtConsistenza.Enabled = false;
                chkStorico.Enabled = false;
                txtRendita.Enabled = false;
                ddlPossesso.Enabled = false;
                txtPercPos.Enabled = false;
                txtNUtilizzatori.Enabled = false;
                txtPercRid.Enabled = false; txtPercEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Inagibilità)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Text = string.Empty;
                ddlTipologia.Enabled = false;
                ddlCat.Enabled = false;
                ddlClasse.Enabled = false;
                ddlZona.Enabled = false;
                GrdVincoli.Enabled = false;
                txtConsistenza.Enabled = false;
                chkStorico.Enabled = false;
                txtRendita.Enabled = false;
                ddlPossesso.Enabled = false;
                txtPercPos.Enabled = false;
                txtNUtilizzatori.Enabled = false;
                txtPercEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.ComodatoUsoGratuito)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Text = string.Empty;
                ddlTipologia.SelectedValue = "-1";
                List<GenericCategory> ListSubset = new List<GenericCategory>();
                ListSubset = new BLL.Settings().LoadConfigForDDL(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Caratteristica, Istanza.TIPO.ComodatoUsoGratuito, string.Empty);
                new General().LoadCombo(ddlTipologia, ListSubset, "CODICE", "DESCRIZIONE");
                ddlTipologia.SelectedValue = ListSubset[0].Codice;
                ddlCat.Enabled = false;
                ddlClasse.Enabled = false;
                ddlZona.Enabled = false;
                GrdVincoli.Enabled = false;
                txtConsistenza.Enabled = false;
                chkStorico.Enabled = false;
                txtRendita.Enabled = false;
                ddlPossesso.Enabled = false;
                txtPercPos.Enabled = false;
                txtNUtilizzatori.Enabled = false;
                txtPercRid.Enabled = false; txtPercEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.ConsultaDich || MySession.Current.TipoIstanza == Istanza.TIPO.ConsultaCatasto || MySession.Current.TipoIstanza.StartsWith(Istanza.TIPO.ConsultaDich))
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();$('#lblForewordUI').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipologia.Enabled = false;
                ddlCat.Enabled = false;
                ddlClasse.Enabled = false;
                ddlZona.Enabled = false;
                GrdVincoli.Enabled = false;
                txtConsistenza.Enabled = false;
                chkStorico.Enabled = false;
                txtRendita.Enabled = false;
                ddlPossesso.Enabled = false;
                txtPercPos.Enabled = false;
                txtNUtilizzatori.Enabled = false;
                txtPercRid.Enabled = false; txtPercEse.Enabled = false;
                ShowHide("divMotivazione", false);
            }
            else if (MySession.Current.TipoIstanza == Istanza.TIPO.Modifica && MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
            {
                txtVia.Enabled = true;
                RegisterScript("$('.BottoneMapMarker').show();", this.GetType());
                txtCivico.Enabled = true;
                txtFoglio.Enabled = true;
                txtNumero.Enabled = true;
                txtSub.Enabled = true;
                txtDataInizio.Enabled = true;
                txtDataFine.Enabled = false;
                ddlTipologia.Enabled = true;
                ddlCat.Enabled = true;
                ddlClasse.Enabled = true;
                ddlZona.Enabled = false;
                GrdVincoli.Enabled = false;
                txtConsistenza.Enabled = true;
                chkStorico.Enabled = true;
                txtRendita.Enabled = true;
                ddlPossesso.Enabled = true;
                txtPercPos.Enabled = true;
                txtNUtilizzatori.Enabled = true;
                txtPercRid.Enabled = true; txtPercEse.Enabled = true;
                ShowHide("divMotivazione", true);
                ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
            }
            else if (MySession.Current.IdIstanza > 0)
            {
                txtVia.Enabled = false;
                RegisterScript("$('.BottoneMapMarker').hide();$('.BottoneMap').hide();$('.BottoneShare').hide();$('#lblComproprietari').hide();", this.GetType());
                txtCivico.Enabled = false;
                txtFoglio.Enabled = false;
                txtNumero.Enabled = false;
                txtSub.Enabled = false;
                txtDataInizio.Enabled = false;
                txtDataFine.Enabled = false;
                ddlTipologia.Enabled = false;
                ddlCat.Enabled = false;
                ddlClasse.Enabled = false;
                ddlZona.Enabled = false;
                GrdVincoli.Enabled = false;
                txtConsistenza.Enabled = false;
                chkStorico.Enabled = false;
                txtRendita.Enabled = false;
                ddlPossesso.Enabled = false;
                txtPercPos.Enabled = false;
                txtNUtilizzatori.Enabled = false;
                txtPercRid.Enabled = false; txtPercEse.Enabled = false;
                ShowHide(BLL.GestForm.PlaceHolderName.Body + "_GrdMotivazioni", false);
            }
            ManageBottoniera(General.TRIBUTO.ICI, UIOrg.Stato);
            if (ddlTipologia.SelectedItem.Text.IndexOf("erren") > 0 || ddlTipologia.SelectedItem.Text.IndexOf("abbricabil") > 0 || ddlTipologia.SelectedItem.Text.IndexOf("dificabil") > 0)
            {
                RegisterScript("$('.BottoneAttention').hide();$('p#inagibile').hide();$('.BottoneUserGroup').hide();$('p#usogratuito').hide();", this.GetType());
            }
            else {
                RegisterScript("$('.BottoneAttention').show();$('p#inagibile').show();$('.BottoneUserGroup').show();$('p#usogratuito').show();", this.GetType());
            }
            if (MySession.Current.UserLogged.IDDelegato > 0)
            {
                RegisterScript("$('.BottoneDiv').hide();$('.BottoneDivIstanza').hide();$('.BottoneSave').hide();", this.GetType());
            }
        }
        #endregion
        #region "Read Data From Form"
        /// <summary>
        /// Funzione per il caricamento dell'istanza dalla pagina
        /// </summary>
        /// <returns>Istanza oggetto istanza</returns>
        private Istanza ReadFormIstanza()
        {
            Istanza myItem = new Istanza();
            try
            {
                myItem.DataPresentazione = DateTime.Now;
                myItem.DataInCarico = DateTime.MaxValue;
                myItem.DataRespinta = DateTime.MaxValue;
                myItem.DataValidata = DateTime.MaxValue;
                myItem.IDContribuente = MySession.Current.UserLogged.IDContribToWork;
                myItem.IDDelegato = (MySession.Current.UserLogged.IDContribToWork != MySession.Current.UserLogged.IDContribLogged) ? MySession.Current.UserLogged.IDContribLogged : -1;
                myItem.IDEnte = MySession.Current.Ente.IDEnte;
                myItem.IDIstanza = -1;
                myItem.IDStato = Istanza.STATO.Presentata;
                myItem.IDTributo = General.TRIBUTO.ICI;
                myItem.Note = "";
                foreach (GridViewRow myRow in GrdMotivazioni.Rows)
                {
                    if (((CheckBox)myRow.Cells[0].FindControl("chkSel")).Checked)
                    {
                        IstanzaMotivazione myMotiv = new IstanzaMotivazione();
                        myMotiv.IDTipo = int.Parse(((HiddenField)myRow.Cells[0].FindControl("hfIdMotivazione")).Value);
                        myMotiv.Note = myRow.Cells[1].Text;
                        myItem.ListMotivazioni.Add(myMotiv);
                    }
                }
                if (txtMotivazione.Text.Trim() != string.Empty)
                {
                    IstanzaMotivazione myMotiv = new IstanzaMotivazione();
                    myMotiv.IDTipo = -1;
                    myMotiv.Note = txtMotivazione.Text;
                    myItem.ListMotivazioni.Add(myMotiv);
                }
                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.ReadFormIstanza::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// Funzione per la lettura dei dati dell'immobile dalla pagina
        /// </summary>
        /// <returns>SPC_DichICI oggetto immobile</returns>
        private SPC_DichICI ReadFormDich()
        {
            SPC_DichICI NewItem = new SPC_DichICI();
            try
            {
                NewItem.ID = default(int);
                NewItem.IDIstanza = default(int);
                NewItem.IDRifOrg = MySession.Current.IdRifCalcolo;
                NewItem.IDVia = int.Parse(hfIdVia.Value);
                NewItem.Via = txtVia.Text;
                NewItem.Civico = txtCivico.Text;
                NewItem.Esponente = "";
                NewItem.Interno = "";
                NewItem.Scala = "";
                NewItem.Foglio = txtFoglio.Text;
                NewItem.Numero = txtNumero.Text;
                NewItem.Sub = txtSub.Text;
                NewItem.DataInizio = DateTime.Parse(txtDataInizio.Text);
                if (txtDataFine.Text.Trim() != string.Empty)
                    NewItem.DataFine = DateTime.Parse(txtDataFine.Text);
                else
                    NewItem.DataFine = DateTime.MaxValue;
                NewItem.Note = "";
                if (ddlTipologia.SelectedValue != "")
                {
                    NewItem.IDTipologia = int.Parse(ddlTipologia.SelectedValue);
                    NewItem.DescrTipologia = ddlTipologia.SelectedItem.Text;
                }
                if (ddlCat.SelectedValue != "")
                {
                    NewItem.IDCategoria = int.Parse(ddlCat.SelectedValue);
                    NewItem.DescrCat = ddlCat.SelectedItem.Text;
                }
                NewItem.IDClasse = ddlClasse.SelectedValue;
                NewItem.DescrClasse = ddlClasse.SelectedItem.Text;
                if (ddlZona.SelectedValue != string.Empty)
                    NewItem.IDZona = int.Parse(ddlZona.SelectedValue);
                if (txtConsistenza.Text != string.Empty)
                    NewItem.Consistenza = decimal.Parse(txtConsistenza.Text.Replace(".", ","));
                NewItem.IsStorico = chkStorico.Checked;
                if (txtPercRid.Text != string.Empty)
                    NewItem.PercRiduzione = decimal.Parse(txtPercRid.Text.Replace(".", ","));
                if (txtPercEse.Text != string.Empty)
                    NewItem.PercEsenzione = decimal.Parse(txtPercEse.Text.Replace(".", ","));
                if (txtRendita.Text != string.Empty)
                    NewItem.RenditaValore = decimal.Parse(txtRendita.Text.Replace(".", ","));
                if (ddlPossesso.SelectedValue != string.Empty)
                    NewItem.IDPossesso = int.Parse(ddlPossesso.SelectedValue);
                if (txtPercPos.Text != string.Empty)
                    NewItem.PercPossesso = decimal.Parse(txtPercPos.Text.Replace(".", ","));
                if (txtNUtilizzatori.Text != string.Empty)
                    NewItem.NUtilizzatori = int.Parse(txtNUtilizzatori.Text);
                NewItem.PertFoglio = txtPertFoglio.Text;
                NewItem.PertNumero = txtPertNumero.Text;
                NewItem.PertSub = txtPertSub.Text;
                List<string> ListVincoli = new List<string>();
                foreach (GenericCategoryWithRate myVincolo in ListVincoliAttr)
                {
                    ListVincoli.Add(myVincolo.ID.ToString());
                }
                NewItem.ListVincoli = ListVincoli;
                return NewItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.ReadFormDich::errore::", ex);
                return null;
            }
        }
        #endregion
        /// <summary>
        /// Funzione di caricamento dati in griglia
        /// </summary>
        /// <param name="page">int numero di pagina su cui posizionare la visualizzazione</param>
        private void LoadSearch(int? page = 0)
        {
            BLL.Settings fncGen = new BLL.Settings();
            try
            {
                GrdStradario.DataSource = fncGen.LoadStradario(MySession.Current.Ente.IDEnte, txtSearch.Text);
                if (page.HasValue)
                    GrdStradario.PageIndex = page.Value;
                GrdStradario.DataBind();
                ShowHide("SearchStradario", true);
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "LoadSearch", "cercato strada", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.LoadSearch::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (MySession.Current.UINewSuggest.IDRifOrg > 0)
                {
                    foreach (SPC_DichICI myRiep in MySession.Current.ListDichUICatasto)
                    {
                        if (myRiep.IDRifOrg == MySession.Current.UINewSuggest.IDRifOrg)
                        {
                            LoadVincoli(myRiep);
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Funzione che determina il tipo di istanza che si sta creando
        /// </summary>
        /// <param name="UIDichCurrent">SPC_DichICI oggetto immobile</param>
        /// <param name="myItem">ref Istanza oggetto istanza che si sta creando</param>
        /// <param name="sErr">ref string errore</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        private bool GetTipoIstanza(SPC_DichICI UIDichCurrent, ref Istanza myItem, ref string sErr)
        {
            try
            {
                if (MySession.Current.TipoIstanza == Istanza.TIPO.Variazione)
                {
                    if (((SPC_DichICI)MySession.Current.UIDichOld).Consistenza != UIDichCurrent.Consistenza
                            || ((SPC_DichICI)MySession.Current.UIDichOld).IDCategoria != UIDichCurrent.IDCategoria
                            || ((SPC_DichICI)MySession.Current.UIDichOld).IDClasse != UIDichCurrent.IDClasse
                            || ((SPC_DichICI)MySession.Current.UIDichOld).IDTipologia != UIDichCurrent.IDTipologia
                            || ((SPC_DichICI)MySession.Current.UIDichOld).IDZona != UIDichCurrent.IDZona
                            || ((SPC_DichICI)MySession.Current.UIDichOld).IsStorico != UIDichCurrent.IsStorico
                            || ((SPC_DichICI)MySession.Current.UIDichOld).NUtilizzatori != UIDichCurrent.NUtilizzatori
                            || ((SPC_DichICI)MySession.Current.UIDichOld).PercPossesso != UIDichCurrent.PercPossesso
                            || ((SPC_DichICI)MySession.Current.UIDichOld).PercEsenzione != UIDichCurrent.PercEsenzione
                            || ((SPC_DichICI)MySession.Current.UIDichOld).PercRiduzione != UIDichCurrent.PercRiduzione
                            || ((SPC_DichICI)MySession.Current.UIDichOld).RenditaValore != UIDichCurrent.RenditaValore
                            || ((SPC_DichICI)MySession.Current.UIDichOld).DataInizio.ToShortDateString() != UIDichCurrent.DataInizio.ToShortDateString()
                            || ((SPC_DichICI)MySession.Current.UIDichOld).DataFine.ToShortDateString() != UIDichCurrent.DataFine.ToShortDateString()
                            || ((SPC_DichICI)MySession.Current.UIDichOld).ListVincoli.Count != UIDichCurrent.ListVincoli.Count
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Variazione;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaAlter", "chiesto inserimento istanza variazione", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    }
                    else if (((SPC_DichICI)MySession.Current.UIDichOld).Via != UIDichCurrent.Via
                            || ((SPC_DichICI)MySession.Current.UIDichOld).Civico != UIDichCurrent.Civico
                            || ((SPC_DichICI)MySession.Current.UIDichOld).Foglio != UIDichCurrent.Foglio
                            || ((SPC_DichICI)MySession.Current.UIDichOld).Numero != UIDichCurrent.Numero
                            || ((SPC_DichICI)MySession.Current.UIDichOld).Sub != UIDichCurrent.Sub
                            || ((SPC_DichICI)MySession.Current.UIDichOld).IDPossesso != UIDichCurrent.IDPossesso
                        )
                    {
                        MySession.Current.TipoIstanza = Istanza.TIPO.Modifica;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "IstanzaUpdate", "chiesto inserimento istanza correzione", General.TRIBUTO.ICI, "", MySession.Current.Ente.IDEnte);
                    }
                    else
                    {
                        sErr = "$('#lblErrorFO').text('Non è stata effettuata nessuna modifica! Impossibile salvare i dati!');$('#lblErrorFO').show();";
                        return false;
                    }
                }
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                ListMyData = new BLL.Settings().LoadTipoIstanze(General.TRIBUTO.ICI, MySession.Current.TipoIstanza, false);
                foreach (GenericCategory myTipo in ListMyData)
                {
                    myItem.IDTipo = myTipo.ID;
                }
                myItem.DescrTipoIstanza = MySession.Current.TipoIstanza;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.GetTipoIstanza::errore::", ex);
                sErr = "$('#lblErrorFO').text('Errore nei dati!');$('#lblErrorFO').show();";
                return false;
            }
        }
        /// <summary>
        /// Funzione per il caricamento dei comproprietari da catasto
        /// </summary>
        /// <param name="myUI">SPC_DichICI oggetto immobile</param>
        private void LoadComproprietari(SPC_DichICI myUI)
        {
            string sScript = string.Empty;
            try
            {
                if ((MySession.Current.Ente.SIT.IsActive == 1 ? true : false))
                {
                    if (myUI.Foglio != string.Empty && myUI.Numero != string.Empty)
                    {
                        try
                        {
                            string requestUriParam = String.Format("?cf_iva={0}&foglio={1}&mappale={2}&subalterno={3}&cod_ente={4}&eff={5}"
                                    , string.Empty
                                    , myUI.Foglio
                                    , myUI.Numero
                                    , myUI.Sub
                                    , MySession.Current.Ente.CodCatastale
                                    , ""
                                );
                            string sErr = string.Empty;
                            var ListUICatasto = string.Empty;

                            new BLL.RestService().MakeRequestByRifCat<string>(MySession.Current.Ente.SIT.Url
                                , requestUriParam
                                , result => ListUICatasto = result
                                , error => sErr = error.Message
                                /*, "Basic " + authInfo*/
                                , "Token: " + MySession.Current.Ente.SIT.Token
                                );
                            if (sErr != string.Empty)
                            {
                                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.consultacatasto::errore::" + sErr);
                            }
                            else
                            {
                                sScript += "<p class=\"col-md-12 lead\">Comproprietari</p>";
                                sScript += "<div class=\"col-md-12\">";
                                sScript += "<table cellspacing=\"0\" cellpadding=\"3\" rules=\"cols\" style=\"border - width:1px; border - style:None; width: 100 %; border - collapse:collapse; \">";
                                sScript += "<tbody>";
                                sScript += "<tr class=\"CartListHead\">";
                                sScript += "<td align=\"justify\">Nominativo</td>";
                                sScript += "<td align=\"justify\">Cod.Fiscale/P.IVA</td>";
                                sScript += "<td align=\"justify\">Diritto</td>";
                                sScript += "<td align=\"justify\">% possesso</td>";
                                sScript += "</tr>";
                                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(ListUICatasto);
                                foreach (object myInt in myJObject["terreni"]["intestatari"])
                                {
                                    var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                    if (((myDetIntest["denominazione"].ToString() != string.Empty) ? myDetIntest["denominazione"].ToString() : myDetIntest["cognome"].ToString()) != string.Empty)
                                    {
                                        sScript += "<tr class=\"CartListItem\">";
                                        sScript += "<td align=\"justify\">" + ((myDetIntest["denominazione"].ToString() != string.Empty) ? myDetIntest["denominazione"].ToString().Replace("'", "&rsquo;") : myDetIntest["cognome"].ToString().Replace("'", "&rsquo;") + " " + myDetIntest["nome"].ToString()).Replace("'", "&rsquo;") + "</td>";
                                        sScript += "<td align=\"justify\">" + myDetIntest["codfiscale"].ToString() + "</td>";
                                        sScript += "<td align=\"justify\">" + myDetIntest["diritto"].ToString().Replace("'", "&rsquo;") + "</td>";
                                        sScript += "<td align=\"right\">" + (decimal.Parse((myDetIntest["quotanum"].ToString() != string.Empty ? myDetIntest["quotanum"].ToString() : "0")) > 0 ? (((decimal.Parse((myDetIntest["quotanum"].ToString() != string.Empty ? myDetIntest["quotanum"].ToString() : "0")) / decimal.Parse((myDetIntest["quotaden"].ToString() != string.Empty ? myDetIntest["quotaden"].ToString() : "0"))) * 100)).ToString("F") : "0,00") + "</td>";
                                        sScript += "</tr>";
                                    }
                                }
                                foreach (object myInt in myJObject["fabbricati"]["intestatari"])
                                {
                                    var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                    if (((myDetIntest["denominazione"].ToString() != string.Empty) ? myDetIntest["denominazione"].ToString() : myDetIntest["cognome"].ToString()) != string.Empty)
                                    {
                                        sScript += "<tr class=\"CartListItem\">";
                                        sScript += "<td align=\"justify\">" + ((myDetIntest["denominazione"].ToString() != string.Empty) ? myDetIntest["denominazione"].ToString().Replace("'", "&rsquo;") : myDetIntest["cognome"].ToString().Replace("'", "&rsquo;") + " " + myDetIntest["nome"].ToString().Replace("'", "&rsquo;")) + "</td>";
                                        sScript += "<td align=\"justify\">" + myDetIntest["codfiscale"].ToString() + "</td>";
                                        sScript += "<td align=\"justify\">" + myDetIntest["diritto"].ToString().Replace("'", "&rsquo;") + "</td>";
                                        sScript += "<td align=\"right\">" + (decimal.Parse((myDetIntest["quotanum"].ToString() != string.Empty ? myDetIntest["quotanum"].ToString() : "0")) > 0 ? (((decimal.Parse((myDetIntest["quotanum"].ToString() != string.Empty ? myDetIntest["quotanum"].ToString() : "0")) / decimal.Parse((myDetIntest["quotaden"].ToString() != string.Empty ? myDetIntest["quotaden"].ToString() : "0"))) * 100)).ToString("F") : "0,00") + "</td>";
                                        sScript += "</tr>";
                                    }
                                }
                                sScript += "</tbody>";
                                sScript += "</table>";
                                sScript += "</div>";
                                MySession.Current.scriptComproprietari = sScript;
                                RegisterScript("$('#divComproprietari').html('" + sScript + "')", this.GetType());
                            }
                        }
                        catch (HttpException ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Riepilogo.consultacatasto::errore::", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICI.Dich.LoadComproprietari::errore::", ex);
            }
        }       
    }
}