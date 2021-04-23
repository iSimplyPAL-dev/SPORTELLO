<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="FO_GestDich.aspx.cs" Inherits="OPENgovSPORTELLO.Istanze.FO_GestDich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand">Gestione Dichiarazioni</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottonePDF"></asp:Button></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottonePrint" OnClick="PrintDichiarazione"></asp:Button></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back"/></li>
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
                <div class="col-md-12 lead_con_barra">
                    <label id="lblHeadIstanza" class="col-md-2">Dati Dichiarazione</label>
                    <ul class="nav navbar-nav lead-right-btn">
                        <li>
                            <div class="BottoneDiv">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneMailBox" OnClick="SendDichiarazione"/>
                                <p id="SendDich" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="col-md-12">
                    <label id="lblDataPresentazione" class="col-md-3"></label>
                    <label id="lblDescrTributo" class="col-md-2"></label>
                </div>
                <div class="col-md-12">            
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="RowOpen" CommandArgument='<%# Eval("IDIstanza") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRTIPO" HeaderText="Tipo Istanza">
                                <headerstyle horizontalalign="Center" Width="200px"></headerstyle>
                                <itemstyle horizontalalign="Justify" Width="200px"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="RIFCAT" HeaderText="Fg.-Num.-Sub.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>                         
                            <asp:TemplateField HeaderText="Motivazioni">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label id="lblMotivi" runat="server" Text='<%# FncGrd.FormattaListMotivazioni(Eval("ListMotivazioni")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField HeaderText="Allegati">
                                <HeaderStyle HorizontalAlign="Center" Width="50px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label id="lblHasAllegati" Width="50px" runat="server" Text='<%# FncGrd.FormattaPresenzaAllegati(Eval("ListAllegati")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRSTATO" HeaderText="Stato" Visible="false">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Stato" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image id="imgStatoRow" runat="server" CssClass='<%# FncGrd.FormattaCSSStato(Eval("IMGSTATO")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
                <div class="col-md-12">
                    <div class="col-md-12 lead_con_barra">
                        <label class="col-md-12">Allegati e comunicazioni</label>
                    </div>
                        <div class="col-md-12">
                            <p>Quì puoi scrivere una nota per il comune e <strong>devi</strong> allegare la dichiarazione firmata.</p><br />
                            <p><label id="lblMotivazione" class="lead">Note/Comunicazioni:</label></p>
                            <asp:TextBox ID="txtNote" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
                        </div>
                        <div class="col-md-12">
                            <div id="FileToUpload">
                                <asp:FileUpload ID="MIOfileUpload" runat="server" AllowMultiple="true" />
                                <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="MIOfileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                            </div>
                            <div id="divAllegati" runat="server"></div>
                        </div>
                </div>
            </div>
        </div>
        <div id="divRiepilogo" class="col-md-12">
            <div class="col-md-12">
                <p>da stampare e conservare</p>
                <p class="lead_con_barra">Ricevuta di Dichiarazione</p>
            </div>
            <div id="divRiepBody" runat="server" class="col-md-12"></div>
            <div id="editor"></div>
        </div>
    </div>    
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
