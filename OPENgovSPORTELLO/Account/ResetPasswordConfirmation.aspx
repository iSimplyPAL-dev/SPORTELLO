<%@ Page Title="Password cambiata" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="ResetPasswordConfirmation.aspx.cs" Inherits="OPENgovSPORTELLO.Account.ResetPasswordConfirmation" Async="true" %>
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
        <h3>Complimenti! La password è stata cambiata.</h3><br /><br /><br />
        <h3>Cliccare <asp:HyperLink ID="login" runat="server" NavigateUrl="~/Account/LoginFO" CssClass="label btn btn-default">quì</asp:HyperLink> per accedere </h3>
    </div>
</asp:Content>
