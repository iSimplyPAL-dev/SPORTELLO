<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_DovutoVersato.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.Analisi.BO_DovutoVersato" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Dovuto/Versato IMU</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">  
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSearch" OnClick="Search" /></li> 
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" /></li>           
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
            <div class="panel panel-primary col-md-12">
                <div class="col-md-12">                                        
                    <div class="col-md-4">
                        <p><label id="lblEnte">Enti:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-11 Azzera"></asp:DropDownList> 
                    </div>     
                    <div class="col-md-1">
                        <p><label id="lblDal">Anno:</label></p>
                        <asp:TextBox runat="server" ID="txtAnno" class="col-md-11 text-right OnlyNumber"></asp:TextBox> 
                    </div> 
                </div> 
            </div>  
            <div id="divRiepilogo" class="col-md-12"></div>
            <div class="col-md-12">
                <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="False">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <FooterStyle CssClass="CartListFooter"></FooterStyle>
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="Anno" DataField="Anno">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Utente" DataField="Nominativo" SortExpression="Nominativo">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Cod.Fiscale/P.IVA" DataField="CodFiscalePIVA">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Dovuto Acconto" DataField="dovutoacc" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Versato Acconto" DataField="versatoacc" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Dovuto Saldo" DataField="dovutosal" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Versato Saldo" DataField="versatosal" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Dovuto Tot." DataField="dovuto" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Versato Tot." DataField="versato" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Differenza" DataField="differenza" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>


