<%@ Page Title="Istanze" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="FO_IstanzeGen.aspx.cs" Inherits="OPENgovSPORTELLO.Istanze.FO_IstanzeGen" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand">Istanze</p>
            <ul class="nav navbar-nav navbar-right-btn">
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
        <div id="divRic" class="col-md-12 pageFO">
            <Grd:RibesGridView ID="GrdIstanze" runat="server" BorderStyle="None" 
                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                AutoGenerateColumns="False" AllowPaging="false"
                OnRowDataBound="GrdIstanzeRowDataBound" OnRowCommand="GrdIstanzeRowCommand">
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
                                            <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="RowOpen" CommandArgument='<%# Eval("IDIstanza") %>'/><p id="apri"></p></li>
                                            <asp:HiddenField ID="hfIDIstanza" runat="server" Value='<%# Eval("IDIstanza") %>' />
                                            <asp:HiddenField ID="hfTributo" runat="server" Value='<%# Eval("IDTributo") %>' />
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="DESCRTRIBUTO" HeaderText="Tributo">
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
                    <asp:TemplateField HeaderText="Registrazione">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataPresentazione")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Inviata">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataInvioDichiarazione")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Accettata">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataAccettazione")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rich.Integrazioni">
                        <headerstyle horizontalalign="Center" Width="140px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="140px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataIntegrazioni")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="In carico">
                        <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataInCarico")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Validata">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataValidata")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Respinta">
                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                        <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                        <ItemTemplate>
                            <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DataRespinta")) %>' CssClass="text-right"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stato">
                        <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:Image id="imgStatoRow" runat="server" CssClass='<%# FncGrd.FormattaCSSStato(Eval("IMGSTATO")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </Grd:RibesGridView>
            <div class="col-md-12 panel panel-primary">
                <div class="col-md-12">
                    <p class="label col-md-2">Legenda</p>
                    <div class="col-md-3">
                        <p><input class="div_StatoWA" />Istanza presentata/accettata</p>
                    </div>
                    <div class="col-md-3">
                        <p><input class="div_StatoOK" />Istanza validata</p>
                    </div>
                    <div class="col-md-3">
                        <p><input class="div_StatoKO" />Istanza respinta</p>
                    </div>
                </div>
                <div>    
                    <div class="col-md-2"></div>
                    <div id="bandieraGialla" class="col-md-3">
                        <p><input class="div_StatoML" />Istanza in lavorazione dal comune</p>
                    </div>
                    <div class="col-md-7">
                        <p id="testoI" class="blink_slow">Quando, oltre alla bandierina gialla è presente una data nella colonna <b>&lsquo;Richiesta integrazioni&rsquo;</b>, clicca sulla casella posta alla sinistra dell'istanza (colonna <b>&lsquo;Sel.&rsquo;</b>) per visualizzare cosa ti segnala l'Ente.</p>
                        <p id="testoE" class="blink_slow">E' presente una data nella colonna <b>&lsquo;Richiesta integrazioni&rsquo;</b>, clicca sulla casella posta alla sinistra dell'istanza (colonna <b>&lsquo;Sel.&rsquo;</b>) per visualizzare cosa ti segnala l'Ente.</p>                    
                    </div>
                </div>
                &nbsp;
            </div>
        </div>
    </div>    
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>