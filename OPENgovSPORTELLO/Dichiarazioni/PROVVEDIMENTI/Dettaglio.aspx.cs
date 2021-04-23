using log4net;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Dettaglio : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dettaglio));
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
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.PROVVEDIMENTI, MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dettaglio.Page_Init::errore::", ex);
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
                    SPC_Provvedimento myProvvedimento = new SPC_Provvedimento();

                    if (MySession.Current.IdRifCalcolo > 0)
                    {
                        if (!new BLL.PROVVEDIMENTI().LoadDich(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, out myProvvedimento))
                            RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                    }
                    LoadForm(myProvvedimento);
                    RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.PROVVEDIMENTI, "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dettaglio.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dettaglio.Page_Load.header->" + GrdDich.Columns[6].HeaderText + " " + GrdAcc.Columns[6].HeaderText);
                if (MySession.Current.Scope == "FO")
                    RegisterScript("$('#FAQ').addClass('HelpFOPROVVEDIMENTI');", this.GetType());
                else
                    RegisterScript("$('#FAQ').addClass('HelpBOPROVVEDIMENTI');", this.GetType());
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
            MySession.Current.TipoIstanza = string.Empty;
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.PROVVEDIMENTI, "", MySession.Current.Ente.IDEnte);
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoPROVVEDIMENTI, Response);
        }
        #endregion
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dettaglio.GrdRowDataBound.sono su header");
                    if (hfIdTributo.Value == Utility.Costanti.TRIBUTO_ICI || hfIdTributo.Value == Utility.Costanti.TRIBUTO_TASI)
                    {
                         GrdDich.Columns[3].Visible = false;
                        GrdDich.Columns[7].Visible = false;
                        GrdAcc.Columns[3].Visible = false;
                        GrdAcc.Columns[7].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dettaglio.GrdRowDataBound.errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        #region "Set Data Into Form"
        /// <summary>
        /// Funzione per il caricamento dei dati nella pagina
        /// </summary>
        /// <param name="myItem">SPC_Provvedimento oggetto da caricare</param>
        private void LoadForm(SPC_Provvedimento myItem)
        {
            string sScript = string.Empty;
            try
            {
                foreach (SPC_UIProvvedimento myUI in myItem.ListAccertato)
                {
                    hfIdTributo.Value = myUI.IDTributo;
                    break;
                }
                sScript = "<p class='lead_bold'>" + myItem.Descrizione;
                sScript += " PER L&rsquo;ANNO " + myItem.Anno + "</p>";
                sScript += "<p class='lead_normal text-right'>NOTIFICATO IL " + FncGrd.FormattaDataGrd(myItem.DataNotifica) + "</p>";
                sScript += "<p class='lead_Emphasized text-right'>STATO " + myItem.Stato + "</p>";
                divIntestazione.InnerHtml = sScript;

                sScript = "";
                sScript += "document.getElementById('lblRidDifImp').innerText='" + myItem.ImpRidotto.DiffImposta + " €';";
                sScript += "document.getElementById('lblRidInte').innerText='" + myItem.ImpRidotto.Interessi + " €';";
                sScript += "document.getElementById('lblRidSanz').innerText='" + myItem.ImpRidotto.Sanzioni + " €';";
                sScript += "document.getElementById('lblRidSanzNoRid').innerText='" + myItem.ImpRidotto.SanzioniNonRid + " €';";
                sScript += "document.getElementById('lblRidArr').innerText='" + myItem.ImpRidotto.Arrotondamento + " €';";
                sScript += "document.getElementById('lblRidSpese').innerText='" + myItem.ImpRidotto.SpeseNotifica + " €';";
                sScript += "document.getElementById('lblRidTot').innerText='" + myItem.ImpRidotto.Totale + " €';";

                sScript += "document.getElementById('lblTotDifImp').innerText='" + myItem.ImpPieno.DiffImposta + " €';";
                sScript += "document.getElementById('lblTotInte').innerText='" + myItem.ImpPieno.Interessi + " €';";
                sScript += "document.getElementById('lblTotSanz').innerText='" + myItem.ImpPieno.Sanzioni + " €';";
                sScript += "document.getElementById('lblTotSanzNoRid').innerText='" + myItem.ImpPieno.SanzioniNonRid + " €';";
                sScript += "document.getElementById('lblTotArr').innerText='" + myItem.ImpPieno.Arrotondamento + " €';";
                sScript += "document.getElementById('lblTotSpese').innerText='" + myItem.ImpPieno.SpeseNotifica + " €';";
                sScript += "document.getElementById('lblTotTot').innerText='" + myItem.ImpPieno.Totale + " €';";
                sScript += "document.getElementById('lblTotPag').innerText='" + myItem.Pagato + " €';";
                if (myItem.Pagato > 0)
                {
                    sScript += "$('#lblTotPag').removeClass('text-danger');$('#lblTotPag').addClass('text-success');";
                }
                else {
                    sScript += "$('#lblTotPag').removeClass('text-success');$('#lblTotPag').addClass('text-danger');";
                }
                RegisterScript(sScript, this.GetType());

                GrdSanz.DataSource = myItem.ListSanzioni;
                GrdSanz.DataBind();
                GrdInt.DataSource = myItem.ListInteressi;
                GrdInt.DataBind();
                GrdDich.DataSource = myItem.ListDichiarato;
                GrdDich.DataBind();
                GrdAcc.DataSource = myItem.ListAccertato;
                GrdAcc.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dettaglio.LoadForm.errore::", ex);
                throw new Exception();
            }
        }
        #endregion
    }
}