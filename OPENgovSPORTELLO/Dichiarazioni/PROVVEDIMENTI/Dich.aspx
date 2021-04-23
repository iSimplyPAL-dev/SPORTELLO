<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Dich.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.PROVVEDIMENTI.Dich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p id="PageTitle" class="navbar-brand col-md-6"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;<a class="ICP text-white" id="TitlePage">Consultazione Accertamenti</a>&ensp;-&ensp;Immobile</p>
            <ul class="nav navbar-nav navbar-right-btn">
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
        <div id="divIstanza" class="col-md-12 pageFO">
            <label id="lblErrorFO" class="text-danger usain"></label>
            <div id="divDatiUI" class="col-md-12">
				<div id="divIntestazione" class="col-md-12"></div>
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDich','Dati Dichiarato')" href="#"></a>
                </div>  
                <div id="divDich" class="col-md-12">
                    <div class="col-md-12">
                        <p class="lead">Dati Dichiarato</p>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdDich" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdUIRowCommand" OnRowDataBound="GrdUIRowDataBound">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
									<asp:BoundField DataField="Ubicazione" HeaderText="Ubicazione">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:TemplateField HeaderText="Dal">
										<headerstyle horizontalalign="Center" Width="80px"></headerstyle>
										<itemstyle horizontalalign="Right" Width="80px"></itemstyle>
										<ItemTemplate>
											<asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DataInizio")) %>' CssClass="text-right"></asp:TextBox>
                                            <asp:HiddenField ID="hfIdTributo" runat="server" Value='<%# Eval("IDTributo") %>' />
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Al">
										<headerstyle horizontalalign="Center" Width="80px"></headerstyle>
										<itemstyle horizontalalign="Right" Width="80px"></itemstyle>
										<ItemTemplate>
											<asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DataFine")) %>' CssClass="text-right"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:BoundField DataField="Durata" HeaderText="Giorni">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Aliquota" HeaderText="Aliquota €">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="MQ" HeaderText="MQ">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="DescrRiduzioni" HeaderText="Rid.">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Importo" HeaderText="Importo €">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            &nbsp;
                        </div>
                    </div>
                </div>
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divAcc','Dati Accertato')" href="#"></a>
                </div>  
                <div id="divAcc" class="col-md-12">
                    <div class="col-md-12">
                        <p class="lead">Dati Accertato</p>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdAcc" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdUIRowCommand" OnRowDataBound="GrdUIRowDataBound">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
									<asp:BoundField DataField="Ubicazione" HeaderText="Ubicazione">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:TemplateField HeaderText="Dal">
										<headerstyle horizontalalign="Center" Width="80px"></headerstyle>
										<itemstyle horizontalalign="Right" Width="80px"></itemstyle>
										<ItemTemplate>
											<asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DataInizio")) %>' CssClass="text-right"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Al">
										<headerstyle horizontalalign="Center" Width="80px"></headerstyle>
										<itemstyle horizontalalign="Right" Width="80px"></itemstyle>
										<ItemTemplate>
											<asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DataFine")) %>' CssClass="text-right"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:BoundField DataField="Durata" HeaderText="Giorni">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="DescrCategoria" HeaderText="Categoria">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Aliquota" HeaderText="Aliquota €">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="MQ" HeaderText="MQ">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="DescrRiduzioni" HeaderText="Rid.">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Importo" HeaderText="Importo €">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            &nbsp;
                        </div>
                    </div>
                </div>
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDich','Dati Sanzioni')" href="#"></a>
                </div>  
                <div id="divSanz" class="col-md-12">
                    <div class="col-md-12">
                        <p class="lead">Dati Sanzioni</p>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdSanz" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdUIRowCommand" OnRowDataBound="GrdUIRowDataBound">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
									<asp:BoundField DataField="Descrizione" HeaderText="Descrizione">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Importo" HeaderText="Importo €">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Motivazione" HeaderText="Motivazione">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            &nbsp;
                        </div>
                    </div>
                </div>
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDich','Dati Interessi')" href="#"></a>
                </div>  
                <div id="divInt" class="col-md-12">
                    <div class="col-md-12">
                        <p class="lead">Dati Interessi</p>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdInt" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdUIRowCommand" OnRowDataBound="GrdUIRowDataBound">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
									<asp:TemplateField HeaderText="Dal">
										<headerstyle horizontalalign="Center" Width="80px"></headerstyle>
										<itemstyle horizontalalign="Right" Width="80px"></itemstyle>
										<ItemTemplate>
											<asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DataInizio")) %>' CssClass="text-right"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:TemplateField HeaderText="Al">
										<headerstyle horizontalalign="Center" Width="80px"></headerstyle>
										<itemstyle horizontalalign="Right" Width="80px"></itemstyle>
										<ItemTemplate>
											<asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DataFine")) %>' CssClass="text-right"></asp:TextBox>
										</ItemTemplate>
									</asp:TemplateField>
									<asp:BoundField DataField="Aliquota" HeaderText="Tasso">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
									<asp:BoundField DataField="Importo" HeaderText="Importo €">
										<headerstyle horizontalalign="Center"></headerstyle>
										<itemstyle horizontalalign="Justify"></itemstyle>
									</asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            &nbsp;
                        </div>
                    </div>
                </div>
                <div class="col-md-12 lead_con_barra">
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDich','Dati Importi')" href="#"></a>
                </div>  
                <div id="divImporti" class="col-md-12">
                    <div class="col-md-12">
                        <p class="lead">Dati Importi</p>
                        <div class="col-md-4">
							<ul class="nav navbar-nav navbar-left">
								<li><label id="lblIntestDifImp" class="col-md-12"></label></li>
								<li><label id="lblIntestInte" class="col-md-12"></label></li> 
								<li><label id="lblIntestSanz" class="col-md-12"></label></li> 
								<li><label id="lblIntestSanzNoRid" class="col-md-12"></label></li>
								<li><label id="lblIntestArr" class="col-md-12"></label></li> 
								<li><label id="lblIntestSpese" class="col-md-12"></label></li> 
								<li><label id="lblIntestTot" class="col-md-12"></label></li>
							</ul>						
                        </div>
                        <div class="col-md-4">
							<ul class="nav navbar-nav navbar-right">
								<li><label id="lblRidDifImp" class="col-md-12"></label></li>
								<li><label id="lblRidInte" class="col-md-12"></label></li> 
								<li><label id="lblRidSanz" class="col-md-12"></label></li> 
								<li><label id="lblRidSanzNoRid" class="col-md-12"></label></li>
								<li><label id="lblRidArr" class="col-md-12"></label></li> 
								<li><label id="lblRidSpese" class="col-md-12"></label></li> 
								<li><label id="lblRidTot" class="col-md-12"></label></li>
							</ul>						
                        </div>
                        <div class="col-md-4">
							<ul class="nav navbar-nav navbar-right">
								<li><label id="lblTotDifImp" class="col-md-12"></label></li>
								<li><label id="lblTotInte" class="col-md-12"></label></li> 
								<li><label id="lblTotSanz" class="col-md-12"></label></li> 
								<li><label id="lblTotSanzNoRid" class="col-md-12"></label></li>
								<li><label id="lblTotArr" class="col-md-12"></label></li> 
								<li><label id="lblTotSpese" class="col-md-12"></label></li> 
								<li><label id="lblTotTot" class="col-md-12"></label></li>
							</ul>						
                        </div>
                    </div>
                </div>
            </div>  
        </div>
    </div>    
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <asp:HiddenField ID="hfIdRow" runat="server" Value="-1" />
</asp:Content>
