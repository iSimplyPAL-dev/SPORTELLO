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
    public class SPC_DichOSAP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichOSAP));
        #region "Variables and constructor"
        public SPC_DichOSAP()
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
        public int IDTipoAtto { get; set; }
        public string DescrTipoAtto { get; set; }
        [Required]
        public DateTime DataAtto { get; set; }
        [Required]
        [StringLength(50)]
        public string NAtto { get; set; }
        [Required]
        public int IDRichiedente { get; set; }
        public string DescrRichiedente { get; set; }
        [Required]
        public int IDTributo { get; set; }
        public string DescrTributo { get; set; }
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
        public DateTime DataInizio { get; set; }
        [Required]
        public DateTime DataFine { get; set; }
        [Required]
        public int IDTipoDurata { get; set; }
        public string DescrTipoDurata { get; set; }
        [Required]
        public int Durata { get; set; }
        [Required]
        public int IDCategoria { get; set; }
        public string DescrCategoria { get; set; }
        [Required]
        public int IDOccupazione { get; set; }
        public string DescrOccupazione { get; set; }
        [Required]
        public int IDConsistenza { get; set; }
        public string DescrConsistenza { get; set; }
        [Required]
        public bool IsAttrazione { get; set; }
        [Required]
        public decimal Consistenza { get; set; }
        [Required]
        public decimal PercMagg { get; set; }
        [Required]
        public decimal ImpDetraz { get; set; }
        public List<GenericCategory> ListAgevolazioni { get; set; }
        public string Stato { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDIstanza = default(int);
            IDRifOrg = default(int);
            IDTipoAtto = default(int);
            DescrTipoAtto = string.Empty;
            DataAtto =DateTime.MaxValue;
            NAtto = string.Empty;
            IDRichiedente = default(int);
            DescrRichiedente = string.Empty;
            IDTributo = default(int);
            DescrTributo = string.Empty;
            IDVia = default(int);
            Via = string.Empty;
            Ubicazione = string.Empty;
            Civico = string.Empty;
            DataInizio =DateTime.MaxValue;
            DataFine =DateTime.MaxValue;
            IDTipoDurata = default(int);
            DescrTipoDurata = string.Empty;
            Durata = default(int);
            IDCategoria = default(int);
            DescrCategoria = string.Empty;
            IDOccupazione = default(int);
            DescrOccupazione = string.Empty;
            IsAttrazione = default(bool);
            IDConsistenza = default(int);
            DescrConsistenza = string.Empty;
            Consistenza = 0;
            PercMagg = 0;
            ImpDetraz = 0;
            ListAgevolazioni = new List<GenericCategory>();
            Stato = string.Empty;
        }
        #endregion
    }
}