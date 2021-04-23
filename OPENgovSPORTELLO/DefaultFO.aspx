<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="DefaultFO.aspx.cs" Inherits="OPENgovSPORTELLO.DefaultFO" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div id="divLeftMenu" class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="col-md-12 lead">
            <label class="col-md-4" style="margin-top:10px;">Sportello Contribuente per il comune di </label>
            <asp:DropDownList runat="server" ID="ddlEnte" CssClass="subtitle col-md-4" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList>
            <p class="text-right col-md-4 text-success usain">Lo sportello è attivo 24 ore su 24.</p>
                            <p style="margin-left: 20px" class="hidden">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="GIS" />
                            </p>
        </div>
        <div id="divConfirmAccount divDescr" class="col-md-12">
            <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true"> 
                <h1>Grazie per aver confermato l’account.</h1>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
                <p class="text-danger">Si è verificato un errore.</p>
            </asp:PlaceHolder>
        </div>
        <div id="BodyHomePage" class="col-md-10 divDescr"></div>
		<div id="HomePagePrivacy" class="col-md-2 text-justified text-italic" style="padding-left: 20px;"></div>
        <div id="TheDialogNews" class="modalLoadingNewsContainer">
            <div class="modalLoadingNews">
            <p>News</p>
            <p>
                Ci sono comunicazioni per te!
            </p>
            </div>
        </div>
        <div class="col-md-12 divDescr">
            <div class="col-md-3 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHead">Tributi&ensp;<a class="Argbtn btn btn-default Tributi">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliTributi" src="Images/Files-icon.png" />
                </div>
                <div class="col-md-11" id="HomePageTributi"></div>
            </div>
            <div class="col-md-3 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHead">Profilo&ensp;<a class="Argbtn btn btn-default Profilo">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliProfilo" src="Images/Addressbook-3-icon.png" />
               </div>
                <div class="col-md-11" id="HomePageProfilo"></div>
            </div>
            <div class="col-md-3 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHead">Istanze&ensp;<a class="Argbtn btn btn-default IstanzeFO">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliIstanze" src="Images/Files-2-icon.png" />
                </div>
                <div class="col-md-11" id="HomePageIstanze"></div>
            </div>
            <div id="divPaga" class="col-md-3 text-justified">
                <div class="col-md-12 lead ArgPan">
                    <div class="ArgHead">Paga&ensp;<a class="Argbtn btn btn-default Paga">Vai &raquo;</a></div>
                    <input type="image" class="ArgImg imgBaseModuli imgBaseModuliLarge imgBaseModuliPaga" src="Images/Dollar-icon.png" />
                </div>
                <div class="col-md-11" id="HomePagePaga"></div>
            </div>
        </div>
        &nbsp;
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
