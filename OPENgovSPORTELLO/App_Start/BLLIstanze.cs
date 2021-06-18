using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnagInterface;using Anagrafica.DLL;
using OPENgovSPORTELLO.Models;
using log4net;
using System.Data.SqlClient;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe di gestione Istanze
    /// </summary>
    public class Istanze
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Istanze));

        private Istanza InnerObj { get; set; }
        private string IDUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        /// <param name="UserID"></param>
        public Istanze(Istanza myItem, string UserID)
        {
            InnerObj = myItem;
            IDUser = UserID;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <returns></returns>
        public bool SetIstanzaAnnullaPrec(int IDIstanza)
        {
            bool retVal = false;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_SetIstanzaAnnullaPrec", "IDISTANZA");
                    if (ctx.ContextDB.Database.SqlQuery<int>(sSQL
                        , ctx.GetParam("IDISTANZA", IDIstanza)
                        ).FirstOrDefault<int>() <= 0)
                        retVal = false;
                    else
                        retVal = true;
                    ctx.Dispose();
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.SetIstanzaAnnullaPrec::errore::", ex);
                return false;
            }
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
                    string sSQL = ctx.GetSQL("prc_TBLISTANZE_IU", "ID", "IDENTE", "IDTRIBUTO", "IDCONTRIBUENTE", "IDTYPE"
                        , "DATA_REGISTRAZIONE", "DATA_INVIODICHIARAZIONE", "DATA_PRESAINCARICO", "DATA_RESPINTA", "DATA_VALIDITA", "NOTE", "DATA_PROTOCOLLO", "NUMERO_PROTOCOLLO", "DATA_INTEGRAZIONI"
                        , "IDDELEGATO");
                    InnerObj.IDIstanza = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.IDIstanza)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                            , ctx.GetParam("IDCONTRIBUENTE", InnerObj.IDContribuente)
                            , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                            , ctx.GetParam("DATA_REGISTRAZIONE", InnerObj.DataPresentazione)
                            , ctx.GetParam("DATA_INVIODICHIARAZIONE", InnerObj.DataInvioDichiarazione)
                            , ctx.GetParam("DATA_PRESAINCARICO", InnerObj.DataInCarico)
                            , ctx.GetParam("DATA_RESPINTA", InnerObj.DataRespinta)
                            , ctx.GetParam("DATA_VALIDITA", InnerObj.DataValidata)
                            , ctx.GetParam("NOTE", InnerObj.Note)
                            , ctx.GetParam("DATA_PROTOCOLLO", InnerObj.DataProtocollo)
                            , ctx.GetParam("NUMERO_PROTOCOLLO", InnerObj.NumeroProtocollo)
                            , ctx.GetParam("DATA_INTEGRAZIONI", InnerObj.DataIntegrazioni)
                            , ctx.GetParam("IDDELEGATO", InnerObj.IDDelegato)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.IDIstanza > 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Save.ListMotivazioni");
                        foreach (IstanzaMotivazione myItem in InnerObj.ListMotivazioni)
                        {
                            myItem.IDIstanza = InnerObj.IDIstanza;
                            if (!new IstanzaMotivazioni(myItem).Save())
                                return false;
                            else
                                if (myItem.IDMotivazione <= 0)
                                return false;
                        }
                        Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Save.ListComunicazioni");
                        foreach (IstanzaComunicazione myItem in InnerObj.ListComunicazioni)
                        {
                            if (myItem.Testo != string.Empty)
                            {
                                myItem.IDIstanza = InnerObj.IDIstanza;
                                if (!new IstanzaComunicazioni(myItem, IDUser).Save())
                                    return false;
                                else
                                    if (myItem.IDComunicazione <= 0)
                                    return false;
                            }
                        }
                        Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Save.ListAllegati");
                        foreach (IstanzaAllegato myItem in InnerObj.ListAllegati)
                        {
                            if (myItem.PostedFile != null)
                            {
                                myItem.IDIstanza = InnerObj.IDIstanza;
                                if (!new IstanzaAllegati(myItem).Save(InnerObj.IDEnte))
                                    return false;
                                else
                                {
                                    if (myItem.IDAllegato <= 0)
                                        return false;
                                }
                            }
                        }
                    }
                    else {
                        Log.Debug("OPENgovSPORTELLO.Models.Istanza.Save::errore in inserimento istanza");
                        return false;
                    }
                }
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Save.Esco");
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.Istanza.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="anno"></param>
        /// <param name="numeroProtocollo"></param>
        /// <param name="prefissoProtocollo"></param>
        /// <param name="suffissoProtocollo"></param>
        /// <param name="dataProtocollo"></param>
        /// <param name="operatore"></param>
        /// <returns></returns>
        public bool SetProtocollo(string IDEnte, int anno, int numeroProtocollo, string prefissoProtocollo, string suffissoProtocollo, DateTime dataProtocollo, string operatore)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLPROTOCOLLO_IU", "ID", "IDENTE"
                        , "ANNO", "NUMERO_PROTOCOLLO", "PREFISSO_PROTOCOLLO", "SUFFISSO_PROTOCOLLO", "DATA_PROTOCOLLO", "OPERATORE");
                    int idRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("ANNO", anno)
                            , ctx.GetParam("NUMERO_PROTOCOLLO", numeroProtocollo)
                            , ctx.GetParam("PREFISSO_PROTOCOLLO", prefissoProtocollo)
                            , ctx.GetParam("SUFFISSO_PROTOCOLLO", suffissoProtocollo)
                            , ctx.GetParam("DATA_PROTOCOLLO", dataProtocollo)
                            , ctx.GetParam("OPERATORE", operatore)
                        ).First<int>();
                    ctx.Dispose();
                    if (idRet <= 0)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.SetProtocollo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public int GetProtocollo(string IDEnte, int anno)
        {
            try
            {
                int idRet = 0;
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetProtocollo", "IDENTE"
                        , "ANNO", "ISNEXT");
                    idRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("ANNO", anno)
                            , ctx.GetParam("ISNEXT", true)
                        ).First<int>();
                    ctx.Dispose();
                }
                return idRet;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.GetProtocollo::errore::", ex);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListIstanze"></param>
        /// <param name="myRiepDeb"></param>
        /// <param name="ListComunicazioni"></param>
        /// <returns></returns>
        public bool LoadRiepilogoDovuto(string IDEnte, int IDContribuente, out List<Istanza> ListIstanze, out RiepilogoDebito myRiepDeb, out List<GenericCategory> ListComunicazioni)
        {
            Log.Debug("LoadRiepilogoDovuto::IDEnte_"+IDEnte.ToString()+" IDContribuente_"+IDContribuente.ToString());
            ListIstanze = new List<Istanza>();
            myRiepDeb = new RiepilogoDebito();
            ListComunicazioni = new List<GenericCategory>();
            try
            {
                                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetRiepilogoDovuto", "IDENTE"
                        , "IDCONTRIBUENTE");
                    myRiepDeb = ctx.ContextDB.Database.SqlQuery<RiepilogoDebito>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).FirstOrDefault<RiepilogoDebito>();
                    ctx.Dispose();
                }

                if (!new Profilo().LoadNews(IDEnte, IDContribuente, out ListComunicazioni))
                    return false;

                return new Dichiarazioni(InnerObj).LoadDichiarazione(IDEnte, IDContribuente, -1, out ListIstanze);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadRiepilogoDovuto::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUI"></param>
        /// <param name="ListDich"></param>
        /// <param name="ListCat"></param>
        /// <param name="ListUIRidEse"></param>
        /// <param name="ListDichRidEse"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadTARSURiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUI, out List<RiepilogoUI> ListDich, out List<CategorieTARSU> ListCat, out List<RidEseTARSU> ListUIRidEse, out List<RidEseTARSU> ListDichRidEse, out List<RiepilogoDovuto> ListDovuto)
        {
            ListUI = new List<RiepilogoUI>(); 
            ListDich = new List<RiepilogoUI>();

            ListCat = new List<CategorieTARSU>();
            ListUIRidEse = new List<RidEseTARSU>();

            ListDichRidEse = new List<RidEseTARSU>();
            ListDovuto = new List<RiepilogoDovuto>();

            List<object> ListObj = new List<object>();

            try
            {
                if (!new TARSU().LoadTARSUUIDich(IDEnte, IDContribuente, -1, -1, "I", new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else 
                {
                    foreach (object myObj in ListObj)
                    {
                        ListUI.Add((RiepilogoUI)myObj);
                    }
                }

                if (!new TARSU().LoadTARSUUIDich(IDEnte, IDContribuente, -1, -1, "D", new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else
                {
                    foreach (object myObj in ListObj)
                    {
                        ListDich.Add((RiepilogoUI)myObj);
                    }
                }

                if (!new TARSU().LoadTARSURidEse(IDEnte, IDContribuente, -1, -1, "I", string.Empty, new RidEseTARSU(), out ListObj))
                {
                    return false;
                }
                else
                {
                    foreach (object myObj in ListObj)
                    {
                        ListUIRidEse.Add((RidEseTARSU)myObj);
                    }
                }

                if (!new TARSU().LoadTARSURidEse(IDEnte, IDContribuente, -1, -1, "D", string.Empty, new RidEseTARSU(), out ListObj))
                {
                    return false;
                }
                else
                {
                    foreach (object myObj in ListObj)
                    {
                        ListDichRidEse.Add((RidEseTARSU)myObj);
                    }
                }

                if (!new TARSU().LoadDovutoVersato(IDEnte, IDContribuente, out ListDovuto))
                {
                    return false;
                }
                    
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadTARSURiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUICalcolo"></param>
        /// <param name="ListUIDich"></param>
        /// <param name="ListUICatasto"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadICIRiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUICalcolo, out List<RiepilogoUI> ListUIDich, out List<RiepilogoUI> ListUICatasto, out List<RiepilogoDovuto> ListDovuto)
        {
            ListUICalcolo = new List<RiepilogoUI>(); ListUIDich = new List<RiepilogoUI>(); ListUICatasto = new List<RiepilogoUI>();
            ListDovuto = new List<RiepilogoDovuto>();
            List<object> ListObj = new List<object>();
            try
            {
                    if (!new ICI().LoadICIUICalcolo(IDEnte, IDContribuente, -1, new RiepilogoUI(), out ListObj))
                    {
                        return false;
                    }
                    else
                    {
                        foreach (object myObj in ListObj)
                        {
                            ListUICalcolo.Add((RiepilogoUI)myObj);
                        }
                    }
                    if (!new ICI().LoadICIUIDich(IDEnte, IDContribuente, -1, -1, new RiepilogoUI(), out ListObj))
                    {
                        return false;
                    }
                    else
                    {
                        foreach (object myObj in ListObj)
                        {
                            ListUIDich.Add((RiepilogoUI)myObj);
                        }
                    }
                    if (!new ICI().LoadICIUICatasto(IDEnte, IDContribuente, -1, new RiepilogoUI(), out ListObj))
                    {
                        return false;
                    }
                    else
                    {
                        foreach (object myObj in ListObj)
                        {
                            ListUICatasto.Add((RiepilogoUI)myObj);
                        }
                    }
                    if (!new ICI().LoadDovutoVersato(IDEnte, IDContribuente, out ListDovuto))
                        return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadICIRiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUICalcolo"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadTASIRiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUICalcolo, out List<RiepilogoDovuto> ListDovuto)
        {
            ListUICalcolo = new List<RiepilogoUI>();
            ListDovuto = new List<RiepilogoDovuto>();
            List<object> ListObj = new List<object>();
            try
            {
                    if (!new TASI().LoadTASIUICalcolo(IDEnte, IDContribuente, -1, new RiepilogoUI(), out ListObj))
                    {
                        return false;
                    }
                    else
                    {
                        foreach (object myObj in ListObj)
                        {
                            ListUICalcolo.Add((RiepilogoUI)myObj);
                        }
                    }
                    if (!new TASI().LoadDovutoVersato(IDEnte, IDContribuente, out ListDovuto))
                        return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadTASIRiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUI"></param>
        /// <param name="ListDich"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadOSAPRiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUI, out List<RiepilogoUI> ListDich, out List<RiepilogoDovuto> ListDovuto)
        {
            ListUI = new List<RiepilogoUI>(); ListDich = new List<RiepilogoUI>();
            ListDovuto = new List<RiepilogoDovuto>();
            List<object> ListObj = new List<object>();
            try
            {
                if (!new OSAP().LoadOSAPUIDich(IDEnte, IDContribuente, -1, -1, "I", new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else {
                    foreach (object myObj in ListObj)
                    {
                        ListUI.Add((RiepilogoUI)myObj);
                    }
                }
                if (!new OSAP().LoadOSAPUIDich(IDEnte, IDContribuente, -1, -1, "D", new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else {
                    foreach (object myObj in ListObj)
                    {
                        ListDich.Add((RiepilogoUI)myObj);
                    }
                }
                if (!new OSAP().LoadDovutoVersato(IDEnte, IDContribuente, out ListDovuto))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadOSAPRiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUI"></param>
        /// <param name="ListDich"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadICPRiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUI, out List<RiepilogoUI> ListDich, out List<RiepilogoDovuto> ListDovuto)
        {
            ListUI = new List<RiepilogoUI>(); ListDich = new List<RiepilogoUI>();
            ListDovuto = new List<RiepilogoDovuto>();
            List<object> ListObj = new List<object>();
            try
            {
                if (!new ICP().LoadICPUIDich(IDEnte, IDContribuente, -1, -1, "I", new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else {
                    foreach (object myObj in ListObj)
                    {
                        ListUI.Add((RiepilogoUI)myObj);
                    }
                }
                if (!new ICP().LoadICPUIDich(IDEnte, IDContribuente, -1, -1, "D", new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else {
                    foreach (object myObj in ListObj)
                    {
                        ListDich.Add((RiepilogoUI)myObj);
                    }
                }
                if (!new ICP().LoadDovutoVersato(IDEnte, IDContribuente, out ListDovuto))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadICPRiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUI"></param>
        /// <param name="ListDovuto"></param>
        /// <returns></returns>
        public bool LoadTESSERERiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUI, out List<RiepilogoDovuto> ListDovuto)
        {
            ListUI = new List<RiepilogoUI>(); 
            ListDovuto = new List<RiepilogoDovuto>();
            List<object> ListObj = new List<object>();
            try
            {
                if (!new TESSERE().LoadTESSEREUIDich(IDEnte, IDContribuente, -1, new RiepilogoUI(), out ListObj))
                {
                    return false;
                }
                else {
                    foreach (object myObj in ListObj)
                    {
                        ListUI.Add((RiepilogoUI)myObj);
                    }
                }
                if (!new TARSU().LoadDovutoVersato(IDEnte, IDContribuente, out ListDovuto))
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadTESSERERiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListDich"></param>
        /// <returns></returns>
        public bool LoadProvvedimentiRiepilogo(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListDich)
        {
            ListDich = new List<RiepilogoUI>();
            try
            {
                if (!new PROVVEDIMENTI().LoadProvvedimenti(IDEnte, IDContribuente, -1, out ListDich))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadProvvedimentiRiepilogo::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListUI"></param>
        /// <param name="imgStato"></param>
        public void LoadBaseStato(List<RiepilogoUI> ListUI, System.Web.UI.HtmlControls.HtmlInputText imgStato)
        {
            try
            {
                bool hasKO, hasMoreOrLess;
                hasKO = hasMoreOrLess = false;
                foreach (RiepilogoUI myItem in ListUI)
                {
                    if (myItem.Stato == "KO")
                        hasKO = true;
                    else if (myItem.Stato == "ML" || myItem.Stato == "WA")
                        hasMoreOrLess = true;
                }
                if (!hasKO && !hasMoreOrLess)
                    imgStato.Attributes.Add("class", "div_StatoOK");
                else if (hasMoreOrLess)
                    imgStato.Attributes.Add("class", "div_StatoML");
                else
                    imgStato.Attributes.Add("class", "div_StatoKO");

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadBaseStato::errore::", ex);
                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="DataPresentazione"></param>
        /// <param name="IDTributo"></param>
        /// <param name="IDStatoIstanze"></param>
        /// <param name="Nominativo"></param>
        /// <param name="CFPIVA"></param>
        /// <param name="IDIstanza"></param>
        /// <param name="IDDichiarazione"></param>
        /// <param name="OnlyGest"></param>
        /// <param name="ListIstanze"></param>
        /// <returns></returns>
        public bool LoadIstanze(string IDEnte, int IDContribuente, DateTime DataPresentazione, string IDTributo, string IDStatoIstanze, string Nominativo, string CFPIVA, int IDIstanza, int IDDichiarazione, bool OnlyGest, out List<Istanza> ListIstanze)
        {
            ListIstanze = new List<Istanza>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetIstanze", "IDENTE"
                        , "IDCONTRIBUENTE", "DATAPRESENTAZIONE", "IDTRIBUTO", "IDSTATOISTANZE", "IDISTANZA", "IDDICHIARAZIONE", "TOGEST"
                        , "NOMINATIVO", "CFPIVA");
                    ListIstanze = ctx.ContextDB.Database.SqlQuery<Istanza>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                            , ctx.GetParam("DATAPRESENTAZIONE", DataPresentazione.Date)
                            , ctx.GetParam("IDTRIBUTO", IDTributo)
                            , ctx.GetParam("IDSTATOISTANZE", IDStatoIstanze)
                            , ctx.GetParam("IDISTANZA", IDIstanza)
                            , ctx.GetParam("IDDICHIARAZIONE", IDDichiarazione)
                            , ctx.GetParam("TOGEST", (!OnlyGest?0:1))
                            , ctx.GetParam("NOMINATIVO", Nominativo)
                            , ctx.GetParam("CFPIVA", CFPIVA)
                        ).ToList<Istanza>();
                    ctx.Dispose();
                    Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadIstanze.query->prc_GetIstanze @IDENTE=" + IDEnte + ", @IDCONTRIBUENTE=" + IDContribuente.ToString() + ", @DATAPRESENTAZIONE=" + DataPresentazione.Date.ToString() + ", @IDTRIBUTO=" + IDTributo + ", @IDSTATOISTANZE=" + IDStatoIstanze + ", @IDISTANZA=" + IDIstanza.ToString() + ", @IDDICHIARAZIONE=" + IDDichiarazione.ToString() + ", @TOGEST=" + (!OnlyGest ? 0 : 1).ToString() + ", @NOMINATIVO=" + Nominativo + ", @CFPIVA=" + CFPIVA);
                    foreach (Istanza myItem in ListIstanze)
                    {
                        myItem.ListMotivazioni = new IstanzaMotivazioni(new IstanzaMotivazione()).LoadMotivazioni(myItem.IDIstanza);
                        myItem.ListAllegati = new IstanzaAllegati(new IstanzaAllegato()).LoadAllegati(myItem.IDIstanza, -1);
                    }
                    if (IDIstanza > 0)
                    {
                        foreach (Istanza myItem in ListIstanze)
                        {
                            myItem.ListComunicazioni = new IstanzaComunicazioni(new IstanzaComunicazione(), IDUser).LoadStatiComunicazioni(myItem.IDIstanza);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadIstanze::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDTributo"></param>
        /// <param name="Anno"></param>
        /// <param name="Tipo"></param>
        /// <param name="ListMyData"></param>
        /// <returns></returns>
        public bool LoadF24(string IDEnte, int IDContribuente, string IDTributo, int Anno, string Tipo, out List<DatiF24> ListMyData)
        {
            ListMyData = new List<DatiF24>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetF24", "IDENTE", "IDCONTRIBUENTE", "IDTRIBUTO", "ANNO", "TIPO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<DatiF24>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("IDTRIBUTO", IDTributo)
                                , ctx.GetParam("ANNO", Anno)
                                , ctx.GetParam("TIPO", Tipo)
                            ).ToList<DatiF24>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadF24::errore::", ex);
                return false;
            }
        }
        #region Ravvedimento
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="Anno"></param>
        /// <param name="ListUICalcolo"></param>
        /// <returns></returns>
        public bool LoadICIRavvedimentoUI(string IDEnte, int IDContribuente, int Anno, out List<RiepilogoUI> ListUICalcolo)
        {
            ListUICalcolo = new List<RiepilogoUI>();
            List<object> ListObj = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    if (!new ICI().LoadICIUIRavvedimento(IDEnte, IDContribuente, Anno, -1, new RiepilogoUI(), out ListObj))
                    {
                        return false;
                    }
                    else
                    {
                        foreach (object myObj in ListObj)
                        {
                            ListUICalcolo.Add((RiepilogoUI)myObj);
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadICIRavvedimentoUI::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="Anno"></param>
        /// <returns></returns>
        public List<RiepilogoUI> LoadICIRavvedimentoImporti(string IDEnte, int IDContribuente, int Anno)
        {
            List<RiepilogoUI> ListMyData = new List<RiepilogoUI>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    if (!new ICI().LoadICIImpRavvedimento(IDEnte, IDContribuente, Anno, out ListMyData))
                    {
                        return new List<RiepilogoUI>();
                    }
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadICIRavvedimentoImporti::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDTributo"></param>
        /// <param name="Anno"></param>
        /// <param name="Tipo"></param>
        /// <param name="ListDati"></param>
        /// <returns></returns>
        public bool LoadF24Ravvedimento(string IDEnte, int IDContribuente, string IDTributo, int Anno, string Tipo, out List<DatiF24> ListDati)
        {
            ListDati = new List<DatiF24>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetF24Ravvedimento", "IDENTE", "IDCONTRIBUENTE", "IDTRIBUTO", "ANNO", "TIPO");
                    ListDati = ctx.ContextDB.Database.SqlQuery<DatiF24>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("IDTRIBUTO", IDTributo)
                                , ctx.GetParam("ANNO", Anno)
                                , ctx.GetParam("TIPO", Tipo)
                            ).ToList<DatiF24>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadF24Ravvedimento::errore::", ex);
                return false;
            }
        }
        #endregion
        #region "Caricamento dati in videata gestione istanza"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sScript"></param>
        /// <param name="GrdStatiComunicazioni"></param>
        /// <param name="GrdAttachments"></param>
        public void LoadFormData(out string sScript, ref Ribes.OPENgov.WebControls.RibesGridView GrdStatiComunicazioni, ref Ribes.OPENgov.WebControls.RibesGridView GrdAttachments)
        {
            try
            {
                sScript = string.Empty;

                

                // BD 07/05/2021 Ordinato per data dal piu' recente
                GrdStatiComunicazioni.DataSource = InnerObj.ListComunicazioni.OrderByDescending(o => o.Data).ToList();
                // BD 07/05/2021 Ordinato per data dal piu' recente

                GrdStatiComunicazioni.DataBind();

                GrdAttachments.DataSource = InnerObj.ListAllegati;
                GrdAttachments.DataBind();

                if (InnerObj.IDDelegato > 0)
                {
                    sScript += "document.getElementById('lblDelegIstanza').innerText='Presentata dal delegato " + InnerObj.NominativoDelegato.Replace("'", "’") + "';";
                }
                else
                {
                    sScript += "$('#lblDelegIstanza').hide();";
                }

                sScript += "document.getElementById('lblEnte').innerText='Ente: " + InnerObj.DescrEnte.Replace("'", "’") + "';";
                sScript += "document.getElementById('lblTipoIstanza').innerText='Tipo: " + (InnerObj.NDichiarazione > 0 ? "N." + InnerObj.NDichiarazione.ToString() + " " : string.Empty) + InnerObj.DescrTipo + "';";
                sScript += "document.getElementById('lblDescrTributo').innerText='Tributo: " + InnerObj.DescrTributo + "';";
                sScript += "document.getElementById('lblDataPresentazione').innerText='Data Registrazione: " + new FunctionGrd().FormattaDataGrd(InnerObj.DataPresentazione) + "';";
                sScript += "document.getElementById('lblStatoIstanza').innerText='Stato: " + InnerObj.DescrStato + "';";
                sScript += "document.getElementById('lblMotivi').innerText = 'Motivazione: ';";
                string sMotivi = string.Empty;
                sMotivi += InnerObj.Note + " ";
                foreach (IstanzaMotivazione myMotivazione in InnerObj.ListMotivazioni)
                {
                    if (sMotivi.Trim() != string.Empty)
                        sMotivi += ", ";
                    sMotivi += myMotivazione.Note.Replace("&nbsp;", " ").Replace("\r\n", " ").Replace("'", "’");
                }
                sScript += "document.getElementById('lblMotivi').innerText+='" + sMotivi + "';";
                if (MySession.Current.Scope == "BO")
                {
                    Log.Debug("Istanze > Consultazione Istanze:: Controllo MySession.Current.Scope == 'BO'");
                    if ((InnerObj.DataValidata.ToShortDateString() != DateTime.MaxValue.ToShortDateString() && InnerObj.DataValidata.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                        || (InnerObj.DataRespinta.ToShortDateString() != DateTime.MaxValue.ToShortDateString() && InnerObj.DataRespinta.ToShortDateString() != DateTime.MinValue.ToShortDateString()))
                    {
                        sScript += "$('.BottoneStop').hide();$('.BottoneAccept').hide();";
                        sScript += "$('p#Respingi').hide();$('p#Valida').hide();";
                    }
                    else if (InnerObj.DataInCarico.ToShortDateString() != DateTime.MaxValue.ToShortDateString() && InnerObj.DataInCarico.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                    {
                        sScript += "$('.BottoneStop').show();$('.BottoneAccept').show();";
                        sScript += "$('p#Respingi').show();$('p#Valida').show();";
                    }
                    else if (InnerObj.DataProtocollo.ToShortDateString() != DateTime.MaxValue.ToShortDateString() && InnerObj.DataProtocollo.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                    {
                        Log.Debug("Faccio lo show di Bottonework");
                        sScript += "$('.BottoneWork').show();";
                        sScript += "$('p#InCarico').show();";
                    }
                    else
                    {
                        sScript += "$('.BottoneSort').hide();$('p#Ribalta').hide();";
                        if (MySettings.GetConfig("TypeProtocollo") == "S")
                        {
                            sScript += "$('.BottoneCounter').show();";
                            sScript += "$('p#Protocolla').show();";
                        }
                        else
                        {
                            if (MySettings.GetConfig("TypeProtocollo") == "E")
                            {
                                if (InnerObj.DataAccettazione.ToShortDateString() != DateTime.MaxValue.ToShortDateString() && InnerObj.DataAccettazione.ToShortDateString() != DateTime.MinValue.ToShortDateString())
                                {
                                    Log.Debug("Faccio lo show di Bottonework");
                                    sScript += "$('.BottoneWork').show();";
                                    sScript += "$('p#InCarico').show();";
                                }
                                else {
                                    Log.Debug("Faccio l'hide di Bottonework 1");
                                    sScript += "$('.BottoneWork').hide();";
                                    sScript += "$('p#InCarico').hide();";
                                }
                            }
                            else {
                                Log.Debug("Faccio l'hide di Bottonework 2");
                                sScript += "$('.BottoneWork').show();";
                                sScript += "$('p#InCarico').show();";
                            }
                        }
                    }
                    if (InnerObj.DescrTipo.ToUpper() == "PAGAMENTO")
                    {
                        sScript += "$('.BottoneWork').hide();$('p#InCarico').hide();";
                        sScript += "$('.BottoneStop').hide();$('p#Respingi').hide();";
                        sScript += "$('.BottoneAccept').hide();$('p#Valida').hide();";
                        sScript += "$('.BottoneCounter').hide();$('p#Protocolla').hide();";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadFormData::errore::", ex);
                throw ex;
            }
        }
        #endregion
        #region "Storico"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListUIDich"></param>
        /// <returns></returns>
        public bool LoadICIRiepilogoStorico(string IDEnte, int IDContribuente, out List<RiepilogoUI> ListUIDich)
        {
            ListUIDich = new List<RiepilogoUI>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    List<object> ListObj = new List<object>();
                    if (!new ICI().LoadICIUIDichStorico(IDEnte, IDContribuente, -1, new RiepilogoUI(), out ListObj))
                    {
                        return false;
                    }
                    else
                    {
                        foreach (object myObj in ListObj)
                        {
                            ListUIDich.Add((RiepilogoUI)myObj);
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadICIRiepilogoStorico::errore::", ex);
                return false;
            }
        }
        #endregion
        #region "Azioni su Istanze"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFiles"></param>
        /// <param name="NoteAggiuntive"></param>
        /// <param name="DestMail"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool Protocolla(HttpFileCollection ListFiles, string NoteAggiuntive, string DestMail, out string sErr)
        {
            sErr = string.Empty;
            List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
            List<IstanzaAllegato> ListDichAttach = new List<IstanzaAllegato>();
            try
            {
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Comunicazione,  ref ListMailAttachments, ref ListDichAttach))
                {
                    sErr = "Errore in lettura allegati!;";
                    return false;
                }
                else {
                    foreach (IstanzaAllegato myAll in ListDichAttach)
                    {
                        InnerObj.ListAllegati.Add(myAll);
                    }
                    int numeroProtocollo = GetProtocollo(InnerObj.IDEnte, InnerObj.DataPresentazione.Year);
                    if (numeroProtocollo > 0)
                    {
                        InnerObj.DataProtocollo = DateTime.Now;
                        InnerObj.NumeroProtocollo = numeroProtocollo;
                        if (NoteAggiuntive != string.Empty || ListDichAttach.Count > 0)
                            InnerObj.ListComunicazioni.Add(new IstanzaComunicazione() { IDIstanza = InnerObj.IDIstanza, IDTipo = Istanza.STATO.Protocollata, Data = DateTime.Now, DataLettura = DateTime.Now, Testo = NoteAggiuntive, ListAllegati = ListDichAttach });

                        if (Save() == true)
                        {
                            SetProtocollo(InnerObj.IDEnte, InnerObj.DataPresentazione.Year, numeroProtocollo, "", "", InnerObj.DataInCarico, MySession.Current.UserLogged.NameUser);
                            string TestoMail = "Buongiorno " + InnerObj.Nominativo + ",";
                            TestoMail += "\nper quanto riguarda l'istanza di " + InnerObj.DescrTipoIstanza + " " + InnerObj.DescrTributo.ToUpper();
                            TestoMail += " presentata in data " + new FunctionGrd().FormattaDataGrd(InnerObj.DataPresentazione);
                            try
                            {
                                if (InnerObj.ListDatiDich != null)
                                {
                                    if (InnerObj.IDTributo == General.TRIBUTO.ICI)
                                    {
                                        TestoMail += " sui riferimenti catastali:\n";
                                        TestoMail += " Foglio " + ((SPC_DichICI)InnerObj.ListDatiDich).Foglio;
                                        TestoMail += " Particella " + ((SPC_DichICI)InnerObj.ListDatiDich).Numero;
                                        TestoMail += " Subalterno " + ((SPC_DichICI)InnerObj.ListDatiDich).Sub;
                                    }
                                    else if (InnerObj.IDTributo == General.TRIBUTO.TARSU)
                                    {
                                        TestoMail += " sui riferimenti catastali:\n";
                                        TestoMail += " Foglio " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Foglio;
                                        TestoMail += " Particella " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Numero;
                                        TestoMail += " Subalterno " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Sub;
                                    }
                                    else if (InnerObj.IDTributo == General.TRIBUTO.OSAP)
                                    {
                                        TestoMail += " per l'indirizzo:\n" + ((SPC_DichOSAP)InnerObj.ListDatiDich).Ubicazione;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Protocolla->oggetto dei riferimenti non trovato::errore::", ex);
                            }
                            TestoMail += ", il numero protocollo assegnato è:" + numeroProtocollo.ToString() + " del " + new FunctionGrd().FormattaDataGrd(InnerObj.DataProtocollo) + ".";
                            if (NoteAggiuntive != string.Empty)
                                TestoMail += "\n" + NoteAggiuntive;
                            TestoMail += "\nCordiali Saluti.";
                            new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { DestMail }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello - Protocollo Istanza", TestoMail, ListMailAttachments, out sErr);
                            if (sErr != string.Empty)
                                return false;
                            else
                            {
                                new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Protocolla", "effettuata protocollazione", "", "", "");
                                return true;
                            }
                        }
                        else
                        {
                            sErr = "Errore in Protocollazione!;";
                            return false;
                        }
                    }
                    else
                    {
                        sErr = "Errore in Assegnazione Numero Protocollo!;";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Protocolla::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFiles"></param>
        /// <param name="NoteAggiuntive"></param>
        /// <param name="DestMail"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool InCarico(HttpFileCollection ListFiles, string NoteAggiuntive, string DestMail, out string sErr)
        {
            sErr = string.Empty;
            List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
            List<IstanzaAllegato> ListDichAttach = new List<IstanzaAllegato>();
            try
            {
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Comunicazione,  ref ListMailAttachments, ref ListDichAttach))
                {
                    sErr = "Errore in lettura allegati!;";
                    return false;
                }
                else {
                    foreach (IstanzaAllegato myAll in ListDichAttach)
                    {
                        InnerObj.ListAllegati.Add(myAll);
                    }
                    InnerObj.DataInCarico = DateTime.Now;
                    if (NoteAggiuntive != string.Empty || ListDichAttach.Count > 0)
                        InnerObj.ListComunicazioni.Add(new IstanzaComunicazione() { IDIstanza = InnerObj.IDIstanza, IDTipo = Istanza.STATO.Validata, Data = DateTime.Now, DataLettura = DateTime.Now, Testo = NoteAggiuntive, ListAllegati = ListDichAttach });

                    if (MySettings.GetConfig("TypeProtocollo") == "A")
                    {
                        if (!Protocolla(null, string.Empty, DestMail, out sErr))
                        {
                            sErr = "Errore in Protocollazione!" + sErr;
                            return false;
                        }
                    }
                    if (Save() == true)
                    {
                        string TestoMail = "Buongiorno " + InnerObj.Nominativo + ",";
                        TestoMail += "\nper quanto riguarda l'istanza di " + InnerObj.DescrTipoIstanza + " " + InnerObj.DescrTributo.ToUpper();
                        TestoMail += " presentata in data " + new FunctionGrd().FormattaDataGrd(InnerObj.DataPresentazione);
                        try
                        {
                            if (InnerObj.ListDatiDich != null)
                            {
                                if (InnerObj.IDTributo == General.TRIBUTO.ICI)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichICI)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichICI)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichICI)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.TARSU)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.OSAP)
                                {
                                    TestoMail += " per l'indirizzo:\n" + ((SPC_DichOSAP)InnerObj.ListDatiDich).Ubicazione;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Istanze.InCarico->oggetto dei riferimenti non trovato::errore::", ex);
                        }
                        TestoMail += ", la richiesta è stata presa in carico.";
                        if (NoteAggiuntive != string.Empty)
                            TestoMail += "\n" + NoteAggiuntive;
                        TestoMail += "\nCordiali Saluti.";
                        new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { DestMail }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello - Istanza presa in carico", TestoMail, ListMailAttachments, out sErr);
                        if (sErr != string.Empty)
                            return false;
                        else
                        {
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Work", "Istanza presa in carico", "", "", "");
                            return true;
                        }
                    }
                    else
                    {
                        sErr = "Errore in salvataggio presa in carico!;";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.InCarico::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFiles"></param>
        /// <param name="NoteAggiuntive"></param>
        /// <param name="DestMail"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool Valida(HttpFileCollection ListFiles, string NoteAggiuntive, string DestMail, out string sErr)
        {
            sErr = string.Empty;
            List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
            List<IstanzaAllegato> ListDichAttach = new List<IstanzaAllegato>();
            try
            {
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Comunicazione,  ref ListMailAttachments, ref ListDichAttach))
                {
                    sErr = "Errore in lettura allegati!;";
                    return false;
                }
                else {
                    foreach (IstanzaAllegato myAll in ListDichAttach)
                    {
                        InnerObj.ListAllegati.Add(myAll);
                    }
                    InnerObj.DataValidata = DateTime.Now;
                    if (NoteAggiuntive != string.Empty || ListDichAttach.Count > 0)
                        InnerObj.ListComunicazioni.Add(new IstanzaComunicazione() { IDIstanza = InnerObj.IDIstanza, IDTipo = Istanza.STATO.Validata, Data = DateTime.Now, DataLettura = DateTime.Now, Testo = NoteAggiuntive, ListAllegati = ListDichAttach });

                    if (Save() == true)
                    {
                        string TestoMail = "Buongiorno " + InnerObj.Nominativo + ",";
                        TestoMail += "\nper quanto riguarda l'istanza di " + InnerObj.DescrTipoIstanza + " " + InnerObj.DescrTributo.ToUpper();
                        TestoMail += " presentata in data " + new FunctionGrd().FormattaDataGrd(InnerObj.DataPresentazione);
                        try
                        {
                            if (InnerObj.ListDatiDich != null)
                            {
                                if (InnerObj.IDTributo == General.TRIBUTO.ICI)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichICI)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichICI)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichICI)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.TARSU)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.OSAP)
                                {
                                    TestoMail += " per l'indirizzo:\n" + ((SPC_DichOSAP)InnerObj.ListDatiDich).Ubicazione;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Valida->oggetto dei riferimenti non trovato::errore::", ex);
                        }
                        TestoMail += ", la richiesta è stata validata.";
                        if (NoteAggiuntive != string.Empty)
                            TestoMail += "\n" + NoteAggiuntive;
                        TestoMail += "\nCordiali Saluti.";
                        new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { DestMail }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello - Istanza Validata", TestoMail, ListMailAttachments, out sErr);
                        if (sErr != string.Empty)
                            return false;
                        else
                        {
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Accept", "Istanza Validata", "", "", "");
                            return true;
                        }
                    }
                    else
                    {
                        sErr = "Errore in Validazione!;";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Valida::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFiles"></param>
        /// <param name="NoteAggiuntive"></param>
        /// <param name="DestMail"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool Respingi(HttpFileCollection ListFiles, string NoteAggiuntive, string DestMail, out string sErr)
        {
            sErr = string.Empty;
            List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
            List<IstanzaAllegato> ListDichAttach = new List<IstanzaAllegato>();
            try
            {
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Comunicazione, ref ListMailAttachments, ref ListDichAttach))
                {
                    sErr = "Errore in lettura allegati!;";
                    return false;
                }
                else {
                    foreach (IstanzaAllegato myAll in ListDichAttach)
                    {
                        InnerObj.ListAllegati.Add(myAll);
                    }
                    InnerObj.DataRespinta = DateTime.Now;
                    if (NoteAggiuntive != string.Empty || ListDichAttach.Count > 0)
                        InnerObj.ListComunicazioni.Add(new IstanzaComunicazione() { IDIstanza = InnerObj.IDIstanza, IDTipo = Istanza.STATO.Respinta, Data = DateTime.Now, Testo = NoteAggiuntive, ListAllegati = ListDichAttach });

                    if (Save() == true)
                    {
                        string TestoMail = "Buongiorno " + InnerObj.Nominativo + ",";
                        TestoMail += "\nper quanto riguarda l'istanza di " + InnerObj.DescrTipoIstanza + " " + InnerObj.DescrTributo.ToUpper();
                        TestoMail += " presentata in data " + new FunctionGrd().FormattaDataGrd(InnerObj.DataPresentazione);
                        try
                        {
                            if (InnerObj.ListDatiDich != null)
                            {
                                if (InnerObj.IDTributo == General.TRIBUTO.ICI)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichICI)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichICI)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichICI)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.TARSU)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.OSAP)
                                {
                                    TestoMail += " per l'indirizzo:\n" + ((SPC_DichOSAP)InnerObj.ListDatiDich).Ubicazione;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Respingi->oggetto dei riferimenti non trovato::errore::", ex);
                        }
                        TestoMail += ", la richiesta è stata respinta.";
                        if (NoteAggiuntive != string.Empty)
                            TestoMail += "\n" + NoteAggiuntive;
                        TestoMail += "\nCordiali Saluti.";
                        new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { DestMail }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello - Istanza Respinta", TestoMail, ListMailAttachments, out sErr);
                        if (sErr != string.Empty)
                            return false;
                        else
                        {
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Stop", "Istanza Respinta", "", "", "");
                            return true;
                        }
                    }
                    else
                    {
                        sErr = "Errore in Rifiuto Istanza!;";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Respingi::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFiles"></param>
        /// <param name="NoteAggiuntive"></param>
        /// <param name="DestMail"></param>
        /// <param name="sErr"></param>
        /// <returns></returns>
        public bool Integrazione(HttpFileCollection ListFiles, string NoteAggiuntive, string DestMail, out string sErr)
        {
            sErr = string.Empty;
            List<System.Web.Mail.MailAttachment> ListMailAttachments = new List<System.Web.Mail.MailAttachment>();
            List<IstanzaAllegato> ListDichAttach = new List<IstanzaAllegato>();
            try
            {
                if (!new General().UploadAttachments(ListFiles, IstanzaAllegato.TIPO.Comunicazione, ref ListMailAttachments, ref ListDichAttach))
                {
                    sErr = "Errore in lettura allegati!;";
                    return false;
                }
                else {
                    foreach (IstanzaAllegato myAll in ListDichAttach)
                    {
                        InnerObj.ListAllegati.Add(myAll);
                    }
                    foreach (IstanzaComunicazione myComu in InnerObj.ListComunicazioni)
                    {
                        if (myComu.DataLettura.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                        {
                            myComu.DataLettura = DateTime.Now;
                        }
                    }
                    InnerObj.ListComunicazioni.Add(new IstanzaComunicazione() { IDIstanza = InnerObj.IDIstanza, IDTipo = ((MySession.Current.Scope == "FO") ? Istanza.STATO.IntegrazioniFO : Istanza.STATO.Integrazioni), Data = DateTime.Now, DataLettura = ((MySession.Current.Scope == "FO") ? DateTime.Now : DateTime.MaxValue), Testo = NoteAggiuntive, ListAllegati = ListDichAttach });
                    InnerObj.DataIntegrazioni = ((MySession.Current.Scope == "FO") ? DateTime.MaxValue : DateTime.Now);
                    if (Save() == true)
                    {
                        string TestoMail = "Buongiorno ";
                        if (MySession.Current.Scope == "BO")
                            TestoMail += InnerObj.Nominativo;
                        TestoMail += ",";
                        TestoMail += "\nper quanto riguarda l'istanza di " + InnerObj.DescrTipoIstanza + " " + InnerObj.DescrTributo.ToUpper();
                        TestoMail += " presentata in data " + new FunctionGrd().FormattaDataGrd(InnerObj.DataPresentazione);
                        try
                        {
                            if (InnerObj.ListDatiDich != null)
                            {
                                if (InnerObj.IDTributo == General.TRIBUTO.ICI)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichICI)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichICI)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichICI)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.TARSU)
                                {
                                    TestoMail += " sui riferimenti catastali:\n";
                                    TestoMail += " Foglio " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Foglio;
                                    TestoMail += " Particella " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Numero;
                                    TestoMail += " Subalterno " + ((SPC_DichTARSU)InnerObj.ListDatiDich).Sub;
                                }
                                else if (InnerObj.IDTributo == General.TRIBUTO.OSAP)
                                {
                                    TestoMail += " per l'indirizzo:\n" + ((SPC_DichOSAP)InnerObj.ListDatiDich).Ubicazione;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Integrazione->oggetto dei riferimenti non trovato::errore::", ex);
                        }
                        TestoMail += ", si comunica";
                        if (NoteAggiuntive != string.Empty)
                            TestoMail += "\n" + NoteAggiuntive;
                        TestoMail += "\nCordiali Saluti.";
                        new EmailService().CreateMail(MySession.Current.Ente.Mail, new List<string>() { DestMail }, new List<string>() { MySession.Current.Ente.Mail.Archive }, "Sportello - Comunicazioni Istanza", TestoMail, ListMailAttachments, out sErr);
                        if (sErr != string.Empty)
                            return false;
                        else
                        {
                            new General().LogActionEvent(DateTime.Now, MySession.Current.UserLogged.NameUser, MySession.Current.Scope, "Istanze", "Dettaglio", "Integrazione", "Comunicazione su Istanza", "", "", "");
                            return true;
                        }
                    }
                    else
                    {
                        sErr = "Errore in Comunicazione su Istanza!;";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.Integrazione::errore::", ex);
                return false;
            }
        }
        #endregion
        #region "Link a Cartografia"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UICatasto"></param>
        /// <param name="HasSIT"></param>
        /// <param name="UrlSIT"></param>
        /// <param name="TokenSIT"></param>
        /// <param name="Foglio"></param>
        /// <param name="Numero"></param>
        /// <param name="Sub"></param>
        /// <param name="UrlWinOpen"></param>
        /// <returns></returns>
        public string GetLinkGIS(SPC_DichICI UICatasto,bool HasSIT, string UrlSIT,string TokenSIT, string Foglio, string Numero, string Sub, ref string UrlWinOpen)
        {
            string LinkGIS = string.Empty;
            try
            {
                if (UICatasto.LinkGIS != string.Empty)
                {
                    LinkGIS = "$('.BottoneMap').on(\"click\",function(){";
                    LinkGIS += "window.open('" + UICatasto.LinkGIS + "', '_blank');";
                    LinkGIS += "});";
                    UrlWinOpen= "window.open('" + UICatasto.LinkGIS + "', '_blank');";
                }
                else
                {
                    if (HasSIT && Foglio != string.Empty && Numero != string.Empty)
                    {
                        try
                        {
                            string requestUriParam = String.Format("?cf_iva={0}&foglio={1}&mappale={2}&subalterno={3}&cod_ente={4}&eff={5}"
                                    , string.Empty
                                    , Foglio
                                    , Numero
                                    , Sub
                                    , MySession.Current.Ente.CodCatastale
                                    , ""
                                );
                            string sErr = string.Empty;
                            var ListUICatasto = string.Empty;
                            new BLL.RestService().MakeRequestByRifCat<string>(UrlSIT
                                , requestUriParam
                                , result => ListUICatasto = result
                                , error => sErr = error.Message
                                , "Token: " + TokenSIT
                                );
                            if (sErr != string.Empty)
                            {
                                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.consultacatasto::errore::" + sErr);
                            }
                            else
                            {
                                var myJObject = Newtonsoft.Json.Linq.JObject.Parse(ListUICatasto);
                                foreach (object myInt in myJObject["fabbricati"]["unita_immobiliari"])
                                {
                                    var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                    if (((myDetIntest["subalterno"].ToString() != string.Empty) ? myDetIntest["subalterno"].ToString() : string.Empty) == Sub)
                                    {
                                        if (myDetIntest["link_mappa"].ToString() != string.Empty)
                                        {
                                            LinkGIS = "$('.BottoneMap').on(\"click\",function(){";
                                            LinkGIS += "window.open('" + myDetIntest["link_mappa"].ToString() + "', '_blank');";
                                            LinkGIS += "});";
                                            UrlWinOpen= "window.open('" + myDetIntest["link_mappa"].ToString() + "', '_blank');";
                                            Log.Debug("link_mappa->" + myDetIntest["link_mappa"].ToString());
                                        }
                                        else
                                        {
                                            LinkGIS = "$('.BottoneMap').on(\"click\",function(){";
                                            LinkGIS += "$('#lblErrorFO').text('Riferimento al SIT non trovato! Impossibile aprire SIT!');$('#lblErrorFO').show();";
                                            LinkGIS += "});";
                                            UrlWinOpen = "alert('Riferimento al SIT non trovato! Impossibile aprire SIT!')";
                                        }
                                    }
                                }
                                foreach (object myInt in myJObject["terreni"]["item_terreni"])
                                {
                                    var myDetIntest = Newtonsoft.Json.Linq.JObject.Parse(myInt.ToString());
                                    if (((myDetIntest["subalterno"].ToString() != string.Empty) ? myDetIntest["subalterno"].ToString() : string.Empty) == Sub)
                                    {
                                        if (myDetIntest["link_mappa"].ToString() != string.Empty)
                                        {
                                            LinkGIS = "$('.BottoneMap').on(\"click\",function(){";
                                            LinkGIS += "window.open('" + myDetIntest["link_mappa"].ToString() + "', '_blank');";
                                            LinkGIS += "});";
                                            UrlWinOpen = "window.open('" + myDetIntest["link_mappa"].ToString() + "', '_blank');";
                                            Log.Debug("link_mappa->" + myDetIntest["link_mappa"].ToString());
                                        }
                                        else
                                        {
                                            LinkGIS = "$('.BottoneMap').on(\"click\",function(){";
                                            LinkGIS += "$('#lblErrorFO').text('Riferimento al SIT non trovato! Impossibile aprire SIT!');$('#lblErrorFO').show();";
                                            LinkGIS += "});";
                                            UrlWinOpen = "alert('Riferimento al SIT non trovato! Impossibile aprire SIT!')";
                                        }
                                    }
                                }
                            }
                        }
                        catch (HttpException ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Istanze.consultacatasto::errore::", ex);
                        }
                        if (LinkGIS == string.Empty)
                        {
                            LinkGIS = "$('.BottoneMap').on(\"click\",function(){";
                            LinkGIS+= "$('#lblErrorFO').text('Riferimenti Catastali non trovati! Impossibile aprire SIT!');$('#lblErrorFO').show();";
                            LinkGIS += "});";
                            UrlWinOpen = "alert('Riferimento al SIT non trovato! Impossibile aprire SIT!')";
                        }
                    }
                }
            }
            catch (HttpException ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.consultacatasto::errore::", ex);
                UrlWinOpen = "alert('Riferimento al SIT non trovato! Impossibile aprire SIT!')";
            }
            return LinkGIS;
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione motivazioni istanze
    /// </summary>
    public class IstanzaMotivazioni : IstanzaMotivazione
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IstanzaMotivazioni));
        private IstanzaMotivazione InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public IstanzaMotivazioni(IstanzaMotivazione myItem)
        {
            InnerObj = myItem;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLISTANZEMOTIVAZIONI_IU", "ID", "IDISTANZA", "IDTYPE", "MOTIVAZIONE");
                    InnerObj.IDMotivazione = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.IDMotivazione)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                            , ctx.GetParam("MOTIVAZIONE", ((InnerObj.IDTipo > 0) ? string.Empty : InnerObj.Note))
                        ).First<int>();

                    ctx.Dispose();
                    if (InnerObj.IDMotivazione <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.IstanzaMotivazioni.Save::errore in inserimento motivazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.IstanzaMotivazioni.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <returns></returns>
        public List<IstanzaMotivazione> LoadMotivazioni(int IDIstanza)
        {
            List<IstanzaMotivazione> ListMyData = new List<IstanzaMotivazione>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetIstanzeMotivazioni", "IDISTANZA");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<IstanzaMotivazione>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)
                        ).ToList<IstanzaMotivazione>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.BLLIstanzaMotivazioni.LoadMotivazioni::errore::", ex);
                return ListMyData;
            }
        }
    }
    /// <summary>
    /// Classe di gestione allegati istanze
    /// </summary>
    public class IstanzaAllegati : IstanzaAllegato
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IstanzaAllegati));
        private IstanzaAllegato InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public IstanzaAllegati(IstanzaAllegato myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <returns></returns>
        public bool Save(string IdEnte)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLALLEGATI_IU", "ID", "IDRIF", "IDTYPE", "POSTEDFILE", "NAMEFILE", "FILEMIMETYPE","IDENTE");
                    InnerObj.IDAllegato = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.IDAllegato)
                            , ctx.GetParam("IDRIF", InnerObj.IDIstanza)
                            , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                            , ctx.GetParam("POSTEDFILE", InnerObj.PostedFile)
                            , ctx.GetParam("NAMEFILE", InnerObj.FileName)
                            , ctx.GetParam("FILEMIMETYPE", InnerObj.FileMIMEType)
                            , ctx.GetParam("IDENTE", IdEnte)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.IDAllegato <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.Allegati.Save::errore in inserimento allegato");
                        return false;
                    }
                    else
                    {
                        System.IO.File.WriteAllBytes(UrlHelper.GetPathAttachments + InnerObj.IDTipo.ToString().PadLeft(9, char.Parse("0")) + "_" + InnerObj.IDIstanza.ToString().PadLeft(9, char.Parse("0")) + "_" + InnerObj.FileName, InnerObj.PostedFile);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.Allegati.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <param name="IDAllegato"></param>
        /// <returns></returns>
        public List<IstanzaAllegato> LoadAllegati(int IDIstanza, int IDAllegato)
        {
            List<IstanzaAllegato> ListMyData = new List<IstanzaAllegato>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetIstanzeAllegati", "IDISTANZA", "IDALLEGATO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<IstanzaAllegato>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)
                            , ctx.GetParam("IDALLEGATO", IDAllegato)
                        ).ToList<IstanzaAllegato>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.BLLIstanzaAllegati.LoadAllegati::errore::", ex);
                return ListMyData;
            }
        }
    }
    /// <summary>
    /// Classe di gestione comunicazioni istanze
    /// </summary>
    public class IstanzaComunicazioni : IstanzaComunicazione
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IstanzaComunicazioni));
        private IstanzaComunicazione InnerObj { get; set; }
        private string IDUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        /// <param name="UserID"></param>
        public IstanzaComunicazioni(IstanzaComunicazione myItem, string UserID)
        {
            InnerObj = myItem;
            IDUser = UserID;
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
                    string sSQL = ctx.GetSQL("prc_TBLISTANZECOMUNICAZIONI_IU", "ID", "IDISTANZA", "IDTYPE", "DATA", "TESTO", "DATAREADING", "IDUSER");
                    InnerObj.IDComunicazione = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.IDComunicazione)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                            , ctx.GetParam("DATA", InnerObj.Data)
                            , ctx.GetParam("TESTO", InnerObj.Testo)
                            , ctx.GetParam("DATAREADING", InnerObj.DataLettura)
                            , ctx.GetParam("IDUSER", IDUser)
                        ).First<int>();

                    ctx.Dispose();
                    if (InnerObj.IDComunicazione <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.IstanzaComunicazioni.Save::errore in inserimento Comunicazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.IstanzaComunicazioni.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <returns></returns>
        public List<IstanzaComunicazione> LoadStatiComunicazioni(int IDIstanza)
        {
            List<IstanzaComunicazione> ListMyData = new List<IstanzaComunicazione>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetIstanzeStatiComunicazioni", "IDISTANZA");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<IstanzaComunicazione>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)
                        ).ToList<IstanzaComunicazione>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.BLLIstanzaComunicazioni.LoadStatiComunicazioni::errore::", ex);
                return ListMyData;
            }
        }
    }
    /// <summary>
    /// Classe gi gestione dichiarazioni
    /// </summary>
    public class Dichiarazioni
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Dichiarazioni));

        private Istanza InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public Dichiarazioni(Istanza myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDDichiarazione"></param>
        /// <param name="ListIstanze"></param>
        /// <returns></returns>
        public bool LoadDichiarazione(string IDEnte, int IDContribuente, int IDDichiarazione, out List<Istanza> ListIstanze)
        {
            ListIstanze = new List<Istanza>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDichiarazioni", "IDENTE"
                        , "IDCONTRIBUENTE"
                        , "ID");
                    ListIstanze = ctx.ContextDB.Database.SqlQuery<Istanza>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                            , ctx.GetParam("ID", IDDichiarazione)
                        ).ToList<Istanza>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.LoadDichiarazione::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetNewDichiarazione()
        {
            try
            {
                int nRet = 0;
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDichiarazioneMaxNum", "IDENTE");
                    nRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", InnerObj.IDEnte)).First<int>();
                    ctx.Dispose();
                }
                return nRet;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Istanze.GetNewDichiarazione::errore::", ex);
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveDichiarazione()
        {
            try
            {
                int nRet = 0;
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLDICHIARAZIONI_IU", "ID", "IDENTE", "NDICHIARAZIONE", "IDISTANZA");
                    nRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("NDICHIARAZIONE", InnerObj.NDichiarazione)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                        ).First<int>();
                    ctx.Dispose();
                    return nRet;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.Istanza.SaveDichiarazione::errore::", ex);
                return -1;
            }
        }
        #region"Stampa"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NDichiarazione"></param>
        /// <param name="IDTributo"></param>
        /// <param name="myEnte"></param>
        /// <param name="myAnag"></param>
        /// <param name="sScript"></param>
        /// <returns></returns>
        public bool StampaDichiarazione(int NDichiarazione, string IDTributo, EntiInLavorazione myEnte, DettaglioAnagrafica myAnag, out string sScript)
        {
            sScript = string.Empty;
            List<object> ListIstanze = new List<object>();
            bool AlreadyOccupanti = false;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    Log.Debug("OPENgovSPORTELLO.Models.Istanza.SaveDichiarazione::prc_GetDichiarazioneUI::" + myEnte.IDEnte.ToString() + "-"+ myAnag.COD_CONTRIBUENTE.ToString() + "-" + NDichiarazione.ToString());
                    string sSQL = ctx.GetSQL("prc_GetDichiarazioneUI", "IDENTE", "IDCONTRIBUENTE", "NDICHIARAZIONE");
                    if (IDTributo == General.TRIBUTO.ICI)
                    {
                        ListIstanze = ((ctx.ContextDB.Database.SqlQuery<SPC_DichICI>(sSQL, ctx.GetParam("IDENTE", myEnte.IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", myAnag.COD_CONTRIBUENTE)
                                    , ctx.GetParam("NDICHIARAZIONE", NDichiarazione)
                                ).ToList<SPC_DichICI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (IDTributo == General.TRIBUTO.TASI)
                    {
                        ListIstanze = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTASI>(sSQL, ctx.GetParam("IDENTE", myEnte.IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", myAnag.COD_CONTRIBUENTE)
                                    , ctx.GetParam("NDICHIARAZIONE", NDichiarazione)
                                ).ToList<SPC_DichTASI>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (IDTributo == General.TRIBUTO.TARSU)
                    {
                        ListIstanze = ((ctx.ContextDB.Database.SqlQuery<SPC_DichTARSU>(sSQL, ctx.GetParam("IDENTE", myEnte.IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", myAnag.COD_CONTRIBUENTE)
                                    , ctx.GetParam("NDICHIARAZIONE", NDichiarazione)
                                ).ToList<SPC_DichTARSU>()) as IEnumerable<object>).Cast<object>().ToList();
                        List<object> ListUI = new List<object>();
                        List<SPC_DichTARSUVani> ListVani = new List<SPC_DichTARSUVani>();
                        List<SPC_DichTARSUOccupanti> ListOccupanti = new List<SPC_DichTARSUOccupanti>();
                        foreach (SPC_DichTARSU myItem in ListIstanze)
                        {
                            if (!new TARSU().LoadTARSURidEse(myEnte.IDEnte, myAnag.COD_CONTRIBUENTE, -1, myItem.IDIstanza, "", SPC_DichTARSURidEse.TYPE.Riduzione, new SPC_DichTARSURidEse(), out ListUI))
                                return false;
                            else
                            {
                                foreach (object myObj in ListUI)
                                {
                                    myItem.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                }
                            }
                            if (!new TARSU().LoadTARSURidEse(myEnte.IDEnte, myAnag.COD_CONTRIBUENTE, -1, myItem.IDIstanza, "", SPC_DichTARSURidEse.TYPE.Esenzione, new SPC_DichTARSURidEse(), out ListUI))
                                return false;
                            else
                            {
                                foreach (object myObj in ListUI)
                                {
                                    myItem.ListRid.Add((SPC_DichTARSURidEse)myObj);
                                }
                            }
                            if (!new TARSU().LoadTARSUVani(myEnte.IDEnte, -1, myItem.IDIstanza,myItem.Tipo, out ListVani))
                                return false;
                            else
                                myItem.ListVani = ListVani;
                            if (!AlreadyOccupanti)
                            {
                                if (!new TARSU().LoadTARSUOccupanti(myEnte.IDEnte, -1, myItem.IDIstanza, -1, out ListOccupanti))
                                    return false;
                                else
                                    myItem.ListOccupanti = ListOccupanti;
                            }
                            AlreadyOccupanti = true;
                        }
                    }
                    else if (IDTributo == General.TRIBUTO.OSAP)
                    {
                        ListIstanze = ((ctx.ContextDB.Database.SqlQuery<SPC_DichOSAP>(sSQL, ctx.GetParam("IDENTE", myEnte.IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", myAnag.COD_CONTRIBUENTE)
                                    , ctx.GetParam("NDICHIARAZIONE", NDichiarazione)
                                ).ToList<SPC_DichOSAP>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                int nUI, MaxUIperDoc, MaxUI;
                nUI = MaxUIperDoc = MaxUI = 0;
                string sAnnotazioni = string.Empty;

                MaxUIperDoc = ((IDTributo == General.TRIBUTO.ICI || IDTributo == General.TRIBUTO.TASI) ? 3 : ((IDTributo == General.TRIBUTO.TARSU) ? 2 : 1));
                MaxUI = ((IDTributo == General.TRIBUTO.ICI) ? 6 : ((IDTributo == General.TRIBUTO.TARSU) ? 4 : ((IDTributo == General.TRIBUTO.TASI) ? 8 : 1)));

                sScript += "<html xmlns='http://www.w3.org/1999/xhtml'>";
                sScript += "<head>";
                sScript += "<link href='../Content/Dich" + IDTributo + ".css' rel='stylesheet'>";
                sScript += "</head>";
                sScript += "<body>";
                sScript += "<div id='dichhome'>";
                sScript += StampaTestata(myEnte, myAnag);
                if (IDTributo == General.TRIBUTO.TARSU)
                {
                    List<SPC_DichTARSU> ListTmp = new List<SPC_DichTARSU>();
                    foreach (SPC_DichTARSU myTmp in ListIstanze)
                        ListTmp.Add(myTmp);
                    sScript += StampaSpedizione(myAnag);
                    sScript += StampaOccupantiTARSU(ListTmp);
                }
                else if (IDTributo == General.TRIBUTO.OSAP)
                {
                    sScript += StampaSpedizione(myAnag);
                }
                else if (IDTributo == General.TRIBUTO.TASI)
                {
                    sScript += "<div id='dich_UIFP'>";
                    for (int x = 0; x < 2; x++)
                    {
                        nUI += 1;
                        if (ListIstanze.Count - 1 >= x)
                            sScript += StampaUITASI((SPC_DichTASI)ListIstanze[x], nUI, "FP", ref sAnnotazioni);
                        else
                            break;
                    }
                    sScript += "</div>";
                    ListIstanze.RemoveAt(0);
                    if (ListIstanze.Count > 0)
                        ListIstanze.RemoveAt(0);
                }
                sScript += "</div>";
                sScript += "<div id='dich_UI'>";
                if (IDTributo == General.TRIBUTO.TARSU)
                {
                    string Tipo = string.Empty;
                    int NC=0;
                    DateTime Inizio, Fine;
                    Inizio = DateTime.MaxValue; Fine = DateTime.MinValue;
                    foreach (Object myUI in ListIstanze)
                    {
                        if (Tipo.IndexOf(((SPC_DichTARSU)myUI).Stato) < 0)
                            Tipo += (Tipo != string.Empty ? "," : string.Empty) + ((SPC_DichTARSU)myUI).Stato;
                        if (((SPC_DichTARSU)myUI).DataInizio < Inizio)
                            Inizio = ((SPC_DichTARSU)myUI).DataInizio;
                        if (((SPC_DichTARSU)myUI).DataFine > Fine)
                            Fine = ((SPC_DichTARSU)myUI).DataFine;
                        NC += ((((SPC_DichTARSU)myUI).ListOccupanti.Count > 1) ? ((SPC_DichTARSU)myUI).ListOccupanti.Count : 0);
                    }
                    sScript += StampaGenTARSU(((SPC_DichTARSU)ListIstanze[0]), Inizio, Fine,Tipo,NC);
                }
                foreach (Object myUI in ListIstanze)
                {
                    nUI += 1;
                    if (nUI <= MaxUI)
                    {
                        if (IDTributo == General.TRIBUTO.ICI)
                        {
                            sScript += StampaUIICI((SPC_DichICI)myUI, nUI, ref sAnnotazioni);
                        }
                        else if (IDTributo == General.TRIBUTO.TASI)
                        {
                            sScript += StampaUITASI((SPC_DichTASI)myUI, nUI, "", ref sAnnotazioni);
                        }
                        else if (IDTributo == General.TRIBUTO.TARSU)
                        {
                            sScript += StampaUITARSU((SPC_DichTARSU)myUI, nUI, ref sAnnotazioni);
                        }
                        else if (IDTributo == General.TRIBUTO.OSAP)
                        {
                            sScript += StampaUIOSAP((SPC_DichOSAP)myUI, nUI, ref sAnnotazioni);
                        }
                        if ((((IDTributo == General.TRIBUTO.TASI) ? nUI - 2 : nUI) % MaxUIperDoc) == 0)
                        {
                            if (nUI == MaxUI)
                            {
                                sAnnotazioni = "Ulteriori immobili presenti non stampati per limiti di stampa.";
                            }
                            sScript += "<div id='dichannotazioni'>";
                            sScript += sAnnotazioni;
                            sScript += "</div>";
                            if (ListIstanze.Count > nUI && nUI < MaxUI)
                            {
                                sScript += "</div>";
                                sScript += "<div id='dich_UI'>";
                                if (IDTributo == General.TRIBUTO.TARSU)
                                {
                                    sScript += "<div id='dich_TipoDich'></div>";
                                }
                            }
                            sAnnotazioni = string.Empty;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (nUI <= MaxUI)
                {
                    for (int x = nUI; (x % MaxUIperDoc) != 0; x++)
                    {
                        sScript += "<div id='dich_DetUI'>";
                        sScript += "</div>";
                    }
                    if (sAnnotazioni != string.Empty)
                    {
                        sScript += "<div id='dichannotazioni'>";
                        sScript += sAnnotazioni;
                        sScript += "</div>";
                        sAnnotazioni = string.Empty;
                    }
                }
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</body>";
                sScript += "</html>";
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazione::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="myAnag"></param>
        /// <returns></returns>
        public string StampaTestata(EntiInLavorazione myEnte, DettaglioAnagrafica myAnag)
        {
            string sScript = string.Empty;
            try
            {
                sScript += "<div id='dich_testata'>";
                sScript += "<div id='dichannotestata'>";
                sScript += "<input type='text' id='dichDannotestata' value='" + DateTime.Now.Year.ToString().Substring(2, 2) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichente'>";
                sScript += "<input type='text' id='dichDente' value='" + myEnte.Descrizione.Replace("à", "a'").Replace("è", "e'").Replace("é", "e'").Replace("ì", "i'").Replace("ò", "o'").Replace("ù", "u'").Replace("'", "&rsquo;").ToUpper() + "'>";
                sScript += "</div>";
                sScript += "<div id='dich_contribuente'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcf'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcf' value='" + ((myAnag.PartitaIva != string.Empty && myAnag.PartitaIva!="00000000000") ? myAnag.PartitaIva : myAnag.CodiceFiscale) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichprefisso'>";
                sScript += "<input type='text' class='dich_dett' id='dichDprefisso' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichntel'>";
                sScript += "<input type='text' class='dich_dett' id='dichDntel' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichmail'>";
                sScript += "<input type='text' class='dich_dett' id='dichDmail' value=''>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcognome'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcognome'  value='" + myAnag.Cognome.Replace("à","a'").Replace("è", "e'").Replace("é", "e'").Replace("ì", "i'").Replace("ò", "o'").Replace("ù", "u'").Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichnome'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnome' value='" + myAnag.Nome.Replace("à", "a'").Replace("è", "e'").Replace("é", "e'").Replace("ì", "i'").Replace("ò", "o'").Replace("ù", "u'").Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatanascGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetGGFromData(myAnag.DataNascita) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatanascMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetMMFromData(myAnag.DataNascita) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatanascAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetAAFromData(myAnag.DataNascita) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichsessoM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDsesso' value='" + ((myAnag.Sesso == "M") ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichsessoF'>";
                sScript += "<input type='text' class='dich_dett' id='dichDsesso' value='" + ((myAnag.Sesso == "F") ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcomunenasc'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomunenasc' value='" + myAnag.ComuneNascita.Replace("à", "a'").Replace("è", "e'").Replace("é", "e'").Replace("ì", "i'").Replace("ò", "o'").Replace("ù", "u'").Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichpvnasc'>";
                sScript += "<input type='text' class='dich_dett' id='dichDpv' value='" + myAnag.ProvinciaNascita + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichres'>";
                sScript += "<input type='text' class='dich_dett' id='dichDres' value='" + myAnag.ViaResidenza.Replace("à", "a'").Replace("è", "e'").Replace("é", "e'").Replace("ì", "i'").Replace("ò", "o'").Replace("ù", "u'").Replace("'", "&rsquo;") + " " + myAnag.CivicoResidenza + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcap'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcap'  value='" + myAnag.CapResidenza + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcomune'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomune' value='" + myAnag.ComuneResidenza.Replace("à", "a'").Replace("è", "e'").Replace("é", "e'").Replace("ì", "i'").Replace("ò", "o'").Replace("ù", "u'").Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichpv'>";
                sScript += "<input type='text' class='dich_dett' id='dichDpv' value='" + myAnag.ProvinciaResidenza + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneTestata::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAnag"></param>
        /// <returns></returns>
        public string StampaSpedizione(DettaglioAnagrafica myAnag)
        {
            string sScript = string.Empty;
            try
            {
                sScript += "<div id='dich_invio'>";
                foreach (ObjIndirizziSpedizione mySped in myAnag.ListSpedizioni)
                {
                    sScript = string.Empty;
                    sScript += "<div class='dicrow'>";
                    sScript += "<div id='dichcognomeCO'>";
                    sScript += "<input type='text' class='dich_dett' id='dichDcognome'  value='" + mySped.CognomeInvio.Replace("'", "&rsquo;") + "'>";
                    sScript += "</div>";
                    sScript += "</div>";
                    sScript += "<div class='dicrow'>";
                    sScript += "<div id='dichnomeCO'>";
                    sScript += "<input type='text' class='dich_dett' id='dichDnome' value='" + mySped.NomeInvio.Replace("'", "&rsquo;") + "'>";
                    sScript += "</div>";
                    sScript += "</div>";
                    sScript += "<br />";
                    sScript += "<br /> ";
                    sScript += "<div class='dicrow'>";
                    sScript += "<div id='dichresCO'>";
                    sScript += "<input type='text' class='dich_dett' id='dichDres' value='" + mySped.ViaRCP.Replace("'", "&rsquo;") + " " + mySped.CivicoRCP + "'>";
                    sScript += "</div>";
                    sScript += "<div id='dichcapCO'>";
                    sScript += "<input type='text' class='dich_dett' id='dichDcap'  value='" + mySped.CapRCP + "'>";
                    sScript += "</div>";
                    sScript += "<div id='dichcomuneCO'>";
                    sScript += "<input type='text' class='dich_dett' id='dichDcomune' value='" + mySped.ComuneRCP.Replace("'", "&rsquo;") + "'>";
                    sScript += "</div>";
                    sScript += "<div id='dichpvCO'>";
                    sScript += "<input type='text' class='dich_dett' id='dichDpv' value='" + mySped.ProvinciaRCP + "'>";
                    sScript += "</div>";
                    sScript += "</div>";
                }
                sScript += "</div>";
                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneSpedizione::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUI"></param>
        /// <param name="NOrdine"></param>
        /// <param name="sAnnotazioni"></param>
        /// <returns></returns>
        public string StampaUIICI(SPC_DichICI myUI, int NOrdine, ref string sAnnotazioni)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dich_DetUI'>";
                sScript += "<br /><br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichNOrdine'>";
                sScript += "<input type='text' class='dich_dett' id='dichDNOrdine' value='" + NOrdine.ToString() + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcaratteristica'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcaratteristica' value='" + ((myUI.DescrTipologia.ToLower().IndexOf("terren") > 0) ? "1" : (myUI.DescrTipologia.ToLower().IndexOf("fabbricabil") > 0 || myUI.DescrTipologia.ToLower().IndexOf("edificabil") > 0) ? "2" : (myUI.DescrTipologia.ToLower().IndexOf("principal") > 0) ? "5" : "3") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichindirizzo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDindirizzo' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br /><br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dicdaticat'>";
                sScript += "<div id='dichsezione'>";
                sScript += "<input type='text' class='dich_dett' id='dichDsezione' value='" + myUI.Sezione + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfoglio'>";
                sScript += "<input type='text' class='dich_dett' id='dichDfoglio' value='" + myUI.Foglio + "'>";
                sScript += "</div>";
                sScript += "<div id='dichnumero'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnumero' value='" + myUI.Numero + "'>";
                sScript += "</div>";
                sScript += "<div id='dichsub'>";
                sScript += "<input type='text' class='dich_dett' id='dichDsub' value='" + myUI.Sub + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcat'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcat' value='" + myUI.DescrCat + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcl'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcl' value='" + myUI.DescrClasse + "'>";
                sScript += "</div>";
                sScript += "<div id='dichprot'>";
                sScript += "<input type='text' class='dich_dett' id='dichDprot' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichanno'>";
                sScript += "<input type='text' class='dich_dett' id='dichDanno' value=''>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br /><br /><br /><br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichstoricoinagibile'>";
                sScript += "<input type='text' id='dichDstoricoinagibile' class='dich_dett' value='" + ((myUI.IsStorico || myUI.Stato == Istanza.TIPO.Inagibilità) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichvalore'>";
                sScript += "<input type='text' class='dich_dett' id='dichDvalore' value='" + myUI.RenditaValore.ToString("#,##0.00") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichposs'>";
                sScript += "<input type='text' class='dich_dett' id='dichDposs' value='" + myUI.PercPossesso.ToString("#,##0.00") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichridta'>";
                sScript += "<input type='text' class='dich_dett' id='dichDridta' value='" + ((myUI.Stato != Istanza.TIPO.Inagibilità && myUI.DescrTipologia.IndexOf("terren") > 0 && myUI.PercRiduzione > 0) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichese'>";
                sScript += "<input type='text' class='dich_dett' id='dichDese' value='" + ((myUI.PercEsenzione > 0) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br /><br /><br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dicdatidate'>";
                sScript += "<div id='dichinizioGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetGGFromData(((myUI.DataFine.ToShortDateString() == DateTime.MaxValue.ToShortDateString() || myUI.DataFine.ToShortDateString() == DateTime.MinValue.ToShortDateString()) ? myUI.DataInizio : myUI.DataFine)) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichinizioMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetMMFromData(((myUI.DataFine.ToShortDateString() == DateTime.MaxValue.ToShortDateString() || myUI.DataFine.ToShortDateString() == DateTime.MinValue.ToShortDateString()) ? myUI.DataInizio : myUI.DataFine)) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichinizioAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetAAFromData(((myUI.DataFine.ToShortDateString() == DateTime.MaxValue.ToShortDateString() || myUI.DataFine.ToShortDateString() == DateTime.MinValue.ToShortDateString()) ? myUI.DataInizio : myUI.DataFine)) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdetraz'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdetraz' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichfinelavoriGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichfinelavoriMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichfinelavoriAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value=''>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br /><br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichacquisto'>";
                sScript += "<input type='text' id='dichDacquisto' class='dich_dett' value='" + ((myUI.DataFine.ToShortDateString() == DateTime.MaxValue.ToShortDateString() || myUI.DataFine.ToShortDateString() == DateTime.MinValue.ToShortDateString()) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcessione'>";
                sScript += "<input type='text' id='dichDcessione' class='dich_dett' value='" + ((myUI.DataFine.ToShortDateString() != DateTime.MaxValue.ToShortDateString() && myUI.DataFine.ToShortDateString() != DateTime.MinValue.ToShortDateString()) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichagentr'>";
                sScript += "<input type='text' class='dich_dett' id='dichHagentr' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichestremi'>";
                sScript += "<input type='text' class='dich_dett' id='dichHestremi' value=''>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sAnnotazioni += ((myUI.Stato == Istanza.TIPO.ComodatoUsoGratuito)
                        ? "N.ordine "
                            + NOrdine.ToString()
                            + " per i riferimenti "
                            + myUI.Foglio
                            + "-" + myUI.Numero
                            + "-" + myUI.Sub
                            + " concesso in Comodato Gratuito dal "
                            + new FunctionGrd().FormattaDataGrd(myUI.DataInizio)
                            + (
                                (myUI.DataFine != DateTime.MaxValue)
                                ? " al " + new FunctionGrd().FormattaDataGrd(myUI.DataFine)
                                : string.Empty
                            )
                            + "<br />"
                        : (
                            (myUI.Stato == Istanza.TIPO.Inagibilità)
                                ? "N.ordine "
                                    + NOrdine.ToString()
                                    + " per i riferimenti "
                                    + myUI.Foglio
                                    + "-" + myUI.Numero
                                    + "-" + myUI.Sub
                                    + " inagibile dal "
                                    + new FunctionGrd().FormattaDataGrd(myUI.DataInizio)
                                    + (
                                        (myUI.DataFine != DateTime.MaxValue)
                                        ? " al " + new FunctionGrd().FormattaDataGrd(myUI.DataFine)
                                        : string.Empty
                                    )
                                    + "<br />"
                                : (
                                    (myUI.Stato == Istanza.TIPO.Modifica)
                                        ? "N.ordine "
                                            + NOrdine.ToString()
                                            + " per i riferimenti "
                                            + myUI.Foglio
                                            + "-" + myUI.Numero
                                            + "-" + myUI.Sub
                                            + " modifica dati non inerenti al calcolo<br />"
                                        : "N.ordine "
                                            + NOrdine.ToString()
                                            + " per i riferimenti "
                                            + myUI.Foglio
                                            + "-" + myUI.Numero
                                            + "-" + myUI.Sub
                                            + " Utilizzo: "+myUI.DescrTipologia+" "
                                    )
                            )
                        );

                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneUIICI::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUI"></param>
        /// <param name="NOrdine"></param>
        /// <param name="SuffissoPosizione"></param>
        /// <param name="sAnnotazioni"></param>
        /// <returns></returns>
        public string StampaUITASI(SPC_DichTASI myUI, int NOrdine, string SuffissoPosizione, ref string sAnnotazioni)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dich_DetUI" + SuffissoPosizione + "'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichNOrdine'>";
                sScript += "<input type='text' class='dich_dett' id='dichDNOrdine' value='" + NOrdine.ToString() + "'>";
                sScript += "</div>";
                sScript += "<div id='dichindirizzo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDindirizzo' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dicdaticat'>";
                sScript += "<div id='dichsezione'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.Sezione + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfoglio'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.Foglio + "'>";
                sScript += "</div>";
                sScript += "<div id='dichnumero'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.Numero + "'>";
                sScript += "</div>";
                sScript += "<div id='dichsub'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.Sub + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcat'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.DescrCategoria + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcl'>";
                sScript += "<input type='text' class='dich_dett' value=''>";
                sScript += "</div>";
                sScript += "<div id='dichvalore'>";
                sScript += "<input type='text' class='dich_dett dichDright' value='" + myUI.RenditaValore.ToString("#,##0.00") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichinizioGG'>";
                sScript += "<input type='text' class='dich_dett dichDdata' value='" + new General().GetGGFromData(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichinizioMM'>";
                sScript += "<input type='text' class='dich_dett dichDdata' value='" + new General().GetMMFromData(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichinizioAA'>";
                sScript += "<input type='text' class='dich_dett dichDdata' value='" + new General().GetAAFromData(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfineGG'>";
                sScript += "<input type='text' class='dich_dett dichDdata' value='" + new General().GetGGFromData(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfineMM'>";
                sScript += "<input type='text' class='dich_dett dichDdata' value='" + new General().GetMMFromData(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichfineAA'>";
                sScript += "<input type='text' class='dich_dett dichDdata' value='" + new General().GetAAFromData(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichposs'>";
                sScript += "<input type='text' class='dich_dett dichDright' value='" + myUI.PercPossesso.ToString("#,##0.00") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichagevolaz'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.DescrAgevolazione.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichnaturatitolo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnaturatitolo' value='" + myUI.DescrNaturaTitolo.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichagentr'>";
                sScript += "<input type='text' class='dich_dett dichDcenter' value='" + myUI.AgEntrateContrattoAffitto.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichestremi'>";
                sScript += "<input type='text' class='dich_dett dichDcenter' value='" + myUI.EstremiContrattoAffitto.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sAnnotazioni += ((myUI.Stato == Istanza.TIPO.ComodatoUsoGratuito)
                        ? "N.ordine "
                            + NOrdine.ToString()
                            + " per i riferimenti "
                            + myUI.Foglio
                            + "-" + myUI.Numero
                            + "-" + myUI.Sub
                            + " concesso in Comodato Gratuito dal "
                            + new FunctionGrd().FormattaDataGrd(myUI.DataInizio)
                            + (
                                (myUI.DataFine != DateTime.MaxValue)
                                ? " al " + new FunctionGrd().FormattaDataGrd(myUI.DataFine)
                                : string.Empty
                            )
                            + "<br />"
                        : string.Empty);

                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneUITASI::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListUI"></param>
        /// <returns></returns>
        public string StampaOccupantiTARSU(List<SPC_DichTARSU> ListUI)
        {
            string sScript = string.Empty;
            int MaxOcc = 6;
            int nOcc = 0;
            try
            {
                foreach (SPC_DichTARSU myUI in ListUI)
                {
                    foreach (SPC_DichTARSUOccupanti myOcc in myUI.ListOccupanti)
                    {
                        nOcc += 1;
                        if (nOcc <= MaxOcc)
                        {
                            if (myOcc.Nominativo != string.Empty)
                            {
                                sScript += "<div class='dicrowOCC'>";
                                sScript += "<div id='dichnominativoOCC'>";
                                sScript += "<input type='text' class='dich_dettOCC' value='" + myOcc.Nominativo.Replace("'", "&rsquo;") + "'>";
                                sScript += "</div>";
                                sScript += "<div id='dichdatanascOCC'>";
                                sScript += "<input type='text' class='dich_dettOCC' value='" + new FunctionGrd().FormattaDataGrd(myOcc.DataNascita) + "'>";
                                sScript += "</div>";
                                sScript += "<div id='dichcomunenascOCC'>";
                                sScript += "<input type='text' class='dich_dettOCC' value='" + myOcc.LuogoNascita.Replace("'", "&rsquo;") + "'>";
                                sScript += "</div>";
                                sScript += "<div id='dichcfOCC'>";
                                sScript += "<input type='text' class='dich_dettOCC' value='" + myOcc.CodFiscale.Replace("'", "&rsquo;") + "'>";
                                sScript += "</div>";
                                sScript += "<div id='dichparentelaOCC'>";
                                sScript += myOcc.DescrParentela.Replace("'", "&rsquo;");
                                sScript += "</div>";
                                sScript += "</div>";
                            }
                        }
                    }
                }
                if (sScript != string.Empty)
                {
                    sScript = "<div id='dich_occupanti'>" + sScript + "</div>";
                }
                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneSpedizione::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUI"></param>
        /// <param name="Inizio"></param>
        /// <param name="Fine"></param>
        /// <param name="TipoDich"></param>
        /// <param name="nComponenti"></param>
        /// <returns></returns>
        public string StampaGenTARSU(SPC_DichTARSU myUI, DateTime Inizio, DateTime Fine,string TipoDich, int nComponenti)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dich_TipoDich'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichtipodic'>";
                sScript += "<input type='text' class='dich_dett' value='" + TipoDich + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichdatainizioGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetGGFromData(Inizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatainizioMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetMMFromData(Inizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatainizioAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetAAFromData(Inizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatafineGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetGGFromData(Fine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatafineMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetMMFromData(Fine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatafineAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetAAFromData(Fine) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichnoccup'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcenter' value='" + nComponenti + "'>";
                sScript += "</div>";
                sScript += "<div id='dichtipooccup'>";
                sScript += "<input type='text' class='dich_dett' value='" + myUI.StatoOccupazione + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>"; return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneGenTARSU::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUI"></param>
        /// <param name="NOrdine"></param>
        /// <param name="sAnnotazioni"></param>
        /// <returns></returns>
        public string StampaUITARSU(SPC_DichTARSU myUI, int NOrdine, ref string sAnnotazioni)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<div id='dich_DetUI'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichNOrdine'>";
                sScript += "<input type='text' class='dich_dett' id='dichDNOrdine' value='" + NOrdine.ToString() + "'>";
                sScript += "</div>";
                sScript += "<div id='dichindirizzo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDindirizzo' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                myUI.ListVani = myUI.ListVani.OrderBy(o => o.MQ).ToList();
                myUI.ListVani.Reverse();
                for (int x = 0; x < 3; x++)
                {
                    if (x <= myUI.ListVani.Count - 1)
                    {
                        SPC_DichTARSUVani myVano = myUI.ListVani[x];
                        sScript += "<div class='dicrow'>";
                        sScript += "<div id='dichfoglio'>";
                        sScript += "<input type='text' class='dich_dett' id='dichDcenter' value='" + myUI.Foglio + "'>";
                        sScript += "</div>";
                        sScript += "<div id='dichnumero'>";
                        sScript += "<input type='text' class='dich_dett' id='dichDcenter' value='" + myUI.Numero + "'>";
                        sScript += "</div>";
                        sScript += "<div id='dichsub'>";
                        sScript += "<input type='text' class='dich_dett' id='dichDcenter' value='" + myUI.Sub + "'>";
                        sScript += "</div>";
                        sScript += "<div id='dichcat'>";
                        sScript += "<input type='text' class='dich_dett' id='dichDcat' value='" + myVano.CategoriaEstesa + "'>";
                        sScript += "</div>";
                        sScript += "<div id='dichmq'>";
                        sScript += "<input type='text' class='dich_dett' id='dichDmq' value='" + ((myVano.IsEsente == 0) ? myVano.MQ.ToString("#,##0.00") : string.Empty) + "'>";
                        sScript += "</div>";
                        sScript += "<div id='dichmqesenti'>";
                        sScript += "<input type='text' class='dich_dett' id='dichDmq' value='" + ((myVano.IsEsente == 1) ? myVano.MQ.ToString("#,##0.00") : string.Empty) + "'>";
                        sScript += "</div>";
                        sScript += "</div>";
                    }
                    else
                    {
                        sScript += "<div class='dicrow'>";
                        sScript += "</div>";
                    }
                }
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichrid'>";
                sScript += "<input type='text' class='dich_dett' value='" + ((myUI.ListRid.Count > 0) ? myUI.ListRid[0].Descrizione : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichese'>";
                sScript += "<input type='text' class='dich_dett' value='" + ((myUI.ListEse.Count > 0) ? myUI.ListEse[0].Descrizione : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sAnnotazioni += ((myUI.Stato == Istanza.TIPO.ComodatoUsoGratuito)
                        ? "N.ordine "
                            + NOrdine.ToString()
                            + " per i riferimenti "
                            + myUI.Foglio
                            + "-" + myUI.Numero
                            + "-" + myUI.Sub
                            + " concesso in Comodato Gratuito dal "
                            + new FunctionGrd().FormattaDataGrd(myUI.DataInizio)
                            + (
                                (myUI.DataFine != DateTime.MaxValue)
                                ? " al " + new FunctionGrd().FormattaDataGrd(myUI.DataFine)
                                : string.Empty
                            )
                            + "<br />"
                        : (myUI.Stato == Istanza.TIPO.Variazione || myUI.Stato==Istanza.TIPO.Modifica)
                        ? myUI.Note!=string.Empty
                            ?"N.ordine "
                                + NOrdine.ToString()
                                + " per i riferimenti "
                                + myUI.Foglio
                                + "-" + myUI.Numero
                                + "-" + myUI.Sub
                                + " " + myUI.Note
                                + "<br />"
                            :string.Empty
                        : string.Empty);

                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneUITARSU::errore::", ex);
                return string.Empty;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUI"></param>
        /// <param name="NOrdine"></param>
        /// <param name="sAnnotazioni"></param>
        /// <returns></returns>
        public string StampaUIOSAP(SPC_DichOSAP myUI, int NOrdine, ref string sAnnotazioni)
        {
            try
            {
                string sScript = string.Empty;
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichindirizzo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextleft' value='" + myUI.Via.Replace("'", "&rsquo;") + " " + myUI.Civico + "'>";
                sScript += "</div>";
                sScript += "<div id='dichattrazione'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcheck' value='" + ((myUI.IsAttrazione) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br /> ";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichdatainizioGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetGGFromData(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatainizioMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetMMFromData(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatainizioAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetAAFromData(myUI.DataInizio) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatafineGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetGGFromData(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatafineMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetMMFromData(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatafineAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdata' value='" + new General().GetAAFromData(myUI.DataFine) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdurata'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextleft' value='" + myUI.Durata.ToString() + " " + myUI.DescrTipoDurata + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcat'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextleft' value='" + myUI.DescrCategoria + "'>";
                sScript += "</div>";
                sScript += "<div id='dichoccupazione'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextleft' value='" + myUI.DescrOccupazione + "'>";
                sScript += "</div>";
                sScript += "<div id='dichconsistenza'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextleft' value='" + myUI.Consistenza.ToString() + " " + myUI.DescrConsistenza + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<div class='dicrowdouble'>";
                sScript += "<div id='dichmaggiorazione'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextright' value='" + ((myUI.PercMagg > 0) ? myUI.PercMagg.ToString("#,##0.00") : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdetrazione'>";
                sScript += "<input type='text' class='dich_dett' id='dichDtextright' value='" + ((myUI.ImpDetraz > 0) ? myUI.ImpDetraz.ToString("#,##0.00") : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichagevolazioni'>";
                string sAgevolazioni = string.Empty;
                foreach (GenericCategory myAg in myUI.ListAgevolazioni)
                {
                    sAgevolazioni += ((sAgevolazioni != string.Empty) ? " - " : "") + myAg.Descrizione;
                }
                sScript += "<textarea class='dich_dett' id='dichDtextleft'>" + sAgevolazioni + "</textarea>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichestremi'>";
                sScript += "<input type='text' class='dich_dett_richiesta' id='dichDtextleft' value='" + myUI.DescrTipoAtto
                    + ((myUI.NAtto != string.Empty) ? " del " + new FunctionGrd().FormattaDataGrd(myUI.DataAtto) : "")
                    + ((myUI.NAtto != string.Empty) ? " n. " + myUI.NAtto : "")
                    + "'>";
                sScript += "</div>";
                sScript += "<div id='dichrichiedente'>";
                sScript += "<input type='text' class='dich_dett_richiesta' id='dichDtextcenter' value='" + myUI.DescrRichiedente + "'>";
                sScript += "</div>";
                sScript += "<div id='dichtributo'>";
                sScript += "<input type='text' class='dich_dett_richiesta' id='dichDtextcenter' value='" + myUI.DescrTributo + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sAnnotazioni += string.Empty;

                return sScript;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Dichiarazioni.StampaDichiarazioneUIOSAP::errore::", ex);
                return string.Empty;
            }
        }
        #endregion
    }
    /// <summary>
    /// Classe di gestione Profilo 
    /// </summary>
    public class Profilo
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Profilo));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDContribuente"></param>
        /// <param name="ddlTributo"></param>
        /// <param name="ddlTipoContatto"></param>
        /// <param name="myAnag"></param>
        /// <returns></returns>
        public bool Load(int IDContribuente, System.Web.UI.WebControls.DropDownList ddlTributo, System.Web.UI.WebControls.DropDownList ddlTipoContatto, DettaglioAnagrafica myAnag)
        {
            General fncGen = new General();

            try
            {
                using (DBModel ctx = new DBModel())
                {
                    fncGen.LoadCombo(ddlTributo, new Settings().LoadTributi(0), "CODICE", "DESCRIZIONE");
                    fncGen.LoadCombo(ddlTipoContatto, myAnag.dsTipiContatti, "IDTipoRiferimento", "DESCRIZIONE");
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Profilo.Load::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAnag"></param>
        /// <param name="IdContribLog"></param>
        /// <returns></returns>
        public string LoadJumbotron(DettaglioAnagrafica myAnag, int IdContribLog)
        {
            string sScript = string.Empty;
            try
            {
                sScript += "<p>&ensp;";
                if (myAnag != null)
                {
                    if (IdContribLog != myAnag.COD_CONTRIBUENTE)
                        sScript += "<label class=\"label text-italic\">per conto di </label>";
                        sScript += "<label class=\"lead_bold\">" + (myAnag.Nome + " " + myAnag.Cognome).Trim().Replace("'", "’") + " </label>";
                        sScript += "<label class=\"label\">&ensp;" + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva) + "</label>";
                        sScript += "<label class\"label\">&ensp;" + (
                                (
                                    (myAnag.Nome + " " + myAnag.Cognome).Trim()
                                    + "&ensp;"
                                    + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                    + "&ensp;"
                                    + myAnag.ViaResidenza
                                    + (myAnag.CivicoResidenza != string.Empty ? ", " + myAnag.CivicoResidenza : " " + myAnag.CivicoResidenza)
                                    + ((myAnag.CapResidenza != string.Empty || myAnag.ComuneResidenza != string.Empty || myAnag.ProvinciaResidenza != string.Empty)
                                        ? " - " + myAnag.CapResidenza + " " + myAnag.ComuneResidenza +
                                            ((myAnag.ProvinciaResidenza != string.Empty)
                                                ? " (" + myAnag.ProvinciaResidenza + ")"
                                                : string.Empty)
                                        : string.Empty)
                                ).Length>109
                                    ? (
                                            (myAnag.Nome + " " + myAnag.Cognome).Trim()
                                            + "&ensp;"
                                            + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                            + "&ensp;"
                                            + myAnag.ViaResidenza
                                            + (myAnag.CivicoResidenza != string.Empty ? ", " + myAnag.CivicoResidenza : " " + myAnag.CivicoResidenza)
                                            + ((myAnag.CapResidenza != string.Empty || myAnag.ComuneResidenza != string.Empty || myAnag.ProvinciaResidenza != string.Empty)
                                                ? " - " + myAnag.CapResidenza + " " + myAnag.ComuneResidenza +
                                                    ((myAnag.ProvinciaResidenza != string.Empty)
                                                        ? " (" + myAnag.ProvinciaResidenza + ")"
                                                        : string.Empty)
                                                : string.Empty)
                                        ).Substring(
                                            ((myAnag.Nome + " " + myAnag.Cognome).Trim()
                                                + "&ensp;"
                                                + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                                + "&ensp;"
                                            ).Length
                                            , 109- ((myAnag.Nome + " " + myAnag.Cognome).Trim()
                                                + "&ensp;"
                                                + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                                + "&ensp;"
                                            ).Length
                                        )
                                    : myAnag.ViaResidenza
                                        + (myAnag.CivicoResidenza != string.Empty ? ", " + myAnag.CivicoResidenza : " " + myAnag.CivicoResidenza)
                                        + ((myAnag.CapResidenza != string.Empty || myAnag.ComuneResidenza != string.Empty || myAnag.ProvinciaResidenza != string.Empty)
                                            ? " - " + myAnag.CapResidenza + " " + myAnag.ComuneResidenza +
                                                ((myAnag.ProvinciaResidenza != string.Empty)
                                                    ? " (" + myAnag.ProvinciaResidenza + ")"
                                                    : string.Empty)
                                            : string.Empty)
                            ).Replace("'", "’") + "</label>";
                }
                sScript += "</p>";
                return "$('.jumbotronAnag').html('" + sScript.Replace("'", "&rsquo;") + "');";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Profilo.LoadJumbotron::errore::", ex);
               return sScript;
            }
        }

        /// <param name="myAnag"></param>
        /// <param name="IdContribLog"></param>
        /// <returns></returns>
        public string LoadJumbotronFO(DettaglioAnagrafica myAnag, int IdContribLog)
        {
            string sScript = string.Empty;
            try
            {
                sScript += "<p>&ensp;";
                if (myAnag != null)
                {
                    if (IdContribLog != myAnag.COD_CONTRIBUENTE)
                        sScript += "<label class=\"label text-italic\">per conto di </label>";
                    sScript += "<label class=\"lead_bold\">" + (myAnag.Nome + " " + myAnag.Cognome).Trim().Replace("'", "’") + " </label>";
                    sScript += "<label class=\"label\">&ensp;" + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva) + "</label>";
                    sScript += "<label class\"label\">&ensp;" + (
                            (
                                (myAnag.Nome + " " + myAnag.Cognome).Trim()
                                + "&ensp;"
                                + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                + "&ensp;"
                                + myAnag.ViaResidenza
                                + (myAnag.CivicoResidenza != string.Empty ? ", " + myAnag.CivicoResidenza : " " + myAnag.CivicoResidenza)
                                + ((myAnag.CapResidenza != string.Empty || myAnag.ComuneResidenza != string.Empty || myAnag.ProvinciaResidenza != string.Empty)
                                    ? " - " + myAnag.CapResidenza + " " + myAnag.ComuneResidenza +
                                        ((myAnag.ProvinciaResidenza != string.Empty)
                                            ? " (" + myAnag.ProvinciaResidenza + ")"
                                            : string.Empty)
                                    : string.Empty)
                            ).Length > 109
                                ? (
                                        (myAnag.Nome + " " + myAnag.Cognome).Trim()
                                        + "&ensp;"
                                        + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                        + "&ensp;"
                                        + myAnag.ViaResidenza
                                        + (myAnag.CivicoResidenza != string.Empty ? ", " + myAnag.CivicoResidenza : " " + myAnag.CivicoResidenza)
                                        + ((myAnag.CapResidenza != string.Empty || myAnag.ComuneResidenza != string.Empty || myAnag.ProvinciaResidenza != string.Empty)
                                            ? " - " + myAnag.CapResidenza + " " + myAnag.ComuneResidenza +
                                                ((myAnag.ProvinciaResidenza != string.Empty)
                                                    ? " (" + myAnag.ProvinciaResidenza + ")"
                                                    : string.Empty)
                                            : string.Empty)
                                    ).Substring(
                                        ((myAnag.Nome + " " + myAnag.Cognome).Trim()
                                            + "&ensp;"
                                            + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                            + "&ensp;"
                                        ).Length
                                        , 109 - ((myAnag.Nome + " " + myAnag.Cognome).Trim()
                                            + "&ensp;"
                                            + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? "Codice Fiscale" : "Partita Iva") + ": " + (myAnag.PartitaIva == string.Empty || myAnag.PartitaIva == "00000000000" ? myAnag.CodiceFiscale : myAnag.PartitaIva)
                                            + "&ensp;"
                                        ).Length
                                    )
                                : myAnag.ViaResidenza
                                    + (myAnag.CivicoResidenza != string.Empty ? ", " + myAnag.CivicoResidenza : " " + myAnag.CivicoResidenza)
                                    + ((myAnag.CapResidenza != string.Empty || myAnag.ComuneResidenza != string.Empty || myAnag.ProvinciaResidenza != string.Empty)
                                        ? " - " + myAnag.CapResidenza + " " + myAnag.ComuneResidenza +
                                            ((myAnag.ProvinciaResidenza != string.Empty)
                                                ? " (" + myAnag.ProvinciaResidenza + ")"
                                                : string.Empty)
                                        : string.Empty)
                        ).Replace("'", "’") + "</label>";
                }
                sScript += "</p>";
                return "$('.jumbotronAnagFO').html('" + sScript.Replace("'", "&rsquo;") + "');";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Profilo.LoadJumbotronFO::errore::", ex);
                return sScript;
            }
        }
        /// <summary>
        /// 
        /// </summary>

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListComunicazioni"></param>
        /// <returns></returns>
        public bool LoadNews(string IDEnte, int IDContribuente, out List<GenericCategory> ListComunicazioni)
        {
            ListComunicazioni = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetNews", "IDENTE"
                        , "IDCONTRIBUENTE");
                    ListComunicazioni = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Profilo.LoadNews::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <param name="myContatti"></param>
        /// <returns></returns>
        public DettaglioAnagrafica LoadAnagrafica(int IDIstanza, ref ContattoAnag myContatti)
        {
            DettaglioAnagrafica myAnag = new DettaglioAnagrafica();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_ANAGRAFICA_S", "IDISTANZA");
                    myAnag = ctx.ContextDB.Database.SqlQuery<DettaglioAnagrafica>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)).FirstOrDefault<DettaglioAnagrafica>();
                    sSQL = ctx.GetSQL("prc_INDIRIZZISPEDIZIONE_S", "IDISTANZA");
                    myAnag.ListSpedizioni = ctx.ContextDB.Database.SqlQuery<ObjIndirizziSpedizione>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)).ToList<ObjIndirizziSpedizione>();
                    sSQL = ctx.GetSQL("prc_CONTATTI_S", "IDISTANZA");
                    myContatti= ctx.ContextDB.Database.SqlQuery<ContattoAnag>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)).FirstOrDefault<ContattoAnag>();
                    ctx.Dispose();
                }
                return myAnag;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Profilo.LoadAnagrafica::errore::", ex);
                return myAnag;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAnagrafica"></param>
        /// <param name="IDUser"></param>
        /// <param name="ScriptError"></param>
        /// <returns></returns>
        public long Save(DettaglioAnagrafica myAnagrafica, string IDUser, out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                GestioneAnagrafica fncAnag = new GestioneAnagrafica();
                long IdContribuente = 0;

                if ((myAnagrafica.COD_CONTRIBUENTE <= 0))
                {
                    IdContribuente = fncAnag.SetAnagraficaCompleta(myAnagrafica, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica);
                }
                else {
                    DettaglioAnagraficaReturn oMyAnag = new DettaglioAnagraficaReturn();
                    oMyAnag = fncAnag.GestisciAnagrafica(myAnagrafica, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false, true);
                    IdContribuente = long.Parse(oMyAnag.COD_CONTRIBUENTE);
                }

                Istanza myIstanza = new Istanza();
                myIstanza.DataPresentazione = myIstanza.DataInvioDichiarazione = DateTime.Now;
                myIstanza.DataInCarico = DateTime.MaxValue;
                myIstanza.DataRespinta = DateTime.MaxValue;
                myIstanza.DataValidata = DateTime.MaxValue;
                myIstanza.IDContribuente = int.Parse(IdContribuente.ToString());
                myIstanza.IDEnte = myAnagrafica.CodEnte;
                myIstanza.IDIstanza = -1;
                myIstanza.IDStato = Istanza.STATO.Presentata;
                List<GenericCategory> ListMyData = new List<GenericCategory>();

                ListMyData = new BLL.Settings().LoadTipoIstanze(string.Empty, Istanza.TIPO.Anagrafica, false);
                foreach (GenericCategory myTipo in ListMyData)
                {
                    myIstanza.IDTipo = myTipo.ID;
                }
                myIstanza.IDTributo = string.Empty;
                myIstanza.Note = "";
                if (!new Istanze(myIstanza, IDUser).Save())
                {
                    if (!SaveAnagrafica(myIstanza.IDIstanza, myAnagrafica))
                    {
                        ScriptError = "alert('Errore in salvataggio istanza di variazione anagrafica!');";
                        return -1;
                    }
                }
                else
                {
                    if (!SaveAnagrafica(myIstanza.IDIstanza, myAnagrafica))
                    {
                        ScriptError = "alert('Errore in salvataggio istanza di variazione anagrafica!');";
                        return -1;
                    }
                }

                return IdContribuente;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Profilo.Save::errore::", ex);
                ScriptError = "alert('Errore in salvataggio!');";
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdIstanza"></param>
        /// <param name="myAnagrafica"></param>
        /// <returns></returns>
        public bool SaveAnagrafica(int IdIstanza, DettaglioAnagrafica myAnagrafica)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSPC_ANAGRAFICA_IU", "IDISTANZA"
                            , "COD_CONTRIBUENTE"
                        );
                    int retVal = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDISTANZA", IdIstanza)
                            , ctx.GetParam("COD_CONTRIBUENTE", myAnagrafica.COD_CONTRIBUENTE)
                        ).First<int>();

                    sSQL = ctx.GetSQL("prc_TBLSPC_INDIRIZZISPEDIZIONE_IU", "IDISTANZA"
                            , "COD_CONTRIBUENTE"
                        );
                    retVal = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDISTANZA", IdIstanza)
                            , ctx.GetParam("COD_CONTRIBUENTE", myAnagrafica.COD_CONTRIBUENTE)
                        ).First<int>();

                    sSQL = ctx.GetSQL("prc_TBLSPC_CONTATTI_IU", "IDISTANZA"
                            , "COD_CONTRIBUENTE"
                        );
                    retVal = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDISTANZA", IdIstanza)
                            , ctx.GetParam("COD_CONTRIBUENTE", myAnagrafica.COD_CONTRIBUENTE)
                        ).First<int>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.Profilo.SaveAnagrafica::errore::", ex);
                return false;
            }
        }
    }
}