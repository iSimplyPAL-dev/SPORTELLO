using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using log4net;
using System.Collections;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ContattoAnag
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ContattoAnag));
        #region "Variables and constructor"
        public ContattoAnag()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDRiferimento { get; set; }
        public string TipoRiferimento { get; set; }
        public string DatiRiferimento { get; set; }
        public string DataValiditaInvioMail { get; set; }
        public int Cod_Contribuente { get; set; }
        public int IdDataAnagrafica { get; set; }
        public string DescrContatto { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDRiferimento = default(int);
            TipoRiferimento = string.Empty;
            DatiRiferimento = string.Empty;
            DataValiditaInvioMail = string.Empty;
            Cod_Contribuente= default(int);
            IdDataAnagrafica= default(int);
            DescrContatto= string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Istanza
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Istanza));
        #region "costanti"
        public static class STATO
        {
            public static int Presentata = 1;
            public static int InCarico = 2;
            public static int Respinta = 3;
            public static int Validata = 4;
            public static int Inviata = 5;
            public static int Integrazioni = 61;
            public static int IntegrazioniFO = 62;
            public static int Protocollata = 7;
            public static int Accettata = 8;
        }
        public static class TIPO
        {
            public static string Registrazione = "Registrazione";
            public static string Anagrafica = "Anagrafica";
            public static string Delega= "Delega";
            public static string Autorizzazione = "Autorizzazione";
            [System.ComponentModel.Description("cambio solo la data di fine")]
            public static string Cessazione = "Cessazione";
            public static string ComodatoUsoGratuito = "Comodato gratuito";
            public static string Inagibilità = "Inagibilità";
            public static string Inutilizzabilità = "Inutilizzabilità";
            [System.ComponentModel.Description("cambio i dati NON sensibili al calcolo")]
            public static string Modifica = "Modifica";
            public static string NuovaDichiarazione = "Nuova Dichiarazione";
            public static string Pagamento = "Pagamento";
            [System.ComponentModel.Description("cambio i dati sensibili al calcolo")]
            public static string Variazione = "Variazione";
            public static string ConsultaDich = "Consulta Dich";
            public static string ConsultaCatasto = "Consulta Catasto";
        }
        #endregion
        #region "Variables and constructor"
        public Istanza()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDIstanza { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        public int IDContribuente { get; set; }
        [Required]
        [StringLength(4)]
        public string IDTributo { get; set; }
        [Required]
        public int IDTipo { get; set; }
        [Required]
        public DateTime DataPresentazione { get; set; }
        [Required]
        public DateTime DataInCarico { get; set; }
        [Required]
        public DateTime DataRespinta { get; set; }
        [Required]
        public DateTime DataValidata { get; set; }
        [Required]
        public DateTime DataInvioDichiarazione { get; set; }
        [Required]
        public DateTime DataIntegrazioni { get; set; }
        [Required]
        public int IDStato { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        public int NDichiarazione { get; set; }
        public string DescrEnte { get; set; }
        public string DescrTributo { get; set; }
        public string DescrTipo { get; set; }
        public string DescrStato { get; set; }
        public string ImgStato { get; set; }
        public string Nominativo { get; set; }
        public string CodFiscalePIVA { get; set; }
        public List<IstanzaMotivazione> ListMotivazioni { get; set; }
        public List<IstanzaAllegato> ListAllegati { get; set; }
        public List<IstanzaComunicazione> ListComunicazioni { get; set; }
        public object ListDatiDich { get; set; }
        public string RifCat { get; set; }
        [Required]
        public DateTime DataProtocollo { get; set; }
        public int NumeroProtocollo { get; set; }
        public string DescrTipoIstanza { get; set; }
        public int IDDelegato { get; set; }
        public string NominativoDelegato { get; set; }
        [Required]
        public DateTime DataAccettazione { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDIstanza = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDTributo = string.Empty;
            IDTipo = default(int);
            DataPresentazione = DateTime.Now;
            DataInCarico = DateTime.MaxValue;
            DataRespinta = DateTime.MaxValue;
            DataValidata = DateTime.MaxValue;
            DataInvioDichiarazione = DateTime.MaxValue;
            DataIntegrazioni= DateTime.MaxValue;
            IDStato = default(int);
            Note = string.Empty;
            NDichiarazione = default(int);
            DescrEnte = string.Empty;
            DescrTributo = string.Empty;
            DescrTipo = string.Empty;
            DescrStato = string.Empty;
            ImgStato = string.Empty;
            DataProtocollo = DateTime.MaxValue;
            NumeroProtocollo = 0;
            DescrTipoIstanza = string.Empty;
            ListMotivazioni = new List<IstanzaMotivazione>();
            ListAllegati = new List<IstanzaAllegato>();
            ListComunicazioni = new List<IstanzaComunicazione>();
            Nominativo = string.Empty;
            CodFiscalePIVA = string.Empty;
            RifCat = string.Empty;
            IDDelegato = -1;
            NominativoDelegato = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class IstanzaMotivazione
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IstanzaMotivazione));
        #region "Variables and constructor"
        public IstanzaMotivazione()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDMotivazione { get; set; }
        [Required]
        public int IDIstanza { get; set; }
        [Required]
        public int IDTipo { get; set; }
        [StringLength(4000)]
        public string Note { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDMotivazione = default(int);
            IDIstanza = default(int);
            IDTipo = default(int);
            Note = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class IstanzaAllegato
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IstanzaAllegato));
        #region "costanti"
        public static class TIPO
        {
            public static int Istanza= 1;
            public static int Dichiarazione= 2;
            public static int Comunicazione = 3;
        }
        #endregion
        #region "Variables and constructor"
        public IstanzaAllegato()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDAllegato { get; set; }
        [Required]
        public int IDIstanza { get; set; }
        [Required]
        public int IDTipo { get; set; }
        public byte[] PostedFile { get; set; }
        public string FileName { get; set; }
        public string FileMIMEType { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDAllegato = default(int);
            IDIstanza = default(int);
            IDTipo = default(int);
            PostedFile = null;
            FileName = string.Empty;
            FileMIMEType = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class IstanzaComunicazione
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(IstanzaComunicazione));
        #region "Variables and constructor"
        public IstanzaComunicazione()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDComunicazione { get; set; }
        [Required]
        public int IDIstanza { get; set; }
        [Required]
        public int IDTipo { get; set; }
        [Required]
        public DateTime Data { get; set; }
        [Required]
        public DateTime DataLettura { get; set; }
        [StringLength(4000)]
        public string Testo { get; set; }
        public List<IstanzaAllegato> ListAllegati { get; set; }
        public string DescrStato { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDComunicazione = default(int);
            IDIstanza = default(int);
            IDTipo = default(int);
            Data = DateTime.Now;
            DataLettura = DateTime.MaxValue;
            Testo = string.Empty;
            ListAllegati = new List<IstanzaAllegato>();
            DescrStato= string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class RiepilogoUI 
    {
         #region "Variables and constructor"
        public RiepilogoUI()
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
        public int IDRifOrg { get; set; }
        public string Stato { get; set; }
        public string Ubicazione { get; set; }
        public string Foglio { get; set; }
        public string Numero { get; set; }
        public string Sub { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public decimal MQ { get; set; }
        public decimal MQCatasto { get; set; }
        public string CodCategoria { get; set; }
        public string RidEse { get; set; }
        public decimal RenditaValore { get; set; }
        public decimal PercPossesso { get; set; }
        public string DescrUtilizzo { get; set; }
        public string Zona { get; set; }
        public decimal ImpDovuto { get; set; }
        public string IsFromDich { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            IDRifOrg = default(int);
            Stato = string.Empty;
            Ubicazione = string.Empty;
            Foglio = string.Empty;
            Numero = string.Empty;
            Sub = string.Empty;
            Dal =DateTime.MaxValue;
            Al =DateTime.MaxValue;
            MQ = default(decimal);
            MQCatasto = default(decimal);
            CodCategoria = string.Empty;
            RidEse = string.Empty;
            RenditaValore = default(decimal);
            PercPossesso = default(decimal);
            DescrUtilizzo = string.Empty;
            Zona = string.Empty;
            ImpDovuto = default(decimal);
            IsFromDich = "NO";
        }        
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class RiepilogoDebito
    {
        #region "Variables and constructor"
        public RiepilogoDebito()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int IDIstanza { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        public int IDContribuente { get; set; }
        public decimal Dovuto { get; set; }
        public decimal Pagato { get; set; }
        public decimal Insoluto { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            IDIstanza = default(int);
            IDEnte = string.Empty;
            IDContribuente = default(int);
            Dovuto = 0;
            Pagato = 0;
            Insoluto = 0;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class DatiF24
    {
        #region "Variables and constructor"
        public DatiF24()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string CFPIVA { get; set; }
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string DataNascita { get; set; }
        public string Sesso { get; set; }
        public string ComuneNasc { get; set; }
        public string PVNasc { get; set; }
        public string IDOperazione { get; set; }
        public string Sezione { get; set; }
        public string idtributo { get; set; }
        public string ente { get; set; }
        public int ravvedimento { get; set; }
        public int uivar { get; set; }
        public int acc { get; set; }
        public int sal { get; set; }
        public string nui { get; set; }
        public string rateazione { get; set; }
        public string anno { get; set; }
        public string impdet { get; set; }
        public string impdeb { get; set; }
        public string impcred { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            CFPIVA = string.Empty;
            Cognome = string.Empty;
            Nome = string.Empty;
            DataNascita = string.Empty;
            Sesso = string.Empty;
            ComuneNasc = string.Empty;
            PVNasc = string.Empty;
            IDOperazione = string.Empty;
            Sezione = string.Empty;
            idtributo = string.Empty;
            ente = string.Empty;
            ravvedimento = 0;
            uivar =0;
            acc =0;
            sal = 0;
            nui = string.Empty;
            rateazione = string.Empty;
            anno = string.Empty;
            impdet = string.Empty;
            impdeb = string.Empty;
            impcred = string.Empty;
        }
        #endregion
    }
}