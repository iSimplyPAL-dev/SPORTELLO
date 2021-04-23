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
    public class SPC_DichICI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichICI));

        #region "Variables and constructor"
        public const decimal RiduzioneInagibile = 50;

        public SPC_DichICI()
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
        public int IDIstanza { get; set; }
        [Required]
        public int IDRifOrg { get; set; }
        [Required]
        public int IDVia { get; set; }
        [Required]
        [StringLength(255)]
        public string Via { get; set; }
        public string Ubicazione { get; set; }
        [Required]
        [StringLength(10)]
        public string Civico { get; set; }
        [Required]
        [StringLength(3)]
        public string Esponente { get; set; }
        [Required]
        [StringLength(3)]
        public string Interno { get; set; }
        [Required]
        [StringLength(3)]
        public string Scala { get; set; }
        [Required]
        [StringLength(4)]
        public string Foglio { get; set; }
        [Required]
        [StringLength(5)]
        public string Numero { get; set; }
        [Required]
        [StringLength(4)]
        public string Sub { get; set; }
        [Required]
        [StringLength(20)]
        public string Sezione { get; set; }
        [Required]
        public DateTime DataInizio { get; set; }
        [Required]
        public DateTime DataFine { get; set; }
        [Required]
        public int IDTipologia { get; set; }
        public string Tipologia { get; set; }
        [Required]
        public int IDCategoria { get; set; }
        [Required]
        public string IDClasse { get; set; }
        [Required]
        public int IDZona { get; set; }
        public string Zona { get; set; }
        [Required]
        public decimal Consistenza { get; set; }
        [Required]
        public decimal RenditaValore { get; set; }
        [Required]
        public int IDUtilizzo { get; set; }
        public string Utilizzo { get; set; }
        [Required]
        public int IDPossesso { get; set; }
        public string Possesso { get; set; }
        [Required]
        public decimal PercPossesso { get; set; }
        [Required]
        public int NUtilizzatori { get; set; }
        [Required]
        public bool IsStorico { get; set; }
        [Required]
        public decimal PercRiduzione { get; set; }
        [Required]
        public decimal PercEsenzione { get; set; }
        [Required]
        [StringLength(4)]
        public string PertFoglio { get; set; }
        [Required]
        [StringLength(5)]
        public string PertNumero { get; set; }
        [Required]
        [StringLength(4)]
        public string PertSub { get; set; }
        [Required]
        [StringLength(4000)]
        public string Note { get; set; }
        public string Stato { get; set; }
        public string DescrTipologia { get; set; }
        public string DescrCat { get; set; }
        public string DescrClasse { get; set; }
        public List<string> ListVincoli { get; set; }
        public string LinkGIS { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDIstanza = default(int);
            IDRifOrg = default(int);
            IDVia = default(int);
            Via = string.Empty;
            Ubicazione = string.Empty;
            Civico = string.Empty;
            Esponente = string.Empty;
            Interno = string.Empty;
            Scala = string.Empty;
            Foglio = string.Empty;
            Numero = string.Empty;
            Sub = string.Empty;
            Sezione = string.Empty;
            DataInizio = DateTime.MaxValue;
            DataFine = DateTime.MaxValue;
            IDTipologia = default(int);
            Tipologia = string.Empty;
            IDCategoria = default(int);
            IDClasse = string.Empty;
            IDZona = default(int);
            Zona = string.Empty;
            Consistenza = default(decimal);
            RenditaValore = default(int);
            IDUtilizzo = default(int);
            Utilizzo = string.Empty;
            IDPossesso = default(int);
            Possesso = string.Empty;
            PercPossesso = default(decimal);
            NUtilizzatori = default(int);
            IsStorico = default(bool);
            PercRiduzione = 0;
            PercEsenzione = 0;
            Note = string.Empty;
            Stato = string.Empty;
            PertFoglio = string.Empty;
            PertNumero = string.Empty;
            PertSub = string.Empty;
            DescrTipologia = string.Empty;
            DescrCat = string.Empty;
            DescrClasse = string.Empty;
            ListVincoli = new List<string>();
            LinkGIS = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_DichTASI
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichTASI));

        #region "Variables and constructor"
        public const decimal RiduzioneInagibile = 50;
         public static class TipoQuota
        {
            public static int CalcoloDaRegolamento = 1;
            public static int CalcoloTotale = 2;
        }
        
        public SPC_DichTASI()
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
        public int IDIstanza { get; set; }
        [Required]
        public int IDRifOrg { get; set; }
        [Required]
        public int IDVia { get; set; }
        [Required]
        [StringLength(1)]
        public string TipoDichiarante { get; set; }
        [Required]
        [StringLength(255)]
        public string Via { get; set; }
        public string Ubicazione { get; set; }
        [Required]
        [StringLength(10)]
        public string Civico { get; set; }
        [Required]
        [StringLength(3)]
        public string Esponente { get; set; }
        [Required]
        [StringLength(3)]
        public string Interno { get; set; }
        [Required]
        [StringLength(3)]
        public string Scala { get; set; }
        [Required]
        [StringLength(4)]
        public string Foglio { get; set; }
        [Required]
        [StringLength(5)]
        public string Numero { get; set; }
        [Required]
        [StringLength(4)]
        public string Sub { get; set; }
        [Required]
        [StringLength(20)]
        public string Sezione { get; set; }
        [Required]
        public DateTime DataInizio { get; set; }
        [Required]
        public DateTime DataFine { get; set; }
        [Required]
        public int IDTipologia { get; set; }
        public string DescrTipologia { get; set; }
        [Required]
        public int IDCategoria { get; set; }
        public string DescrCategoria { get; set; }
        [Required]
        public decimal RenditaValore { get; set; }
        [Required]
        public int IDNaturaTitolo { get; set; }
        public string DescrNaturaTitolo { get; set; }
        [Required]
        public int IDCaratteristica { get; set; }
        public string DescrCaratteristica { get; set; }
        [Required]
        public decimal PercPossesso { get; set; }
        [Required]
        public int IDAgevolazione { get; set; }
        public string DescrAgevolazione { get; set; }
        [Required]
        [StringLength(50)]
        public string AgEntrateContrattoAffitto { get; set; }
        [Required]
        [StringLength(125)]
        public string EstremiContrattoAffitto { get; set; }
        public string Note { get; set; }
        public string Stato { get; set; }
        public int TypeQuotaCalcolo {get; set;}
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDIstanza = default(int);
            IDRifOrg = default(int);
            IDVia = default(int);
            TipoDichiarante = "I";
            Via = string.Empty;
            Ubicazione = string.Empty;
            Civico = string.Empty;
            Esponente = string.Empty;
            Interno = string.Empty;
            Scala = string.Empty;
            Foglio = string.Empty;
            Numero = string.Empty;
            Sub = string.Empty;
            Sezione = string.Empty;
            DataInizio = DateTime.MaxValue;
            DataFine = DateTime.MaxValue;
            IDTipologia = default(int);
            DescrTipologia = string.Empty;
            IDCategoria = default(int);
            DescrCategoria = string.Empty;
            RenditaValore = default(int);
            IDNaturaTitolo = default(int);
            DescrNaturaTitolo = string.Empty;
            IDCaratteristica = default(int);
            DescrCaratteristica = string.Empty;
            PercPossesso = 100;
            IDAgevolazione = default(int);
            DescrAgevolazione = string.Empty;
            Note = string.Empty;
            Stato = string.Empty;
            AgEntrateContrattoAffitto = string.Empty;
            EstremiContrattoAffitto = string.Empty;
             TypeQuotaCalcolo = SPC_DichTASI.TipoQuota.CalcoloDaRegolamento;
        }
        #endregion
    }
}