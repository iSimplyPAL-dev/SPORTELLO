<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Riepilogo.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.TASI.Riepilogo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;Consultazione TASI</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottonePDF" OnClick="PrintPDF"></asp:Button></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>
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
        <div id="divBase" class="col-md-12 pageFO">
            <label id="lblErrorFO" class="text-danger usain"></label>
            <div class="col-md-12">
                <div class="col-md-12 text-justified">
                    Quì puoi prendere visione dei dati conosciuti dall'Ufficio Tributi del tuo Comune, fare dichiarazioni, fare il calcolo del dovuto TASI anno in corso e stampare il modello F24 per il pagamento.
                </div>
            </div>
            <div id="divCalcolo" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <div class="col-md-4 lead_header">Situazione calcolo</div>
                    <ul class="nav navbar-nav lead-right-btn">
                        <li>
                            <div class="BottoneDiv">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneInbox" OnClick="PrintDichiarazione"></asp:Button>
                                <p id="PrintDich" class="TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDiv">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneF24" OnClick="PrintF24"></asp:Button>
                                <p id="F24" class="TextCmdBlack"></p>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="col-md-12">
                    <div id="lblForewordCalc" class="col-md-11"></div>
                    <ul class="nav navbar-nav lead-right-btn">
                        <li>
                            <div class="BottoneDiv">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn BottoneGrd BottoneNewGrd" OnClick="IstanzaNew" />
                                <p id="NuovaUI" class="TextCmdBlack"></p>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="col-md-12">        
                    <Grd:RibesGridView ID="GrdCalcolo" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false" 
                        OnRowDataBound="GrdCalcoloRowDataBound" OnRowCommand="GrdCalcoloRowCommand">
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="UIOpen" CommandArgument='<%# Eval("ID") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UBICAZIONE" HeaderText="Via">
                                <headerstyle horizontalalign="Center" Width="200px"></headerstyle>
                                <itemstyle horizontalalign="Justify" Width="200px"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FOGLIO" HeaderText="Fg.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NUMERO" HeaderText="Num.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SUB" HeaderText="Sub.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="RENDITAVALORE" HeaderText="Rendita/Valore" DataFormatString="{0:N}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Dal">
                                <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDal" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Al">
                                <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAl" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRUTILIZZO" HeaderText="Utilizzo">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="IMPDOVUTO" HeaderText="Dovuto" DataFormatString="{0:N}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <label id="lblTotDovuto" class="col-md-12 text-right usain text-success"></label>
                </div>
            </div>
        </div>
        <div id="divRiepilogo" class="col-md-12 pageFO">
            <div class="col-md-12">
                <p class="lead_con_barra">Ricevuta di Istanza</p>
                <p>La tua dichiarazione online è stata elaborata, puoi stamparla tramite il pulsante <b>&lsquo;PDF&rsquo;</b> che trovi in alto a destra.<br />
                    Ricordati che dopo averla stampata e firmata dovrai inviarla al Comune tramite l’apposita funzione che trovi nella pagina <a href="" class="Tributi usain">&lsquo;Tributi&rsquo;</a> nella sezione <b><i>&lsquo;dichiarazioni da inviare&rsquo;</i></b>.</p>
            </div>
            <div id="divRiepBody" runat="server" class="col-md-12 hidden"></div>
            <embed id="myEmbedPDF" src="" width="900" height="750" alt="pdf" pluginspage="http://www.adobe.com/products/acrobat/readstep2.html">
        </div>
        <div id="divF24" class="col-md-12 pageFO">
            <div class="col-md-12">
                <asp:RadioButton ID="optAcconto" runat="server" GroupName="TipoF24" Text="Acconto" AutoPostBack="true" OnCheckedChanged="TipoF24CheckedChanged" />
                <asp:RadioButton ID="optSaldo" runat="server" GroupName="TipoF24" Text="Saldo" AutoPostBack="true" OnCheckedChanged="TipoF24CheckedChanged" />
                <asp:RadioButton ID="optUS" runat="server" GroupName="TipoF24" Text="Soluzione Unica" AutoPostBack="true" OnCheckedChanged="TipoF24CheckedChanged" />
            </div>
            <div id="divBodyF24" runat="server" class="col-md-12"></div>
        </div>
    </div>    
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <asp:HiddenField ID="hdTypePDF" runat="server" />
</asp:Content>
