using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;
using System.Web.UI.WebControls;
using AnagInterface;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Classe di gestioni generale
    /// </summary>
    public class General
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(General));
        /// <summary>
        /// 
        /// </summary>
        public static class TRIBUTO
        {
            public static string TARSU = "0434";
            public static string ICI = "8852";
            public static string TASI = "TASI";
            public static string OSAP = "0453";
            public static string ICP = "9763";
            public static string TESSERE = "TESS";
            public static string PROVVEDIMENTI = "9999";
        }
        /// <summary>
        /// 
        /// </summary>
        public void ClearSession()
        {
            MySession.Current.IDDichiarazioneIstanze = -1;
            MySession.Current.HasNewDich = 0;
            MySession.Current.IdIstanza = -1;
            MySession.Current.IdMessage = -1;
            MySession.Current.IsBackToTributi = false;
            MySession.Current.TotShoppingCart = 0;
            MySession.Current.ListShoppingCart = new List<string>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tDataGrd"></param>
        /// <returns></returns>
        public string GetGGFromData(object tDataGrd)
        {
            string myRet = "";

            if (tDataGrd != null)
            {
                DateTime myDate = DateTime.MaxValue;
                DateTime.TryParse(tDataGrd.ToString(), out myDate);
                if (myDate == DateTime.MinValue || myDate == DateTime.MaxValue || myDate.ToString() == DateTime.MaxValue.ToShortDateString() || myDate.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                    myRet = "";
                else
                    myRet = myDate.Day.ToString().PadLeft(2, char.Parse("0"));
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tDataGrd"></param>
        /// <returns></returns>
        public string GetMMFromData(object tDataGrd)
        {
            string myRet = "";

            if (tDataGrd != null)
            {
                DateTime myDate = DateTime.MaxValue;
                DateTime.TryParse(tDataGrd.ToString(), out myDate);
                if (myDate == DateTime.MinValue || myDate == DateTime.MaxValue || myDate.ToString() == DateTime.MaxValue.ToShortDateString() || myDate.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                    myRet = "";
                else
                    myRet = myDate.Month.ToString().PadLeft(2, char.Parse("0"));
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tDataGrd"></param>
        /// <returns></returns>
        public string GetAAFromData(object tDataGrd)
        {
            string myRet = "";

            if (tDataGrd != null)
            {
                DateTime myDate = DateTime.MaxValue;
                DateTime.TryParse(tDataGrd.ToString(), out myDate);
                if (myDate == DateTime.MinValue || myDate == DateTime.MaxValue || myDate.ToString() == DateTime.MaxValue.ToShortDateString() || myDate.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                    myRet = "";
                else
                    myRet = myDate.Year.ToString().PadLeft(4, char.Parse("0")).Substring(2);
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tDataGrd"></param>
        /// <returns></returns>
        public string GetAAAAFromData(object tDataGrd)
        {
            string myRet = "";

            if (tDataGrd != null)
            {
                DateTime myDate = DateTime.MaxValue;
                DateTime.TryParse(tDataGrd.ToString(), out myDate);
                if (myDate == DateTime.MinValue || myDate == DateTime.MaxValue || myDate.ToString() == DateTime.MaxValue.ToShortDateString() || myDate.ToShortDateString() == DateTime.MaxValue.ToShortDateString())
                    myRet = "";
                else
                    myRet = myDate.Year.ToString().PadLeft(4, char.Parse("0"));
            }
            return myRet;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="myDDL"></param>
        /// <param name="myList"></param>
        /// <param name="FieldValue"></param>
        /// <param name="FieldText"></param>
        public void LoadCombo(System.Web.UI.WebControls.DropDownList myDDL, object myList, string FieldValue, string FieldText)
        {
            try
            {
                myDDL.DataSource = myList;
                myDDL.DataValueField = FieldValue;
                myDDL.DataTextField = FieldText;
                myDDL.DataBind();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.General.LoadCombo::errore::", ex);
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="EMail"></param>
        /// <returns></returns>
        public bool ValidateMail(string EMail)
        {
            try
            {
                var x = EMail;
                var atpos = x.IndexOf("@");
                var dotpos = x.LastIndexOf(".");
                if (atpos < 1 || dotpos < atpos + 2 || dotpos + 2 >= x.Length)
                {
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(EMail, @"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+(?:[A-Z]{2}|com|org|net|edu|gov|mil|biz|info|mobi|name|aero|asia|jobs|museum)\b", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.General.ValidateMail::errore::", ex);
                return false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CodiceFiscale"></param>
        /// <returns></returns>
        public bool ValidateCodiceFiscale(string CodiceFiscale)
        {
            bool result = false;
            const int caratteri = 16;
            if (CodiceFiscale == null)
            {
                Log.Debug("ValidateCodiceFiscale::codice fiscale nullo");
                return result;
            }

            // se il codice fiscale non è di 16 caratteri il controllo
            // è già finito prima ancora di cominciare

            if (CodiceFiscale.Length != caratteri)
            {
                Log.Debug("ValidateCodiceFiscale::lunghezza errata->" + CodiceFiscale.Length.ToString());
                return result;
            }

            // stringa per controllo e calcolo omocodia 
            const string omocodici = "LMNPQRSTUV";
            // per il calcolo del check digit e la conversione in numero
            const string listaControllo = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            int[] listaPari = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };
            int[] listaDispari = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };

            CodiceFiscale = CodiceFiscale.ToUpper();
            char[] cCodice = CodiceFiscale.ToCharArray();

            // check della correttezza formale del codice fiscale
            // elimino dalla stringa gli eventuali caratteri utilizzati negli 
            // spazi riservati ai 7 che sono diventati carattere in caso di omocodia
            for (int k = 6; k < 15; k++)
            {
                if ((k == 8) || (k == 11))
                    continue;
                int x = (omocodici.IndexOf(cCodice[k]));
                if (x != -1)
                    cCodice[k] = x.ToString().ToCharArray()[0];
            }

            result = true;            

            // normalizzato il codice fiscale se la regular non ha buon
            // fine è inutile continuare
            if (result)
            {
                int somma = 0;
                // ripristino il codice fiscale originario 
                // grazie a Lino Barreca che mi ha segnalato l'errore
                cCodice = CodiceFiscale.ToCharArray();
                for (int i = 0; i < 15; i++)
                {
                    char c = cCodice[i];
                    int x = "0123456789".IndexOf(c);
                    if (x != -1)
                        c = listaControllo.Substring(x, 1).ToCharArray()[0];
                    x = listaControllo.IndexOf(c);
                    // i modulo 2 = 0 è dispari perchè iniziamo da 0
                    if ((i % 2) == 0)
                        x = listaDispari[x];
                    else
                        x = listaPari[x];
                    somma += x;
                }

                result = (listaControllo.Substring(somma % 26, 1) == CodiceFiscale.Substring(15, 1));
                if (!result)
                {
                    Log.Debug("ValidateCodiceFiscale::codice fiscale sbagliato->calcolato=" + listaControllo.Substring(somma % 26, 1) + " inserito=" + CodiceFiscale.Substring(15, 1));
                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PartitaIva"></param>
        /// <returns></returns>
        public bool ValidatePartitaIva(string PartitaIva)
        {
            int[] ListaPari = { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };
            // normalizziamo la cifra
            if (PartitaIva.Length < 11)
                PartitaIva = PartitaIva.PadLeft(11, '0');
            // lunghezza errata non fa neanche il controllo
            if (PartitaIva.Length != 11)
                return false;
            int Somma = 0;
            for (int k = 0; k < 11; k++)
            {
                string s = PartitaIva.Substring(k, 1);
                // otteniamo contemporaneamente
                // il valore, la posizione e testiamo se ci sono
                // caratteri non numerici
                int i = "0123456789".IndexOf(s);
                if (i == -1)
                    return false;
                int x = int.Parse(s);
                if (k % 2 == 1) // Pari perchè iniziamo da zero
                    x = ListaPari[i];
                Somma += x;
            }
            return ((Somma % 10 == 0) && (Somma != 0));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataEvento">DateTime momento dell'evento</param>
        /// <param name="userEvento">string operatore</param>
        /// <param name="Ambiente">string Ambiente {BO=back end, FO=front office}</param>
        /// <param name="Argomento" >stringArgomento {Tributi, Istanze, Reports, Profilo, Paga}</param>
        /// <param name="DettaglioArgomento">string Dettaglio Argomento {Riepilogo, Dichiarazione, ecc...}</param>
        /// <param name="Funzione">string Funzione {Page_Load, IstanzaNew, ecc...}</param>
        /// <param name="Azione">string Azione {chiesto consultazione ui, uscito pagina, ecc...}</param>
        /// <param name="idTributo">string tributo</param>
        /// <param name="idIstanza">string istanza</param>
        /// <param name="idEnte">string ente</param>
        public void LogActionEvent(DateTime dataEvento, string userEvento, string Ambiente, string Argomento, string DettaglioArgomento, string Funzione, string Azione, string idTributo, string idIstanza, string idEnte)
        {
            try
            {
                using (DBModel ctx = new DBModel())
                {
                    string sSQL = ctx.GetSQL("prc_TBLLOGACTIONEVENT_IU", "DATA_EVENTO", "USER_EVENTO", "EVENTO", "ID_TRIBUTO", "ID_ISTANZA", "IDENTE");
                    ctx.ContextDB.Database.ExecuteSqlCommand(sSQL, ctx.GetParam("DATA_EVENTO", dataEvento)
                            , ctx.GetParam("USER_EVENTO", userEvento)
                            , ctx.GetParam("EVENTO", Ambiente + "|" + Argomento + "|" + DettaglioArgomento + "|" + Funzione + "|" + Azione)
                            , ctx.GetParam("ID_TRIBUTO", idTributo)
                            , ctx.GetParam("ID_ISTANZA", idIstanza)
                            , ctx.GetParam("IDENTE", idEnte)
                        );
                    ctx.Dispose();
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.General.LogActionEvent::errore::", ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ListFiles"></param>
        /// <param name="IDTypeAttachTO"></param>
        /// <param name="ListAttachments"></param>
        /// <param name="ListAllegati"></param>
        /// <returns></returns>
        public bool UploadAttachments(HttpFileCollection ListFiles, int IDTypeAttachTO, ref List<System.Web.Mail.MailAttachment> ListAttachments, ref List<IstanzaAllegato> ListAllegati)
        {
            try
            {
                if (ListFiles != null)
                {
                    for (int i = 0; i < ListFiles.Count; i++)
                    {
                        HttpPostedFile PostedFile = ListFiles[i];
                        try
                        {
                            if (PostedFile.ContentLength > 0)
                            {
                                IstanzaAllegato myAll = new IstanzaAllegato();
                                myAll.IDTipo = IDTypeAttachTO;
                                byte[] data = new Byte[PostedFile.ContentLength];
                                PostedFile.InputStream.Read(data, 0, PostedFile.ContentLength);
                                myAll.PostedFile = data;
                                myAll.FileName = PostedFile.FileName;
                                myAll.FileMIMEType = PostedFile.ContentType;
                                System.IO.Stream myStream = new System.IO.MemoryStream(data);
                                //ListAttachments.Add(new System.Net.Mail.Attachment(myStream, PostedFile.FileName));
                                System.IO.File.WriteAllBytes(UrlHelper.GetPathAttachments + IDTypeAttachTO.ToString().PadLeft(3, char.Parse("0")) + "_" + PostedFile.FileName, data);
                                ListAttachments.Add(new System.Web.Mail.MailAttachment(UrlHelper.GetPathAttachments + IDTypeAttachTO.ToString().PadLeft(3, char.Parse("0")) + "_" + PostedFile.FileName));
                                ListAllegati.Add(myAll);
                            }
                        }
                        catch (Exception Ex)
                        {
                            Log.Debug("OPENgovSPORTELLO.General.UploadAttachments::errore::", Ex);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.General.UploadAttachments::errore::", ex);
                return false;
            }
        }
    }
    /// <summary>
    /// Classe di gestione dati sessione
    /// </summary>
    /// <revisionHistory><revision date="14/04/2021"><strong>SPID</strong>Se mi sono autenticato con SPID ma non sono registrato precompilo i campi</revision></revisionHistory>
    public class MySession
    {
        #region "Generali"
        public SortDirection SortDirection { get; set; }
        public string Scope { get; set; }
        public UserRole UserLogged { get; set; }
        public string ComuneSito { get; set; }
        public string scriptComproprietari { get; set; }
        #endregion
        #region "Back Office"
        public List<EntiInLavorazione> ListEnti { get; set; }
        public List<Istanza> GestIstanze { get; set; }
        public List<string> ParamRicIstanze { get; set; }
        public List<ComunicazioniBOvsFO> GestComunicazioniBOvsFO { get; set; }
        public List<GestPWD> GestPWD { get; set; }
        public List<Message> ListMessages { get; set; }
        #endregion
        #region "Front Office"
        public EntiInLavorazione Ente { get; set; }

        public DettaglioAnagrafica myAnag { get; set; }
        public List<ObjIndirizziSpedizione> IndirizziSpedizione { get; set; }
        public Anagrafica.DLL.dsContatti DataSetContatti { get; set; }

        public string TipoIstanza { get; set; }
        public int IdIstanza { get; set; }
        public int IdRifCalcolo { get; set; }
        public int IdRifDBOrg { get; set; }
        public object UIDichOld { get; set; }
        public int IDDichiarazioneIstanze { get; set; }
        public bool IsInitDich { get; set; }
        public string TipoStorico { get; set; }
        public List<RiepilogoUI> ListRiepUICatasto { get; set; }
        public RiepilogoUI UINewSuggest { get; set; }
        public CatastoBySoggetto myCatastoVSSoggetto { get; set; }
        public dynamic myCatastoVSRifCat { get; set; }
        public List<SPC_DichICI> ListDichUICatasto { get; set; }
        public int IdMessage { get; set; }
        public bool IsBackToTributi { get; set; }
        public int HasNewDich { get; set; }

        public List<string> ListShoppingCart { get; set; }
        public decimal TotShoppingCart { get; set; }
        public List<UserRole> UserNoConfirmed { get; set; }
        public SPIDAuthn SPIDAuthn { get; set; }
        #endregion

        /// <summary>
        /// Gets the current session.
        /// </summary>
        public static MySession Current
        {
            get
            {
                MySession session;
                try
                {
                    session = (MySession)HttpContext.Current.Session["__MySession__"];
                    if (session == null)
                    {
                        session = new MySession();
                        HttpContext.Current.Session["__MySession__"] = session;
                    }
                }
                catch
                {
                    session = new MySession();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <revisionHistory><revision date="11/03/2020"><strong>SPID</strong>Le regole SPID vogliono che il pulsante di accesso sia subito visibile; bisogna quindi spostare la selezione dell'ente dopo l'autenticazione</revision></revisionHistory>
        private MySession()
        {
            SortDirection = SortDirection.Descending;
            Scope = MySettings.GetConfig("Scope");
            UserLogged = new UserRole();

            ListEnti = new List<EntiInLavorazione>();
            GestIstanze = new List<Istanza>();
            ParamRicIstanze = new List<string>();
            GestComunicazioniBOvsFO = new List<ComunicazioniBOvsFO>();
            ListMessages = new List<Message>();

            Ente = null;

            myAnag = null;
            IndirizziSpedizione = new List<AnagInterface.ObjIndirizziSpedizione>();
            DataSetContatti = new Anagrafica.DLL.dsContatti();

            TipoIstanza = string.Empty;
            IdIstanza = -1;
            IdRifCalcolo = -1;
            IdRifDBOrg = -1;
            UIDichOld = null;
            IDDichiarazioneIstanze = -1;
            IsInitDich = false;
            TipoStorico = string.Empty;
            ListRiepUICatasto = new List<RiepilogoUI>();
            UINewSuggest = new RiepilogoUI();
            myCatastoVSSoggetto = new CatastoBySoggetto();
            ListDichUICatasto = new List<SPC_DichICI>();
            IdMessage = -1;
            IsBackToTributi = false;
            HasNewDich = 0;

            ListShoppingCart = new List<string>();
            TotShoppingCart = 0;
            UserNoConfirmed = new List<UserRole>();
            SPIDAuthn = null;
        }
    }

}