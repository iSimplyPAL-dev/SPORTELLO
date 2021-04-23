using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe di gestione dati sul verticale tributi
    /// </summary>
    public class VerticaleTrib
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(VerticaleTrib));

        private RiepilogoUIVerticale InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public VerticaleTrib(RiepilogoUIVerticale myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <param name="ListDatiUI"></param>
        /// <param name="ListRifCat"></param>
        /// <param name="ListContrib"></param>
        /// <returns></returns>
        public bool LoadIstanzeForVerticaleTrib(int IDIstanza, out List<string> ListDatiUI, out List<RiepilogoUIVerticale> ListRifCat, out List<RiepilogoUIVerticale> ListContrib)
        {
            ListRifCat = ListContrib = new List<RiepilogoUIVerticale>();
            ListDatiUI = new List<string>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetRiepilogoIstanzaUI", "IDISTANZA");
                    ListDatiUI = ctx.ContextDB.Database.SqlQuery<string>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)).ToList<string>();
                    
                    sSQL = ctx.GetSQL("prc_GetVerticaleUIVSRifCat", "IDISTANZA");
                    ListRifCat = ctx.ContextDB.Database.SqlQuery<RiepilogoUIVerticale>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)).ToList<RiepilogoUIVerticale>();
                    sSQL = ctx.GetSQL("prc_GetVerticaleUIVSContrib", "IDISTANZA");
                    ListContrib = ctx.ContextDB.Database.SqlQuery<RiepilogoUIVerticale>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)).ToList<RiepilogoUIVerticale>();

                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.VerticaleTrib.LoadIstanzeForVerticaleTrib::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <param name="Operatore"></param>
        /// <returns></returns>
        public bool AddUI(int IDIstanza, string Operatore)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_VerticaleUIAdd", "IDISTANZA", "OPERATORE");
                    int IDNewVerticale = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)
                            , ctx.GetParam("OPERATORE", Operatore)
                        ).First<int>();
                    ctx.Dispose();
                    if (IDNewVerticale<= 0) { 
                        Log.Debug("OPENgovSPORTELLO.BLL.VerticaleTrib.AddUI::errore in chiusura posizione verticale");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.VerticaleTrib.AddUI::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDIstanza"></param>
        /// <param name="IDVerticale"></param>
        /// <param name="Operatore"></param>
        /// <returns></returns>
        public bool CloseUI(int IDIstanza, int IDVerticale, string Operatore)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_VerticaleUIClose", "IDISTANZA", "IDVERTICALE", "OPERATORE");
                    int IDNewVerticale = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDISTANZA", IDIstanza)
                            , ctx.GetParam("IDVERTICALE", IDVerticale)
                            , ctx.GetParam("OPERATORE", Operatore)
                        ).First<int>();
                    ctx.Dispose();
                    if (IDNewVerticale <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLL.VerticaleTrib.CloseUI::errore in chiusura posizione verticale");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.VerticaleTrib.CloseUI::errore::", ex);
                return false;
            }
        }
    }
}