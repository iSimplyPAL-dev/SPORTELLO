<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_ComunicazioniBOvsFO.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.BO_ComunicazioniBOvsFO" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Log Attività</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn"> 
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSearch" OnClientClick="return FieldValidatorRicBOIstanze();" OnClick="Search" /></li>              
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
    <div class="body_page"><br />
        <div class="col-md-12 pageBO">
            <div class="panel panel-primary col-md-12">
                <div class="col-md-12">                                        
                    <div class="col-md-2">
                        <p><label id="lblEnte">Ente:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-11 Azzera"></asp:DropDownList> 
                    </div>     
                    <div class="col-md-1">
                        <p><label id="lblDal">Dal:</label></p>
                        <asp:TextBox runat="server" ID="txtDal" class="col-md-11 text-right Azzera" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox> 
                    </div> 
                    <div class="col-md-1">
                        <p><label id="lblAl">Al:</label></p>
                        <asp:TextBox runat="server" ID="txtAl" class="col-md-11 text-right Azzera" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox> 
                    </div>
                    <div class="col-md-2">
                        <p><label id="lblOperatore">Operatore:</label></p>
                        <asp:TextBox runat="server" ID="txtOperatore" class="col-md-11"></asp:TextBox> 
                    </div>  
                    <div class="col-md-2">
                        <p><label id="lblCFPIVA">Cod.Fiscale/P.IVA:</label></p>
                        <asp:TextBox runat="server" ID="txtCFPIVA" class="col-md-11"></asp:TextBox> 
                    </div>  
                    <div class="col-md-2">
                        <p><label id="lblTipoIstanze">Tipo Istanze:</label></p>
                        <asp:DropDownList runat="server" ID="ddlTipoIstanze" class="col-md-10 Azzera"></asp:DropDownList> 
                    </div>
                </div>
            </div>     
            <Grd:RibesGridView ID="GrdComunicazioni" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true"
                OnSorting="GrdComunicazioniSorting">
                <PagerSettings Position="Bottom"></PagerSettings>
                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                <RowStyle CssClass="CartListItem"></RowStyle>
                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField HeaderText="Utente" DataField="Nominativo" SortExpression="Nominativo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Cod.Fiscale/P.IVA" DataField="CodFiscalePIVA">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Tipo Istanza" DataField="DescrIstanza" SortExpression="DescrIstanza">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Data Operazione">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("Data")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Provenienza" DataField="Provenienza">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Stato" DataField="Stato" SortExpression="Stato">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Operatore" DataField="Operatore" SortExpression="Operatore">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Grd:RibesGridView>
        </div>
     </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>

