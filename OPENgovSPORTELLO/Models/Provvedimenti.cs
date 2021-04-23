using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.ComponentModel.DataAnnotations;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class SPC_Provvedimento
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_Provvedimento));
        #region "Variables and constructor"
        public SPC_Provvedimento()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        public int IDContribuente { get; set; }
        [Required]
        public string IDTributo { get; set; }
        public string Anno { get; set; }
        public string Descrizione { get; set; }
        public DateTime DataNotifica { get; set; }
        public decimal Dovuto { get; set; }
        public decimal Pagato { get; set; }
        public string Stato { get; set; }
        public List<SPC_UIProvvedimento> ListDichiarato { get; set; }
        public List<SPC_UIProvvedimento> ListAccertato { get; set; }
        public List<SPC_SanzioniProvvedimento> ListSanzioni { get; set; }
        public List<SPC_InteressiProvvedimento> ListInteressi { get; set; }
        public SPC_ImportiProvvedimento ImpRidotto { get; set; }
        public SPC_ImportiProvvedimento ImpPieno { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDTributo = string.Empty;
            Anno = string.Empty;
            Descrizione = string.Empty;
            DataNotifica = DateTime.MaxValue;
            Dovuto = 0;
            Pagato = 0;
            Stato = string.Empty;
            ListDichiarato = new List<SPC_UIProvvedimento>();
            ListAccertato = new List<SPC_UIProvvedimento>();
            ListSanzioni = new List<SPC_SanzioniProvvedimento>();
            ListInteressi = new List<SPC_InteressiProvvedimento>();
            ImpRidotto = new SPC_ImportiProvvedimento();
            ImpPieno = new SPC_ImportiProvvedimento();
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_UIProvvedimento
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_UIProvvedimento));
        #region "Variables and constructor"
        public SPC_UIProvvedimento()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string IDTributo { get; set; }
        public string Ubicazione { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public string Durata { get; set; }
        public string DescrCategoria { get; set; }
        public decimal Aliquota { get; set; }
        public string MQ { get; set; }
        public string DescrRiduzioni { get; set; }
        public decimal Importo { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDTributo = string.Empty;
            Ubicazione = string.Empty;
            DataInizio = DateTime.MaxValue;
            DataFine = DateTime.MaxValue;
            Durata = string.Empty;
            DescrCategoria = string.Empty;
            Aliquota = 0;
            MQ = "0";
            DescrRiduzioni = string.Empty;
            Importo = 0;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_SanzioniProvvedimento
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_SanzioniProvvedimento));
        #region "Variables and constructor"
        public SPC_SanzioniProvvedimento()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Descrizione { get; set; }
        public string Motivazione { get; set; }
        public decimal Importo { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Descrizione = string.Empty;
            Motivazione = string.Empty;
            Importo = 0;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_InteressiProvvedimento
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_InteressiProvvedimento));
        #region "Variables and constructor"
        public SPC_InteressiProvvedimento()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public decimal Aliquota { get; set; }
        public decimal Importo { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            DataInizio = DateTime.MaxValue;
            DataFine = DateTime.MaxValue;
            Aliquota = 0;
            Importo = 0;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_ImportiProvvedimento
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_ImportiProvvedimento));
        #region "Variables and constructor"
        public SPC_ImportiProvvedimento()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public decimal DiffImposta { get; set; }
        public decimal Interessi { get; set; }
        public decimal Sanzioni { get; set; }
        public decimal SanzioniNonRid { get; set; }
        public decimal Arrotondamento { get; set; }
        public decimal SpeseNotifica { get; set; }
        public decimal Totale { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            DiffImposta = 0;
            Interessi = 0;
            Sanzioni = 0;
            SanzioniNonRid = 0;
            Arrotondamento = 0;
            SpeseNotifica = 0;
            Totale = 0;
        }
        #endregion
    }
}