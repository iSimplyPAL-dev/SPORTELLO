<%@ Page Title="Istanze" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_IstanzeGen.aspx.cs" Inherits="OPENgovSPORTELLO.Istanze.BO_IstanzeGen" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Gestione Istanze</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">                
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneDataBase" OnClick="PagamentiToVerticale" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSearch" OnClientClick="return FieldValidatorRicBOIstanze();" OnClick="Search" /></li>                
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
            <div class="panel panel-primary">
                <div class="col-md-12">                                        
                    <div class="col-md-2">
                        <p><label id="lblEnte">Ente:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-11" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList> 
                    </div>
                    <div class="col-md-3">
                        <p><label id="lblNominativo">Nominativo:</label></p>
                        <asp:TextBox runat="server" ID="txtNominativo" class="col-md-11"></asp:TextBox> 
                    </div>
                    <div class="col-md-2">
                        <p><label id="lblCFPIVA">Cod.Fiscale/P.IVA:</label></p>
                        <asp:TextBox runat="server" ID="txtCFPIVA" class="col-md-11"></asp:TextBox> 
                    </div>                        
                    <div class="col-md-1">
                        <p><label id="lblTributo">Tributo:</label></p>
                        <asp:DropDownList runat="server" ID="ddlTributo" class="col-md-11"></asp:DropDownList>
                    </div>
                    <div class="col-md-2">
                        <p><label id="lblDataPresentazione">Data Registrazione:</label></p>
                        <asp:TextBox runat="server" ID="txtDataPresentazione" class="col-md-11 text-right" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:TextBox> 
                    </div>
                    <div class="col-md-2">
                        <p><label id="lblStatoIstanze">Stato Istanze:</label></p>
                        <asp:DropDownList runat="server" ID="ddlStatoIstanze" class="col-md-11"></asp:DropDownList>
                    </div>
                </div>&nbsp;
            </div>     
            <div class="col-md-12">
                <label id="lblResult" class="text-danger"></label>
                <Grd:RibesGridView ID="GrdResult" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowDataBound="GrdResultRowDataBound"
                    OnRowCommand="GrdResultRowCommand" OnSorting="GrdResultRowSorting">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <FooterStyle CssClass="CartListFooter"></FooterStyle>
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <RowStyle CssClass="CartListItem" />
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Sel.">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <div id="GrdBtnCol">
                                    <asp:CheckBox ID="chkSel" runat="server"/>
                                    <div class="divGrdBtn">
                                        <div class="panel panelGrd">
                                            <ul class="nav navbar-nav">
                                                <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="UIOpen" CommandArgument='<%# Eval("IDIstanza") %>'/><p id="apri"></p></li>
                                                <asp:HiddenField ID="hfIDIstanza" runat="server" Value='<%# Eval("IDIstanza") %>' />
                                                <asp:HiddenField ID="hfTributo" runat="server" Value='<%# Eval("IDTributo") %>' />
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Utente" DataField="Nominativo" SortExpression="Nominativo">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Cod.Fiscale/P.IVA" DataField="CodFiscalePIVA" SortExpression="CodFiscalePIVA">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tributo" DataField="DescrTributo" SortExpression="DescrTributo">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Tipo Istanza">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                            <ItemTemplate>
                                <asp:TextBox runat="server" Text='<%# FncGrd.FormattaNDichIstanza(Eval("NDichiarazione"),Eval("DescrTipo")) %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Data Presentazione">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                            <ItemTemplate>
                                <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataPresentazione")) %>' CssClass="text-right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Stato/Esito" DataField="DescrStato">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div class="col-md-12 hidden">
                <div class="col-md-6 text-justified">
                    <div class="col-md-12"><input type="image" class="imgConfigurazioni" src="/Images/Settings-icon.png" /></div>
                    <h2 class="col-md-12">Configurazioni&ensp;<a class="btn btn-default Configurazioni">Vai &raquo;</a></h2>
                    <div class="col-md-11">La sezione Configurazioni per la gestione di tutte le tabelle di Sistema e le tabelle degli specifici tributi. Qui sono configurabili anche gli operatori del sistema a cura dell’Amministratore.<p/>
				    Si accede alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b></div>
                </div>
                <div class="col-md-6 text-justified">
                    <div class="col-md-12"><input type="image" class="imgCruscotto" src="/Images/Analytics-2-icon.png" /></div>
                    <h2>Cruscotto&ensp;<a class="ReportBO btn btn-default">Vai &raquo;</a></h2>
                    <div class="col-md-11">Il cruscotto  ha l’obiettivo di consentire l’analisi dei dati di sportello, ma anche la verifica dello stato di lavorazione delle istanze. Il Cruscotto  incluse le funzioni di estrazioni e reportistica del back office.<br />
                    Si accede alla sezione con un clic su <b>&lsquo;Vai &raquo;&rsquo;</b>.</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>