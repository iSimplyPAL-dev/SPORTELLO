using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.Settings
{
    /// <summary>
    /// Dal server deve essere possibile inviare comunicazioni verso chi ha scaricato l’APP, ma anche verso le persone registrate allo sportello on-line.
    /// La funzione di back office deve consentire di scegliere la tipologia di comunicazione, ed in base alla scelta inviare a contribuenti diversi.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class MngMessages : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MngMessages));
        protected FunctionGrd FncGrd = new FunctionGrd();
        private string sScript = string.Empty;

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
                sScript += new BLL.GestForm().GetLabel(BLL.GestForm.FormName.Comunicazioni , string.Empty);
                sScript += new BLL.GestForm().GetHelp("HelpBOMngMessages", (MySession.Current.Ente!= null? MySession.Current.Ente.UrlWiki:string.Empty));
                sScript += "$('.BottoneSave').hide();$('.BottoneSearch').show();";
                sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();";
                sScript += "$('#divSubSetAnag').hide();";
                sScript += "$('.divGrdBtn').hide();";
                RegisterScript(sScript, this.GetType());
                ShowHide("divMng", false); ShowHide("divVisual", false); ; ShowHide("divDest", false);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.Page_Init::errore::", ex);
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
                if (!Page.IsPostBack)
                {
                    ShowHide("SearchSubset", false);
                    List<GenericCategory> ListUserEnti = new BLL.User(new UserRole() { NameUser = MySession.Current.UserLogged.NameUser, IDTipoProfilo = MySession.Current.UserLogged.IDTipoProfilo }).LoadUserEnti(string.Empty, MySession.Current.UserLogged.NameUser);
                    new General().LoadCombo(ddlEnte, ListUserEnti, "CODICE", "DESCRIZIONE");
                    new General().LoadCombo(ddlDestinatari, new BLL.Settings().LoadConfigForDDL(string.Empty, 0, GenericCategory.TIPO.MsgRecipient, string.Empty, string.Empty), "CODICE", "DESCRIZIONE");

                    new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Messaggi", "Page_Load", "ingresso pagina", "", "", "");
                    if (MySession.Current.IdMessage > 0)
                    {
                        List<Message> ListMessages = new BLL.Messages(new Message()).LoadMessages(string.Empty, -1, string.Empty, DateTime.MaxValue, MySession.Current.IdMessage);
                        txtMessage.Text = ListMessages[0].Testo;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.Page_Load::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                if (MySession.Current.IdMessage > 0)
                {
                    ShowHide("divMng", true); ShowHide("divVisual", false); ShowHide("divParamSearch", false);

                    sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();";
                    sScript += "$('.BottoneDiv').hide();$('.BottoneSave').hide();$('.BottoneSearch').hide();";
                    RegisterScript(sScript, this.GetType());
                }
                else
                {
                    sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();";
                    RegisterScript(sScript, this.GetType());
                }
            }
        }
        #region "Bottoni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Search(object sender, EventArgs e)
        {
            try
            {
                List<Message> ListMessages = new BLL.Messages(new Message()).LoadMessages(ddlEnte.SelectedValue, int.Parse(ddlDestinatari.SelectedValue), GetMezzo(), ((txtDataInvio.Text != string.Empty) ? DateTime.Parse(txtDataInvio.Text) : DateTime.MaxValue), -1);
                GrdMessages.DataSource = ListMessages;
                GrdMessages.DataBind();
                MySession.Current.ListMessages = ListMessages;
                ShowHide("divMng", false); ShowHide("divVisual", true);
                RegisterScript("Azzera();$('.BottoneSave').hide();$('.BottoneSearch').show();", this.GetType());
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Messaggi", "Search", "", "", "", ddlEnte.SelectedValue);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.Search::errore", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide("SearchSubset", false);
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();";
                RegisterScript(sScript, this.GetType());
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void NewMsg(object sender, EventArgs e)
        {
            try
            {
                LoadSearchSubset();
                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Messaggi", "Nuovo Messaggio", "", "", "", ddlEnte.SelectedValue);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.NewMsg::errore", ex);
                LoadException(ex);
            }
            finally
            {
                sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();";
                RegisterScript(sScript, this.GetType());
            }
        }
        /// <summary>
        /// Bottone per l'uscita dalla videata
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Back(object sender, EventArgs e)
        {
            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Messaggi", "Back", "uscita pagina", "", "", "");
            if (MySession.Current.IsBackToTributi)
                IdentityHelper.RedirectToReturnUrl(UrlHelper.GetFOTributi, Response);
            else
                Response.Redirect(UrlHelper.GetBO_ReportGen);
        }
        /// <summary>
        /// Bottone per il salvataggio dei dati
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save(object sender, EventArgs e)
        {
            try
            {
                string sMyErr = string.Empty;
                string sEnte = ddlEnte.SelectedValue;
                if (int.Parse(hfIdRow.Value) > 0)
                    sEnte = ddlEnteDest.SelectedValue;
                hfSubsetRecipient.Value = string.Empty;
                if (int.Parse(ddlDestinatari.SelectedValue) == 3)
                {
                    if(txtCognome.Text.Trim()==string.Empty && txtNome.Text.Trim() == string.Empty && txtCFPIVA.Text.Trim() == string.Empty )
                    {
                        string sScript = "$('#lblErrorBO').text('Inserire Cognome Nome e Cod.Fiscale/P.IVA!');$('#lblErrorBO').show();";
                        RegisterScript(sScript, this.GetType());
                        return;
                    }
                    List<GenericCategory> ListSubset = new List<GenericCategory>();
                    ListSubset = new BLL.Settings().LoadSubsetRecipient(ddlEnte.SelectedValue, int.Parse(ddlDestinatari.SelectedValue), txtCognome.Text, txtNome.Text, txtCFPIVA.Text);
                    foreach(GenericCategory mySubSet in ListSubset )
                    {
                        hfSubsetRecipient.Value += mySubSet.ID;
                    }
                }
                else {
                    foreach (GridViewRow myRow in GrdSubset.Rows)
                    {
                        if (((CheckBox)myRow.FindControl("chkSel")).Checked)
                        {
                            hfSubsetRecipient.Value += ((hfSubsetRecipient.Value != string.Empty) ? "," : "") + ((HiddenField)myRow.FindControl("hfIdSubset")).Value;
                        }
                    }
                }
                if (new BLL.Messages(new Message()).FieldValidator(sEnte, int.Parse(ddlDestinatari.SelectedValue), hfSubsetRecipient.Value, GetMezzo(), ((txtDataInvio.Text != string.Empty) ? DateTime.Parse(txtDataInvio.Text) : DateTime.MaxValue), txtMessage.Text, out sMyErr))
                {
                    if (!new BLL.Messages(new Message { IDEnte = sEnte, IDTypeRecipient = int.Parse(ddlDestinatari.SelectedValue), SubsetRecipient = hfSubsetRecipient.Value, TypeMezzo = GetMezzo(), DataInvio = ((txtDataInvio.Text != string.Empty) ? DateTime.Parse(txtDataInvio.Text) : DateTime.MaxValue), Testo = txtMessage.Text }).Save())
                    {
                        RegisterScript(sMyErr, this.GetType());
                    }
                    else
                    {
                        if (ddlEnte.SelectedValue != string.Empty)
                        {
                            MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ddlEnte.SelectedValue, MySession.Current.UserLogged.NameUser);
                        }
                        else
                            MySession.Current.Ente = null;
                        new BLL.Messages(new Message()).SendMail();
                        List<Message> ListMessages = new BLL.Messages(new Message()).LoadMessages(ddlEnte.SelectedValue, int.Parse(ddlDestinatari.SelectedValue), GetMezzo(), ((txtDataInvio.Text != string.Empty) ? DateTime.Parse(txtDataInvio.Text) : DateTime.MaxValue), -1);
                        GrdMessages.DataSource = ListMessages;
                        GrdMessages.DataBind();
                        ShowHide("divMng", false); ShowHide("divVisual", true);
                        hfIdRow.Value = "-1";
                        string sScript = "Azzera();$('.BottoneSave').hide();$('.BottoneSearch').show();$('#MainContent_ddlEnte').attr('disabled','false');$('#MainContent_ddlDestinatari').attr('disabled','false');";
                        sScript += "$('#lblErrorBO').text('Salvataggio effettuato con successo!');$('#lblErrorBO').show();";
                        RegisterScript(sScript, this.GetType());
                        MySession.Current.Ente = null;
                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Messaggi", "Save", "salvataggio messaggio", "", "", ddlEnte.SelectedValue);
                    }
                }
                else
                {
                    sMyErr = "$('#lblErrorBO').text(" + sMyErr.Replace("alert(", "").Replace(");", "") + ");$('#lblErrorBO').show();";
                    RegisterScript(sMyErr, this.GetType());
                    RegisterScript("$('.BottoneSave').show();$('.BottoneSearch').hide();", this.GetType());
                    ShowHide("divMng", true); ShowHide("divVisual", false); ShowHide("divDest", false);
                    ShowHide("SearchSubset", true);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.Save::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide("SearchSubset", false);
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
                sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();";
                RegisterScript(sScript, this.GetType());
            }
        }
        #endregion
        #region "Griglie"
        /// <summary>
        /// Funzione di gestione cambio pagina della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdSubsetPageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            LoadSearchSubset(e.NewPageIndex);
            sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();";
            RegisterScript(sScript, this.GetType());
        }
        /// <summary>
        /// Funzione di gestione del popolamento della griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdMessagesRowDataBound(object sender, GridViewRowEventArgs e)
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
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.GrdMessagesRowDataBound::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// Funzione di gestione degli eventi sulla griglia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void GrdMessagesRowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                string sMyErr = string.Empty;
                int IDRow;
                int.TryParse(e.CommandArgument.ToString(), out IDRow);
                switch (e.CommandName)
                {
                    case "RowCopy":
                        foreach (Message myItem in MySession.Current.ListMessages)
                        {
                            if (myItem.ID == IDRow)
                            {
                                hfIdRow.Value = IDRow.ToString();
                                ddlEnte.SelectedValue = myItem.IDEnte;
                                new General().LoadCombo(ddlEnteDest, new BLL.Settings().LoadEnti(string.Empty, string.Empty), "CODICE", "DESCRIZIONE");
                                ddlDestinatari.SelectedValue = myItem.IDTypeRecipient.ToString();
                                if (myItem.TypeMezzo.IndexOf("A") > 0)
                                    chkApp.Checked = true;
                                if (myItem.TypeMezzo.IndexOf("M") > 0)
                                    chkMail.Checked = true;
                                txtDataInvio.Text = myItem.DataInvio.ToShortDateString();
                                txtMessage.Text = myItem.Testo;
                                LoadSearchSubset();
                                RegisterScript("$('.BottoneSave').show();$('.BottoneSearch').hide();$('#MainContent_ddlEnte').attr('disabled','true');$('#MainContent_ddlDestinatari').attr('disabled','true');", this.GetType());
                                ShowHide("divMng", true); ShowHide("divVisual", false); ShowHide("divDest", true);
                                ShowHide("SearchSubset", true);
                                break;
                            }
                        }
                        break;
                    case "RowDelete":
                        foreach (Message myItem in MySession.Current.ListMessages)
                        {
                            if (myItem.ID == IDRow)
                            {
                                if (myItem.DataInvio <= DateTime.Now)
                                {
                                    sScript += "$('#lblErrorBO').text('Impossibile eliminare un messaggio già inviato.');$('#lblErrorBO').show();";
                                    RegisterScript(sScript, this.GetType());
                                    return;
                                }
                                else
                                {
                                    if (!new BLL.Messages(new Message { ID = IDRow }).Delete())
                                    {
                                        RegisterScript(sMyErr, this.GetType());
                                        return;
                                    }
                                    else
                                    {
                                        List<Message> ListMessages = new BLL.Messages(new Message()).LoadMessages(ddlEnte.SelectedValue, int.Parse(ddlDestinatari.SelectedValue), GetMezzo(), ((txtDataInvio.Text != string.Empty) ? DateTime.Parse(txtDataInvio.Text) : DateTime.MaxValue), -1);
                                        GrdMessages.DataSource = ListMessages;
                                        GrdMessages.DataBind();
                                        ShowHide("divMng", false); ShowHide("divVisual", true);
                                        RegisterScript("Azzera();$('.BottoneSave').hide();$('.BottoneSearch').show();$('#lblErrorBO').text('Salvataggio effettuato con successo!');$('#lblErrorBO').show();", this.GetType());
                                        new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Configurazioni", "Messaggi", "RowCopy", "salvataggio messaggio", "", "", ddlEnte.SelectedValue);
                                    }
                                }
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.GrdMessagesRowCommand::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                ShowHide("SearchSubset", false);
                sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();";
                RegisterScript(sScript, this.GetType());
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetMezzo()
        {
            string Mezzo = string.Empty;
            try
            {
                if (chkSito.Checked)
                    Mezzo += Message.MEZZO.Sito;
                if (chkApp.Checked)
                    Mezzo += Message.MEZZO.App;
                if (chkMail.Checked)
                    Mezzo += Message.MEZZO.Mail;

                return Mezzo;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.GetMezzo::errore::", ex);
                return Mezzo;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        private void LoadSearchSubset(int? page = 0)
        {
            try
            {
                if (ddlEnte.SelectedValue == string.Empty)
                {
                    sScript += "$('#lblErrorBO').text('Selezionare un ente.');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                    ShowHide("divMng", false); ShowHide("divVisual", true);
                    return;
                }
                if (int.Parse(ddlDestinatari.SelectedValue) <= 0)
                {
                    sScript += "$('#lblErrorBO').text('Selezionare una tipologia di destinatari.');$('#lblErrorBO').show();";
                    RegisterScript(sScript, this.GetType());
                    ShowHide("divMng", false); ShowHide("divVisual", true);
                    return;
                }
                List<GenericCategory> ListSubset = new List<GenericCategory>();
                if (int.Parse(ddlDestinatari.SelectedValue) != 3)
                {
                    ListSubset = new BLL.Settings().LoadSubsetRecipient(ddlEnte.SelectedValue, int.Parse(ddlDestinatari.SelectedValue), string.Empty, string.Empty, string.Empty);
                    GrdSubset.DataSource = ListSubset;
                    if (ListSubset.Count > 0)
                        GrdSubset.PageSize = ListSubset.Count;
                    GrdSubset.AllowPaging = false;
                    if (page.HasValue)
                        GrdSubset.PageIndex = page.Value;
                    GrdSubset.DataBind();
                    if (GrdSubset.Rows.Count > 0)
                        GrdSubset.Visible = true;
                    else
                        GrdSubset.Visible = false;
                }
                else
                {
                    GrdSubset.Visible = false;
                    RegisterScript("$('#divSubSetAnag').show();", this.GetType());
                }
                RegisterScript("$('.BottoneSave').show();$('.BottoneSearch').hide();", this.GetType());
                ShowHide("divMng", true); ShowHide("divVisual", false); ShowHide("divDest", false);
                ShowHide("SearchSubset", true);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Settings.MngMessages.LoadSearchSubset::errore::", ex);
                LoadException(ex);
            }
        }
    }
}