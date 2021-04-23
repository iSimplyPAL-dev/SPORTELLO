using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ManageForm
    {
        #region "Variables and constructor"
        public ManageForm()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDManage { get; set; }
        [Required]
        [StringLength(50)]
        public string NomeForm { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        [StringLength(50)]
        public string NomeControllo { get; set; }
        [Required]
        [StringLength(100)]
        public string Testo { get; set; }
        [Required]
        public int IsVisible { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDManage = default(int);
            NomeForm = string.Empty;
            IDEnte = string.Empty;
            NomeControllo = string.Empty;
            Testo = string.Empty;
            IsVisible = default(int);
        }
        #endregion
    }
}