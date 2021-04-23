using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;
using System.IO;

namespace OPENgovSPORTELLO.BLL
{
    /// <summary>
    /// Classe di gestione chiamate ai servizi
    ///-------------------------example of use-------------
    ///MakeXmlRequest<XmlDocument>("your_uri",result=>your_xmlDocument_variable =     result,error=>your_exception_Var = error);
    ///MakeJsonRequest<classwhateveryouwant>("your_uri",result=>your_classwhateveryouwant_variable=result,error=>your_exception_Var=error)
    ///-------------------------------------------------------------------------------
    ///public void MakeXmlRequest<T>(string uri, Action<XmlDocument> successAction, Action<Exception> errorAction)
    ///{
    ///    XmlDocument XMLResponse = new XmlDocument();
    ///    string wufooAPIKey = ""; //or username as well
    ///    string password = "";
    ///    StringBuilder url = new StringBuilder();
    ///    url.Append(uri);
    ///    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url.ToString());
    ///    string authInfo = wufooAPIKey + ":" + password;
    ///    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
    ///    request.Timeout = 30000;
    ///    request.KeepAlive = false;
    ///    request.Headers["Authorization"] = "Basic " + authInfo;
    ///    string documento = "";
    ///    MakeRequest(request, response => documento = response,
    ///            ///            (error) =>
    ///            ///            {
    ///            ///            ///    if (errorAction != null)
    ///            ///            ///    {
    ///            ///            ///        errorAction(error);
    ///            ///            ///    }
    ///            ///            }
    ///               );
    ///    XMLResponse.LoadXml(documento);
    ///    successAction(XMLResponse);
    ///}
    /// </summary>
    public class RestService
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RestService));
        public string UserImport = "t7I4aRn56O18m2-3";
        public string UserConvert = "540E62i_csR7o18V";
        public string UserAvgTimes = "gt1eI9a-6Vm54203";
        public string ReasonImport = "IMPORT";
        public string ReasonConvert = "CONVERT";
        public string ReasonAvgTimes = "AVGTIM";
        #region "Chiamate specifiche"
        /// <summary>
        /// Interfacciamento con SIT
        /// Chiamata per soggetto restituisce tutti i riferimenti catastali a lui intestati
        /// <see cref="API_trignosinello.doc"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Uri">Url del servizio</param>
        /// <param name="UriParam">Parametri della richiesta</param>
        /// <param name="successAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="repositoryAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="errorAction">Errore</param>
        /// <param name="HeaderParam">Parametri dell'header</param>
        public void MakeRequestBySoggetto<T>(string Uri, string UriParam, Action<List<RiepilogoUI>> successAction, Action<List<SPC_DichICI>> repositoryAction, Action<Exception> errorAction, params string[] HeaderParam)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Uri + UriParam);
            request.Timeout = 30000;
            request.KeepAlive = false;
            request.Method = System.Net.WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            foreach (string myItem in HeaderParam)
                request.Headers.Add(myItem);
            MakeRequest(
               request,
               (response) =>
               {
                   if (successAction != null)
                   {
                       try
                       {
                           MySession.Current.myCatastoVSSoggetto = Newtonsoft.Json.JsonConvert.DeserializeObject<CatastoBySoggetto>(response);
                           List<RiepilogoUI> toReturn = new List<RiepilogoUI>();
                           List<SPC_DichICI> RepositoryReturn = new List<SPC_DichICI>();
                           RepositoryReturn = CastResponseAsRepository(Newtonsoft.Json.JsonConvert.DeserializeObject<CatastoBySoggetto>(response), errorAction);
                           toReturn = CastResponseAsRiepilogo(RepositoryReturn, errorAction);
                           successAction(toReturn);
                           repositoryAction(RepositoryReturn);
                       }
                       catch (Exception ex)
                       {
                           MySession.Current.myCatastoVSSoggetto = new CatastoBySoggetto();
                           errorAction(ex);
                       }
                   }
               },
               (error) =>
               {
                   if (errorAction != null)
                   {
                       errorAction(error);
                   }
               }
            );
        }
        /// <summary>
        /// Interfacciamento con SIT
        /// Chiamata per riferimento catastale restituisce tutti i soggetti a lui intestati
        /// <see cref="API_trignosinello.doc"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Uri">Url del servizio</param>
        /// <param name="UriParam">Parametri della richiesta</param>
        /// <param name="successAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="errorAction">Errore</param>
        /// <param name="HeaderParam">Parametri dell'header</param>
        public void MakeRequestByRifCat<T>(string Uri, string UriParam, Action<string> successAction, Action<Exception> errorAction, params string[] HeaderParam)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Uri + UriParam);
            request.Timeout = 30000;
            request.KeepAlive = false;
            request.Method = System.Net.WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            foreach (string myItem in HeaderParam)
                request.Headers.Add(myItem);
            MakeRequest(
               request,
               (response) =>
               {
                   if (successAction != null)
                   {
                       successAction(response);
                   }
               },
               (error) =>
               {
                   if (errorAction != null)
                   {
                       errorAction(error);
                   }
               }
            );
        }
        /// <summary>
        /// Interfacciamento con Importazione banca dati esterna
        /// Chiamata per conversione flussi di carico o chiamata per soggetto per importazione banca dati puntuale
        /// <see cref="API ConvertImportAvgTimes.docx"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Uri">Url del servizio</param>
        /// <param name="UriParam">Parametri della richiesta</param>
        /// <param name="successAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="errorAction">Errore</param>
        /// <param name="HeaderParam">Parametri dell'header</param>
        public void MakeRequestForDatiVerticale<T>(string Uri, string UriParam, Action<ImportInterface.TributiModel> successAction, Action<Exception> errorAction, params string[] HeaderParam)
        {
            Log.Debug("MakeRequestForDatiVerticale.inizio");
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Uri + UriParam);
            //request.Timeout = 300000;
            request.KeepAlive = false;
            request.Method = System.Net.WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            foreach (string myItem in HeaderParam)
            request.Headers.Add(myItem); 
            Log.Debug("MakeRequestForDatiVerticale.call MakeRequest");
            MakeRequest(
               request,
               (response) =>
               {
                   if (successAction != null)
                   {
                       try
                       {
                           ImportInterface.TributiModel toReturn = new ImportInterface.TributiModel();
                           toReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<ImportInterface.TributiModel>(response);
                           successAction(toReturn);
                       }
                       catch (Exception ex)
                       {
                           errorAction(ex);
                       }
                   }
               },
               (error) =>
               {
                   if (errorAction != null)
                   {
                       errorAction(error);
                   }
               }
            );
        }
        /// <summary>
        /// Interfacciamento con Analisi Tempi Medi
        /// Chiamata per avere l'analisi dei tempi medi
        /// <see cref="API ConvertImportAvgTimes.docx"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Uri">Url del servizio</param>
        /// <param name="UriParam">Parametri della richiesta</param>
        /// <param name="successAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="errorAction">Errore</param>
        /// <param name="HeaderParam">Parametri dell'header</param>
        public void MakeRequestForTempiMedi<T>(string Uri, string UriParam, Action<ImportInterface.TempiMediModel> successAction, Action<Exception> errorAction, params string[] HeaderParam)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Uri + UriParam);
            request.Timeout = 30000;
            request.KeepAlive = false;
            request.Method = System.Net.WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            foreach (string myItem in HeaderParam)
                request.Headers.Add(myItem);
            MakeRequest(
               request,
               (response) =>
               {
                   if (successAction != null)
                   {
                       try
                       {
                           ImportInterface.TempiMediModel toReturn = new ImportInterface.TempiMediModel();
                           toReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<ImportInterface.TempiMediModel>(response);
                           successAction(toReturn);
                       }
                       catch (Exception ex)
                       {
                           errorAction(ex);
                       }
                   }
               },
               (error) =>
               {
                   if (errorAction != null)
                   {
                       errorAction(error);
                   }
               }
            );
        }

        /// <summary>
        /// Interfacciamento con Gateway PAGOPA della regione Toscana
        /// Chiamata per effettuare il pagamento
        /// <see cref="RT_IRIS_MAN_Gateway_PagamentiOTF.docx"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Uri">Url del servizio</param>
        /// <param name="UriParam">Parametri della richiesta</param>
        /// <param name="successAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="errorAction">Errore</param>
        /// <param name="HeaderParam">Parametri dell'header</param>
        public void MakeRequestOTF<T>(string Uri, srOTF.IdpAllineamentoPendenzeEnteOTF UriParam, Action<string> successAction, Action<Exception> errorAction, string AuthParam, params string[] HeaderParam)
        {
            string requestXml = "";
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Uri);
            request.Timeout = 3000000;
            request.KeepAlive = false;
            request.Method = System.Net.WebRequestMethods.Http.Post;
            if (AuthParam != string.Empty)
                request.Headers.Add("Authorization", AuthParam);
            foreach (string myItem in HeaderParam)
                request.Headers.Add(myItem);

            requestXml = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'>";
            requestXml += "<SOAP-ENV:Body>";
            requestXml += "<prox:IdpAllineamentoPendenzeEnteOTF xmlns:prox='http://www.cart.rete.toscana.it/servizi/iris_1_1/proxy'>";
            requestXml += "<ns2:IdpAllineamentoPendenzeOTF";
            requestXml += " Versione='01.03-02'";
            requestXml += " xmlns:prox='http://www.cart.rete.toscana.it/servizi/iris_1_1/proxy'";
            requestXml += " xmlns:ns2='http://www.cart.rete.toscana.it/servizi/iris_1_1/IdpAllineamentoPendenze'";
            requestXml += " xmlns='http://www.cart.rete.toscana.it/servizi/iris_1_1/IdpHeader'";
            requestXml += " xmlns:ns3='http://www.cart.rete.toscana.it/servizi/iris_1_1/IdpInclude'>";
            requestXml += "<IdpHeader>";
            requestXml += "<TRT>";
            requestXml += "<ServiceName>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.ServiceName + "</ServiceName>";
            requestXml += "<MsgId>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.MsgId + "</MsgId>";
            requestXml += "<XMLCrtDt>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.XMLCrtDt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</XMLCrtDt>";
            requestXml += "<Sender>";
            requestXml += "<SenderId>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.Sender.SenderId + "</SenderId>";//CPomarance
            requestXml += "<SenderSys>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.Sender.SenderSys + "</SenderSys>";//SIL_CPOMARANCE_COSAP
            requestXml += "</Sender>";
            requestXml += "<Receiver>";
            requestXml += "<ReceiverId>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.Receiver.ReceiverId + "</ReceiverId>";//RTIRIS
            requestXml += "<ReceiverSys>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.TRT.Receiver.ReceiverSys + "</ReceiverSys>";//SIL_IRIS_ITR
            requestXml += "</Receiver>";
            requestXml += "</TRT>";
            requestXml += "<E2E>";
            requestXml += "<E2ESrvcNm>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.E2ESrvcNm + "</E2ESrvcNm>";
            requestXml += "<E2EMsgId>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.E2EMsgId + "</E2EMsgId>";
            requestXml += "<XMLCrtDt>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.XMLCrtDt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</XMLCrtDt>";
            requestXml += "<Sender>";
            requestXml += "<E2ESndrId>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.Sender.E2ESndrId + "</E2ESndrId>";//CPomarance
            requestXml += "<E2ESndrSys>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.Sender.E2ESndrSys + "</E2ESndrSys>";//SIL_CPOMARANCE_COSAP
            requestXml += "</Sender>";
            requestXml += "<Receiver>";
            requestXml += "<E2ERcvrId>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.Receiver.E2ERcvrId + "</E2ERcvrId>";//RTIRIS
            requestXml += "<E2ERcvrSys>" + UriParam.IdpAllineamentoPendenzeOTF.IdpHeader.E2E.Receiver.E2ERcvrSys + "</E2ERcvrSys>";//SIL_IRIS_ITR
            requestXml += "</Receiver>";
            requestXml += "</E2E>";
            requestXml += "</IdpHeader>";
            requestXml += "<IdpOTF>";
            requestXml += "<URL_BACK>" + UriParam.IdpAllineamentoPendenzeOTF.IdpOTF.URL_BACK + "</URL_BACK>";
            requestXml += "<URL_CANCEL>" + UriParam.IdpAllineamentoPendenzeOTF.IdpOTF.URL_CANCEL + "</URL_CANCEL>";
            requestXml += "<OFFLINE_PAYMENT_METHODS>" + UriParam.IdpAllineamentoPendenzeOTF.IdpOTF.OFFLINE_PAYMENT_METHODS.ToString().ToLower() + "</OFFLINE_PAYMENT_METHODS>";
            requestXml += "</IdpOTF>";
            requestXml += "<ns2:IdpBody>";
            foreach (srOTF.Pendenza myBody in UriParam.IdpAllineamentoPendenzeOTF.IdpBody)
            {
                requestXml += "<ns2:Pendenza TipoOperazione='Insert' TipoPendenza='" + myBody.TipoPendenza + "'>";
                requestXml += "<ns2:IdPendenza>" + myBody.IdPendenza + "</ns2:IdPendenza>";
                requestXml += "<ns2:Mittente>";
                requestXml += "<ns2:Id>" + myBody.Mittente.Id + "</ns2:Id>";
                requestXml += "<ns2:Descrizione>" + myBody.Mittente.Descrizione + "</ns2:Descrizione>";
                requestXml += "</ns2:Mittente>";
                requestXml += "<ns2:Destinatari>";
                foreach (srOTF.Destinatario myDest in myBody.Destinatari)
                {
                    requestXml += "<ns2:Destinatario Tipo='" + myDest.Tipo + "'>";
                    requestXml += "<ns2:Id>" + myDest.Id + "</ns2:Id>";
                    requestXml += "<ns2:Descrizione>" + myDest.Descrizione + "</ns2:Descrizione>";
                    requestXml += "</ns2:Destinatario>";
                }
                requestXml += "</ns2:Destinatari>";
                requestXml += "<ns2:Insert>";
                requestXml += "<ns2:DescrizioneCausale>" + myBody.Insert.DescrizioneCausale + "</ns2:DescrizioneCausale>";
                requestXml += "<ns2:DataCreazione>" + myBody.Insert.DataCreazione.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</ns2:DataCreazione>";
                requestXml += "<ns2:DataEmissione>" + myBody.Insert.DataEmissione.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</ns2:DataEmissione>";
                requestXml += "<ns2:DataPrescrizione>" + myBody.Insert.DataPrescrizione.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</ns2:DataPrescrizione>";
                requestXml += "<ns2:AnnoRiferimento>" + myBody.Insert.AnnoRiferimento + "</ns2:AnnoRiferimento>";
                requestXml += "<ns2:DataModificaEnte>" + myBody.Insert.DataModificaEnte.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</ns2:DataModificaEnte>";
                requestXml += "<ns2:Stato>" + myBody.Insert.Stato + "</ns2:Stato>";
                requestXml += "<ns2:ImportoTotale>" + myBody.Insert.ImportoTotale.ToString().Replace(",", ".") + "</ns2:ImportoTotale>";
                requestXml += "<ns2:Divisa>EUR</ns2:Divisa>";
                foreach (srOTF.PendenzaInsertReplaceInfoPagamento myInfo in myBody.Insert.InfoPagamento)
                {
                    requestXml += "<ns2:InfoPagamento TipoPagamento='Pagamento Unico'>";
                    requestXml += "<ns2:DettaglioPagamento>";
                    foreach (srOTF.DettaglioPagamentoInsertReplace myDet in myInfo.DettaglioPagamento)
                    {
                        requestXml += "<ns2:IdPagamento>" + myDet.IdPagamento + "</ns2:IdPagamento>";
                        requestXml += "<ns2:DataScadenza>" + myDet.DataScadenza.ToString("yyyy'-'MM'-'dd") + "</ns2:DataScadenza>";
                        requestXml += "<ns2:DataInizioValidita>" + myDet.DataInizioValidita.ToString("yyyy'-'MM'-'dd") + "</ns2:DataInizioValidita>";
                        requestXml += "<ns2:DataFineValidita>" + myDet.DataFineValidita.ToString("yyyy'-'MM'-'dd") + "</ns2:DataFineValidita>";
                        requestXml += "<ns2:Stato>Non Pagato</ns2:Stato>";
                        requestXml += "<ns2:Importo>" + myDet.Importo.ToString().Replace(",", ".") + "</ns2:Importo>";
                        requestXml += "<ns2:CausalePagamento>" + myDet.CausalePagamento + "</ns2:CausalePagamento>";
                    }
                    requestXml += "</ns2:DettaglioPagamento>";
                    requestXml += "</ns2:InfoPagamento>";
                }
                requestXml += "</ns2:Insert>";
                requestXml += "</ns2:Pendenza>";
            }
            requestXml += "</ns2:IdpBody>";
            requestXml += "</ns2:IdpAllineamentoPendenzeOTF>";
            requestXml += "</prox:IdpAllineamentoPendenzeEnteOTF>";
            requestXml += "</SOAP-ENV:Body>";
            requestXml += "</SOAP-ENV:Envelope>";

            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            try
            {
                MakeRequest(
                   request,
                   (response) =>
                   {
                       if (successAction != null)
                       {
                           try
                           {
                               successAction(response);
                           }
                           catch (Exception ex)
                           {
                               errorAction(ex);
                           }
                       }
                   },
                   (error) =>
                   {
                       if (errorAction != null)
                       {
                           errorAction(error);
                       }
                   }
                );
            }
            catch (Exception ex)
            {
                string sErr = ex.Message;
            }
        }
        /// <summary>
        /// Interfacciamento con Gateway PAGOPA della regione Toscana
        /// Chiamata per controllare lo stato del pagamento
        /// <see cref="RT_IRIS_MAN_Gateway_PagamentiOTF.docx"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Uri">Url del servizio</param>
        /// <param name="UriParam">Parametri della richiesta</param>
        /// <param name="successAction">Lista di oggetti restituiti dalla chiamata</param>
        /// <param name="errorAction">Errore</param>
        /// <param name="HeaderParam">Parametri dell'header</param>
        public void MakeRequestVerificaStato<T>(string Uri, srVerificaStato.IdpVerificaStatoPagamento UriParam, Action<string> successAction, Action<Exception> errorAction, string AuthParam, params string[] HeaderParam)
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Uri);
            request.Timeout = 3000000;
            request.KeepAlive = false;
            request.Method = System.Net.WebRequestMethods.Http.Post;
            if (AuthParam != string.Empty)
                request.Headers.Add("Authorization", AuthParam);
            foreach (string myItem in HeaderParam)
                request.Headers.Add(myItem);

            string requestXml = "<SOAP-ENV:Envelope";
            requestXml += " xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'";
            requestXml += " xmlns:prox='http://www.cart.rete.toscana.it/servizi/iris_1_1/proxy'";
            requestXml += " xmlns:idp='http://www.cart.rete.toscana.it/servizi/iris_1_1/IdpInformativaPagamento'";
            requestXml += " xmlns:idp1='http://www.cart.rete.toscana.it/servizi/iris_1_1/IdpHeader'>";
            requestXml += "<SOAP-ENV:Body>";
            requestXml += "<prox:IdpVerificaStatoPagamenti>";
            requestXml += "<idp:IdpVerificaStatoPagamento Versione='01.03-02'>";
            requestXml += "<idp1:IdpHeader>";
            requestXml += "<idp1:TRT>";
            requestXml += "<idp1:ServiceName>" + UriParam.IdpHeader.TRT.ServiceName + "</idp1:ServiceName>";
            requestXml += "<idp1:MsgId>" + UriParam.IdpHeader.TRT.MsgId + "</idp1:MsgId>";
            requestXml += "<idp1:XMLCrtDt>" + UriParam.IdpHeader.TRT.XMLCrtDt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</idp1:XMLCrtDt>";
            requestXml += "<idp1:Sender>";
            requestXml += "<idp1:SenderId>" + UriParam.IdpHeader.TRT.Sender.SenderId + "</idp1:SenderId>";
            requestXml += "<idp1:SenderSys>" + UriParam.IdpHeader.TRT.Sender.SenderSys + "</idp1:SenderSys>";
            requestXml += "</idp1:Sender>";
            requestXml += "<idp1:Receiver>";
            requestXml += "<idp1:ReceiverId>" + UriParam.IdpHeader.TRT.Receiver.ReceiverId + "</idp1:ReceiverId>";
            requestXml += "<idp1:ReceiverSys>" + UriParam.IdpHeader.TRT.Receiver.ReceiverSys + "</idp1:ReceiverSys>";
            requestXml += "</idp1:Receiver>";
            requestXml += "</idp1:TRT>";
            requestXml += "<idp1:E2E>";
            requestXml += "<idp1:E2ESrvcNm>" + UriParam.IdpHeader.E2E.E2ESrvcNm + "</idp1:E2ESrvcNm>";
            requestXml += "<idp1:E2EMsgId>" + UriParam.IdpHeader.E2E.E2EMsgId + "</idp1:E2EMsgId>";
            requestXml += "<idp1:XMLCrtDt>" + UriParam.IdpHeader.E2E.XMLCrtDt.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + "</idp1:XMLCrtDt>";
            requestXml += "<idp1:Sender>";
            requestXml += "<idp1:E2ESndrId>" + UriParam.IdpHeader.E2E.Sender.E2ESndrId + "</idp1:E2ESndrId>";
            requestXml += "<idp1:E2ESndrSys>" + UriParam.IdpHeader.E2E.Sender.E2ESndrSys + "</idp1:E2ESndrSys>";
            requestXml += "</idp1:Sender>";
            requestXml += "<idp1:Receiver>";
            requestXml += "<idp1:E2ERcvrId>" + UriParam.IdpHeader.E2E.Receiver.E2ERcvrId + "</idp1:E2ERcvrId>";
            requestXml += "<idp1:E2ERcvrSys>" + UriParam.IdpHeader.E2E.Receiver.E2ERcvrSys + "</idp1:E2ERcvrSys>";
            requestXml += "</idp1:Receiver>";
            requestXml += "</idp1:E2E>";
            requestXml += "</idp1:IdpHeader>";
            requestXml += "<idp:IdpBody>";
            foreach (srVerificaStato.idPagamento myPag in UriParam.IdpBody.IdPagamento)
                requestXml += "<idp:IdPagamento TipoPendenza='" + myPag.TipoPendenza + "'>" + myPag.Value + "</idp:IdPagamento>";
            requestXml += "<idp:richiestaInformazioniPagamento>true</idp:richiestaInformazioniPagamento>";
            requestXml += "</idp:IdpBody>";
            requestXml += "</idp:IdpVerificaStatoPagamento>";
            requestXml += "</prox:IdpVerificaStatoPagamenti>";
            requestXml += "</SOAP-ENV:Body>";
            requestXml += "</SOAP-ENV:Envelope>";

            byte[] bytes;
            bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
            request.ContentType = "text/xml; encoding='utf-8'";
            request.ContentLength = bytes.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();

            try
            {
                MakeRequest(
                   request,
                   (response) =>
                   {
                       if (successAction != null)
                       {
                           try
                           {
                               successAction(response);
                           }
                           catch (Exception ex)
                           {
                               errorAction(ex);
                           }
                       }
                   },
                   (error) =>
                   {
                       if (errorAction != null)
                       {
                           errorAction(error);
                       }
                   }
                );
            }
            catch (Exception ex)
            {
                string sErr = ex.Message;
            }
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="successAction"></param>
        /// <param name="errorAction"></param>
        private void MakeRequest(System.Net.HttpWebRequest request, Action<string> successAction, Action<Exception> errorAction)
        {
            try
            {
                Log.Debug("MakeRequest.inizio");
                System.Net.ServicePointManager.SecurityProtocol = /*System.Net.SecurityProtocolType.Ssl3 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | */System.Net.SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.Expect100Continue = false;
                Log.Debug("SecurityProtocol solo System.Net.SecurityProtocolType.Tls12");
                using (System.Net.HttpWebResponse webResponse = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    Log.Debug("MakeRequest.ho webresponse");
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(webResponse.GetResponseStream()))
                    {
                        Log.Debug("MakeRequest.ho responsestream");
                        var objText = reader.ReadToEnd();
                        Log.Debug("MakeRequest.risposta."+objText);
                        successAction(objText);
                    }
                }
            }
            catch (HttpException ex)
            {
                Log.Debug("MakeRequest.HttpException.", ex);
                errorAction(ex);
            }
            catch (Exception err)
            {
                Log.Debug("MakeRequest.Exception.", err);
                errorAction(err);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseBody"></param>
        /// <returns></returns>
        private T Deserialize<T>(string responseBody)
        {
            try
            {
                var toReturns = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseBody);
                return toReturns;
            }
            catch (Exception ex)
            {
                string errores;
                errores = ex.Message;
            }
            var toReturn = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(responseBody);
            return toReturn;
        }
        #region "Conversioni"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mySource"></param>
        /// <param name="errorAction"></param>
        /// <returns></returns>
        private List<SPC_DichICI> CastResponseAsRepository(CatastoBySoggetto mySource, Action<Exception> errorAction)
        {
            List<SPC_DichICI> ListData = new List<SPC_DichICI>();
            List<SPC_DichICI> ListDistinctData = new List<SPC_DichICI>();
            List<GenericCategory> ListTipologia = new List<GenericCategory>();
            List<GenericCategory> ListCategorie = new List<GenericCategory>();
            List<GenericCategory> ListClasse = new List<GenericCategory>();
            List<GenericCategoryWithRate> ListZona = new List<GenericCategoryWithRate>();
            List<GenericCategory> ListDiritto = new List<GenericCategory>();
            List<GenericCategoryWithRate> ListVincoli = new List<GenericCategoryWithRate>();

            try
            {
                ListTipologia = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Caratteristica, string.Empty, string.Empty);
                ListCategorie = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Categorie, string.Empty, string.Empty);
                ListClasse = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Classe, string.Empty, string.Empty);
                ListDiritto = new BLL.Settings().LoadConfig(MySession.Current.Ente.IDEnte, 0, GenericCategory.TIPO.ICI_Possesso, string.Empty, string.Empty);
                ListZona = new BLL.Settings().LoadTariffe(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.ICI_Zone, string.Empty, string.Empty);
                ListVincoli = new BLL.Settings().LoadTariffe(MySession.Current.Ente.IDEnte, DateTime.Now.Year, GenericCategory.TIPO.ICI_Vincoli, string.Empty, string.Empty);

                ListData = CastFabbricato(mySource, ListCategorie, ListClasse, ListZona, ListDiritto);
                foreach (SPC_DichICI myUI in ListData)
                {
                    ListDistinctData.Add(myUI);
                }
                ListData = CastTerreno(mySource, ListTipologia, ListClasse, ListZona, ListDiritto, ListVincoli, ListCategorie);
                foreach (SPC_DichICI myUI in ListData)
                {
                    ListDistinctData.Add(myUI);
                }

                for (int x = 0; x <= ListDistinctData.Count() - 1; x++)
                {
                    ListDistinctData[x].IDRifOrg = x + 1;
                }
            }
            catch (HttpException ex)
            {
                ListDistinctData = new List<SPC_DichICI>();
                errorAction(ex);
            }
            return ListDistinctData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="RepositoryReturn"></param>
        /// <param name="errorAction"></param>
        /// <returns></returns>
        private List<RiepilogoUI> CastResponseAsRiepilogo(List<SPC_DichICI> RepositoryReturn, Action<Exception> errorAction)
        {
            List<RiepilogoUI> ListData = new List<RiepilogoUI>();

            try
            {
                SPC_DichICI myPrec = new SPC_DichICI();
                bool AddUI = false;

                foreach (SPC_DichICI myUI in RepositoryReturn)
                {
                    RiepilogoUI myItem = new RiepilogoUI();
                    if (myUI.DescrTipologia.ToLower().IndexOf("agricolo") > 0)
                    {
                        if (myUI.Foglio == myPrec.Foglio && myUI.Numero == myPrec.Numero && myUI.Sub == myPrec.Sub && myUI.IDTipologia == myPrec.IDTipologia)
                        {
                            AddUI = false;
                        }
                        else
                        {
                            AddUI = true;
                        }
                    }
                    else {
                        AddUI = true;
                    }
                    if (AddUI)
                    {
                        myItem.Ubicazione = myUI.Via + " " + myUI.Civico;
                        myItem.Foglio = myUI.Foglio;
                        myItem.Numero = myUI.Numero;
                        myItem.Sub = myUI.Sub;
                        myItem.Dal = myUI.DataInizio;
                        myItem.Al = myUI.DataFine;
                        myItem.CodCategoria = myUI.DescrCat;
                        myItem.RenditaValore = myUI.RenditaValore;
                        if (myUI.DescrTipologia.ToLower().IndexOf("agricolo") > 0)
                        {
                            myItem.DescrUtilizzo = "Terreno";
                        }
                        else if (myUI.DescrTipologia.ToLower().IndexOf("icabil") > 0)
                        {
                            myItem.DescrUtilizzo = "Area Edificabile";
                            myItem.MQCatasto = myUI.PercRiduzione;
                            myUI.PercRiduzione = 0;
                        }
                        else
                        {
                            myItem.DescrUtilizzo = "Fabbricato";
                        }
                        myItem.PercPossesso = myUI.PercPossesso;
                        myItem.MQ = myUI.Consistenza;
                        myItem.Zona = myUI.Zona;
                        myItem.Stato = "M";
                        myItem.IDRifOrg = myUI.IDRifOrg;
                        ListData.Add(myItem);
                    }
                    else {
                        int n = ListData.Count - 1;
                        ListData[n].MQ += myUI.Consistenza;
                        ListData[n].MQCatasto += myUI.PercRiduzione;
                    }
                    myPrec = myUI;
                }
            }
            catch (HttpException ex)
            {
                ListData = new List<RiepilogoUI>();
                errorAction(ex);
            }
            return ListData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mySource"></param>
        /// <param name="ListCategorie"></param>
        /// <param name="ListClasse"></param>
        /// <param name="ListZona"></param>
        /// <param name="ListDiritto"></param>
        /// <returns></returns>
        private List<SPC_DichICI> CastFabbricato(CatastoBySoggetto mySource, List<GenericCategory> ListCategorie, List<GenericCategory> ListClasse, List<GenericCategoryWithRate> ListZona, List<GenericCategory> ListDiritto)
        {
            List<SPC_DichICI> ListData = new List<SPC_DichICI>();

            try
            {
                if (mySource.fabbricati != null)
                {
                    foreach (Fabbricato myGroupUI in mySource.fabbricati)
                    {
                        List<SPC_DichICI> ListGroupData = new List<SPC_DichICI>();
                        SPC_DichICI myItem = new SPC_DichICI();
                        if (myGroupUI.item_fabbricati != null)
                        {
                            foreach (DatiFabbricato myUI in myGroupUI.item_fabbricati)
                            {
                                myItem = new SPC_DichICI();
                                if ((myUI.dataefficacia_finale != null ? DateTime.Parse(myUI.dataefficacia_finale).Year : DateTime.MaxValue.Year) >= (DateTime.Now.Year - 1))
                                {
                                    myItem.Civico = (myUI.civico1 != null ? myUI.civico1 : string.Empty);
                                    myItem.Consistenza = (myUI.consistenza != null ? decimal.Parse(myUI.consistenza.Replace(".", ",")) : 0);
                                    myItem.DataInizio = (myUI.dataefficacia_iniziale != null ? DateTime.Parse(myUI.dataefficacia_iniziale) : DateTime.MaxValue);
                                    myItem.DataFine = (myUI.dataefficacia_finale != null ? DateTime.Parse(myUI.dataefficacia_finale) : DateTime.MaxValue);
                                    myItem.Foglio = (myUI.foglio != null ? myUI.foglio : string.Empty);
                                    if (myUI.categoria != null)
                                    {
                                        foreach (GenericCategory myGenItem in ListCategorie)
                                        {
                                            if (myGenItem.IDOrg == myUI.categoria)
                                            {
                                                myItem.IDCategoria = myGenItem.ID;
                                                myItem.DescrCat = myGenItem.Codice;
                                                break;
                                            }
                                        }
                                    }
                                    if (myUI.classe != null)
                                    {
                                        foreach (GenericCategory myGenItem in ListClasse)
                                        {
                                            if (myGenItem.Descrizione == myUI.classe.Replace("0", string.Empty))
                                            {
                                                myItem.IDClasse = myGenItem.Codice;
                                                break;
                                            }
                                        }
                                    }
                                    if (myUI.zona != null)
                                    {
                                        foreach (GenericCategoryWithRate myGenItem in ListZona)
                                        {
                                            if (myGenItem.IDOrg == myUI.zona)
                                            {
                                                myItem.IDZona = myGenItem.ID;
                                                myItem.Zona = myGenItem.Descrizione;
                                                break;
                                            }
                                        }
                                    }
                                    myItem.Numero = (myUI.numero != null ? myUI.numero : string.Empty);
                                    myItem.RenditaValore = (myUI.renditaeuro != null ? decimal.Round(decimal.Parse(myUI.renditaeuro.Replace(".", ",")), 2) : 0);
                                    myItem.Sub = (myUI.subalterno != null ? myUI.subalterno : string.Empty);
                                    myItem.Via = (myUI.indirizzo != null ? myUI.indirizzo : string.Empty);
                                    myItem.LinkGIS = myUI.link_mappa;
                                    ListGroupData.Add(myItem);
                                }
                            }
                        }
                        if (myGroupUI.item_intestatari != null)
                        {
                            foreach (Intestatario myInt in myGroupUI.item_intestatari)
                            {
                                foreach (SPC_DichICI myRiep in ListGroupData)
                                {
                                    if (myInt.diritto != null)
                                    {
                                        foreach (GenericCategory myGenItem in ListDiritto)
                                        {
                                            if (myGenItem.Descrizione == myInt.diritto.Replace("'", string.Empty))
                                            {
                                                myRiep.IDPossesso = myGenItem.ID;
                                                break;
                                            }
                                        }
                                    }
                                    if (myInt.quotanum > 0 && myInt.quotaden > 0)
                                        myRiep.PercPossesso = (decimal.Parse(myInt.quotanum.ToString() ) / decimal.Parse(myInt.quotaden.ToString() )) * 100;
                                }
                            }
                        }
                        foreach (SPC_DichICI myGroup in ListGroupData)
                            ListData.Add(myGroup);
                    }
                }
            }
            catch
            {
                return null;
            }
            return ListData;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mySource"></param>
        /// <param name="ListTipologia"></param>
        /// <param name="ListClasse"></param>
        /// <param name="ListZona"></param>
        /// <param name="ListDiritto"></param>
        /// <param name="ListVincoli"></param>
        /// <param name="ListCategoria"></param>
        /// <returns></returns>
        private List<SPC_DichICI> CastTerreno(CatastoBySoggetto mySource, List<GenericCategory> ListTipologia, List<GenericCategory> ListClasse, List<GenericCategoryWithRate> ListZona, List<GenericCategory> ListDiritto, List<GenericCategoryWithRate> ListVincoli, List<GenericCategory> ListCategoria)
        {
            List<SPC_DichICI> ListData = new List<SPC_DichICI>();

            try
            {
                if (mySource.terreni != null)
                {
                    foreach (Terreno myGroupUI in mySource.terreni)
                    {
                        List<SPC_DichICI> ListGroupData = new List<SPC_DichICI>();
                        SPC_DichICI myItem = new SPC_DichICI();
                        if (myGroupUI.item_terreni != null)
                        {
                            foreach (DatiTerreno myUI in myGroupUI.item_terreni)
                            {
                                myItem = new SPC_DichICI();
                                if ((myUI.dataefficacia_finale != null ? DateTime.Parse(myUI.dataefficacia_finale).Year : DateTime.MaxValue.Year) >= (DateTime.Now.Year - 1))
                                {
                                    if (myUI.item_strumenti_urbanistici != null)
                                    {
                                        List<SPC_DichICI> ListSubGroupData = new List<SPC_DichICI>();

                                        foreach (DatiUrbanistici myUrb in myUI.item_strumenti_urbanistici)
                                        {
                                            if ((myUrb.ctg_cod != null ? myUrb.ctg_cod : string.Empty) == "Zonizzazioni")
                                            {
                                                myItem = new SPC_DichICI();
                                                myItem.Consistenza = decimal.Parse(myUrb.cdu_area.Replace(".", ","));
                                                myItem.PercRiduzione = decimal.Parse(myUrb.cdu_percentuale.Replace(".", ","));
                                                myItem.DataInizio = (myUI.dataefficacia_inizio != null ? DateTime.Parse(myUI.dataefficacia_inizio) : DateTime.MaxValue);
                                                myItem.DataFine = (myUI.dataefficacia_finale != null ? DateTime.Parse(myUI.dataefficacia_finale) : DateTime.MaxValue);
                                                myItem.Foglio = (myUI.foglio != null ? myUI.foglio : string.Empty);
                                                myItem.Numero = (myUI.numero != null ? myUI.numero : string.Empty);
                                                myItem.Sub = (myUI.subalterno != null ? myUI.subalterno : string.Empty);
                                                myItem.RenditaValore = decimal.Round(myUI.redditodomeuro, 2);

                                                if ((myUrb.zon_descr != null ? myUrb.zon_descr : string.Empty).ToLower().IndexOf("agricol") > 1)
                                                {
                                                    foreach (GenericCategory myGenItem in ListTipologia)
                                                    {
                                                        if (myGenItem.Descrizione.ToLower().IndexOf("agricol") > 0)
                                                        {
                                                            myItem.IDTipologia = myGenItem.ID;
                                                            myItem.DescrTipologia = myGenItem.Descrizione;
                                                            myItem.PercRiduzione = 0;
                                                            break;
                                                        }
                                                    }
                                                    foreach (GenericCategory myGenItem in ListCategoria)
                                                    {
                                                        if (myGenItem.Codice.ToUpper() == "TA")
                                                        {
                                                            myItem.IDCategoria = myGenItem.ID;
                                                            break;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (GenericCategory myGenItem in ListTipologia)
                                                    {
                                                        if (myGenItem.Descrizione.ToLower().IndexOf("icabil") > 0)
                                                        {
                                                            myItem.IDTipologia = myGenItem.ID;
                                                            myItem.DescrTipologia = myGenItem.Descrizione;
                                                            break;
                                                        }
                                                    }
                                                    foreach (GenericCategory myGenItem in ListCategoria)
                                                    {
                                                        if (myGenItem.Codice.ToUpper() == "AF")
                                                        {
                                                            myItem.IDCategoria = myGenItem.ID;
                                                            break;
                                                        }
                                                    }

                                                    foreach (GenericCategoryWithRate myZona in ListZona)
                                                    {
                                                        if (myUrb.prg_nta == null)
                                                        {
                                                            myUrb.prg_nta = "";
                                                        }
                                                        if (myZona.IDOrg == myUrb.zon_cod + myUrb.prg_nta.Replace("null", ""))
                                                        {
                                                            myItem.IDZona = myZona.ID;
                                                            myItem.Zona = myZona.Descrizione;
                                                            if (myZona.Valore > 0)
                                                                myItem.RenditaValore = decimal.Round((myItem.Consistenza * myZona.Valore), 2);
                                                            break;
                                                        }
                                                    }
                                                }
                                                ListSubGroupData.Add(myItem);
                                            }
                                            else if ((myUrb.ctg_cod != null ? myUrb.ctg_cod : string.Empty) == "Vincoli")
                                            {
                                                foreach (GenericCategoryWithRate myVincolo in ListVincoli)
                                                {
                                                    if (myVincolo.IDOrg == myUrb.zon_cod)
                                                    {
                                                        foreach (SPC_DichICI myTmp in ListSubGroupData)
                                                        {
                                                            if (myTmp.Foglio == myUrb.cat_foglio && myTmp.Numero == myUrb.cat_mappale)
                                                            {
                                                                if (myTmp.DescrTipologia.ToLower().IndexOf("agricol") <= 0)
                                                                    myTmp.ListVincoli.Add(myVincolo.ID.ToString());
                                                            }
                                                        }
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        foreach (SPC_DichICI mySingle in ListSubGroupData)
                                            ListGroupData.Add(mySingle);
                                    }
                                    else {
                                        myItem.Consistenza = (myUI.are != null ? decimal.Parse(myUI.are) * 100 : 0) + (myUI.centiare != null ? decimal.Parse(myUI.centiare) : 0);
                                        myItem.DataInizio = (myUI.dataefficacia_inizio != null ? DateTime.Parse(myUI.dataefficacia_inizio) : DateTime.MaxValue);
                                        myItem.DataFine = (myUI.dataefficacia_finale != null ? DateTime.Parse(myUI.dataefficacia_finale) : DateTime.MaxValue);
                                        myItem.Foglio = (myUI.foglio != null ? myUI.foglio : string.Empty);
                                        if (myUI.classe != null)
                                        {
                                            foreach (GenericCategory myGenItem in ListClasse)
                                            {
                                                if (myGenItem.Descrizione == myUI.classe.Replace("0", string.Empty))
                                                {
                                                    myItem.IDClasse = myGenItem.ID.ToString();
                                                    break;
                                                }
                                            }
                                        }
                                        myItem.Numero = (myUI.numero != null ? myUI.numero : string.Empty);
                                        myItem.RenditaValore = decimal.Round(myUI.redditodomeuro, 2) ;
                                        myItem.Sub = (myUI.subalterno != null ? myUI.subalterno : string.Empty);
                                        ListGroupData.Add(myItem);
                                    }
                                }
                            }
                        }
                        if (myGroupUI.item_intestatari != null)
                        {
                            foreach (Intestatario myInt in myGroupUI.item_intestatari)
                            {
                                foreach (SPC_DichICI myRiep in ListGroupData)
                                {
                                    if (myInt.diritto != null)
                                    {
                                        foreach (GenericCategory myGenItem in ListDiritto)
                                        {
                                            if (myGenItem.Descrizione == myInt.diritto.Replace("'", string.Empty))
                                            {
                                                myRiep.IDPossesso = myGenItem.ID;
                                                break;
                                            }
                                        }
                                    }
                                    if (myInt.quotanum > 0 && myInt.quotaden > 0)
                                        myRiep.PercPossesso = (decimal.Parse( myInt.quotanum.ToString() ) / decimal.Parse( myInt.quotaden.ToString() )) * 100;
                                }
                            }
                        }
                        foreach (SPC_DichICI myGroup in ListGroupData)
                            ListData.Add(myGroup);
                    }
                }
            }
            catch 
            {
                return null;
            }
            return ListData;
        }
        #endregion
    }
}