<%@ Page Title="Gestione Profilo" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="ProfiloFO.aspx.cs" Inherits="OPENgovSPORTELLO.Account.ProfiloFO" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand textheader">Profilo</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSave" OnClick="SaveDelega" ID="CmdDelegaSave" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" ID="CmdDelegaBack" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneDelegate" OnClick="GestDeleghe" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneKey" OnClick="ChangePassword" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSave" OnClientClick="return FieldValidatorAnag();" OnClick="Save"  CausesValidation="True" ValidationGroup="SaveDich" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClientClick="return FieldValidatorAnag();" OnClick="Back" /></li>
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
        <div id="SearchEnti" class="modal" style="width:100%;height:100%">
            <div class="modal-dialog">
                <div class="modal-content" style="padding:10px 10px 10px 10px">
                    <div class="panel panel-primary" style="height:100px">
                        <h4>Ricerca Enti</h4>
                        <div class="col-md-9">
                            <label>Ente</label>
                            <asp:TextBox ID="txtEnteSearch" runat="server"></asp:TextBox>                        
                        </div>
                        <div class="col-md-3">
                            <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSearch" OnClick="SearchEnti" />&ensp;
                            <asp:Button runat="server" ID="CmdBackSearchEnti" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                        </div>
                    </div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdComuni" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                            OnRowCommand="GrdComuniRowCommand" OnPageIndexChanging="GrdComuniPageIndexChanging">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="IDEnte" HeaderText="Codice">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIZIONE" HeaderText="Ente">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CAP" HeaderText="C.A.P.">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PROVINCIA" HeaderText="Prov.">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CODCATASTALE" HeaderText="Cod.Cat.">
                                    <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                    <ItemStyle horizontalalign="Justify"></ItemStyle>
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <itemtemplate>
                                        <asp:ImageButton id="imgAtt" runat="server" text="OK" CssClass="SubmitBtn BottoneGrd BottoneAttachGrd" CausesValidation="False" CommandName="AttachRow" CommandArgument='<%# Eval("IDEnte") %>' />
                                    </itemtemplate>
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
                                </asp:TemplateField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>&nbsp;
                </div>
            </div>
        </div>
        <div class="col-md-12 pageBO">
        <label id="lblErrorBO" class="text-danger usain"></label>
            <div id="Profilo">
                <div id="divDatiIstanza" class="col-md-12">
                    <div class="col-md-12 lead_con_barra">
                        <a id="lblHeadIstanza" class="col-md-2" onclick="ShowHideDiv(this,'divDetIstanza','Dati Istanza')" href="#">Dati Istanza</a>
                        <ul class="nav navbar-nav lead-right-btn" id="BottoniIstanza">
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
                                <div class="BottoneDivIstanza hidden">
                                    <asp:Button runat="server" Text="" CssClass="Bottone BottoneWork" OnClientClick="return ConfirmProtocollo()" Onclick="Work" />
                                    <p id="InCarico" class="col-md-12 TextCmdBlack"></p><asp:HiddenField runat="server" ID="hfTypeProtocollo" />
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
                        <div class="col-md-8">
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
                                        <headerstyle horizontalalign="Center" Width="100px"></headerstyle>
                                        <itemstyle horizontalalign="Right" Width="100px"></itemstyle>
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" Width="100px" Text='<%# FncGrd.FormattaDataGrd(Eval("DATA")) %>' CssClass="text-right"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DESCRSTATO" HeaderText="Stato">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="TESTO" HeaderText="Comunicazione">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                        </div>
                        <div class="col-md-1"></div>
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
                <div id="divProfilo" class="col-md-12">
                    <div class="col-md-12" >
		                <p class="lead_con_barra">Dati Generali</p>
                        <div class="col-md-12" >
		                    <div class="col-md-4" style="height:65px;">
			                    <p>
				                    <asp:Button class="Bottone BottoneTesseraSanitaria" runat="server" OnClientClick="if (!CodiceFiscale()) return false;" OnClick="CalcoloCFFromDati" />
				                    <label id="lblCodFiscale">Codice Fiscale</label>
				                    <asp:Button class="Bottone BottoneRecycle BottoneRecycleCF" runat="server" OnClick="CalcoloDatiFromCF" />
			                    </p>
                                <asp:textbox id="txtCodiceFiscale" tabIndex="1" runat="server" MaxLength="16" ToolTip="Codice Fiscale" OnBlur="return ControllaCF(this.value)"></asp:textbox>
		                    </div>
		                    <div class="col-md-4" style="height:65px;">
			                    <p>
				                    <br /><label id="lblPIVA">Partita Iva</label>
			                    </p>
			                    <asp:textbox id="txtPartitaIva" tabIndex="2" runat="server" MaxLength="11" ToolTip="Partita Iva" OnBlur="return ControllaPIVA(this.value)"></asp:textbox>
                            </div>
                            <div class="col-md-4" style="height:65px;">
                            </div>
                        </div>
                        <div class="col-md-12">
		                    <div class="col-md-6">
			                    <p><label id="lblCognome">Cognome/Rag.Sociale</label></p>
                                <asp:textbox id="txtCognome" CssClass="col-md-11" tabIndex="3" runat="server" MaxLength="100" ToolTip="Cognome"></asp:textbox>
		                    </div>        
                            <div class="col-md-4">
                                <p><label id="lblNome">Nome</label></p>
                                <asp:textbox id="txtNome" CssClass="col-md-11" tabIndex="4" runat="server" MaxLength="50" ToolTip="Nome"></asp:textbox>
                            </div>
		                    <div class="col-md-2">
			                    <p>
				                    <label id="lblSesso">Sesso</label>
			                    </p>
				                    <asp:dropdownlist id="ddlSesso" CssClass="col-md-11" onfocus="TrackBlur(this);" tabIndex="5" runat="server" AutoPostBack="false">
					                    <asp:listitem Value="-1">...</asp:listitem>
					                    <asp:listitem Value="M">MASCHIO</asp:listitem>
					                    <asp:listitem Value="F">FEMMINA</asp:listitem>
					                    <asp:listitem Value="G">PERSONA GIURIDICA</asp:listitem>
				                    </asp:dropdownlist>
		                    </div>
                        </div>
                        <div class="col-md-12">
		                    <div class="col-md-8">
			                    <p><label id="lblLuogoNascita">Luogo di Nascita</label>  
				                    <img class="SubmitBtn BottoneMini BottoneMapMarker" onclick="$('#MainContent_hdTypeComune').val('NAS');Scopri('SearchEnti');"/>
			                    </p>
                                <asp:textbox id="txtLuogoNascita" CssClass="col-md-11" tabIndex="6" runat="server" MaxLength="50" ToolTip="Comune Di Nascita"></asp:textbox>				
		                    </div>
                            <div class="col-md-2">
                                <p><label id="lblPVNascita">Provincia</label></p>
                                <asp:textbox id="txtPVNascita" CssClass="col-md-11" tabIndex="7" runat="server" MaxLength="2" ToolTip="Provincia Nascita"></asp:textbox>
                            </div>
                            <div class="col-md-2">
                                <p><label id="lblDataNascita">Data di Nascita</label></p>
                                <asp:textbox id="txtDataNascita" CssClass="col-md-11 text-right" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);TrackBlur(this);" tabIndex="8" runat="server" MaxLength="10" ToolTip="Data Di Nascita"></asp:textbox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <div class="col-md-12 lead_con_barra">Residenza/Sede Legale</div>
		                <div class="col-md-8">
			                <p>
                                <label id="lblComuneRes">Comune</label>
                                <img class="SubmitBtn BottoneMini BottoneMapMarker" onclick="$('#MainContent_hdTypeComune').val('RES');Scopri('SearchEnti');"/>
			                </p>
                            <asp:textbox id="txtComuneRes" CssClass="col-md-11" onblur="if(this.value!='') document.getElementById('linkapricomuneRes').click();" onfocus="TrackBlur(this);" tabIndex="15" runat="server" MaxLength="50" ToolTip="Comune di Res"></asp:textbox>
		                </div>	
                        <div class="col-md-2">
                            <p><label id="lblCAPRes">Cap</label></p>
                            <asp:textbox id="txtCAPRes" CssClass="col-md-11 OnlyNumber" onfocus="TrackBlur(this);" tabIndex="16" runat="server" MaxLength="10" ToolTip="Codice Avviamento Postale"></asp:textbox>
                        </div>		
		                <div class="col-md-2">
			                <p><label id="lblPVRes">Provincia</label></p>
                            <asp:textbox id="txtProvinciaRes" CssClass="col-md-11" onfocus="TrackBlur(this);" tabIndex="17" runat="server" MaxLength="2" ToolTip="Provincia di Res"></asp:textbox>
		                </div>
		                <div class="col-md-8">
			                <p>
                                <label id="lblViaRes">Via/Piazza/Corso</label>
			                </p>
                            <asp:textbox id="txtViaRes" CssClass="col-md-11" tabIndex="18" runat="server" MaxLength="50" ToolTip="Indirizzo Res"></asp:textbox>
		                </div>
		                <div class="col-md-1">
			                <p><label id="lblCivico">N.Civico</label></p>
                            <asp:textbox id="txtCivicoRes" CssClass="col-md-11" onkeypress="//return NumbersOnly(event);" tabIndex="20" runat="server" MaxLength="10" ToolTip="Numero civico Res"></asp:textbox>				
		                </div>
		                <div class="col-md-1">
			                <p><label id="lblEsponente">Esponente</label></p>
                            <asp:textbox id="txtEsponenteCivicoRes" CssClass="col-md-11" tabIndex="22" runat="server" ToolTip="Esponente legato al numero civico di Res"></asp:textbox>
		                </div>
		                <div class="col-md-1">
			                <p><label id="lblScala">Scala</label></p>
                            <asp:textbox id="txtScalaRes" CssClass="col-md-11" tabIndex="23" runat="server" MaxLength="20" ToolTip="Numero Scala Res"></asp:textbox>
		                </div>
		                <div class="col-md-1">
                            <p><label id="lblInterno">Interno</label></p>
                            <asp:textbox id="txtInternoRes" CssClass="col-md-11" tabIndex="24" runat="server" MaxLength="20" ToolTip="Interno Res"></asp:textbox>
		                </div>
		                <div class="col-md-12">
			                <p><label id="lblFrazioneRes">Frazione</label></p>
			                <asp:textbox id="txtFrazioneRes" CssClass="col-md-7" tabIndex="19" runat="server" MaxLength="50" ToolTip="Frazione Res"></asp:textbox>				
		                </div>
                    </div>
                    <div class="col-md-12">
		                <div class="col-md-12 lead_con_barra">
                            <a id="lblHeadRecapito" onclick="ShowHideDiv(this,'divRecapito','Recapito')" href="#">Recapito</a>
                            <a href="" target="_blank" class="tooltip HelpFOAccountRecapito">
                                <img class="BottoneMini BottoneHelpMini" />
                                <span>
                                    <iframe src="" class="HelpFOAccountRecapito"></iframe>
                                </span>
                            </a>
                            <ul class="nav navbar-nav lead-right-btn">
                                <li>
                                    <div class="BottoneDiv">
                                        <asp:Button runat="server" Text="" CssClass="Bottone BottoneDelete" OnClick="DeleteRecapito" />
                                        <p id="Elimina" class="col-md-12 TextCmdBlack"></p>
                                    </div>
                                </li>
                            </ul>
                        </div>
                        <div id="divRecapito" class="col-md-12">
                            <Grd:RibesGridView ID="GrdInvio" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowDataBound="GrdInvioRowDataBound" OnRowCommand="GrdInvioRowCommand">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField HeaderText="Tributo" DataField="descrtributo"></asp:BoundField>
				                    <asp:TemplateField HeaderText="Nominativo">
					                    <ItemTemplate>
						                    <asp:Label ID="Label1" runat="server" Text='<%# FncGrd.FormattaNominativo(DataBinder.Eval(Container, "DataItem.cognomeinvio"),DataBinder.Eval(Container, "DataItem.nomeinvio")) %>'></asp:Label>
					                    </ItemTemplate>
				                    </asp:TemplateField>
				                    <asp:TemplateField HeaderText="Località">
					                    <ItemTemplate>
                		                    <asp:Label ID="Label2" runat="server" Text='<%# FncGrd.FormattaComune(DataBinder.Eval(Container, "DataItem.caprcp"),DataBinder.Eval(Container, "DataItem.comunercp"),DataBinder.Eval(Container, "DataItem.provinciarcp")) %>'></asp:Label>
					                    </ItemTemplate>
				                    </asp:TemplateField>
				                    <asp:TemplateField HeaderText="Indirizzo">
					                    <ItemTemplate>
						                    <asp:Label ID="Label3" runat="server" Text='<%# FncGrd.FormattaVia(DataBinder.Eval(Container, "DataItem.viarcp"),DataBinder.Eval(Container, "DataItem.civicorcp"),DataBinder.Eval(Container, "DataItem.esponentecivicorcp"),DataBinder.Eval(Container, "DataItem.scalacivicorcp"),DataBinder.Eval(Container, "DataItem.internocivicorcp"),DataBinder.Eval(Container, "DataItem.frazionercp"),"","","") %>'></asp:Label>
					                    </ItemTemplate>
				                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <itemtemplate>
                                            <asp:ImageButton id="imgOpen" runat="server" CssClass="SubmitBtn Bottone BottoneOpenGrd" CausesValidation="False" CommandName="OpenRow" CommandArgument='<%# Eval("ID_DATA_SPEDIZIONE") %>'></asp:ImageButton> 
                                        </itemtemplate>
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Center"></itemstyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <itemtemplate>
                                            <asp:ImageButton id="imgDelete" runat="server" CssClass="Bottone BottoneDeleteGrd" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# Eval("ID_DATA_SPEDIZIONE") %>' OnClientClick="return confirm('Si vuole eliminare l\'indirizzo?')"></asp:ImageButton> 
                                        </itemtemplate>
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Center"></itemstyle>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
		                    <div id="DivIndSped" runat="server">
			                    <div id="divCmdCO" class="col-md-12">
                                    <div class="divBottone">
			                            <asp:HiddenField ID="hdIdCO" runat="server" Value="-1" Visible="false"/>
                                        <asp:Button runat="server" id="CmdSaveSpedizione" class="SubmitBtn Bottone BottoneSave" OnClientClick="return confirm('Si vogliono salvare le modifiche apportate all\'indirizzo?')" OnClick="SaveSpedizione" />
        		                        <asp:Button runat="server" class="SubmitBtn Bottone BottoneRecycle hidden" OnClientClick="return confirm('Si vuole ribaltare l\'indirizzo su tutti i tributi sovrascrivendo gli eventuali gia\' presenti?')" OnClick="SaveSpedizione" />&nbsp; 
        		                        <asp:Button runat="server" ID="CmdBackMngInvio" class="SubmitBtn Bottone BottoneBack" OnClick="Back" />&nbsp; 
                                    </div>
                                </div>
			                    <div class="col-md-4 hidden">
				                    <p><label id="lblTributo">Tributo</label> </p>
                                    <asp:DropDownList ID="ddlTributo" TabIndex="26" runat="server" CssClass="AzzeraIndInvio"></asp:DropDownList>
			                    </div>
			                    <div class="col-md-6">
				                    <p><label id="lblCognomeCO">Cognome&nbsp;<%--<font class="pal">*</font>--%></label></p>
					                    <asp:textbox id="txtCognomeCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="26" runat="server" MaxLength="100" ToolTip="Cognome da Indicare per la Spedizione"></asp:textbox>
			                    </div>
			                    <div class="col-md-4">
				                    <p><label id="lblNomeCO">Nome</label> </p>
                                    <asp:textbox id="txtNomeCO" CssClass="col-md-10 AzzeraIndInvio" tabIndex="27" runat="server" MaxLength="50" ToolTip="Nome da Indicare per la Spedizione"></asp:textbox>
			                    </div>
                                <div class="col-md-2">&nbsp;</div>
			                    <div class="col-md-8">
				                    <p><label id="lblComuneCO">Comune</label>
					                    <img class="SubmitBtn BottoneMini BottoneMapMarker" onclick="$('#MainContent_hdTypeComune').val('SPE');Scopri('SearchEnti');"/>
				                    </p>
                                    <asp:textbox id="txtComuneCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="28" runat="server" MaxLength="50" ToolTip="Comune Spedizione" OnChange="Azzera();"></asp:textbox>
			                    </div>
			                    <div class="col-md-2">
				                    <p><label id="lblCAPCO">Cap</label> </p>
				                    <asp:textbox id="txtCAPCO" CssClass="col-md-11 Azzera AzzeraIndInvio OnlyNumber" tabIndex="29" runat="server" MaxLength="5" ToolTip="CAP di Spedizione"></asp:textbox>
			                    </div>
			                    <div class="col-md-2">
				                    <p><label id="lblPVCO">Provincia</label></p>
                                    <asp:textbox id="txtProvinciaCO" CssClass="col-md-11 Azzera AzzeraIndInvio" tabIndex="30" runat="server" MaxLength="2" ToolTip="Provincia di Spedizione"></asp:textbox>
			                    </div>
			                    <div class="col-md-8">
				                    <p><label id="lblViaCO">Via/Piazza/Corso</label></p>
				                    <asp:textbox id="txtIndirizzoCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="31" runat="server" MaxLength="50" ToolTip="Indirizzo di Spedizione"></asp:textbox>
			                    </div>
			                    <div class="col-md-1">
				                    <p><label id="lblCivicoCO">N.Civico</label> </p>
				                    <asp:textbox id="txtNumeroCivicoCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="33" runat="server" MaxLength="10" ToolTip="Numero Civico Spedizione"></asp:textbox>
			                    </div>
			                    <div class="col-md-1">
				                    <p><label id="lblEsponenteCO">Esponente</label> </p>
                                    <asp:textbox id="txtEsponenteCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="35" runat="server" MaxLength="50" ToolTip="Esponente legato al numero civico di Spedizione"></asp:textbox>
			                    </div>
			                    <div class="col-md-1">
				                    <p><label id="lblScalaCO">Scala</label> </p>
                                    <asp:textbox id="txtScalaCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="36" runat="server" MaxLength="20" ToolTip="Scala"></asp:textbox>
			                    </div>
			                    <div class="col-md-1">
				                    <p><label id="lblInternoCO">Interno</label> </p>
                                    <asp:textbox id="txtInternoCO" CssClass="col-md-11 AzzeraIndInvio" tabIndex="37" runat="server" MaxLength="20" ToolTip="Interno"></asp:textbox>
			                    </div>
			                    <div class="col-md-12">
				                    <p><label id="lblFrazioneCO">Frazione</label> </p>
				                    <asp:textbox id="txtFrazioneCO" CssClass="col-md-7 AzzeraIndInvio" tabIndex="32" runat="server" MaxLength="20" ToolTip="Frazione di Spedizione"></asp:textbox>
			                    </div>
		                    </div>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <p class="lead_con_barra">Contatti
                            <a href="" target="_blank" class="tooltip HelpFOAccountContatti">
                                <img class="BottoneMini BottoneHelpMini" />
                                <span>
                                    <iframe src="" class="HelpFOAccountContatti"></iframe>
                                </span>
                            </a>
                        </p>
		                <div class="col-md-12">
			                <asp:datagrid id="GrdContatti" runat="server" Width="100%" OnItemDataBound="GrdContattiDataBound"
				                onmouseover="this.className='riga_tabella_mouse_over'" onmouseout="this.className='riga_tabella'"
				                BorderColor="Gainsboro" BackColor="White" AutoGenerateColumns="False" ShowHeader="False">
				                <AlternatingItemStyle ForeColor="Black" CssClass="CartListItemAlt"></AlternatingItemStyle>
				                <FooterStyle CssClass="CartListFooter"></FooterStyle>
				                <ItemStyle ForeColor="Black" CssClass="CartListItem" BackColor="White"></ItemStyle>
				                <columns>
					                <asp:templatecolumn Visible="False">
						                <itemtemplate>
							                <asp:Label id="lblIDRIFERIMENTO" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IDRIFERIMENTO") %>'></asp:Label>
						                </itemtemplate>
					                </asp:templatecolumn>
					                <asp:templatecolumn Visible="False">
						                <itemtemplate>
							                <asp:Label id="lblTipoRiferimento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.TipoRiferimento") %>'></asp:Label>
						                </itemtemplate>
					                </asp:templatecolumn>
					                <asp:templatecolumn HeaderText="Tipo Riferimento">
						                <itemtemplate>
							                <asp:Label id="lblDecrizioneRiferimento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DescrContatto") %>'></asp:Label>
						                </itemtemplate>
					                </asp:templatecolumn>
					                <asp:templatecolumn>
						                <itemtemplate>
							                <asp:Label id="DatiRiferimento" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DatiRiferimento") %>'></asp:Label>
						                </itemtemplate>
					                </asp:templatecolumn>
					                <asp:templatecolumn>
						                <headerstyle width="200px"></headerstyle>
						                <itemtemplate>
							                <asp:Label id="DataInizioInvio" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.DataValiditaInvioMAIL") %>'></asp:Label>
						                </itemtemplate>
					                </asp:templatecolumn>															
				                </columns>
			                </asp:datagrid>
		                </div>
                        <div class="col-md-12">
		                    <div class="col-md-3">					
			                    <p><label id="lblTipoContatto">Tipo Riferimento</label>
                                    <a href="" target="_blank" class="tooltip HelpFOAccountTipoRif">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src="" class="HelpFOAccountTipoRif"></iframe>
                                        </span>
                                    </a>
			                    </p>
                                <asp:dropdownlist id="ddlTipoContatto" tabIndex="38" runat="server" CssClass="col-md-11"></asp:dropdownlist>
                                <asp:HiddenField id="hdIdContatto" runat="server" value="-1" />
                            </div>
                            <div class="col-md-6">
                                <p><label id="lblDatiRiferimento">Dati Riferimento</label>
                                    <a href="" target="_blank" class="tooltip HelpFOAccountDatiRif">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src="" class="HelpFOAccountDatiRif"></iframe>
                                        </span>
                                    </a></p>
                                <asp:textbox id="txtDatiRiferimento" CssClass="col-md-11" tabIndex="39" runat="server" MaxLength="50" size="30"></asp:textbox>
                                <p class="ac" id="MessageEmail"></p>
                            </div>
                            <div class="col-md-3 hidden">
                                <p><label id="lblInformativeByMail">Invia Informative Via Mail</label></p>
                                <asp:checkbox id="chkInvioInformativeViaMail" runat="server" AutoPostBack="false" Checked="false" tabIndex="40"></asp:checkbox>
                            </div>
                            <div class="col-md-3 hidden">
                                <p><label id="lblDataInizioInvio">Data Inizio Invio</label></p>
                                <asp:textbox id="txtDataInizioInvio" tabIndex="41" runat="server" MaxLength="10" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);"></asp:textbox>
                            </div>
                            <div class="col-md-3">
                                <div class="col-md-4">
			                        <asp:Button runat="server" id="CmdCleanContatto" class="Bottone BottoneErase" OnClick="EraseContatto" />
                                    <p id="Pulisci" class="col-md-12 TextCmdBlack"></p>
                                </div>
                                <div class="col-md-4">
			                        <asp:Button runat="server" id="CmdSaveContatto" class="Bottone BottoneSave" OnClientClick="return VerificaDatiContratto();" OnClick="SaveContatto" />
                                    <p id="Salva" class="col-md-12 TextCmdBlack"></p>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button runat="server" id="CmdDeleteContatto" class="Bottone BottoneDelete" OnClick="DeleteContatto" />
                                    <p id="Elimina" class="col-md-12 TextCmdBlack"></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="Deleghe">
                <div class="col-md-12 lead_con_barra"><div id="lblInstructionsDeleghe" class="text-normal text-italic"></div></div>                
                <div class="col-md-12">
                    <Grd:RibesGridView ID="GrdDeleghe" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdDelegheRowDataBound" OnRowCommand="GrdDelegheRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
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
                                                    <li><asp:ImageButton runat="server" CssClass="Bottone BottonePrint" CausesValidation="False" CommandName="RowPrint" CommandArgument='<%# Eval("ID") %>' OnClientClick="$('.divGrdBtn').hide();"/><p id="Stampa" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneAttach" CausesValidation="False" CommandName="RowAttach" CommandArgument='<%# Eval("ID") %>' OnClientClick="$('.divGrdBtn').hide();" /><p id="Allega" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="Bottone BottoneDelete" CausesValidation="False" CommandName="RowDelete" CommandArgument='<%# Eval("ID") %>' OnClientClick="$('.divGrdBtn').hide();return confirm('Attenzione!\nStai per eliminare una delega:\nVuoi procedere?')" /><p id="Elimina" class="TextCmdGrd"></p></li><br>                                                    
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nominativo">
                                <HeaderTemplate>
                                    <asp:label runat="server">Nominativo</asp:label>&ensp;
                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn Bottone BottoneNewGrd" CausesValidation="False" CommandName="RowNew" CommandArgument='-1'></asp:ImageButton> 
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify" width="275px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNominativo" runat="server" Text='<%# Eval("Nominativo") %>' Width="100%"></asp:TextBox>
                                    <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cod.Fiscale/P.IVA">
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify" width="275px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCFPIVA" runat="server" Text='<%# Eval("CodFiscalePIVA") %>' Width="100%" OnBlur="return ControllaCF(this.value)"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tributo">
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify" Width="200px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTributo" runat="server" DataSource='<%# LoadPageCombo("TRIBUTO") %>' DataTextField="Descrizione" DataValueField="Codice" SelectedValue='<%# Eval("IdTributo") %>' Width="100%"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stato">
                                <HeaderTemplate>
                                    <asp:label runat="server">Stato</asp:label>
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblStato" runat="server" Text='<%# Eval("Stato") %>' Width="100%"></asp:Label>
                                    <asp:HiddenField ID="hfIdStato" runat="server" Value='<%# Eval("IDStato") %>' />
                                    <asp:HiddenField ID="hfIdIstanza" runat="server" Value='<%# Eval("IDIstanza") %>' />
                                    <asp:HiddenField ID="hfSubentro" runat="server" Value='<%# Eval("Subentro") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
                <div id="divDelegaAttach" class="col-md-4"><br />
                    <div class="col-md-12 lead_con_barra">
                        <a class="col-md-3" onclick="ShowHideDiv(this,'divDetDelAttach','Allegati')" href="#">Allegati</a>
                        <ul class="nav navbar-nav lead-right-btn">
                            <li><asp:Button runat="server" CssClass="SubmitBtn Bottone BottoneSave" OnClick="SaveDelegaAttach" ID="CmdSaveDelegaAttach" /></li>
                            <li><asp:Button runat="server" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" ID="CmdDelegaAttachBack" /></li>
                        </ul>
                    </div> 
                    <div id="divDetDelAttach" class="col-md-12">
                        <Grd:RibesGridView ID="GrdDelegaAttach" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="False" PageSize="20"
                            OnRowDataBound="GrdDelegaAttachRowDataBound" OnRowCommand="GrdDelegaAttachRowCommand">
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
                        <div id="FileToUploadDelega"><br />
                            <asp:FileUpload ID="fuDelega" runat="server" AllowMultiple="true" />
                            <asp:RequiredFieldValidator ID="rfvFileUploadDelega" runat="server" ControlToValidate="fuDelega" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                        </div>
                    </div> 
                </div>
            </div>
			<div id="divMotivazione" class="col-md-12">
				<div class="lead_con_barra col-md-12">
					<a id="lblHeadMotivazione" onclick="ShowHideDiv(this,'divDetMotivi','Motivazione')" href="#">Motivazione</a>
				</div>
				<div id="divDetMotivi" class="col-md-12">
					<div class="col-md-12">
						<p><label id="lblMotivazione">Note aggiuntive di motivazione:</label></p>
						<asp:TextBox ID="txtMotivazione" runat="server" TextMode="MultiLine" Width="100%" CssClass="Azzera"></asp:TextBox>
					</div>
					<div class="col-md-12">
						<asp:CheckBox ID="chkAllegati" runat="server" CssClass="Input_Label_bold" Text="Allego Documentazione" AutoPostBack="true" OnCheckedChanged="GestAllegati" />
						<div id="FileToUpload">
							<asp:FileUpload ID="MIOfileUpload" runat="server" AllowMultiple="true"/>
							<asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="MIOfileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
						</div>
					</div>
				</div>
			</div>
        </div>
    </div>
    <asp:HiddenField ID="hdIdContribuente" runat="server" Value="-1"/>
	<asp:HiddenField ID="hdIdAnagrafica" runat="server" Value="-1"/>
	<asp:HiddenField id="hdCodRappresentanteLegale" runat="server" value="-1" />
	<asp:HiddenField id="hdCodViaResidenza" runat="server" value="-1" />
	<asp:HiddenField id="hdCodViaSpedizione" runat="server" value="-1" />
    <asp:HiddenField ID="hdTypeComune" runat="server" />

	<asp:TextBox id="txtCodComuneNascita" runat="server" style="display:none"></asp:TextBox>
	<asp:TextBox id="txtCodComuneResidenza" runat="server" style="display:none"></asp:TextBox>
	<asp:TextBox id="txtCodComuneSpedizione" runat="server" style="display:none"></asp:TextBox>
                
    <asp:Button id="CmdSave" style="DISPLAY: none" runat="server"></asp:Button>
    <asp:Button id="CmdDelete" style="DISPLAY: none" runat="server"></asp:Button>

    <asp:Button id="CmdPrintDelega" style="DISPLAY: none" runat="server" OnClick="PrintDelega"></asp:Button>
    		
    <asp:HiddenField ID="hfIdIstanzaDelega" runat="server" Value="-1" />
    <asp:HiddenField ID="hfTakeover" runat="server" Value="0" />
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <!--<a class="Tributi">AAAAAAAAAAA...avevo delle A in più</a>
    <button id="Exit">AAAAAAAAAAA...avevo delle A in più</button>-->
</asp:Content>
