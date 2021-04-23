using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using log4net;
using System.Data.SqlClient;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CategorieTARSU
    {
        #region "Variables and constructor"
        public CategorieTARSU()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDCat { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        public string Codice { get; set; }
        [Required]
        public string Descrizione { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDCat = default(int);
            IDEnte = string.Empty;
            Codice = string.Empty;
            Descrizione = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class RidEseTARSU
    {
        #region "Variables and constructor"
        public RidEseTARSU()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDRidEse { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string Codice { get; set; }
        [Required]
        public string Descrizione { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDRidEse = default(int);
            IDEnte = string.Empty;
            Tipo = string.Empty;
            Codice = string.Empty;
            Descrizione = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class RiepilogoDovuto
    {
        #region "Variables and constructor"
        public RiepilogoDovuto()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDDovuto { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        public int IDContribuente { get; set; }
        public string Anno { get; set; }
        public decimal Fissa { get; set; }
        public decimal Variabile { get; set; }
        public decimal Conferimenti { get; set; }
        public decimal Imposta { get; set; }
        public decimal Provinciale { get; set; }
        public decimal Maggiorazione { get; set; }
        public decimal Arrotondamento { get; set; }
        public decimal Dovuto { get; set; }
        public decimal Versato { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDDovuto = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            Anno = string.Empty;
            Fissa = default(decimal);
            Variabile = default(decimal);
            Conferimenti = default(decimal);
            Imposta = default(decimal);
            Provinciale = default(decimal);
            Maggiorazione = default(decimal);
            Arrotondamento = default(decimal);
            Dovuto = default(decimal);
            Versato = default(decimal);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_DichTARSU 
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichTARSU));

        #region "Variables and constructor"
        public SPC_DichTARSU()
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
        public DateTime DataInizio { get; set; }
        [Required]
        public DateTime DataFine { get; set; }
        [Required]
        public string IDStatoOccupazione { get; set; }
        public string StatoOccupazione { get; set; }
        [Required]
        public int IDTipoVano { get; set; }
        [Required]
        public int IDCatTIA { get; set; }
        [Required]
        public string CodCategoria { get; set; }
        [Required]
        public string ScopeCat { get; set; }
        [Required]
        public int NComponenti { get; set; }
        [Required]
        public int NComponentiPV { get; set; }
        [Required]
        public int NVani { get; set; }
        [Required]
        public decimal MQ { get; set; }
        [Required]
        public int IsEsente { get; set; }
        [Required]
        [StringLength(4000)]
        public string Note { get; set; }
        public string Stato { get; set; }
        public string Tipo { get; set; }
        public List<SPC_DichTARSUVani> ListVani { get; set; }
        public List<SPC_DichTARSURidEse> ListRid { get; set; }
        public List<SPC_DichTARSURidEse> ListEse { get; set; }
        public List<SPC_DichTARSUOccupanti> ListOccupanti { get; set; }
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
            Civico = string.Empty;
            Esponente = string.Empty;
            Interno = string.Empty;
            Scala = string.Empty;
            Foglio = string.Empty;
            Numero = string.Empty;
            Sub = string.Empty;
            DataInizio =DateTime.MaxValue;
            DataFine =DateTime.MaxValue;
            IDStatoOccupazione = string.Empty;
            StatoOccupazione = string.Empty;
            IDTipoVano = default(int);
            IDCatTIA = default(int);
            CodCategoria= string.Empty;
            ScopeCat = string.Empty;
            NComponenti = default(int);
            NComponentiPV = default(int);
            NVani = default(int);
            MQ = default(decimal);
            IsEsente = default(int);
            Note = string.Empty;
            Stato = string.Empty;
            ListRid = new List<SPC_DichTARSURidEse>();
            ListEse = new List<SPC_DichTARSURidEse>();
            ListOccupanti = new List<SPC_DichTARSUOccupanti>();
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_DichTARSUVani
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichTARSUVani));

        #region "Variables and constructor"
        public SPC_DichTARSUVani()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        [Required]
        public int IDDichTARSU { get; set; }
        [Required]
        public int IDTipoVano { get; set; }
        [Required]
        public int IDCatTIA { get; set; }
        [Required]
        public string CodCategoria { get; set; }
        [Required]
        public string ScopeCat { get; set; }
        [Required]
        public int NComponenti { get; set; }
        [Required]
        public int NComponentiPV { get; set; }
        [Required]
        public int NVani { get; set; }
        [Required]
        public decimal MQ { get; set; }
        [Required]
        public int IsEsente { get; set; }
        public string ScopeCatDescr { get; set; }
        public string CodCategoriaDescr { get; set; }
        public string CategoriaEstesa { get; set; }
        public string DescrVano { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDDichTARSU = default(int);
            IDTipoVano = default(int);
            IDCatTIA = default(int);
            CodCategoria = string.Empty;
            ScopeCat = "D";
            NComponenti = default(int);
            NComponentiPV = default(int);
            NVani = default(int);
            MQ = default(decimal);
            IsEsente = 0;
            ScopeCatDescr = string.Empty;
            CodCategoriaDescr = string.Empty;
            CategoriaEstesa = string.Empty;
            DescrVano = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_DichTARSURidEse
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichTARSURidEse));

        #region "Variables and constructor"
        public SPC_DichTARSURidEse()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public static class TYPE
        {
            public static string Riduzione = "R";
            public static string Esenzione = "E";
        }
        [Key]
        public int ID { get; set; }
        [Required]
        public int IDDichTARSU { get; set; }
        [Required]
        public string IDType { get; set; }
        [Required]
        public string Codice { get; set; }
        public string Descrizione { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDDichTARSU = default(int);
            IDDichTARSU = default(int);
            IDType = string.Empty;
            Codice = string.Empty;
            Descrizione = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_DichTARSUOccupanti
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichTARSUOccupanti));

        #region "Variables and constructor"
        public SPC_DichTARSUOccupanti()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        [Required]
        public int IDDichTARSU { get; set; }
        [Required]
        public string Nominativo { get; set; }
        public string CodFiscale { get; set; }
        public string LuogoNascita { get; set; }
        public DateTime DataNascita { get; set; }
        [Required]
        public int IDParentela { get; set; }
        public string DescrParentela { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDDichTARSU = default(int);
            Nominativo = string.Empty;
            CodFiscale = string.Empty;
            LuogoNascita = string.Empty;
            DataNascita = DateTime.MaxValue;
            IDParentela = default(int);
            DescrParentela = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class SPC_DichTESSERE
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SPC_DichTESSERE));

        #region "Variables and constructor"
        public SPC_DichTESSERE()
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
        [StringLength(50)]
        public string NTessera { get; set; }
        [Required]
        public int IDTipoTessera { get; set; }
        [Required]
        [StringLength(255)]
        public string DescrTipoTessera { get; set; }
        [Required]
        public DateTime DataInizio { get; set; }
        [Required]
        public DateTime DataFine { get; set; }
        [Required]
        public int NConferimenti { get; set; }
        [Required]
        public decimal Litri { get; set; }
        public List<EventiRaffronto> ListConferimenti { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            NTessera = string.Empty;
            IDTipoTessera = default(int);
            DescrTipoTessera = string.Empty;
            DataInizio = DateTime.MaxValue;
            DataFine = DateTime.MaxValue;
            NConferimenti = default(int);
            Litri = default(decimal);
            ListConferimenti = new List<EventiRaffronto>();
        }
        #endregion
    }
}