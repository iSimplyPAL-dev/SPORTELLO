using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe di gestione OSAP
    /// </summary>
    public class OSAP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(OSAP));
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
        public bool LoadOSAPUIDich(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, string OrigineUI, object myTypeObj, out List<object> ListObj)
        {
            ListObj = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetOSAP_UI", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA", "TIPO");
                    if (myTypeObj.GetType() == typeof(SPC_DichOSAP))
                    {
                        ListObj = ((ctx.ContextDB.Database.SqlQuery<SPC_DichOSAP>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                    , ctx.GetParam("TIPO", OrigineUI)
                                ).ToList<SPC_DichOSAP>()) as IEnumerable<object>).Cast<object>().ToList();
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
                Log.Debug("OPENgovSPORTELLO.BLL.OSAP.LoadOSAPUIDich::errore::", ex);
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
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadOSAPAgevolazioni(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, out List<GenericCategory> ListObj)
        {
            ListObj = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetOSAP_UIAgevolazioni", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA");
                    ListObj = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                   , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                   , ctx.GetParam("IDRIFORG", IDRif)
                                   , ctx.GetParam("IDISTANZA", IDIstanza)
                               ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.OSAP.LoadOSAPAgevolazioni::errore::", ex);
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
        public bool LoadDich(string TypeConsulta, string IDEnte, int IDContribuente, int IDRif, int IDIstanza, out SPC_DichOSAP myDich)
        {
            myDich = new SPC_DichOSAP();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = String.Empty;
                    List<object> ListObj = new List<object>();
                    List<GenericCategory> ListAgevolaz = new List<GenericCategory>();
                    if (!LoadOSAPUIDich(IDEnte, IDContribuente, IDRif, IDIstanza,"", new SPC_DichOSAP(), out ListObj))
                    {
                        return false;
                    }
                    else {
                        if (ListObj.Count > 0)
                        {
                            myDich = (SPC_DichOSAP)ListObj[0];
                            if (!LoadOSAPAgevolazioni(IDEnte, IDContribuente, IDRif, IDIstanza, out ListAgevolaz))
                                return false;
                            else
                                myDich.ListAgevolazioni = ListAgevolaz;
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.OSAP.LoadDich::errore::", ex);
                return false;
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
        public bool FieldValidator(string TipoIstanza, Istanza myIstanza, SPC_DichOSAP myDich, DateTime DataInizioOrg, out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                if (myDich != null)
                {
                    if (TipoIstanza == Istanza.TIPO.Cessazione && (myDich.DataFine == DateTime.MaxValue))
                    {
                        ScriptError += "alert('Compilare la data fine!');";
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
                }
                else {
                    ScriptError += "alert('Compilare tutti i campi!');";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.OSAP.FieldValidator::errore::", ex);
                return false;
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
                    string sSQL = ctx.GetSQL("prc_GetOSAP_Dovuto", "IDENTE", "IDCONTRIBUENTE");
                    ListDovuto = ctx.ContextDB.Database.SqlQuery<RiepilogoDovuto>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).ToList<RiepilogoDovuto>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.OSAP.LoadDovutoVersato::errore::", ex);
                return false;
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione dichiarazioni OSAP
    /// </summary>
    public class BLLSPC_DichOSAP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichOSAP));
        private SPC_DichOSAP InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichOSAP(SPC_DichOSAP myItem)
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
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONIOSAP_IU", "ID", "IDISTANZA", "IDRIFORG"
                        , "IDTIPOATTO", "DATAATTO", "NATTO", "IDRICHIEDENTE", "IDTRIBUTO"
                        , "IDVIA", "VIA", "CIVICO"
                        , "DATAINIZIO", "DATAFINE", "IDTIPODURATA", "DURATA"
                        , "IDCONSISTENZA", "CONSISTENZA"
                        , "IDCATEGORIA", "IDOCCUPAZIONE", "ISATTRAZIONE"
                        , "PERCMAGG", "IMPDETRAZ");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDRIFORG", InnerObj.IDRifOrg)
                            , ctx.GetParam("IDTIPOATTO", InnerObj.IDTipoAtto)
                            , ctx.GetParam("DATAATTO", InnerObj.DataAtto)
                            , ctx.GetParam("NATTO", InnerObj.NAtto)
                            , ctx.GetParam("IDRICHIEDENTE", InnerObj.IDRichiedente)
                            , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                            , ctx.GetParam("IDVIA", InnerObj.IDVia)
                            , ctx.GetParam("VIA", InnerObj.Via)
                            , ctx.GetParam("CIVICO", InnerObj.Civico)
                            , ctx.GetParam("DATAINIZIO", InnerObj.DataInizio)
                            , ctx.GetParam("DATAFINE", InnerObj.DataFine)
                            , ctx.GetParam("IDTIPODURATA", InnerObj.IDTipoDurata)
                            , ctx.GetParam("DURATA", InnerObj.Durata)
                            , ctx.GetParam("IDCONSISTENZA", InnerObj.IDConsistenza)
                            , ctx.GetParam("CONSISTENZA", InnerObj.Consistenza)
                            , ctx.GetParam("IDCATEGORIA", InnerObj.IDCategoria)
                            , ctx.GetParam("IDOCCUPAZIONE", InnerObj.IDOccupazione)
                            , ctx.GetParam("ISATTRAZIONE", InnerObj.IsAttrazione)
                            , ctx.GetParam("PERCMAGG", InnerObj.PercMagg)
                            , ctx.GetParam("IMPDETRAZ", InnerObj.ImpDetraz)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichOSAP.Save::errore in inserimento dichiarazione");
                        ctx.Dispose();
                        return false;
                    }
                    else
                    {
                        foreach (GenericCategory myItem in InnerObj.ListAgevolazioni)
                        {
                            sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONIOSAPAGEVOLAZIONI_IU", "ID", "IDDICH", "CODICE");
                            int IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                                    , ctx.GetParam("IDDICH", InnerObj.ID)
                                    , ctx.GetParam("CODICE", myItem.ID)
                                ).First<int>();
                            if (IDRet <= 0)
                            {
                                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichOSAP.Save::errore in inserimento agevolazione dichiarazione");
                                ctx.Dispose();
                                return false;
                            }
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichOSAP.Save::errore::", ex);
                return false;
            }
        }
    }
}