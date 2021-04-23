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
    public class SPC_DichICP
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichICP));
        #region "Variables and constructor"
        public SPC_DichICP()
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
        public DateTime DataAtto { get; set; }
        [Required]
        [StringLength(50)]
        public string NAtto { get; set; }
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
        public int IDTipologia { get; set; }
        public string DescrTipologia { get; set; }
        [Required]
        public int IDCaratteristica { get; set; }
        public string DescrCaratteristica { get; set; }
        [Required]
        public int IDTipoDurata { get; set; }
        public string DescrTipoDurata { get; set; }
        [Required]
        public string Mezzo { get; set; }
        [Required]
        public decimal Qta { get; set; }
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
            DataAtto =DateTime.MaxValue;
            NAtto = string.Empty;
            IDVia = default(int);
            Via = string.Empty;
            Ubicazione = string.Empty;
            Civico = string.Empty;
            DataInizio = DateTime.MaxValue;
            DataFine = DateTime.MaxValue;
            IDTipologia = default(int);
            DescrTipologia = string.Empty;
            IDCaratteristica = default(int);
            DescrCaratteristica = string.Empty;
            IDTipoDurata = default(int);
            DescrTipoDurata = string.Empty;
            Mezzo= string.Empty;
            Qta= 0;
            Stato = string.Empty;
        }
        #endregion
    }
}