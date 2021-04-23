using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace OPENgovSPORTELLO.Account
{
    /// <summary>
    /// Pagina di selezione delegante sul quale lavorare
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class SelDelegante : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SelDelegante));
        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string sScript = string.Empty;
            try
            {
                if (!Page.IsPostBack)
                {
                    List<string> ListDeleg = MySession.Current.UserLogged.ListDeleganti.Split(char.Parse(",")).ToList();

                    Startup.CountScript += 1;
                    string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                    sScript = "<script language='javascript'>";
                    sScript += "$('#divPrincipale').html('";
                    sScript += "<div id=\"CfgDeleg\" class=\"col-md-12\">";
                    sScript += "<div class=\"col-md-6\">";
                    sScript += "<a runat=\"server\" id=\"lnk" + MySession.Current.UserLogged.IDContribLogged + "\" name=\"lnk" + MySession.Current.UserLogged.IDContribLogged + "\" onclick=\"SelContribuente(" + MySession.Current.UserLogged.IDContribLogged + ");\" class=\"nav navbar-nav SubmitBtn Bottone BottoneGoTo\"></a>";
                    sScript += "<label class=\"etichetta\" onclick=\"SelContribuente(" + MySession.Current.UserLogged.IDContribLogged + ");\">" + MySession.Current.UserLogged.Nominativo + "</label>";
                    sScript += "<a class=\"btn btn-default hidebotton\">Vai &raquo;</a>";
                    sScript += "</div>";
                    sScript += "</div>";
                    sScript += "');";

                    sScript += "$('#divDeleganti').html('";
                    sScript += "<div id=\"CfgDeleg\" class=\"col-md-12\">";
                    foreach (string myItem in ListDeleg)
                    {
                        List<string> ListDetails = myItem.Split(char.Parse("|")).ToList();
                        if (ListDetails[0] != MySession.Current.UserLogged.IDContribLogged.ToString())
                        {
                            if (ListDetails[0] != string.Empty)
                            {
                                sScript += "<div class=\"col-md-6\">";
                                sScript += "<a runat=\"server\" id=\"lnk" + ListDetails[0] + "\" name=\"lnk" + ListDetails[0] + "\" onclick=\"SelContribuente(" + ListDetails[0] + ");\" class=\"nav navbar-nav SubmitBtn Bottone BottoneGoTo\"></a>";
                                sScript += "<label class=\"etichetta\" onclick=\"SelContribuente(" + ListDetails[0] + ");\">" + ListDetails[1] + "</label>";
                                sScript += "<a class=\"btn btn-default hidebotton\">Vai &raquo;</a>";
                                sScript += "</div>";
                            }
                        }
                    }
                    sScript += "</div>";
                    sScript += "')";
                    sScript += "</script>";
                    ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.SelDelegante.Page_Load::errore::", ex);
                LoadException(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <revisionHistory><revision date="23/03/2021">Aggiunto selezione ente per autenticazione SPID</revision></revisionHistory>
        protected void SetContribuente(object sender, EventArgs e)
        {
            string uniqueId = string.Empty;
            try
            {
                MySession.Current.UserLogged.IDContribToWork = int.Parse(hfIdContribuente.Value);
                if (MySession.Current.Ente != null)
                {
                    if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E")
                    {
                        Startup.CountScript += 1;
                        uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, "$('#upProgress').show();");
                        if (MySession.Current.myAnag == null)
                        {
                            MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                        }
                        string IdSoggetto = string.Empty;
                        if ((MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale).Trim() != string.Empty)
                        {
                            Log.Debug("SelDelegante.SetContribuente.ReadDatiVerticale per->" + (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale));
                            new BLLImport().ReadDatiVerticale(MySession.Current.Ente.IDEnte, (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale), ref IdSoggetto);
                            new BLLImport().SetDatiVerticaleRead(MySession.Current.Ente.IDEnte, IdSoggetto);
                        }
                        else
                        {
                            Log.Debug("SelDelegante.SetContribuente.NO ReadDatiVerticale perché non ho riferimento");
                        }
                        MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
                        Startup.CountScript += 1;
                        uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                        ClientScript.RegisterStartupScript(this.GetType(), uniqueId, "$('#upProgress').hide();");
                    }
                    IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
                }
                else
                {
                    string sScript = string.Empty;
                    sScript = "alert('Selezionare un’ente prima di poter accedere!');";
                    Startup.CountScript += 1;
                    uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                    ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);
                }
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.Account.SelDelegante.SetContribuente::errore::", ex);
                LoadException(ex);
            }
        }
        //protected void SetContribuente(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        MySession.Current.UserLogged.IDContribToWork = int.Parse(hfIdContribuente.Value);
        //        if (MySession.Current.Ente != null)
        //        {
        //            if (MySession.Current.Ente.DatiVerticali.TipoBancaDati == "E")
        //            {
        //                Startup.CountScript += 1;
        //                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
        //                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, "$('#upProgress').show();");
        //                if (MySession.Current.myAnag == null)
        //                {
        //                    MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
        //                }
        //                string IdSoggetto = string.Empty;
        //                if ((MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale).Trim() != string.Empty)
        //                {
        //                    Log.Debug("SelDelegante.SetContribuente.ReadDatiVerticale per->" + (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale));
        //                    new BLLImport().ReadDatiVerticale(MySession.Current.Ente.IDEnte, (MySession.Current.myAnag.PartitaIva != string.Empty ? MySession.Current.myAnag.PartitaIva : MySession.Current.myAnag.CodiceFiscale), ref IdSoggetto);
        //                    new BLLImport().SetDatiVerticaleRead(MySession.Current.Ente.IDEnte, IdSoggetto);
        //                }
        //                else
        //                {
        //                    Log.Debug("SelDelegante.SetContribuente.NO ReadDatiVerticale perché non ho riferimento");
        //                }
        //                 MySession.Current.myAnag = new Anagrafica.DLL.GestioneAnagrafica().GetAnagrafica(MySession.Current.UserLogged.IDContribToWork, Utility.Costanti.INIT_VALUE_NUMBER, string.Empty, RouteConfig.TypeDB, RouteConfig.StringConnectionAnagrafica, false);
        //                Startup.CountScript += 1;
        //                uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
        //                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, "$('#upProgress').hide();");
        //            }
        //        }
        //        IdentityHelper.RedirectToReturnUrl(UrlHelper.GetDefaultFO, Response);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Debug("OPENgovSPORTELLO.Account.SelDelegante.SetContribuente::errore::", ex);
        //        LoadException(ex);
        //    }
        //}
    }
}