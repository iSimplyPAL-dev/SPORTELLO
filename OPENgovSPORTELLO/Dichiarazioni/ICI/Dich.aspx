<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Dich.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.ICI.Dich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p id="PageTitle" class="navbar-brand"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;<a class="ICI text-white" id="TitlePage">Consultazione IMU</a>&ensp;-&ensp;Immobile</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header">
                    <asp:Button runat="server" ID="CmdSave" Text="" CssClass="Bottone BottoneSave" OnClientClick="return FieldValidatorICI();" OnClick="Save" /></li>
                <li class="bottoni_header">
                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" /></li>
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
    <div id="MenuBO" class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeBO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeBO nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="Configurazioni nav navbar-nav Bottone BottoneTools"></a></li>
            <li><a class="Configurazioni nav navbar-nav">Configurazioni</a></li>
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
    <div class="body_page">
        <div class="jumbotronAnag"></div>
        <div id="SearchStradario" class="modal" style="width: 100%; height: 100%">
            <div class="modal-dialog">
                <div class="modal-content" style="padding: 10px 10px 10px 10px">
                    <div class="panel panel-primary" style="height: 100px">
                        <p class="lead">Ricerca Stradario</p>
                        <div class="col-md-9">
                            <label>Via</label>
                            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSearch" OnClick="SearchStradario" />&ensp;
                            <asp:Button runat="server" ID="CmdBackSearchStradario" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                        </div>
                    </div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdStradario" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdStradarioRowCommand" OnPageIndexChanging="GrdStradarioPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Via">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStradario" Text='<%#Eval("DESCRIZIONE") %>'></asp:Label>
                                        <asp:HiddenField runat="server" ID="hdIdStradario" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgAtt" runat="server" CssClass="SubmitBtn BottoneGrd BottoneAttachGrd" CausesValidation="False" CommandName="AttachRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                        &nbsp;
                    </div>
                    &nbsp;
                </div>
                &nbsp;
            </div>
        </div>
        <div id="divVincoliGrd" class="modal" style="width: 100%; height: 100%">
            <div class="modal-dialog">
                <div class="modal-content col-md-12" style="padding: 10px 10px 10px 10px">
                    <div class="panel panel-primary col-md-12">
                        <div class="col-md-10">
                            <p class="lead">Vincoli</p>
                        </div>
                        <div class="col-md-2">
                            <asp:Button runat="server" ID="CmdVincoliBack" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                        </div>
                    </div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdVincoli" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <Columns>
                                <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Sel.">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSel" runat="server" Checked='<%# FncGrd.FormattaBoolGrd(Eval("IsActive")) %>'></asp:CheckBox>
                                        <asp:HiddenField ID="hfCodice" runat="server" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                        &nbsp;                                                
                       &nbsp;
                    </div>
                    &nbsp;
                </div>
                &nbsp;
            </div>
        </div>
        <div id="divConfirm" class="modal" style="width: 100%; height: 100%">
            <div class="modal-dialog">
                <div class="modal-content">
                    <p>
                        <label id="lblDescrConfirm" class="usain text-warning"></label>
                        <br />
                    </p>
                    <div>
                        <ul class="nav navbar-nav lead-right-btn">
                            <li>
                                <div>
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneOK" ID="OKCatasto" OnClick="SaveIstanza"></asp:Button>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneOK" ID="OKNew" OnClientClick="return Azzera();"></asp:Button>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneKO" OnClick="Back"></asp:Button>
                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
        <div id="divIstanza" class="col-md-12 pageFO">
            <label id="lblErrorFO" class="text-danger usain"></label>
            <div id="divDatiIstanza" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <a id="lblHeadIstanza" class="col-md-2" onclick="ShowHideDiv(this,'divDetIstanza','Dati Istanza')" href="#">Dati Istanza</a>
                    <ul class="nav navbar-nav lead-right-btn">
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneSort" OnClick="Ribalta" />
                                <p id="Ribalta" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneMailBox" OnClientClick="return FieldValidatorGestIstanze();" OnClick="MailBox" />
                                <p id="Comunicazione" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneStop" OnClientClick="return FieldValidatorGestIstanze();" OnClick="Stop" />
                                <p id="Respingi" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneAccept" OnClick="Accept" />
                                <p id="Valida" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneWork" OnClientClick="return ConfirmProtocollo()" OnClick="Work" />
                                <p id="InCarico" class="col-md-12 TextCmdBlack"></p>
                                <asp:HiddenField runat="server" ID="hfTypeProtocollo" />
                            </div>
                        </li>
                    </ul>
                </div>
                <div id="divDetIstanza" class="col-md-12">
                    <label id="lblDelegIstanza" class="col-md-12 lead_Emphasized"></label>
                    <div class="col-md-12">
                        <label id="lblEnte" class="col-md-2"></label>
                        <label id="lblTipoIstanza" class="col-md-3"></label>
                        <label id="lblDescrTributo" class="col-md-2"></label>
                        <label id="lblDataPresentazione" class="col-md-3"></label>
                        <label id="lblStatoIstanza" class="col-md-2"></label>
                    </div>
                    <div class="col-md-12">
                        <label id="lblMotivi" class="col-md-11"></label>
                    </div>
                    <div class="col-md-9">
                        <Grd:RibesGridView ID="GrdStatiIstanza" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="False" PageSize="20">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Data">
                                    <HeaderStyle HorizontalAlign="Center" Width="100px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Right" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" Width="100px" Text='<%# FncGrd.FormattaDataGrd(Eval("DATA")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DESCRSTATO" HeaderText="Stato">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TESTO" HeaderText="Comunicazione">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-3">
                        <Grd:RibesGridView ID="GrdAllegati" runat="server" BorderStyle="None"
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="False" PageSize="20"
                            OnRowDataBound="GrdAllegatiRowDataBound" OnRowCommand="GrdAllegatiRowCommand">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:TemplateField HeaderText="Scarica">
                                    <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <div id="GrdBtnCol">
                                            <asp:CheckBox ID="chkSel" runat="server" />
                                            <div class="divGrdBtn">
                                                <div class="panel panelGrd">
                                                    <ul class="nav navbar-nav">
                                                        <li>
                                                            <asp:ImageButton runat="server" CssClass="Bottone BottoneAttach" CausesValidation="False" CommandName="RowDownload" CommandArgument='<%# Eval("IDALLEGATO") %>' /><p id="download" class="TextCmdGrd"></p>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FILENAME" HeaderText="Allegato">
                                    <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                    <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                </div>
            </div>
            <div id="divDatiUI" class="col-md-12">
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDetUI','Dati Immobile')" href="#"></a>
                    <div id="lblForewordUI" class="col-md-7 text-11 text-italic" style="margin-top: 30px;"></div>
                    <ul class="nav navbar-nav lead-right-btn">
                        <li>
                            <div class="BottoneDiv">
                                <asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneAttention" OnClick="Inagibile" />
                                <p id="inagibile" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDiv">
                                <asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneUserGroup" OnClick="UsoGratuito" />
                                <p id="usogratuito" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li id="ColCmdClose">
                            <div class="BottoneDiv">
                                <asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneClose" OnClick="Close" />
                                <p id="cessazione" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                    </ul>
                </div>
                <div id="divSuggestFromCatasto" class="col-md-12"></div>
                <div id="divDetUI" class="col-md-12">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <p>
                                <label id="lblVia">Ubicazione - Via:</label>
                                <img class="SubmitBtn BottoneMini BottoneMapMarker" onclick="Scopri('SearchStradario');" />
                                <a href="" target="_blank" class="tooltip HelpFODichICIUbicazione">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichICIUbicazione"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:TextBox runat="server" ID="txtVia" CssClass="Azzera txtVia col-md-10"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblCivico">Civico:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtCivico" CssClass="Azzera col-md-10" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblDataInizio">Data Inizio possesso:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-10 text-right" TabIndex="2"></asp:TextBox>
                            <label id="lblDataInizioORG" runat="server" class="text-italic"></label>
                        </div>
                        <div class="col-md-3">
                            <p>
                                <label id="lblDataFine">Data Fine possesso:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-7 text-right" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-1">
                            <p>
                                <label id="lblFoglio">Foglio:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtFoglio" CssClass="Azzera col-md-10" AutoPostBack="true" OnTextChanged="RifCatChanged" TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblNumero">Numero:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtNumero" CssClass="Azzera col-md-10" AutoPostBack="true" OnTextChanged="RifCatChanged" TabIndex="5"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblSub">Subalterno:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtSub" CssClass="Azzera col-md-10" AutoPostBack="true" OnTextChanged="RifCatChanged" TabIndex="6"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblSIT">Cartografia</label>
                            </p>
                            <p style="margin-left: 20px">
                                <img class="BottoneMini BottoneMap" />
                            </p>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblCat">Categoria:</label>
                                <a href="" target="_blank" class="tooltip HelpFODichICICategoria">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichICICategoria"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:DropDownList ID="ddlCat" runat="server" CssClass="Azzera col-md-11" TabIndex="7"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblClasse">Classe:</label>
                            </p>
                            <asp:DropDownList ID="ddlClasse" runat="server" CssClass="Azzera VincoloVariazione col-md-11" TabIndex="8"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblConsistenza">Consistenza/MQ:</label>
                                <a href="" target="_blank" class="tooltip HelpFODichICIConsistenza">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichICIConsistenza"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:TextBox runat="server" ID="txtConsistenza" CssClass="Azzera VincoloVariazione OnlyNumber text-right col-md-8" TabIndex="9"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblRendita">Rendita/Valore/Reddito D.:</label>
                            </p>
                            <asp:TextBox ID="txtRendita" runat="server" CssClass="Azzera VincoloVariazione OnlyNumber text-right col-md-8" TabIndex="10"></asp:TextBox>
                        </div>
                        <div id="lblInfoRendita" class="col-md-2 text-11 text-italic"></div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <p>
                                <label id="lblPossesso">Possesso:</label>
                            </p>
                            <asp:DropDownList ID="ddlPossesso" runat="server" class="Azzera col-md-11" TabIndex="12"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblPercPos">% Possesso:</label>
                            </p>
                            <asp:TextBox ID="txtPercPos" runat="server" CssClass="Azzera VincoloVariazione OnlyNumber text-right col-md-11" TabIndex="13"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblComproprietari">Visualizza Comproprietari</label>
                            </p>
                            <p style="margin-left: 60px">
                                <img class="BottoneMini BottoneShare" onclick="ShowHideDiv(this,'divComproprietari','Comproprietari');" />
                            </p>
                        </div>
                        <div class="col-md-1">
                            <p>&nbsp;</p>
                            <asp:CheckBox ID="chkStorico" runat="server" Width="100%" Text="Storico" CssClass="Azzera"></asp:CheckBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblNUtilizzatori">Utilizzatori:</label>
                            </p>
                            <asp:TextBox ID="txtNUtilizzatori" runat="server" CssClass="Azzera VincoloVariazione OnlyNumber text-right col-md-11" TabIndex="14"></asp:TextBox>
                        </div>
                        <div id="divPertinenza" class="col-md-5">
                            <div class="col-md-3">
                                <p>
                                    <label id="lblPert">Pertinenza di</label>
                                    <a href="" class="tooltip">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src=""></iframe>
                                        </span>
                                    </a>
                                </p>
                            </div>
                            <div class="col-md-3">
                                <p>
                                    <label id="lblPertFoglio">Foglio:</label>
                                </p>
                                <asp:TextBox runat="server" ID="txtPertFoglio" CssClass="Azzera col-md-10" TabIndex="15"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <p>
                                    <label id="lblPertNumero">Numero:</label>
                                </p>
                                <asp:TextBox runat="server" ID="txtPertNumero" CssClass="Azzera col-md-10" TabIndex="16"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <p>
                                    <label id="lblPertSub">Subalterno:</label>
                                </p>
                                <asp:TextBox runat="server" ID="txtPertSub" CssClass="Azzera col-md-10" TabIndex="17"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div id="divComproprietari" class="col-md-12"></div>
                    <div class="col-md-12">
                        <div class="col-md-10">
                            <p>
                                <label id="lblTipologia">Utilizzo:</label><label id="lblDescrTipologiaORG" runat="server" class="text-italic"></label><asp:HiddenField ID="hfIDTipologiaORG" runat="server" />
                                <a href="" target="_blank" class="tooltip HelpFODichICIUtilizzo">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichICIUtilizzo"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="Azzera VincoloVariazione col-md-11" TabIndex="18"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblPercRid">% Riduzione:</label>
                            </p>
                            <asp:TextBox ID="txtPercRid" runat="server" CssClass="Azzera VincoloVariazione OnlyNumber text-right col-md-11 DefaultPercRid" onblur="isNumber(this.value,3,0,0,100)" TabIndex="19"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblPercEse">% Esenzione:</label>
                            </p>
                            <asp:TextBox ID="txtPercEse" runat="server" CssClass="Azzera VincoloVariazione OnlyNumber text-right col-md-11 DefaultPercEse" onblur="isNumber(this.value,3,0,0,100)" TabIndex="20"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12" id="divVincoliZone">
                        <div class="col-md-12">
                            <p>
                                <label id="lblZona">Zona P.R.G.:</label>
                                <a href="" target="_blank" class="tooltip HelpFODichICIZona">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <b></b>
                                        <iframe src="" class="HelpFODichICIZona"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:DropDownList ID="ddlZona" runat="server" CssClass="Azzera VincoloVariazione col-md-8 InLine" TabIndex="11"></asp:DropDownList>
                            <div id="divVincoli" class="col-md-2">
                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                <p style="display: inline-block">
                                    <label id="lblVincoli">Vincoli:</label>
                                </p>
                                <img class="SubmitBtn BottoneMini BottoneVincoli" onclick="Scopri('divVincoliGrd');" />
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <div id="divMotivazione" class="col-md-12">
                <div class="lead_con_barra col-md-12">
                    <a id="lblHeadMotivazione" onclick="ShowHideDiv(this,'divDetMotivi','Motivazione')" href="#">Motivazione</a>
                </div>
                <div id="divDetMotivi" class="col-md-12">
                    <Grd:RibesGridView ID="GrdMotivazioni" runat="server" BorderStyle="None"
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="false">
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
                                    <asp:CheckBox ID="chkSel" runat="server" />
                                    <asp:HiddenField ID="hfIdMotivazione" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRIZIONE" HeaderText="Motivazione">
                                <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Justify"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <div class="col-md-12">
                        <p>
                            <label id="lblMotivazione">Note aggiuntive di motivazione:</label>
                        </p>
                        <asp:TextBox ID="txtMotivazione" runat="server" TextMode="MultiLine" Width="100%" CssClass="Azzera" TabIndex="21"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkAllegati" runat="server" CssClass="Input_Label_bold" Text="Allego Documentazione" AutoPostBack="true" OnCheckedChanged="GestAllegati" />
                        <div id="FileToUpload">
                            <asp:FileUpload ID="MIOfileUpload" runat="server" AllowMultiple="true" />
                            <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="MIOfileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <!-- The file upload form used as target for the file upload widget -->
                    <div style="display: none" id="fileupload" data-ng-app="demo" data-ng-controller="DemoFileUploadController" data-file-upload="options" data-ng-class="{'fileupload-processing': processing() || loadingFiles}">
                        <!-- Redirect browsers with JavaScript disabled to the origin page -->
                        <noscript><input type="hidden" name="redirect" value="https://blueimp.github.io/jQuery-File-Upload/" /></noscript>
                        <!-- The fileupload-buttonbar contains buttons to add/delete files and start/cancel the upload -->
                        <div class="row fileupload-buttonbar">
                            <div class="col-lg-7">
                                <!-- The fileinput-button span is used to style the file input field as button -->
                                <span class="btn btn-success fileinput-button" ng-class="{disabled: disabled}">
                                    <i class="glyphicon glyphicon-plus"></i>
                                    <span>Add files...</span>
                                    <input type="file" name="files[]" multiple ng-disabled="disabled">
                                </span>
                                <button type="button" class="btn btn-primary start" data-ng-click="submit()">
                                    <i class="glyphicon glyphicon-upload"></i>
                                    <span>Start upload</span>
                                </button>
                                <button type="button" class="btn btn-warning cancel" data-ng-click="cancel()">
                                    <i class="glyphicon glyphicon-ban-circle"></i>
                                    <span>Cancel upload</span>
                                </button>
                                <!-- The global file processing state -->
                                <span class="fileupload-process"></span>
                            </div>
                            <!-- The global progress state -->
                            <div class="col-lg-5 fade" data-ng-class="{in: active()}">
                                <!-- The global progress bar -->
                                <div class="progress progress-striped active" data-file-upload-progress="progress()">
                                    <div class="progress-bar progress-bar-success" data-ng-style="{width: num + '%'}"></div>
                                </div>
                                <!-- The extended global progress state -->
                                <div class="progress-extended">&nbsp;</div>
                            </div>
                        </div>
                        <!-- The table listing the files available for upload/download -->
                        <table class="table table-striped files ng-cloak">
                            <tr data-ng-repeat="file in queue" data-ng-class="{'processing': file.$processing()}">
                                <td data-ng-switch data-on="!!file.thumbnailUrl">
                                    <div class="preview" data-ng-switch-when="true">
                                        <a data-ng-href="{{file.url}}" title="{{file.name}}" download="{{file.name}}" data-gallery>
                                            <img data-ng-src="{{file.thumbnailUrl}}" alt=""></a>
                                    </div>
                                    <div class="preview" data-ng-switch-default data-file-upload-preview="file"></div>
                                </td>
                                <td>
                                    <p class="name" data-ng-switch data-on="!!file.url">
                                        <span data-ng-switch-when="true" data-ng-switch data-on="!!file.thumbnailUrl">
                                            <a data-ng-switch-when="true" data-ng-href="{{file.url}}" title="{{file.name}}" download="{{file.name}}" data-gallery>{{file.name}}</a>
                                            <a data-ng-switch-default data-ng-href="{{file.url}}" title="{{file.name}}" download="{{file.name}}">{{file.name}}</a>
                                        </span>
                                        <span data-ng-switch-default>{{file.name}}</span>
                                    </p>
                                    <strong data-ng-show="file.error" class="error text-danger">{{file.error}}</strong>
                                </td>
                                <td>
                                    <p class="size">{{file.size | formatFileSize}}</p>
                                    <div class="progress progress-striped active fade" data-ng-class="{pending: 'in'}[file.$state()]" data-file-upload-progress="file.$progress()">
                                        <div class="progress-bar progress-bar-success" data-ng-style="{width: num + '%'}"></div>
                                    </div>
                                </td>
                                <td>
                                    <button type="button" class="btn btn-primary start" data-ng-click="file.$submit()" data-ng-hide="!file.$submit || options.autoUpload" data-ng-disabled="file.$state() == 'pending' || file.$state() == 'rejected'">
                                        <i class="glyphicon glyphicon-upload"></i>
                                        <span>Start</span>
                                    </button>
                                    <button type="button" class="btn btn-warning cancel" data-ng-click="file.$cancel()" data-ng-hide="!file.$cancel">
                                        <i class="glyphicon glyphicon-ban-circle"></i>
                                        <span>Cancel</span>
                                    </button>
                                    <button data-ng-controller="FileDestroyController" type="button" class="btn btn-danger destroy" data-ng-click="file.$destroy()" data-ng-hide="!file.$destroy">
                                        <i class="glyphicon glyphicon-trash"></i>
                                        <span>Delete</span>
                                    </button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfIdVia" runat="server" Value="-1" />
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <asp:HiddenField id="UrlAuthGIS" runat="server" value="080042cad6356ad5dc0a720c18b53b8e53d4c274" />
    <asp:HiddenField id="TokenAuthGIS" runat="server" value="080042cad6356ad5dc0a720c18b53b8e53d4c274"/>
    <asp:HiddenField id="UrlSpecGIS" runat="server" value="080042cad6356ad5dc0a720c18b53b8e53d4c274"/>
    <asp:Button runat="server" ID="CmdSuggestNew" Text="" CssClass="hidden" OnClick="SuggestNew" />
    <asp:Button runat="server" ID="CmdSaveIstanza" Text="" CssClass="hidden" OnClick="SaveIstanza" />
</asp:Content>
