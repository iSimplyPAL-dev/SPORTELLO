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
    /// Classe di gestione provvedimenti
    /// </summary>
    public class PROVVEDIMENTI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PROVVEDIMENTI));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="ListObj"></param>
        /// <returns></returns>
        public bool LoadProvvedimenti(string IDEnte, int IDContribuente, int IDRif, out List<RiepilogoUI> ListObj)
        {
            ListObj = new List<RiepilogoUI>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetProvvedimenti", "IDENTE", "IDCONTRIBUENTE", "IDRIFORG");
                    ListObj = ctx.ContextDB.Database.SqlQuery<RiepilogoUI>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                                , ctx.GetParam("IDRIFORG", IDRif)
                            ).ToList<RiepilogoUI>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadProvvedimenti::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="IDRif"></param>
        /// <returns></returns>
        public SPC_ImportiProvvedimento LoadImporti(string Type, int IDRif)
        {
            SPC_ImportiProvvedimento myItem = new SPC_ImportiProvvedimento();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetProvvedimentiImporti", "TIPO", "IDRIFORG");
                    myItem = ctx.ContextDB.Database.SqlQuery<SPC_ImportiProvvedimento>(sSQL, ctx.GetParam("TIPO", Type)
                                , ctx.GetParam("IDRIFORG", IDRif)
                            ).First<SPC_ImportiProvvedimento>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadImporti::errore::", ex);
                myItem = new SPC_ImportiProvvedimento();
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="IDRif"></param>
        /// <returns></returns>
        public List<SPC_UIProvvedimento> LoadUI(string Type, int IDRif)
        {
            List<SPC_UIProvvedimento> ListObj = new List<SPC_UIProvvedimento>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetProvvedimentiUI", "TIPO", "IDRIFORG");
                    ListObj = ctx.ContextDB.Database.SqlQuery<SPC_UIProvvedimento>(sSQL, ctx.GetParam("TIPO", Type)
                                , ctx.GetParam("IDRIFORG", IDRif)
                            ).ToList<SPC_UIProvvedimento>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadUI::errore::", ex);
                ListObj = new List<SPC_UIProvvedimento>();
            }
            return ListObj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDRif"></param>
        /// <returns></returns>
        public List<SPC_InteressiProvvedimento> LoadInteressi(int IDRif)
        {
            List<SPC_InteressiProvvedimento> ListObj = new List<SPC_InteressiProvvedimento>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetProvvedimentiInteressi", "IDRIFORG");
                    ListObj = ctx.ContextDB.Database.SqlQuery<SPC_InteressiProvvedimento>(sSQL, ctx.GetParam("IDRIFORG", IDRif)).ToList<SPC_InteressiProvvedimento>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadInteressi::errore::", ex);
                ListObj = new List<SPC_InteressiProvvedimento>();
            }
            return ListObj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDRif"></param>
        /// <returns></returns>
        public List<SPC_SanzioniProvvedimento> LoadSanzioni(int IDRif)
        {
            List<SPC_SanzioniProvvedimento> ListObj = new List<SPC_SanzioniProvvedimento>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetProvvedimentiSanzioni", "IDRIFORG");
                    ListObj = ctx.ContextDB.Database.SqlQuery<SPC_SanzioniProvvedimento>(sSQL, ctx.GetParam("IDRIFORG", IDRif)).ToList<SPC_SanzioniProvvedimento>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadSanzioni::errore::", ex);
                ListObj = new List<SPC_SanzioniProvvedimento>();
            }
            return ListObj;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="IDRif"></param>
        /// <param name="myProvvedimento"></param>
        /// <returns></returns>
        public bool LoadDich(string IDEnte, int IDContribuente, int IDRif, out SPC_Provvedimento myProvvedimento)
        {
            myProvvedimento = new SPC_Provvedimento();
            try
            {
                List<RiepilogoUI> ListObj = new List<RiepilogoUI>();
                if (!LoadProvvedimenti(IDEnte, IDContribuente, IDRif, out ListObj))
                {
                    return false;
                }
                foreach (RiepilogoUI myItem in ListObj)
                {
                    myProvvedimento.ID = myItem.ID;
                    myProvvedimento.IDContribuente = myItem.IDContribuente;
                    myProvvedimento.IDEnte = myItem.IDEnte;
                    myProvvedimento.Anno = myItem.Zona;
                    myProvvedimento.DataNotifica = myItem.Dal;
                    myProvvedimento.Descrizione = myItem.Ubicazione;
                    myProvvedimento.Dovuto = myItem.ImpDovuto;
                    myProvvedimento.Pagato = myItem.RenditaValore;
                    myProvvedimento.Stato = myItem.Stato;
                    myProvvedimento.ImpPieno = LoadImporti("P", IDRif);
                    myProvvedimento.ImpRidotto = LoadImporti("R", IDRif);
                    myProvvedimento.ListAccertato = LoadUI("A", IDRif);
                    myProvvedimento.ListDichiarato = LoadUI("D", IDRif);
                    myProvvedimento.ListInteressi = LoadInteressi(IDRif);
                    myProvvedimento.ListSanzioni = LoadSanzioni(IDRif);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadDich::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="ListProvvedimenti"></param>
        /// <returns></returns>
        public bool LoadListProvvedimenti(string IDEnte, int IDContribuente, out List<SPC_Provvedimento> ListProvvedimenti)
        {
            ListProvvedimenti = new List<SPC_Provvedimento>();
            try
            {
                List<RiepilogoUI> ListObj = new List<RiepilogoUI>();
                if (!LoadProvvedimenti(IDEnte, IDContribuente, -1, out ListObj))
                {
                    return false;
                }
                foreach (RiepilogoUI myItem in ListObj)
                {
                    SPC_Provvedimento myProvvedimento = new SPC_Provvedimento();
                    myProvvedimento.ID = myItem.ID;
                    myProvvedimento.IDContribuente = myItem.IDContribuente;
                    myProvvedimento.IDEnte = myItem.IDEnte;
                    myProvvedimento.Anno = myItem.Zona;
                    myProvvedimento.DataNotifica = myItem.Dal;
                    myProvvedimento.Descrizione = myItem.Ubicazione;
                    myProvvedimento.Dovuto = myItem.ImpDovuto;
                    myProvvedimento.Pagato = myItem.RenditaValore;
                    myProvvedimento.Stato = myItem.Stato;
                    myProvvedimento.ImpPieno = LoadImporti("U", myItem.ID);
                    ListProvvedimenti.Add(myProvvedimento);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PROVVEDIMENTI.LoadListProvvedimenti::errore::", ex);
                return false;
            }
        }
    }
}