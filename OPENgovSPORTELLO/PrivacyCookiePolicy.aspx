<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="PrivacyCookiePolicy.aspx.cs" Inherits="OPENgovSPORTELLO.PrivacyCookiePolicy" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div id="divLeftMenu" class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="Login nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="Login nav navbar-nav">Home</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="col-md-12 lead_con_barra">
            <div class="col-md-4 lead_header">Privacy and Cookie Policy</div>
        </div>
        <div id="BodyPrivacyCookiePolicy" class="col-md-12 divDescr text-justified"></div>
        &nbsp;
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
