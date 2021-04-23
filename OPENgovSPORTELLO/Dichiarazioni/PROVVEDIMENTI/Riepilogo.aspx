<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Riepilogo.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Riepilogo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;Consultazione Accertamenti</p>
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
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="body_page">
        <div class="jumbotronAnag"></div>
        <label id="OnlyNumber_error" class="text-danger usain"></label>
        <div id="divBase" class="col-md-12 pageFO">
            <div id="divDich" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <label class="col-md-9">Situazione banca dati Comunale</label>
                </div>
                <div class="col-md-12">         
                    <Grd:RibesGridView ID="GrdDich" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false"
                        OnRowDataBound="GrdDichRowDataBound" OnRowCommand="GrdDichRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="UIOpen" CommandArgument='<%# Eval("IDRifOrg") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ZONA" HeaderText="Anno">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="UBICAZIONE" HeaderText="Tipo">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Notifica">
                                <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IMPDOVUTO" HeaderText="Dovuto">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="RENDITAVALORE" HeaderText="Pagato">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="STATO" HeaderText="Stato">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                         </Columns>
                    </Grd:RibesGridView>
                </div>
            </div>
        </div>
    </div>    
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
