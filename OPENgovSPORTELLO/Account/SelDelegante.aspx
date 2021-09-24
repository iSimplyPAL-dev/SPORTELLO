<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="SelDelegante.aspx.cs" Inherits="OPENgovSPORTELLO.Account.SelDelegante" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Selezione Delegante</li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div id="divLeftMenu" class="container hidden">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeFO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeFO nav navbar-nav">Home</a></li>
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
            <li><a class="ReportFO nav navbar-nav Bottone BottoneReport"></a></li>
            <li><a class="ReportFO nav navbar-nav">Cartella Unica</a></li>
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
    <div class="col-md-12 form-group lead" style="padding-bottom: 30px;">
        <label class="col-md-4" style="margin-top:10px;">Sportello Contribuente per il comune di </label>
        <%--<asp:DropDownList runat="server" ID="ddlEnte" CssClass="subtitle col-md-4" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList>--%>
    </div>
    <div class="body_page">
       <div class="col-md-12 pageFO">
            <div class="col-md-12">
                <div id="divPrincipale" class="col-md-12"></div>
            </div><div class="lead_con_barra">Deleganti</div>
            <div class="col-md-12">
                <div id="divDeleganti" class="col-md-12"></div>
            </div>
       </div>
    </div>
    <asp:Button ID="CmdSetContribuente" runat="server" Text="" CssClass="hidden" OnClientClick="return FireServerSideClick()" OnClick="SetContribuente"></asp:Button>
    <asp:HiddenField ID="hfIdContribuente" runat="server" Value="-1" />
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
