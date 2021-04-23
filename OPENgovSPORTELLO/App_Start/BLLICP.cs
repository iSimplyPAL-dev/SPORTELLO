using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;
using System.Web.UI.WebControls;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe generale di gestione ICP
    /// </summary>
    public class ICP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ICP));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="IDIstanza"></param>
        /// <param name="OrigineUI"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadICPUIDich(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, string OrigineUI, object myTypeObj, out List<object> ListObj)
        {
            ListObj = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetICP_UI", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA", "TIPO");
                    if (myTypeObj.GetType() == typeof(SPC_DichICP))
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICP>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                    , ctx.GetParam("TIPO", OrigineUI)
                                ).ToList<SPC_DichICP>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                    , ctx.GetParam("TIPO", OrigineUI)
                                ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICP.LoadICPUIDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TypeConsulta"></param>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="IDIstanza"></param>
        /// <param name="ListUI"></param>
        /// <returns></returns>
        public bool LoadDich(string TypeConsulta, string IDEnte, int IDContribuente, int IDRif, int IDIstanza, out List<SPC_DichICP> ListUI)
        {
            ListUI = new List<SPC_DichICP>();
            try
            {
                List<object> ListObj = new List<object>();
                List<GenericCategory> ListAgevolaz = new List<GenericCategory>();
                if (!LoadICPUIDich(IDEnte, IDContribuente, IDRif, IDIstanza, "", new SPC_DichICP(), out ListObj))
                {
                    return false;
                }
                foreach(SPC_DichICP myItem in ListObj)
                ListUI.Add( myItem);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICP.LoadDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TipoIstanza"></param>
        /// <param name="myIstanza"></param>
        /// <param name="myDich"></param>
        /// <param name="ScriptError"></param>
        /// <returns></returns>
        public bool FieldValidator(string TipoIstanza, Istanza myIstanza, List<SPC_DichICP> myDich, out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                if (myDich != null)
                {
                    foreach (SPC_DichICP myItem in myDich)
                    {
                        if (myItem.Via == string.Empty)
                        {
                            ScriptError += "alert('Inserisci la Via!');";
                            return false;
                        }
                        else if (myItem.IDTipologia <= 0)
                        {
                            ScriptError += "alert('Inserisci la Tipologia!');";
                            return false;
                        }
                        else if (myItem.IDCaratteristica <= 0)
                        {
                            ScriptError += "alert('Inserisci la Caratteristica!');";
                            return false;
                        }
                        else if (myItem.IDTipoDurata <= 0)
                        {
                            ScriptError += "alert('Inserisci la Tipologia di Durata!');";
                            return false;
                        }
                        else if (myItem.DataInizio == DateTime.MaxValue)
                        {
                            ScriptError += "alert('Inserisci la Data Inizio!');";
                            return false;
                        }
                        else if (myItem.Qta <= 0)
                        {
                            ScriptError += "alert('Inserisci la Quantità!');";
                            return false;
                        }
                        else {
                            if (TipoIstanza == Istanza.TIPO.Cessazione && (myItem.DataFine == DateTime.MaxValue))
                            {
                                ScriptError += "alert('Compilare la data fine!');";
                                return false;
                            }
                        }
                    }
                }
                else {
                    ScriptError += "alert('Compilare tutti i campi!');";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICP.FieldValidator::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <param name="DataAtto"></param>
        /// <param name="NAtto"></param>
        /// <returns></returns>
        public SPC_DichICP LoadUIFromGrd(GridViewRow myRow, DateTime DataAtto, string NAtto)
        {
            try
            {
                SPC_DichICP myItem = new SPC_DichICP();

                myItem.ID = int.Parse(((HiddenField)myRow.FindControl("hfIdRow")).Value.ToString());
                myItem.DataAtto = DataAtto;
                myItem.NAtto = NAtto;
                myItem.Via = ((TextBox)myRow.FindControl("txtVia")).Text;
                myItem.IDVia = int.Parse(((HiddenField)myRow.FindControl("hfIdVia")).Value);
                myItem.Civico = ((TextBox)myRow.FindControl("txtCivico")).Text;
                int idTipologia;
                int.TryParse(((DropDownList)myRow.FindControl("ddlTipologia")).SelectedValue, out idTipologia);
                myItem.IDTipologia = idTipologia;
                myItem.DescrTipologia = ((DropDownList)myRow.FindControl("ddlTipologia")).SelectedItem.Text;
                int idCaratteristica;
                int.TryParse(((DropDownList)myRow.FindControl("ddlCaratteristica")).SelectedValue, out idCaratteristica);
                myItem.IDCaratteristica = idCaratteristica;
                myItem.DescrCaratteristica = ((DropDownList)myRow.FindControl("ddlCaratteristica")).SelectedItem.Text;
                int idTipoDurata;
                int.TryParse(((DropDownList)myRow.FindControl("ddlTipoDurata")).SelectedValue, out idTipoDurata);
                myItem.IDTipoDurata = idTipoDurata;
                myItem.DescrTipoDurata = ((DropDownList)myRow.FindControl("ddlTipoDurata")).SelectedItem.Text;
                myItem.Mezzo = ((TextBox)myRow.FindControl("txtMezzo")).Text;
                if(((TextBox)myRow.FindControl("txtDataInizio")).Text!=string.Empty)
                myItem.DataInizio = DateTime.Parse(((TextBox)myRow.FindControl("txtDataInizio")).Text);
                if(((TextBox)myRow.FindControl("txtDataFine")).Text!=string.Empty)
                myItem.DataFine = DateTime.Parse(((TextBox)myRow.FindControl("txtDataFine")).Text);
                myItem.Qta = decimal.Parse(((TextBox)myRow.FindControl("txtQta")).Text.Replace(".", ","));

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICP.LoadUIFromGrd::errore::", ex);
                return new SPC_DichICP();
            }
        }
        #region "Emesso/Versato"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadDovutoVersato(string IDEnte, int IDContribuente, out List<RiepilogoDovuto> ListDovuto)
        {
            ListDovuto = new List<RiepilogoDovuto>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetICP_Dovuto", "IDENTE", "IDCONTRIBUENTE");
                    ListDovuto = ctx.ContextDB.Database.SqlQuery<RiepilogoDovuto>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).ToList<RiepilogoDovuto>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICP.LoadDovutoVersato::errore::", ex);
                return false;
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione dichiarazione ICP
    /// </summary>
    public class BLLSPC_DichICP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichICP));
        private SPC_DichICP InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichICP(SPC_DichICP myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIstanza"></param>
        /// <param name="sScriptDich"></param>
        /// <returns></returns>
        public bool Save(Istanza itemIstanza, ref string sScriptDich)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONIICP_IU", "ID", "IDISTANZA", "IDRIFORG"
                        , "DATAATTO", "NATTO"
                        , "IDVIA", "VIA", "CIVICO"
                        , "DATAINIZIO", "DATAFINE"
                        , "IDTIPOLOGIA", "IDCARATTERISTICA", "IDTIPODURATA"
                        , "MEZZO", "QUANTITA");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDRIFORG", InnerObj.IDRifOrg)
                            , ctx.GetParam("DATAATTO", InnerObj.DataAtto)
                            , ctx.GetParam("NATTO", InnerObj.NAtto)
                            , ctx.GetParam("IDVIA", InnerObj.IDVia)
                            , ctx.GetParam("VIA", InnerObj.Via)
                            , ctx.GetParam("CIVICO", InnerObj.Civico)
                            , ctx.GetParam("DATAINIZIO", InnerObj.DataInizio)
                            , ctx.GetParam("DATAFINE", InnerObj.DataFine)
                            , ctx.GetParam("IDTIPOLOGIA", InnerObj.IDTipologia)
                            , ctx.GetParam("IDCARATTERISTICA", InnerObj.IDCaratteristica)
                            , ctx.GetParam("IDTIPODURATA", InnerObj.IDTipoDurata)
                            , ctx.GetParam("MEZZO", InnerObj.Mezzo)
                            , ctx.GetParam("QUANTITA", InnerObj.Qta)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICP.Save::errore in inserimento dichiarazione");
                        ctx.Dispose();
                        return false;
                    }
                    ctx.Dispose();
                }
                sScriptDich += GetRPTIstanza(itemIstanza, InnerObj);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICP.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        /// <param name="myUI"></param>
        /// <returns></returns>
        public string GetRPTIstanza(Istanza myItem, SPC_DichICP myUI)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dichICP_UI'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichICPndich'><input type='text' class='dichICP_header' id='dichICPHndich' value='N.'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDndich' value='" + myUI.NAtto + "'>";
                sScript += "</div>";
                sScript += "<div id='dichICPdata'><input type='text' class='dichICP_header' id='dichICPHdata' value=''>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDdata' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataAtto) + "'>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichICPindirizzo'><input type='text' class='dichICP_header' id='dichICPHindirizzo' value='Indirizzo'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDindirizzo' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichICPtipologia'><input type='text' class='dichICP_header' id='dichICPHtipologia' value='tipologia'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDtipologia' value='" + myUI.DescrTipologia.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichICPcat'><input type='text' class='dichICP_header' id='dichICPHcaratteristica' value='caratteristica'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDcaratteristica' value='" + myUI.DescrCaratteristica.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichICPtipodurata'><input type='text' class='dichICP_header' id='dichICPHtipodurata' value='Durata'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDtipodurata' value='" + myUI.DescrTipoDurata + "'>";
                sScript += "</div>";
                sScript += "<div id='dichICPmezzo'><input type='text' class='dichICP_header' id='dichICPHmezzo' value='mezzo'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDmezzo' value='" + myUI.Mezzo + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichICPinizio'><input type='text' class='dichICP_header' id='dichICPHinizio' value='Inizio del possesso'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDinizio' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichICPfine'><input type='text' class='dichICP_header' id='dichICPHfine' value='Termine del possesso'>";
                sScript += "<input type='text' class='dichICP_dett' id='dichICPDfine' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.ICP.Dich.GetRPTIstanza::errore::", ex);
                return string.Empty;
            }
        }
    }
}