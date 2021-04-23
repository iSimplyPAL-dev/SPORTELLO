<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_TempiPagamento.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.Analisi.BO_TempiPagamento" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Tempi di Pagamento</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">  
                <!--<li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSearch" OnClientClick="return FieldValidatorRicTempiPagamento();" OnClick="Search" /></li>-->
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSearch" OnClick="Search" /></li> 
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" /></li>
                <!--il bottone è nascosto ma permette l'import dei dati-->
                <li class="bottoni_header hide"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneReport" OnClick="TempiMedi" /></li>         
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
            <label id="lblErrorBO" class="text-danger usain"></label>
            <div class="panel panel-primary col-md-12">
                <div class="col-md-12">                                        
                    <div class="col-md-4">
                        <p><label id="lblEnte">Enti:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-11 Azzera" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectedIndexChanged"></asp:DropDownList> 
                    </div>     
                    <div class="col-md-2">
                        <p><label id="lblTributo">Tributo:</label></p>
                        <asp:DropDownList runat="server" ID="ddlTributo" class="col-md-11 Azzera" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectedIndexChanged01"></asp:DropDownList> 
                    </div>  
                    <div class="col-md-2">
                        <p><label id="lblDal">Data Emissione:</label></p>
                        <asp:DropDownList runat="server" ID="ddlDataEmissione" class="col-md-11 Azzera" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectedIndexChanged02"></asp:DropDownList> 
                    </div>                                  
                    <div class="col-md-2">
                        <p><label id="lblScadenza">Scadenza:</label></p>
                        <asp:DropDownList runat="server" ID="ddlScadenza" class="col-md-11 Azzera" AutoPostBack="true" OnSelectedIndexChanged="ddlSelectedIndexChanged03"></asp:DropDownList> 
                    </div>      
                </div> 
            </div>  
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
                        <asp:TemplateField HeaderText="Scadenza">
                            <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                            <ItemTemplate>
                                <asp:TextBox ID="txtScadenza" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("Scadenza")) %>' CssClass="text-right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Periodo" DataField="Periodo">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="N.Contribuenti" DataField="Contribuenti" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Importo" DataField="Importo" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="% Contribuenti" DataField="PercContribuenti" DataFormatString="{0:N2}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="% Pagato" DataField="PercPagato" DataFormatString="{0:N2}">
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


