<%@ Page Title="Dichiara Nuovo" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Dich.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.TARSU.Dich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p id="PageTitle" class="navbar-brand"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;<a class="TARSU text-white" id="TitlePage">Consultazione TARI</a>&ensp;-&ensp;Immobile</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" ID="CmdSave" Text="" CssClass="Bottone BottoneSave" OnClientClick="return FieldValidatorTARSU();" OnClick="Save" /></li>
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
        <div id="SearchStradario" class="modal" style="width:100%;height:100%">
            <div class="modal-dialog">
                <div class="modal-content" style="padding:10px 10px 10px 10px">
                    <div class="panel panel-primary" style="height:100px">
                        <h4>Ricerca Stradario</h4>
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
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lblStradario" Text='<%#Eval("DESCRIZIONE") %>'></asp:Label>
                                        <asp:HiddenField runat="server" id="hdIdStradario" Value='<%# Eval("ID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <itemtemplate>
                                        <asp:ImageButton id="imgAtt" runat="server" CssClass="SubmitBtn BottoneGrd BottoneAttachGrd" CausesValidation="False" CommandName="AttachRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    </itemtemplate>
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
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
        <div id="divConfirm" class="modal" style="width:100%;height:100%">
            <div class="modal-dialog">
                <div class="modal-content">
                    <p><label id="lblDescrConfirm" class="usain text-warning"></label><br /></p>
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
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneKO" ID="KOBack" OnClick="Back"></asp:Button>
                                </div>
                            </li>
                            <li>
                                <div>
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneKO" ID="KORest" OnClientClick="return true;"></asp:Button>
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
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneSort" Onclick="Ribalta" />
                                <p id="Ribalta" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneMailBox" OnClientClick="return FieldValidatorGestIstanze();" Onclick="MailBox" />
                                <p id="Comunicazione" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneStop" OnClientClick="return FieldValidatorGestIstanze();" Onclick="Stop" />
                                <p id="Respingi" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneAccept" Onclick="Accept" />
                                <p id="Valida" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneWork" OnClientClick="return ConfirmProtocollo()" Onclick="Work" />
                                <p id="InCarico" class="col-md-12 TextCmdBlack"></p><asp:HiddenField runat="server" ID="hfTypeProtocollo" />
                            </div>
                        </li>
                        <li>
                            <div class="BottoneDivIstanza">
                                <asp:Button runat="server" Text="" CssClass="Bottone BottoneCounter" OnClientClick="return confirm('Ti e\' arrivata la mail dal protocollo?');" Onclick="Protocolla" />
                                <p id="Protocolla" class="col-md-12 TextCmdBlack"></p>
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
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DATA")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DESCRSTATO" HeaderText="Stato">
                                    <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                    <itemstyle horizontalalign="Justify" Width="120px"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TESTO" HeaderText="Comunicazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
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
                                            <asp:CheckBox ID="chkSel" runat="server"/>
                                            <div class="divGrdBtn">
                                                <div class="panel panelGrd">
                                                    <ul class="nav navbar-nav">
                                                        <li><asp:ImageButton runat="server" CssClass="Bottone BottoneAttach" CausesValidation="False" CommandName="RowDownload" CommandArgument='<%# Eval("IDALLEGATO") %>'/><p id="download" class="TextCmdGrd"></p></li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="FILENAME" HeaderText="Allegato">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
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
                                <asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneWarning" OnClick="Inutilizzo" />
                                <p id="inutilizzato" class="col-md-12 TextCmdBlack"></p>
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
                                <img class="SubmitBtn BottoneMini BottoneMapMarker" onclick="Scopri('SearchStradario');"/>
                                <a href="" target="_blank" class="tooltip HelpFODichTARSUUbicazione">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichTARSUUbicazione"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:TextBox runat="server" ID="txtVia" CssClass="Azzera txtVia col-md-10"></asp:TextBox>
                        </div>
                        <div class="col-md-1">                        
                            <p><label id="lblCivico">Civico:</label></p>
                            <asp:TextBox runat="server" ID="txtCivico" CssClass="Azzera col-md-10" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblDataInizio">Data Inizio Occupazione:</label>
                                <label id="lblDataInizioORG" runat="server" class="text-italic"></label>
                                <a href="" target="_blank" class="tooltip HelpFODichTARSUInizio">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichTARSUInizio"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:TextBox runat="server" ID="txtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-10 text-right" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <p><label id="lblDataFine">Data Fine Occupazione:</label></p>
                            <asp:TextBox runat="server" ID="txtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-7 text-right" TabIndex="3"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-1">
                            <p><label id="lblFoglio">Foglio:</label></p>
                            <asp:TextBox runat="server" ID="txtFoglio" CssClass="Azzera col-md-10" AutoPostBack="true" OnTextChanged="RifCatChanged" TabIndex="4"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p><label id="lblNumero">Numero:</label></p>
                            <asp:TextBox runat="server" ID="txtNumero" CssClass="Azzera col-md-10" AutoPostBack="true" OnTextChanged="RifCatChanged" TabIndex="5"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p>
                                <label id="lblSub">Subalterno:</label>
                            </p>
                            <asp:TextBox runat="server" ID="txtSub" CssClass="Azzera col-md-6" AutoPostBack="true" OnTextChanged="RifCatChanged" TabIndex="6"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <p>
                                <label id="lblSIT">Cartografia</label>
                            </p>
                            <p style="margin-left:20px">
                                <img class="BottoneMini BottoneMap" />
                            </p>
                        </div>
                        <div class="col-md-7">
                            <p><label id="lblStatoOccupazione">Stato Occupazione:</label>
                                <a href="" target="_blank" class="tooltip HelpFODichTARSUOccupazione">
                                    <img class="BottoneMini BottoneHelpMini" />
                                    <span>
                                        <iframe src="" class="HelpFODichTARSUOccupazione"></iframe>
                                    </span>
                                </a>
                            </p>
                            <asp:DropDownList ID="ddlStatoOccupazione" runat="server" CssClass="Azzera col-md-6" TabIndex="7"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <p class="lead">Dati Per Calcolo
                            <a href="" target="_blank" class="tooltip HelpFODichTARSUCalcolo">
                                <img class="BottoneMini BottoneHelpMini" />
                                <span>
                                    <iframe src="" class="HelpFODichTARSUCalcolo"></iframe>
                                </span>
                            </a>
                        </p>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdVani" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdVaniRowCommand" OnRowDataBound="GrdVaniRowDataBound">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Tipo Utilizzo">
                                        <HeaderTemplate>
                                            <asp:label runat="server">Tipo Utilizzo</asp:label>
                                            <a href="" target="_blank" class="tooltip HelpFODichTARSUUtilizzo">
                                                <img class="BottoneMini BottoneHelpMini" />
                                                <span>
                                                    <iframe src="" class="HelpFODichTARSUUtilizzo"></iframe>
                                                </span>
                                            </a>&ensp;
                                            <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn BottoneGrd BottoneNewGrd" CausesValidation="False" CommandName="NewRow" CommandArgument='-1'></asp:ImageButton> 
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center" Width="150px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify" Width="150px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlUtilizzo" runat="server" DataSource='<%# LoadPageCombo("UTILIZZO") %>' DataTextField="Descrizione" DataValueField="Codice" SelectedValue='<%# Eval("SCOPECAT") %>' Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged" TabIndex="8"></asp:DropDownList>
                                            <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Componenti/Attività">
                                        <HeaderTemplate>
                                            <asp:label runat="server">Componenti/Attività</asp:label>
                                            <a href="" target="_blank" class="tooltip HelpFODichTARSUNC">
                                                <img class="BottoneMini BottoneHelpMini" />
                                                <span>
                                                    <iframe src="" class="HelpFODichTARSUNC"></iframe>
                                                </span>
                                            </a>
                                            <a href="" target="_blank" class="tooltip HelpFODichTARSUNonDom">
                                                <img class="BottoneMini BottoneHelpMini" />
                                                <span>
                                                    <iframe src="" class="HelpFODichTARSUNonDom"></iframe>
                                                </span>
                                            </a>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNC" runat="server" Text='<%# Eval("NCOMPONENTI") %>' CssClass="text-right" Width="15%" onblur="ShowHideOccupanti();" AutoPostBack="true" OnTextChanged="ControlSelectedChanged" TabIndex="9">0</asp:TextBox>
                                            <asp:DropDownList ID="ddlCatND" runat="server" CssClass="col-md-11" TabIndex="10"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vani">
                                        <HeaderTemplate>
                                            <asp:label runat="server">Vani</asp:label>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center" Width="200px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify" Width="200px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlVani" runat="server" Width="100%" TabIndex="11"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>                  
                                    <asp:TemplateField HeaderText="MQ">
                                        <HeaderStyle horizontalalign="Center" Width="80px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right" Width="80px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMQ" runat="server" Text='<%# Eval("MQ") %>' Width="100%" CssClass="OnlyNumber text-right" onblur="ShowHideOccupanti();" AutoPostBack="true" OnTextChanged="ControlSelectedChanged" TabIndex="12">0</asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:Label runat="server">Esente</asp:Label>
                                            <a href="" target="_blank" class="tooltip HelpFODichTARSUEsente">
                                                <img class="BottoneMini BottoneHelpMini" />
                                                <span>
                                                    <iframe src="" class="HelpFODichTARSUEsente"></iframe>
                                                </span>
                                            </a>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center" Width="80px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Center" Width="80px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkEsente" runat="server" Checked='<%# FncGrd.FormattaBoolGrd(Eval("ISESENTE")) %>' onchange="ShowHideOccupanti();" TabIndex="13"></asp:CheckBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <itemtemplate>
                                            <asp:ImageButton id="imgDelete" runat="server" CssClass="Bottone BottoneDeleteGrd" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Attenzione!\nStai per eliminare una voce:\nVuoi procedere?')"></asp:ImageButton> 
                                        </itemtemplate>
                                        <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                        <itemstyle horizontalalign="Center" Width="80px"></itemstyle>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
                            <div class="col-md-12 text-right">
                                <p><label id="lblTotMQ" class="usain Azzera"></label>&ensp;<label id="lblTotTassabili" class="usain Azzera"></label></p>
                            </div>
                            <div id="divOccupanti" class="col-md-12">
                                <a class="lead col-md-12" onclick="ShowHideDiv(this,'divDetOccupanti','Dati Occupanti')" href="#">Dati Occupanti
                                    <a href="" target="_blank" class="tooltip HelpFODichTARSUOccupanti">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src="" class="HelpFODichTARSUOccupanti"></iframe>
                                        </span>
                                    </a>
                                </a>
                                <div id="divDetOccupanti">
                                    <Grd:RibesGridView ID="GrdOccupanti" runat="server" BorderStyle="None" 
                                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                        AutoGenerateColumns="False" AllowPaging="False"
                                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                        OnRowCommand="GrdOccupantiRowCommand" OnRowDataBound="GrdOccupantiRowDataBound">
                                        <PagerSettings Position="Bottom"></PagerSettings>
                                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateField HeaderText="Cognome e Nome">
                                                <HeaderTemplate>
                                                    <asp:label runat="server">Cognome e Nome</asp:label>
                                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn BottoneGrd BottoneNewGrd" CausesValidation="False" CommandName="NewRow" CommandArgument='-1'></asp:ImageButton> 
                                                </HeaderTemplate>
                                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtNominativo" runat="server" Text='<%# Eval("nominativo") %>' CssClass="col-md-11" TabIndex="14"></asp:TextBox>
                                                    <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cod.Fiscale">
                                                <HeaderStyle horizontalalign="Center" Width="140px"></HeaderStyle>
                                                <ItemStyle horizontalalign="Justify" Width="140px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtCF" Text='<%# Eval("codfiscale") %>' CssClass="col-md-11" TabIndex="15" OnBlur="return ControllaCF(this.value)"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Data nascita">
                                                <HeaderStyle horizontalalign="Center" Width="100px"></HeaderStyle>
                                                <ItemStyle horizontalalign="Right" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtDataNascita" Text='<%# FncGrd.FormattaDataGrd(Eval("datanascita")) %>' onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="text-right col-md-11" TabIndex="16"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Luogo nascita">
                                                <HeaderStyle horizontalalign="Center" Width="150px"></HeaderStyle>
                                                <ItemStyle horizontalalign="Justify" Width="150px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:TextBox runat="server" ID="txtLuogoNascita" Text='<%# Eval("Luogonascita") %>' CssClass="col-md-11" TabIndex="17"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Parentela">
                                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="ddlParentela" runat="server" Width="100%" TabIndex="18"></asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>                  
                                            <asp:TemplateField>
                                                <itemtemplate>
                                                    <asp:ImageButton id="imgDelete" runat="server" CssClass="Bottone BottoneDeleteGrd" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Attenzione!\nStai per eliminare una voce:\nVuoi procedere?')"></asp:ImageButton> 
                                                </itemtemplate>
                                                <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                                <itemstyle horizontalalign="Center" Width="80px"></itemstyle>
                                            </asp:TemplateField>
                                        </Columns>
                                    </Grd:RibesGridView>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <p><label id="lblRid">Riduzioni:</label></p>
                                <asp:DropDownList ID="ddlRid" runat="server" Width="100%" TabIndex="19"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <p><label id="lblEse">Esenzioni:</label></p>
                                <asp:DropDownList ID="ddlEse" runat="server" Width="100%" TabIndex="20"></asp:DropDownList>
                            </div>
                            &nbsp;
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
                                    <asp:CheckBox ID="chkSel" runat="server"/>
                                    <asp:HiddenField ID="hfIdMotivazione" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRIZIONE" HeaderText="Motivazione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                    <div class="col-md-12">
                        <p><label id="lblMotivazione">Note aggiuntive di motivazione:</label></p>
                        <asp:TextBox ID="txtMotivazione" runat="server" TextMode="MultiLine" Width="100%" TabIndex="21"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkAllegati" runat="server" CssClass="Input_Label_bold" Text="Allego Documentazione" AutoPostBack="true" OnCheckedChanged="GestAllegati" />
                        <a href="" target="_blank" class="tooltip HelpFODichTARSUAttach">
                            <img class="BottoneMini BottoneHelpMini" />
                            <span>
                                <iframe src="" class="HelpFODichTARSUAttach"></iframe>
                            </span>
                        </a>
                        <div id="FileToUpload">
                            <asp:FileUpload ID="MIOfileUpload" runat="server" AllowMultiple="true" />
                            <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="MIOfileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <!-- The file upload form used as target for the file upload widget -->
                    <div style="display:none" id="fileupload" data-ng-app="demo" data-ng-controller="DemoFileUploadController" data-file-upload="options" data-ng-class="{'fileupload-processing': processing() || loadingFiles}">
                        <!-- Redirect browsers with JavaScript disabled to the origin page -->
                        <noscript><input type="hidden" name="redirect" value="https://blueimp.github.io/jQuery-File-Upload/"></noscript>
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
                                <div class="progress progress-striped active" data-file-upload-progress="progress()"><div class="progress-bar progress-bar-success" data-ng-style="{width: num + '%'}"></div></div>
                                <!-- The extended global progress state -->
                                <div class="progress-extended">&nbsp;</div>
                            </div>
                        </div>
                        <!-- The table listing the files available for upload/download -->
                        <table class="table table-striped files ng-cloak">
                            <tr data-ng-repeat="file in queue" data-ng-class="{'processing': file.$processing()}">
                                <td data-ng-switch data-on="!!file.thumbnailUrl">
                                    <div class="preview" data-ng-switch-when="true">
                                        <a data-ng-href="{{file.url}}" title="{{file.name}}" download="{{file.name}}" data-gallery><img data-ng-src="{{file.thumbnailUrl}}" alt=""></a>
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
                                    <div class="progress progress-striped active fade" data-ng-class="{pending: 'in'}[file.$state()]" data-file-upload-progress="file.$progress()"><div class="progress-bar progress-bar-success" data-ng-style="{width: num + '%'}"></div></div>
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
    <asp:Button runat="server" ID="CmdSaveIstanza" Text="" CssClass="hidden" OnClick="SaveIstanza" />
</asp:Content>
