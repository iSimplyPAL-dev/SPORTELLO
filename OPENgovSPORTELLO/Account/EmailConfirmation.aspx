<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="EmailConfirmation.aspx.cs" Inherits="OPENgovSPORTELLO.Account.EmailConfirmation" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div id="divLeftMenu" class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeFO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeFO nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="body_page">
        <p class="lead_con_barra">Registrazione Effettuata</p>
        <div class="form-horizontal">
            <br />
            <h4>Grazie per esserti registrato.</h4>
            <br />
            <h4>Prima di effettuare il primo accesso controlla le tue email e clicca sul link per abilitare il tuo nuovo Account!</h4>
            <br />
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
