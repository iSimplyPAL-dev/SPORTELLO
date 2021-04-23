using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using ImportInterface;
using System.Text;
using OPENgovSPORTELLO.Models;
using System.Threading;
using System.IO;
using System.Data;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Classe di gestione importazione dati
    /// </summary>
    public class BLLImport
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BLLImport));
        private List<Utility.DichManagerSTRADE.Stradario> ListVie = new List<Utility.DichManagerSTRADE.Stradario>();
        private BLL.Settings fncMng = new BLL.Settings();

        #region Import flussi carico
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="CFPIVA"></param>
        /// <param name="IdSoggetto"></param>
        public void ReadDatiVerticale(string IdEnte, string CFPIVA, ref string IdSoggetto)
        {
            try
            {
                Log.Debug("BLLImport::ReadDatiVerticale::sono dentro>");
                IdSoggetto = "";

                string requestUriParam = String.Format("?CFPIVA={0}"
                                            , (MySession.Current.myAnag.CodiceFiscale != string.Empty ? MySession.Current.myAnag.CodiceFiscale : MySession.Current.myAnag.PartitaIva)
                                        );
                string sErr = string.Empty;
                TributiModel myResponse = new TributiModel();

                Log.Debug("Richiamo import Url->" + MySettings.GetConfig("UrlImportDati") + " :: Param->" + requestUriParam);
                new BLL.RestService().MakeRequestForDatiVerticale<TributiModel>(MySettings.GetConfig("UrlImportDati")
                    , requestUriParam
                    , result => myResponse = result
                    , error => sErr = error.Message
                    , "Token: " + GetTokenGWImport(new BLL.RestService().UserImport, new BLL.RestService().ReasonImport, IdEnte, MySession.Current.Ente.DatiVerticali.TipoFornitore, MySettings.GetConfig("PathImportDati") + IdEnte + "\\")
                    );
                Log.Debug("restituito oggetto");
                if (sErr != string.Empty)
                {
                    Log.Debug("OPENgovSPORTELLO.BLLImport.ReadDatiVerticale::errore::" + sErr);
                }
                else
                {
                    if (myResponse.Stato == "200 OK")
                    {
                        Log.Debug("BLLImport::ReadDatiVerticale::200>");
                        if (myResponse.Anagrafica != null)
                        {
                            Log.Debug("BLLImport::ReadDatiVerticale::Anagrafica!=null>");
                            IdSoggetto = myResponse.Anagrafica.CodIndividuale;
                            Log.Debug("lavoro ANAGRAFE id->" + IdSoggetto);
                            ListVie = new Utility.DichManagerSTRADE(RouteConfig.TypeDB, RouteConfig.StringConnectionStradario).GetStrade(IdEnte);
                            try { Log.Debug("ho trovato " + ListVie.Count.ToString() + " vie"); }
                            catch
                            { Log.Debug("non ho trovato vie"); }
                            if ((myResponse.Anagrafica.CodIndividuale != "" ? int.Parse(myResponse.Anagrafica.CodIndividuale) : 0) > 0)
                            {

                                Log.Debug("devo convertire ANAGRAFICA");
                                AnagInterface.DettaglioAnagrafica myAnag = CastToAnag(MySession.Current.myAnag.COD_CONTRIBUENTE,myResponse.Anagrafica, ListVie);
                                Log.Debug("devo salvare ANAGRAFICA");
                                AnagInterface.DettaglioAnagraficaReturn Anag = new Anagrafica.DLL.GestioneAnagrafica().GestisciAnagrafica(myAnag, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, true, true);
                                if (int.Parse(Anag.COD_CONTRIBUENTE) <= 0)
                                    new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "Impossibile salvare l'anagrafica! Identificativo:" + myAnag.CodIndividuale, "", string.Empty, string.Empty);
                                else
                                {
                                    MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(int.Parse(Anag.COD_CONTRIBUENTE), Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                                    if (myResponse.Dich8852 != null)
                                    {
                                        Log.Debug("lavoro DICH8852");
                                        List<GenericCategory> ListTipo = new List<GenericCategory>();
                                        List<GenericCategory> ListPossesso = new List<GenericCategory>();
                                        ListTipo = new BLL.Settings().LoadConfig(IdEnte, 0, GenericCategory.TIPO.ICI_Caratteristica, string.Empty, string.Empty);
                                        ListPossesso = new BLL.Settings().LoadConfig(IdEnte, 0, GenericCategory.TIPO.ICI_Possesso, string.Empty, string.Empty);
                                        Utility.DichManagerICI fncICI = new Utility.DichManagerICI(RouteConfig.TypeDB, RouteConfig.StringConnectionICI);
                                        fncICI.ClearBancaDatiDich(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo convertire DICH8852 testate->" + myResponse.Dich8852.Count.ToString());
                                        List<Utility.DichManagerICI.TestataRow> ListDich8852 = CastToDich8852(myResponse.Dich8852, int.Parse(Anag.COD_CONTRIBUENTE), ListVie, ListPossesso, ListTipo);
                                        Log.Debug("devo salvare DICH8852");
                                        foreach (Utility.DichManagerICI.TestataRow myDich8852 in ListDich8852)
                                        {
                                            int IdTestata = 0;
                                            if (!fncICI.SetTestata(Utility.Costanti.AZIONE_NEW, myDich8852, out IdTestata))
                                            {
                                                new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dichiarazione", "", string.Empty, string.Empty);
                                            }
                                            foreach (Utility.DichManagerICI.OggettiRow myUI in myDich8852.listOggetti)
                                            {
                                                int IDOggetto = 0;
                                                if (!fncICI.SetOggetti(Utility.Costanti.AZIONE_NEW, myUI, IdTestata, out IDOggetto))
                                                {
                                                    new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dichiarazione", "", string.Empty, string.Empty);
                                                }
                                                if (IDOggetto <= 0)
                                                {
                                                    new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dichiarazione", "", string.Empty, string.Empty);
                                                }
                                                else
                                                {
                                                    Utility.DichManagerICI.DettaglioTestataRow myDet = myUI.oDettaglio;
                                                    myDet.IdOggetto = IDOggetto;
                                                    myDet.IdTestata = IdTestata;
                                                    if (!fncICI.SetDettaglioTestata(Utility.Costanti.AZIONE_NEW, myDet, out IDOggetto))
                                                    {
                                                        new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dichiarazione", "", string.Empty, string.Empty);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (myResponse.Pag8852 != null)
                                    {
                                        Log.Debug("lavoro PAG8852");
                                        Utility.DichManagerICI fncICI = new Utility.DichManagerICI(RouteConfig.TypeDB, RouteConfig.StringConnectionICI);
                                        fncICI.ClearBancaDatiVersamenti(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo convertire PAG8852 pag->" + myResponse.Pag8852.Count.ToString());
                                        List<Utility.DichManagerICI.VersamentiRow> ListPag8852 = CastToPag8852(myResponse.Pag8852, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo salvare PAG8852");
                                        foreach (Utility.DichManagerICI.VersamentiRow myPag8852 in ListPag8852)
                                        {
                                            int IdPag = 0;
                                            if (!fncICI.SetVersamenti(Utility.Costanti.AZIONE_NEW, myPag8852, out IdPag))
                                            {
                                                new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio versamento", "", string.Empty, string.Empty);
                                            }
                                        }
                                    }
                                    if (myResponse.Dich0434 != null)
                                    {
                                        try
                                        {
                                            Log.Debug("lavoro DICH0434");
                                            Utility.DichManagerTARSU fncTARSU = new Utility.DichManagerTARSU(RouteConfig.TypeDB, RouteConfig.StringConnectionTARSU, "", MySession.Current.Ente.IDEnte);
                                            fncTARSU.ClearBancaDatiDich(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                            Log.Debug("devo convertire DICH0434 testate->" + myResponse.Dich0434.Count.ToString());
                                            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata> ListDich0434 = CastToDich0434(myResponse.Dich0434, int.Parse(Anag.COD_CONTRIBUENTE), ListVie);
                                            Log.Debug("devo salvare DICH0434");
                                            foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata myDich0434 in ListDich0434)
                                            {
                                                if (fncTARSU.SetDichiarazione(myDich0434, "") <= 0)
                                                {
                                                    new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dichiarazione", "", string.Empty, string.Empty);
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Log.Debug("ggg", ex);
                                        }

                                    }
                                    if (myResponse.RidEse0434 != null)
                                    {
                                        Log.Debug("lavoro RIDESE0434");
                                        Utility.DichManagerTARSU fncTARSU = new Utility.DichManagerTARSU(RouteConfig.TypeDB, RouteConfig.StringConnectionTARSU, "", MySession.Current.Ente.IDEnte);
                                        fncTARSU.ClearBancaDatiDichRidEse(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo salvare RIDESE0434");
                                        foreach (RidEse0434 myRidEse0434 in myResponse.RidEse0434)
                                        {
                                            if (fncTARSU.SetRidEseImport(RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRidEse.TIPO_RIDUZIONI, myRidEse0434.sNDichiarazione + "|" + myRidEse0434.CodImmobile + "|" + myRidEse0434.ProgImmobile, myRidEse0434.Codice) <= 0)
                                            {
                                                new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio riduzioni dichiarazione", "", string.Empty, string.Empty);
                                            }
                                        }
                                    }
                                    if (myResponse.Avvisi0434 != null)
                                    {
                                        Log.Debug("lavoro AVVISI0434");
                                        Utility.DichManagerTARSU fncTARSU = new Utility.DichManagerTARSU(RouteConfig.TypeDB, RouteConfig.StringConnectionTARSU, "", MySession.Current.Ente.IDEnte);
                                        fncTARSU.ClearBancaDatiAvvisi(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo convertire AVVISI0434 avvisi->" + myResponse.Avvisi0434.Count.ToString());
                                        List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso> ListAvvisi0434 = CastToAvvisi0434(myResponse.Avvisi0434, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo salvare AVVISI0434");
                                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso myAvviso0434 in ListAvvisi0434)
                                        {
                                            int IdAvviso = fncTARSU.SetAvviso(myAvviso0434, -1, Utility.Costanti.AZIONE_NEW);
                                            if (IdAvviso <= 0)
                                            {
                                                new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio avviso", "", string.Empty, string.Empty);
                                            }
                                            else
                                            {
                                                foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci myDet in myAvviso0434.oDetVoci)
                                                {
                                                    if (fncTARSU.SetDetVoci(myDet, IdAvviso, -1, Utility.Costanti.AZIONE_NEW) <= 0)
                                                    {
                                                        new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dettaglio voci avviso", "", string.Empty, string.Empty);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (myResponse.Rate0434 != null)
                                    {
                                        Log.Debug("lavoro RATE0434");
                                        Utility.DichManagerTARSU fncTARSU = new Utility.DichManagerTARSU(RouteConfig.TypeDB, RouteConfig.StringConnectionTARSU, "", MySession.Current.Ente.IDEnte);
                                        fncTARSU.ClearBancaDatiRate(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo convertire RATE0434");
                                        List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata> ListRate0434 = CastToRate0434(IdEnte,myResponse.Rate0434, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo salvare RATE0434");
                                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata myRata0434 in ListRate0434)
                                        {
                                            if (fncTARSU.SetRata(myRata0434, -1, Utility.Costanti.AZIONE_NEW) <= 0)
                                            {
                                                new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio rata", "", string.Empty, string.Empty);
                                            }
                                            else
                                            {
                                                foreach (RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRataDettaglio myDet in myRata0434.oDetVoci)
                                                {
                                                    if (fncTARSU.SetRataDettaglio(myDet, myRata0434, -1, Utility.Costanti.AZIONE_NEW) <= 0)
                                                    {
                                                        new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio dettaglio voci rata", "", string.Empty, string.Empty);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (myResponse.Pag0434 != null)
                                    {
                                        Log.Debug("lavoro PAG0434");
                                        Utility.DichManagerTARSU fncTARSU = new Utility.DichManagerTARSU(RouteConfig.TypeDB, RouteConfig.StringConnectionTARSU, "", MySession.Current.Ente.IDEnte);
                                        fncTARSU.ClearBancaDatiPagamenti(IdEnte, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo convertire PAG0434 pag->" + myResponse.Pag0434.Count.ToString());
                                        List<RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti> ListPag0434 = CastToPag0434(myResponse.Pag0434, int.Parse(Anag.COD_CONTRIBUENTE));
                                        Log.Debug("devo salvare PAG0434");
                                        foreach (RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti myPag0434 in ListPag0434)
                                        {
                                            if (fncTARSU.SetPagamento(myPag0434, Utility.Costanti.AZIONE_NEW) <= 0)
                                            {
                                                new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in salvataggio pagamento", "", string.Empty, string.Empty);
                                            }
                                        }
                                    }
                                }
                            }
                            Log.Debug("finito");
                        }
                        else
                        {
                            new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "soggetto non trovato", "", string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        new General().LogActionEvent(DateTime.Now, CFPIVA, MySession.Current.Scope, "ReadDatiVerticale", IdEnte + "-" + CFPIVA, "", "errore in lettura dati da verticale:" + myResponse.Stato, "", string.Empty, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.ReadDatiVerticale::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ToCast"></param>
        /// <param name="myListVie"></param>
        /// <returns></returns>
        private AnagInterface.DettaglioAnagrafica CastToAnag(int IdContribuente,ImportInterface.Anagrafica ToCast, List<Utility.DichManagerSTRADE.Stradario> myListVie)
        {
            AnagInterface.DettaglioAnagrafica myItem = new AnagInterface.DettaglioAnagrafica();
            try
            {
                myItem.CodEnte = ToCast.CodEnte;
                myItem.COD_CONTRIBUENTE = IdContribuente;
                myItem.CodIndividuale = ToCast.CodIndividuale;
                myItem.CodiceFiscale = ToCast.CodiceFiscale;
                myItem.PartitaIva = ToCast.PartitaIva;
                myItem.Cognome = ToCast.Cognome;
                myItem.Nome = ToCast.Nome;
                myItem.Sesso = ToCast.Sesso;
                myItem.DataNascita = ToCast.DataNascita;
                myItem.DataMorte = ToCast.DataMorte;
                myItem.ComuneNascita = ToCast.ComuneNascita;
                myItem.ProvinciaNascita = ToCast.ProvinciaNascita;
                string myDescrVia = ToCast.ViaResidenza;
                myItem.CodViaResidenza = GetIdVia(ToCast.CodViaResidenza, myListVie, ref myDescrVia).ToString();
                myItem.ViaResidenza = myDescrVia;
                myItem.FrazioneResidenza = ToCast.FrazioneResidenza;
                myItem.CivicoResidenza = ToCast.CivicoResidenza;
                myItem.EsponenteCivicoResidenza = ToCast.EsponenteCivicoResidenza;
                myItem.InternoCivicoResidenza = ToCast.InternoCivicoResidenza;
                myItem.CapResidenza = ToCast.CapResidenza;
                myItem.ComuneResidenza = ToCast.ComuneResidenza;
                myItem.ProvinciaResidenza = ToCast.ProvinciaResidenza;
                if (ToCast.listSpedizioni != null)
                {
                    foreach (IndirizziSpedizione Invio in ToCast.listSpedizioni)
                    {
                        List<AnagInterface.ObjIndirizziSpedizione> list = new List<AnagInterface.ObjIndirizziSpedizione>();
                        AnagInterface.ObjIndirizziSpedizione sped = new AnagInterface.ObjIndirizziSpedizione();
                        sped.CognomeInvio = Invio.CognomeInvio;
                        sped.ViaRCP = Invio.ViaRCP;
                        sped.CivicoRCP = Invio.CivicoRCP;
                        sped.CapRCP = Invio.CapRCP;
                        sped.ComuneRCP = Invio.ComuneRCP;
                        sped.ProvinciaRCP = Invio.ProvinciaRCP;
                        if (sped.CognomeInvio != string.Empty)
                        {
                            list.Add(sped);
                            myItem.ListSpedizioni = list;
                        }
                    }
                }
                /*if (ToCast.Contatti != null)
                {
                    foreach (Contatti Contatto in ToCast.Contatti)
                    {
                        if (Contatto.Descrizione != string.Empty)
                        {
                            Anagrafica.DLL.dsContatti mio = new Anagrafica.DLL.dsContatti();
                            myItem.dsContatti = new Anagrafica.DLL.GestioneAnagrafica().SetContatti((Anagrafica.DLL.dsContatti)mio, (Contatto.IdTipo != "" ? int.Parse(Contatto.IdTipo) : 0), Contatto.Descrizione, string.Empty, -1, string.Empty);
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToAnag::errore::", ex);
            }
            return myItem;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListCast"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="myListVie"></param>
        /// <returns></returns>
        private List<Utility.DichManagerICI.TestataRow> CastToDich8852(List<Dich8852> ListCast, int IdContribuente, List<Utility.DichManagerSTRADE.Stradario> myListVie, List<GenericCategory> myListPossesso, List<GenericCategory> myListUtilizzo)
        {
            List<Utility.DichManagerICI.TestataRow> ListTestata = new List<Utility.DichManagerICI.TestataRow>();

            try
            {
                foreach (Dich8852 ToCast in ListCast)
                {
                    Utility.DichManagerICI.TestataRow myTestata = new Utility.DichManagerICI.TestataRow();

                    //myTestata
                    myTestata.ID = -1;

                    myTestata.DataFine = DateTime.MinValue;
                    myTestata.DataInizio = DateTime.MinValue;
                    myTestata.Bonificato = false;
                    myTestata.Annullato = false;
                    myTestata.DataInizioValidità = DateTime.Now;
                    myTestata.DataFineValidità = DateTime.MaxValue;
                    myTestata.Operatore = "import";
                    myTestata.IDContribuente = -1;
                    myTestata.IDDenunciante = -1;
                    myTestata.IDProvenienza = 8;

                    myTestata.Ente = ToCast.Ente;

                    myTestata.NumeroProtocollo = ToCast.NumeroProtocollo;
                    myTestata.DataProtocollo = ToCast.DataProtocollo;
                    myTestata.AnnoDichiarazione = myTestata.DataProtocollo.Year.ToString();
                    List<Utility.DichManagerICI.OggettiRow> myList = new List<Utility.DichManagerICI.OggettiRow>();
                    foreach (Oggetti8852 ToCastUI in ToCast.Immobili)
                    {
                        Utility.DichManagerICI.OggettiRow myOggetti = new Utility.DichManagerICI.OggettiRow();
                        Utility.DichManagerICI.DettaglioTestataRow myDettaglio = new Utility.DichManagerICI.DettaglioTestataRow();

                        //myOggetto
                        myOggetti.Ente = ToCast.Ente;
                        myOggetti.Comune = string.Empty;
                        myOggetti.IdTestata = -1;
                        myOggetti.NumeroOrdine = "";
                        myOggetti.NumeroModello = "";
                        myOggetti.TipoImmobile = 0;
                        myOggetti.PartitaCatastale = 0;
                        myOggetti.Sezione = "";
                        myOggetti.NumeroProtCatastale = "";
                        myOggetti.AnnoDenunciaCatastale = "";
                        myOggetti.Storico = false;
                        myOggetti.IDValuta = 1;
                        myOggetti.FlagValoreProvv = false;
                        myOggetti.CodComune = 0;
                        myOggetti.EspCivico = "";
                        myOggetti.Barrato = "";
                        myOggetti.NumeroEcografico = 0;
                        myOggetti.TitoloAcquisto = 0;
                        myOggetti.TitoloCessione = 0;
                        myOggetti.DescrUffRegistro = "";
                        myOggetti.DataInizioValidità = DateTime.Now;
                        myOggetti.DataFineValidità = DateTime.MaxValue;
                        myOggetti.Bonificato = false;
                        myOggetti.Annullato = false;
                        myOggetti.DataUltimaModifica = DateTime.Now;
                        myOggetti.Operatore = myTestata.Operatore;

                        //myDettaglio
                        myDettaglio.Ente = ToCast.Ente;
                        myDettaglio.NumeroOrdine = "";
                        myDettaglio.NumeroModello = "";
                        myDettaglio.MesiEsclusioneEsenzione = 0;
                        myDettaglio.MesiRiduzione = 0;
                        myDettaglio.ImpDetrazAbitazPrincipale = 0;
                        myDettaglio.Contitolare = false;
                        myDettaglio.Bonificato = false;
                        myDettaglio.Annullato = false;
                        myDettaglio.DataInizioValidità = DateTime.Now;
                        myDettaglio.DataFineValidità = DateTime.MinValue;
                        myDettaglio.Operatore = myTestata.Operatore;

                        myOggetti.CodUI = ToCastUI.CodUI;
                        myDettaglio.IdSoggetto = IdContribuente;
                        string myDescrVia = ToCastUI.Via;
                        myOggetti.CodVia = GetIdVia(ToCastUI.CodVia, myListVie, ref myDescrVia).ToString();
                        myOggetti.Via = myDescrVia;
                        myOggetti.NumeroCivico = ToCastUI.NumeroCivico;
                        myOggetti.Scala = ToCastUI.Scala;
                        myOggetti.Piano = ToCastUI.Piano;
                        myOggetti.Interno = ToCastUI.Interno;
                        myOggetti.Foglio = ToCastUI.Foglio;
                        myOggetti.Numero = ToCastUI.Numero;
                        myOggetti.Subalterno = ToCastUI.Subalterno;
                        myOggetti.DataInizio = ToCastUI.DataInizio;
                        myOggetti.DataFine = ToCastUI.DataFine;
                        myOggetti.Caratteristica = ToCastUI.Caratteristica;
                        if (ToCastUI.CodRendita.Trim().Length == 0)
                            ToCastUI.CodRendita = "RE";
                        myOggetti.CodRendita = new Utility.DichManagerICI(RouteConfig.TypeDB, RouteConfig.StringConnectionICI).GetCodRendita(ToCastUI.CodRendita);
                        myOggetti.Zona = ToCastUI.Zona;
                        myOggetti.CodCategoriaCatastale = ToCastUI.CodCategoriaCatastale.Replace("A0", "A/").Replace("B0", "B/").Replace("C0", "C/").Replace("D0", "D/").Replace("E0", "E/").Replace("F0", "F/");
                        myOggetti.CodClasse = ToCastUI.CodClasse;
                        myOggetti.Rendita = ToCastUI.Rendita;
                        myOggetti.ValoreImmobile = ToCastUI.ValoreImmobile;
                        myOggetti.Consistenza = ToCastUI.Consistenza;
                        myOggetti.NoteIci = ToCastUI.NoteIci;
                        myOggetti.IDImmobilePertinente = ToCastUI.IdImmobilePertinente;
                        myDettaglio.MesiPossesso = ToCastUI.Dettaglio.MesiPossesso;
                        myDettaglio.TipoPossesso = GetSettingId(ToCastUI.Dettaglio.TipoPossesso, myListPossesso);
                        myDettaglio.TipoUtilizzo = GetSettingId(ToCastUI.Dettaglio.TipoUtilizzo, myListUtilizzo);
                        myDettaglio.PercPossesso = ToCastUI.Dettaglio.PercPossesso;
                        myDettaglio.AbitazionePrincipaleAttuale = ToCastUI.Dettaglio.AbitazionePrincipaleAttuale;
                        myDettaglio.EsclusioneEsenzione = ToCastUI.Dettaglio.EsclusioneEsenzione; //verificare se gestito al contrario
                        myDettaglio.MesiEsclusioneEsenzione = ToCastUI.Dettaglio.MesiEsclusioneEsenzione;
                        myDettaglio.Riduzione = ToCastUI.Dettaglio.Riduzione;//verificare se gestito al contrario
                        myDettaglio.MesiRiduzione = ToCastUI.Dettaglio.MesiRiduzione;
                        myDettaglio.NumeroFigli = ToCastUI.Dettaglio.NumeroFigli;
                        myDettaglio.NumeroUtilizzatori = ToCastUI.Dettaglio.NumeroUtilizzatori;

                        myOggetti.oDettaglio = myDettaglio;
                        myList.Add(myOggetti);
                        myTestata.listOggetti = ((Utility.DichManagerICI.OggettiRow[])myList.ToArray());
                    }
                    ListTestata.Add(myTestata);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToDich8852::errore::", ex);
            }
            return ListTestata;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListCast"></param>
        /// <param name="IdContribuente"></param>
        /// <returns></returns>
        private List<Utility.DichManagerICI.VersamentiRow> CastToPag8852(List<Pag8852> ListCast, int IdContribuente)
        {
            List<Utility.DichManagerICI.VersamentiRow> ListPag = new List<Utility.DichManagerICI.VersamentiRow>();

            try
            {
                foreach (Pag8852 ToCast in ListCast)
                {
                    Utility.DichManagerICI.VersamentiRow myItem = new Utility.DichManagerICI.VersamentiRow();

                    myItem.ID = -1;
                    myItem.Ente = string.Empty;
                    myItem.IdAnagrafico = -1;
                    myItem.CodTributo = string.Empty;
                    myItem.AnnoRiferimento = string.Empty;
                    myItem.CodiceFiscale = string.Empty;
                    myItem.PartitaIva = string.Empty;
                    myItem.ImportoPagato = 0;
                    myItem.DataPagamento = DateTime.MaxValue;
                    myItem.NumeroBollettino = string.Empty;
                    myItem.NumeroFabbricatiPosseduti = -1;
                    myItem.Acconto = false;
                    myItem.Saldo = false;
                    myItem.RavvedimentoOperoso = false;
                    myItem.ImportoTerreni = 0;
                    myItem.ImportoAreeFabbric = 0;
                    myItem.ImportoAbitazPrincipale = 0;
                    myItem.ImportoAltrifabbric = 0;
                    myItem.DetrazioneAbitazPrincipale = 0;
                    myItem.ImportoTerreniStatale = 0;
                    myItem.ImportoAreeFabbricStatale = 0;
                    myItem.ImportoAltrifabbricStatale = 0;
                    myItem.ImportoFabRurUsoStrum = 0;
                    myItem.ImportoFabRurUsoStrumStatale = 0;
                    myItem.ImportoUsoProdCatD = 0;
                    myItem.ImportoUsoProdCatDStatale = 0;
                    myItem.ContoCorrente = string.Empty;
                    myItem.ComuneUbicazioneImmobile = string.Empty;
                    myItem.ComuneIntestatario = string.Empty;
                    myItem.Bonificato = true;
                    myItem.DataInizioValidità = DateTime.MaxValue;
                    myItem.DataFineValidità = DateTime.MaxValue;
                    myItem.Operatore = string.Empty;
                    myItem.Annullato = false;
                    myItem.ImportoSoprattassa = 0;
                    myItem.ImportoPenaPecuniaria = 0;
                    myItem.Interessi = 0;
                    myItem.Violazione = false;
                    myItem.IDProvenienza = -1;
                    myItem.Provenienza = string.Empty;
                    myItem.NumeroAttoAccertamento = string.Empty;
                    myItem.DataProvvedimentoViolazione = DateTime.MaxValue;
                    myItem.ImportoPagatoArrotondamento = 0;
                    myItem.DataRiversamento = DateTime.MaxValue;
                    myItem.FlagFabbricatiExRurali = false;
                    myItem.NumeroProvvedimentoViolazione = string.Empty;
                    myItem.ImportoImposta = 0;
                    myItem.Note = string.Empty;
                    myItem.DetrazioneStatale = 0;
                    myItem.Ente = ToCast.Ente;
                    myItem.ID = ToCast.ID;
                    myItem.Provenienza = ToCast.Provenienza;
                    myItem.AnnoRiferimento = ToCast.AnnoRiferimento;
                    myItem.IdAnagrafico = IdContribuente;
                    myItem.ImportoPagato = ToCast.ImportoPagato;
                    myItem.DataRiversamento = ToCast.DataRiversamento;
                    myItem.DataPagamento = ToCast.DataPagamento;
                    myItem.NumeroFabbricatiPosseduti = ToCast.NumeroFabbricatiPosseduti;
                    myItem.Acconto = ToCast.Acconto;
                    myItem.Saldo = ToCast.Saldo;
                    myItem.RavvedimentoOperoso = ToCast.RavvedimentoOperoso;
                    myItem.CodTributo = ToCast.CodTributo;
                    myItem.ImportoImposta = ToCast.ImportoImposta;
                    ListPag.Add(myItem);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToPag8852::errore::", ex);
            }
            return ListPag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListCast"></param>
        /// <param name="IdContribuente"></param>
        /// <param name="myListVie"></param>
        /// <returns></returns>
        private List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata> CastToDich0434(List<Dich0434> ListCast, int IdContribuente, List<Utility.DichManagerSTRADE.Stradario> myListVie)
        {
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata> ListPag = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata>();

            try
            {
                foreach (Dich0434 ToCast in ListCast)
                {
                    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata myTestata = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjTestata();
                    List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata> ListDettaglioTestata = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata>();
                    List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti> ListOggetti = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti>();

                    myTestata.sEnte = ToCast.sEnte;
                    myTestata.tDataInserimento = DateTime.Now;
                    myTestata.sOperatore = "IMPORT";

                    myTestata.sNDichiarazione = ToCast.sNDichiarazione;
                    myTestata.tDataDichiarazione = ToCast.tDataDichiarazione;

                    myTestata.IdContribuente = IdContribuente;
                    foreach (Dettaglio0434 ToCastUI in ToCast.Immobili)
                    {
                        Log.Debug("CastToDich0434.ToCast.Immobili.Count->" + ToCast.Immobili.Count.ToString());
                        RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata myDettaglioTestata = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata();
                        myDettaglioTestata.tDataInserimento = DateTime.Now;
                        myDettaglioTestata.sOperatore = "IMPORT";
                        myDettaglioTestata.nVani = 1; myDettaglioTestata.nMQ = 1;

                        myDettaglioTestata.CodImmobile = ToCastUI.CodImmobile;
                        myDettaglioTestata.ProgImmobile = ToCastUI.ProgImmobile;
                        myDettaglioTestata.Key = myTestata.sNDichiarazione + "|" + myDettaglioTestata.CodImmobile + "|" + myDettaglioTestata.ProgImmobile;
                        Log.Debug("cerco via->" + ToCastUI.sCodVia);
                        string myDescrVia = ToCastUI.sVia;
                        myDettaglioTestata.sCodVia = GetIdVia(ToCastUI.sCodVia, myListVie, ref myDescrVia).ToString();
                        myDettaglioTestata.sVia = myDescrVia;
                        int Civico = 0;
                        int.TryParse(ToCastUI.sCivico, out Civico);
                        myDettaglioTestata.sCivico = Civico.ToString();
                        myDettaglioTestata.sScala = ToCastUI.sScala;
                        myDettaglioTestata.sInterno = ToCastUI.sInterno;
                        myDettaglioTestata.sFoglio = ToCastUI.sFoglio;
                        myDettaglioTestata.sNumero = ToCastUI.sNumero;
                        myDettaglioTestata.sSubalterno = ToCastUI.sSubalterno;
                        myDettaglioTestata.tDataInizio = ToCastUI.tDataInizio;
                        myDettaglioTestata.tDataFine = ToCastUI.tDataFine;
                        myDettaglioTestata.nMQCatasto = ToCastUI.nMQCatasto;
                        myDettaglioTestata.sNoteUI = ToCastUI.sNoteUI;
                        foreach (Oggetto0434 ToCastVano in ToCastUI.Oggetti)
                        {
                            RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti myOggetti = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti();
                            myOggetti.tDataInserimento = DateTime.Now;
                            myOggetti.sOperatore = "IMPORT";

                            myOggetti.IdTipoVano = ToCastVano.IdTipoVano;
                            myOggetti.nMq = ToCastVano.nMq;
                            myOggetti.IdCategoria = ToCastVano.IdCategoria;
                            myOggetti.IdCatTARES = new Utility.DichManagerTARSU(RouteConfig.TypeDB, RouteConfig.StringConnectionTARSU, "", MySession.Current.Ente.IDEnte).GetIdCat(ToCastVano.IdCatTARES, ToCast.sEnte);
                            Log.Debug("prelevato catTARES per ToCast.sEnte=" + ToCast.sEnte + ", ToCastVano.IdCatTARES=" + ToCastVano.IdCatTARES + ", IdCatTARES=" + myOggetti.IdCatTARES.ToString());
                            myOggetti.nNC = ToCastVano.nNC;
                            myOggetti.nNCPV = ToCastVano.nNCPV;
                            myOggetti.bIsEsente = ToCastVano.bIsEsente;

                            ListOggetti.Add(myOggetti);
                            myDettaglioTestata.oOggetti = ((RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjOggetti[])(ListOggetti.ToArray())); ;
                        }
                        ListDettaglioTestata.Add(myDettaglioTestata);
                        myTestata.oImmobili = ((RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDettaglioTestata[])(ListDettaglioTestata.ToArray()));
                    }
                    ListPag.Add(myTestata);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToDich0434::errore::", ex);
            }
            return ListPag;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListCast"></param>
        /// <param name="IdContribuente"></param>
        /// <returns></returns>
        private List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso> CastToAvvisi0434(List<Avviso0434> ListCast, int IdContribuente)
        {
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso> ListAvvisi = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso>();

            try
            {
                foreach (Avviso0434 ToCast in ListCast)
                {
                    RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso myItem = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjAvviso();
                    List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci> ListDetVoci = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci>();

                    myItem.IdEnte = ToCast.IdEnte;
                    myItem.tDataInserimento = DateTime.Now;
                    myItem.IdContribuente = IdContribuente;
                    myItem.sAnnoRiferimento = ToCast.sAnnoRiferimento;
                    myItem.sCodiceCartella = ToCast.sCodiceCartella;
                    myItem.tDataEmissione = ToCast.tDataEmissione;
                    myItem.impCarico = ToCast.impCarico;
                    myItem.impDovuto = ToCast.impDovuto;
                    foreach (DetVociAvviso0434 ToCastDet in ToCast.DetVoci)
                    {
                        RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci myVoce = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjDetVoci();

                        myVoce.sCapitolo = ToCastDet.sCapitolo;
                        myVoce.impDettaglio = ToCastDet.impDettaglio;
                        ListDetVoci.Add(myVoce);
                    }
                    myItem.oDetVoci = ListDetVoci.ToArray();
                    ListAvvisi.Add(myItem);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToAvvisi0434::errore::", ex);
            }
            return ListAvvisi;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListCast"></param>
        /// <param name="IdContribuente"></param>
        /// <returns></returns>
        private List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata> CastToRate0434(string IdEnte,List<Rate0434> ListCast, int IdContribuente)
        {
            List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata> ListRate = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata>();

            try
            {
                foreach (Rate0434 ToCast in ListCast)
                {
                    if (Utility.StringOperation.FormatString(ToCast.sCodiceCartella).Trim() != string.Empty)
                    {
                        List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRataDettaglio> ListDet = new List<RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRataDettaglio>();
                        RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRataDettaglio myDet = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRataDettaglio();
                        RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata myRata = new RemotingInterfaceMotoreTarsu.MotoreTarsuVARIABILE.Oggetti.ObjRata();

                        myRata.IdEnte = IdEnte;
                        myRata.IdContribuente = IdContribuente;
                        myRata.sCodiceCartella = ToCast.sCodiceCartella;
                        myRata.tDataEmissione = ToCast.tDataEmissione;
                        myRata.sNRata = ToCast.sNRata;
                        myRata.tDataScadenza = ToCast.tDataScadenza;
                        myRata.IdAvviso = GetIdAvviso(myRata.IdEnte, myRata.IdContribuente, myRata.sCodiceCartella, myRata.tDataEmissione);
                        foreach (DetRata0434 ToCastDet in ToCast.DetRata)
                        {
                            myDet.IdAvviso = myRata.IdAvviso;
                            myDet.NRata = myRata.sNRata;
                            myDet.Tributo = ToCastDet.Tributo;
                            myDet.Importo = ToCastDet.Importo;
                            ListDet.Add(myDet);
                        }
                        myRata.oDetVoci = ListDet.ToArray();
                        ListRate.Add(myRata);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToRate0434::errore::", ex);
            }
            return ListRate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListCast"></param>
        /// <param name="IdContribuente"></param>
        /// <returns></returns>
        private List<RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti> CastToPag0434(List<Pag0434> ListCast, int IdContribuente)
        {
            List<RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti> ListRate = new List<RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti>();

            try
            {
                foreach (Pag0434 ToCast in ListCast)
                {
                    RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti anag = new RemotingInterfaceMotoreTarsu.MotoreTarsu.Oggetti.OggettoPagamenti();

                    anag.IdEnte = ToCast.IdEnte;
                    anag.IdContribuente = IdContribuente;
                    anag.sNumeroAvviso = ToCast.sNumeroAvviso;
                    anag.sAnno = ToCast.sAnno;
                    anag.sProvenienza = ToCast.sProvenienza;
                    anag.tDataAccredito = ToCast.tDataAccredito;
                    anag.tDataPagamento = ToCast.tDataPagamento;
                    anag.sSegno = ToCast.sSegno;
                    anag.dImportoPagamento = ToCast.dImportoPagamento;

                    ListRate.Add(anag);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.CastToPag0434::errore::", ex);
            }
            return ListRate;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ViaDemografico"></param>
        /// <param name="ListVie"></param>
        /// <param name="sVia"></param>
        /// <returns></returns>
        private int GetIdVia(string ViaDemografico, List<Utility.DichManagerSTRADE.Stradario> ListVie, ref string sVia)
        {
            int IdVia = -1;
            try
            {
                foreach (Utility.DichManagerSTRADE.Stradario myItem in ListVie)
                {
                    if (ViaDemografico == myItem.IdDemografico.ToString())
                    {
                        IdVia = myItem.IDVia;
                        if (sVia == string.Empty)
                            sVia = myItem.Descrizione;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("GetIdVia.errore::", ex);
                IdVia = -1;
            }
            return IdVia;
        }
        private int GetSettingId(string TipoSetting, List<GenericCategory> ListSettings)
        {
            int myId = -1;
            try
            {
                foreach (GenericCategory myItem in ListSettings)
                {
                    if (TipoSetting == myItem.IDOrg.ToString())
                    {
                        int.TryParse(myItem.Codice, out myId);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("GetSettingId.errore::", ex);
                myId = -1;
            }
            return myId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDEnte"></param>
        /// <param name="IDContribuente"></param>
        /// <param name="CodCartella"></param>
        /// <param name="DataEmissione"></param>
        /// <returns></returns>
        public int GetIdAvviso(string IDEnte, int IDContribuente, string CodCartella, DateTime DataEmissione)
        {
            int IdAvviso = 0;
            try
            {
                if (DataEmissione == DateTime.MinValue)
                    DataEmissione = DateTime.MaxValue;
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_GetIdAvviso", "IDENTE", "IDCONTRIBUENTE", "CODCARTELLA", "DATAEMISIONE");
                    IdAvviso = ctx.ContextDB.Database.SqlQuery<int>(sSQL, ctx.GetParam("IDENTE", IDEnte)
                            , ctx.GetParam("IDCONTRIBUENTE", IDContribuente)
                            , ctx.GetParam("CODCARTELLA", CodCartella)
                            , ctx.GetParam("DATAEMISIONE", DataEmissione)
                        ).FirstOrDefault<int>();
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.GetIdAvviso.errore.parametryquery->IDENTE="+IDEnte+",IDCONTRIBUENTE="+ IDContribuente.ToString()+",CODCARTELLA="+ CodCartella+",DATAEMISIONE="+ DataEmissione.ToString());
                Log.Debug("OPENgovSPORTELLO.BLLImport.GetIdAvviso::errore::", ex);
            }
            return IdAvviso;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="reason"></param>
        /// <param name="IdEnte"></param>
        /// <param name="Fornitore"></param>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public string GetTokenGWImport(string user, string reason, string IdEnte, string Fornitore, string PathFile)
        {
            string myToken = string.Empty;
            try
            {
                Fornitore = Fornitore.PadRight(20, ' ');
                myToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(DateTime.UtcNow + "|" + user + "|" + IdEnte + "|" + reason + "|" + (Fornitore != string.Empty ? Fornitore : string.Empty) + "|" + PathFile));
            }
            catch (Exception ex)
            {
                string myErr = ex.Message;
            }
            return myToken;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="CodSoggetto"></param>
        public void SetDatiVerticaleRead(string IdEnte, string CodSoggetto)
        {
            Thread t = new Thread(() => SetDatiVerticaleReadThreadEntryPoint(IdEnte, CodSoggetto));
            t.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="CodSoggetto"></param>
        private void SetDatiVerticaleReadThreadEntryPoint(string IdEnte, string CodSoggetto)
        {
            try
            {
                string PathFile = MySettings.GetConfig("PathImportDati") + IdEnte + "\\";
                //ANAGRAFICA
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FileAnagrafica);
                    var writer = new StreamWriter(PathFile + TypeFile.FileAnagrafica);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 6)
                        {
                            if (NewLine.Substring(0, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FileAnagrafica, ex);
                }
                //DEMOGRAFICO
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FileDemografico);
                    var writer = new StreamWriter(PathFile + TypeFile.FileDemografico);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 7)
                        {
                            if (NewLine.Substring(0, 7).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FileDemografico, ex);
                }
                //DICH ICI
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FileDich8852);
                    var writer = new StreamWriter(PathFile + TypeFile.FileDich8852);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 74)
                        {
                            if (NewLine.Substring(68, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FileDich8852, ex);
                }
                //PAG ICI
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FilePag8852);
                    var writer = new StreamWriter(PathFile + TypeFile.FilePag8852);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 19)
                        {
                            if (NewLine.Substring(13, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FilePag8852, ex);
                }
                //DICH TARSU
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FileDich0434);
                    var writer = new StreamWriter(PathFile + TypeFile.FileDich0434);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 59)
                        {
                            if (NewLine.Substring(53, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FileDich0434, ex);
                }
                //AVVISI TARSU
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FileAvvisi0434);
                    var writer = new StreamWriter(PathFile + TypeFile.FileAvvisi0434);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 6)
                        {
                            if (NewLine.Substring(0, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FileAvvisi0434, ex);
                }
                //RATE TARSU
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FileRate0434);
                    var writer = new StreamWriter(PathFile + TypeFile.FileRate0434);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 6)
                        {
                            if (NewLine.Substring(0, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FileRate0434, ex);
                }
                //PAG TARSU
                try
                {
                    string[] arr = File.ReadAllLines(PathFile + TypeFile.FilePag0434);
                    var writer = new StreamWriter(PathFile + TypeFile.FilePag0434);
                    for (int i = 0; i < arr.Length; i++)
                    {
                        string NewLine = arr[i];
                        if (NewLine.Length > 6)
                        {
                            if (NewLine.Substring(0, 6).Trim() == CodSoggetto)
                                NewLine += "R";
                        }
                        writer.WriteLine(NewLine);
                    }
                    writer.Flush();
                    writer.Close();
                }
                catch (Exception ex)
                {
                    Log.Debug("errore in agg " + PathFile + TypeFile.FilePag0434, ex);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.SetDatiVerticaleReadThreadEntryPoint::errore::", ex);
            }
        }
        #endregion
        #region Upload flussi carico
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myEnte"></param>
        /// <param name="zipName"></param>
        /// <param name="Esito"></param>
        public void UploadFlussiCarico(EntiInLavorazione myEnte, string zipName, out string Esito)
        {
            try
            {
                string requestUriParam = string.Empty;
                string sErr = string.Empty;
                TributiModel myResponse = new TributiModel();

                if (myEnte.IDEnte != string.Empty)
                {
                    //unzip file
                    ICSharpCode.SharpZipLib.Zip.FastZip fastZip = new ICSharpCode.SharpZipLib.Zip.FastZip();
                    string fileFilter = null;
                    if (!Directory.Exists(UrlHelper.GetPathFlussiCarico + myEnte.IDEnte))
                    {
                        Directory.CreateDirectory(UrlHelper.GetPathFlussiCarico + myEnte.IDEnte);
                    }
                    // Will always overwrite if target filenames already exist
                    fastZip.ExtractZip(UrlHelper.GetPathFlussiCarico + "\\" + zipName, UrlHelper.GetPathFlussiCarico + myEnte.IDEnte + "\\", fileFilter);

                    new BLL.RestService().MakeRequestForDatiVerticale<TributiModel>(MySettings.GetConfig("UrlConvertFlussi")
                        , requestUriParam
                        , result => myResponse = result
                        , error => sErr = error.Message
                        , "Token: " + GetTokenGWImport(new BLL.RestService().UserConvert, new BLL.RestService().ReasonConvert, myEnte.IDEnte, myEnte.DatiVerticali.TipoFornitore, UrlHelper.GetPathFlussiCarico /*+ myEnte.IDEnte*/ + "\\")
                        );
                    Log.Debug("restituito oggetto");
                    if (sErr != string.Empty)
                    {
                        Log.Debug("OPENgovSPORTELLO.BLLImport.UploadFlussiCarico::errore::" + sErr);
                        Esito = sErr;
                    }
                    else
                    {
                        if (myResponse.Stato == "200 OK")
                        {
                            //se l'upload è andato a buon fine carico i tempi di pagamento
                            ReadTempiPagamento(myEnte.IDEnte, General.TRIBUTO.ICI);
                            ReadTempiPagamento(myEnte.IDEnte, General.TRIBUTO.TARSU);
                            Esito = "Carico effettuato con successo";
                        }
                        else
                        {
                            Esito = myResponse.Stato;
                            new General().LogActionEvent(DateTime.Now, string.Empty, MySession.Current.Scope, "UploadFlussiCarico", myEnte.IDEnte, "", "errore in upload dati da verticale:" + myResponse.Stato, "", string.Empty, string.Empty);
                        }
                    }
                }
                else
                {
                    Esito = "Ente non valido";
                    new General().LogActionEvent(DateTime.Now, string.Empty, MySession.Current.Scope, "UploadFlussiCarico", myEnte.IDEnte, "", "ente non valido upload dati da verticale", "", string.Empty, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Esito = "Errore in upload file";
                Log.Debug("OPENgovSPORTELLO.BLLImport.UploadFlussiCarico::errore::", ex);
            }
        }
        #endregion
        #region Tempi Pagamento
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IdEnte"></param>
        /// <param name="IdTributo"></param>
        public void ReadTempiPagamento(string IdEnte, string IdTributo)
        {
            try
            {
                //come parametro solo idtriuto perchè l'idente lo passo da Token
                string requestUriParam = String.Format("?IDTRIBUTO={0}", (IdTributo));
                string sErr = string.Empty;
                TempiMediModel myResponse = new TempiMediModel();
                Log.Debug("Richiamo importTempiMedi Url->" + MySettings.GetConfig("UrlImportTempiPagamento"));
                //Restituisce dentro myResponse il JSON già convertito
                new BLL.RestService().MakeRequestForTempiMedi<TempiMediModel>(MySettings.GetConfig("UrlImportTempiPagamento")
                    , requestUriParam
                    , result => myResponse = result
                    , error => sErr = error.Message
                    , "Token: " + GetTokenGWImport(new BLL.RestService().UserAvgTimes, new BLL.RestService().ReasonAvgTimes, IdEnte, string.Empty, MySettings.GetConfig("PathImportDati") + IdEnte + "\\")
                    );
                Log.Debug("restituito oggetto");
                if (sErr != string.Empty)
                {
                    Log.Debug("OPENgovSPORTELLO.BLLImport.ReadTempiPagamento::errore::" + sErr);
                }
                else
                {
                    //controllo la response
                    if (myResponse.Stato == "200 OK")
                    {
                        if (myResponse.TempiMediPagamento != null)
                        {
                            Log.Debug("lavoro TempiMediPagamento");
                            //svuoto tabella tbltempipagamento
                            new BLL.Analisi().ClearTBLTempiPagamento(IdEnte);

                            //ciclo su tutti i record
                            foreach (TempiMediPagamento TempiMediPagamentoTmp in myResponse.TempiMediPagamento)
                            {
                                //eseguo l'insert per ogni riga, se fallisce riporto il log 
                                if (new BLL.Analisi().InsertTempiMedi(TempiMediPagamentoTmp.IdEnte, TempiMediPagamentoTmp.NumeroRata, TempiMediPagamentoTmp.DataEmissione, TempiMediPagamentoTmp.DataScadenza, TempiMediPagamentoTmp.TipoPeriodo, TempiMediPagamentoTmp.ContatoreAvvisi, TempiMediPagamentoTmp.SommatoriaPagato, TempiMediPagamentoTmp.CodTributo) != true)
                                {
                                    new General().LogActionEvent(DateTime.Now, "", MySession.Current.Scope, "ReadTempiPagamento", IdEnte + "-", "", "errore in inserimento TempiPagamento", "", string.Empty, string.Empty);
                                }
                            }
                            Log.Debug("finito");
                        }
                        else
                        {
                            new General().LogActionEvent(DateTime.Now, "", MySession.Current.Scope, "TempiMediPagamento", IdEnte + "-", "", "oggetto non trovato", "", string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        new General().LogActionEvent(DateTime.Now, "", MySession.Current.Scope, "TempiMediPagamento", IdEnte + "-", "", "errore in lettura dati:" + myResponse.Stato, "", string.Empty, string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BLLImport.ReadTempiPagamento::errore::", ex);
            }
        }
        #endregion
    }
}