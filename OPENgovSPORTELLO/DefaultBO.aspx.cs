using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// La videata di apertura del back office presenta le macro attività alle quali l’operatore può accedere.
    /// Se l’operatore è abilitato a più enti, la loro selezione sarà direttamente nelle pagine delle funzionalità.
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public partial class _Default : GeneralPage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(_Default));

        /// <summary>
        /// Caricamento della pagina
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            MySession.Current.Scope = "BO";
            //codice per rimuovere la registrazione BO
            //creo e compongo stringa sScript da eseguire
            string sScript = "<script language='javascript'>";
            sScript += "$('.nascondiregistrati').hide();";
            sScript += "</script>";
            //registro script con type, "spc"+data&ora+millisecondi per avere una key unica nella pagina, sScript da eseguire
            ClientScript.RegisterStartupScript(this.GetType(), "spc" + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString(), sScript);
            MySession.Current.ParamRicIstanze = null;

             try
            {
                Startup.CountScript += 1;
                string uniqueId = "spc_" + Startup.CountScript.ToString() + DateTime.Now.ToString() + "." + DateTime.Now.Millisecond.ToString();
                sScript = "<script language='javascript'>";
                sScript += new BLL.GestForm().GetLabel("DefaultBO", "");
                sScript += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), uniqueId, sScript);

                new BLL.Messages(new Models.Message()).SendMail();
                new BLL.GestForm().KillSleepProcess();
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BasePage.GetLabelForm::errore::", ex);
            }
        }
    }
}