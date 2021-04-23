using OPENgovSPORTELLO;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Ribes.OPENgov.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace OPENgovSPORTELLO.Istanze
{
    /// <summary>
    /// Le informazioni sono visualizzate tenendo conto del tipo di dichiarazione e del tributo. Viene visualizzato:
    /// <list type="bullet">
    /// <item>Ente</item>
    /// <item>Tributo</item>
    /// <item>Tipo dichiarazione</item>
    /// <item>Griglia con dettaglio stati passati(Stato, Data, Motivazione, etc.); se l’istanza è VALIDATA saranno visualizzate 3 righe per ognuno degli stati intercorsi: REGISTRATA, PRESA IN CARICO e VALIDATA</item>
    /// <item>Dati anagrafici completi(incluso indirizzo mail…) – i dati anagrafici sono quelli eventualmente comunicati dal contribuente</item>
    /// <item>Tutte le informazioni della dichiarazione </item>
    /// <item>Elenco documenti allegati</item>
    /// </list>
    /// Attenzione l’operatore del back office verifica manualmente la presenza di allegati contro firmati.
    /// <strong>Protocollo</strong>
    /// Il protocollo può essere interno(automatico) o esterno al sistema(a cura dell’Ente).
    /// Se <i>Interno</i> Lato FO, va bene che stampi la dichiarazione con possibilità di firma. Lato BO:
    /// •	arriva l’istanza, 
    /// •	in fase di “presa in carico” il sistema protocolla automaticamente l’istanza(registro elettronico del protocollo che riparte da 1 ad ogni inizio anno). 
    /// •	Nella risposta al contribuente di presa in carico, viene indicato il numero
    /// Se <i>Esterno</i> non c’è gestione automatico del protocollo, le dichiarazioni vengono inviate alla mail del protocollo ed il protocollo(autonomamente) invia le dichiarazioni all’ufficio tributi. Il back office riceve l’istanza ed in fase di presa in carico, chiede conferma all’operatore che abbia ricevuto la dichiarazione dal protocollo.
    /// <strong>Gestione incrociata con Verticale</strong>	
    /// Non devono essere fatte acquisizioni, ma, tramite voce di menu, il sistema presenta tutte le dichiarazioni inserite in attesa di validazione.
    /// Se nuova dichiarazione, il sistema popola una nuova dichiarazione e consente la verifica, correzione da parte dell’operatore.La provenienza viene compilata con …… (attenzione ora in open il campo è disabilitato).
    /// Il protocollo deve essere inserito a cura dell’ufficio in base a comunicazione dell’ufficio protocollo.
    /// Si completa con procedura alla pressione del pulsante “Salva “predispone una mail di risposta da inviare al contribuente con il dettaglio dichiarazione validato.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class BO_IstanzeDet : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_IstanzeDet));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private static Istanza myIstanza = new Istanza();
        private static string NominativoIstanza, RifCatIstanza, TributoIstanza, DatiIstanza = string.Empty;

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
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.Page_Init::errore::", ex);
            }
        }
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            List<string> ListDettaglio = new List<string>();/*{posizione 0-> nominativo, posizione 1-> rif cat, posizione 2-> tributo, posizione 3-> dati istanza}*/
            List<RiepilogoUIVerticale> ListTribRifCat = new List<RiepilogoUIVerticale>();
            List<RiepilogoUIVerticale> ListTribContrib = new List<RiepilogoUIVerticale>();
            try
            {
                if (!Page.IsPostBack)
                {
                    if (!new BLL.VerticaleTrib(new RiepilogoUIVerticale()).LoadIstanzeForVerticaleTrib(MySession.Current.IdIstanza, out ListDettaglio, out ListTribRifCat, out ListTribContrib))
                        RegisterScript("$('#OnlyNumber_error').text('Errore in caricamento pagina.');$('#OnlyNumber_error').show();", this.GetType());
                    else {
                        GrdRifCat.DataSource = ListTribRifCat;
                        GrdRifCat.DataBind();

                        GrdContrib.DataSource = ListTribContrib;
                        GrdContrib.DataBind();

                        NominativoIstanza = ListDettaglio[0];
                        RifCatIstanza = ListDettaglio[1];
                        TributoIstanza = ListDettaglio[2];
                        DatiIstanza = ListDettaglio[3];
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Page_Load", "ingresso pagina", "", "", "");
                    }
                }
                sScript += "$('#divDetUI').html('" + DatiIstanza + "');";
                sScript += "$('#lblHeadRifCat').text('Riferimento Catastale " + RifCatIstanza + "');";
                sScript += "$('#lblHeadContrib').text('Nominativo " + NominativoIstanza + "');";
                RegisterScript(sScript, this.GetType());
                if (TributoIstanza == General.TRIBUTO.TARSU)
                {
                    GrdRifCat.Columns[5].HeaderText = "MQ";
                    GrdContrib.Columns[6].HeaderText = "MQ";
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.Page_Load::errore::", ex);
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
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Back", "uscita pagina", "", "", "");
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_IstanzeGen, Response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NewDichiarazione(object sender, EventArgs e)
        {
            try
            {
                if (!new BLL.VerticaleTrib(new RiepilogoUIVerticale()).AddUI(MySession.Current.IdIstanza, MySession.Current.UserLogged.NameUser))
                    RegisterScript("$('#OnlyNumber_error').text('Errore in inserimento posizione.');$('#OnlyNumber_error').show();", this.GetType());
                else {
                    RegisterScript("$('#OnlyNumber_error').text('inserimento dichiarazione effettuata con successo.');$('#OnlyNumber_error').show();", this.GetType());
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "NewDichiarazione", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.NewDichiarazione::errore::", ex);
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
        protected void GrdRifCatRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.GrdRifCatRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdRifCatRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "RowOpen":
                        RowOpen(IDRow);
                        break;
                    case "RowClose":
                        RowClose(IDRow);
                        break;
                    case "RowCloseAdd":
                        RowCloseAdd(IDRow);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.GrdRifCatRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdContribRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.GrdContribRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdContribRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "RowOpen":
                        RowOpen(IDRow);
                        break;
                    case "RowClose":
                        RowClose(IDRow);
                        break;
                    case "RowCloseAdd":
                        RowCloseAdd(IDRow);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.GrdContribRowCommand::errore::", ex);
                LoadException(ex);
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdRow"></param>
        private void RowOpen(int IdRow)
        {
            string sScript = string.Empty;
            try
            {
                 if (TributoIstanza == General.TRIBUTO.ICI)
                {
                    sScript = "window.open('"+ UrlHelper.GetUrlVerticale+ UrlHelper.GetUrlVerticaleICI + "/FrmUI.aspx?ParamUIBody=IDImmobile=" + IdRow + "$TYPEOPERATION=DETTAGLIO$Sportello=1', '_blank');";
                }
                else if (TributoIstanza == General.TRIBUTO.TARSU)
                {
                    sScript = "window.open('" + UrlHelper.GetUrlVerticale + UrlHelper.GetUrlVerticaleTARSU + "/Dichiarazioni/FrmUI.aspx?ParamUIBody=IdUniqueUI=" + IdRow + "$AzioneProv=1$Provenienza=1$IsFromVariabile=0$Sportello=1', '_blank');";
                }
                else
                    sScript = "$('#OnlyNumber_error').text('Tributo non disponibile!');$('#OnlyNumber_error').show();";
                RegisterScript(sScript, this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Gestione Verticale", "RowOpen", "chiesto consultazione ui - istanza->" + MySession.Current.IdIstanza.ToString() + " - UI->" + IdRow.ToString(), "", "", "");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.RowOpen::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdRow"></param>
        private void RowClose(int IdRow)
        {
            try
            {
                if (!new BLL.VerticaleTrib(new RiepilogoUIVerticale()).CloseUI(MySession.Current.IdIstanza, IdRow, MySession.Current.UserLogged.NameUser))
                    RegisterScript("$('#OnlyNumber_error').text('Errore in chiusura posizione.');$('#OnlyNumber_error').show();", this.GetType());
                else {
                    RegisterScript("$('#OnlyNumber_error').text('Chiusura posizione effettuata con successo.');$('#OnlyNumber_error').show();", this.GetType());
                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Gestione Verticale", "RowClose", "chiusura ui - istanza->" + MySession.Current.IdIstanza.ToString() + " - UI->" + IdRow.ToString(), "", "", "");
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.RowClose::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdRow"></param>
        private void RowCloseAdd(int IdRow)
        {
            try
            {
                if (!new BLL.VerticaleTrib(new RiepilogoUIVerticale()).CloseUI(MySession.Current.IdIstanza, IdRow, MySession.Current.UserLogged.NameUser))
                    RegisterScript("$('#OnlyNumber_error').text('Errore in chiusura posizione.');$('#OnlyNumber_error').show();", this.GetType());
                else {
                    if (!new BLL.VerticaleTrib(new RiepilogoUIVerticale()).AddUI(MySession.Current.IdIstanza, MySession.Current.UserLogged.NameUser))
                        RegisterScript("$('#OnlyNumber_error').text('Errore in inserimento posizione.');$('#OnlyNumber_error').show();", this.GetType());
                    else {
                        RegisterScript("$('#OnlyNumber_error').text('Chiusura ed inserimento posizione effettuata con successo.');$('#OnlyNumber_error').show();", this.GetType());
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Gestione Verticale", "RowCloseAdd", "chiusura e riapertura ui - istanza->" + MySession.Current.IdIstanza.ToString() + " - UI->" + IdRow.ToString(), "", "", "");
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Istanze.BO_IstanzeDet.RowCloseAdd::errore::", ex);
                LoadException(ex);
            }
        }
    }
}