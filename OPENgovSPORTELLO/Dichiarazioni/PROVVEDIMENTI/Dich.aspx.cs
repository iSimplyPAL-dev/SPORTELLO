using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI
{
    public partial class Dich : BaseIstanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dich));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.PROVVEDIMENTI fncMng = new BLL.PROVVEDIMENTI();

        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.UIDettaglio + General.TRIBUTO.PROVVEDIMENTI, MySession.Current.Ente.IDEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dich.Page_Init::errore::", ex);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string sScript = string.Empty;

                if (MySession.Current.IsInitDich)
                    if (!Page.IsPostBack)
                    {
                        General fncGen = new General();
                        SPC_Provvedimento myProvvedimento = new SPC_Provvedimento();

                        if (MySession.Current.IdRifCalcolo > 0)
                        {
                            if (!fncMng.LoadDich(MySession.Current.Ente.IDEnte, MySession.Current.UserLogged.IDContribToWork, MySession.Current.IdRifCalcolo, out myProvvedimento))
                                RegisterScript("$('#lblErrorFO').text('Errore in caricamento pagina!');$('#lblErrorFO').show();", this.GetType());
                        }
                        LoadForm(myProvvedimento);
                        RegisterScript(new BLL.Profilo().LoadJumbotron(MySession.Current.myAnag, MySession.Current.UserLogged.IDContribLogged), this.GetType());
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Page_Load", "ingresso pagina", General.TRIBUTO.PROVVEDIMENTI, "", MySession.Current.Ente.IDEnte);
                    }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dich.Page_Load::errore::", ex);
                LoadException(ex);//throw ex;
            }
            finally
            {
                if (MySession.Current.Scope == "FO")
                    RegisterScript("$('#FAQ').addClass('HelpFOPROVVEDIMENTI');", this.GetType());
                else
                    RegisterScript("$('#FAQ').addClass('HelpBOPROVVEDIMENTI');", this.GetType());
            }
        }
        #region "Bottoni"
        protected void Back(object sender, EventArgs e)
        {
            MySession.Current.TipoIstanza = string.Empty;
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Tributi", "Dich", "Back", "uscita pagina", General.TRIBUTO.PROVVEDIMENTI, "", MySession.Current.Ente.IDEnte);
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetGestRiepilogoPROVVEDIMENTI, Response);
        }
        #endregion
        #region "Griglie"
        protected void GrdUIRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if (((HiddenField)e.Row.FindControl("hfIdTributo")).Value == Utility.Costanti.TRIBUTO_ICI || ((HiddenField)e.Row.FindControl("hfIdTributo")).Value == Utility.Costanti.TRIBUTO_TASI)
                    {
                        GrdDich.Columns[3].Visible = false;
                        GrdDich.Columns[7].Visible = false;
                        GrdDich.Columns[6].HeaderText = "Consistenza";
                        GrdAcc.Columns[3].Visible = false;
                        GrdAcc.Columns[7].Visible = false;
                        GrdAcc.Columns[6].HeaderText = "Consistenza";
                    }
                    else if (((HiddenField)e.Row.FindControl("hfIdTributo")).Value == Utility.Costanti.TRIBUTO_OSAP)
                    {
                        GrdDich.Columns[3].HeaderText = "Durata";
                        GrdDich.Columns[5].HeaderText = "Tariffa €";
                        GrdDich.Columns[6].HeaderText = "Consistenza";
                        GrdAcc.Columns[3].HeaderText = "Durata";
                        GrdAcc.Columns[5].HeaderText = "Tariffa €";
                        GrdAcc.Columns[6].HeaderText = "Consistenza";
                    }
                    else if (((HiddenField)e.Row.FindControl("hfIdTributo")).Value == Utility.Costanti.TRIBUTO_TARSU)
                    {
                        GrdDich.Columns[5].HeaderText = "Tariffa €";
                        GrdAcc.Columns[5].HeaderText = "Tariffa €";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dich.GrdUIRowDataBound::errore::", ex);
                LoadException(ex);//throw ex;
            }
        }
        #endregion
        #region "Set Data Into Form"
        private void LoadForm(SPC_Provvedimento myItem)
        {
            string sScript = string.Empty;
            try
            {
                sScript += "document.getElementById('lblRidDifImp').innerText='Ente: " + myItem.ImpRidotto.DiffImposta + "';";
                sScript += "document.getElementById('lblRidInte').innerText='Ente: " + myItem.ImpRidotto.Interessi + "';";
                sScript += "document.getElementById('lblRidSanz').innerText='Ente: " + myItem.ImpRidotto.Sanzioni + "';";
                sScript += "document.getElementById('lblRidSanzNoRid').innerText='Ente: " + myItem.ImpRidotto.SanzioniNonRid + "';";
                sScript += "document.getElementById('lblRidArr').innerText='Ente: " + myItem.ImpRidotto.Arrotondamento + "';";
                sScript += "document.getElementById('lblRidSpese').innerText='Ente: " + myItem.ImpRidotto.SpeseNotifica + "';";
                sScript += "document.getElementById('lblRidTot').innerText='Ente: " + myItem.ImpRidotto.Totale + "';";

                sScript += "document.getElementById('lblTotInte').innerText='Ente: " + myItem.ImpPieno.DiffImposta + "';";
                sScript += "document.getElementById('lblTotSanz').innerText='Ente: " + myItem.ImpPieno.Interessi + "';";
                sScript += "document.getElementById('lblTotSanzNoRid').innerText='Ente: " + myItem.ImpPieno.Sanzioni + "';";
                sScript += "document.getElementById('lblTotArr').innerText='Ente: " + myItem.ImpPieno.SanzioniNonRid + "';";
                sScript += "document.getElementById('lblTotSpese').innerText='Ente: " + myItem.ImpPieno.Arrotondamento + "';";
                sScript += "document.getElementById('lblTotTot').innerText='Ente: " + myItem.ImpPieno.Totale + "';";

                sScript += "$('#lblTotSanzNoRid').hide();";
                RegisterScript(sScript, this.GetType());

                GrdDich.DataSource = myItem.ListDichiarato;
                GrdDich.DataBind();
                GrdAcc.DataSource = myItem.ListAccertato;
                GrdAcc.DataBind();
                GrdSanz.DataSource = myItem.ListSanzioni;
                GrdSanz.DataBind();
                GrdInt.DataSource = myItem.ListInteressi;
                GrdInt.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dich.LoadForm::errore::", ex);
                throw new Exception();
            }
        }
        #endregion
    }
}