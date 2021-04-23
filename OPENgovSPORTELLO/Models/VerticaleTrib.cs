using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class RiepilogoUIVerticale
    {
        #region "Variables and constructor"
        public RiepilogoUIVerticale()
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
        public int IDTestata { get; set; }
        public int IDRifOrg { get; set; }
        public string Nominativo { get; set; }
        public string RifCat { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Sub { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public string Ubicazione { get; set; }
        public decimal Quota { get; set; }
        public string DescrCategoria { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDTestata = default(int);
            IDRifOrg = default(int);
            Nominativo = string.Empty;
            Ubicazione = string.Empty;
            RifCat = string.Empty;
            Foglio = string.Empty;
            Numero = string.Empty;
            Sub = string.Empty;
            Dal = DateTime.MaxValue;
            Al = DateTime.MaxValue;
            DescrCategoria = string.Empty;
            Quota = default(decimal);
        }
        #endregion
    }
}