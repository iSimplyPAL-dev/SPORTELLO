<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="MngMessages.aspx.cs" Inherits="OPENgovSPORTELLO.Settings.MngMessages" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container" >
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand" style="height:50px;"><a class="title" id="TitlePage"></a></li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSave" OnClientClick="return FieldValidatorComunicazioni();" OnClick="Save" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSearch" OnClick="Search" /></li> 
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
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
            <li><a class="IstanzeBO nav navbar-nav Bottone BottoneArchive"></a></li>
            <li><a class="IstanzeBO nav navbar-nav">Istanze</a></li>
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
    <div class="body_page"><br />
        <div class="pageBO">
            <label id="lblErrorBO" class="col-md-12 text-danger usain"></label>
            <div id="divParamSearch" class="panel panel-primary">
                <div class="col-md-12">                    
                    <div class="col-md-3">
                        <p><label id="lblEnte">Ente:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-11"></asp:DropDownList> 
                    </div>&nbsp;    
                    <div class="col-md-3">
                        <p><label id="lblDestinatari">Destinatari:</label></p>
                        <asp:DropDownList runat="server" ID="ddlDestinatari" class="col-md-11"></asp:DropDownList> 
                    </div>&nbsp;    
                    <div class="col-md-3">
                        <p><label id="lblMezzo">Mezzo:</label></p>
                        <asp:CheckBox ID="chkSito" runat="server" Text="Sportello" Checked="true" Enabled="false" />&nbsp;
                        <asp:CheckBox ID="chkApp" runat="server" Text="App" />&nbsp;
                        <asp:CheckBox ID="chkMail" runat="server" Text="Mail" />&nbsp;
                    </div>&nbsp;
                    <div class="col-md-2">
                        <p><label id="lblDataInvio">Data Invio:</label></p>
                        <asp:TextBox runat="server" ID="txtDataInvio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-10 text-right"></asp:TextBox>
                    </div>&nbsp;   
                </div> 
            </div>
            <div class="col-md-12 lead_con_barra">
                <a id="lblHeadMsg" class="col-md-2">Messaggi</a>
                <ul class="nav navbar-nav lead-right-btn">
                    <li>
                        <div class="BottoneDiv">
                            <asp:Button runat="server" Text="" CssClass="SubmitBtn BottoneGrd BottoneNewGrd" OnClick="NewMsg" />
                            <p id="NuovoMsg" class="TextCmdBlack"></p>
                        </div>
                    </li>
                </ul>
            </div>
            <div id="divMng" class="col-md-12">
                <div class="col-md-12" id="divDest">
                    <div class="col-md-8">
                        <p><label id="lblEnteDest">Ente sul quale ribaltare:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnteDest" class="col-md-6"></asp:DropDownList> 
                        <asp:HiddenField ID="hfIsCopyTo" runat="server" Value="0" />
                    </div>&nbsp;    
                </div>     
                <div class="col-md-12">
                    <Grd:RibesGridView ID="GrdSubset" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                        OnPageIndexChanging="GrdSubsetPageIndexChanging">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItem"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Sel.">
                                <HeaderStyle HorizontalAlign="Center" Width="30px"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSel" runat="server"/>
                                    <asp:HiddenField ID="hfIdSubset" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>&nbsp;
                    <div id="divSubSetAnag" class="col-md-12">
                        <div class="col-md-12">
                            <div id="lblForewordSubSetAnag" class="col-md-11 text-11 text-italic"></div>
                        </div>
		                <div class="col-md-6">
			                <p><label id="lblCognome">Cognome/Rag.Sociale</label></p>
                            <asp:textbox id="txtCognome" CssClass="col-md-11" runat="server" MaxLength="100" ToolTip="Cognome"></asp:textbox>
		                </div>        
                        <div class="col-md-3">
                            <p><label id="lblNome">Nome</label></p>
                            <asp:textbox id="txtNome" CssClass="col-md-11" runat="server" MaxLength="50" ToolTip="Nome"></asp:textbox>
                        </div>       
                        <div class="col-md-3">
                            <p><label id="lblCFPIVA">Cod.Fiscale/P.IVA</label></p>
                            <asp:textbox id="txtCFPIVA" CssClass="col-md-11" runat="server" MaxLength="16" ToolTip="Cod.Fiscale/P.IVA"></asp:textbox>
                        </div>
                    </div>
                </div>&nbsp;
                <div class="col-md-12">
                    <p><label id="lblMessage">Testo:</label></p>
                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Width="100%" CssClass="Azzera"></asp:TextBox>
                </div>
            </div>
            <div id="divVisual" class="col-md-12">
                <Grd:RibesGridView ID="GrdMessages" runat="server" BorderStyle="None" 
                    BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                    AutoGenerateColumns="False" AllowPaging="False"
                    ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                    OnRowDataBound="GrdMessagesRowDataBound" OnRowCommand="GrdMessagesRowCommand">
                    <PagerSettings Position="Bottom"></PagerSettings>
                    <FooterStyle CssClass="CartListFooter"></FooterStyle>
                    <RowStyle CssClass="CartListItem"></RowStyle>
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
                                                <li id="ColCmdCopy"><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneRecycle" CausesValidation="False" CommandName="RowCopy" CommandArgument='<%# Eval("ID") %>' /><p id="Copia" class="TextCmdGrd"></p></li>
                                                <li id="ColCmdDelete"><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneDelete" CausesValidation="False" CommandName="RowDelete" CommandArgument='<%# Eval("ID") %>' /><p id="Elimina" class="TextCmdGrd"></p></li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="DESCRENTE" HeaderText="Ente">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DESCRDESTINATARI" HeaderText="Destinatari">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="DESCRMEZZO" HeaderText="Mezzo">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="TESTO" HeaderText="Testo">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Justify"></ItemStyle>
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Data Invio">
                            <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                            <itemstyle horizontalalign="Right"></itemstyle>
                            <ItemTemplate>
                                <asp:TextBox runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DATAINVIO")) %>' CssClass="text-right"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="NSEND" HeaderText="N.Inviati" DataFormatString="{0:N0}">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Right"></ItemStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="NREAD" HeaderText="N.Letti" DataFormatString="{0:N0}">
                            <HeaderStyle horizontalalign="Center"></HeaderStyle>
                            <ItemStyle horizontalalign="Right"></ItemStyle>
                        </asp:BoundField>
                    </Columns>
                </Grd:RibesGridView>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfIdRow" runat="server" Value="-1" />
    <asp:HiddenField ID="hfSubsetRecipient" runat="server" Value="" />
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>
