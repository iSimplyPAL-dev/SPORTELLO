using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using log4net;


namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class idCodice
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Comuni));

        #region "Variables and constructor"
        public idCodice()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
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
    public class GenericCategory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GenericCategory));

        #region "Constant"
        public static class TIPO
        {
            public static string Enti = "01";
            public static string Stradario = "02";
            public static string Operatori = "03";
            public static string Documenti = "04";
            public static string LegameParentela = "05";
            public static string MsgRecipient = "06";
            public static string TARSU_Categorie = "50";
            public static string TARSU_StatoOccupazione = "51";
            public static string TARSU_Riduzioni = "52";
            public static string TARSU_Esenzioni = "53";
            public static string TARSU_Motivazioni = "54";
            public static string TARSU_Vani = "55";
            public static string ICI_Caratteristica = "60";
            public static string ICI_Utilizzo = "61";
            public static string ICI_Possesso = "62";
            public static string ICI_Categorie = "63";
            public static string ICI_Zone = "64";
            public static string ICI_Motivazioni = "65";
            public static string ICI_Vincoli = "66";
            public static string ICI_Aliquote = "67";
            public static string ICI_Classe = "68";
            public static string OSAP_TipoDichiarazione = "70";
            public static string OSAP_Tributo = "71";
            public static string OSAP_TipoDurata = "72";
            public static string OSAP_TipoConsistenza = "73";
            public static string OSAP_Richiedente = "74";
            public static string OSAP_Categoria = "75";
            public static string OSAP_Occupazione = "76";
            public static string OSAP_Agevolazioni = "77";
            public static string OSAP_Motivazioni = "78";
            public static string ICP_Tipologia = "80";
            public static string ICP_Caratteristica = "81";
            public static string ICP_TipoDurata = "82";
            public static string ICP_Motivazioni = "83";
            public static string TASI_NaturaTitolo = "90";
            public static string TASI_Caratteristica = "91";
            public static string TASI_Agevolazioni = "92";
            public static string TASI_Aliquote = "93";
            public static string TASI_Motivazioni = "94";
        }
        #endregion
        #region "Variables and constructor"
        public GenericCategory()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(2)]
        public string IDTipo { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        [StringLength(4)]
        public string IDTributo { get; set; }
        [Required]
        public int Anno { get; set; }
        [Required]
        public string Codice { get; set; }
        [Required]
        public string Descrizione { get; set; }
        [Required]
        public string IDOrg { get; set; }
        public int IsActive { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            IDTipo = string.Empty;
            IDEnte = string.Empty;
            IDTributo = string.Empty;
            Anno = default(int);
            Codice = string.Empty;
            Descrizione = string.Empty;
            IDOrg = string.Empty;
            IsActive = 0;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class GenericCategoryWithRate
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(GenericCategoryWithRate));

        #region "Variables and constructor"
        public GenericCategoryWithRate()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(2)]
        public string IDTipo { get; set; }
        [Required]
        [StringLength(6)]
        public string IDEnte { get; set; }
        [Required]
        [StringLength(4)]
        public string IDTributo { get; set; }
        [Required]
        public int Anno { get; set; }
        [Required]
        public string Codice { get; set; }
        [Required]
        public string Descrizione { get; set; }
        [Required]
        public decimal Valore { get; set; }
        [Required]
        public decimal PercProprietario { get; set; }
        [Required]
        public decimal PercInquilino { get; set; }
        [Required]
        public string IDOrg { get; set; }
        public int IsActive { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            IDTipo = string.Empty;
            IDEnte = string.Empty;
            IDTributo = string.Empty;
            Anno = default(int);
            Codice = string.Empty;
            Descrizione = string.Empty;
            Valore = default(decimal);
            PercProprietario = 100;
            PercInquilino = 0;
            IDOrg = string.Empty;
            IsActive = 0;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Comuni
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Comuni));

        #region "Variables and constructor"
        public Comuni()
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
        [StringLength(50)]
        public string Descrizione { get; set; }
        [Required]
        [StringLength(5)]
        public string CAP { get; set; }
        [Required]
        [StringLength(2)]
        public string Provincia { get; set; }
        [Required]
        [StringLength(4)]
        public string CodCatastale { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            Descrizione = string.Empty;
            CAP = string.Empty;
            Provincia = string.Empty;
            CodCatastale = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class EntiInLavorazione
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EntiInLavorazione));

        #region "Variables and constructor"
        public EntiInLavorazione()
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
        [StringLength(50)]
        public string Descrizione { get; set; }
        [Required]
        [StringLength(128)]
        public string Ambiente { get; set; }
        [Required]
        [StringLength(5)]
        public string CAP { get; set; }
        [Required]
        [StringLength(2)]
        public string Provincia { get; set; }
        [Required]
        [StringLength(4)]
        public string CodCatastale { get; set; }
        [Required]
        [StringLength(50)]
        public string MailEnte { get; set; }
        public List<GenericCategory> ListTributi { get; set; }
        public BaseLogo Logo { get; set; }
        public BaseMail Mail { get; set; }
        public BaseCartografia SIT { get; set; }
        public BaseVerticali DatiVerticali { get; set; }
        public string UrlWiki { get; set; }
        public string UrlConferimenti { get; set; }
        public int SplitPWD { get; set; }
        public BasePagoPA DatiPagoPA { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            Descrizione = string.Empty;
            Ambiente = string.Empty;
            CAP = string.Empty;
            Provincia = string.Empty;
            CodCatastale = string.Empty;
            MailEnte = string.Empty;
            ListTributi = new List<GenericCategory>();
            Logo = new BaseLogo();
            Mail = new BaseMail();
            SIT = new BaseCartografia();
            DatiVerticali = new BaseVerticali();
            UrlWiki = MySettings.GetConfig("UrlWiki");
            UrlConferimenti = MySettings.GetConfig("UrlConferimenti");
            SplitPWD = default(int);
            DatiPagoPA = new BasePagoPA();
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserRole
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserRole));

        #region "Constant"
        public static class PROFILO
        {
            public static int Amministratore = 1;
            public static int ResponsabileEnte = 2;
            public static int OperatoreCS = 3;
            public static int OperatoreEnte = 4;
            public static int UtenteFrontEnd = 5;
        }
        #endregion
        #region "Variables and constructor"
        public UserRole()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        [StringLength(128)]
        public string ID { get; set; }
        [Required]
        [StringLength(50)]
        public string NameUser { get; set; }
        [Required]
        public int IDTipoProfilo { get; set; }
        [Required]
        public List<string> Enti { get; set; }
        [Required]
        public List<string> Tributi { get; set; }
        [Required]
        public string CFPIVA { get; set; }
        public int IDContribLogged { get; set; }
        public int IDContribToWork { get; set; }
        public int IDDelegato { get; set; }
        public string Nominativo { get; set; }
        public string ListDeleganti { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(string);
            NameUser = string.Empty;
            Enti = new List<string>();
            Tributi = new List<string>();
            IDTipoProfilo = -1;
            CFPIVA = "";
            IDContribLogged = -1;
            IDContribToWork = -1;
            IDDelegato = -1;
            Nominativo = string.Empty;
            ListDeleganti = string.Empty;
            PasswordHash = string.Empty;
            SecurityStamp = string.Empty;
            LastPasswordChangedDate = DateTime.MaxValue;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class UserRoleStampa
    {
        #region "Variables and constructor"
        public UserRoleStampa()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        public string NameUser { get; set; }
        public string CFPIVA { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            NameUser = CFPIVA = string.Empty;
            LastPasswordChangedDate = DateTime.MaxValue;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class DocToAttach
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DocToAttach));

        #region "Variables and constructor"
        public DocToAttach()
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
        [StringLength(4)]
        public string IDTributo { get; set; }
        [Required]
        public int IDTipoIstanza { get; set; }
        [Required]
        [StringLength(250)]
        public string Documento { get; set; }
        [Required]
        public bool IsObbligatorio { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDEnte = string.Empty;
            IDTributo = string.Empty;
            IDTipoIstanza = default(int);
            Documento = string.Empty;
            IsObbligatorio = default(bool);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseLogo
    {
        #region "Variables and constructor"
        public BaseLogo()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string NameLogo { get; set; }
        public byte[] PostedFile { get; set; }
        public string FileMIMEType { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            NameLogo = string.Empty;
            PostedFile = null;
            FileMIMEType = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseMail
    {
        #region "Variables and constructor"
        public BaseMail()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public int SSL { get; set; }
        public string Server { get; set; }
        public string ServerPort { get; set; }
        public string Password { get; set; }
        public string Ente { get; set; }
        public string Archive { get; set; }
        public string BackOffice { get; set; }
        public string Protocollo { get; set; }
        public string WarningRecipient { get; set; }
        public string WarningSubject { get; set; }
        public string WarningMessage { get; set; }
        public string SendErrorMessage { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            Sender = My.MySettings.mailSender;
            SenderName = My.MySettings.mailSenderName;
            SSL = (My.MySettings.mailSSL ? 1 : 0);
            Server = My.MySettings.mailServer;
            ServerPort = My.MySettings.mailServerPort;
            Password = My.MySettings.mailPassword;
            Ente = string.Empty;
            Archive = My.MySettings.MailArchive;
            BackOffice = My.MySettings.MailBackOffice;
            Protocollo = My.MySettings.MailProtocollo;
            WarningRecipient = My.MySettings.MailWarningRecipient;
            WarningSubject = My.MySettings.MailWarningSubject;
            WarningMessage = My.MySettings.MailWarningMessage;
            SendErrorMessage = My.MySettings.MailSendErrorMessage;
        }

        public static implicit operator List<object>(BaseMail v)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseCartografia
    {
        #region "Variables and constructor"
        public BaseCartografia()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Url { get; set; }
        public string UrlAuth { get; set; }
        public string Token { get; set; }
        public int IsActive { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            Url = MySettings.GetConfig("UrlCatasto");
            UrlAuth = MySettings.GetConfig("UrlCatastoAuth");
            Token = MySettings.GetConfig("TokenAuthCatasto");
            IsActive = (bool.Parse(MySettings.GetConfig("VisualCatasto")) ? 1 : 0);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class BaseVerticali
    {
        #region "Variables and constructor"
        public BaseVerticali()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public int AnnoVerticaleICI { get; set; }
        public string AnniUsoGratuito { get; set; }
        public DateTime DataAggiornamento { get; set; }
        public string TipoBancaDati { get; set; }
        public string TipoFornitore { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            AnnoVerticaleICI = default(int);
            AnniUsoGratuito = string.Empty;
            DataAggiornamento = DateTime.MaxValue;
            TipoBancaDati = "E";
            TipoFornitore = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class BasePagoPA
    {
        public BasePagoPA()
        {
            Reset();
        }
        [Key]
        public int ID { get; set; }
        public string EndpointOTF { get; set; }
        public string EndpointVerificaStato { get; set; }
        public string EndpointUser { get; set; }
        public string EndpointPwd { get; set; }
        public string CARTId { get; set; }
        public string CARTSys { get; set; }
        public string IdRiscossore { get; set; }
        public string DescrRiscossore { get; set; }
        public string IBAN { get; set; }
        public string DescrIBAN { get; set; }
        public int HScadenza { get; set; }
        public void Reset()
        {
            ID = -1;
            EndpointOTF = string.Empty;
            EndpointVerificaStato = string.Empty;
            EndpointUser = string.Empty;
            EndpointPwd = string.Empty;
            CARTId = string.Empty;
            CARTSys = string.Empty;
            IdRiscossore = string.Empty;
            DescrRiscossore = string.Empty;
            IBAN = string.Empty;
            DescrIBAN = string.Empty;
            HScadenza = 0;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Message
    {
        #region "Constant"
        public static class MEZZO
        {
            public static string Sito = "S";
            public static string App = "A";
            public static string Mail = "M";
        }
        #endregion
        #region "Variables and constructor"
        public Message()
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
        public int IDTypeRecipient { get; set; }
        [Required]
        public string SubsetRecipient { get; set; }
        [Required]
        [StringLength(3)]
        public string TypeMezzo { get; set; }
        [Required]
        public DateTime DataInvio { get; set; }
        [Required]
        [StringLength(250)]
        public string Testo { get; set; }
        public int NSend { get; set; }
        public int NRead { get; set; }
        public string DescrEnte { get; set; }
        public string DescrDestinatari { get; set; }
        public string DescrMezzo { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            IDEnte = string.Empty;
            IDTypeRecipient = -1;
            SubsetRecipient = string.Empty;
            TypeMezzo = string.Empty;
            DataInvio = DateTime.MaxValue;
            Testo = string.Empty;
            NSend = 0;
            NRead = 0;
            DescrEnte = string.Empty;
            DescrDestinatari = string.Empty;
            DescrMezzo = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class Delega
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Delega));

        #region "Constant"
        public static class TipoStato
        {
            public static int Nuova = 0;
            public static int InAttesa = 1;
            public static int Validata = 2;
            public static int Cancellata = 3;
            public static int Respinta = 4;
        }
        public static class OrigineCessazione
        {
            public static int Delegato = 1;
            public static int Delegante = 2;
            public static int Subentro = 3;
        }
        #endregion
        #region "Variables and constructor"
        public Delega()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public int IDDelegante { get; set; }
        public int IDDelegato { get; set; }
        public int IDIstanza { get; set; }
        public int IDStato { get; set; }
        public int IDOrigineCessazione { get; set; }
        public string Nominativo { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string NominativoDelegato { get; set; }
        public string CodFiscalePIVADelegato { get; set; }
        public string IdTributo { get; set; }
        public string DescrTributo { get; set; }
        public string Stato { get; set; }
        public DateTime DataInserimento { get; set; }
        public DateTime DataCessazione { get; set; }
        public string Subentro { get; set; }
        public DateTime DataNascita { get; set; }
        public string ComuneNascita { get; set; }
        public string PVNascita { get; set; }
        public string Comune { get; set; }
        public string PV { get; set; }
        public string Indirizzo { get; set; }
        public string ComuneDelegato { get; set; }
        public string PVDelegato { get; set; }
        public string IndirizzoDelegato { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            ID = -1;
            IDDelegante = -1;
            IDDelegato = -1;
            IDIstanza = -1;
            IDStato = -1;
            IDOrigineCessazione = 0;
            Nominativo = string.Empty;
            CodFiscalePIVA = string.Empty;
            NominativoDelegato = string.Empty;
            CodFiscalePIVADelegato = string.Empty;
            IdTributo = "";
            DescrTributo = string.Empty;
            Stato = string.Empty;
            DataInserimento = DateTime.MaxValue;
            DataCessazione = DateTime.MaxValue;
            Subentro = "KO";
            DataNascita = DateTime.MaxValue;
            ComuneNascita = string.Empty;
            PVNascita = string.Empty;
            Comune = string.Empty;
            PV = string.Empty;
            Indirizzo = string.Empty;
            ComuneDelegato = string.Empty;
            PVDelegato = string.Empty;
            IndirizzoDelegato = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// Oggetto per la memorizzazione dei dati ricevuti dall'autenticazione SPID
    /// </summary>
    /// <revisionHistory><revision date="11/03/2020"><strong>SPID</strong>Le regole SPID vogliono che il pulsante di accesso sia subito visibile; bisogna quindi spostare la selezione dell'ente dopo l'autenticazione</revision></revisionHistory>
    public class SPIDAuthn
    {
        #region "Variables and constructor"
        public SPIDAuthn()
        {
            Reset();
        }
        #endregion
        #region "Public properties"
        [Key]
        public string email { get; set; }
        [Required]
        [StringLength(400)]
        public string spidCode { get; set; }
        [Required]
        [StringLength(16)]
        public string fiscalNumber { get; set; }
        [Required]
        [StringLength(11)]
        public string ivaCode { get; set; }
        #endregion
        #region DbObject methods
        public void Reset()
        {
            email = string.Empty;
            spidCode = string.Empty;
            fiscalNumber = string.Empty;
            ivaCode = string.Empty;
        }
        #endregion
    }
}