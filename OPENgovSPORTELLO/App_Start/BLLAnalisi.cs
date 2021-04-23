using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe per la gestione delle analisi di cruscotto BackOffice.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    /// <revisionHistory>
    /// <revision date="06/03/2019">
    /// Cancellazione registrazioni parziali
    /// </revision>
    /// </revisionHistory>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class Analisi
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Analisi));
        /// <summary>
        /// 
        /// </summary>
        public enum TypeGestUser
        {
            GestPWD
        , UtentiNonConfermati
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenericCategory> LoadTipoEvento()
        {
            try
            {
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTipoEvento");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadTipoEvento::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenericCategory> LoadPeriodo()
        {
            try
            {
                List<GenericCategory> ListMyData = new List<GenericCategory>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTipoPeriodoAnalisi");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadPeriodo::errore::", ex);
                return null;
            }
        }
        #region"Analisi Istanze FO"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="TipoAccesso"></param>
        /// <param name="HasTributo"></param>
        /// <returns></returns>
        public List<Eventi> LoadEventi(string IdEnte, DateTime Dal, DateTime Al, int TipoAccesso, bool HasTributo)
        {
            try
            {
                List<Eventi> ListMyData = new List<Eventi>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnalisiEventi", "IDENTE"
                        , "DAL"
                        , "AL"
                        , "TIPOACCESSO"
                        , "SUTRIBUTO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Eventi>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("TIPOACCESSO", TipoAccesso)
                            , ctx.GetParam("SUTRIBUTO", (HasTributo?1:0))
                        ).ToList<Eventi>();
                    ctx.Dispose();
                }
                Log.Debug("ho " + ListMyData.Count.ToString()+" righe");
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadEventi::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// Funzione per l'estrazione dell'analisi istanze analitica
        /// </summary>
        /// <param name="IdEnte">string codice ente</param>
        /// <param name="Dal">datetime data inizio</param>
        /// <param name="Al">datetime data fine</param>
        /// <param name="TipoAccesso">int tipo istanza</param>
        /// <param name="myTypeObj">object tipo di oggetto da popolare</param>
        /// <param name="ListMyData">out List<object> record che rispettano i criteri di selezione</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        public bool LoadEventiAnalitica(string IdEnte, DateTime Dal, DateTime Al, int TipoAccesso, object myTypeObj, out List<object> ListMyData)
        {
                ListMyData = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnalisiEventiAnalitica", "IDENTE"
                        , "DAL"
                        , "AL"
                        , "TIPOACCESSO");
                    if (myTypeObj.GetType() == typeof(EventiAnalitica))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<EventiAnalitica>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("TIPOACCESSO", TipoAccesso)
                                ).ToList<EventiAnalitica>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<EventiAnaliticaStampa>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("TIPOACCESSO", TipoAccesso)
                                ).ToList<EventiAnaliticaStampa>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadEventi::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="TipoAccesso"></param>
        /// <param name="TipoPeriodo"></param>
        /// <param name="NPeriodo"></param>
        /// <returns></returns>
        public List<EventiRaffronto> LoadEventiRaffronto(string IdEnte, DateTime Dal, DateTime Al, int TipoAccesso, int TipoPeriodo, int NPeriodo)
        {
            try
            {
                List<EventiRaffronto> ListMyData = new List<EventiRaffronto>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnalisiEventiRaffronto", "IDENTE"
                        , "DAL"
                        , "AL"
                        , "TIPOACCESSO"
                        ,"TIPOPERIODO"
                        ,"NPERIODO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<EventiRaffronto>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("TIPOACCESSO", TipoAccesso)
                            , ctx.GetParam("TIPOPERIODO", TipoPeriodo)
                            , ctx.GetParam("NPERIODO", NPeriodo)
                        ).ToList<EventiRaffronto>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadEventiRaffronto::errore::", ex);
                return null;
            }
        }
        #endregion
        #region"Analisi Istanze BO"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="IsRaffronto"></param>
        /// <returns></returns>
        public List<Attivita> LoadAttivita(string IdEnte, DateTime Dal, DateTime Al, bool IsRaffronto)
        {
            try
            {
                List<Attivita> ListMyData = new List<Attivita>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnalisiStatoIstanze", "IDENTE"
                        , "DAL"
                        , "AL"
                        , "ISRAFFRONTO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Attivita>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("ISRAFFRONTO", IsRaffronto)
                        ).ToList<Attivita>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadAttivita::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="IsRaffronto"></param>
        /// <returns></returns>
        public List<TempiMedi> LoadTempiMedi(string IdEnte, DateTime Dal, DateTime Al, bool IsRaffronto)
        {
            try
            {
                List<TempiMedi> ListMyData = new List<TempiMedi>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnalisiTempiMedi", "IDENTE"
                        , "DAL"
                        , "AL"
                        , "ISRAFFRONTO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<TempiMedi>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal)
                            , ctx.GetParam("AL", Al)
                            , ctx.GetParam("ISRAFFRONTO", IsRaffronto)
                        ).ToList<TempiMedi>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadTempiMedi::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <returns></returns>
        public List<AttivitaAnalitica> LoadAttivitaAnalitica(string IdEnte, DateTime Dal, DateTime Al)
        {
            try
            {
                List<AttivitaAnalitica> ListMyData = new List<AttivitaAnalitica>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnalisiStatoIstanzeAnalitica", "IDENTE"
                        , "DAL"
                        , "AL");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<AttivitaAnalitica>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                        ).ToList<AttivitaAnalitica>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadAttivitAnalitica::errore::", ex);
                return null;
            }
        }
        #endregion
        #region "Dovuto VS Versato"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Anno"></param>
        /// <returns></returns>
        public List<DovutoVSVersato> LoadRaffrontoDovutoVersato(string IdEnte, int Anno)
        {
            try
            {
                List<DovutoVSVersato> ListMyData = new List<DovutoVSVersato>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDovutoVSVersato", "IDENTE"
                            , "ANNO"
                        );
                    ListMyData = ctx.ContextDB.Database.SqlQuery<DovutoVSVersato>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("ANNO", Anno)
                        ).ToList<DovutoVSVersato>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadDovutoVSVersato::errore::", ex);
                return null;
            }
        }
        #endregion
        #region "Tempi di pagamento"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="DataEmissione"></param>
        /// <param name="IdTributo"></param>
        /// <param name="Scadenza"></param>
        /// <returns></returns>
        public List<TempiPagamento> LoadTempiPagamento(string IdEnte, DateTime DataEmissione, string IdTributo, string Scadenza)
        {
            try
            {
                List<TempiPagamento> ListMyData = new List<TempiPagamento>();

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTempiPagamento", "IDENTE"
                            , "DATAEMISSIONE"
                            , "TRIBUTO"
                            ,"SCADENZA"
                        );
                    ListMyData = ctx.ContextDB.Database.SqlQuery<TempiPagamento>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DATAEMISSIONE", DataEmissione)
                            , ctx.GetParam("TRIBUTO",IdTributo)
                            , ctx.GetParam("SCADENZA", Scadenza)
                        ).ToList<TempiPagamento>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadTempiPagamento::errore::", ex);
                return null;
            }
        }
        #endregion
        #region "Comunicazioni BO vs FO"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Dal"></param>
        /// <param name="Al"></param>
        /// <param name="TipoAccesso"></param>
        /// <param name="Operatore"></param>
        /// <param name="CodFiscalePIVA"></param>
        /// <returns></returns>
        public List<ComunicazioniBOvsFO> LoadComunicazioniBOvsFO(string IdEnte, DateTime Dal, DateTime Al, int TipoAccesso, string Operatore, string CodFiscalePIVA)
        {
            try
            {
                List<ComunicazioniBOvsFO> ListMyData = new List<ComunicazioniBOvsFO>();
                Log.Debug("OPENgovSPORTELLO.Cruscotto.BO_ComunicazioniBOvsFO.prc_GetComunicazioniBOvsFO:" + IdEnte.ToString() + "-" + Dal.Date.ToString() + "-" + Al.Date.ToString() + "-" + TipoAccesso.ToString() + "-" + Operatore.ToString() + "-" + CodFiscalePIVA.ToString());
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetComunicazioniBOvsFO", "IDENTE"
                        , "DAL"
                        , "AL"
                        , "TIPOACCESSO"
                        , "OPERATORE"
                        , "CFPIVA"
                        );
                    ListMyData = ctx.ContextDB.Database.SqlQuery<ComunicazioniBOvsFO>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DAL", Dal.Date)
                            , ctx.GetParam("AL", Al.Date)
                            , ctx.GetParam("TIPOACCESSO", TipoAccesso)
                            , ctx.GetParam("OPERATORE", Operatore)
                            , ctx.GetParam("CFPIVA", CodFiscalePIVA)
                        ).ToList<ComunicazioniBOvsFO>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadComunicazioniBOvsFO::errore::", ex);
                return null;
            }
        }
        #endregion
        #region "Password via mail+via posta"
        /// <summary>
        /// Elenco password da inviare
        /// </summary>
        /// <param name="myTypeObj">object tipo di oggetto da popolare</param>
        /// <param name="ListMyData">out List<object> record che rispettano i criteri di selezione</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        /// <returns>Lista di tipo UserRole</returns>
        /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
        /// <revisionHistory>
        /// <revision date="05/04/2019">
        /// <strong>Sportello_GestioneRegistrazioni.docx</strong>
        /// </revision>
        /// </revisionHistory>
        public bool LoadGestPWD(object myTypeObj, out List<object> ListMyData)
        {
            ListMyData = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetPWDToSend");
                    if (myTypeObj.GetType() == typeof(GestPWD))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<GestPWD>(sSQL).ToList<GestPWD>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<GestPWDStampa>(sSQL).ToList<GestPWDStampa>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadGestPWD::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sIDUtente"></param>
        /// <returns></returns>
        public bool SetSendPWD(string sIDUtente)
        {
            bool retVal = false;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_SetPWDSend", "IDUSER");
                    if (ctx.ContextDB.Database.SqlQuery<int>(sSQL
                        , ctx.GetParam("IDUSER", sIDUtente)
                        ).FirstOrDefault<int>() <= 0)
                        retVal= false;
                    else
                        retVal= true;
                    ctx.Dispose();
                }
                return retVal;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.SetSendPWD::errore::", ex);
                return false;
            }
        }
        #endregion
        #region "Utenti non confermati"
        /// <summary>
        /// Elenco degli utenti registrati ma non confermati 
        /// </summary>
        /// <param name="myTypeObj">object tipo di oggetto da popolare</param>
        /// <param name="ListMyData">out List<object> record che rispettano i criteri di selezione</param>
        /// <returns>bool false in caso di errore altrimenti true</returns>
        /// <returns>Lista di tipo UserRole</returns>
        /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
        /// <revisionHistory>
        /// <revision date="05/04/2019">
        /// <strong>Sportello_GestioneRegistrazioni.docx</strong>
        /// </revision>
        /// </revisionHistory>
        public bool LoadUserNoConfirmed(object myTypeObj, out List<object> ListMyData)
        {
            ListMyData = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                     string sSQL = ctx.GetSQL("prc_GetUserNoConfirmed");
                   if (myTypeObj.GetType() == typeof(UserRole))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<UserRole>(sSQL).ToList<UserRole>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<UserRoleStampa>(sSQL).ToList<UserRoleStampa>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                   Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadUserNoConfirmed::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// Cancellazione di un utente
        /// </summary>
        /// <param name="IdUser">Id da cancellare</param>
        /// <returns>1 a buon fine, altrimenti 0</returns>
        public int DeleteUserNoConfirmed(string IdUser)
        {
            int myRet = 0;
            try
            {
                if (IdUser!=string.Empty)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_UserNoConfirmed_D", "ID");
                        myRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", IdUser)).First<int>();
                        ctx.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.DeleteUserNoConfirmed::errore::", ex);
                myRet=0;
            }
            return myRet;
        }
        #endregion
        #region "Cartella Unica"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tributo"></param>
        /// <param name="TypeReport"></param>
        /// <param name="myTypeObj"></param>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListMyData"></param>
        /// <returns></returns>
        public bool LoadCartellaUnica(string Tributo,string TypeReport, object myTypeObj, string IDEnte, int IDContribuente, out List<object> ListMyData)
        {
            ListMyData = new List<object>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_Get"+ TypeReport, "IDENTE"
                            , "IDCONTRIBUENTE"
                            , "IDTRIBUTO"
                        );
                    if (myTypeObj.GetType() == typeof(ReportDich8852))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDich8852>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDich8852>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDovuto8852))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDovuto8852>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDovuto8852>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportPagato8852))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportPagato8852>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportPagato8852>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDich8852))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDich8852>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDich8852>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDovuto8852))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDovuto8852>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDovuto8852>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportPagato8852))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportPagato8852>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportPagato8852>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDich0434))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDich0434>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDich0434>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDovuto0434))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDovuto0434>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDovuto0434>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportPagato0434))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportPagato0434>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportPagato0434>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDich0453))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDich0453>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDich0453>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportDovuto0434))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportDovuto0434>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportDovuto0434>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else if (myTypeObj.GetType() == typeof(ReportPagato0434))
                    {
                        ListMyData = ((ctx.ContextDB.Database.SqlQuery<ReportPagato0434>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                    , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                    , ctx.GetParam("IDTRIBUTO", Tributo)
                                ).ToList<ReportPagato0434>()) as IEnumerable<object>).Cast<object>().ToList();
                    }
                    else {
                        ListMyData = new List<object>();
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.LoadCartellaUnica::errore::", ex);
                return false;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="NumeroRata"></param>
        /// <param name="DataEmissione"></param>
        /// <param name="DataScadenza"></param>
        /// <param name="TipoPeriodo"></param>
        /// <param name="ContatoreAvvisi"></param>
        /// <param name="SommatoriaPagato"></param>
        /// <param name="CodTributo"></param>
        /// <returns></returns>
        public bool InsertTempiMedi(string IDEnte, string NumeroRata, DateTime DataEmissione, DateTime DataScadenza, int TipoPeriodo, int ContatoreAvvisi, double SommatoriaPagato, string CodTributo)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLTEMPIPAGAMENTO_IU", "IDENTE"
                        , "NUMERORATA", "DATAEMISSIONE", "DATASCADENZA", "TIPOPERIODO", "CONTATOREAVVISI", "SOMMATORIAPAGATO", "CODTRIBUTO");
                    int idRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL 
                            , ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("NUMERORATA", NumeroRata)
                            , ctx.GetParam("DATAEMISSIONE", DataEmissione)
                            , ctx.GetParam("DATASCADENZA", DataScadenza)
                            , ctx.GetParam("TIPOPERIODO", TipoPeriodo)
                            , ctx.GetParam("CONTATOREAVVISI", ContatoreAvvisi)
                            , ctx.GetParam("SOMMATORIAPAGATO", SommatoriaPagato)
                            , ctx.GetParam("CODTRIBUTO", CodTributo)
                        ).First<int>();
                    ctx.Dispose();
                    if (idRet <= 0)
                    {
                        return false;
                    }
                }
                Log.Info(SommatoriaPagato);
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.InsertTempiMedi::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <returns></returns>
        public bool ClearTBLTempiPagamento(string IdEnte)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_ClearTBLTEMPIPAGAMENTO", "IDENTE");
                    if (ctx.ContextDB.Database.SqlQuery<int>(sSQL
                        , ctx.GetParam("IDENTE", IdEnte)
                        ).FirstOrDefault<int>() <= 0)
                        ctx.Dispose();
                }
                return true;

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.ClearTBLTempiPagamento::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerName"></param>
        /// <returns></returns>
        public string GetHeaderXLS(string headerName)
        {
            string myRet = string.Empty;
            try
            {
               switch (headerName.ToUpper())
                {
                    case "CODFISCALEPIVA":
                    case "CODICEFISCALE":
                    case "CFPIVA":
                        myRet = "Cod.Fiscale/P.Iva";
                        break;
                    case "TIPOACCESSO":                    
                        myRet = "Tipo Istanza";
                        break;
                    case "LASTPASSWORDCHANGEDDATE":
                        myRet = "Data";
                        break;
                    default:
                        myRet = headerName;
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Analisi.GetHeaderXLS::errore::", ex);
                myRet=string.Empty;
            }
            return myRet;
        }
    }
}