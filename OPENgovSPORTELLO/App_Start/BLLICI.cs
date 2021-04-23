using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe generale di gestione ICI
    /// </summary>
    public class ICI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ICI));
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
        public bool LoadDich(string TypeConsulta, string IDEnte, int IDContribuente, int IDRif, int IDIstanza, out SPC_DichICI myDich)
        {
            myDich = new SPC_DichICI();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = String.Empty;
                    
                    List<object> ListUI = new List<object>();
                    if (TypeConsulta == Istanza.TIPO.ConsultaDich)
                    {
                        if (!LoadICIUIDich(IDEnte, IDContribuente, IDRif, IDIstanza, new SPC_DichICI(), out ListUI))
                        {
                            return false;
                        }
                        else
                        {
                            if (ListUI.Count > 0)
                            {
                                myDich = (SPC_DichICI)ListUI[0];
                                myDich.ListVincoli = LoadICIUICalcoloVincoli(IDEnte, myDich.ID, IDIstanza);
                            }
                            }                    
                        }
                    else if (TypeConsulta == Istanza.TIPO.ConsultaCatasto)
                    {
                        foreach(SPC_DichICI myCat in MySession.Current.ListDichUICatasto)
                        {
                            if (myCat.IDRifOrg == IDRif)
                            {
                                myDich = myCat;
                                break;
                            }
                        }
                    }
                    else if (TypeConsulta.StartsWith(Istanza.TIPO.ConsultaDich))
                    {
                        if (!LoadICIUIDichStorico(IDEnte, IDContribuente, IDRif, new SPC_DichICI(), out ListUI))
                        {
                            return false;
                        }
                        else
                        {
                            if (ListUI.Count > 0)
                                myDich = (SPC_DichICI)ListUI[0];
                        }
                    }
                    else
                    {
                        if (!LoadICIUICalcolo(IDEnte, IDContribuente, IDRif, new SPC_DichICI(), out ListUI))
                        {
                            return false;
                        }
                        else
                        {
                            if (ListUI.Count > 0)
                            {
                                myDich = (SPC_DichICI)ListUI[0];
                                myDich.ListVincoli = LoadICIUICalcoloVincoli(IDEnte, myDich.ID, IDIstanza);
                            }
                        }
                    }
                        ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadDich::errore::", ex);
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
        /// <param name="myTypeObj"></param>
        /// <param name="ListUIDich"></param>
        /// <returns></returns>
        public bool LoadICIUIDich(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, object myTypeObj, out List<object> ListUIDich)
        {
            ListUIDich = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    Log.Debug("OPENgovSPORTELLO.BLL.ICI.prc_GetICI_UI::" + "idente:" + IDEnte.ToString() + " idcontribuente:" + IDContribuente.ToString() + " idriforg:" + IDRif.ToString() + " idistanza:" + IDIstanza.ToString());
                    string sSQL = ctx.GetSQL("prc_GetICI_UI", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA");
                    if (myTypeObj.GetType() == typeof(SPC_DichICI))
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                ).ToList<SPC_DichICI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIUIDich::errore::", ex);
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
        /// <param name="ListUICalcolo"></param>
        /// <returns></returns>
        public bool LoadICIUICalcolo(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListUICalcolo)
        {
            ListUICalcolo = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetICI_UICalcolo", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG");
                    if (myTypeObj.GetType() == typeof(SPC_DichICI))
                    {
                        ListUICalcolo = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<SPC_DichICI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUICalcolo = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
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
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIUICalcolo::errore::", ex);
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
        public bool LoadICIUICatasto(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListUICatasto)
        {
            ListUICatasto = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL  = ctx.GetSQL("prc_GetICI_UICatasto", "IDENTE", "IDCONTRIBUENTE", "IDRIF");
                    if (myTypeObj.GetType() == typeof(SPC_DichICI))
                    {
                        ListUICatasto = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIF", IDRif)
                                ).ToList<SPC_DichICI>()) as IEnumerable<object>).Cast<object>().ToList();
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
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIUICatasto::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDRif"></param>
        /// <param name="IDIstanza"></param>
        /// <returns></returns>
        public List<string> LoadICIUICalcoloVincoli(string IDEnte, int IDRif, int IDIstanza)
        {
            List<string> ListVincoli = new List<string>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    Log.Debug("OPENgovSPORTELLO.BLL.ICI.prc_GetICI_UIVincoli::" + "idente:" + IDEnte.ToString() + " idriforg:" + IDRif.ToString() + " idistanza:" + IDIstanza.ToString());
                    string sSQL = ctx.GetSQL("prc_GetICI_UIVincoli", "IDENTE", "IDRIFORG", "IDISTANZA");
                     ListVincoli = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDRIFORG", IDRif)
                                , ctx.GetParam("IDISTANZA", IDIstanza)
                            ).ToList<string>();
                    ctx.Dispose();
                }
                return ListVincoli;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIUICalcoloVincoli::errore::", ex);
                return new List<string>();
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
        public bool FieldValidator(string TipoIstanza, Istanza myIstanza, SPC_DichICI myDich, DateTime DataInizioOrg, out string ScriptError)
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
                    else {
                        if (TipoIstanza == Istanza.TIPO.Cessazione && (myDich.DataFine == DateTime.MaxValue))
                        {
                            ScriptError += "alert('Compilare la data fine!');";
                            return false;
                        }
                        if ((TipoIstanza == Istanza.TIPO.Inagibilità || TipoIstanza == Istanza.TIPO.ComodatoUsoGratuito) && (myDich.DataInizio == DateTime.MaxValue))
                        {
                            ScriptError += "alert('Compilare data inizio!');";
                            return false;
                        }
                        if (myDich.IDTipologia <= 0)
                        {
                            ScriptError += "alert('Compilare l’utilizzo!');";
                            return false;
                        }
                        if (myDich.Tipologia.IndexOf("rincipale") > 0 && myDich.NUtilizzatori <= 0)
                        {
                            ScriptError += "alert('Compilare il numero utilizzatori!');";
                            return false;
                        }
                        if (myDich.IDCategoria <= 0  && myDich.DescrTipologia.IndexOf("erren") <= 0 && myDich.DescrTipologia.IndexOf("abbricabil") <= 0 && myDich.DescrTipologia.IndexOf("dificabil") <= 0)
                        {
                            ScriptError += "alert('Compilare la categoria!');";
                            return false;
                        }
                        if (myDich.IDClasse == string.Empty && myDich.DescrTipologia.IndexOf("erren")<= 0 && myDich.DescrTipologia.IndexOf("abbricabil") <= 0 && myDich.DescrTipologia.IndexOf("dificabil") <= 0 && !myDich.DescrCat.StartsWith("F"))
                        {
                            ScriptError += "alert('Compilare la classe!');";
                            return false;
                        }
                        if (myDich.RenditaValore <= 0 && !myDich.DescrCat.StartsWith("F"))
                        {
                            ScriptError += "alert('Compilare la rendita/valore!');";
                            return false;
                        }
                        if (myDich.IDPossesso <= 0)
                        {
                            ScriptError += "alert('Compilare il tipo possesso!');";
                            return false;
                        }
                        if (myDich.PercPossesso <= 0)
                        {
                            ScriptError += "alert('Compilare la percentuale di possesso!');";
                            return false;
                        }
                        if (myDich.DataFine <= myDich.DataInizio)
                        {
                            ScriptError += "alert('La data fine deve essere successiva a quella di inizio!');";
                            return false;
                        }
                        if(myDich.DataInizio>DateTime.Now)
                        {
                            ScriptError += "alert('La data inizio non può essere successiva a quella odierna!');";
                            return false;
                        }
                        if (myDich.DataFine> DateTime.Now && myDich.DataFine.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                        {
                            ScriptError += "alert('La data fine non può essere successiva a quella odierna!');";
                            return false;
                        }
                        if (myDich.DataInizio < DataInizioOrg)
                        {
                            ScriptError += "alert('La data inizio deve essere successiva a quella iniziale!');";
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
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.FieldValidator::errore::", ex);
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
        public bool LoadICIUIDichStorico(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListUIDich)
        {
            ListUIDich = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    Log.Debug("OPENgovSPORTELLO.BLL.ICI.prc_GetICI_UIStorico::" + "idente:" + IDEnte.ToString() + " idcontribuente:" + IDContribuente.ToString() + " idriforg:" + IDRif.ToString());
                    string sSQL = ctx.GetSQL("prc_GetICI_UIStorico", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG");
                    if (myTypeObj.GetType() == typeof(SPC_DichICI))
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<SPC_DichICI>()) as IEnumerable<object>).Cast<object>().ToList();
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
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIUIDichStorico::errore::", ex);
                return false;
            }
        }
        #endregion
        #region Ravvedimento
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="IDRif"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="ListUICalcolo"></param>
        /// <returns></returns>
        public bool LoadICIUIRavvedimento(string IDEnte, int IDContribuente, int Anno, int IDRif, object myTypeObj, out List<object> ListUICalcolo)
        {
            ListUICalcolo = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetICI_UIRavvedimento", "IDENTE", "IDCONTRIBUENTE", "ANNO", "IDRIFORG");
                    if (myTypeObj.GetType() == typeof(SPC_DichICI))
                    {
                        ListUICalcolo = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("ANNO",Anno)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<SPC_DichICI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUICalcolo = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("ANNO", Anno)
                                , ctx.GetParam("IDRIFORG", IDRif)
                            ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIUIRavvedimento::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="ListUICalcolo"></param>
        /// <returns></returns>
        public bool LoadICIImpRavvedimento(string IDEnte, int IDContribuente, int Anno, out List<RiepilogoUI> ListUICalcolo)
        {
            ListUICalcolo = new List<RiepilogoUI>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetICI_ImpRavvedimento", "IDENTE", "IDCONTRIBUENTE", "ANNO");
                    ListUICalcolo = ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("ANNO", Anno)
                            ).ToList<RiepilogoUI>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadICIImpRavvedimento::errore::", ex);
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
                    string sSQL = ctx.GetSQL("prc_GetICI_Dovuto", "IDENTE", "IDCONTRIBUENTE");
                    ListDovuto = ctx.ContextDB.Database.SqlQuery<RiepilogoDovuto>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).ToList<RiepilogoDovuto>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.ICI.LoadDovutoVersato::errore::", ex);
                return false;
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione dichiarazione ICI
    /// </summary>
    public class BLLSPC_DichICI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichICI));
        private SPC_DichICI InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichICI(SPC_DichICI myItem)
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
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONIICI_IU", "ID", "IDISTANZA", "IDRIFORG"
                        , "IDVIA", "VIA", "CIVICO", "ESPONENTE", "INTERNO", "SCALA"
                        , "FOGLIO", "NUMERO", "SUBALTERNO", "SEZIONE"
                        , "DATA_INIZIO", "DATA_FINE"
                        , "IDCARATTERISTICA", "IDCATEGORIA", "CODCLASSE", "IDZONA", "CONSISTENZA", "RENDITAVALORE"
                        , "IDTIPOUTILIZZO", "IDTIPOPOSSESSO", "PERCPOSSESSO", "NUMEROUTILIZZATORI", "NUMEROFIGLI", "COLTIVATOREDIRETTO", "STORICO"
                        , "RIDUZIONE", "ESCLUSIONEESENZIONE"
                        , "IDIMMOBILEPERTINENTE");

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
                        , ctx.GetParam("SEZIONE", InnerObj.Sezione)
                        , ctx.GetParam("DATA_INIZIO", InnerObj.DataInizio)
                        , ctx.GetParam("DATA_FINE", InnerObj.DataFine)
                        , ctx.GetParam("IDCARATTERISTICA", InnerObj.IDTipologia)
                        , ctx.GetParam("IDCATEGORIA", InnerObj.IDCategoria)
                        , ctx.GetParam("CODCLASSE", InnerObj.IDClasse)
                        , ctx.GetParam("IDZONA", InnerObj.IDZona)
                        , ctx.GetParam("CONSISTENZA", InnerObj.Consistenza)
                        , ctx.GetParam("RENDITAVALORE", InnerObj.RenditaValore)
                        , ctx.GetParam("IDTIPOUTILIZZO", InnerObj.IDUtilizzo)
                        , ctx.GetParam("IDTIPOPOSSESSO", InnerObj.IDPossesso)
                        , ctx.GetParam("PERCPOSSESSO", InnerObj.PercPossesso)
                        , ctx.GetParam("NUMEROUTILIZZATORI", InnerObj.NUtilizzatori)
                        , ctx.GetParam("NUMEROFIGLI", 0)
                        , ctx.GetParam("COLTIVATOREDIRETTO", false)
                        , ctx.GetParam("STORICO", InnerObj.IsStorico)
                        , ctx.GetParam("RIDUZIONE", 0)
                        , ctx.GetParam("ESCLUSIONEESENZIONE", 0)
                        , ctx.GetParam("IDIMMOBILEPERTINENTE", 0)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.Save::errore in inserimento dichiarazione");
                        return false;
                    }
                    foreach (string myVincolo in InnerObj.ListVincoli)
                    {
                        sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONIICI_VINCOLI_IU", "ID", "IDRIF", "IDVINCOLO");
                        int IDVincolo = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                                , ctx.GetParam("IDRIF", InnerObj.ID)
                                , ctx.GetParam("IDVINCOLO", myVincolo)
                            ).First<int>();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIstanza"></param>
        /// <param name="sScriptDich"></param>
        /// <param name="IDCalcolo"></param>
        /// <returns></returns>
        public bool SaveCalcolo(Istanza itemIstanza, ref string sScriptDich, out int IDCalcolo)
        {
            IDCalcolo = 0;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_UICALCOLOICI_IU", "ID", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA"
                        , "IDVIA", "VIA", "CIVICO", "ESPONENTE", "INTERNO", "SCALA"
                        , "FOGLIO", "NUMERO", "SUBALTERNO", "SEZIONE"
                        , "DATA_INIZIO", "DATA_FINE"
                        , "IDCARATTERISTICA", "IDCATEGORIA", "CODCLASSE", "IDZONA", "CONSISTENZA", "RENDITAVALORE"
                        , "IDTIPOPOSSESSO", "PERCPOSSESSO", "NUMEROUTILIZZATORI", "NUMEROFIGLI", "STORICO"
                        , "RIDUZIONE", "ESCLUSIONEESENZIONE"
                        , "PERTINENZADI_FOGLIO", "PERTINENZADI_NUMERO", "PERTINENZADI_SUBALTERNO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", InnerObj.IDContribuente)
                            , ctx.GetParam("IDRIFORG", InnerObj.IDRifOrg)
                            , ctx.GetParam("IDISTANZA", itemIstanza.IDIstanza)
                            , ctx.GetParam("IDVIA", InnerObj.IDVia)
                            , ctx.GetParam("VIA", InnerObj.Via)
                            , ctx.GetParam("CIVICO", InnerObj.Civico)
                            , ctx.GetParam("ESPONENTE", InnerObj.Esponente)
                            , ctx.GetParam("INTERNO", InnerObj.Interno)
                            , ctx.GetParam("SCALA", InnerObj.Scala)
                            , ctx.GetParam("FOGLIO", InnerObj.Foglio)
                            , ctx.GetParam("NUMERO", InnerObj.Numero)
                            , ctx.GetParam("SUBALTERNO", InnerObj.Sub)
                            , ctx.GetParam("SEZIONE", InnerObj.Sezione)
                            , ctx.GetParam("DATA_INIZIO", InnerObj.DataInizio)
                            , ctx.GetParam("DATA_FINE", InnerObj.DataFine)
                            , ctx.GetParam("IDCARATTERISTICA", InnerObj.IDTipologia)
                            , ctx.GetParam("IDCATEGORIA", InnerObj.IDCategoria)
                            , ctx.GetParam("CODCLASSE", InnerObj.IDClasse)
                            , ctx.GetParam("IDZONA", InnerObj.IDZona)
                            , ctx.GetParam("CONSISTENZA", InnerObj.Consistenza)
                            , ctx.GetParam("RENDITAVALORE", InnerObj.RenditaValore)
                            , ctx.GetParam("IDTIPOPOSSESSO", InnerObj.IDPossesso)
                            , ctx.GetParam("PERCPOSSESSO", InnerObj.PercPossesso)
                            , ctx.GetParam("NUMEROUTILIZZATORI", InnerObj.NUtilizzatori)
                            , ctx.GetParam("NUMEROFIGLI", 0)
                            , ctx.GetParam("STORICO", InnerObj.IsStorico)
                            , ctx.GetParam("RIDUZIONE", InnerObj.PercRiduzione)
                            , ctx.GetParam("ESCLUSIONEESENZIONE", InnerObj.PercEsenzione)
                            , ctx.GetParam("PERTINENZADI_FOGLIO", InnerObj.PertFoglio)
                            , ctx.GetParam("PERTINENZADI_NUMERO", InnerObj.PertNumero)
                            , ctx.GetParam("PERTINENZADI_SUBALTERNO", InnerObj.PertSub)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.SaveCalcolo::errore in inserimento dichiarazione");
                        return false;
                    }
                    foreach(string myVincolo in InnerObj.ListVincoli)
                    {
                        sSQL = ctx.GetSQL("prc_TBLSPC_UICALCOLOICI_VINCOLI_IU", "ID", "IDRIF", "IDVINCOLO");
                        int IDVincolo = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                                , ctx.GetParam("IDRIF", InnerObj.ID)
                                , ctx.GetParam("IDVINCOLO", myVincolo)
                            ).First<int>();
                    }
                    IDCalcolo = InnerObj.ID;
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.SaveCalcolo::errore::", ex);
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
        /// <param name="IDCalcolo"></param>
        /// <returns></returns>
        public bool SaveVariazione(DateTime DalOld, DateTime DalPres, DateTime AlPres, Istanza itemIstanza, SPC_DichICI itemUIOrg, ref string sScriptDich, out int IDCalcolo)
        {
                IDCalcolo =0;
            try
            {
                int x;
                int IDOrg = itemUIOrg.ID;
                //inserisco il periodo digitato
                InnerObj.ID = -1;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich, out IDCalcolo))
                    return false;

                //controllo se devo riaprire
                if (InnerObj.DataFine != DateTime.MaxValue)
                {
                    InnerObj = itemUIOrg;
                    InnerObj.ID = -1;
                    InnerObj.DataInizio = AlPres.AddDays(1);
                    if (!SaveCalcolo(itemIstanza, ref sScriptDich, out x))
                        return false;
                }
                //il periodo cliccato deve essere chiuso al dal-1
                itemUIOrg.ID = IDOrg;
                itemUIOrg.DataInizio = DalOld;
                if (DalPres != itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = DalPres.AddDays(-1);
                }
                else if (DalPres == itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = itemUIOrg.DataInizio;
                }
                InnerObj = itemUIOrg;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich, out x))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.SaveVariazione::errore::", ex);
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
        /// <param name="IDCalcolo"></param>
        /// <returns></returns>
        public bool SaveInagibile(DateTime DalOld, DateTime DalPres, DateTime AlPres, Istanza itemIstanza, SPC_DichICI itemUIOrg, ref string sScriptDich, out int IDCalcolo)
        {
            IDCalcolo = 0;
            try
            {
                int x;
                int IDOrg = itemUIOrg.ID;
                //inserisco il periodo digitato
                InnerObj.ID = -1;
                InnerObj.PercRiduzione = SPC_DichICI.RiduzioneInagibile;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich, out IDCalcolo))
                    return false;
                IDCalcolo = InnerObj.ID;
                //controllo se devo riaprire
                if (InnerObj.DataFine != DateTime.MaxValue)
                {
                    InnerObj = itemUIOrg;
                    InnerObj.ID = -1;
                    InnerObj.DataInizio = AlPres.AddDays(1);
                    InnerObj.PercRiduzione = 0;
                    if (!SaveCalcolo(itemIstanza, ref sScriptDich, out x))
                        return false;
                }
                //il periodo cliccato deve essere chiuso al dal-1
                itemUIOrg.ID = IDOrg;
                itemUIOrg.DataInizio = DalOld;
                if (DalPres != itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = DalPres.AddDays(-1);
                }
                else if (DalPres == itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = itemUIOrg.DataInizio;
                }
                InnerObj = itemUIOrg;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich, out x))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.SaveInagibile::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="DalOld"></param>
        /// <param name="DalPres"></param>
        /// <param name="AlPres"></param>
        /// <param name="IDTipologiaOld"></param>
        /// <param name="itemIstanza"></param>
        /// <param name="itemUIOrg"></param>
        /// <param name="sScriptDich"></param>
        /// <param name="IDCalcolo"></param>
        /// <returns></returns>
        public bool SaveComodatoGratuito(DateTime DalOld, DateTime DalPres, DateTime AlPres, int IDTipologiaOld, Istanza itemIstanza, SPC_DichICI itemUIOrg, ref string sScriptDich, out int IDCalcolo)
        {
            IDCalcolo = 0;
            try
            {
                int x;
                int IDOrg = itemUIOrg.ID;
                //inserisco il periodo digitato
                InnerObj.ID = -1;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich, out IDCalcolo))
                    return false;
                IDCalcolo = InnerObj.ID;
                //controllo se devo riaprire
                if (InnerObj.DataFine != DateTime.MaxValue)
                {
                    InnerObj = itemUIOrg;
                    InnerObj.ID = -1;
                    InnerObj.DataInizio = AlPres.AddDays(1);
                    if (!SaveCalcolo(itemIstanza, ref sScriptDich, out x))
                        return false;
                }
                //il periodo cliccato deve essere chiuso al dal-1
                itemUIOrg.ID = IDOrg;
                itemUIOrg.DataInizio = DalOld;
                if (DalPres != itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = DalPres.AddDays(-1);
                }
                else if (DalPres == itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = itemUIOrg.DataInizio;
                }
                InnerObj = itemUIOrg;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich, out x))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichICI.SaveComodatoGratuito::errore::", ex);
                return false;
            }
        }
     }
    /// <summary>
    /// Classe di generale di gestione TASI
    /// </summary>
    public class TASI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TASI));
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
        public bool LoadDich(string TypeConsulta, string IDEnte, int IDContribuente, int IDRif, int IDIstanza, out SPC_DichTASI myDich)
        {
            myDich = new SPC_DichTASI();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = String.Empty;

                    List<object> ListUI = new List<object>();
                    if (TypeConsulta == Istanza.TIPO.ConsultaDich)
                    {
                        if (!LoadTASIUIDich(IDEnte, IDContribuente, IDRif, IDIstanza, new SPC_DichTASI(), out ListUI))
                        {
                            return false;
                        }
                        else
                        {
                            if (ListUI.Count > 0)
                                myDich = (SPC_DichTASI)ListUI[0];
                        }
                    }
                    else {
                        if (!LoadTASIUICalcolo(IDEnte, IDContribuente, IDRif, new SPC_DichTASI(), out ListUI))
                        {
                            return false;
                        }
                        else
                        {
                            if (ListUI.Count > 0)
                                myDich = (SPC_DichTASI)ListUI[0];
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TASI.LoadDich::errore::", ex);
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
        /// <param name="ListUICalcolo"></param>
        /// <returns></returns>
        public bool LoadTASIUICalcolo(string IDEnte, int IDContribuente, int IDRif, object myTypeObj, out List<object> ListUICalcolo)
        {
            ListUICalcolo = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTASI_UICalcolo", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG");
                    if (myTypeObj.GetType() == typeof(SPC_DichTASI))
                    {
                        ListUICalcolo = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTASI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                ).ToList<SPC_DichTASI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUICalcolo = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
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
                Log.Debug("OPENgovSPORTELLO.BLL.TASI.LoadTASIUICalcolo::errore::", ex);
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
        /// <param name="myTypeObj"></param>
        /// <param name="ListUIDich"></param>
        /// <returns></returns>
        public bool LoadTASIUIDich(string IDEnte, int IDContribuente, int IDRif, int IDIstanza, object myTypeObj, out List<object> ListUIDich)
        {
            ListUIDich = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTASI_UI", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA");
                    if (myTypeObj.GetType() == typeof(SPC_DichTASI))
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTASI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                ).ToList<SPC_DichTASI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListUIDich = ((ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDRIFORG", IDRif)
                                    , ctx.GetParam("IDISTANZA", IDIstanza)
                                ).ToList<RiepilogoUI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TASI.LoadTASIUIDich::errore::", ex);
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
        public bool FieldValidator(string TipoIstanza, Istanza myIstanza, SPC_DichTASI myDich, DateTime DataInizioOrg, out string ScriptError)
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
                    else {
                        if (TipoIstanza == Istanza.TIPO.Cessazione && (myDich.DataFine == DateTime.MaxValue))
                        {
                            ScriptError += "alert('Compilare la data fine!');";
                            return false;
                        }
                        if ((TipoIstanza == Istanza.TIPO.Inagibilità || TipoIstanza == Istanza.TIPO.ComodatoUsoGratuito) && (myDich.DataInizio == DateTime.MaxValue))
                        {
                            ScriptError += "alert('Compilare data inizio!');";
                            return false;
                        }
                        if (myDich.IDTipologia <= 0)
                        {
                            ScriptError += "alert('Compilare l’utilizzo!');";
                            return false;
                        }
                        if (myDich.IDCategoria <= 0)
                        {
                            ScriptError += "alert('Compilare la categoria!');";
                            return false;
                        }
                        if (myDich.RenditaValore <= 0)
                        {
                            ScriptError += "alert('Compilare la rendita/valore!');";
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
                        if (myDich.DataFine > DateTime.Now && myDich.DataFine.ToShortDateString() != DateTime.MaxValue.ToShortDateString())
                        {
                            ScriptError += "alert('La data fine non può essere successiva a quella odierna!');";
                            return false;
                        }
                        if (myDich.DataInizio < DataInizioOrg)
                        {
                            ScriptError += "alert('La data inizio deve essere successiva a quella iniziale!');";
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
                Log.Debug("OPENgovSPORTELLO.BLL.TASI.FieldValidator::errore::", ex);
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
                    string sSQL = ctx.GetSQL("prc_GetTASI_Dovuto", "IDENTE", "IDCONTRIBUENTE");
                    ListDovuto = ctx.ContextDB.Database.SqlQuery<RiepilogoDovuto>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).ToList<RiepilogoDovuto>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.TASI.LoadDovutoVersato::errore::", ex);
                return false;
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe gi gestione dichiarazione TASI
    /// </summary>
    public class BLLSPC_DichTASI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLSPC_DichTASI));
        private SPC_DichTASI InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public BLLSPC_DichTASI(SPC_DichTASI myItem)
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
                    string sSQL = ctx.GetSQL("prc_TBLSPC_DICHIARAZIONITASI_IU", "ID", "IDISTANZA", "IDRIFORG"
                            , "IDVIA", "VIA", "CIVICO", "ESPONENTE", "INTERNO", "SCALA"
                            , "FOGLIO", "NUMERO", "SUBALTERNO", "SEZIONE"
                            , "DATA_INIZIO", "DATA_FINE"
                            , "IDNATURATITOLO", "AGENTRATECONTRATTOAFFITTO", "ESTREMICONTRATTOAFFITTO"
                            , "IDCARATTERISTICA", "IDCATEGORIA", "RENDITAVALORE", "IDTIPOUTILIZZO", "PERCPOSSESSO", "IDAGEVOLAZIONE"
                            , "QUOTACALCOLO"                   
                        );

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
                            , ctx.GetParam("SEZIONE", InnerObj.Sezione)
                            , ctx.GetParam("DATA_INIZIO", InnerObj.DataInizio)
                            , ctx.GetParam("DATA_FINE", InnerObj.DataFine)
                            , ctx.GetParam("IDNATURATITOLO", InnerObj.IDNaturaTitolo)
                            , ctx.GetParam("AGENTRATECONTRATTOAFFITTO", InnerObj.AgEntrateContrattoAffitto)
                            , ctx.GetParam("ESTREMICONTRATTOAFFITTO", InnerObj.EstremiContrattoAffitto)
                            , ctx.GetParam("IDCARATTERISTICA", InnerObj.IDCaratteristica)
                            , ctx.GetParam("IDCATEGORIA", InnerObj.IDCategoria)
                            , ctx.GetParam("RENDITAVALORE", InnerObj.RenditaValore)
                            , ctx.GetParam("IDTIPOUTILIZZO", InnerObj.IDTipologia)
                            , ctx.GetParam("PERCPOSSESSO", InnerObj.PercPossesso)
                            , ctx.GetParam("IDAGEVOLAZIONE", InnerObj.IDAgevolazione)
                            , ctx.GetParam("QUOTACALCOLO",InnerObj.TypeQuotaCalcolo)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTASI.Save::errore in inserimento dichiarazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTASI.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemIstanza"></param>
        /// <param name="sScriptDich"></param>
        /// <returns></returns>
        public bool SaveCalcolo(Istanza itemIstanza, ref string sScriptDich)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_UICALCOLOTASI_IU", "ID", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG", "IDISTANZA"
                            , "IDVIA", "VIA", "CIVICO", "ESPONENTE", "INTERNO", "SCALA"
                            , "FOGLIO", "NUMERO", "SUBALTERNO", "SEZIONE"
                            , "DATA_INIZIO", "DATA_FINE"
                            , "IDNATURATITOLO", "AGENTRATECONTRATTOAFFITTO", "ESTREMICONTRATTOAFFITTO"
                            , "IDCARATTERISTICA", "IDCATEGORIA", "RENDITAVALORE", "IDTIPOUTILIZZO", "PERCPOSSESSO", "IDAGEVOLAZIONE"
                            , "QUOTACALCOLO"
                        );
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", InnerObj.IDContribuente)
                            , ctx.GetParam("IDRIFORG", InnerObj.IDRifOrg)
                            , ctx.GetParam("IDISTANZA", itemIstanza.IDIstanza)
                            , ctx.GetParam("IDVIA", InnerObj.IDVia)
                            , ctx.GetParam("VIA", InnerObj.Via)
                            , ctx.GetParam("CIVICO", InnerObj.Civico)
                            , ctx.GetParam("ESPONENTE", InnerObj.Esponente)
                            , ctx.GetParam("INTERNO", InnerObj.Interno)
                            , ctx.GetParam("SCALA", InnerObj.Scala)
                            , ctx.GetParam("FOGLIO", InnerObj.Foglio)
                            , ctx.GetParam("NUMERO", InnerObj.Numero)
                            , ctx.GetParam("SUBALTERNO", InnerObj.Sub)
                            , ctx.GetParam("SEZIONE", InnerObj.Sezione)
                            , ctx.GetParam("DATA_INIZIO", InnerObj.DataInizio)
                            , ctx.GetParam("DATA_FINE", InnerObj.DataFine)
                            , ctx.GetParam("IDNATURATITOLO", InnerObj.IDNaturaTitolo)
                            , ctx.GetParam("AGENTRATECONTRATTOAFFITTO", InnerObj.AgEntrateContrattoAffitto)
                            , ctx.GetParam("ESTREMICONTRATTOAFFITTO", InnerObj.EstremiContrattoAffitto)
                            , ctx.GetParam("IDCARATTERISTICA", InnerObj.IDCaratteristica)
                            , ctx.GetParam("IDCATEGORIA", InnerObj.IDCategoria)
                            , ctx.GetParam("RENDITAVALORE", InnerObj.RenditaValore)
                            , ctx.GetParam("IDTIPOUTILIZZO", InnerObj.IDTipologia)
                            , ctx.GetParam("PERCPOSSESSO", InnerObj.PercPossesso)
                            , ctx.GetParam("IDAGEVOLAZIONE", InnerObj.IDAgevolazione)
                            , ctx.GetParam("QUOTACALCOLO", InnerObj.TypeQuotaCalcolo.ToString())
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTASI.SaveCalcolo::errore in inserimento dichiarazione");
                        return false;
                    }
                }
                sScriptDich += GetRPTIstanza(itemIstanza, InnerObj);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTASI.SaveCalcolo::errore::", ex);
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
        public bool SaveVariazione(DateTime DalOld, DateTime DalPres, DateTime AlPres, Istanza itemIstanza, SPC_DichTASI itemUIOrg, ref string sScriptDich)
        {
            try
            {
                int IDOrg = itemUIOrg.ID;
                //inserisco il periodo digitato
                InnerObj.ID = -1;
                InnerObj.DataInizio = DalPres;
                InnerObj.DataFine = AlPres;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich))
                    return false;
                //controllo se devo riaprire
                if (InnerObj.DataFine != DateTime.MaxValue)
                {
                    InnerObj = itemUIOrg;
                    InnerObj.ID = -1;
                    InnerObj.DataInizio = AlPres.AddDays(1);
                    if (!SaveCalcolo(itemIstanza, ref sScriptDich))
                        return false;
                }
                //il periodo cliccato deve essere chiuso al dal-1
                itemUIOrg.ID = IDOrg;
                itemUIOrg.DataInizio = DalOld;
                if (DalPres != itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = DalPres.AddDays(-1);
                }
                else if (DalPres == itemUIOrg.DataInizio)
                {
                    itemUIOrg.DataFine = itemUIOrg.DataInizio;
                }
                InnerObj = itemUIOrg;
                if (!SaveCalcolo(itemIstanza, ref sScriptDich))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTASI.SaveVariazione::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        /// <param name="myUI"></param>
        /// <returns></returns>
        public string GetRPTIstanza(Istanza myItem, SPC_DichTASI myUI)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dich_UI'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcaratteristica'><input type='text' class='dich_header' id='dichHcaratteristica' value='Caratteristica'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcaratteristica' value='" + ((myUI.DescrTipologia.ToLower().IndexOf("terren") > 0) ? "1" : (myUI.DescrTipologia.ToLower().IndexOf("fabbricabil") > 0 || myUI.DescrTipologia.ToLower().IndexOf("edificabil") > 0) ? "2" : (myUI.DescrTipologia.ToLower().IndexOf("principal") > 0) ? "5" : "3") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichindirizzo'><input type='text' class='dich_header' id='dichHindirizzo' value='Indirizzo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDindirizzo' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dicdaticat'>";
                sScript += "<div id='dichsezione'><input type='text' class='dich_header' id='dichHsezione' value='Sezione'>";
                sScript += "<input type='text' class='dich_dett' id='dichDsezione' value='" + myUI.Sezione + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfoglio'><input type='text' class='dich_header' id='dichHfoglio' value='Foglio'>";
                sScript += "<input type='text' class='dich_dett' id='dichDfoglio' value='" + myUI.Foglio + "'>";
                sScript += "</div>";
                sScript += "<div id='dichnumero'><input type='text' class='dich_header' id='dichHnumero' value='Particella'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnumero' value='" + myUI.Numero + "'>";
                sScript += "</div>";
                sScript += "<div id='dichsub'><input type='text' class='dich_header' id='dichHsub' value='Subalterno'>";
                sScript += "<input type='text' class='dich_dett' id='dichDsub' value='" + myUI.Sub + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcat'><input type='text' class='dich_header' id='dichHcat' value='Categoria'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcat' value='" + myUI.DescrCategoria + "'>";
                sScript += "</div>";
                sScript += "<div id='dichprot'><input type='text' class='dich_header' id='dichHprot' value='n.Protocollo'><input type='text' class='dich_dett' id='dichDprot' value=''></div>";
                sScript += "<div id='dichanno'><input type='text' class='dich_header' id='dichHanno' value='Anno'>";
                sScript += "<input type='text' class='dich_dett' id='dichDanno' value='" + myUI.DataInizio.Year.ToString() + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichvalore'><input type='text' class='dich_header' id='dichHvalore' value='Valore'>";
                sScript += "<input type='text' class='dich_dett' id='dichDvalore' value='" + myUI.RenditaValore.ToString("#,##0.00") + "'>";
                sScript += "</div>";
                  sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichinizio'><input type='text' class='dich_header' id='dichHinizio' value='Inizio del possesso o variazione imposta'>";
                sScript += "<input type='text' class='dich_dett' id='dichDinizio' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfine'><input type='text' class='dich_header' id='dichHfine' value='Termine del possesso o variazione imposta'>";
                sScript += "<input type='text' class='dich_dett' id='dichDfine' value='" + new FunctionGrd().FormattaDataGrd(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdetraz'><input type='text' class='dich_header' id='dichHdetraz' value='Detrazione per abitazione principale'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdetraz' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichfinelavori'><input type='text' class='dich_header' id='dichHfinelavori' value='Data di ultimazione dei lavori'>";
                sScript += "<input type='text' class='dich_dett' id='dichDfinelavori' value=''>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichacquisto'><input type='text' class='dich_header' id='dichHacquisto' value='Acquisto'>";
                sScript += "<input type='text' id='dichDacquisto' class='dich_dett' value='";
                if (myItem.DescrTipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                    sScript += "X";
                sScript += "'></div>";
                sScript += "<div id='dichcessione'><input type='text' class='dich_header' id='dichHcessione' value='Cessione'>";
                sScript += "<input type='text' id='dichDcessione' class='dich_dett' value='";
                if (myItem.DescrTipoIstanza == Istanza.TIPO.Cessazione)
                    sScript += "X";
                sScript += "'></div>";
                sScript += "<div id='dichagentr'><input type='text' class='dich_header' id='dichHagentr' value=''></div>";
                sScript += "<div id='dichestremi'><input type='text' class='dich_dett' id='dichHestremi' value=''></div>";
                sScript += "</div>";
                sScript += "</div>";
                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.SPC_DichTASI.GetRPTIstanza::errore::", ex);
                return string.Empty;
            }
        }
    }
}