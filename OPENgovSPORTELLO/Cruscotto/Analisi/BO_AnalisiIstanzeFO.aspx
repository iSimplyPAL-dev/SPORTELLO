<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_AnalisiIstanzeFO.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeFO" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Analisi Istanze</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn"> 
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneExportXLS" OnClick="ExportXLS" /></li>              
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
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <div class="body_page"><br />
        <div class="col-md-12 pageBO">
            <div class="panel panel-primary col-md-12">
                <div class="col-md-12"> 
                    <div class="col-md-1">
                        <p>&nbsp;</p>
                        <asp:RadioButton id="optSintetica" Text=" Sintetica" GroupName="TypeAnalisi" checked="true" runat="server"/>
                    </div> 
                    <div class="col-md-1">
                        <p>&nbsp;</p>
                        <asp:RadioButton id="optAnalitica" Text=" Analitica" GroupName="TypeAnalisi" runat="server"/>
                    </div>
                    <div class="col-md-1">
                        <p>&nbsp;</p>
                        <asp:RadioButton id="optRaffronto" Text=" Raffronto" GroupName="TypeAnalisi" runat="server"/>
                    </div>                                        
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
                        <p><label id="lblTipoIstanze">Tipo Istanze:</label></p>
                        <asp:DropDownList runat="server" ID="ddlTipoIstanze" class="col-md-10 Azzera"></asp:DropDownList> 
                    </div>  
                    <div class="col-md-3 ParamRaffronto">
                        <p><label id="lblTipo">Tipo Periodo (quantità+tipo):</label></p>
                        <asp:TextBox runat="server" ID="TxtNPeriodo"  CssClass="OnlyNumber text-right col-md-4 Azzera"></asp:TextBox>&nbsp; 
                        <asp:DropDownList runat="server" ID="ddlPeriodo" class="col-md-6"></asp:DropDownList> 
                    </div>
                </div>
            </div>     
            <label id="OnlyNumber_error" class="text-danger usain"></label>
            <div id="divDownloadXLS" class="col-md-12">
                <button id="btnDownload" class="SubmitBtn"><i class="fa fa-download"></i> Scarica il file</button>
            </div>
            <div class="col-md-12">
                <div class="col-md-5">
                    <Grd:RibesGridView ID="GrdSyntheticNoTrib" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="70%"
                        AutoGenerateColumns="False" AllowPaging="False">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Tipo" DataField="TipoAccesso">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="N." DataField="Numero" DataFormatString="{0:N0}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right" ></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <Grd:RibesGridView ID="GrdSynthetic" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="70%"
                        AutoGenerateColumns="False" AllowPaging="False">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                        <Columns>
                            <asp:BoundField HeaderText="Tipo" DataField="TipoAccesso">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="N." DataField="Numero" DataFormatString="{0:N0}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right" ></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <Grd:RibesGridView ID="GrdCompare" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
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
                            <asp:BoundField HeaderText="Tipo Istanza" DataField="TipoAccesso">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField HeaderText="N.Istanze" DataField="Numero" DataFormatString="{0:N0}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right" ></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
                <div class="col-md-1"></div>
                <div id="chart_div" class="col-md-6" style="height:400px;"></div>
                <Grd:RibesGridView ID="GrdAnalytic" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="False">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <FooterStyle CssClass="CartListFooter"></FooterStyle>
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="Ente" DataField="Ente">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Nominativo" DataField="Nominativo">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Codice Fiscale" DataField="CodiceFiscale">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Tipo Istanza" DataField="TipoAccesso">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="N.Istanze" DataField="Numero" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                    </Columns>
                </Grd:RibesGridView>   
            </div>
            <div class="row hidden">
                <div class="col-md-6">
                    <h2>Cruscotto Attività&ensp;<a class="ReportBO btn btn-default">Vai &raquo;</a></h2>
                    <p>
                        Introduzione alla funzionalità.<br />
                        Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                    </p>
                </div>
                <div class="col-md-6">
                    <h2>Cruscotto Stato Istanze&ensp;<a class="ReportBO btn btn-default">Vai &raquo;</a></h2>
                    <p>
                        Introduzione alla funzionalità.<br />
                        Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.
                    </p>
                </div>
            </div>
        </div>
     </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>

