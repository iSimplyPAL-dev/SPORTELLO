using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Web.UI.WebControls;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Classe di gestione griglie
    /// </summary>
    public class FunctionGrd
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(FunctionGrd));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string FormattaItemGrd(object item)
        {
            string myRet = string.Empty;

            if (item != null)
                myRet = item.ToString();

            return myRet.Trim();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool FormattaBoolGrd(object item)
        {
            bool myRet = false;

            if (item != null)
            {
                bool.TryParse(item.ToString(), out myRet);
                if (item.ToString() == "1" || item.ToString() == "true")
                    myRet = true;
            }

            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tDataGrd"></param>
        /// <returns></returns>
        public string FormattaDataGrd(object tDataGrd)
        {
            string myRet = "";

            if (tDataGrd != null)
            {
                DateTime myDate = DateTime.MaxValue;
                DateTime.TryParse(tDataGrd.ToString(), out myDate);
                if (myDate == DateTime.MinValue || myDate == DateTime.MaxValue || myDate.ToString() == DateTime.MaxValue.ToShortDateString() || myDate.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                    myRet = "";
                else
                    myRet = myDate.ToShortDateString();
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <returns></returns>
        public string FormattaHideIdListGrd(object oList)
        {
            string myRet = "";
            try
            {
                if (oList != null)
                {
                    foreach (string myItem in (List<string>)oList)
                    {
                        myRet += myItem + "|";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaHideIdListGrd::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <returns></returns>
        public string FormattaHideListGenGrd(object oList)
        {
            string myRet = "";
            try
            {
                if (oList != null)
                {
                    foreach (GenericCategory myItem in (List<GenericCategory>)oList)
                    {
                        if (myItem.IsActive == 1)
                            myRet += myItem.Codice + "|";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaHideListGenGrd::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <returns></returns>
        public string FormattaHideListMailGrd(object oList)
        {
            string myRet = "";
            try
            {
                if (oList != null)
                {
                    BaseMail myItem = (BaseMail)oList;
                    if (myItem.Sender != string.Empty)
                    {
                        myRet += myItem.Sender;
                        myRet += "|" + myItem.SenderName;
                        myRet += "|" + myItem.SSL.ToString();
                        myRet += "|" + myItem.Server;
                        myRet += "|" + myItem.ServerPort;
                        myRet += "|" + myItem.Password;
                        myRet += "|" + myItem.Ente;
                        myRet += "|" + myItem.BackOffice;
                        myRet += "|" + myItem.Archive;
                        myRet += "|" + myItem.Protocollo;
                        myRet += "|" + myItem.WarningRecipient;
                        myRet += "|" + myItem.WarningSubject;
                        myRet += "|" + myItem.WarningMessage;
                        myRet += "|" + myItem.SendErrorMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaHideListMailGrd::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <returns></returns>
        public string FormattaHideListCartografiaGrd(object oList)
        {
            string myRet = "";

            try
            {
                if (oList != null)
                {
                    BaseCartografia myItem = (BaseCartografia)oList;
                    myRet += myItem.Url;
                    myRet += "|" + myItem.Token;
                    myRet += "|" + myItem.IsActive.ToString();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaHideListCartografiaGrd::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <returns></returns>
        public string FormattaHideListVerticaliGrd(object oList)
        {
            string myRet = "";
            try
            {
                if (oList != null)
                {
                    BaseVerticali myItem = (BaseVerticali)oList;
                    myRet += myItem.AnnoVerticaleICI.ToString();
                    myRet += "|" + myItem.AnniUsoGratuito;
                    myRet += "|" + FormattaDataGrd(myItem.DataAggiornamento);
                    myRet += "|" + myItem.TipoBancaDati;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaHideListVerticaliGrd::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oList"></param>
        /// <returns></returns>
        public string FormattaHideListPagoPAGrd(object oList)
        {
            string myRet = "";
            try
            {
                if (oList != null)
                {
                    BasePagoPA myItem = (BasePagoPA)oList;
                    myRet += myItem.CARTId;
                    myRet += "|" + myItem.CARTSys;
                    myRet += "|" + myItem.IBAN;
                    myRet += "|" + myItem.DescrIBAN;
                    myRet += "|" + myItem.IdRiscossore;
                    myRet += "|" + myItem.DescrRiscossore;
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaHideListPagoPAGrd::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string FormattaCSSStato(object item)
        {
            string myRet = string.Empty;

            myRet = "div_Stato" + item.ToString() + "Grd";
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public string FormattaCSSCorrispondenza(object item)
        {
            string myRet = "imgGrd ";

            switch (item.ToString())
            {
                case "S"://SAME
                    myRet += "BottoneMatchGrd";
                    break;
                case "D"://DISTINCT
                    myRet += "BottoneNoMatchGrd";
                    break;
                case "M"://MISSING
                    myRet += "BottoneMissingGrd";
                    break;
                default:
                    break;
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cognome"></param>
        /// <param name="Nome"></param>
        /// <returns></returns>
        public string FormattaNominativo(object Cognome, object Nome)
        {
            string myRet = string.Empty;

            if (Cognome != null)
                myRet = Cognome.ToString();

            if (Nome != null)
            {
                if (Nome.ToString() != "")
                    myRet += " " + Nome.ToString();
            }

            return myRet.Trim();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodFiscale"></param>
        /// <param name="PartitaIva"></param>
        /// <returns></returns>
        public string FormattaCFPIVA(object CodFiscale, object PartitaIva)
        {
            string myRet = string.Empty;

            if (PartitaIva != null)
            {
                if (PartitaIva.ToString() != "")
                    myRet = PartitaIva.ToString();
                else
                    if (CodFiscale != null)
                    myRet = CodFiscale.ToString();
                else
                    myRet = "";
            }
            else if (CodFiscale != null)
                myRet = CodFiscale.ToString();

            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Via"></param>
        /// <param name="Civico"></param>
        /// <param name="Esponente"></param>
        /// <param name="Scala"></param>
        /// <param name="Interno"></param>
        /// <param name="Frazione"></param>
        /// <param name="Foglio"></param>
        /// <param name="Numero"></param>
        /// <param name="Subalterno"></param>
        /// <returns></returns>
        public string FormattaVia(object Via, object Civico, object Esponente, object Scala, object Interno, object Frazione, object Foglio, object Numero, object Subalterno)
        {
            string myRet = string.Empty;

            if (Via != null)
                myRet = Via.ToString();
            else
                myRet = "";

            if (Civico != null)
            {
                if (Civico.ToString() != "0")
                    myRet += " " + Civico.ToString();
                else
                    myRet += "";

            }
            if (Esponente != null)
            {
                if (Esponente.ToString() != "")
                    myRet += " " + Esponente.ToString();
                else
                    myRet += "";
            }
            else
                myRet += "";
            if (Scala != null)
            {
                if (Scala.ToString() != "")
                    myRet += " " + Scala.ToString();
                else
                    myRet += "";

            }
            else
                myRet += "";

            if (Interno != null)
            {
                if (Interno.ToString() != "")
                    myRet += " " + Interno.ToString();
                else
                    myRet += "";
            }
            else
                myRet += "";

            if (Frazione != null)
            {
                if (Frazione.ToString() != "")
                    myRet += " " + Frazione.ToString();
                else
                    myRet += "";
            }
            else
                myRet += "";

            if (Foglio != null)
            {
                if (Foglio.ToString() != "")
                    myRet += " (" + Foglio.ToString();
                else
                    myRet += "";
            }
            else
                myRet += "";

            if (Numero != null)
            {
                if (Numero.ToString() != "")
                    myRet += "/" + Numero.ToString();
                else
                    myRet += "";
            }
            else
                myRet += "";

            if (Subalterno != null)
            {
                if (Subalterno.ToString() != "")
                    myRet += "/" + Subalterno.ToString();
                else
                    myRet += "";
            }
            else
                myRet += "";

            if (Foglio != null)
            {
                if (Foglio.ToString() != "")
                    myRet += ")";
                else
                    myRet += "";
            }

            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CAP"></param>
        /// <param name="Comune"></param>
        /// <param name="Prov"></param>
        /// <returns></returns>
        public string FormattaComune(object CAP, object Comune, object Prov)
        {
            string myRet = "";
            try
            {
                if (CAP != null)
                    if (CAP.ToString() != "")
                        myRet += CAP.ToString();

                if (Comune != null)
                    if (Comune.ToString() != "")
                        myRet += " " + Comune.ToString();

                if (Prov != null)
                    if (Prov.ToString() != "")
                        myRet += " (" + Prov.ToString() + ")";
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaComune::errore::", ex);
                myRet = "";
            }
            return myRet.Trim();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prdStatus"></param>
        /// <returns></returns>
        public string FormattaContatto(object prdStatus)
        {
            string myRet = string.Empty;
            try
            {
                Anagrafica.DLL.GestioneAnagrafica oAnagrafica = new Anagrafica.DLL.GestioneAnagrafica();
                myRet = oAnagrafica.DescrizioneTipoContatto(prdStatus, RouteConfig.StringConnectionAnagrafica);
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaContatto::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Articolo"></param>
        /// <returns></returns>
        public bool FormattaHasRidDet(object Articolo)
        {
            bool myRet = false;
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListMotivi"></param>
        /// <returns></returns>
        public string FormattaListMotivazioni(object ListMotivi)
        {
            string myRet = string.Empty;
            try
            {
                if (ListMotivi != null)
                {
                    foreach (IstanzaMotivazione myItem in (List<IstanzaMotivazione>)ListMotivi)
                    {
                        if (myRet != string.Empty)
                            myRet += ",";
                        myRet += " " + myItem.Note;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaListMotivazioni::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListAttach"></param>
        /// <returns></returns>
        public string FormattaPresenzaAllegati(object ListAttach)
        {
            string myRet = "NO";
            try
            {
                if (ListAttach != null)
                {
                    if (((List<IstanzaAllegato>)ListAttach).Count > 0)
                        myRet = "SI";
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.FunctionGrd.FormattaPresenzaAllegati::errore::", ex);
                myRet = "";
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NDich"></param>
        /// <param name="DescrIstanza"></param>
        /// <returns></returns>
        public string FormattaNDichIstanza(object NDich, object DescrIstanza)
        {
            string myRet = string.Empty;

            if (DescrIstanza != null)
            {
                if (NDich != null && DescrIstanza.ToString() != "Anagrafica" && DescrIstanza.ToString() != "Delega" && DescrIstanza.ToString() != "Pagamento")
                {
                    int n = 0;
                    int.TryParse(NDich.ToString(), out n);
                    if (n>0)
                    myRet = "N. " + NDich.ToString();
                    else
                        myRet = NDich.ToString();
                }
                if (DescrIstanza.ToString() != "")
                    myRet += " " + DescrIstanza.ToString();
            }

            return myRet.Trim();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MQ"></param>
        /// <param name="MQCastasto"></param>
        /// <returns></returns>
        public string FormattaMQAreeFabCatasto(object MQ, object MQCastasto)
        {
            string myRet = "";

            if (MQCastasto != null)
            {
                if (float.Parse(MQCastasto.ToString())>0)
                    myRet = float.Parse(MQ.ToString()).ToString("#,##0.00")+" ("+ float.Parse(MQCastasto.ToString()).ToString("#,##0.00")+"%)";
                else
                    myRet = float.Parse(MQ.ToString()).ToString("#,##0.00");
            }
            return myRet;
        }
        #region "Sorting"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public string ConvertSortDirectionToSql(SortDirection sortDirection)
        {
            string newSortDirection = String.Empty;

            switch (sortDirection)
            {
                case SortDirection.Ascending:
                    newSortDirection = "ASC";
                    break;

                case SortDirection.Descending:
                    newSortDirection = "DESC";
                    break;
            }
            return newSortDirection;
        }
        #endregion
    }
}