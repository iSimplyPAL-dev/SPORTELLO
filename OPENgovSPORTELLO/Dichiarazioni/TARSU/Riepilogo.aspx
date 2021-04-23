<%@ Page Title="Consultazione TARI" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Riepilogo.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.TARSU.Riepilogo" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;Consultazione TARI</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottonePDF" OnClick="PrintPDF"></asp:Button></li>
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
            <div class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <div class="col-md-4 lead_header">Situazione dichiarazioni</div>
                    <ul class="nav navbar-nav lead-right-btn">
                        <li>
                            <div class="BottoneDiv">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneInbox" OnClick="PrintDichiarazione"></asp:Button>
                                <p id="PrintDich" class="TextCmdBlack"></p>
                            </div>
                        </li>
                    </ul>
                    <div class="col-md-3 lead text-right hidden">
                        <label><asp:Button ID="StoricoDich" runat="server" Text="" CssClass="Storico nav navbar-nav Bottone BottoneGoTo" OnClick="Storico"></asp:Button>Consulta Storico</label>                        
                    </div>
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
                    <Grd:RibesGridView ID="GrdUI" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false"
                        OnRowDataBound="GrdUIRowDataBound" OnRowCommand="GrdUIRowCommand">
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
                                                    <%--<li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneAlter" CausesValidation="False" CommandName="IstanzaAlter" CommandArgument='<%# Eval("IDRifOrg") %>' /><p id="variazione" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneUpdate" CausesValidation="False" CommandName="IstanzaUpdate" CommandArgument='<%# Eval("IDRifOrg") %>' /><p id="correggi" class="TextCmdGrd"></p></li><br>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneAttention" CausesValidation="False" CommandName="IstanzaInagibile" CommandArgument='<%# Eval("IDRifOrg") %>' /><p id="inagibile" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneWarning" CausesValidation="False" CommandName="IstanzaInutilizzo" CommandArgument='<%# Eval("IDRifOrg") %>' /><p id="inutilizzato" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneClose" CausesValidation="False" CommandName="IstanzaClose" CommandArgument='<%# Eval("IDRifOrg") %>' /><p id="cessazione" class="TextCmdGrd"></p></li>--%>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hfStato" runat="server" Value='<%# Eval("STATO") %>' />
                                    <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stato" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image id="imgStatoUI" runat="server" CssClass='<%# FncGrd.FormattaCSSStato(Eval("STATO")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UBICAZIONE" HeaderText="Via">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FOGLIO" HeaderText="Foglio">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NUMERO" HeaderText="Numero">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SUB" HeaderText="Sub.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
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
                            <asp:BoundField DataField="MQ" HeaderText="MQ">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="RIDESE" HeaderText="Riduzione/Esenzione **">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br />
                    <div id="divListUIRidEse" runat="server" class="col-md-12 panel panel-primary"></div>
                </div>
            </div>
            <div class="col-md-12 panel panel-primary hidden">
                <div class="col-md-12">
                    <p class="label">Legenda</p>
                </div>
                <div>
                    <div class="col-md-2">
                        <p><input class="div_StatoWA" />Istanza presentata</p>
                    </div>
                    <div class="col-md-5">
                        <p><input class="div_StatoOK" />Quanto risulta in banca dati comunale e/o istanza validita</p>
                    </div>
                    <div class="col-md-3">
                        <p><input class="div_StatoML" />Istanza in lavorazione dal comune</p>
                    </div>
                    <div class="col-md-2">
                        <p><input class="div_StatoKO" />Istanza respinta</p>
                    </div>
                </div>
                &nbsp;
            </div>
            <div class="col-md-12 panel panel-primary hidden"><p class="label">Stato generale della situazione dichiarata</p>&ensp;&ensp;&ensp;<input style="height:50px" id="imgStato" runat="server" class="div_StatoKO" /></div>                
            <div id="divDich" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <label class="col-md-9">Situazione banca dati Comunale</label>
                </div>
                <div class="col-md-12">
                    <div id="lblForewordDich" class="col-md-11"></div>
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
                            <asp:TemplateField HeaderText="Stato" Visible="false">
                                <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image id="imgStatoUI" runat="server" CssClass='<%# FncGrd.FormattaCSSStato(Eval("STATO")) %>' />
                                    <asp:HiddenField ID="hfIdRifOrg" runat="server" Value='<%# Eval("IDRifOrg") %>' />
                                    <asp:HiddenField ID="hfIdUI" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="UBICAZIONE" HeaderText="Via">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="FOGLIO" HeaderText="Foglio">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="NUMERO" HeaderText="Numero">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="SUB" HeaderText="Sub.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
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
                            <asp:BoundField DataField="MQ" HeaderText="MQ">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="RIDESE" HeaderText="Riduzione/Esenzione **">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <br />
                    <div id="divListDichRidEse" runat="server" class="col-md-12 panel panel-primary"></div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <div class="col-md-12 lead_header">Situazione Dovuto</div>                   
                </div>
                <div id="lblForewordDovuto" class="col-md-12"></div>
                <label id="lblResultDovuto">Non risultano avvisi di pagamenti emessi</label>
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
                        <asp:TemplateField HeaderText="F24">
                            <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <div id="GrdBtnCol">
                                    <asp:CheckBox ID="chkSel" runat="server"/>
                                    <div class="divGrdBtn">
                                        <div class="panel panelGrd">
                                            <ul class="nav navbar-nav">
                                                <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneF24" CausesValidation="False" CommandName="UIOpen" CommandArgument='<%# Eval("ANNO") %>'/><p id="F24" class="TextCmdGrd"></p></li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ANNO" HeaderText="Anno">
                            <headerstyle horizontalalign="Center" Width="50px"></headerstyle>
                            <itemstyle horizontalalign="Justify" Width="50px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="FISSA" HeaderText="Fissa €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="VARIABILE" HeaderText="Variabile €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="IMPOSTA" HeaderText="Totale Imposta €">
                            <headerstyle horizontalalign="Center" Width="140px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="140px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PROVINCIALE" HeaderText="Provinciale €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="MAGGIORAZIONE" HeaderText="Maggiorazione €" Visible="false">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="ARROTONDAMENTO" HeaderText="Arrotondamento €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DOVUTO" HeaderText="Dovuto €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="VERSATO" HeaderText="Versato €">
                            <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                            <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                        </asp:BoundField>                               
                    </Columns>
                </Grd:RibesGridView>
            </div>
        </div>
        <div id="divRiepilogo" class="col-md-12 pageFO">
            <div class="col-md-12">
                <p class="lead_con_barra">Ricevuta di Istanza</p>
                <p>La tua dichiarazione online è stata elaborata, puoi stamparla tramite il pulsante <b>&lsquo;PDF&rsquo;</b> che trovi in alto a destra.<br />
                    Ricordati che dopo averla stampata e firmata dovrai inviarla al Comune tramite l’apposita funzione che trovi nella pagina <a href="" class="Tributi usain">&lsquo;Tributi&rsquo;</a> nella sezione <b><i>&lsquo;dichiarazioni da inviare&rsquo;</i></b>.</p>
            </div>
            <div id="divRiepBody" runat="server" class="col-md-12 hidden"></div>
            <!--<iframe src="http://docs.google.com/gview?url=http://localhost:59706/HTMLtoPDF/DICH_TARI_BENUCCI_ENGELSE_BNCNLS32B05G804F.pdf&embedded=true" style="width:600px; height:500px;" frameborder="0"></iframe>-->
            <embed id="myEmbedPDF" src="" width="900" height="750" alt="pdf" pluginspage="http://www.adobe.com/products/acrobat/readstep2.html">
        </div>        
        <div id="divF24" class="col-md-12 pageFO">
            <div id="divF24Rate" class="col-md-12">
                <asp:RadioButton runat="server" ID="opt1" GroupName="TipoF24" Text="Prima Rata" Checked="true" AutoPostBack="true" OnCheckedChanged="TipoF24CheckedChanged" />
                <asp:RadioButton runat="server" ID="opt2" GroupName="TipoF24" Text="Seconda Rata" AutoPostBack="true" OnCheckedChanged="TipoF24CheckedChanged" />
                <asp:RadioButton runat="server" ID="optU" GroupName="TipoF24" Text="Soluzione Unica" AutoPostBack="true" OnCheckedChanged="TipoF24CheckedChanged" />
            </div>
            <div id="divBodyF24" runat="server" class="col-md-12"></div>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <asp:HiddenField ID="hfTypePDF" runat="server" />
    <asp:HiddenField ID="hfF24Rata" runat="server" />
</asp:Content>
