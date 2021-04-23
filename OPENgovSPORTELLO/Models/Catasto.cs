using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CatastoBySoggetto
    {
        #region "Variables and constructor"
        public CatastoBySoggetto()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public string stato { get; set; }
        public Soggetto soggetto { get; set; }
        public List<Fabbricato> fabbricati { get; set; }
        public List<Terreno> terreni { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Soggetto
    {
        #region "Variables and constructor"
        public Soggetto()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public string cognome { get; set; }
        public string nome { get; set; }
        public string datanascita { get; set; }
        public string codfiscale { get; set; }
        public string nominativo { get; set; }
        public string luogo_nascita { get; set; }
        public string sede { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Fabbricato
    {
        #region "Variables and constructor"
        public Fabbricato()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public int identifmutazini { get; set; }
        public string comune { get; set; }
        public string codcomune { get; set; }
        public Intestatario intestatari { get; set; }
        public List<Intestatario> item_intestatari { get; set; }
        public List<DatiFabbricato> item_fabbricati { get; set; }
        //public UnitaImmobiliare unita_immobiliari{get;set;}
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Terreno
    {
        #region "Variables and constructor"
        public Terreno()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public string identifmutazini { get; set; }
        public string decodifica { get; set; }
        public string codcomune { get; set; }
        public List<Intestatario> intestatari { get; set; }
        public List<Intestatario> item_intestatari { get; set; }
        public List<DatiTerreno> item_terreni { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
     public class Intestatario
    {
        #region "Variables and constructor"
        public Intestatario()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public int quotanum { get; set; }
        public int quotaden { get; set; }
        public string codfiscale { get; set; }
        public string tipopersona { get; set; }
        public string cognome { get; set; }
        public string nome { get; set; }
        public string datanascita { get; set; }
        public string luogonascita { get; set; }
        public string denominazione { get; set; }
        public string sede { get; set; }
        public string decodifica { get; set; }
        public string dec_sede { get; set; }
        public string diritto { get; set; }
        public string coddiritto { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class DatiFabbricato
    {
        #region "Variables and constructor"
        public DatiFabbricato()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public string sezione { get; set; }
        public string foglio { get; set; }
        public string numero { get; set; }
        public string subalterno { get; set; }
        public string zona { get; set; }
        public string categoria { get; set; }
        public string classe { get; set; }
        public string consistenza { get; set; }
        public string superficie { get; set; }
        public string renditaeuro { get; set; }
        public string piano1 { get; set; }
        public string piano2 { get; set; }
        public string piano3 { get; set; }
        public string piano4 { get; set; }
        public string interno1 { get; set; }
        public string interno2 { get; set; }
        public string indirizzo { get; set; }
        public string civico1 { get; set; }
        public string civico2 { get; set; }
        public string civico3 { get; set; }
        public string tiponota { get; set; }
        public string decodifica { get; set; }
        public string dataefficacia_iniziale { get; set; }
        public string dataefficacia_finale { get; set; }
        public string annotazione { get; set; }
        public string foglio_s { get; set; }
        public string numero_s { get; set; }
        public string subalterno_s { get; set; }
        public string superficie_tarsu { get; set; }
        public string link_mappa { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class DatiTerreno
    {
        #region "Variables and constructor"
        public DatiTerreno()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public int idparticella { get; set; }
        public int progr { get; set; }
        public string codcomune { get; set; }
        public string sezione { get; set; }
        public string idporzione { get; set; }
        public string foglio_s { get; set; }
        public string foglio { get; set; }
        public string numero_s { get; set; }
        public string numero { get; set; }
        public string denominatore { get; set; }
        public string subalterno_s { get; set; }
        public string subalterno { get; set; }
        public string edificialita { get; set; }
        public string partkey { get; set; }
        public string codqualita { get; set; }
        public string classe { get; set; }
        public string ettari { get; set; }
        public string are { get; set; }
        public string centiare { get; set; }
        public decimal superficie { get; set; }
        public string flagreddito { get; set; }
        public string flagporzione { get; set; }
        public string flagdeduzioni { get; set; }
        public string redditodomlire { get; set; }
        public string redditoagrlire { get; set; }
        public decimal redditodomeuro { get; set; }
        public decimal redditoagreuro { get; set; }
        public string partita { get; set; }
        public string annotazione { get; set; }
        public int idmutazioneiniziale { get; set; }
        public int idmutazionefinale { get; set; }
        public string natura { get; set; }
        public string codiceesito { get; set; }
        public string dataefficacia_inizio { get; set; }
        public string dataefficacia_finale { get; set; }
        public string qualita_unica { get; set; }
        public string decodifica { get; set; }
        public string numeronota { get; set; }
        public string dataregistrazione { get; set; }
        public string ubicazione { get; set; }
        public List<DatiUrbanistici> item_strumenti_urbanistici { get; set; }
        public string link_mappa { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class DatiUrbanistici
    {
        #region "Variables and constructor"
        public DatiUrbanistici()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public int ID { get; set; }
        public string cat_foglio { get; set; }
        public string cat_mappale { get; set; }
        public string ctg_cod { get; set; }
        public string zon_cod { get; set; }
        public string cat_area { get; set; }
        public string cdu_area { get; set; }
        public string cdu_percentuale { get; set; }
        public string sup_cat { get; set; }
        public string ctg_descr { get; set; }
        public string zon_descr { get; set; }
        public string link_mappa { get; set; }
        public string prg_nta { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
        }
        #endregion
    }
}

