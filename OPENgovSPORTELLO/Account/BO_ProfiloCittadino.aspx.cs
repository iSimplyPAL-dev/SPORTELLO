using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Anagrafica.DLL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Account
{
    public partial class BO_ProfiloCittadino : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BO_ProfiloCittadino));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private BLL.Profilo fncMng = new BLL.Profilo();

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {

                if (!Page.IsPostBack)
                {
                    ShowHide(BLL.GestForm.PlaceHolderName.Body + "_DivIndSped", false);
                    ShowHide("SearchEnti", false);

                    DettaglioAnagrafica myAnag = new DettaglioAnagrafica();
                    myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);

                    LoadAnagrafica(myAnag);

                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.Page_Load::errore::", ex);
                throw ex;
            }
        }



        #region "Griglie"
        protected void GrdContattiDataBound(object sender, DataGridItemEventArgs e)
        {
            try
            {
                Label lblSID, lblDescrizione, lblIDRIFERIMENTO, lblDataInizioInvio;
                string strArgumentsID, strArgumentsDESC, strArgumentsIDRIFERIMENTO, strArgumentsDataInizioInvio;

                switch (e.Item.ItemType)
                {
                    case ListItemType.Item:
                        lblSID = ((Label)(e.Item.FindControl("lblTipoRiferimento")));
                        lblIDRIFERIMENTO = ((Label)(e.Item.FindControl("lblIDRIFERIMENTO")));
                        lblDescrizione = ((Label)(e.Item.FindControl("DatiRiferimento")));
                        lblDataInizioInvio = ((Label)(e.Item.FindControl("DataInizioInvio")));
                        strArgumentsID = ("\'"
                                    + (lblSID.Text + "\'"));
                        strArgumentsDESC = ("\'"
                                    + (lblDescrizione.Text + "\'"));
                        strArgumentsIDRIFERIMENTO = ("\'"
                                    + (lblIDRIFERIMENTO.Text + "\'"));
                        strArgumentsDataInizioInvio = ("\'"
                                    + (lblDataInizioInvio.Text.Replace("Data validità invio: ", "") + "\'"));
                        e.Item.Attributes.Add("OnClick", "ModificaContatti(" + strArgumentsID + "," + strArgumentsDESC + "," + strArgumentsIDRIFERIMENTO + "," + strArgumentsDataInizioInvio + ");");
                        break;
                }
                //if ((e.Item.ItemType == ListItemType.Item))
                //{
                //    e.Item.Attributes.Add("bgcolor", "White");
                //}

                //if ((e.Item.ItemType == ListItemType.AlternatingItem))
                //{
                //    e.Item.Attributes.Add("bgcolor", "WhiteSmoke");
                //}

                //e.Item.Attributes.Add("onmouseover", "this.className=\'riga_tabella_mouse_over_Normal\'");
                //e.Item.Attributes.Add("onmouseout", "this.className=\'riga_tabella_Normal\'");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO.GrdContattiDataBound.errore::", ex);
                throw ex;

            }
        }
        #region "Spedizione"
        protected void GrdInvioRowDataBound(object sender, GridViewRowEventArgs e)
        {
            DropDownList ddlMyDati = new DropDownList();
            ImageButton MyBtn;
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    if ((((ObjIndirizziSpedizione)(e.Row.DataItem)).ID_DATA_SPEDIZIONE == -1))
                    {
                        MyBtn = ((ImageButton)(e.Row.FindControl("imgOpen")));
                        MyBtn.CssClass = "SubmitBtn Bottone BottoneNewGrd";
                        e.Row.Cells[5].Enabled = false;
                    }
                    else {
                        MyBtn = ((ImageButton)(e.Row.FindControl("imgOpen")));
                        MyBtn.CssClass = "SubmitBtn Bottone BottoneOpenGrd";
                        e.Row.Cells[5].Enabled = true;
                    }
                }
                GrdInvio.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::GrdInvioRowDataBound::errore::", ex);
                throw ex;
            }
        }
        #endregion
        #endregion
        private void LoadAnagrafica(DettaglioAnagrafica myItem)
        {
            try
            {
                if (!(myItem == null))
                {
                    hdIdContribuente.Value = myItem.COD_CONTRIBUENTE.ToString();
                    hdIdAnagrafica.Value = myItem.ID_DATA_ANAGRAFICA.ToString();
                    hdCodRappresentanteLegale.Value = myItem.CodContribuenteRappLegale;
                    txtCodiceFiscale.Text = myItem.CodiceFiscale;
                    txtPartitaIva.Text = myItem.PartitaIva;
                    txtCognome.Text = myItem.Cognome;
                    txtNome.Text = myItem.Nome;
                    ddlSesso.SelectedValue = myItem.Sesso;
                    txtLuogoNascita.Text = myItem.ComuneNascita;
                    txtPVNascita.Text = myItem.ProvinciaNascita;
                    txtDataNascita.Text = myItem.DataNascita;
                    txtCodComuneNascita.Text = myItem.CodiceComuneNascita;
                    txtCodComuneResidenza.Text = myItem.CodiceComuneResidenza;
                    hdCodViaResidenza.Value = myItem.CodViaResidenza;
                    txtComuneRes.Text = myItem.ComuneResidenza;
                    txtCAPRes.Text = myItem.CapResidenza;
                    txtProvinciaRes.Text = myItem.ProvinciaResidenza;
                    txtViaRes.Text = myItem.ViaResidenza;
                    txtCivicoRes.Text = myItem.CivicoResidenza;
                    txtEsponenteCivicoRes.Text = myItem.EsponenteCivicoResidenza;
                    txtScalaRes.Text = myItem.ScalaCivicoResidenza;
                    txtInternoRes.Text = myItem.InternoCivicoResidenza;
                    txtFrazioneRes.Text = myItem.FrazioneResidenza;
                    myItem.ListSpedizioni.Add(new ObjIndirizziSpedizione());
                    GrdInvio.DataSource = myItem.ListSpedizioni;
                    GrdInvio.DataBind();
                    GrdContatti.DataSource = myItem.dsContatti;
                    GrdContatti.DataBind();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.ProfiloFO::LoadToForm::errore::", ex);
                throw ex;
            }
        }
        protected void Back(object sender, EventArgs e)
        {
            IdentityHelper.RedirectToReturnUrl(UrlHelper.GetBO_AnalisiEventi, Response);
        }
    }
}