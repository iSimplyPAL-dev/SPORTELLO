<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Dich.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.TESSERE.Dich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p id="PageTitle" class="navbar-brand"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;<a class="TARSU text-white" id="TitlePage">Consultazione TESSERE</a>&ensp;-&ensp;Dettaglio</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
     <div id="divLeftMenu" class="container">
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
    <div id="MenuBO" class="container">
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
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <div class="body_page">
        <div class="jumbotronAnag"></div>
        <div id="divIstanza" class="col-md-12 pageFO">
            <label id="lblErrorFO" class="text-danger usain"></label>
            <div id="divDatiUI" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDetUI','Dati Tessera')" href="#"></a>
                    <div id="lblForewordUI" class="col-md-7 text-11 text-italic" style="margin-top: 30px;"></div>
                </div>  
                <div id="divDetUI" class="col-md-12">
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <p>
                                <label id="lblNTessera">N. Tessera:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtNTessera" CssClass="Azzera col-md-10"></asp:TextBox>
                        </div>
                        <div class="col-md-3">                        
                            <p><label id="lblTipo">Tipo:</label></p>
                            <asp:DropDownList runat="server" ID="ddlTipoTessera" class="col-md-6"></asp:DropDownList> 
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblLnkConf">Conferimenti</label>
                            </p>
                            <p style="margin-left:20px">
                                <img class="BottoneMini BottoneTessera"/>
                            </p>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblDataInizio">Data Rilascio:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtDataInizio" CssClass="Azzera col-md-10 text-right" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p><label id="lblDataFine">Data Cessazione:</label></p>
                            <asp:TextBox runat="server" ID="txtDataFine" CssClass="Azzera col-md-7 text-right" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div id="divDatiConf" class="col-md-12">
                    <div class="col-md-12 lead_con_barra">
                        <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDetConf','Dati Conferimenti')" href="#"></a>
                        <div id="lblForewordConf" class="col-md-7 text-11 text-italic" style="margin-top: 30px;"></div>
                        <ul class="nav navbar-nav lead-right-btn">
                            <li id="ColCmdSearch">
                                <div class="BottoneDiv">
                                    <asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneSearch" OnClick="Search" />
                                    <p id="ricerca" class="col-md-12 TextCmdBlack"></p>
                                </div>
                            </li>
                        </ul>
                    </div>  
                    <div class="col-md-12"> 
                        <div class="col-md-1">
                            <p><label id="lblDal">Dal:</label></p>
                            <asp:TextBox runat="server" ID="txtDal" class="col-md-11 text-right Azzera" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox> 
                        </div> 
                        <div class="col-md-1">
                            <p><label id="lblAl">Al:</label></p>
                            <asp:TextBox runat="server" ID="txtAl" class="col-md-11 text-right Azzera" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox> 
                        </div>
                        <div class="col-md-3 ParamRaffronto">
                            <p><label id="lblTipoPeriodo">Tipo Periodo (tipo+quantità):</label></p>
                            <asp:DropDownList runat="server" ID="ddlPeriodo" class="col-md-6"></asp:DropDownList> 
                            <asp:TextBox runat="server" ID="TxtNPeriodo"  CssClass="OnlyNumber text-right col-md-4 Azzera"></asp:TextBox>&nbsp; 
                        </div>
                    </div>
                    <div id="divDetConf" class="col-md-12"> 
                        <div class="col-md-5">
                            <Grd:RibesGridView ID="GrdConf" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Dal">
                                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Al">
                                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="N.Conferimenti" DataField="NUMERO" DataFormatString="{0:N0}">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right" ></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Litri" DataField="VALORE" DataFormatString="{0:N0}">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Right" ></itemstyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            &nbsp;
                        </div>
                        <div class="col-md-1"></div>
                        <div id="chart_div" class="col-md-6" style="height:400px;"></div>
                    </div>
                </div>        
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
