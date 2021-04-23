<%@ Page Title="Dichiarazioni" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="FO_Base.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.FO_Base" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand text-white">Benvenuto/a &ensp;</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
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
        <div class="col-md-12 pageFO">
            <p>Selezionando una delle opzioni sotto proposte, è possibile accedere alla sezione specifica del tributo per verificare i dati conosciuti dall'Ufficio Tributi, fare dichiarazioni e stampare modelli di pagamento.</p>
            <div id="ICI" class="col-md-6">
                &ensp;<a runat="server" id="lnk8852Btn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label>Imposta Municipale Propria <u><a runat="server" id="lnk8852Descr">(IMU)</a></u></label>
                <a class="ICI btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div id="TASI" class="col-md-6">
                &ensp;<a runat="server" id="lnkTASIBtn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label>Tassa sui Servizi Indivisibili <u><a runat="server" id="lnkTASIDescr">(TASI)</a></u></label>
                <a class="TASI btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div id="TARSU" class="col-md-6">
                &ensp;
                <a runat="server" id="lnk0434Btn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label>Tassa sui rifiuti <u><a runat="server" id="lnk0434Descr">(TARI)</a></u></label>
                &ensp;<a class="TARSU btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div id="TESSERE" class="col-md-6">
                &ensp;
                <a runat="server" id="lnkTESSBtn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label><a runat="server" id="lnkTESSDescr">Tessere </a></label>
                &ensp;<a class="TESSERE btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div id="OSAP" class="col-md-6">
                &ensp;
                <a runat="server" id="lnk0453Btn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label>Tassa sull'Occupazione Suolo Pubblico <u><a runat="server" id="lnk0453Descr">(OSAP)</a></u></label>
                &ensp;<a class="OSAP btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div id="ICP" class="col-md-6">
                &ensp;
                <a runat="server" id="lnk9763Btn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label>Imposta sulla Pubblicità <u><a runat="server" id="lnk9763Descr">(ICP)</a></u></label>
                &ensp;<a class="ICP btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div id="PROVVEDIMENTI" class="col-md-6">
                &ensp;
                <a runat="server" id="lnk9999Btn" class="nav navbar-nav Bottone BottoneGoTo"></a>
                <label>Accertamenti <u><a runat="server" id="lnk9999Descr">(Accertamenti)</a></u></label>
                &ensp;<a class="PROVVEDIMENTI btn btn-default hidebotton">Vai &raquo;</a>
            </div>
            <div class="col-md-12 hidden">
                <p class="lead_con_barra">Situazione debito/credito verso il comune di&ensp;<asp:Label ID="lblnomeente" runat="server" CssClass="lead"></asp:Label></p>
                <div class="col-md-6">
                    <p>Totale Dovuto&ensp;<asp:Label ID="LblImpDovuto" runat="server" CssClass="usain text-right" Width="80px"></asp:Label> €</p>
                    <p>Totale Pagato&ensp;<asp:Label ID="LblImpPagato" runat="server" CssClass="usain text-right" Width="80px"></asp:Label> €</p>
                    <p>Totale Insoluto&ensp;<asp:Label ID="LblImpInsoluto" runat="server" CssClass="usain text-right" Width="80px"></asp:Label> €</p>
                </div>
                <div id="chart_div" class="col-md-6" style="height:150px;"></div>
            </div>
            <div class="col-md-12">
                <div id="divDichToSend" class="col-md-3">
                    <div class="lead_con_barra col-md-12">
                        Dichiarazioni da inviare
                        <a class="tooltip">
                            <img src="#" class="BottoneMini BottoneHelpMini HelpFOTributiDichToSend" />
                            <span>
                                <iframe class="HelpFOTributiDichToSend"></iframe>
                            </span>
                        </a>         
                    </div>
                   <Grd:RibesGridView ID="GrdDichiarazioni" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        OnRowDataBound="GrdDichiarazioniRowDataBound" OnRowCommand="GrdDichiarazioniRowCommand">
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="UIOpen" CommandArgument='<%# Eval("IDIstanza") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# FncGrd.FormattaNDichIstanza(Eval("DESCRTRIBUTO"),Eval("DescrTipo")) %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Data di Registrazione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DATAPRESENTAZIONE")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            </Columns>
                    </Grd:RibesGridView>
                </div>
                <div class="col-md-1"></div>
                <div id="divComunicazioni" class="col-md-8">
                    <div class="lead_con_barra col-md-12">
                        Quì trovi le comunicazioni che ti ha inviato l'ufficio tributi
                        <a class="tooltip hidden">
                            <img src="" class="BottoneMini BottoneHelpMini HelpFOComunicazioni" />
                            <span>
                                <iframe style="width:800px;height:480px;"></iframe>
                            </span>
                        </a>         
                    </div>
                    <div class="divNews panel-info">
                        <span id="News"></span>
                    </div>
                    <Grd:RibesGridView ID="GrdComunicazioni" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False" PageSize="20"
                        OnRowDataBound="GrdDichiarazioniRowDataBound" OnRowCommand="GrdDichiarazioniRowCommand">
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="UIOpen" CommandArgument='<%# Eval("IDIstanza") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRIZIONE" HeaderText="Testo">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                                <asp:TemplateField HeaderText="Data di Presentazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DATAPRESENTAZIONE")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                    </Grd:RibesGridView>
                </div>
             </div>
            <div class="col-md-12">
                <p class="lead_con_barra col-md-12"></p>
                <p class="col-md-12 usain">Dalle frecce di accesso allo specifico Tributo puoi, ad esempio:</p>
                <div class="col-md-4">
                    <div class="col-md-2"><input type="image" class="ArgImg imgBaseModuli imgBaseModuliSmall imgBaseModuliDelete" src="../Images/Recyclebin-icon.png" /></div>
                    <div class="col-md-05"></div>
                    <div class="col-md-9 text-justified text-11"><p>Se devi comunicare una variazione (es. hai ampliato la superficie di casa tua) usa questo sportello per fare la dichiarazione TARI.</p></div>
                </div>
                <div class="col-md-4">
                    <div class="col-md-2"><input type="image" class="ArgImg imgBaseModuli imgBaseModuliSmall imgBaseModuliHome" src="../Images/Home-icon.png" /></div>
                    <div class="col-md-05"></div>
                    <div class="col-md-9 text-justified text-11"><p>Se devi fare il calcolo IMU, stampare il modello F24 o presentare la relativa dichiarazione usa questo sportello.</p></div>
                </div>
                <div class="col-md-4">
                    <div class="col-md-2"><input type="image" class="ArgImg imgBaseModuli imgBaseModuliSmall imgBaseModuliCar" src="../Images/Car-icon.png" /></div>
                    <div class="col-md-05"></div>
                    <div class="col-md-9 text-justified text-11"><p>Se hai un passo carrabile o hai iniziato un'attività con utilizzo di suolo pubblico, usa questo sportello per fare la tua dichiarazione.</p></div>
                </div>
            </div>
            <br /><br />
        </div>
    </div>
    <asp:Button ID="CmdReadNews" runat="server" Text="" CssClass="hidden" OnClientClick="return FireServerSideClick()" OnClick="ReadNews"></asp:Button>
    <asp:HiddenField ID="hfIdNews" runat="server" Value="-1" />
    <asp:HiddenField ID="hfIdGenNews" runat="server" Value="-1" />
    <asp:HiddenField ID="hfTributoNews" runat="server" Value="" />
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>