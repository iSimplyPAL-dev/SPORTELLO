<%@ Page Title="Cruscotto" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="FO_ReportGen.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.FO_ReportGen" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand">Cartella Unica</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeFO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeFO nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="Profilo nav navbar-nav Bottone BottoneAccount"></a></li>
            <li><a class="Profilo nav navbar-nav">Profilo</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="Tributi nav navbar-nav Bottone BottoneTributi"></a></li>
            <li><a class="Tributi nav navbar-nav">Tributi</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="IstanzeFO nav navbar-nav Bottone BottoneArchive"></a></li>
            <li><a class="IstanzeFO nav navbar-nav">Istanze</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="Paga nav navbar-nav Bottone BottonePayment"></a></li>
            <li><a class="Paga nav navbar-nav">Paga</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="col-md-12 pageFO">
            <div class="lead_con_barra">Prospetti possibili</div>
            <div class="col-md-12">
                <div class="col-md-12">
                    <div class="col-md-6">
                        <div class="col-md-12">
                            <a class="FOCartellaUnica nav navbar-nav Bottone BottoneGoTo"></a>
                            <label class="etichetta_conf">Cartella Unica</label>
                        </div>
                        <div class="col-md-12 hidden">
                            <a class="nav navbar-nav Bottone BottoneGoTo nonImplementato"></a>
                            <label class="etichetta_conf">Elenco dichiarazioni/comunicazioni</label>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="col-md-12 hidden">
                            <a class="nav navbar-nav Bottone BottoneGoTo nonImplementato"></a>
                            <label class="etichetta_conf">Elenco pagamenti effettuati</label>
                        </div>
                        <div class="col-md-12 hidden">
                            <a class="nav navbar-nav Bottone BottoneGoTo nonImplementato"></a>
                            <label class="etichetta_conf">Elenco avvisi di pagamenti non pagati</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>