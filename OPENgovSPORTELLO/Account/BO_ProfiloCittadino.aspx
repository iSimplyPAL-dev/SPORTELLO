<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_ProfiloCittadino.aspx.cs" Inherits="OPENgovSPORTELLO.Account.BO_ProfiloCittadino" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand">Profilo Cittadino</li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>              
            </ul>
        </div>
    </div>
</asp:Content>

<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div class="container">
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

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12" style="height:260px;">
		<p class="lead_con_barra">Dati Generali</p>
        <div class="col-md-12" >
		    <div class="col-md-4" style="height:65px;">
			    <p>
				    <label id="lblCodFiscale">Codice Fiscale</label>
			    </p>
                <asp:textbox id="txtCodiceFiscale" tabIndex="1" runat="server" MaxLength="16" ToolTip="Codice Fiscale"></asp:textbox>
		    </div>
		    <div class="col-md-4" style="height:65px;">
			    <p>
				    <br /><label id="lblPIVA">Partita Iva</label>
			    </p>
			    <asp:textbox id="txtPartitaIva" tabIndex="2" runat="server" MaxLength="11" ToolTip="Partita Iva"></asp:textbox>
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
    <div class="col-md-12" style="height:225px;">
        <p class="lead_con_barra">Residenza/Sede Legale</p>
		<div class="col-md-8">
			<p>
                <label id="lblComuneRes">Comune</label>
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

    <div class="col-md-12" style="">
		<p class="lead_con_barra">Recapito
            <a href="http://89.31.204.114/sportellowiki/index.php?title=Profilo_spedizione" class="tooltip">
                <img class="SubmitBtn BottoneMini BottoneHelpMini" />
                <span>
                    <iframe src="http://89.31.204.114/sportellowiki/index.php?title=Profilo_spedizione" style="width:800px;height:480px;"></iframe>
                </span>
            </a>
		</p>
        <Grd:RibesGridView ID="GrdInvio" runat="server" BorderStyle="None" 
            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
            AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
            ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red" OnRowDataBound="GrdInvioRowDataBound">
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
    </div>
    <div class="col-md-12">
        <p class="lead_con_barra">Contatti
            <a href="http://89.31.204.114/sportellowiki/index.php?title=Profilo_contatti" class="tooltip">
                <img class="SubmitBtn BottoneMini BottoneHelpMini" />
                <span>
                    <iframe src="http://89.31.204.114/sportellowiki/index.php?title=Profilo_contatti" style="width:800px;height:480px;"></iframe>
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
							<asp:Label id="lblDecrizioneRiferimento" runat="server" Text='<%# FncGrd.FormattaContatto(DataBinder.Eval(Container, "DataItem.TipoRiferimento")) %>'></asp:Label>
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

    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>
