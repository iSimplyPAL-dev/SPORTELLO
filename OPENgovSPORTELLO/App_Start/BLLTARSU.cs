using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;
using Anagrafica.DLL;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe di gestione TARSU
    /// </summary>
    public class TARSU
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TARSU));
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
        public bool LoadTARSUUIDich(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, string OrigineUI, object myTypeObj, out List<object> ListObj)
        {
            ListObj = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_UI", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA", "TIPO");
                    if (myTypeObj.GetType() == typeof(SPC_DichTARSU))
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTARSU>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                    , ctx.GetParam("TIPO", OrigineUI)
                                ).ToList<SPC_DichTARSU>()) as IEnumerable<object>).Cast<object>().ToList();
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
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadTARSUUIDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDRif"></param>
        /// <param name="IDIstanza"></param>
        /// <param name="Tipo"></param>
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadTARSUVani(string IDEnte, int IDRif, int IDIstanza, string Tipo, out List<SPC_DichTARSUVani> ListObj)
        {
            ListObj = new List<SPC_DichTARSUVani>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_UIVani", "IDENTE", "IDRIFORG", "IDISTANZA", "TIPO");
                    ListObj = ctx.ContextDB.Database.SqlQuery<SPC_DichTARSUVani>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDRIFORG", IDRif)
                            , ctx.GetParam("IDISTANZA", IDIstanza)
                            , ctx.GetParam("TIPO", Tipo)
                        ).ToList<SPC_DichTARSUVani>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadTARSUVani::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDRif"></param>
        /// <param name="IDIstanza"></param>
        /// <param name="NDichiarazione"></param>
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadTARSUOccupanti(string IDEnte, int IDRif, int IDIstanza, int NDichiarazione, out List<SPC_DichTARSUOccupanti> ListObj)
        {
            ListObj = new List<SPC_DichTARSUOccupanti>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_UIOccupanti", "IDRIFORG", "IDISTANZA", "NDICHIARAZIONE");
                    ListObj = ctx.ContextDB.Database.SqlQuery<SPC_DichTARSUOccupanti>(sSQL, ctx.GetParam("IDRIFORG", IDRif)
                            , ctx.GetParam("IDISTANZA", IDIstanza)
                            , ctx.GetParam("NDICHIARAZIONE", NDichiarazione)
                        ).ToList<SPC_DichTARSUOccupanti>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadTARSUOccupanti::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="IDIstanza"></param>
        /// <param name="OrigineUI"></param>
        /// <param name="TypeObj"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadTARSURidEse(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, string OrigineUI, string TypeObj, object myTypeObj, out List<object> ListObj)
        {
            ListObj = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_UIRidEse", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "TIPO", "IDISTANZA", "TIPOUI");
                    if (myTypeObj.GetType() == typeof(SPC_DichTARSURidEse))
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTARSURidEse>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("TIPO", TypeObj)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                    , ctx.GetParam("TIPOUI", OrigineUI)
                            ).ToList<SPC_DichTARSURidEse>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<RidEseTARSU>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("TIPO", TypeObj)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                    , ctx.GetParam("TIPOUI", OrigineUI)
                            ).ToList<RidEseTARSU>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    Log.Debug("LoadTARSURidEse= prc_GetTARSU_UIRidEse '" + IDEnte + "', " + IDContribuente.ToString() + "," + IDRif.ToString() + ", '" + TypeObj + "', " + IDIstanza.ToString() + ", '" + OrigineUI + "'");
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadTARSURidEse::errore::", ex);
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
        /// <param name="myDich"></param>
        /// <returns></returns>
        public bool LoadDich(string TypeConsulta, string IDEnte, int IDContribuente, int IDRif, int IDIstanza, out SPC_DichTARSU myDich)
        {
            myDich = new SPC_DichTARSU();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    List<object> ListUI = new List<object>();
                    List<SPC_DichTARSUVani> ListVani = new List<SPC_DichTARSUVani>();
                    List<SPC_DichTARSUOccupanti> ListOccupanti = new List<SPC_DichTARSUOccupanti>();
                    if (TypeConsulta == Istanza.TIPO.ConsultaDich)
                    {
                        if (!LoadTARSUUIDich(IDEnte, IDContribuente, IDRif, IDIstanza, "", new SPC_DichTARSU(), out ListUI))
                        {
                            return false;
                        }
                        else {
                            if (ListUI.Count > 0)
                            {
                                myDich = (SPC_DichTARSU)ListUI[0];
                                if (!LoadTARSURidEse(IDEnte, IDContribuente, IDRif, IDIstanza, "", SPC_DichTARSURidEse.TYPE.Riduzione, new SPC_DichTARSURidEse(), out ListUI))
                                    return false;
                                else
                                {
                                    foreach (object myObj in ListUI)
                                    {
                                        myDich.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                    }
                                }
                                if (!LoadTARSURidEse(IDEnte, IDContribuente, IDRif, IDIstanza, "", SPC_DichTARSURidEse.TYPE.Esenzione, new SPC_DichTARSURidEse(), out ListUI))
                                    return false;
                                else
                                {
                                    foreach (object myObj in ListUI)
                                    {
                                        myDich.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                    }
                                }
                                if (!LoadTARSUVani(IDEnte, IDRif, IDIstanza, myDich.Tipo, out ListVani))
                                    return false;
                                else
                                    myDich.ListVani = ListVani;
                                if (!LoadTARSUOccupanti(IDEnte, IDRif, IDIstanza, -1, out ListOccupanti))
                                    return false;
                                else
                                    myDich.ListOccupanti = ListOccupanti;
                            }
                        }
                    }
                    else if (TypeConsulta.StartsWith(Istanza.TIPO.ConsultaDich))
                    {
                        if (!LoadTARSUUIDichStorico(IDEnte, IDContribuente, IDRif, new SPC_DichTARSU(), out ListUI))
                        {
                            return false;
                        }
                        else
                        {
                            if (ListUI.Count > 0)
                            {
                                myDich = (SPC_DichTARSU)ListUI[0];
                                if (!LoadTARSURidEse(IDEnte, IDContribuente, IDRif, IDIstanza, "", SPC_DichTARSURidEse.TYPE.Riduzione, new SPC_DichTARSURidEse(), out ListUI))
                                    return false;
                                else
                                {
                                    foreach (object myObj in ListUI)
                                    {
                                        myDich.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                    }
                                }
                                if (!LoadTARSURidEse(IDEnte, IDContribuente, IDRif, IDIstanza, "", SPC_DichTARSURidEse.TYPE.Esenzione, new SPC_DichTARSURidEse(), out ListUI))
                                    return false;
                                else
                                {
                                    foreach (object myObj in ListUI)
                                    {
                                        myDich.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                    }
                                }
                                if (!LoadTARSUVani(IDEnte, IDRif, IDIstanza, myDich.Tipo, out ListVani))
                                    return false;
                                else
                                    myDich.ListVani = ListVani;
                                if (!LoadTARSUOccupanti(IDEnte, IDRif, IDIstanza, -1, out ListOccupanti))
                                    return false;
                                else
                                    myDich.ListOccupanti = ListOccupanti;
                            }
                        }
                    }
                    else
                    {
                        if (!LoadTARSUUIDich(IDEnte, IDContribuente, IDRif, IDIstanza, "", new SPC_DichTARSU(), out ListUI))
                        {
                            return false;
                        }
                        else {
                            if (ListUI.Count > 0)
                            {
                                myDich = (SPC_DichTARSU)ListUI[0];
                                if (!LoadTARSURidEse(IDEnte, IDContribuente, IDRif, IDIstanza, "", SPC_DichTARSURidEse.TYPE.Riduzione, new SPC_DichTARSURidEse(), out ListUI))
                                    return false;
                                else
                                {
                                    foreach (object myObj in ListUI)
                                    {
                                        myDich.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                    }
                                }
                                if (!LoadTARSURidEse(IDEnte, IDContribuente, IDRif, IDIstanza, "", SPC_DichTARSURidEse.TYPE.Esenzione, new SPC_DichTARSURidEse(), out ListUI))
                                    return false;
                                else
                                {
                                    foreach (object myObj in ListUI)
                                    {
                                        myDich.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                    }
                                }
                                if (!LoadTARSUVani(IDEnte, IDRif, IDIstanza, myDich.Tipo, out ListVani))
                                    return false;
                                else
                                    myDich.ListVani = ListVani;
                                if (!LoadTARSUOccupanti(IDEnte, IDRif, IDIstanza, -1, out ListOccupanti))
                                    return false;
                                else
                                    myDich.ListOccupanti = ListOccupanti;
                            }
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="ListUICatasto"></param>
        /// <returns></returns>
        public bool LoadTARSUUICatasto(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListUICatasto)
        {
            ListUICatasto = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_UICatasto", "IDENTE", "IDCONTRIBUENTE", "IDRIF");
                    if (myTypeObj.GetType() == typeof(SPC_DichTARSU))
                    {
                        ListUICatasto = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTARSU>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIF", IDRif)
                                ).ToList<SPC_DichTARSU>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUICatasto = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("IDRIF", IDRif)
                            ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadTARSUUICatasto::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <param name="IdUI"></param>
        /// <param name="bMemoID"></param>
        /// <returns></returns>
        public SPC_DichTARSUVani LoadVanoFromGrd(GridViewRow myRow, int IdUI, bool bMemoID)
        {
            SPC_DichTARSUVani myItem = new SPC_DichTARSUVani();
            try
            {
                myItem.ID = (bMemoID ? int.Parse(((HiddenField)myRow.FindControl("hfIdRow")).Value.ToString()) : -1);
                myItem.IDDichTARSU = IdUI;
                myItem.ScopeCat = ((DropDownList)myRow.FindControl("ddlUtilizzo")).SelectedValue;
                myItem.ScopeCatDescr = ((DropDownList)myRow.FindControl("ddlUtilizzo")).SelectedItem.Text;
                int idCat;
                int.TryParse(((DropDownList)myRow.FindControl("ddlCatND")).SelectedValue, out idCat);
                myItem.IDCatTIA = idCat;
                myItem.CodCategoriaDescr = (((DropDownList)myRow.FindControl("ddlCatND")).SelectedItem != null) ? ((DropDownList)myRow.FindControl("ddlCatND")).SelectedItem.Text : string.Empty;
                if (myItem.ScopeCat != "N")
                {
                    myItem.NComponenti = int.Parse(((TextBox)myRow.FindControl("txtNC")).Text);
                    if (myItem.ScopeCat == "D")
                    {
                        myItem.NComponentiPV = myItem.NComponenti;
                        myItem.CategoriaEstesa = "Domestica " + myItem.NComponenti.ToString() + " Componenti";
                    }
                    else {
                        myItem.CategoriaEstesa = "Pertinenziale " + myItem.NComponenti.ToString() + " Componenti";
                    }
                }
                else {
                    myItem.CategoriaEstesa = (((DropDownList)myRow.FindControl("ddlCatND")).SelectedItem != null) ? ((DropDownList)myRow.FindControl("ddlCatND")).SelectedItem.Text : string.Empty;
                }
                int idVano;
                int.TryParse(((DropDownList)myRow.FindControl("ddlVani")).SelectedValue, out idVano);
                myItem.IDTipoVano = idVano;
                myItem.DescrVano = ((DropDownList)myRow.FindControl("ddlVani")).SelectedItem.Text;
                myItem.MQ = decimal.Parse(((TextBox)myRow.FindControl("txtMQ")).Text.Replace(".", ","));
                myItem.IsEsente = ((((CheckBox)myRow.FindControl("chkEsente")).Checked) ? 1 : 0);

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadVanoFromGrd::errore::", ex);
                return myItem;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <param name="IdUI"></param>
        /// <returns></returns>
        public SPC_DichTARSUOccupanti LoadOccupantiFromGrd(GridViewRow myRow, int IdUI)
        {
            SPC_DichTARSUOccupanti myItem = new SPC_DichTARSUOccupanti();
            try
            {
                myItem.ID = int.Parse(((HiddenField)myRow.FindControl("hfIdRow")).Value.ToString());
                myItem.IDDichTARSU = IdUI;
                myItem.Nominativo = ((TextBox)myRow.FindControl("txtNominativo")).Text;
                myItem.CodFiscale = ((TextBox)myRow.FindControl("txtCF")).Text;
                myItem.DataNascita = (((TextBox)myRow.FindControl("txtDataNascita")).Text != string.Empty) ? DateTime.Parse(((TextBox)myRow.FindControl("txtDataNascita")).Text) : DateTime.MaxValue;
                int idParent;
                int.TryParse(((DropDownList)myRow.FindControl("ddlParentela")).SelectedValue, out idParent);
                myItem.IDParentela = idParent;
                myItem.DescrParentela = ((DropDownList)myRow.FindControl("ddlParentela")).SelectedItem.Text;
                myItem.LuogoNascita = ((TextBox)myRow.FindControl("txtLuogoNascita")).Text;

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadOccupantiFromGrd::errore::", ex);
                return myItem;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TipoIstanza"></param>
        /// <param name="myIstanza"></param>
        /// <param name="myDich"></param>
        /// <param name="DataInizioOrg"></param>
        /// <param name="ScriptError"></param>
        /// <returns></returns>
        public bool FieldValidator(string TipoIstanza, Istanza myIstanza, SPC_DichTARSU myDich, DateTime DataInizioOrg, out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                if (myDich != null)
                {
                    if (myDich.Foglio == string.Empty || myDich.Numero == string.Empty)
                    {
                        ScriptError += "alert('Compilare foglio e numero!');";
                        return false;
                    }
                    if (myDich.DataFine <= myDich.DataInizio)
                    {
                        ScriptError += "alert('La data fine deve essere successiva a quella di inizio!');";
                        return false;
                    }
                    if (myDich.DataInizio > DateTime.Now)
                    {
                        ScriptError += "alert('La data inizio non può essere successiva a quella odierna!');";
                        return false;
                    }
                    if (myDich.DataInizio < DataInizioOrg)
                    {
                        ScriptError += "alert('La data inizio deve essere successiva a quella iniziale!');";
                        return false;
                    }
                    else {
                        if (TipoIstanza == Istanza.TIPO.Cessazione && (myDich.DataFine == DateTime.MaxValue))
                        {
                            ScriptError += "alert('Compilare la data fine!');";
                            return false;
                        }
                        if ((TipoIstanza == Istanza.TIPO.Inagibilità || TipoIstanza == Istanza.TIPO.Inutilizzabilità) && (myDich.DataInizio == DateTime.MaxValue || myDich.IDStatoOccupazione == string.Empty))
                        {
                            ScriptError += "alert('Compilare data inizio e/o stato occupazione!');";
                            return false;
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
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.FieldValidator::errore::", ex);
                return false;
            }
        }
        #region Storico
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="ListUIDich"></param>
        /// <returns></returns>
        public bool LoadTARSUUIDichStorico(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListUIDich)
        {
            ListUIDich = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_UIStorico", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG");
                    if (myTypeObj.GetType() == typeof(SPC_DichTARSU))
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTARSU>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<SPC_DichTARSU>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadTARSUUIDichStorico::errore::", ex);
                return false;
            }
        }
        #endregion
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
                    string sSQL = ctx.GetSQL("prc_GetTARSU_Dovuto", "IDENTE", "IDCONTRIBUENTE");
                    ListDovuto = ctx.ContextDB.Database.SqlQuery<RiepilogoDovuto>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).ToList<RiepilogoDovuto>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TARSU.LoadDovutoVersato::errore::", ex);
                return false;
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione dichiarazioni TARSU
    /// </summary>
    public class BLLSPC_DichTARSU
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichTARSU));
        private SPC_DichTARSU InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichTARSU(SPC_DichTARSU myItem)
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
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONITARSU_IU", "ID", "IDISTANZA", "IDRIFORG", "IDVIA", "VIA", "CIVICO", "ESPONENTE", "INTERNO", "SCALA", "FOGLIO", "NUMERO", "SUBALTERNO", "DATA_INIZIO", "DATA_FINE", "IDSTATOOCCUPAZIONE", "IDTIPOVANO", "IDCATTIA", "NCOMPONENTI", "NCOMPONENTI_PV", "NVANI", "MQ", "FLAG_ESENTE", "NOTE");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDRIFORG", InnerObj.IDRifOrg)
                            , ctx.GetParam("IDVIA", InnerObj.IDVia)
                            , ctx.GetParam("VIA", InnerObj.Via)
                            , ctx.GetParam("CIVICO", InnerObj.Civico)
                            , ctx.GetParam("ESPONENTE", InnerObj.Esponente)
                            , ctx.GetParam("INTERNO", InnerObj.Interno)
                            , ctx.GetParam("SCALA", InnerObj.Scala)
                            , ctx.GetParam("FOGLIO", InnerObj.Foglio)
                            , ctx.GetParam("NUMERO", InnerObj.Numero)
                            , ctx.GetParam("SUBALTERNO", InnerObj.Sub)
                            , ctx.GetParam("DATA_INIZIO", InnerObj.DataInizio)
                            , ctx.GetParam("DATA_FINE", InnerObj.DataFine)
                            , ctx.GetParam("IDSTATOOCCUPAZIONE", InnerObj.IDStatoOccupazione)
                            , ctx.GetParam("IDTIPOVANO", InnerObj.IDTipoVano)
                            , ctx.GetParam("NVANI", InnerObj.NVani)
                            , ctx.GetParam("IDCATTIA", InnerObj.IDCatTIA)
                            , ctx.GetParam("NCOMPONENTI", InnerObj.NComponenti)
                            , ctx.GetParam("NCOMPONENTI_PV", InnerObj.NComponentiPV)
                            , ctx.GetParam("MQ", InnerObj.MQ)
                            , ctx.GetParam("FLAG_ESENTE", InnerObj.IsEsente)
                            , ctx.GetParam("NOTE", InnerObj.Note)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSU.Save::errore in inserimento dichiarazione");
                        return false;
                    }
                    else
                    {
                        foreach (SPC_DichTARSUVani myItem in InnerObj.ListVani)
                        {
                            myItem.IDDichTARSU = InnerObj.ID;
                            if (!new BLLSPC_DichTARSUVani(myItem).Save())
                                return false;
                            else
                                if (myItem.ID <= 0)
                                return false;
                        }
                        foreach (SPC_DichTARSUOccupanti myItem in InnerObj.ListOccupanti)
                        {
                            myItem.IDDichTARSU = InnerObj.ID;
                            if (!new BLLSPC_DichTARSUOccupanti(myItem).Save())
                                return false;
                            else
                                if (myItem.ID <= 0)
                                return false;
                        }
                        foreach (SPC_DichTARSURidEse myItem in InnerObj.ListRid)
                        {
                            if (int.Parse(myItem.Codice) > 0)
                            {
                                myItem.IDDichTARSU = InnerObj.ID;
                                if (!new BLLSPC_DichTARSURidEse(myItem).Save())
                                    return false;
                                else
                                    if (myItem.ID <= 0)
                                    return false;
                            }
                        }
                        foreach (SPC_DichTARSURidEse myItem in InnerObj.ListEse)
                        {
                            if (int.Parse(myItem.Codice) > 0)
                            {
                                myItem.IDDichTARSU = InnerObj.ID;
                                if (!new BLLSPC_DichTARSURidEse(myItem).Save())
                                    return false;
                                else
                                    if (myItem.ID <= 0)
                                    return false;
                            }
                        }
                    }
                }
                sScriptDich += GetRPTIstanza(itemIstanza, InnerObj);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSU.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DalOld"></param>
        /// <param name="DalPres"></param>
        /// <param name="AlPres"></param>
        /// <param name="itemIstanza"></param>
        /// <param name="itemUIOrg"></param>
        /// <param name="sScriptDich"></param>
        /// <returns></returns>
        public bool SaveVariazione(DateTime DalOld, DateTime DalPres, DateTime AlPres, Istanza itemIstanza, SPC_DichTARSU itemUIOrg, ref string sScriptDich)
        {
            try
            {
                InnerObj.ID = -1;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!Save(itemIstanza, ref sScriptDich))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSU.SaveVariazione::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DalOld"></param>
        /// <param name="DalPres"></param>
        /// <param name="AlPres"></param>
        /// <param name="itemIstanza"></param>
        /// <param name="itemUIOrg"></param>
        /// <param name="sScriptDich"></param>
        /// <returns></returns>
        public bool SaveInagibile(DateTime DalOld, DateTime DalPres, DateTime AlPres, Istanza itemIstanza, SPC_DichTARSU itemUIOrg, ref string sScriptDich)
        {
            try
            {
                //inserisco il periodo digitato
                InnerObj.ID = -1;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!Save(itemIstanza, ref sScriptDich))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSU.SaveInagibile::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DalOld"></param>
        /// <param name="DalPres"></param>
        /// <param name="AlPres"></param>
        /// <param name="itemIstanza"></param>
        /// <param name="itemUIOrg"></param>
        /// <param name="sScriptDich"></param>
        /// <returns></returns>
        public bool SaveInutilizzato(DateTime DalOld, DateTime DalPres, DateTime AlPres, Istanza itemIstanza, SPC_DichTARSU itemUIOrg, ref string sScriptDich)
        {
            try
            {
                //inserisco il periodo digitato
                InnerObj.ID = -1;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!Save(itemIstanza, ref sScriptDich))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSU.SaveInutilizzato::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        /// <param name="myUI"></param>
        /// <returns></returns>
        public string GetRPTIstanza(Istanza myItem, SPC_DichTARSU myUI)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dichTARSU_UI'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichTARSUindirizzo'><input type='text' class='dich_header' id='dichTARSUHindirizzo' value='Indirizzo'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDindirizzo' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichTARSUfoglio'><input type='text' class='dich_header' id='dichTARSUHfoglio' value='Foglio'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDfoglio' value='" + myUI.Foglio + "'>";
                sScript += "</div>";
                sScript += "<div id='dichTARSUnumero'><input type='text' class='dich_header' id='dichHnumero' value='Particella'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnumero' value='" + myUI.Numero + "'>";
                sScript += "</div>";
                sScript += "<div id='dichTARSUsub'><input type='text' class='dich_header' id='dichTARSUHsub' value='Subalterno'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDsub' value='" + myUI.Sub + "'>";
                sScript += "</div>";
                sScript += "<div id='dichTARSUstatooccupaz'><input type='text' class='dich_header' id='dichTARSUHstatooccupaz' value='Stato Occupazione'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDstatooccupaz' value='" + myUI.StatoOccupazione.Replace("...", "") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichprot'><input type='text' class='dich_header' id='dichHprot' value='n.Protocollo'><input type='text' class='dich_dett' id='dichDprot' value=''></div>";
                sScript += "<div id='dichTARSUanno'><input type='text' class='dich_header' id='dichTARSUHanno' value='Anno'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDanno' value='" + myUI.DataInizio.Year.ToString() + "'>";
                sScript += "</div>";
                sScript += "</div>";
                foreach (SPC_DichTARSUVani myVano in myUI.ListVani)
                {
                    sScript += "<div class='dicrow'>";
                    sScript += "<div id='dichTARSUcat'><input type='text' class='dich_header' id='dichTARSUHcat' value='Categoria'>";
                    sScript += "<input type='text' class='dich_dett' id='dichTARSUDcat' value='" + myVano.CategoriaEstesa + "'>";
                    sScript += "</div>";
                    sScript += "<div id='dichTARSUmq'><input type='text' class='dich_header' id='dichTARSUHmq' value='MQ'>";
                    sScript += "<input type='text' class='dich_dett' id='dichTARSUDmq' value='" + myVano.MQ + "'>";
                    sScript += "</div>";
                    sScript += "<div id='dichTARSUvano'><input type='text' class='dich_header' id='dichTARSUHvano' value='Vano'>";
                    sScript += "<input type='text' class='dich_dett' id='dichTARSUDvano' value='" + myVano.DescrVano + "'>";
                    sScript += "</div>";
                    sScript += "<div id='dichTARSUesente'><input type='text' class='dich_header' id='dichTARSUHese' value='Esente'>";
                    sScript += "<input type='text' id='dichTARSUDesente' class='dich_dett' value='";
                    if (myVano.IsEsente == 1)
                        sScript += "X";
                    sScript += "'></div>";
                    sScript += "</div>";
                }
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichTARSUrid'><input type='text' class='dich_header' id='dichTARSUHrid' value='Riduzioni'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDrid' value='";
                foreach (SPC_DichTARSURidEse myRid in myUI.ListRid)
                {
                    if (myRid.Descrizione != "...")
                        sScript += "- " + myRid.Descrizione + " ";
                }
                sScript += "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichTARSUese'><input type='text' class='dich_header' id='dichTARSUHese' value='Esenzioni'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDese' value='";
                foreach (SPC_DichTARSURidEse myEse in myUI.ListEse)
                {
                    if (myEse.Descrizione != "...")
                        sScript += "- " + myEse.Descrizione + " ";
                }
                sScript += "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichTARSUinizio'><input type='text' class='dich_header' id='dichTARSUHinizio' value='Inizio del possesso o variazione imposta'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDinizio' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichTARSUfine'><input type='text' class='dich_header' id='dichTARSUHfine' value='Termine del possesso o variazione imposta'>";
                sScript += "<input type='text' class='dich_dett' id='dichTARSUDfine' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich.GetRPTIstanza::errore::", ex);
                return string.Empty;
            }
        }
    }
    /// <summary>
    /// Classe di gestione vani dichiarazioni TARSU
    /// </summary>
    public class BLLSPC_DichTARSUVani
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichTARSUVani));
        private SPC_DichTARSUVani InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichTARSUVani(SPC_DichTARSUVani myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONITARSU_VANI_IU", "ID", "IDDICHTARSU", "IDTIPOVANO", "IDCATTIA", "NCOMPONENTI", "NCOMPONENTI_PV", "NVANI", "MQ", "FLAG_ESENTE");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDDICHTARSU", InnerObj.IDDichTARSU)
                            , ctx.GetParam("IDTIPOVANO", InnerObj.IDTipoVano)
                            , ctx.GetParam("NVANI", InnerObj.NVani)
                            , ctx.GetParam("IDCATTIA", InnerObj.IDCatTIA)
                            , ctx.GetParam("NCOMPONENTI", InnerObj.NComponenti)
                            , ctx.GetParam("NCOMPONENTI_PV", InnerObj.NComponentiPV)
                            , ctx.GetParam("MQ", InnerObj.MQ)
                            , ctx.GetParam("FLAG_ESENTE", InnerObj.IsEsente)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSUVani.Save::errore in inserimento dichiarazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSUVani.Save::errore::", ex);
                return false;
            }
        }
    }
    /// <summary>
    /// Classe di gestione occupanti dichiarazioni TARSU
    /// </summary>
    public class BLLSPC_DichTARSUOccupanti
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichTARSUOccupanti));
        private SPC_DichTARSUOccupanti InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichTARSUOccupanti(SPC_DichTARSUOccupanti myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONITARSU_OCCUPANTI_IU", "ID", "IDDICH", "NOMINATIVO", "CODFISCALE", "LUOGONASCITA", "DATANASCITA", "IDPARENTELA");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDDICH", InnerObj.IDDichTARSU)
                            , ctx.GetParam("NOMINATIVO", InnerObj.Nominativo)
                            , ctx.GetParam("CODFISCALE", InnerObj.CodFiscale)
                            , ctx.GetParam("LUOGONASCITA", InnerObj.LuogoNascita)
                            , ctx.GetParam("DATANASCITA", InnerObj.DataNascita)
                            , ctx.GetParam("IDPARENTELA", InnerObj.IDParentela)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSUOccupanti.Save::errore in inserimento dichiarazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSUOccupanti.Save::errore::", ex);
                return false;
            }
        }
    }
    /// <summary>
    /// Classe di gestione riduzioni/esenzioni dichiarazioni TARSU
    /// </summary>
    public class BLLSPC_DichTARSURidEse
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichTARSURidEse));
        private SPC_DichTARSURidEse InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichTARSURidEse(SPC_DichTARSURidEse myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONITARSU_RIDESE_IU", "ID", "IDDICH", "IDTYPE", "CODICE");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDDICH", InnerObj.IDDichTARSU)
                            , ctx.GetParam("IDTYPE", InnerObj.IDType)
                            , ctx.GetParam("CODICE", InnerObj.Codice)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSURidEse.Save::errore in inserimento dichiarazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTARSURidEse.Save::errore::", ex);
                return false;
            }
        }
    }
    /// <summary>
    /// Classe di gestione Tessere TARSU
    /// </summary>
    public class TESSERE
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TESSERE));
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenericCategory> LoadTipoTessera()
        {
            try
            {
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTipoTessera");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TESSERE.LoadTipoTessera::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadTESSEREUIDich(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListObj)
        {
            ListObj = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTessere", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG");
                    if (myTypeObj.GetType() == typeof(SPC_DichTESSERE))
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTESSERE>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<SPC_DichTESSERE>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TESSERE.LoadTESSEREUIDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="TipoPeriodo"></param>
        /// <param name="NPeriodo"></param>
        /// <param name="myDich"></param>
        /// <returns></returns>
        public bool LoadDich(string IDEnte, int IDContribuente, int IDRif, DateTime Dal, DateTime Al, int TipoPeriodo, int NPeriodo, out SPC_DichTESSERE myDich)
        {
            myDich = new SPC_DichTESSERE();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    List<object> ListUI = new List<object>();
                    if (!LoadTESSEREUIDich(IDEnte, IDContribuente, IDRif, new SPC_DichTESSERE(), out ListUI))
                    {
                        return false;
                    }
                    else {
                        if (ListUI.Count > 0)
                        {
                            myDich = (SPC_DichTESSERE)ListUI[0];
                            myDich.ListConferimenti = LoadTESSEREConferimenti(IDEnte, IDRif, Dal, Al, TipoPeriodo, NPeriodo);
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TESSERE.LoadDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IDRif"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="TipoPeriodo"></param>
        /// <param name="NPeriodo"></param>
        /// <returns></returns>
        public List<EventiRaffronto> LoadTESSEREConferimenti(string IdEnte, int IDRif, DateTime Dal, DateTime Al, int TipoPeriodo, int NPeriodo)
        {
            try
            {
                List<EventiRaffronto> ListMyData = new List<EventiRaffronto>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetConferimenti", "IDENTE"
                        , "IDRIF"
                        , "DAL"
                        , "AL"
                        , "TIPOPERIODO"
                        , "NPERIODO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<EventiRaffronto>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("IDRIF", IDRif)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("TIPOPERIODO", TipoPeriodo)
                            , ctx.GetParam("NPERIODO", NPeriodo)
                        ).ToList<EventiRaffronto>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TESSERE.LoadTESSEREConferimenti::errore::", ex);
                return null;
            }
        }
    }
}