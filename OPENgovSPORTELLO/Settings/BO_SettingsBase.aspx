<%@ Page Title="Configurazioni" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_SettingsBase.aspx.cs" Inherits="OPENgovSPORTELLO.Settings.BO_SettingsBase" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Configurazioni</li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
     <div class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeBO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeBO nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="IstanzeBO nav navbar-nav Bottone BottoneArchive"></a></li>
            <li><a class="IstanzeBO nav navbar-nav">Istanze</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="ReportBO nav navbar-nav Bottone BottoneReport"></a></li>
            <li><a class="ReportBO nav navbar-nav">Cruscotto</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQBO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQBO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="col-md-12 pageBO">
            <label id="lblErrorBO" class="col-md-12 text-danger usain"></label>
            <div class="lead_con_barra">Configurazioni possibili</div>
            <div class="col-md-12">
                <div class="col-md-12" id="divSistema">
                    <a onclick="ShowHideSettings('CfgSistema');"><h4>Tabelle di Sistema</h4></a>
                    <div id="CfgSistema">
                        <div class="col-md-6">
                            <div class="col-md-12">
                                <a runat="server" id="lnk01" class="nav navbar-nav Bottone BottoneGoTo"></a>
                                <label class="etichetta_conf">Comuni</label>
                                <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=01">Vai &raquo;</a>
                            </div>
                            <div class="col-md-12">
                                <a runat="server" id="lnk02" class="nav navbar-nav Bottone BottoneGoTo"></a>
                                <label class="etichetta_conf">Stradario</label>
                                <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=02">Vai &raquo;</a>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="col-md-12">
                                <a runat="server" id="lnk03" class="nav navbar-nav Bottone BottoneGoTo"></a>
                                <label class="etichetta_conf">Operatori</label>
                                <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=03">Vai &raquo;</a>
                            </div>
                            <div class="col-md-12">
                                <a runat="server" id="lnk04" class="nav navbar-nav Bottone BottoneGoTo"></a>
                                <label class="etichetta_conf">Documenti</label>
                                <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=04">Vai &raquo;</a>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<div class="col-md-5" style="margin:0 auto;">
                    <h4>Impostazioni di sistema</h4>
                    <div class="col-md-12">
                        <a href="" class="nav navbar-nav Bottone BottoneGoTo"></a>
                        <label class="etichetta_conf">Videate Dichiarazioni</label>
                        <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Vai &raquo;</a>
                    </div>
                    <div class="col-md-12">
                        <a href="" class="nav navbar-nav Bottone BottoneGoTo"></a>
                        <label class="etichetta_conf">Report</label>
                        <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Vai &raquo;</a>
                    </div>
                    <div class="col-md-12">
                        <a href="" class="nav navbar-nav Bottone BottoneGoTo"></a>
                        <label class="etichetta_conf">Help</label>
                        <a class="btn btn-default" href="http://go.microsoft.com/fwlink/?LinkId=301948">Vai &raquo;</a>
                    </div>
                </div>-->
            </div>
            <div class="col-md-12">
                <div id="divICI" class="col-md-4">
                    <a onclick="ShowHideSettings('CfgICI');"><h4>Tabelle IMU</h4></a>
                    <div id="CfgICI">
                        <div class="col-md-12">
                            <a runat="server" id="lnk60" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Utilizzo</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=60">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12 hidden">
                            <a runat="server" id="lnk61" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Tipo utilizzo</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=61">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk62" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Tipo Possesso</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=62">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk63" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Categorie catastali</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=63">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk64" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Zone PRG</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=64">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk66" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Vincoli PRG</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=66">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk67" class="nav navbar-nav Bottone BottoneGoTo" onclick="RedirectTo()"></a>
                            <label class="etichetta_conf">Aliquote</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=67">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk65" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Motivazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=65">Vai &raquo;</a>
                        </div>
                    </div>
                </div>
                <div id="divTARSU" class="col-md-4">
                    <a onclick="ShowHideSettings('CfgTARSU');"><h4>Tabelle TARI</h4></a>
                    <div id="CfgTARSU">
                        <div class="col-md-12">
                            <a runat="server" id="lnk50" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Categorie</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=50">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk55" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Vani</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=55">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk51" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Stato Occupazione</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=51">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk52" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Riduzioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=52">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk53" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Esenzioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=53">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk54" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Motivazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=54">Vai &raquo;</a>
                        </div>
                    </div>
                </div>
                <div id="divOSAP" class="col-md-4">
                    <a onclick="ShowHideSettings('CfgOSAP');"><h4>Tabelle OSAP</h4></a>
                    <div id="CfgOSAP">
                        <div class="col-md-12">
                            <a runat="server" id="lnk74" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Richiedente</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=74">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk75" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Categorie</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=75">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk76" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Tipo occupazione</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=76">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk77" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Agevolazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=77">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk78" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Motivazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=78">Vai &raquo;</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div id="divTASI" class="col-md-4">
                    <a onclick="ShowHideSettings('CfgTASI');"><h4>Tabelle TASI</h4></a>
                    <div id="CfgTASI">
                        <div class="col-md-12">
                            <a runat="server" id="lnk93" class="nav navbar-nav Bottone BottoneGoTo" onclick="RedirectTo()"></a>
                            <label class="etichetta_conf">Aliquote</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=93">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk92" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Agevolazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=92">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk94" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Motivazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=94">Vai &raquo;</a>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                </div>
                <div id="divICP" class="col-md-4">
                    <a onclick="ShowHideSettings('CfgICP');"><h4>Tabelle ICP</h4></a>
                    <div id="CfgICP">
                        <div class="col-md-12">
                            <a runat="server" id="lnk80" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Tipologia</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=80">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk81" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Caratteristica</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=81">Vai &raquo;</a>
                        </div>
                        <div class="col-md-12">
                            <a runat="server" id="lnk83" class="nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Motivazioni</label>
                            <a class="btn btn-default hidebotton" href="MngSettings.aspx?TOCONF=83">Vai &raquo;</a>
                        </div>
                    </div>
                </div>
            </div>
       </div>
        <div class="col-md-12 hidden">
            <div class="col-md-6 text-justified">
                <div class="col-md-12"><input type="image" class="imgIstanze" src="/Images/Files-2-icon.png" /></div>
                <h2 class="col-md-12">Istanze&ensp;<a class="btn btn-default IstanzeBO">Vai &raquo;</a></h2>
                <div class="col-md-11">Nella sezione Istanze si trovano le funzionalità necessarie alla gestione di tutte le comunicazioni/dichiarazioni registrate on-line dai contribuenti.<br />
                Si accede alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>.</div>
            </div>
            <div class="col-md-6 text-justified">
                <div class="col-md-12"><input type="image" class="imgCruscotto" src="/Images/Analytics-2-icon.png" /></div>
                <h2>Cruscotto&ensp;<a class="ReportBO btn btn-default">Vai &raquo;</a></h2>
                <div class="col-md-11">Il cruscotto  ha l’obiettivo di consentire l’analisi dei dati di sportello, ma anche la verifica dello stato di lavorazione delle istanze. Il Cruscotto  incluse le funzioni di estrazioni e reportistica del back office.<br />
                Si accede alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>.</div>
            </div>
        </div>
    </div>
</asp:Content>