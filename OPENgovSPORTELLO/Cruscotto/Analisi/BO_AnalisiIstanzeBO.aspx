<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_AnalisiIstanzeBO.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.Analisi.BO_AnalisiIstanzeBO" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Analisi Attività</li>
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
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <div class="body_page">
        <div class="col-md-12 pageBO">
            <label id="OnlyNumber_error" class="text-danger usain"></label>
            <div class="panel panel-primary col-md-12">
                <div class="col-md-12">                                        
                    <div class="col-md-4">
                        <p><label id="lblEnte">Enti:</label></p>
                        <Grd:RibesGridView ID="GrdEnti" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sel.">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSel" runat="server" Checked='<%# FncGrd.FormattaBoolGrd(Eval("IsActive")) %>'></asp:CheckBox>
                                        <asp:HiddenField ID="hfCodice" runat="server" Value='<%# Eval("CODICE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>&nbsp;
                    </div>     
                    <div class="col-md-1">
                        <p><label id="lblDal">Dal:</label></p>
                        <asp:TextBox runat="server" ID="txtDal" class="col-md-11 text-right" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox> 
                    </div> 
                    <div class="col-md-1">
                        <p><label id="lblAl">Al:</label></p>
                        <asp:TextBox runat="server" ID="txtAl" class="col-md-11 text-right" onblur="txtDateLostfocus(this);VerificaData(this);"></asp:TextBox> 
                    </div> 
                    <div class="col-md-3">
                        <p><label id="lblTipo">Tipo Analisi:</label></p>
                        <div class="col-md-4">
                            <asp:RadioButton id="optSintetica" Text=" Sintetica" GroupName="TypeAnalisi" checked="true" runat="server"/>
                        </div> 
                        <div class="col-md-4">
                            <asp:RadioButton id="optAnalitica" Text=" Analitica" GroupName="TypeAnalisi" runat="server"/>
                        </div>
                        <div class="col-md-4">
                            <asp:RadioButton id="optRaffronto" Text=" Raffronto" GroupName="TypeAnalisi" runat="server"/>
                        </div> 
                    </div>
                </div> 
            </div>  
            <div id="Synthetic" class="col-md-12">
                <div class="col-md-12">
                    <div id="divGrdStati" class="col-md-4">
                        <Grd:RibesGridView ID="GrdStati" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="False"
                            OnRowDataBound="GrdStatiRowDataBound">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField HeaderText="Ente" DataField="ENTE">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Stati" DataField="DESCRIZIONE">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.Istanze" DataField="NISTANZE" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.Registrate" DataField="NRegistrate" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.Protocollate" DataField="NProtocollate" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.In Carico" DataField="NInCarico" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.Respinte" DataField="NRespinte" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.Integrazioni" DataField="NIntegrazioni" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="N.Validate" DataField="NValidate" DataFormatString="{0:N0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div id="piechart_div" class="col-md-8" style="height:200px;"></div>
                </div>
                <div class="col-md-12">
                    <div id="divGrdTempi" class="col-md-6">
                        <Grd:RibesGridView ID="GrdTempi" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="False"
                            OnRowDataBound="GrdTempiRowDataBound">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField HeaderText="Ente" DataField="ENTE">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Stati" DataField="DESCRIZIONE">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="GG" DataField="GG" DataFormatString="{0:N2}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Registrate - Inviate al Protocollo" DataField="ToProtocollo" DataFormatString="{0:N2}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Registrate - Prese in carico" DataField="ToCarico" DataFormatString="{0:N2}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Registrate - Respinte" DataField="ToRespinte" DataFormatString="{0:N2}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Registrate - Validate" DataField="ToValidate" DataFormatString="{0:N2}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div id="barchart_div" class="col-md-6" style="height:200px;"></div>
                </div>
            </div>
            <div id="Analytic" class="col-md-12">
                <Grd:RibesGridView ID="GrdAnalytic" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="False">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <FooterStyle CssClass="CartListFooter"></FooterStyle>
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:BoundField HeaderText="Ente" DataField="ENTE">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Justify"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Accessi" DataField="NACCESSI" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Registrazioni" DataField="NREGISTRAZIONI" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Anagrafica" DataField="NANAGRAFICA" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Istanze IMU" DataField="NIMU" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Istanze TASI" DataField="NTASI" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Istanze TARI" DataField="NTARSU" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Istanze OSAP" DataField="NOSAP" DataFormatString="{0:N0}">
                            <headerstyle horizontalalign="Center"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Istanze ICP" DataField="NICP" DataFormatString="{0:N0}">
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


