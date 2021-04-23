using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using OPENgovSPORTELLO.Models;

namespace OPENgovSPORTELLO
{
    /// <summary>
    /// Classe di gestione pagina base per le istanze
    /// </summary>
    /// <remarks>In ottemperanza alle linee guida di sviluppo 1.0</remarks>
    public class BaseIstanze : BasePage
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(BaseIstanze));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", false);
            base.OnInit(e);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tributo"></param>
        /// <param name="StatoUI"></param>
        protected void ManageBottoniera(string Tributo, string StatoUI)
        {
            try
            {
                string sScript, StyleTributo, DescrTributo;
                sScript=StyleTributo=DescrTributo= "";

                if (Tributo == General.TRIBUTO.ICI)
                {
                    StyleTributo = "ICI";
                    DescrTributo = "IMU";
                    if (MySession.Current.Scope == "FO")
                        sScript += "$('#lblInfoRendita').html('Si ricorda che la rendita non è calcolata dal sistema ma deve sempre essere inserita.');";
                    RegisterScript(sScript, this.GetType());
                }
                else if (Tributo == General.TRIBUTO.TASI)
                {
                    StyleTributo = "TASI";
                    DescrTributo = "TASI";
                }
                else if(Tributo == General.TRIBUTO.TARSU)
                {
                    StyleTributo = "TARSU";
                    DescrTributo = "TARI";
                }
                else if (Tributo == General.TRIBUTO.OSAP)
                {
                    StyleTributo = "OSAP";
                    DescrTributo = "OSAP";
                }
                else if (Tributo == General.TRIBUTO.ICP)
                {
                    StyleTributo = "ICP";
                    DescrTributo = "ICP";
                }
                else if (Tributo == General.TRIBUTO.PROVVEDIMENTI)
                {
                    StyleTributo = "PROVVEDIMENTI";
                    DescrTributo = "ACCERTAMENTI";
                }
                else

                if (MySession.Current.TipoIstanza == Istanza.TIPO.NuovaDichiarazione)
                    ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
                if (MySession.Current.TipoIstanza != Istanza.TIPO.Modifica && MySession.Current.TipoIstanza != Istanza.TIPO.Variazione)
                {
                    if ((MySession.Current.TipoIstanza != Istanza.TIPO.ConsultaDich && MySession.Current.TipoIstanza != Istanza.TIPO.ConsultaCatasto && !MySession.Current.TipoIstanza.StartsWith( Istanza.TIPO.ConsultaDich)) && MySession.Current.IdIstanza <= 0)
                    {
                        if (MySession.Current.TipoIstanza != Istanza.TIPO.NuovaDichiarazione)
                        { sScript += "$('#lblForewordUI').show();"; }
                        else { sScript += "$('#lblForewordUI').hide();"; }
                        sScript += "$('.lead_header').text('Dati Immobile";
                        sScript += " - Istanza di " + ((Tributo == General.TRIBUTO.OSAP) ? MySession.Current.TipoIstanza.Replace("Dichiarazione","Richiesta") : MySession.Current.TipoIstanza) + "');";
                        ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
                    }
                    else
                    {
                        sScript += "$('#lblForewordUI').hide();";
                        sScript += "$('.lead_header').text('Dati Immobile');";
                    }
                }
                else
                {
                    sScript += "$('#lblForewordUI').show();";
                    sScript += "$('.lead_header').text('Dati Immobile');";
                    if (StatoUI != "ML")
                    {
                        if (MySession.Current.HasNewDich == 2 || MySession.Current.HasNewDich==0)
                        {
                            sScript += "$('.lead_header').removeClass('col-md-5');";
                            sScript += "$('.lead_header').addClass('col-md-2');";
                            sScript += "$('.BottoneDiv').show();";
                            ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", true);
                        }
                        else if (MySession.Current.HasNewDich == 1 && MySession.Current.TipoIstanza!=Istanza.TIPO.NuovaDichiarazione)
                        {
                            sScript += "$('#lblForewordUI').hide();";
                            ShowHide(BLL.GestForm.PlaceHolderName.Title + "_CmdSave", false);
                        }
                    }
                    else
                    {
                        ShowHide("divMotivazione", false);
                    }
                }
                if (MySession.Current.IdIstanza > 0)
                {
                    if (MySettings.GetConfig("TypeProtocollo") == "S")
                    {
                        sScript += "$('.BottoneCounter').hide();";
                    }
                    sScript += "$('#lblForewordUI').hide();";
                    sScript += "$('#divDatiIstanza').show();";
                    sScript += "$('#lblHeadMotivazione').text('Comunicazioni');";
                    if (MySession.Current.Scope == "BO")
                    {
                        sScript += "$('#MenuBO').show();$('#divLeftMenu').hide();$('#lblMotivazione').hide();";
                        sScript += "$('#PageTitle').html('<a class=\"IstanzeBO text-white\">Istanze</a>&ensp;-&ensp;Consultazione Istanza');";
                    }
                    else
                    {
                        sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();$('#lblMotivazione').show();";
                        sScript += "$('#PageTitle').html('<a class=\"Tributi text-white\">Tributi</a>&ensp;-&ensp;<a class=\""+ StyleTributo +" text-white\" id=\"TitlePage\">Consultazione "+DescrTributo+"</a>&ensp;-&ensp;Immobile');";
                    }
                }
                else
                {
                    sScript += "$('#divDatiIstanza').hide();";
                    sScript += "$('#divLeftMenu').show();$('#MenuBO').hide();$('#lblMotivazione').show();";
                    sScript += "$('#lblHeadMotivazione').text('Motivazioni');";
                    sScript += "$('#PageTitle').html('<a class=\"Tributi text-white\">Tributi</a>&ensp;-&ensp;<a class=\""+StyleTributo+" text-white\" id=\"TitlePage\">Consultazione "+DescrTributo+"</a>&ensp;-&ensp;Immobile');";
                }
                if ((MySession.Current.Ente.SIT.IsActive == 1 ? true : false))
                    sScript += "$('#lblSIT').show();$('.BottoneMap').show();";
                else
                    sScript += "$('#lblSIT').hide();$('.BottoneMap').hide();";
                RegisterScript(sScript, this.GetType());
            }
            catch (Exception ex)
            {
                Log.Debug("OPENgovSPORTELLO.BaseIstanze.ManageBottoniera::errore::", ex);
                LoadException(ex);
            }
            finally
            {
                RegisterScript("$('.divGrdBtn').hide();", this.GetType());
            }
        }
    }
}