<%@ Page Title="Paga" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="FO_PayGen.aspx.cs" Inherits="OPENgovSPORTELLO.Paga.FO_PayGen" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand">Paga</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneShoppingCart" OnClick="Paga" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" /></li>
           </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div class="container">
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
            <div class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <div class="col-md-12 lead_header">Situazione Dovuto</div>                   
                </div>
                <div id="lblForeword" class="col-md-12 text-11 text-italic" style="margin-top: 30px;"></div>
                <label id="lblResultDovuto" class="col-md-12 text-danger usain">Non risultano avvisi da pagare</label>
                <Grd:RibesGridView ID="GrdDovuto" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="80%"
                    AutoGenerateColumns="False" AllowPaging="false" PageSize="20" HoverRowCssClass="riga_tabella_mouse_over"
                    OnRowDataBound="GrdDovutoRowDataBound" OnRowCommand="GrdDovutoRowCommand">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <FooterStyle CssClass="CartListFooter"></FooterStyle>
                    <RowStyle CssClass="CartListItem"></RowStyle>
                    <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                    <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Paga">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <div id="GrdBtnCol">
                                    <asp:CheckBox ID="chkSel" runat="server"/>
                                    <div class="divGrdBtn">
                                        <div class="panel panelGrd">
                                            <ul class="nav navbar-nav">
                                                <li><asp:ImageButton runat="server" CssClass="SubmitBtn BottoneGrd BottoneAddGrd" CausesValidation="False" CommandName="RowAdd" CommandArgument='<%# Eval("id") %>'/><p id="AddCart" class="TextCmdGrd"></p></li>
                                                <li><asp:ImageButton runat="server" CssClass="SubmitBtn BottoneGrd BottoneDelGrd" CausesValidation="False" CommandName="RowDel" CommandArgument='<%# Eval("id") %>'/><p id="DelCart" class="TextCmdGrd"></p></li>
                                                <asp:HiddenField ID="hfId" runat="server" Value='<%# Eval("id") %>' />
                                                <asp:HiddenField ID="hfDebito" runat="server" Value='<%# Eval("IMPDEBITO") %>' />
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="id" HeaderText="Identificativo">
                            <headerstyle horizontalalign="Center" Width="150px"></headerstyle>
                            <itemstyle horizontalalign="Justify" Width="150px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno">
                            <headerstyle horizontalalign="Center" Width="50px"></headerstyle>
                            <itemstyle horizontalalign="Justify" Width="50px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="descrtributo" HeaderText="Tributo">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Justify" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Scadenza">
                            <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                            <ItemTemplate>
                                <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("scadenza")) %>' CssClass="text-right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="IMPDOVUTO" HeaderText="Dovuto €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IMPVERSATO" HeaderText="Versato €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>  
                        <asp:BoundField DataField="IMPDEBITO" HeaderText="Debito €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>                             
                    </Columns>
                </Grd:RibesGridView>
            </div>
            <div id="divCart" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <div class="col-md-12 lead_header">Carrello</div>                   
                </div>
                <div class="col-md-12">
                    <div id="lblListCart" class="col-md-12"></div>
                    <div id="lblTotCart" class="col-md-12 text-right usain text-success"></div>
                </div>
            </div>
        </div>
    </div>    
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <asp:HiddenField ID="hdTypePDF" runat="server" />
</asp:Content>
