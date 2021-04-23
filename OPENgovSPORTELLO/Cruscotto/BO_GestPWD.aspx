<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_GestPWD.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.BO_GestPWD" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand" style="height:50px;"><a class="title" id="TitlePage"></a></li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn"> 
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneAccept" OnClick="SetSend" /></li>              
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneExportXLS" OnClick="Print" /></li>              
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
        <div id="divGestPWD" class="col-md-12 pageBO">
            <Grd:RibesGridView ID="GrdGestionePWD" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true"
                OnSorting="GrdGestPWDSorting">
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
                    <asp:BoundField HeaderText="Indirizzo" DataField="Indirizzo" SortExpression="Indirizzo">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="E-mail" DataField="EMail">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:TemplateField HeaderText="Sel.">
                        <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkSel" runat="server"/>
                            <asp:HiddenField ID="hfIdToSend" runat="server" Value='<%# Eval("IDUSER") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
        </div>
        <div id="divUserNoConfirmed" class="col-md-12 pageBO">
            <Grd:RibesGridView ID="GrdUserNoConfirmed" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="False" AllowSorting="true"
                OnRowDataBound="GrdUserNoConfirmedRowDataBound" OnRowCommand="GrdUserNoConfirmedRowCommand" OnSorting="GrdUserNoConfirmedSorting">
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
                                            <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneDelete" CausesValidation="False" CommandName="RowDel" CommandArgument='<%# Eval("ID") %>'/><p id="Elimina" class="col-md-12 TextCmdBlack"></p></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Username" DataField="NameUser" SortExpression="NameUser">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Cod.Fiscale/P.IVA" DataField="CFPIVA">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Data Registrazione" DataField="LastPasswordChangedDate" SortExpression="LastPasswordChangedDate">
                        <headerstyle horizontalalign="Center"></headerstyle>
                        <itemstyle horizontalalign="Justify"></itemstyle>
                    </asp:BoundField>
                </Columns>
            </Grd:RibesGridView>
        </div>
        <label id="OnlyNumber_error" class="text-danger usain"></label>
        <div id="divDownloadXLS" class="col-md-12">
            <button id="btnDownload" class="SubmitBtn"><i class="fa fa-download"></i> Scarica il file</button>
        </div>
     </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>

