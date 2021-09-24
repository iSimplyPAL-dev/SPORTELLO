using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using OPENgovSPORTELLO.Models;
using log4net;
using System.Data.SqlClient;
using Microsoft.AspNet.Identity;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe di gestione parametri configurazioni
    /// </summary>
    public class Settings
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Settings));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isForTempiMedi"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadTributi(int isForTempiMedi)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTributi", "ISFORTEMPIMEDI");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("ISFORTEMPIMEDI", isForTempiMedi)).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTributi::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tributo"></param>
        /// <param name="Ente"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadDataEmissione(string Tributo, string Ente)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDataEmissione", "TRIBUTO", "IDENTE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, 
                        ctx.GetParam("TRIBUTO", Tributo),
                        ctx.GetParam("IDENTE", Ente)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTributi::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Operatore"></param>
        /// <returns></returns>
        public List<string> LoadTributiGestiti(string Operatore)
        {
            List<string> ListMyData = new List<string>();
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTributiGestiti", "OPERATORE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("OPERATORE", Operatore)).ToList<string>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTributiGestiti::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenericCategory> LoadProfili()
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {               
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTipoProfili");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadProfili::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdTributo"></param>
        /// <param name="sDescrizione"></param>
        /// <param name="IsForDLL"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadTipoIstanze(string IdTributo, string sDescrizione, bool IsForDLL)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTipoIstanze", "IDTRIBUTO", "DESCRIZIONE", "ISFORDLL");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDTRIBUTO", IdTributo)
                            , ctx.GetParam("DESCRIZIONE", sDescrizione)
                            , ctx.GetParam("ISFORDLL", IsForDLL)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTipoIstanze::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenericCategory> LoadStatoIstanze()
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetStatoIstanze");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadStatoIstanze::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Anno"></param>
        /// <param name="Type"></param>
        /// <param name="Descr"></param>
        /// <param name="IDEsterno"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadConfig(string IdEnte, int Anno, string Type, string Descr, string IDEsterno)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetConfigurazioni", "IDENTE"
                        , "TIPO"
                        , "ANNO"
                        , "VALORE"
                        , "ISFORDDL"
                        , "TRIBUTO"
                        , "IDESTERNO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("TIPO", Type)
                            , ctx.GetParam("ANNO", Anno)
                            , ctx.GetParam("VALORE", Descr)
                            , ctx.GetParam("ISFORDDL", 0)
                            , ctx.GetParam("TRIBUTO", string.Empty)
                            , ctx.GetParam("IDESTERNO", IDEsterno)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadConfig::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Anno"></param>
        /// <param name="Type"></param>
        /// <param name="Descr"></param>
        /// <param name="IDTributo"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadConfigForDDL(string IdEnte, int Anno, string Type, string Descr, string IDTributo)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
                        try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetConfigurazioni", "IDENTE"
                        , "TIPO"
                        , "ANNO"
                        , "VALORE"
                        , "ISFORDDL"
                        , "TRIBUTO"
                        , "IDESTERNO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("TIPO", Type)
                            , ctx.GetParam("ANNO", Anno)
                            , ctx.GetParam("VALORE", Descr)
                            , ctx.GetParam("ISFORDDL", 1)
                            , ctx.GetParam("TRIBUTO", IDTributo)
                            , ctx.GetParam("IDESTERNO", string.Empty)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadConfigForDDL::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Anno"></param>
        /// <param name="Type"></param>
        /// <param name="Descr"></param>
        /// <param name="IDEsterno"></param>
        /// <returns></returns>
        public List<GenericCategoryWithRate> LoadTariffe(string IdEnte, int Anno, string Type, string Descr, string IDEsterno)
        {
            List<GenericCategoryWithRate> ListMyData = new List<GenericCategoryWithRate>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetConfigurazioni", "IDENTE"
                            , "TIPO"
                            , "ANNO"
                            , "VALORE"
                            , "ISFORDDL"
                            , "TRIBUTO"
                            , "IDESTERNO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategoryWithRate>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("TIPO", Type)
                            , ctx.GetParam("ANNO", Anno)
                            , ctx.GetParam("VALORE", Descr)
                            , ctx.GetParam("ISFORDDL", 0)
                            , ctx.GetParam("TRIBUTO", string.Empty)
                            , ctx.GetParam("IDESTERNO", IDEsterno)
                        ).ToList<GenericCategoryWithRate>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTariffe::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myIdEnte"></param>
        /// <param name="myComune"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadEnti(string myIdEnte, string myComune)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEnti", "IDENTE", "COMUNE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", myIdEnte)
                            , ctx.GetParam("COMUNE", myComune)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadEnti::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamSearch"></param>
        /// <returns></returns>
        public List<Comuni> LoadComuni(string ParamSearch)
        {
            List<Comuni> ListMyData = new List<Comuni>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetComuni", "COMUNE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Comuni>(sSQL, ctx.GetParam("COMUNE", ParamSearch)).ToList<Comuni>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadComuni::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="ParamSearch"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadStradario(string IdEnte, string ParamSearch)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetStradario", "IDENTE", "VIA");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL
                            , ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("VIA", ParamSearch)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadComuni::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ParamSearch"></param>
        /// <param name="CFPIVA"></param>
        /// <param name="filtro"></param>
        /// <param name="IdEnte"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<UserRole> LoadUserRole(string ParamSearch, string CFPIVA, bool filtro, string IdEnte, string username)
        {
            List<UserRole> ListMyData = new List<UserRole>();
            try
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadUserRole::entro per operatore->" + ParamSearch + ", cfpiva->" + CFPIVA + ", filtro->" + ((filtro) ? "1" : "0") + ", idEnte->" + IdEnte);
                using (DBModel ctx = new DBModel())
                {
                    Log.Debug("prc_GetAutorizzazioni " + ParamSearch + "," + ((filtro) ? 1 : 0).ToString() + "," + CFPIVA + "," + IdEnte);
                    string sSQL = ctx.GetSQL("prc_GetAutorizzazioni", "OPERATORE", "FILTRO", "CFPIVA", "IDENTE", "USERNAME");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<UserRole>(sSQL, ctx.GetParam("OPERATORE", ParamSearch)
                                , ctx.GetParam("FILTRO", ((filtro) ? 1 : 0))
                                , ctx.GetParam("CFPIVA", CFPIVA)
                                , ctx.GetParam("IDENTE", IdEnte)
                                , ctx.GetParam("USERNAME", username)
                            ).ToList<UserRole>();
                    foreach (UserRole myItem in ListMyData)
                    {
                        Log.Debug("LoadUserRole myItem.ListDeleganti " + myItem.ListDeleganti + ", '', 1");
                        Log.Debug("LoadUserRoleprc_GetUserEnti " + myItem.ID + ", '', 1");
                        sSQL = ctx.GetSQL("prc_GetUserEnti", "IDUSER", "USERNAME", "ONLYCOD");
                        myItem.Enti = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("IDUSER", myItem.ID)
                                , ctx.GetParam("USERNAME", string.Empty)
                                , ctx.GetParam("ONLYCOD", 1)
                            ).ToList<string>();
                        Log.Debug("prc_GetUserTributi " + myItem.ID + ", 1");
                        sSQL = ctx.GetSQL("prc_GetUserTributi", "IDUSER"
                            , "ONLYCOD");
                        myItem.Tributi = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("IDUSER", myItem.ID)
                                , ctx.GetParam("ONLYCOD", 1)
                            ).ToList<string>();
                    }
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadUserRole::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<GenericCategory> LoadTARSUScopeCat()
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_ScopeCat");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTARSUScopeCat::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="TypeSearch"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadTARSUCat(string IdEnte, string TypeSearch)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTARSU_Categorie", "IDENTE", "TYPE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("TYPE", TypeSearch)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadTARSUCat::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="Anno"></param>
        /// <param name="IdTributo"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadScadenze(string IdEnte, int Anno, string IdTributo)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {             
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetScadenze", "IDENTE", "ANNO", "TRIBUTO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("ANNO", Anno)
                            , ctx.GetParam("TRIBUTO", IdTributo)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadScadenze::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="DataEmissione"></param>
        /// <param name="IdTributo"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadScadenzeTempiPagamento(string IdEnte, DateTime DataEmissione, string IdTributo)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetScadenzeTempiPagamento", "IDENTE", "DATAEMISSIONE", "TRIBUTO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("DATAEMISSIONE", DataEmissione)
                            , ctx.GetParam("TRIBUTO", IdTributo)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadScadenzeTempiPgamento::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="myTypeDestinatari"></param>
        /// <param name="myCognome"></param>
        /// <param name="myNome"></param>
        /// <param name="myCFPIVA"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadSubsetRecipient(string myEnte, int myTypeDestinatari, string myCognome, string myNome, string myCFPIVA)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetMessageSubsetRecipient", "IDENTE", "IDTYPEDESTINATARI"
                            , "COGNOME", "NOME", "CFPIVA"
                        );
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", myEnte)
                            , ctx.GetParam("IDTYPEDESTINATARI", myTypeDestinatari)
                            , ctx.GetParam("COGNOME", myCognome)
                            , ctx.GetParam("NOME", myNome)
                            , ctx.GetParam("CFPIVA", myCFPIVA)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadSubsetRecipient::errore::", ex);
                return ListMyData;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idEnte"></param>
        /// <returns></returns>
        public List<GenericCategory> LoadFornitori(string idEnte)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTipoFornitori", "IDENTE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", idEnte)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadFornitori::errore::", ex);
                return ListMyData;
            }
        }
        #region "leggo Dati da Griglia"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <returns></returns>
        public EntiInLavorazione LoadEntiFromGrd(GridViewRow myRow)
        {
            EntiInLavorazione myItem = new EntiInLavorazione();
            try
            {
                myItem.IDEnte = ((TextBox)myRow.FindControl("txtCodice")).Text;
                myItem.ID = int.Parse(((HiddenField)myRow.FindControl("hfIdEnte")).Value.ToString());                

                myItem.Descrizione = ((TextBox)myRow.FindControl("txtEnte")).Text;
                myItem.Ambiente = ((TextBox)myRow.FindControl("txtAmbiente")).Text;
                myItem.SplitPWD = ((CheckBox)myRow.FindControl("chkSplitPWD")).Checked ? 1 : 0;
                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadEntiFromGrd::errore::", ex);
                return myItem;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <param name="myEnte"></param>
        /// <param name="myTipo"></param>
        /// <param name="myTributo"></param>
        /// <param name="myAnno"></param>
        /// <returns></returns>
        public GenericCategory LoadGenericCatFromGrd(GridViewRow myRow, string myEnte, string myTipo, string myTributo, int myAnno)
        {
            GenericCategory myItem = new GenericCategory();
            try
            {               
                myItem.IDEnte = myEnte;
                myItem.IDTipo = myTipo;
                myItem.IDTributo = myTributo;
                myItem.Anno = myAnno;
                myItem.ID = int.Parse(((HiddenField)myRow.Cells[0].FindControl("hfIdSetting")).Value.ToString());
                myItem.Codice = ((TextBox)myRow.Cells[0].FindControl("txtCodice")).Text;
                myItem.Descrizione = ((TextBox)myRow.Cells[1].FindControl("txtDescrizione")).Text;
                myItem.IDOrg = ((TextBox)myRow.Cells[2].FindControl("txtIdOrg")).Text;

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadGenericCatFromGrd::errore::", ex);
                return myItem;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <returns></returns>
        public UserRole LoadUserFromGrd(GridViewRow myRow)
        {
            UserRole myItem = new UserRole();
            try
            {                
                myItem.ID = ((HiddenField)myRow.Cells[0].FindControl("hfIdRow")).Value.ToString();
                myItem.NameUser = ((TextBox)myRow.Cells[0].FindControl("txtOperatore")).Text;
                myItem.IDTipoProfilo = int.Parse(((DropDownList)myRow.Cells[1].FindControl("ddlProfilo")).SelectedValue);
                myItem.Enti = ((HiddenField)myRow.Cells[2].FindControl("hfEnti")).Value.Split(char.Parse("|")).ToList();
                List<string> myList = new List<string>();
                foreach (string myDetail in myItem.Enti)
                {
                    if (myDetail != "")
                        myList.Add(myDetail);
                }
                myItem.Enti = myList;
                myItem.Tributi = ((HiddenField)myRow.Cells[3].FindControl("hfTributi")).Value.Split(char.Parse("|")).ToList();
                myList = new List<string>();
                foreach (string myDetail in myItem.Tributi)
                {
                    if (myDetail != "")
                        myList.Add(myDetail);
                }
                myItem.Tributi = myList;

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadUserFromGrd::errore::", ex);
                return myItem;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <param name="myEnte"></param>
        /// <returns></returns>
        public DocToAttach LoadDocFromGrd(GridViewRow myRow, string myEnte)
        {
            DocToAttach myItem = new DocToAttach();
            try
            {
                               myItem.ID = int.Parse(((HiddenField)myRow.Cells[0].FindControl("hfIdRow")).Value.ToString());
                myItem.IDEnte = myEnte;
                myItem.IDTributo = ((DropDownList)myRow.Cells[0].FindControl("ddlTributo")).SelectedValue;
                myItem.IDTipoIstanza = int.Parse(((DropDownList)myRow.Cells[1].FindControl("ddlIstanze")).SelectedValue);
                myItem.Documento = ((TextBox)myRow.Cells[2].FindControl("txtDoc")).Text;
                myItem.IsObbligatorio = ((CheckBox)myRow.Cells[3].FindControl("chkIsConstraint")).Checked;

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadDocFromGrd::errore::", ex);
                return myItem;
            }
        }
/// <summary>
/// 
/// </summary>
/// <param name="idEnte"></param>
/// <param name="descrizione"></param>
/// <param name="idType"></param>
/// <returns></returns>
        public List<idCodice> getCodiceCopyTo(string idEnte, string descrizione, string idType)
        {
            try
            {
                List <idCodice> ListMyData = new List<idCodice>();
                 using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_getCodiceCopyTo", "IDENTE", "DESCRIZIONE", "IDTYPE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<idCodice>(sSQL, ctx.GetParam("IDENTE", idEnte)
                            , ctx.GetParam("DESCRIZIONE", descrizione)
                            , ctx.GetParam("IDTYPE", idType)
                        ).ToList<idCodice>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.getCodiceCopyTo::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myRow"></param>
        /// <param name="myEnte"></param>
        /// <param name="myTipo"></param>
        /// <param name="myTributo"></param>
        /// <param name="myAnno"></param>
        /// <param name="isCopyTo"></param>
        /// <returns></returns>
        public GenericCategoryWithRate LoadGenericCatWithRateFromGrd(GridViewRow myRow, string myEnte, string myTipo, string myTributo, int myAnno, bool isCopyTo)
        {
            GenericCategoryWithRate myItem = new GenericCategoryWithRate();
            try
            {                
                myItem.IDEnte = myEnte;
                myItem.IDTipo = myTipo;
                myItem.IDTributo = myTributo;
                myItem.Anno = myAnno;
                myItem.ID = int.Parse(((HiddenField)myRow.FindControl("hfIdRow")).Value.ToString());
                if (myTipo == GenericCategory.TIPO.ICI_Aliquote || myTipo == GenericCategory.TIPO.TASI_Aliquote)
                {
                                        myItem.Codice = ((DropDownList)myRow.FindControl("ddlTipologia")).SelectedValue;
                    myItem.Descrizione = ((DropDownList)myRow.FindControl("ddlTipologia")).SelectedItem.Text;
                                    }
                else
                {
                    myItem.Codice = ((TextBox)myRow.FindControl("txtCodice")).Text;
                    myItem.Descrizione = ((TextBox)myRow.FindControl("txtDescrizione")).Text;
                }
                myItem.Valore = decimal.Parse(((TextBox)myRow.FindControl("txtValore")).Text.Replace(".", ","));
                if (((TextBox)myRow.FindControl("txtPercProprietario")).Text != string.Empty)
                    myItem.PercProprietario = decimal.Parse(((TextBox)myRow.FindControl("txtPercProprietario")).Text.Replace(".", ","));
                if (((TextBox)myRow.FindControl("txtPercInquilino")).Text != string.Empty)
                    myItem.PercInquilino = decimal.Parse(((TextBox)myRow.FindControl("txtPercInquilino")).Text.Replace(".", ","));
                myItem.IDOrg = ((TextBox)myRow.FindControl("txtIdOrg")).Text;

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.LoadGenericCatWithRateFromGrd::errore::", ex);
                return myItem;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EntePrinc"></param>
        /// <param name="AnnoPrinc"></param>
        /// <param name="IsCopy"></param>
        /// <param name="EnteDest"></param>
        /// <param name="AnnoDest"></param>
        /// <param name="TypeConfig"></param>
        /// <param name="ScriptError"></param>
        /// <returns></returns>
        public bool FieldValidator(string EntePrinc, string AnnoPrinc, string IsCopy, string EnteDest, string AnnoDest, string TypeConfig, out string ScriptError)
        {
            ScriptError = string.Empty;
            int myAnno = 0;
            try
            {
                if (TypeConfig != GenericCategory.TIPO.Enti && TypeConfig != GenericCategory.TIPO.Operatori)
                {
                    if (TypeConfig == GenericCategory.TIPO.Stradario || TypeConfig == GenericCategory.TIPO.Documenti
                        || TypeConfig == GenericCategory.TIPO.TARSU_Motivazioni || TypeConfig == GenericCategory.TIPO.TARSU_Vani || TypeConfig == GenericCategory.TIPO.TARSU_StatoOccupazione
                        || TypeConfig == GenericCategory.TIPO.ICI_Motivazioni || TypeConfig == GenericCategory.TIPO.ICI_Categorie || TypeConfig == GenericCategory.TIPO.ICI_Caratteristica || TypeConfig == GenericCategory.TIPO.ICI_Possesso 
                        || TypeConfig == GenericCategory.TIPO.TASI_Motivazioni
                        || TypeConfig == GenericCategory.TIPO.OSAP_Richiedente || TypeConfig == GenericCategory.TIPO.OSAP_Motivazioni || TypeConfig == GenericCategory.TIPO.OSAP_Categoria || TypeConfig == GenericCategory.TIPO.OSAP_Occupazione
                        || TypeConfig == GenericCategory.TIPO.ICP_Motivazioni)
                    {
                        if (EntePrinc == "")
                        {
                            ScriptError += "alert('Compilare ente!');";
                            return false;
                        }
                    }
                    else {
                        if (EntePrinc == "" || AnnoPrinc == "")
                        {
                            ScriptError += "alert('Compilare sia ente che anno!');";
                            return false;
                        }
                    }
                    try
                    {
                        int.TryParse(AnnoPrinc, out myAnno);
                    }
                    catch
                    {
                        ScriptError += "alert('Anno non valido!');";
                        return false;
                    }
                    if (IsCopy == "1")
                    {
                        if (TypeConfig == GenericCategory.TIPO.Stradario)
                        {
                            ScriptError += "alert('Ribaltamento non permesso per lo stradario!');";
                            return false;
                        }
                        else {
                            if (TypeConfig == GenericCategory.TIPO.Stradario || TypeConfig == GenericCategory.TIPO.Documenti
                        || TypeConfig == GenericCategory.TIPO.TARSU_Motivazioni || TypeConfig == GenericCategory.TIPO.TARSU_Vani || TypeConfig == GenericCategory.TIPO.TARSU_StatoOccupazione
                        || TypeConfig == GenericCategory.TIPO.ICI_Motivazioni || TypeConfig == GenericCategory.TIPO.ICI_Categorie || TypeConfig == GenericCategory.TIPO.ICI_Caratteristica && TypeConfig != GenericCategory.TIPO.ICI_Possesso
                        || TypeConfig == GenericCategory.TIPO.TASI_Motivazioni
                        || TypeConfig == GenericCategory.TIPO.OSAP_Richiedente || TypeConfig == GenericCategory.TIPO.OSAP_Motivazioni || TypeConfig == GenericCategory.TIPO.OSAP_Categoria || TypeConfig == GenericCategory.TIPO.OSAP_Occupazione
                        || TypeConfig == GenericCategory.TIPO.ICP_Motivazioni)
                            {
                                if (EnteDest == "")
                                {
                                    ScriptError += "alert('Compilare ente su cui ribaltare i dati!');";
                                }
                            }
                            else {
                                if (EnteDest == "" || AnnoDest == "")
                                {
                                    ScriptError += "alert('Compilare sia ente che anno su cui ribaltare i dati!');";
                                }
                                try
                                {
                                    int.TryParse(AnnoDest, out myAnno);
                                }
                                catch
                                {
                                    ScriptError += "alert('Anno su cui ribaltare non valido!');";
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.FieldValidator::errore::", ex);
                return false;
            }
        }
        
        /// <summary>
        /// funzione creata per effettuare update del codice nella tabella settings per l'idtype='67', per caricare correttamente le DDL è infatti necessario che il campo codice della riga con
        ///idtype='67' corrisponda all'ID della riga con idtype='60' dell'ente interessato, in fase di copyto (ribaltamento) viene copiato e mantenuto il codice che fa riferimento all'ID dell'ente di origine
        /// </summary>
        /// <param name="idente"></param>
        /// <param name="anno"></param>
        /// <returns></returns>
        public bool updateCodice(string idente, int anno)
        {
            bool retVal = false;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_updateCodice", "IDENTE", "ANNO");
                    if (ctx.ContextDB.Database.SqlQuery<int>(sSQL
                        , ctx.GetParam("IDENTE", idente)
                        , ctx.GetParam("ANNO", anno)
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
                Log.Debug("OPENgovSPORTELLO.BLLSettings::updateCodice", ex);
                return false;
            }
        }
        /// <summary>
        /// funzione che ribalta il valore delle aliquote, la eseguo subito dopo updateCodice
        /// </summary>
        /// <param name="idente"></param>
        /// <param name="anno"></param>
        /// <param name="identeFrom"></param>
        /// <param name="annoFrom"></param>
        /// <returns></returns>
        public bool updateValore(string idente, int anno, string identeFrom, int annoFrom)
        {
            bool retVal = false;
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_updateCodice", "IDENTE", "ANNO");
                    if (ctx.ContextDB.Database.SqlQuery<int>(sSQL
                        , ctx.GetParam("IDENTE", idente)
                        , ctx.GetParam("ANNO", anno)
                        , ctx.GetParam("IDENTEFROM", identeFrom)
                        , ctx.GetParam("ANNOFROM", annoFrom)
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
                Log.Debug("OPENgovSPORTELLO.BLLSettings::updateValore", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myGrd"></param>
        /// <param name="ListEnti"></param>
        /// <param name="myEnte"></param>
        /// <param name="myTipo"></param>
        /// <param name="myTributo"></param>
        /// <param name="myAnno"></param>
        /// <param name="myIsCopyTo"></param>
        /// <param name="myEnteDest"></param>
        /// <param name="myAnnoDest"></param>
        /// <param name="ScriptError"></param>
        /// <returns></returns>
        public bool Save(Ribes.OPENgov.WebControls.RibesGridView myGrd, List<EntiInLavorazione> ListEnti, string myEnte, string myTipo, string myTributo, int myAnno, bool myIsCopyTo, string myEnteDest, int myAnnoDest, out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                if (myTipo == GenericCategory.TIPO.Enti)
                {

                    EntiInLavorazione myEnteGrd = new EntiInLavorazione();
                    foreach (EntiInLavorazione myItem in ListEnti)
                    {
                        if (myItem != null)
                        {
                            foreach (GridViewRow myRow in myGrd.Rows)
                            {
                                if (((TextBox)myRow.FindControl("txtCodice")).Text == myItem.IDEnte) { 
                                myEnteGrd = LoadEntiFromGrd(myRow);
                                //M.B.
                                myEnteGrd.DatiVerticali.TipoFornitore = myItem.DatiVerticali.TipoFornitore;
                            }
                            }
                            myItem.Descrizione = myEnteGrd.Descrizione;
                            myItem.Ambiente = myEnteGrd.Ambiente;
                            myItem.SplitPWD = myEnteGrd.SplitPWD;
                            if (!new EntiSistema(myItem).Save())
                            {
                                ScriptError = "alert('Errore in salvataggio!');";
                                return false;
                            }
                        }
                        else {
                            ScriptError = "alert('Ente non valido!');";
                            return false;
                        }
                    }
                }
                else if (myTipo == GenericCategory.TIPO.Operatori)
                {
                    foreach (GridViewRow myRow in myGrd.Rows)
                    {
                        UserRole myItem = new UserRole();
                        myItem = LoadUserFromGrd(myRow);
                        if (myItem.ID == string.Empty)
                        {
                            ScriptError = "alert('Inserire la password per l’operatore " + myItem.NameUser + "!');";
                            return false;
                        }
                        if (myItem.IDTipoProfilo == UserRole.PROFILO.OperatoreEnte && myItem.Enti.Count > 1)
                        {
                            ScriptError = "alert('Selezionare un ente soltanto per l’operatore " + myItem.NameUser + "!');";
                            return false;
                        }
                        if (myItem.IDTipoProfilo > 0 && myItem.Enti.Count == 0)
                        {
                            ScriptError = "alert('Selezionare almeno un ente per l’operatore " + myItem.NameUser + "!');";
                            return false;
                        }
                        if (myItem.IDTipoProfilo > 0 && myItem.Tributi.Count == 0)
                        {
                            ScriptError = "alert('Selezionare almeno un tributo per l’operatore " + myItem.NameUser + "!');";
                            return false;
                        }
                        if (!new User(myItem).Save())
                        {
                            ScriptError = "alert('Errore in salvataggio!');";
                            return false;
                        }
                    }
                }
                else if (myTipo == GenericCategory.TIPO.Documenti)
                {
                    if (!myIsCopyTo)
                    {
                        foreach (GridViewRow myRow in myGrd.Rows)
                        {
                            DocToAttach myItem = new DocToAttach();
                            myItem = LoadDocFromGrd(myRow, myEnte);
                            if (!new BLLDocToAttach(myItem).Save())
                            {
                                ScriptError = "alert('Errore in salvataggio!');";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (!(new BLLDocToAttach(new DocToAttach { IDEnte = myEnte }).CopyTo(myEnteDest)))
                        {
                            ScriptError = "alert('Errore in salvataggio!');";
                            return false;
                        }
                    }
                }
                else if (myTipo == GenericCategory.TIPO.ICI_Aliquote || myTipo == GenericCategory.TIPO.ICI_Zone || myTipo == GenericCategory.TIPO.ICI_Vincoli
                    || myTipo == GenericCategory.TIPO.TASI_Aliquote || myTipo == GenericCategory.TIPO.TASI_Agevolazioni)
                {
                    if (!myIsCopyTo)
                    {
                        foreach (GridViewRow myRow in myGrd.Rows)
                        {
                            GenericCategoryWithRate myItem = new GenericCategoryWithRate();
                            myItem = LoadGenericCatWithRateFromGrd(myRow, myEnte, myTipo, myTributo, myAnno, false);
                            if (myItem.Codice != string.Empty)
                            {
                                if (myItem.Valore < 0)
                                {
                                    ScriptError = "alert('Inserire tutte le aliquote!');";
                                    return false;
                                }
                                if ((myTipo == GenericCategory.TIPO.ICI_Aliquote || myTipo == GenericCategory.TIPO.TASI_Aliquote) && myItem.IDTributo == string.Empty)
                                {
                                    ScriptError = "alert('Inserire tutti i tributi di riferimento!');";
                                    return false;
                                }
                                if (myTipo == GenericCategory.TIPO.TASI_Aliquote && (myItem.PercProprietario + myItem.PercInquilino) != 100)
                                {
                                    ScriptError = "alert('Inserire correttamente la percentuale di ripartizione tra Proprietario ed Inquilino');";
                                    return false;
                                }
                            }
                        }
                        foreach (GridViewRow myRow in myGrd.Rows)
                        {
                            GenericCategoryWithRate myItem = new GenericCategoryWithRate();
                            myItem = LoadGenericCatWithRateFromGrd(myRow, myEnte, myTipo, myTributo, myAnno, false);
                            if (!new BLLGenericCategoryWithRate(myItem).Save())
                            {
                                ScriptError = "alert('Errore in salvataggio!');";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (!(new BLLGenericCategoryWithRate(new GenericCategoryWithRate { IDEnte = myEnte, IDTipo = myTipo, IDTributo = myTributo, Anno = myAnno }).CopyTo(myEnteDest, myAnnoDest)))
                        {
                            ScriptError = "alert('Errore in salvataggio!');";
                            return false;
                        }
                        //Aggiorno il campo codice delle righe idtype='67' con l'id corrispondente delle righe idtype='60' dell'ente su cui viene fatto il ribaltamento
                        updateCodice(myEnteDest, myAnnoDest);
                    }
                }
                else {
                    if (!myIsCopyTo)
                    {
                        foreach (GridViewRow myRow in myGrd.Rows)
                        {
                            GenericCategory myItem = new GenericCategory();
                            myItem = LoadGenericCatFromGrd(myRow, myEnte, myTipo, myTributo, myAnno);
                            if (!new BLLGenericCategory(myItem).Save())
                            {
                                ScriptError = "alert('Errore in salvataggio!');";
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (!(new BLLGenericCategory(new GenericCategory { IDEnte = myEnte, IDTipo = myTipo, IDTributo = myTributo, Anno = myAnno }).CopyTo(myEnteDest, myAnnoDest)))
                        {
                            ScriptError = "alert('Errore in salvataggio!');";
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Settings.Save::errore::", ex);
                return false;
            }
        }
    }

















    


















    public class BLLGenericCategory : GenericCategory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLGenericCategory));
        private GenericCategory InnerObj { get; set; }

        public BLLGenericCategory(GenericCategory myItem)
        {
            InnerObj = myItem;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSETTINGS_IU", "ID", "IDENTE", "IDTYPE", "IDTRIBUTO", "ANNO", "CODICE", "DESCRIZIONE", "VALORE", "IDESTERNO", "PERCPROPRIETARIO", "PERCINQUILINO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                            , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                            , ctx.GetParam("ANNO", InnerObj.Anno)
                            , ctx.GetParam("CODICE", InnerObj.Codice)
                            , ctx.GetParam("DESCRIZIONE", InnerObj.Descrizione)
                            , ctx.GetParam("VALORE", 0)
                            , ctx.GetParam("IDESTERNO", InnerObj.IDOrg)
                            , ctx.GetParam("PERCPROPRIETARIO", 0)
                            , ctx.GetParam("PERCINQUILINO", 0)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.Save::errore in inserimento configurazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.ID > 0)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLSETTINGS_D", "ID");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)).First<int>();
                        ctx.Dispose();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.Delete::errore in cancellazione configurazione");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.Delete::errore::", ex);
                return false;
            }
        }
        public bool CopyTo(string IDEnteTo, int AnnoTo)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSETTINGS_COPYTO", "IDENTEFROM", "IDTYPE", "IDTRIBUTO", "ANNOFROM", "IDENTETO", "ANNOTO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTEFROM", InnerObj.IDEnte)
                        , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                        , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                        , ctx.GetParam("ANNOFROM", InnerObj.Anno)
                        , ctx.GetParam("IDENTETO", IDEnteTo)
                        , ctx.GetParam("ANNOTO", AnnoTo)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.CopyTo::errore in ribaltamento configurazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategory.CopyTo::errore::", ex);
                return false;
            }
        }

    }
    public class BLLGenericCategoryWithRate : GenericCategoryWithRate
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLGenericCategoryWithRate));
        private GenericCategoryWithRate InnerObj { get; set; }

        public BLLGenericCategoryWithRate(GenericCategoryWithRate myItem)
        {
            InnerObj = myItem;
            
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSETTINGS_IU", "ID", "IDENTE", "IDTYPE", "IDTRIBUTO", "ANNO", "CODICE", "DESCRIZIONE", "VALORE", "IDESTERNO", "PERCPROPRIETARIO", "PERCINQUILINO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                            , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                            , ctx.GetParam("ANNO", InnerObj.Anno)
                            , ctx.GetParam("CODICE", InnerObj.Codice)
                            , ctx.GetParam("DESCRIZIONE", InnerObj.Descrizione)
                            , ctx.GetParam("VALORE", InnerObj.Valore)
                            , ctx.GetParam("IDESTERNO", InnerObj.IDOrg)
                            , ctx.GetParam("PERCPROPRIETARIO", InnerObj.PercProprietario)
                            , ctx.GetParam("PERCINQUILINO", InnerObj.PercInquilino)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.GenericCategoryWithRate.Save::errore in inserimento configurazione");
                        return false;
                    }
                    ctx.Dispose();

                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategoryWithRate.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.ID > 0)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLSETTINGS_D", "ID");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)).First<int>();
                        ctx.Dispose();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.GenericCategoryWithRate.Delete::errore in cancellazione configurazione");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategoryWithRate.Delete::errore::", ex);
                return false;
            }
        }
        public bool CopyTo(string IDEnteTo, int AnnoTo)
        {
            try
            {

                

                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLSETTINGS_COPYTO", "IDENTEFROM", "IDTYPE", "IDTRIBUTO", "ANNOFROM", "IDENTETO", "ANNOTO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTEFROM", InnerObj.IDEnte)
                        , ctx.GetParam("IDTYPE", InnerObj.IDTipo)
                        , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                        , ctx.GetParam("ANNOFROM", InnerObj.Anno)
                        , ctx.GetParam("IDENTETO", IDEnteTo)
                        , ctx.GetParam("ANNOTO", AnnoTo)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.GenericCategoryWithRate.CopyTo::errore in ribaltamento configurazione");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.GenericCategoryWithRate.CopyTo::errore::", ex);
                return false;
            }
        }
    }
    public class EntiSistema
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntiSistema));
        private EntiInLavorazione InnerObj { get; set; }

        public EntiSistema(EntiInLavorazione myItem)
        {
            InnerObj = myItem;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLENTIINLAVORAZIONE_IU", "ID", "IDENTE", "DESCRIZIONE", "AMBIENTE", "MAIL", "SPLITPWD");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                        , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        , ctx.GetParam("DESCRIZIONE", InnerObj.Descrizione)
                        , ctx.GetParam("AMBIENTE", InnerObj.Ambiente)
                        , ctx.GetParam("MAIL", (InnerObj.MailEnte!=null? InnerObj.MailEnte:string.Empty))
                        , ctx.GetParam("SPLITPWD", InnerObj.SplitPWD)
                        /*, ctx.GetParam("ANNOICI", InnerObj.AnnoVerticaleICI)
                        , ctx.GetParam("ANNIUSOGRATUITO", InnerObj.AnniUsoGratuito)
                        , ctx.GetParam("DATAAGGIORNAMENTO", InnerObj.DataAggiornamento)
                        , ctx.GetParam("HASICI", InnerObj.ICI)
                        , ctx.GetParam("HASTARSU", InnerObj.TARSU)
                        , ctx.GetParam("HASTASI", InnerObj.TASI)
                        , ctx.GetParam("HASOSAP", InnerObj.OSAP)
                        , ctx.GetParam("HASICP", InnerObj.ICP)*/
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento ente");
                        return false;
                    }
                    else
                    {
                        sSQL = ctx.GetSQL("prc_TBLENTITRIBUTI_D", "ID", "IDENTE");
                        int IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        ).First<int>();
                        foreach (GenericCategory myTrib in InnerObj.ListTributi)
                        {
                            if (myTrib.Codice != string.Empty && myTrib.IsActive==1)
                            {
                                sSQL = ctx.GetSQL("prc_TBLENTITRIBUTI_IU", "ID", "IDENTE", "IDTRIBUTO");
                                IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                                    , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                                    , ctx.GetParam("IDTRIBUTO", myTrib.IDTributo)
                                ).First<int>();
                                if (IDRet <= 0)
                                {
                                    Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento tributi ente");
                                    return false;
                                }
                            }
                        }

                        sSQL = ctx.GetSQL("prc_TBLENTIBASEMAIL_D", "ID", "IDENTE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        ).First<int>();
                        if (InnerObj.Mail.Sender != string.Empty)
                        {
                            sSQL = ctx.GetSQL("prc_TBLENTIBASEMAIL_IU", "ID", "IDENTE", "SENDER", "SENDERNAME", "SSL", "SERVER", "SERVERPORT", "PASSWORD", "ARCHIVE", "BACKOFFICE", "PROTOCOLLO", "WARNINGRECIPIENT", "WARNINGSUBJECT", "WARNINGMESSAGE", "SENDERRORMESSAGE");
                            IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                                , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                                , ctx.GetParam("SENDER", InnerObj.Mail.Sender)
                                , ctx.GetParam("SENDERNAME", InnerObj.Mail.SenderName)
                                , ctx.GetParam("SSL", InnerObj.Mail.SSL)
                                , ctx.GetParam("SERVER", InnerObj.Mail.Server)
                                , ctx.GetParam("SERVERPORT", InnerObj.Mail.ServerPort)
                                , ctx.GetParam("PASSWORD", InnerObj.Mail.Password)
                                , ctx.GetParam("ARCHIVE", InnerObj.Mail.Archive)
                                , ctx.GetParam("BACKOFFICE", InnerObj.Mail.BackOffice)
                                , ctx.GetParam("PROTOCOLLO", InnerObj.Mail.Protocollo)
                                , ctx.GetParam("WARNINGRECIPIENT", InnerObj.Mail.WarningRecipient)
                                , ctx.GetParam("WARNINGSUBJECT", InnerObj.Mail.WarningSubject)
                                , ctx.GetParam("WARNINGMESSAGE", InnerObj.Mail.WarningMessage)
                                , ctx.GetParam("SENDERRORMESSAGE", InnerObj.Mail.SendErrorMessage)
                            ).First<int>();
                            if (IDRet <= 0)
                            {
                                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento basemail ente");
                                return false;
                            }
                        }

                        sSQL = ctx.GetSQL("prc_TBLENTICARTOGRAFIA_D", "ID", "IDENTE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        ).First<int>();
                        if (InnerObj.SIT.UrlAuth == null)
                            InnerObj.SIT.UrlAuth = "";
                        Log.Debug("prc_TBLENTICARTOGRAFIA_IU -> param:ID=" + InnerObj.ID.ToString()
                            + ",IDENTE=" + InnerObj.IDEnte
                            + ",ISACTIVE=" + InnerObj.SIT.IsActive.ToString()
                            + ",URL=" + InnerObj.SIT.Url
                            + ",TOKEN=" + InnerObj.SIT.Token
                            + ",URLAUTH=" + InnerObj.SIT.UrlAuth);
                        sSQL = ctx.GetSQL("prc_TBLENTICARTOGRAFIA_IU", "ID", "IDENTE", "ISACTIVE", "URL", "TOKEN","URLAUTH");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("ISACTIVE", InnerObj.SIT.IsActive)
                            , ctx.GetParam("URL", InnerObj.SIT.Url)
                            , ctx.GetParam("TOKEN", InnerObj.SIT.Token)
                            , ctx.GetParam("URLAUTH", InnerObj.SIT.UrlAuth)
                        ).First<int>();
                        if (IDRet <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento cartografia ente");
                            return false;
                        }

                        sSQL = ctx.GetSQL("prc_TBLENTIVERTICALI_D", "ID", "IDENTE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        ).First<int>();
                        sSQL = ctx.GetSQL("prc_TBLENTIVERTICALI_IU", "ID", "IDENTE", "ANNOICI", "ANNIUSOGRATUITO", "DATAAGGIORNAMENTO", "BANCADATITRIBUTI", "FORNITORE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("ANNOICI", InnerObj.DatiVerticali.AnnoVerticaleICI)
                            , ctx.GetParam("ANNIUSOGRATUITO", InnerObj.DatiVerticali.AnniUsoGratuito)
                            , ctx.GetParam("DATAAGGIORNAMENTO", InnerObj.DatiVerticali.DataAggiornamento)
                            , ctx.GetParam("BANCADATITRIBUTI", InnerObj.DatiVerticali.TipoBancaDati)
                            , ctx.GetParam("FORNITORE", InnerObj.DatiVerticali.TipoFornitore)
                        ).First<int>();
                        if (IDRet <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento dativerticali ente");
                            return false;
                        }

                        sSQL = ctx.GetSQL("prc_TBLENTIPAGOPA_D", "ID", "IDENTE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        ).First<int>();
                        sSQL = ctx.GetSQL("prc_TBLENTIPAGOPA_IU", "ID", "IDENTE", "CARTID", "CARTSYS", "IBAN", "DESCRIBAN", "IDRISCOSSORE", "DESCRRISCOSSORE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                            , ctx.GetParam("CARTID", InnerObj.DatiPagoPA.CARTId)
                            , ctx.GetParam("CARTSYS", InnerObj.DatiPagoPA.CARTSys)
                            , ctx.GetParam("IBAN", InnerObj.DatiPagoPA.IBAN)
                            , ctx.GetParam("DESCRIBAN", InnerObj.DatiPagoPA.DescrIBAN)
                            , ctx.GetParam("IDRISCOSSORE", InnerObj.DatiPagoPA.IdRiscossore)
                            , ctx.GetParam("DESCRRISCOSSORE", InnerObj.DatiPagoPA.DescrRiscossore)
                        ).First<int>();
                        if (IDRet <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento datiPagoPA ente");
                            return false;
                        }

                        sSQL = ctx.GetSQL("prc_TBLENTILOGO_D", "ID", "IDENTE");
                        IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                            , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        ).First<int>();
                        if (InnerObj.Logo.NameLogo != string.Empty)
                        {
                            sSQL = ctx.GetSQL("prc_TBLENTILOGO_IU", "ID", "IDENTE", "POSTEDFILE", "NAMELOGO", "FILEMIMETYPE");
                            IDRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                                , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                                , ctx.GetParam("POSTEDFILE", InnerObj.Logo.PostedFile)
                                , ctx.GetParam("NAMELOGO", InnerObj.Logo.NameLogo)
                                , ctx.GetParam("FILEMIMETYPE", InnerObj.Logo.FileMIMEType)
                            ).First<int>();
                            if (IDRet <= 0)
                            {
                                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore in inserimento logo ente");
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
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.ID > 0)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLENTIINLAVORAZIONE_D", "ID");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)).First<int>();
                        ctx.Dispose();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Delete::errore in cancellazione ente");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.Delete::errore::", ex);
                return false;
            }
        }
        public EntiInLavorazione LoadEnte(string IdEnte, string UserName)
        {
            try
            {
                List<EntiInLavorazione> ListGen = LoadEntiSistema(IdEnte, UserName);
                return ListGen[0];
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadEnte::errore::", ex);
                return new EntiInLavorazione();
            }
        }
        public List<EntiInLavorazione> LoadEntiSistema(string IdEnte, string UserName)
        {
            List<EntiInLavorazione> ListMyData = new List<EntiInLavorazione>();
            try
            {
                using (DBModel ctx = new DBModel())
                {


                    string sSQL = ctx.GetSQL("prc_GetEntiInLavorazione", "IDENTE","USERNAME");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<EntiInLavorazione>(sSQL, ctx.GetParam("IDENTE", IdEnte), ctx.GetParam("USERNAME", UserName)).ToList<EntiInLavorazione>();
                    foreach (EntiInLavorazione myItem in ListMyData)
                    {
                        myItem.ListTributi = LoadEntiTributi(myItem.IDEnte, -1,0);
                        myItem.Mail = LoadBaseMail(myItem.IDEnte, -1);
                        myItem.SIT = LoadBaseCartografia(myItem.IDEnte, -1);
                        myItem.DatiVerticali = LoadBaseVerticali(myItem.IDEnte, -1);
                        myItem.DatiPagoPA = LoadBasePagoPA(myItem.IDEnte, -1);
                        List<BaseLogo> ListLogo = LoadBaseLogo(myItem.IDEnte, -1);
                        if (ListLogo.Count > 0)
                            myItem.Logo = ListLogo[0];
                        Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadEntiSistema::SplitPWD::" + myItem.SplitPWD.ToString());
                    }
                    ctx.Dispose();
                    
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadEntiSistema::errore::", ex);
                return ListMyData;
            }
        }
        public List<GenericCategory> LoadEntiTributi(string IdEnte, int IdRow,int OnlyActive)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEntiVSTributi", "IDENTE", "IDROW","ONLYACTIVE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("IDROW", IdRow)
                            , ctx.GetParam("ONLYACTIVE", OnlyActive)
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadEntiTributi::errore::", ex);
                return ListMyData;
            }
        }
        public BaseMail LoadBaseMail(string IdEnte, int IdRow)
        {
            BaseMail myData = new BaseMail();
            try
            {
                List<BaseMail> ListMyData = new List<BaseMail>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEntiBaseMail", "IDENTE", "IDROW");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<BaseMail>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                        , ctx.GetParam("IDROW", IdRow)
                    ).ToList<BaseMail>();
                    if (ListMyData.Count > 0)
                        myData = ListMyData[0];
                    ctx.Dispose();
                }
                return myData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadBaseMail::errore::", ex);
                return myData;
            }
        }
        public BaseCartografia LoadBaseCartografia(string IdEnte, int IdRow)
        {
            BaseCartografia myData = new BaseCartografia();
            try
            {
                List<BaseCartografia> ListMyData = new List<BaseCartografia>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEntiBaseCartografia", "IDENTE", "IDROW");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<BaseCartografia>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                        , ctx.GetParam("IDROW", IdRow)
                    ).ToList<BaseCartografia>();
                    if (ListMyData.Count > 0)
                        myData = ListMyData[0];
                    ctx.Dispose();
                }
                return myData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadBaseCartografia::errore::", ex);
                return myData;
            }
        }
        public BaseVerticali LoadBaseVerticali(string IdEnte, int IdRow)
        {
            BaseVerticali myData = new BaseVerticali();
            try
            {
                List<BaseVerticali> ListMyData = new List<BaseVerticali>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEntiBaseVerticali", "IDENTE", "IDROW");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<BaseVerticali>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                        , ctx.GetParam("IDROW", IdRow)
                    ).ToList<BaseVerticali>();
                    if (ListMyData.Count > 0)
                        myData = ListMyData[0];
                    ctx.Dispose();
                }
                return myData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadBaseVerticali::errore::", ex);
                return myData;
            }
        }
        public BasePagoPA LoadBasePagoPA(string IdEnte, int IdRow)
        {
            BasePagoPA myData = new BasePagoPA();
            try
            {
                List<BasePagoPA> ListMyData = new List<BasePagoPA>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEntiBasePagoPA", "IDENTE", "IDROW");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<BasePagoPA>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                        , ctx.GetParam("IDROW", IdRow)
                    ).ToList<BasePagoPA>();
                    if (ListMyData.Count > 0)
                        myData = ListMyData[0];
                    ctx.Dispose();
                }
                return myData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadBasePagoPA::errore::", ex);
                return myData;
            }
        }
        public List<BaseLogo> LoadBaseLogo(string IdEnte, int IdRow)
        {
            List<BaseLogo> ListMyData = new List<BaseLogo>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetEntiBaseLogo", "IDENTE", "IDROW");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<BaseLogo>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                        , ctx.GetParam("IDROW", IdRow)
                    ).ToList<BaseLogo>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.EntiSistema.LoadBaseLogo::errore::", ex);
                return ListMyData;
            }
        }
    }
    public class User : UserRole
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(User));
        private UserRole InnerObj { get; set; }

        public User(UserRole myItem)
        {
            InnerObj = myItem;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLUSERS_IU", "ID", "USERNAME", "IDROLE");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                        , ctx.GetParam("USERNAME", InnerObj.NameUser)
                        , ctx.GetParam("IDROLE", InnerObj.IDTipoProfilo)
                        ).First<string>();
                    if (InnerObj.ID == string.Empty)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Save::errore in inserimento ente");
                        return false;
                    }
                    else
                    {
                        sSQL = ctx.GetSQL("prc_TBLUSERSENTI_D", "IDUSER");
                        if (ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDUSER", InnerObj.ID)).First<int>() < 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Save::errore in inserimento ente");
                            return false;
                        }
                        foreach (string myItem in InnerObj.Enti)
                        {
                            if (myItem != string.Empty)
                            {
                                sSQL = ctx.GetSQL("prc_TBLUSERSENTI_IU", "ID", "IDUSER", "IDENTE");
                                if (ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                                    , ctx.GetParam("IDUSER", InnerObj.ID)
                                    , ctx.GetParam("IDENTE", myItem)
                                    ).First<int>() <= 0)
                                {
                                    Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Save::errore in inserimento ente");
                                    return false;
                                }
                            }
                        }
                        sSQL = ctx.GetSQL("prc_TBLUSERSTRIBUTI_D", "IDUSER");
                        if (ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDUSER", InnerObj.ID)).First<int>() < 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Save::errore in inserimento ente");
                            return false;
                        }
                        foreach (string myItem in InnerObj.Tributi)
                        {
                            if (myItem != string.Empty)
                            {
                                sSQL = ctx.GetSQL("prc_TBLUSERSTRIBUTI_IU", "ID", "IDUSER", "IDTRIBUTO");
                                if (ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", -1)
                                    , ctx.GetParam("IDUSER", InnerObj.ID)
                                    , ctx.GetParam("IDTRIBUTO", myItem)
                                    ).First<int>() <= 0)
                                {
                                    Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Save::errore in inserimento ente");
                                    return false;
                                }
                            }
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.ID != string.Empty)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLUSERS_D", "ID");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("ID", InnerObj.ID)).First<string>();
                        ctx.Dispose();
                        if (InnerObj.ID == string.Empty)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Delete::errore in cancellazione ente");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.UserRole.Delete::errore::", ex);
                return false;
            }
        }

        public UserRole LoadUser(string Nominativo, string CFPIVA, string UserName, int IDContribuente, string IdEnte)
        {
            UserRole MyData = new UserRole();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetAnagrafica", "NOMEUTENTE", "IDCONTRIBUENTE", "NOMINATIVO", "CFPIVA","IDENTE");
                    MyData = ctx.ContextDB.Database.SqlQuery<UserRole>(sSQL, ctx.GetParam("NOMEUTENTE", UserName)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("NOMINATIVO", Nominativo)
                                , ctx.GetParam("CFPIVA", CFPIVA)
                                , ctx.GetParam("IDENTE", IdEnte)
                            ).FirstOrDefault<UserRole>();
                    ctx.Dispose();
                }
                return MyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.UserRole.LoadUser::errore::", ex);
                return MyData;
            }
        }
        public List<GenericCategory> LoadUserEnti(string UserID, string UserName)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetUserEnti", "IDUSER", "USERNAME", "ONLYCOD");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDUSER", UserID)
                                , ctx.GetParam("USERNAME", UserName)
                                , ctx.GetParam("ONLYCOD", 0)
                            ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.UserRole.LoadUserEnti::errore::", ex);
                return ListMyData;
            }
        }
        public List<GenericCategory> LoadUserTributi(string ParamSearch, bool bOnlyCod)
        {
            List<GenericCategory> ListMyData = new List<GenericCategory>();
            try
            {
                 using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetUserTributi", "IDUSER", "ONLYCOD");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<GenericCategory>(sSQL, ctx.GetParam("IDUSER", ParamSearch)
                        , ctx.GetParam("ONLYCOD", ((bOnlyCod) ? 1 : 0))
                        ).ToList<GenericCategory>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.UserRole.LoadUserTributi::errore::", ex);
                return ListMyData;
            }
        }
    }
    public class BLLDocToAttach : DocToAttach
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLDocToAttach));
        private DocToAttach InnerObj { get; set; }

        public BLLDocToAttach(DocToAttach myItem)
        {
            InnerObj = myItem;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLDOCTOATTACH_IU", "ID", "IDENTE", "IDTRIBUTO", "IDTIPOISTANZA", "DOCUMENTO", "ISOBBLIGATORIO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                        , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        , ctx.GetParam("IDTRIBUTO", InnerObj.IDTributo)
                        , ctx.GetParam("IDTIPOISTANZA", InnerObj.IDTipoIstanza)
                        , ctx.GetParam("DOCUMENTO", InnerObj.Documento)
                        , ctx.GetParam("ISOBBLIGATORIO", InnerObj.IsObbligatorio)
                        ).First<int>();
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.DocToAttach.Save::errore in inserimento ente");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.DocToAttach.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.ID > 0)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLDOCTOATTACH_D", "ID");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)).First<int>();
                        ctx.Dispose();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.DocToAttach.Delete::errore in cancellazione ente");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.DocToAttach.Delete::errore::", ex);
                return false;
            }
        }
        public bool CopyTo(string IDEnteTo)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLDOCTOATTACH_COPYTO", "IDENTEFROM", "IDENTETO");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTEFROM", InnerObj.IDEnte)
                        , ctx.GetParam("IDENTETO", IDEnteTo)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.DocToAttach.CopyTo::errore in ribaltamento configurazione");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.DocToAttach.CopyTo::errore::", ex);
                return false;
            }
        }
        public List<DocToAttach> LoadDocToAttach(string ParamSearch)
        {
            try
            {
                List<DocToAttach> ListMyData = new List<DocToAttach>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDocToAttach", "IDENTE");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<DocToAttach>(sSQL, ctx.GetParam("IDENTE", ParamSearch)).ToList<DocToAttach>();
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.BLLDocToAttach.LoadDocToAttach::errore::", ex);
                return new List<DocToAttach>();
            }
        }
    }
    public class Messages
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Messages));
        private Message InnerObj { get; set; }

        public Messages(Message myItem)
        {
            InnerObj = myItem;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLCOMUNICAZIONI_IU", "ID", "IDENTE", "IDTYPEDESTINATARI", "SUBSETDESTINATARI", "TYPEMEZZO", "DATAINSERIMENTO", "DATAINVIO", "TESTO", "DATAINVIOMAIL");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                        , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                        , ctx.GetParam("IDTYPEDESTINATARI", InnerObj.IDTypeRecipient)
                        , ctx.GetParam("SUBSETDESTINATARI", InnerObj.SubsetRecipient)
                        , ctx.GetParam("TYPEMEZZO", InnerObj.TypeMezzo)
                        , ctx.GetParam("DATAINSERIMENTO", DateTime.Now)
                        , ctx.GetParam("DATAINVIO", InnerObj.DataInvio)
                        , ctx.GetParam("TESTO", InnerObj.Testo)
                        , ctx.GetParam("DATAINVIOMAIL", DateTime.MaxValue)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.Messages.Save::errore in inserimento comunicazione");
                        return false;
                    }
                    else
                    {
                        Log.Debug("devo mandare ai soggetti:query= prc_SetComunicazioniSoggetti IDENTE=" + InnerObj.IDEnte + ", IDTYPEDESTINATARI=" + InnerObj.IDTypeRecipient.ToString() + ", IDCOMUNICAZIONE=" + InnerObj.ID.ToString());
                        sSQL = ctx.GetSQL("prc_SetComunicazioniSoggetti", "IDENTE", "IDTYPEDESTINATARI", "IDCOMUNICAZIONE");
                        int ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL
                                , ctx.GetParam("IDENTE", InnerObj.IDEnte)
                                , ctx.GetParam("IDTYPEDESTINATARI", InnerObj.IDTypeRecipient)
                                , ctx.GetParam("IDCOMUNICAZIONE", InnerObj.ID)
                            ).First<int>();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Messages.Save::errore in inserimento soggetti comunicazione");
                            return false;
                        }
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.ID > 0)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLMESSAGES_D", "ID");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)).First<int>();
                        ctx.Dispose();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Messages.Delete::errore in cancellazione comunicazione");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.Delete::errore::", ex);
                return false;
            }
        }
        public bool SetReading(int IdNews, string TypeNews)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_SetMessageRead", "ID", "TYPE");
                    int RetID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", IdNews)
                            , ctx.GetParam("TYPE", TypeNews)
                        ).First<int>();
                    if (RetID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.Messages.SetReading::errore in inserimento data lettura");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.SetReading::errore::", ex);
                return false;
            }
        }
        public List<Message> LoadMessages(string myEnte, int myTypeDestinatari, string myTypeMezzo, DateTime myDataInvio, int IdNews)
        {
            List<Message> ListMyData = new List<Message>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetMessage", "IDENTE", "IDTYPEDESTINATARI", "TYPEMEZZO", "DATAINVIO", "ID");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Message>(sSQL, ctx.GetParam("IDENTE", myEnte)
                        , ctx.GetParam("IDTYPEDESTINATARI", myTypeDestinatari)
                        , ctx.GetParam("TYPEMEZZO", myTypeMezzo)
                        , ctx.GetParam("DATAINVIO", myDataInvio)
                        , ctx.GetParam("ID", IdNews)
                        ).ToList<Message>();
                    ctx.Dispose();
                }

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.LoadMessages::errore::", ex);
                return ListMyData;
            }
        }
        public bool FieldValidator(string myEnte, int myTypeRecipient, string mySubsetRecipient, string myTypeMezzo, DateTime myDataInvio, string myTesto, out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                if (myEnte == string.Empty)
                {
                    ScriptError += "alert('Compilare ente!');";
                    return false;
                }
                if (myTypeRecipient <= 0)
                {
                    ScriptError += "alert('Compilare i destinatari!');";
                    return false;
                }
                if (myTypeRecipient > 1 && mySubsetRecipient == string.Empty)
                {
                    ScriptError += "alert('Compilare i range di destinatari!');";
                    return false;
                }
                if (myTypeMezzo == string.Empty)
                {
                    ScriptError += "alert('Compilare il tipo di invio!');";
                    return false;
                }
                if (myDataInvio == DateTime.MaxValue)
                {
                    ScriptError += "alert('Compilare la data di invio!');";
                    return false;
                }
                if (myTesto == string.Empty)
                {
                    ScriptError += "alert('Compilare il testo!');";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.FieldValidator::errore::", ex);
                return false;
            }
        }
        public void SendMail()
        {
            try
            {
                Log.Debug("SendMail");
                string sErr = string.Empty;
                List<IdentityMessage> DataMail = new List<IdentityMessage>();
                List<string> ListRecipient = new List<string>();

                if (MySession.Current.Ente != null)
                {
                    Log.Debug("ho ente");
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_GetToSendDataMail");
                        DataMail = ctx.ContextDB.Database.SqlQuery<IdentityMessage>(sSQL).ToList<IdentityMessage>();
                        Log.Debug("devo mandare " + DataMail.Count.ToString() + " mail di comunicazioni");
                        if (DataMail.Count > 0)
                        {
                            foreach (IdentityMessage myMessage in DataMail)
                            {
                                /*sSQL = ctx.GetSQL("prc_GetToSendRecipient", "ID");
                                ListRecipient = ctx.ContextDB.Database.SqlQuery<string>(sSQL
                                        , ctx.GetParam("ID", myMessage.Destination)
                                    ).ToList<string>();*/
                                ListRecipient = new List<string>();
                                ListRecipient.Add(myMessage.Destination);
                                Log.Debug("destinatario:" + myMessage.Destination);
                                new EmailService().CreateMail(MySession.Current.Ente.Mail, ListRecipient, new List<string>() { MySession.Current.Ente.Mail.Archive }, myMessage.Subject, myMessage.Body, new List<System.Web.Mail.MailAttachment>(), out sErr);
                                if (sErr != string.Empty)
                                {
                                    Log.Debug("OPENgovSPORTELLO.BLL.Messages.SendMail::errore in createmal::" + sErr);
                                    break;
                                }
                            }
                            sSQL = ctx.GetSQL("prc_SetSentMail");
                            int retVal = ctx.ContextDB.Database.SqlQuery<int>(sSQL).First<int>();
                            if (retVal <= 0)
                            {
                                Log.Debug("OPENgovSPORTELLO.BLL.Messages.SendMail::errore in inserimento invio mail");
                            }
                        }
                        ctx.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.SendMail::errore::", ex);
            }
        }
    }
    public class Deleghe
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Deleghe));
        private Delega InnerObj { get; set; }
        private string IDEnte { get; set; }

        public Deleghe(Delega myItem, string myEnte)
        {
            InnerObj = myItem;
            IDEnte = myEnte;
        }
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLDELEGHE_IU", "ID", "IDISTANZA", "IDENTE", "NOMINATIVO", "CFPIVA", "IDDELEGANTE", "IDTRIBUTO", "DATA_CESSAZIONE", "IDORIGINECESSAZIONE");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                            , ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("NOMINATIVO", InnerObj.Nominativo)
                            , ctx.GetParam("CFPIVA", InnerObj.CodFiscalePIVA)
                            , ctx.GetParam("IDDELEGANTE", InnerObj.IDDelegante)
                            , ctx.GetParam("IDTRIBUTO", InnerObj.IdTributo)
                            , ctx.GetParam("DATA_CESSAZIONE", InnerObj.DataCessazione)
                            , ctx.GetParam("IDORIGINECESSAZIONE", InnerObj.IDOrigineCessazione)
                        ).First<int>();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.Save::errore in inserimento delega");
                        return false;
                    }
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.Save::errore::", ex);
                return false;
            }
        }
        public bool Delete()
        {
            try
            {
                if (InnerObj.IDIstanza > 0)
                {
                    using (DBModel ctx = new DBModel())
                    {
                        string sSQL = ctx.GetSQL("prc_TBLDELEGHE_D", "IDISTANZA", "IDORIGINECESSAZIONE");
                        InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDISTANZA", InnerObj.IDIstanza)
                            , ctx.GetParam("IDORIGINECESSAZIONE", InnerObj.IDOrigineCessazione)
                        ).First<int>();
                        ctx.Dispose();
                        if (InnerObj.ID <= 0)
                        {
                            Log.Debug("OPENgovSPORTELLO.BLL.Messages.Delete::errore in cancellazione comunicazione");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.Delete::errore::", ex);
                return false;
            }
        }

        public List<Delega> LoadDeleghe(string myEnte, int myDelegato, int myIstanza)
        {
            List<Delega> ListMyData = new List<Delega>();

            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDeleghe", "IDENTE", "IDDELEGATO", "IDISTANZA");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Delega>(sSQL, ctx.GetParam("IDENTE", myEnte)
                            , ctx.GetParam("IDDELEGATO", myDelegato)
                            , ctx.GetParam("IDISTANZA", myIstanza)
                        ).ToList<Delega>();
                    ctx.Dispose();
                }

                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.LoadDeleghe::errore::", ex);
                return ListMyData;
            }
        }
        public Delega LoadDataFromGrd(GridViewRow myRow, ref int Takeover)
        {
            Delega myItem = new Delega();

            try
            {
                myItem.ID = int.Parse(((HiddenField)myRow.FindControl("hfIdRow")).Value.ToString());
                myItem.Nominativo = ((TextBox)myRow.FindControl("txtNominativo")).Text;
                myItem.CodFiscalePIVA = ((TextBox)myRow.FindControl("txtCFPIVA")).Text;
                myItem.IdTributo = ((DropDownList)myRow.FindControl("ddlTributo")).SelectedValue;
                myItem.DescrTributo = ((DropDownList)myRow.FindControl("ddlTributo")).SelectedItem.Text;
                myItem.IDStato = int.Parse(((HiddenField)myRow.FindControl("hfIdStato")).Value.ToString());
                myItem.IDIstanza = int.Parse(((HiddenField)myRow.FindControl("hfIdIstanza")).Value.ToString());
                myItem.Stato = ((Label)myRow.FindControl("lblStato")).Text;
                myItem.Subentro = ((HiddenField)myRow.FindControl("hfSubentro")).Value.ToString();
                if (myItem.Subentro == "KO" && Takeover == 0)
                {
                    myItem.Subentro = CheckDelega(myItem.CodFiscalePIVA, myItem.IdTributo);
                }
                else
                {
                    myItem.Subentro = "OK";
                    Takeover = 0;
                }

                return myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.LoadDataFromGrd::errore::", ex);
                return myItem;
            }
        }
        public string CheckDelega(string myCFPIVA, string myTributo)
        {
            string myCheck = "KO";
            try
            {                
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_CheckDelega", "CFPIVA", "TRIBUTO");
                    myCheck = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("CFPIVA", myCFPIVA)
                            , ctx.GetParam("TRIBUTO", myTributo)
                        ).First<string>();
                    ctx.Dispose();
                }

                return myCheck;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.CheckDelega::errore::", ex);
                return myCheck;
            }
        }
        public bool FieldValidator(out string ScriptError)
        {
            ScriptError = string.Empty;
            try
            {
                if (InnerObj.Nominativo == string.Empty)
                {
                    ScriptError += "alert('Compilare nominativo delegante!');";
                    return false;
                }
                if (InnerObj.CodFiscalePIVA == string.Empty)
                {
                    ScriptError += "alert('Compilare codice fiscale o partita iva del delegante!');";
                    return false;
                }
                if (InnerObj.IdTributo == string.Empty)
                {
                    ScriptError += "alert('Compilare il tributo!');";
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Messages.FieldValidator::errore::", ex);
                return false;
            }
        }
        public bool GetDocPrint(int myIstanza, EntiInLavorazione myEnte, out string sScript)
        {
            sScript = string.Empty;
            List<Delega> ListMyData = new List<Delega>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDeleghe", "IDENTE", "IDDELEGATO", "IDISTANZA");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Delega>(sSQL, ctx.GetParam("IDENTE", myEnte.IDEnte)
                            , ctx.GetParam("IDDELEGATO", -1)
                            , ctx.GetParam("IDISTANZA", myIstanza)
                        ).ToList<Delega>();
                    ctx.Dispose();
                }

                Delega myItem = ListMyData[0];
                sScript += "<html xmlns='http://www.w3.org/1999/xhtml'>";
                sScript += "<head>";
                sScript += "<link href='../Content/Delega"+ MySession.Current.ComuneSito + ".css' rel='stylesheet'>";
                sScript += "</head>";
                sScript += "<body>";
                sScript += "<div id='dichhome'>";
                sScript += "<div id='dich_testata'>";
                sScript += "<div id='dich_contribuente'>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichnominativo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnominativo'  value='" + myItem.Nominativo.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcf'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcf' value='" + myItem.CodFiscalePIVA + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcomunenasc'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomunenasc' value='" + myItem.ComuneNascita.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichpvnasc'>";
                sScript += "<input type='text' class='dich_dett' id='dichDpv' value='" + myItem.PVNascita + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatanascGG'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetGGFromData(myItem.DataNascita) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatanascMM'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetMMFromData(myItem.DataNascita) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichdatanascAA'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdatanasc' value='" + new General().GetAAFromData(myItem.DataNascita) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcomune'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomune' value='" + myItem.Comune.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichpv'>";
                sScript += "<input type='text' class='dich_dett' id='dichDpv' value='" + myItem.PV + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichres'>";
                sScript += "<input type='text' class='dich_dett' id='dichDres' value='" + myItem.Indirizzo.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichdelegici'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdeleg'  value='" + ((myItem.IdTributo == General.TRIBUTO.ICI) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcomunedelegici'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomunedelegici' value='" + ((myItem.IdTributo == General.TRIBUTO.ICI) ? myEnte.Descrizione.Replace("'", "&rsquo;") : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichdelegtarsu'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdeleg'  value='" + ((myItem.IdTributo == General.TRIBUTO.TARSU) ? "X" : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcomunedelegtarsu'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomunedelegtarsu' value='" + ((myItem.IdTributo == General.TRIBUTO.TARSU) ? myEnte.Descrizione.Replace("'", "&rsquo;") : string.Empty) + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichnominativodelegato'>";
                sScript += "<input type='text' class='dich_dett' id='dichDnominativodelegato'  value='" + myItem.NominativoDelegato.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichcfdelegato'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcfdelegato' value='" + myItem.CodFiscalePIVADelegato + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichresdelegato'>";
                sScript += "<input type='text' class='dich_dett' id='dichDresdelegato' value='" + myItem.IndirizzoDelegato.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichcomunedelegato'>";
                sScript += "<input type='text' class='dich_dett' id='dichDcomune' value='" + myItem.ComuneDelegato.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "<div id='dichpvdelegato'>";
                sScript += "<input type='text' class='dich_dett' id='dichDpv' value='" + myItem.PVDelegato + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<br />";
                sScript += "<div class='dicrow'>";
                sScript += "<div id='dichdescrtributo'>";
                sScript += "<input type='text' class='dich_dett' id='dichDdescrtributo' value='" + myItem.DescrTributo.Replace("'", "&rsquo;") + "'>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</div>";
                sScript += "</body>";
                sScript += "</html>";
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.GetDocPrint::errore::", ex);
                return false;
            }
        }
        public string GetMailUser(int myIstanza)
        {
            string sMyRet =string.Empty;

            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetDelegheMailUser", "IDENTE", "IDISTANZA");
                    sMyRet = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDISTANZA", myIstanza)
                        ).First<string>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.Deleghe.GetMailUser::errore::", ex);
                sMyRet = string.Empty;
            }
            return sMyRet;
        }
    }
}