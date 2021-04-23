using log4net;
using OPENgovSPORTELLO.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace OPENgovSPORTELLO.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class PagamentoPendenza
    {
        public int ID { get; set; }
        public string IdEnte { get; set; }
        public string IdTributo { get; set; }
        public int IdContribuente { get; set; }
        public int IdIstanza { get; set; }
        public string IdPendenza { get; set; }
        public string TipoTributo { get; set; }
        public string IdMessaggio { get; set; }
        public DateTime DataCreazione { get; set; }
        public string Note { get; set; }
        public List<DovutoPendenza> ListDovuto { get; set; }
        public string CanalePagamento { get; set; }
        public string DescrCanalePagamento { get; set; }
        public string MezzoPagamento { get; set; }

        public PagamentoPendenza()
        {
            ID = default(int);
            IdContribuente = default(int);
            IdIstanza = default(int);
            IdEnte = string.Empty;
            IdTributo = string.Empty;
            IdPendenza = string.Empty;
            TipoTributo = string.Empty;
            IdMessaggio = string.Empty;
            Note = string.Empty;
            DataCreazione = DateTime.MaxValue;
            ListDovuto = new List<DovutoPendenza>();
            CanalePagamento = string.Empty;
            DescrCanalePagamento = string.Empty;
            MezzoPagamento = string.Empty;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DovutoPendenza
    {
        public string Anno { get; set; }
        public DateTime DataEmissione { get; set; }
        public string Causale { get; set; }
        public decimal Importo { get; set; }
        public List<DettaglioPendenza> ListDettaglio { get; set; }

        public DovutoPendenza()
        {
            Anno = string.Empty;
            Causale = string.Empty;
            DataEmissione = DateTime.MaxValue;
            Importo = default(decimal);
            ListDettaglio = new List<DettaglioPendenza>();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class DettaglioPendenza
    {
        public string CodAvviso { get; set; }
        public DateTime DataScadenza { get; set; }
        public DateTime DataInizioVal { get; set; }
        public DateTime DataFineVal { get; set; }
        public string Causale { get; set; }
        public decimal Importo { get; set; }
        public string Tipo { get; set; }
        public string Codice { get; set; }
        public string Descrizione { get; set; }

        public DettaglioPendenza()
        {
            CodAvviso = string.Empty;
            Causale = string.Empty;
            Tipo = string.Empty;
            Codice = string.Empty;
            Descrizione = string.Empty;
            DataScadenza = DateTime.MaxValue;
            DataInizioVal = DateTime.MaxValue;
            DataFineVal = DateTime.MaxValue;
            Importo = default(decimal);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Dovuto
    {
        public string Id { get; set; }
        public string Anno { get; set; }
        public string DescrTributo { get; set; }
        public DateTime Scadenza { get; set; }
        public decimal ImpDovuto { get; set; }
        public decimal ImpVersato { get; set; }
        public decimal ImpDebito { get; set; }

        public Dovuto()
        {
            Id = string.Empty;
            Anno = string.Empty;
            DescrTributo = string.Empty;
            Scadenza = DateTime.MaxValue;
            ImpDovuto = default(decimal);
            ImpVersato = default(decimal);
            ImpDebito = default(decimal);
        }
    }
}
namespace OPENgovSPORTELLO.Paga
{
    #region "Methods"
    /// <summary>
    /// 
    /// </summary>
    public class clsPagoPA
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(clsPagoPA));
        #region "Token"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="User"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="IdContribToWork"></param>
        /// <param name="Esito"></param>
        /// <returns></returns>
        public string GetTokenGWPag(string IdEnte,string User, string IdContribuente, string IdContribToWork, string Esito)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.UtcNow+"|"+IdEnte + "|" + User + "|" + IdContribuente + "|" + IdContribToWork + "|" + Esito));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="HScadenza"></param>
        /// <param name="Esito"></param>
        /// <returns></returns>
        public TokenValidation ValidateTokenGWPag(string token,int HScadenza,out string Esito)
        {
            var result = new TokenValidation();
            byte[] data = Convert.FromBase64String(token);
            string _data = System.Text.Encoding.UTF8.GetString(data);
            string[] ListData = _data.Split(char.Parse("|"));
            if (ListData.GetLength(0) != 6)
                result.Errors.Add(TokenValidationStatus.WrongGuid);

            DateTime when = DateTime.Parse(ListData[0]);
            if (when < DateTime.UtcNow.AddHours(-HScadenza))
            {
                result.Errors.Add(TokenValidationStatus.Expired);
            }
            MySession.Current.Ente = new BLL.EntiSistema(new EntiInLavorazione()).LoadEnte(ListData[1], string.Empty);
            if (MySession.Current.Ente == null)
            {
                result.Errors.Add(TokenValidationStatus.WrongPurpose);
            }
            Esito = ListData[5];
            List<UserRole> ListGen = new BLL.Settings().LoadUserRole(ListData[2], string.Empty, true, MySession.Current.Ente.IDEnte, ListData[2]);
            if (ListGen.Count > 0)
            {
                MySession.Current.UserLogged = ListGen[0];
                MySession.Current.UserLogged.IDContribLogged = int.Parse(ListData[3].Trim());
                MySession.Current.UserLogged.IDContribToWork = int.Parse(ListData[4].Trim());
            }
            else
                result.Errors.Add(TokenValidationStatus.WrongUser);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public class TokenValidation
        {
            public bool Validated { get { return Errors.Count == 0; } }
            public readonly List<TokenValidationStatus> Errors = new List<TokenValidationStatus>();
        }
        /// <summary>
        /// 
        /// </summary>
        public enum TokenValidationStatus
        {
            Expired,
            WrongUser,
            WrongPurpose,
            WrongGuid
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="myAnag"></param>
        /// <param name="IdTributo"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="ListIdToPay"></param>
        /// <param name="UrlBack"></param>
        /// <param name="UrlCancel"></param>
        /// <param name="myPayment"></param>
        /// <param name="UrlGW"></param>
        /// <param name="sError"></param>
        /// <returns></returns>
        public bool MakePayment(EntiInLavorazione myEnte,AnagInterface.DettaglioAnagrafica myAnag, string IdTributo, int IdContribuente, List<string> ListIdToPay, string UrlBack,string UrlCancel, out PagamentoPendenza myPayment, out string UrlGW,out string sError )
        {
            bool myRet = false;
            myPayment = new PagamentoPendenza();
            UrlGW = string.Empty;
                          sError = string.Empty;
           try
            {
                string myList = string.Empty;
                foreach (string myString in ListIdToPay)
                    myList += ((myList != string.Empty) ? "," : string.Empty) + myString;

                myPayment = new BLL.PagoPA(new PagamentoPendenza()).LoadPayment(myEnte.IDEnte, IdTributo, IdContribuente, myList);
                if (myPayment != null)
                {
                    if (new BLL.PagoPA(myPayment).SaveTmpPendenze())
                    {
                         srOTF.IdpAllineamentoPendenzeEnteOTFRequest myOTFRequest = new srOTF.IdpAllineamentoPendenzeEnteOTFRequest();
                        srOTF.IdpAllineamentoPendenzeEnteOTFResponse myOTFResponse = new srOTF.IdpAllineamentoPendenzeEnteOTFResponse();
                        string myOTFEsito = string.Empty;
                        string sErr = string.Empty;

                        myOTFRequest.IdpAllineamentoPendenzeEnteOTF = new clsOTF().GetPendenze(myEnte, myAnag, myPayment,UrlBack,UrlCancel);
                        
                        Log.Debug("Richiamo EndpointOTF Url->" + MySession.Current.Ente.DatiPagoPA.EndpointOTF);
                        new BLL.RestService().MakeRequestOTF<srOTF.IdpAllineamentoPendenzeEnteOTFEsito>(MySession.Current.Ente.DatiPagoPA.EndpointOTF
                            ,myOTFRequest.IdpAllineamentoPendenzeEnteOTF
                            , result => myOTFEsito = result
                            , error => sErr = error.Message
                            , "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(MySession.Current.Ente.DatiPagoPA.EndpointUser+ ":" + MySession.Current.Ente.DatiPagoPA.EndpointPwd))
                            , "SOAPAction: IdpAllineamentoPendenzeEnteOTF"
                            );
                        if (sErr != string.Empty)
                        {
                            Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.MakePayment.MakeRequestOTF::errore::" + sErr);
                        }
                        else
                        {
                            if (myOTFEsito!=string.Empty)
                            {
                                try {
                                    if (myOTFEsito.ToUpper().IndexOf("ELABORATO CON ERRORI") > 0)
                                    {
                                        sError = myOTFEsito.Substring(myOTFEsito.IndexOf("<ns2:Descrizione>") + 17, myOTFEsito.IndexOf("</ns2:Descrizione>") - (myOTFEsito.IndexOf("<ns2:Descrizione>") + 17));
                                        new BLL.PagoPA(myPayment).ClearTmpPendenze();
                                    }
                                    else
                                    {
                                        try
                                        {
                                            UrlGW = myOTFEsito.Substring(myOTFEsito.ToUpper().IndexOf("<NS2:URLGW>") + 11, myOTFEsito.ToUpper().IndexOf("</NS2:URLGW>") - (myOTFEsito.ToUpper().IndexOf("<NS2:URLGW>") + 11));
                                            myRet = true;
                                        }
                                        catch (Exception exUrl)
                                        {
                                            Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.MakePayment.GetUrl.errore::", exUrl);
                                        }
                                    }
                                }
                                catch(Exception exXml)
                                {
                                    Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.MakePayment.LetturaXMLEsito::errore::", exXml);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.MakePayment::errore::", ex);
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="myUser"></param>
        /// <param name="myItem"></param>
        /// <returns></returns>
        public string VerificaStatoPendenza(EntiInLavorazione myEnte,UserRole myUser,PagamentoPendenza myItem)
        {
            string sRet = string.Empty;
            try
            {
                srVerificaStato.IdpVerificaStatoPagamentiRequest myVerificaRequest = new srVerificaStato.IdpVerificaStatoPagamentiRequest();
                myVerificaRequest.IdpVerificaStatoPagamenti = new clsVerificaOTF().GetVerificaPendenze(myEnte.DatiPagoPA, myItem);
                string myVerificaEsito = string.Empty;
                
                string sErr = string.Empty;

                Log.Debug("Richiamo EndpointVerificaStato Url->" + myEnte.DatiPagoPA.EndpointVerificaStato);
                new BLL.RestService().MakeRequestVerificaStato<srVerificaStato.IdpVerificaStatoPagamentiEsito>(myEnte.DatiPagoPA.EndpointVerificaStato
                    , myVerificaRequest.IdpVerificaStatoPagamenti.IdpVerificaStatoPagamento
                    , result => myVerificaEsito = result
                    , error => sErr = error.Message
                    , "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(myEnte.DatiPagoPA.EndpointUser + ":" + myEnte.DatiPagoPA.EndpointPwd))
                    , "SOAPAction: IdpVerificaStatoPagamenti"
                    );
                if (sErr != string.Empty)
                {
                    Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.VerificaStatoPendenza::errore::" + sErr);
                }
                else
                {
                    if (myVerificaEsito != string.Empty)
                    {
                        try
                        {
                            if (myVerificaEsito.ToUpper().IndexOf("POSIZIONE_PAGATA") > 0 || myVerificaEsito.ToUpper().IndexOf("POSIZIONE_PAGATA_SBF") > 0)
                            {
                                try
                                {
                                    myItem.DataCreazione = DateTime.Parse(myVerificaEsito.Substring(myVerificaEsito.IndexOf("<ns3:DataOraPagamento>") + 22, myVerificaEsito.IndexOf("</ns3:DataOraPagamento>") - (myVerificaEsito.IndexOf("<ns3:DataOraPagamento>") + 22)));
                                    string CanalePag = string.Empty;
                                    CanalePag = myVerificaEsito.Substring(myVerificaEsito.IndexOf("<ns3:CanalePagamento") + 20, myVerificaEsito.IndexOf("</ns3:CanalePagamento>") - (myVerificaEsito.IndexOf("<ns3:CanalePagamento") + 20));
                                    myItem.CanalePagamento = CanalePag.Substring(CanalePag.IndexOf("Tipo=") + 6, CanalePag.IndexOf(">") - (CanalePag.IndexOf("Tipo=") + 7));
                                    myItem.DescrCanalePagamento = CanalePag.Substring(CanalePag.IndexOf("<ns3:Descrizione>") + 17, CanalePag.IndexOf("</ns3:Descrizione>") - (CanalePag.IndexOf("<ns3:Descrizione>") + 17));
                                    myItem.MezzoPagamento = myVerificaEsito.Substring(myVerificaEsito.IndexOf("<ns3:MezzoPagamento Tipo=") + 26, myVerificaEsito.IndexOf("/>", myVerificaEsito.IndexOf("<ns3:MezzoPagamento Tipo=")) - (myVerificaEsito.IndexOf(" <ns3:MezzoPagamento Tipo=") + 28));
                                  sRet=  SetIstanzaPagamento(myUser, myItem);
                                }
                                catch (Exception exEsito)
                                {
                                    Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.VerificaStatoPendenza.LetturaDatiPag.errore::", exEsito);
                                    sRet = "$('#OnlyNumber_error').text('Errore in lettura Dati Pagamento!');$('#OnlyNumber_error').show();";
                                    return sRet;
                                }
                            }
                            else if (myVerificaEsito.ToUpper().IndexOf("POSIZIONE_CON_PAGAMENTO_IN_CORSO") > 0)
                            {
                                sRet = "$('#OnlyNumber_error').text('Pagamento in corso! Attendere che il pagamento sia terminato prima di procedere con una nuova istanza.');$('#OnlyNumber_error').show();";
                                sRet += "$('.BottoneShoppingCart').hide();";
                            }
                            else
                            {
                                sRet = "$('#OnlyNumber_error').text('Pagamento non eseguito!');$('#OnlyNumber_error').show();";
                            }
                            if (myVerificaEsito.ToUpper().IndexOf("POSIZIONE_CON_PAGAMENTO_IN_CORSO") <= 0)
                            {
                                new BLL.PagoPA(myItem).ClearTmpPendenze();
                            }
                        }
                        catch (Exception exXml)
                        {
                            Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.VerificaStatoPendenza.LetturaXMLEsito::errore::", exXml);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sRet = string.Empty;
                Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.VerificaStatoPendenza::errore::", ex);
            }
            return sRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myUser"></param>
        /// <param name="myPagamento"></param>
        /// <returns></returns>
        private string SetIstanzaPagamento(UserRole myUser,PagamentoPendenza myPagamento)
        {
            string sRet = string.Empty;
            try
            {
                Istanza myIstanza = new Istanza();

                myIstanza.DataPresentazione = DateTime.Now;
                myIstanza.DataInvioDichiarazione = DateTime.Now;
                myIstanza.DataInCarico = DateTime.MaxValue;
                myIstanza.DataRespinta = DateTime.MaxValue;
                myIstanza.DataValidata = DateTime.MaxValue;
                myIstanza.IDContribuente = myUser.IDContribToWork;
                myIstanza.IDDelegato = (myUser.IDContribToWork != myUser.IDContribLogged) ? myUser.IDContribLogged : -1;
                myIstanza.IDEnte = MySession.Current.Ente.IDEnte;
                myIstanza.IDIstanza = -1;
                myIstanza.IDStato = Istanza.STATO.Presentata;
                myIstanza.IDTributo = General.TRIBUTO.OSAP;
                myIstanza.Note = "";

                foreach (GenericCategory myTipo in new BLL.Settings().LoadTipoIstanze(General.TRIBUTO.OSAP, Istanza.TIPO.Pagamento, false))
                {
                    myIstanza.IDTipo = myTipo.ID;
                }
                if (new BLL.Istanze(myIstanza, myUser.ID).Save())
                {
                    myPagamento.IdIstanza = myIstanza.IDIstanza;
                    if (new BLL.PagoPA(myPagamento).Save())
                    {
                        sRet = "$('#OnlyNumber_error').text('Pagamento effettuato con successo!');$('#OnlyNumber_error').show();";
                    }
                    else
                    {
                        sRet = "$('#OnlyNumber_error').text('Errore in salvataggio pagamento');$('#OnlyNumber_error').show();";
                        new General().LogActionEvent(DateTime.Now, myUser.NameUser, MySession.Current.Scope, "PagoPA", "", "Paga", "pagamento non a buon fine", "", "", MySession.Current.Ente.IDEnte);
                    }
                }
                else
                {
                    sRet = "$('#OnlyNumber_error').text('Errore in salvataggio istanza!');$('#OnlyNumber_error').show();";
                    new General().LogActionEvent(DateTime.Now, myUser.NameUser, MySession.Current.Scope, "PagoPA", "", "Paga", "pagamento non a buon fine", "", "", MySession.Current.Ente.IDEnte);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FO_PayGen.SetIstanzaPagamento::errore::", ex);
                sRet = "$('#OnlyNumber_error').text('Errore in istanza!');$('#OnlyNumber_error').show();";
            }
            return sRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <returns></returns>
        public bool PaymentToVerticale(string IdEnte)
        {
            try
            {
                return new BLL.PagoPA(new PagamentoPendenza()).PaymentInToVerticale(IdEnte);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsPagoPA.PaymentToVerticale::errore::", ex);
                return false;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class clsOTF
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(clsOTF));
        public static string CodiceServizio = "IdpAllineamentoPendenze";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="myAnag"></param>
        /// <param name="myPagamento"></param>
        /// <param name="UrlBack"></param>
        /// <param name="UrlCancel"></param>
        /// <returns></returns>
        public srOTF.IdpAllineamentoPendenzeEnteOTF GetPendenze(EntiInLavorazione myEnte,AnagInterface.DettaglioAnagrafica myAnag, PagamentoPendenza myPagamento, string UrlBack, string UrlCancel)
        {
            srOTF.IdpAllineamentoPendenzeEnteOTF myRet = new srOTF.IdpAllineamentoPendenzeEnteOTF();
            srOTF.IdpAllineamentoPendenzeOTF myItem = new srOTF.IdpAllineamentoPendenzeOTF();
            try
            {
                srOTF.IdpOTF myOTF = new srOTF.IdpOTF();
                srOTF.IdpHeader myHeader = new srOTF.IdpHeader();
                List<srOTF.Pendenza> myBody = new List<srOTF.Pendenza>();

                myItem.Versione = srOTF.Versione.Item010302;
                myItem.IdpHeader = GetHeader(myEnte.DatiPagoPA, myPagamento);
                myItem.IdpOTF = GetOTF(UrlBack, UrlCancel);
                myItem.IdpBody = GetBody(myEnte, myAnag, myPagamento).Cast<srOTF.Pendenza>().ToArray();
                myRet.IdpAllineamentoPendenzeOTF = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetPendenze::errore::", ex);
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srOTF.IdpHeader GetHeader(BasePagoPA myDataPagoPA, PagamentoPendenza Pagamento)
        {
            srOTF.IdpHeader myItem = new srOTF.IdpHeader();
            try
            {
                myItem.TRT = GetHeaderTRT(myDataPagoPA, Pagamento);
                myItem.E2E = GetHeaderE2E(myDataPagoPA, Pagamento);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetHeader::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UrlBack"></param>
        /// <param name="UrlCancel"></param>
        /// <returns></returns>
        private srOTF.IdpOTF GetOTF(string UrlBack, string UrlCancel)
        {
            srOTF.IdpOTF myItem = new srOTF.IdpOTF();
            try
            {
                myItem.URL_BACK = UrlBack;
                myItem.URL_CANCEL = UrlCancel;
                myItem.OFFLINE_PAYMENT_METHODSSpecified = true;
                myItem.OFFLINE_PAYMENT_METHODS = false;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetOTF::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="myAnag"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private List<srOTF.Pendenza> GetBody(EntiInLavorazione myEnte,AnagInterface.DettaglioAnagrafica myAnag, PagamentoPendenza Pagamento)
        {
            List<srOTF.Pendenza> ListItem = new List<srOTF.Pendenza>();
            try
            {
                foreach (DovutoPendenza myDovuto in Pagamento.ListDovuto)
                {
                    srOTF.Pendenza myItem = new srOTF.Pendenza();
                    myItem.TipoOperazione = srOTF.TipoOperazione.Insert;
                    myItem.TipoPendenza = Pagamento.TipoTributo;
                    myItem.IdPendenza = Pagamento.IdPendenza;//Identifica la pendenza in modo univoco nell'ambito dell'Ente, del SIL  e del TipoPendenza che invia la posizione debitoria. I valori assegnati a IdPendenza sono proprietari dell’Ente, generati in modo autonomo dall’Ente e non vengono interpretati dal IRIS
                    myItem.Mittente = GetMittente(myEnte);
                    myItem.Destinatari = GetDestinatari(myAnag).Cast<srOTF.Destinatario>().ToArray();
                    myItem.CartellaDiPagamentoSpecified = true;
                    myItem.CartellaDiPagamento = true;//Cartella di pagamento  (flag): se presente con valore “vero” indica che le condizioni di pagamento associate alla pendenza devono essere pagate congiuntamente
                    myItem.Note = Pagamento.Note;
                    myItem.Insert = GetPendenzaInsertReplace(myEnte.DatiPagoPA, myDovuto, Pagamento.DataCreazione);

                    ListItem.Add(myItem);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetBody::errore::", ex);
            }
            return ListItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srOTF.HeaderTRT GetHeaderTRT(BasePagoPA myDataPagoPA, PagamentoPendenza Pagamento)
        {
            srOTF.HeaderTRT myItem = new srOTF.HeaderTRT();
            try
            {
                myItem.ServiceName = srOTF.ServiceName.IdpAllineamentoPendenze;
                myItem.MsgId = Pagamento.IdMessaggio;//Identificativo del Messaggio; non sono posti vincoli di nomenclatura; il suo valore insieme all'identificativo del sistema adottato dall' Ente specificato in 1.1.1.4.2    SenderSys identificano univocamente un messaggio presso   IRIS 
                myItem.XMLCrtDt = Pagamento.DataCreazione;
                myItem.Sender = GetTRTSender(myDataPagoPA);
                myItem.Receiver = GetTRTReceiver();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetHeaderTRT::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srOTF.HeaderE2E GetHeaderE2E(BasePagoPA myDataPagoPA, PagamentoPendenza Pagamento)
        {
            srOTF.HeaderE2E myItem = new srOTF.HeaderE2E();
            try
            {
                myItem.E2ESrvcNm = CodiceServizio;//fisso
                myItem.E2EMsgId = Pagamento.IdMessaggio;//Identificativo del Messaggio presente in HeaderTRT
                myItem.XMLCrtDt = Pagamento.DataCreazione;//stessa presente in HeaderTRT
                myItem.Sender = GetE2ESender(myDataPagoPA);
                myItem.Receiver = GetE2EReceiver();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetHeaderE2E::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <returns></returns>
        private srOTF.TRTSender GetTRTSender(BasePagoPA myDataPagoPA)
        {
            srOTF.TRTSender myItem = new srOTF.TRTSender();
            try
            {//Identificativo che individua univocamente un Ente mittente all’interno della infrastruttura CART;si utilizza il nome del soggetto SpCoop definito sull’infrastruttura CART assegnato ad un Ente a fronte dell’accordo di servizio stipulato per usufruire del servizio IRIS tramite la RFC corrente
                myItem.SenderId = myDataPagoPA.CARTId;
                myItem.SenderSys = myDataPagoPA.CARTSys;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetTRTSender::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private srOTF.TRTReceiver GetTRTReceiver()
        {
            srOTF.TRTReceiver myItem = new srOTF.TRTReceiver();
            try
            {
                myItem.ReceiverId = MySettings.GetConfig("CART_IRIS_Id");
                myItem.ReceiverSys = MySettings.GetConfig("CART_IRIS_Sys");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetTRTReceiver::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <returns></returns>
        private srOTF.E2ESender GetE2ESender(BasePagoPA myDataPagoPA)
        {
            srOTF.E2ESender myItem = new srOTF.E2ESender();
            try
            {
                myItem.E2ESndrId = myDataPagoPA.CARTId;
                myItem.E2ESndrSys = myDataPagoPA.CARTSys;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetE2ESender::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private srOTF.E2EReceiver GetE2EReceiver()
        {
            srOTF.E2EReceiver myItem = new srOTF.E2EReceiver();
            try
            {
                myItem.E2ERcvrId = MySettings.GetConfig("CART_IRIS_Id");
                myItem.E2ERcvrSys = MySettings.GetConfig("CART_IRIS_Sys");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetE2EReceiver::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <returns></returns>
        private srOTF.Mittente GetMittente(EntiInLavorazione myEnte)
        {
            srOTF.Mittente myItem = new srOTF.Mittente();
            try
            {//Identificativo ufficio mittente: testo libero. IRIS non verifica il contenuto del campo, che è gestito dall’Ente che trasmette la pendenza.
                myItem.Descrizione = MySettings.GetConfig("PagoPAUfficioMittente") + myEnte.Descrizione;
                myItem.Id = MySettings.GetConfig("PagoPAUfficioMittente") + myEnte.IDEnte;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetMittente::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myAnag"></param>
        /// <returns></returns>
        private List<srOTF.Destinatario> GetDestinatari(AnagInterface.DettaglioAnagrafica myAnag)
        {
            List<srOTF.Destinatario> ListItem = new List<srOTF.Destinatario>();
            try
            {//Soggetto a carico del quale sussiste la posizione debitoria;  descrive il cittadino tramite codice fiscale
                srOTF.Destinatario myItem = new srOTF.Destinatario();
                myItem.Descrizione = (myAnag.Cognome+" "+ myAnag.Nome).Trim();
                myItem.Id = (myAnag.PartitaIva==string.Empty? myAnag.CodiceFiscale: myAnag.PartitaIva).Trim();
                myItem.Tipo = srOTF.TipoDestinatario.Cittadino;
                ListItem.Add(myItem);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetDestinatari::errore::", ex);
            }
            return ListItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Dovuto"></param>
        /// <param name="DataCreazione"></param>
        /// <returns></returns>
        private srOTF.PendenzaInsertReplace GetPendenzaInsertReplace(BasePagoPA myDataPagoPA, DovutoPendenza Dovuto, DateTime DataCreazione)
        {
            srOTF.PendenzaInsertReplace myItem = new srOTF.PendenzaInsertReplace();
            try
            {
                myItem.DescrizioneCausale = Dovuto.Causale;//deve essere valorizzata obbligatoriamente con le chiavi semantiche del tributo secondo le regole definite per lo specifico tributo in AppendiceA.AnalisiRFC127
                myItem.Riscossore = GetRiscossore(myDataPagoPA);
                myItem.DataCreazione = DataCreazione;
                myItem.DataEmissione = Dovuto.DataEmissione;
                myItem.DataPrescrizione = DateTime.Now.AddMonths(1);
                myItem.AnnoRiferimento = Dovuto.Anno;
                myItem.DataModificaEnteSpecified = true;
                myItem.DataModificaEnte = DataCreazione;
                myItem.Stato = srOTF.StatoPendenza.Aperta;
                myItem.ImportoTotale = Dovuto.Importo;
                myItem.Divisa = srOTF.Divisa.EUR;
                myItem.InfoPagamento = GetInfoPagamento(myDataPagoPA, Dovuto).Cast<srOTF.PendenzaInsertReplaceInfoPagamento>().ToArray();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetPendenzaInsertReplace::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <returns></returns>
        private srOTF.Riscossore GetRiscossore(BasePagoPA myDataPagoPA)
        {
            srOTF.Riscossore myItem = new srOTF.Riscossore();
            try
            {//Identificativo riscossore: i valori assunti da tale elemento sono “proprietari” dell’Ente ed identificano univocamente un riscossore presso l’Ente
                myItem.Id = myDataPagoPA.IdRiscossore;
                myItem.Riferimento = myDataPagoPA.DescrRiscossore;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetRiscossore::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Dovuto"></param>
        /// <returns></returns>
        private List<srOTF.PendenzaInsertReplaceInfoPagamento> GetInfoPagamento(BasePagoPA myDataPagoPA, DovutoPendenza Dovuto)
        {
            List<srOTF.PendenzaInsertReplaceInfoPagamento> myList = new List<srOTF.PendenzaInsertReplaceInfoPagamento>();
            srOTF.PendenzaInsertReplaceInfoPagamento myItem = new srOTF.PendenzaInsertReplaceInfoPagamento();
            try
            {
                myItem.TipoPagamento = srOTF.TipoPagamento.PagamentoUnico;
                foreach (DettaglioPendenza myPag in Dovuto.ListDettaglio)
                {
                    myItem.DettaglioPagamento = GetDettaglioPagamento(myDataPagoPA, myPag).Cast<srOTF.DettaglioPagamentoInsertReplace>().ToArray();
                    myList.Add(myItem);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetInfoPagamento::errore::", ex);
            }
            return myList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private List<srOTF.DettaglioPagamentoInsertReplace> GetDettaglioPagamento(BasePagoPA myDataPagoPA, DettaglioPendenza Pagamento)
        {
            List<srOTF.DettaglioPagamentoInsertReplace> myList = new List<srOTF.DettaglioPagamentoInsertReplace>();
            srOTF.DettaglioPagamentoInsertReplace myItem = new srOTF.DettaglioPagamentoInsertReplace();
            try
            {
                myItem.IdPagamento = Pagamento.CodAvviso;
                myItem.DataScadenza = Pagamento.DataScadenza;
                myItem.DataInizioValiditaSpecified = true;
                myItem.DataInizioValidita = Pagamento.DataInizioVal;
                myItem.DataFineValidita = Pagamento.DataFineVal;
                myItem.Stato = srOTF.StatoPagamento.NonPagato;
                myItem.Importo = Pagamento.Importo;
                myItem.DettaglioImporto = GetVoceImporto(Pagamento).Cast<srOTF.VoceImporto>().ToArray();
                myItem.CausalePagamento = Pagamento.Causale;
                myItem.AccreditoPagamento = GetCoordinateBancarie(myDataPagoPA);
                myList.Add(myItem);

            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetDettaglioPagamento::errore::", ex);
            }
            return myList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private List<srOTF.VoceImporto> GetVoceImporto(DettaglioPendenza Pagamento)
        {
            List<srOTF.VoceImporto> myList = new List<srOTF.VoceImporto>();
            srOTF.VoceImporto myItem = new srOTF.VoceImporto();
            try
            {
                myItem.Tipo = Pagamento.Tipo;
                myItem.Codice = Pagamento.Codice;
                myItem.Descrizione = Pagamento.Descrizione;
                myItem.Importo = Pagamento.Importo;
                myList.Add(myItem);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetVoceImporto::errore::", ex);
            }
            return myList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <returns></returns>
        private srOTF.CoordinateBancarie GetCoordinateBancarie(BasePagoPA myDataPagoPA)
        {
            srOTF.CoordinateBancarie myItem = new srOTF.CoordinateBancarie();
            try
            {
                myItem.CodiceIBAN = myDataPagoPA.IBAN;
                myItem.Beneficiario = myDataPagoPA.DescrIBAN;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsOTF.GetCoordinateBancarie::errore::", ex);
            }
            return myItem;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class clsVerificaOTF
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(clsVerificaOTF));
        public static string CodiceServizio = "IdpInformativaPagamento";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="myPagamento"></param>
        /// <returns></returns>
        public srVerificaStato.IdpVerificaStatoPagamenti GetVerificaPendenze(BasePagoPA myDataPagoPA, PagamentoPendenza myPagamento)
        {
            srVerificaStato.IdpVerificaStatoPagamenti myRet = new srVerificaStato.IdpVerificaStatoPagamenti();
            srVerificaStato.IdpVerificaStatoPagamento myItem = new srVerificaStato.IdpVerificaStatoPagamento();
            try
            {
                srVerificaStato.IdpHeader myHeader = new srVerificaStato.IdpHeader();

                myItem.Versione = srVerificaStato.Versione.Item010302;
                myItem.IdpHeader = GetHeader(myDataPagoPA, myPagamento);
                myItem.IdpBody = GetBody(myPagamento);
                myRet.IdpVerificaStatoPagamento = myItem;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetVerificaPendenze::errore::", ex);
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srVerificaStato.IdpHeader GetHeader(BasePagoPA myDataPagoPA, PagamentoPendenza Pagamento)
        {
            srVerificaStato.IdpHeader myItem = new srVerificaStato.IdpHeader();
            try
            {
                myItem.TRT = GetHeaderTRT(myDataPagoPA, Pagamento);
                myItem.E2E = GetHeaderE2E(myDataPagoPA, Pagamento);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetHeader::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srVerificaStato.IdpVerificaStatoPagamentoIdpBody GetBody(PagamentoPendenza Pagamento)
        {
            srVerificaStato.IdpVerificaStatoPagamentoIdpBody myItem = new srVerificaStato.IdpVerificaStatoPagamentoIdpBody();
            List<srVerificaStato.idPagamento> ListPag = new List<srVerificaStato.idPagamento>();
            try
            {
                foreach(DovutoPendenza myDovuto in Pagamento.ListDovuto)
                {
                    foreach (DettaglioPendenza myDet in myDovuto.ListDettaglio)
                    {
                        srVerificaStato.idPagamento myPag = new srVerificaStato.idPagamento();
                        myPag.TipoPendenza = Pagamento.TipoTributo;
                        myPag.Value = myDet.CodAvviso;
                        ListPag.Add(myPag);
                    }
                }
                myItem.IdPagamento = ListPag.Cast<srVerificaStato.idPagamento>().ToArray();
                myItem.richiestaInformazioniPagamento = true;//deve essere valorizzato a “true” dall'Ente che ha richiesto a IRIS un pagamento OTF
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetBody::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srVerificaStato.HeaderTRT GetHeaderTRT(BasePagoPA myDataPagoPA, PagamentoPendenza Pagamento)
        {
            srVerificaStato.HeaderTRT myItem = new srVerificaStato.HeaderTRT();
            try
            {
                myItem.ServiceName = srVerificaStato.ServiceName.IdpInformativaPagamento;
                myItem.MsgId = Pagamento.IdMessaggio;//Identificativo del Messaggio; non sono posti vincoli di nomenclatura; il suo valore insieme all'identificativo del sistema adottato dall' Ente specificato in 1.1.1.4.2    SenderSys identificano univocamente un messaggio presso   IRIS 
                myItem.XMLCrtDt = Pagamento.DataCreazione;
                myItem.Sender = GetTRTSender(myDataPagoPA);
                myItem.Receiver = GetTRTReceiver();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetHeaderTRT::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <param name="Pagamento"></param>
        /// <returns></returns>
        private srVerificaStato.HeaderE2E GetHeaderE2E(BasePagoPA myDataPagoPA, PagamentoPendenza Pagamento)
        {
            srVerificaStato.HeaderE2E myItem = new srVerificaStato.HeaderE2E();
            try
            {
                myItem.E2ESrvcNm = CodiceServizio;//fisso
                myItem.E2EMsgId = Pagamento.IdMessaggio;//Identificativo del Messaggio presente in HeaderTRT
                myItem.XMLCrtDt = Pagamento.DataCreazione;//stessa presente in HeaderTRT
                myItem.Sender = GetE2ESender(myDataPagoPA);
                myItem.Receiver = GetE2EReceiver();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetHeaderE2E::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <returns></returns>
        private srVerificaStato.TRTSender GetTRTSender(BasePagoPA myDataPagoPA)
        {
            srVerificaStato.TRTSender myItem = new srVerificaStato.TRTSender();
            try
            {//Identificativo che individua univocamente un Ente mittente all’interno della infrastruttura CART;si utilizza il nome del soggetto SpCoop definito sull’infrastruttura CART assegnato ad un Ente a fronte dell’accordo di servizio stipulato per usufruire del servizio IRIS tramite la RFC corrente
                myItem.SenderId = myDataPagoPA.CARTId;
                myItem.SenderSys = myDataPagoPA.CARTSys;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetTRTSender::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private srVerificaStato.TRTReceiver GetTRTReceiver()
        {
            srVerificaStato.TRTReceiver myItem = new srVerificaStato.TRTReceiver();
            try
            {
                myItem.ReceiverId = MySettings.GetConfig("CART_IRIS_Id");
                myItem.ReceiverSys = MySettings.GetConfig("CART_IRIS_Sys");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetTRTReceiver::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDataPagoPA"></param>
        /// <returns></returns>
        private srVerificaStato.E2ESender GetE2ESender(BasePagoPA myDataPagoPA)
        {
            srVerificaStato.E2ESender myItem = new srVerificaStato.E2ESender();
            try
            {
                myItem.E2ESndrId = myDataPagoPA.CARTId;
                myItem.E2ESndrSys = myDataPagoPA.CARTSys;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetE2ESender::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private srVerificaStato.E2EReceiver GetE2EReceiver()
        {
            srVerificaStato.E2EReceiver myItem = new srVerificaStato.E2EReceiver();
            try
            {
                myItem.E2ERcvrId = MySettings.GetConfig("CART_IRIS_Id");
                myItem.E2ERcvrSys = MySettings.GetConfig("CART_IRIS_Sys");
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Paga.clsVerificaOTF.GetE2EReceiver::errore::", ex);
            }
            return myItem;
        }
    }
    #endregion
}
namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class PagoPA
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PagoPA));

        private PagamentoPendenza InnerObj { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myItem"></param>
        public PagoPA(PagamentoPendenza myItem)
        {
            InnerObj = myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="ListMyData"></param>
        /// <returns></returns>
        public bool LoadDovuto(string IdEnte, int IdContribuente, out List<Dovuto> ListMyData)
        {
            ListMyData = new List<Dovuto>();
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetPendenzaDaPagare", "IDENTE"
                            , "IDCONTRIBUENTE"
                        );
                    ListMyData = ctx.ContextDB.Database.SqlQuery<Dovuto>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IdContribuente)
                        ).ToList<Dovuto>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PagoPA.LoadDovuto::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IdTributo"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="ListToPay"></param>
        /// <returns></returns>
        public PagamentoPendenza LoadPayment(string IdEnte, string IdTributo, int IdContribuente, string ListToPay)
        {
            try
            {
                PagamentoPendenza ListMyData = new PagamentoPendenza();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetPendenzaGen", "IDENTE", "IDTRIBUTO");
                    ListMyData = ctx.ContextDB.Database.SqlQuery<PagamentoPendenza>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("IDTRIBUTO", IdTributo)
                        ).First<PagamentoPendenza>();
                    ListMyData.IdEnte = IdEnte;
                    ListMyData.IdTributo = IdTributo;
                    ListMyData.IdContribuente = IdContribuente;
                    sSQL = ctx.GetSQL("prc_GetPendenzaDovuto", "IDENTE"
                            , "IDCONTRIBUENTE"
                            , "LISTAVVISI"
                        );
                    ListMyData.ListDovuto = ctx.ContextDB.Database.SqlQuery<DovutoPendenza>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IdContribuente)
                            , ctx.GetParam("LISTAVVISI", ListToPay)
                        ).ToList<DovutoPendenza>();
                    foreach (DovutoPendenza myDovuto in ListMyData.ListDovuto)
                    {
                        sSQL = ctx.GetSQL("prc_GetPendenzaDettaglio", "IDENTE"
                                , "IDCONTRIBUENTE"
                                , "LISTAVVISI"
                            );
                        myDovuto.ListDettaglio = ctx.ContextDB.Database.SqlQuery<DettaglioPendenza>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                                , ctx.GetParam("IDCONTRIBUENTE", IdContribuente)
                                , ctx.GetParam("LISTAVVISI", ListToPay)
                            ).ToList<DettaglioPendenza>();
                    }
                    ctx.Dispose();
                }
                return ListMyData;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PagoPA.LoadPayment::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    foreach (DovutoPendenza myDovuto in InnerObj.ListDovuto)
                    {
                        foreach (DettaglioPendenza myDettaglio in myDovuto.ListDettaglio)
                        {
                            InnerObj.ID = default(int);
                            string sSQL = ctx.GetSQL("prc_TBLSPC_PAGAMENTI_IU", "ID", "IDENTE", "IDTRIBUTO", "IDCONTRIBUENTE", "IDISTANZA", "IDAVVISO", "DATA_AVVISO", "DATA_PAGAMENTO", "IMPORTO", "IDPENDENZA", "CANALEPAGAMENTO", "DESCRCANALEPAGAMENTO", "MEZZOPAGAMENTO");
                            InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("ID", InnerObj.ID)
                                    , ctx.GetParam("IDENTE", InnerObj.IdEnte)
                                    , ctx.GetParam("IDTRIBUTO", InnerObj.IdTributo)
                                    , ctx.GetParam("IDCONTRIBUENTE", InnerObj.IdContribuente)
                                    , ctx.GetParam("IDISTANZA", InnerObj.IdIstanza)
                                    , ctx.GetParam("IDAVVISO", myDettaglio.CodAvviso)
                                    , ctx.GetParam("DATA_AVVISO", myDettaglio.DataInizioVal)
                                    , ctx.GetParam("DATA_PAGAMENTO", InnerObj.DataCreazione)
                                    , ctx.GetParam("IMPORTO", myDettaglio.Importo)
                                    , ctx.GetParam("IDPENDENZA", InnerObj.IdPendenza)
                                    , ctx.GetParam("CANALEPAGAMENTO", InnerObj.CanalePagamento)
                                    , ctx.GetParam("DESCRCANALEPAGAMENTO", InnerObj.DescrCanalePagamento)
                                    , ctx.GetParam("MEZZOPAGAMENTO", InnerObj.MezzoPagamento)
                                ).First<int>();
                        }
                    }
                    ctx.Dispose();
                    if (InnerObj.ID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.PagoPA.Save::errore in inserimento pagamento");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.PagoPA.Save::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <returns></returns>
        public bool PaymentInToVerticale(string IdEnte)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_VerticalePendenzaAdd", "IDENTE");
                    int nRet = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IdEnte)).FirstOrDefault<int>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PagoPA.LoadDovuto::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IdContribuente"></param>
        /// <returns></returns>
        public PagamentoPendenza LoadTmpPendenze(string IdEnte, int IdContribuente)
        {
            try
            {
                List<PagamentoPendenza> ListItem = new List<PagamentoPendenza>();
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetTmpPendenze", "IDENTE", "IDCONTRIBUENTE");
                    ListItem = ctx.ContextDB.Database.SqlQuery<PagamentoPendenza>(sSQL, ctx.GetParam("IDENTE", IdEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IdContribuente)
                        ).ToList<PagamentoPendenza>();
                    if (ListItem.Count > 0)
                    {
                        sSQL = ctx.GetSQL("prc_GetTmpPendenzeDettaglio", "IDPENDENZA");
                        List<DettaglioPendenza> ListDett = ctx.ContextDB.Database.SqlQuery<DettaglioPendenza>(sSQL, ctx.GetParam("IDPENDENZA", ListItem[0].IdPendenza)).ToList<DettaglioPendenza>();
                        List<DovutoPendenza> ListDov = new List<DovutoPendenza>();
                        ListDov.Add(new DovutoPendenza() { ListDettaglio = ListDett });
                        ListItem[0].ListDovuto = ListDov;

                        ctx.Dispose();
                        return ListItem[0];
                    }
                    else
                        return new PagamentoPendenza();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLL.PagoPA.LoadTmpPendenze::errore::", ex);
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveTmpPendenze()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLTMPPENDENZE_IU", "IDENTE", "IDTRIBUTO", "IDCONTRIBUENTE", "IDMESSAGGIO", "DATACREAZIONE", "TIPOTRIBUTO", "IDPENDENZA");
                    int myID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", InnerObj.IdEnte)
                           , ctx.GetParam("IDTRIBUTO", InnerObj.IdTributo)
                           , ctx.GetParam("IDCONTRIBUENTE", InnerObj.IdContribuente)
                           , ctx.GetParam("IDMESSAGGIO", InnerObj.IdMessaggio)
                           , ctx.GetParam("DATACREAZIONE", InnerObj.DataCreazione)
                           , ctx.GetParam("TIPOTRIBUTO", InnerObj.TipoTributo)
                           , ctx.GetParam("IDPENDENZA", InnerObj.IdPendenza)
                       ).First<int>();
                    if (myID <= 0)
                    {
                        Log.Debug("OPENgovSPORTELLO.Models.PagoPA.SaveTmpPendenze::errore in inserimento pagamento");
                        return false;
                    }
                    else
                    {
                        foreach(DovutoPendenza myDov in InnerObj.ListDovuto)
                        {
                            foreach(DettaglioPendenza myDet in myDov.ListDettaglio)
                            {
                                sSQL = ctx.GetSQL("prc_TBLTMPPENDENZEDETTAGLIO_IU", "IDPENDENZA", "IDAVVISO", "DATAEMISSIONE", "IMPORTO");
                                myID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDPENDENZA", InnerObj.IdPendenza)
                                        , ctx.GetParam("IDAVVISO", myDet.CodAvviso)
                                        , ctx.GetParam("DATAEMISSIONE", myDet.DataInizioVal)
                                        , ctx.GetParam("IMPORTO", myDet.Importo)
                                    ).First<int>();
                                if (myID <= 0)
                                {
                                    Log.Debug("OPENgovSPORTELLO.Models.PagoPA.SaveTmpPendenzeDettaglio::errore in inserimento pagamento");
                                    return false;
                                }
                            }
                        }
                    }
                     ctx.Dispose();
               }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.PagoPA.SaveTmpPendenze::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ClearTmpPendenze()
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLTMPPENDENZE_D", "IDPENDENZA");
                    InnerObj.ID = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDPENDENZA", InnerObj.IdPendenza)).First<int>();
                    ctx.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Models.PagoPA.ClearTmpPendenze::errore::", ex);
                return false;
            }
        }
    }
}