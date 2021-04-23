using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPENgovSPORTELLO.Models
{
    #region"Analisi Istanze"
    /// <summary>
    /// 
    /// </summary>
    public class Eventi
    {
        #region "Variables and constructor"
        public Eventi()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Ente { get; set; }
        public string TipoAccesso { get; set; }
        public int Numero { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Ente = string.Empty;
            TipoAccesso = string.Empty;
            Numero = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class EventiAnalitica
    {
        #region "Variables and constructor"
        public EventiAnalitica()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Ente { get; set; }
        public string Nominativo { get; set; }
        public string CodiceFiscale { get; set; }
        public string TipoAccesso { get; set; }
        public int Numero { get; set; }
        public int Cod_Contribuente { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Ente = string.Empty;
            Nominativo = string.Empty;
            CodiceFiscale = string.Empty;
            TipoAccesso = string.Empty;
            Numero = default(int);
            Cod_Contribuente = default(int);
        }
        #endregion

    }
    /// <summary>
    /// 
    /// </summary>
    public class EventiAnaliticaStampa
    {
        #region "Variables and constructor"
        public EventiAnaliticaStampa()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public string Nominativo { get; set; }
        public string CodiceFiscale { get; set; }
        public string TipoAccesso { get; set; }
        public DateTime Data { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            Nominativo = string.Empty;
            CodiceFiscale = string.Empty;
            TipoAccesso = string.Empty;
            Data = DateTime.MaxValue;
        }
        #endregion

    }
    /// <summary>
    /// 
    /// </summary>
    public class EventiRaffronto
    {
        #region "Variables and constructor"
        public EventiRaffronto()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public string Ente { get; set; }
        public string TipoAccesso { get; set; }
        public int Numero { get; set; }
        public decimal Valore { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Dal = DateTime.MaxValue;
            Al = DateTime.MaxValue;
            Ente = string.Empty;
            TipoAccesso = string.Empty;
            Numero = default(int);
            Valore = default(decimal);
        }
        #endregion
    }
    #endregion
    #region"Analisi Eventi"
    /// <summary>
    /// 
    /// </summary>
    public class Attivita
    {
        #region "Variables and constructor"
        public Attivita()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Ente { get; set; }
        public string Descrizione { get; set; }
        public int NIstanze { get; set; }
        public int NRegistrate { get; set; }
        public int NProtocollate { get; set; }
        public int NInCarico { get; set; }
        public int NRespinte { get; set; }
        public int NIntegrazioni { get; set; }
        public int NValidate { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Ente = string.Empty;
            Descrizione = string.Empty;
            NIstanze = default(int);
            NRegistrate = default(int);
            NProtocollate = default(int);
            NInCarico = default(int);
            NRespinte = default(int);
            NIntegrazioni = default(int);
            NValidate = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class AttivitaAnalitica
    {
        #region "Variables and constructor"
        public AttivitaAnalitica()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string ENTE { get; set; }
        public int NACCESSI { get; set; }
        public int NREGISTRAZIONI { get; set; }
        public int NANAGRAFICA { get; set; }
        public int NIMU { get; set; }
        public int NTASI { get; set; }
        public int NTARSU { get; set; }
        public int NOSAP { get; set; }
        public int NICP { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            ENTE = string.Empty;
            NACCESSI = default(int);
            NREGISTRAZIONI = default(int);
            NANAGRAFICA = default(int);
            NIMU = default(int);
            NTASI = default(int);
            NTARSU = default(int);
            NOSAP = default(int);
            NICP = default(int);
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class TempiMedi
    {
        #region "Variables and constructor"
        public TempiMedi()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Ente { get; set; }
        public string Descrizione { get; set; }
        public decimal GG { get; set; }
        public decimal ToProtocollo { get; set; }
        public decimal ToCarico { get; set; }
        public decimal ToRespinte { get; set; }
        public decimal ToValidate { get; set; }
        public string CodificaInterna { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Ente = string.Empty;
            Descrizione = string.Empty;
            GG = default(decimal);
            ToProtocollo = default(decimal);
            ToCarico = default(decimal);
            ToRespinte = default(decimal);
            ToValidate = default(decimal);
            CodificaInterna = string.Empty;
        }
        #endregion
    }
    #endregion
    #region "TempiPagamento"
    /// <summary>
    /// 
    /// </summary>
    public class TempiPagamento
    {
        #region "Variables and constructor"
        public TempiPagamento()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public DateTime Scadenza { get; set; }
        public string Periodo { get; set; }
        public int Contribuenti { get; set; }
        public decimal Importo { get; set; }
        public decimal PercContribuenti { get; set; }
        public decimal PercPagato { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = Contribuenti = default(int);
            Scadenza = DateTime.MaxValue;
            Periodo = string.Empty;
            Importo = PercContribuenti = PercPagato = default(decimal);
        }
        #endregion
    }
    #endregion
    #region "DovutoVSVersato"
    /// <summary>
    /// 
    /// </summary>
    public class DovutoVSVersato
    {
        #region "Variables and constructor"
        public DovutoVSVersato()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Ente { get; set; }
        public int Anno { get; set; }
        public string Nominativo { get; set; }
        public string CodFiscalePIVA { get; set; }
        public decimal DovutoAcc { get; set; }
        public decimal DovutoSal { get; set; }
        public decimal Dovuto { get; set; }
        public decimal VersatoAcc { get; set; }
        public decimal VersatoSal { get; set; }
        public decimal Versato { get; set; }
        public decimal Differenza { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = Anno = default(int);
            Ente = Nominativo = CodFiscalePIVA = string.Empty;
            DovutoAcc = DovutoSal = Dovuto = VersatoAcc = VersatoSal = Versato = Differenza = 0;
        }
        #endregion
    }
    #endregion
    #region "ComunicazioniBOvsFO"
    /// <summary>
    /// 
    /// </summary>
    public class ComunicazioniBOvsFO
    {
        #region "Variables and constructor"
        public ComunicazioniBOvsFO()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string Ente { get; set; }
        public string Nominativo { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string Operatore { get; set; }
        public string DescrIstanza { get; set; }
        public string Stato { get; set; }
        public string Provenienza { get; set; }
        public DateTime Data { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            Ente = Nominativo = CodFiscalePIVA = Operatore = DescrIstanza = Stato = Provenienza = string.Empty;
            Data = DateTime.MaxValue;
        }
        #endregion
    }
    #endregion
    #region "Password via mail+via posta"
    /// <summary>
    /// 
    /// </summary>
    public class GestPWD
    {
        #region "Variables and constructor"
        public GestPWD()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string IDUser { get; set; }
        public string Password { get; set; }
        public string IDEnte { get; set; }
        public string Nominativo { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string Indirizzo { get; set; }
        public string EMail { get; set; }
        public string DescrEnte { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            IDUser = Password = IDEnte = Nominativo = CodFiscalePIVA = Indirizzo = EMail = DescrEnte = string.Empty;
        }
        #endregion
    }
    /// <summary>
    /// 
    /// </summary>
    public class GestPWDStampa
    {
        #region "Variables and constructor"
        public GestPWDStampa()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        public string Nominativo { get; set; }
        public string CodFiscalePIVA { get; set; }
        public string Indirizzo { get; set; }
        public string EMail { get; set; }
        public string Password { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            Nominativo = CodFiscalePIVA = Indirizzo = EMail = Password = string.Empty;
        }
        #endregion
    }
    #endregion
    #region "Cruscotto"
    /// <summary>
    /// 
    /// </summary>
    public class ReportDich8852
    {
        public string Ubicazione { get; set; }
        public string Rifcat { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public string CodCategoria { get; set; }
        public decimal RenditaValore { get; set; }
        public decimal PercPossesso { get; set; }
        public string DescrUtilizzo { get; set; }
        public string TipoDichiarante { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportDovuto8852
    {
        public string Anno { get; set; }
        public string Tributo { get; set; }
        public decimal Importo { get; set; }
        public decimal NFab { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportPagato8852
    {
        public string Anno { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public decimal Totale { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportDich0434
    {
        public string Ubicazione { get; set; }
        public string Rifcat { get; set; }
        public DateTime Dal { get; set; }
        public DateTime Al { get; set; }
        public string CodCategoria { get; set; }
        public string Nc { get; set; }
        public decimal Mq { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportDovuto0434
    {
        public string Anno { get; set; }
        public string NAvviso { get; set; }
        public decimal Fissa { get; set; }
        public decimal Variabile { get; set; }
        public decimal Provinciale { get; set; }
        public decimal Totale { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ReportPagato0434
    {
        public string Anno { get; set; }
        public string NAvviso { get; set; }
        public DateTime Data { get; set; }
        public decimal Totale { get; set; }
    }
    /// <summary>
    /// /
    /// </summary>
    public class ReportDich0453
    {
        public string Ubicazione { get; set; }
        public string TipoOccupazione { get; set; }
        public string CodCategoria { get; set; }
        public string Durata { get; set; }
        public string Consistenza { get; set; }
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    public class TempiMediDettaglio
    {
        #region "Variables and constructor"
        public TempiMediDettaglio()
        {
            Reset();
        }
        #endregion

        #region "Public properties"
        [Key]
        public int ID { get; set; }
        public string ENTE { get; set; }
        public string TRIBUTO { get; set; }
        public string OPERATORE { get; set; }
        public string NOMINATIVO { get; set; }
        public string CODICEFISCALE { get; set; }
        public string TIPOISTANZA { get; set; }
        public DateTime DATAREGISTRAZIONE { get; set; }
        public DateTime DATAFINALE { get; set; }
        #endregion

        #region DbObject methods
        public void Reset()
        {
            ID = default(int);
            ENTE = string.Empty;
            TRIBUTO = string.Empty;
            OPERATORE = string.Empty;
            NOMINATIVO = string.Empty;
            CODICEFISCALE = string.Empty;
            TIPOISTANZA = string.Empty;
            DATAREGISTRAZIONE = default(DateTime);
            DATAFINALE = default(DateTime);
        }
        #endregion
    }
}