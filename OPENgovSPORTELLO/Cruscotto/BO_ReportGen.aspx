<%@ Page Title="Cruscotto" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_ReportGen.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.BO_ReportGen" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand">Cruscotto</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>
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
            <li><a class="Configurazioni nav navbar-nav Bottone BottoneTools"></a></li>
            <li><a class="Configurazioni nav navbar-nav">Configurazioni</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="IstanzeBO nav navbar-nav Bottone BottoneArchive"></a></li>
            <li><a class="IstanzeBO nav navbar-nav">Istanze</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQBO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQBO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="col-md-12 pageBO"><br />
            <div class="col-md-12">
                <div id="divComunicazioni" class="col-md-12">
                    <div id="CfgComunicazioni" class="col-md-6">
                        <a runat="server" id="lnkComunicazioni" class="nav navbar-nav Bottone BottoneGoTo"></a>
                        <label class="etichetta_conf">Comunicazioni</label>
                        <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
                    </div>
                    <div id="CfgPWDToSend" class="col-md-6">
                        <a runat="server" id="lnkPWDToSend" class="nav navbar-nav Bottone BottoneGoTo PWDToSend"></a>
                        <label class="etichetta_conf">Password da inviare</label>
                        <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
                    </div>
                    <div id="CfgUserNoConfirmed" class="col-md-6">
                        <a runat="server" id="lnkUserNoConfirmed" class="nav navbar-nav Bottone BottoneGoTo UserNoConfirmed"></a>
                        <label class="etichetta_conf">Utenti non confermati</label>
                        <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
                    </div>
                </div>
            </div>
            <label class="lead_con_barra col-md-12"></label>
            <div class="col-md-6">
                <a class="nav navbar-nav Bottone BottoneGoTo AnalisiIstanze"></a>
                <label class="col-md-11">Analisi Istanze</label>
                <a class="btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div class="col-md-6">
                <a class="nav navbar-nav Bottone BottoneGoTo AnalisiEventi"></a>
                <label class="col-md-11">Analisi Attività</label>
                <a class="btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div class="col-md-6">
                <a href="" class="nav navbar-nav Bottone BottoneGoTo ComunicazioniBOvsFO"></a>
                <label class="col-md-11">Log Attività</label>
                <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
            </div>
            <div class="col-md-6">
                <a href="" class="nav navbar-nav Bottone BottoneGoTo BOTempiPagamento"></a>
                <label class="col-md-11">Tempi di pagamento</label>
                <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
            </div>
            <div class="col-md-6">
                <a href="" class="nav navbar-nav Bottone BottoneGoTo BOCartellaUnica"></a>
                <label class="col-md-11">Cartella Unica</label>
                <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
            </div>
            <div class="col-md-6">
                <a href="" class="nav navbar-nav Bottone BottoneGoTo BODovutoVersato"></a>
                <label class="col-md-11">Raffronto dovuto/versato</label>
                <a class="btn btn-default hidebotton" href="">Vai &raquo;</a>
            </div>
        </div>
        <div class="col-md-12 hidden">
            <div class="col-md-6 text-justified">
                <div class="col-md-12"><input type="image" class="imgConfigurazioni" src="/Images/Settings-icon.png" /></div>
                <h2 class="col-md-12">Configurazioni&ensp;<a class="btn btn-default Configurazioni">Vai &raquo;</a></h2>
                <div class="col-md-11">La sezione Configurazioni per la gestione di tutte le tabelle di Sistema e le tabelle degli specifici tributi. Qui sono configurabili anche gli operatori del sistema a cura dell’Amministratore.<p/>
				Si accede alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b></div>
            </div>
            <div class="col-md-6 text-justified">
                <div class="col-md-12"><input type="image" class="imgIstanze" src="/Images/Files-2-icon.png" /></div>
                <h2 class="col-md-12">Istanze&ensp;<a class="btn btn-default IstanzeBO">Vai &raquo;</a></h2>
                <div class="col-md-11">Nella sezione Istanze si trovano le funzionalità necessarie alla gestione di tutte le comunicazioni/dichiarazioni registrate on-line dai contribuenti.<br />
                Si accede alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>.</div>
            </div>
        </div>
    </div>
</asp:Content>