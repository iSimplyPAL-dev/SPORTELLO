<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Dich.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.OSAP.Dich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p id="PageTitle" class="navbar-brand col-md-6"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;<a class="OSAP text-white" id="TitlePage"></a></p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" ID="CmdSave" Text="" CssClass="Bottone BottoneSave" OnClientClick="return FieldValidatorOSAP();" OnClick="Save"/></li>
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
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneOK" OnClientClick="return Azzera();"></asp:Button>
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
                    <a class="lead_header col-md-2" onclick="ShowHideDiv(this,'divDetUI','Dati Generali')" href="#"></a>
                    <ul class="nav navbar-nav lead-right-btn hidden">
                        <li id="ColCmdClose">
                            <div class="BottoneDiv">
                                <asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneClose" OnClick="Close" />
                                <p id="cessazione" class="col-md-12 TextCmdBlack"></p>
                            </div>
                        </li>
                    </ul>
                </div>  
                <div id="divDetUI" class="col-md-12">
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <p><label id="lblTipoAtto">Tipo</label></p>
                            <asp:DropDownList runat="server" ID="ddlTipoAtto" CssClass="Azzera col-md-11" TabIndex="1"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <p><label id="lblDataAtto">Data</label></p>
                            <asp:TextBox runat="server" ID="txtDataAtto" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-6 text-right" TabIndex="2"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <p><label id="lblNumAtto">N.</label></p>
                            <asp:TextBox runat="server" ID="txtNumAtto" CssClass="Azzera col-md-11" TabIndex="3"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <p><label id="lblRichiedente">Richiedente</label></p>
                            <asp:DropDownList runat="server" ID="ddlRichiedente" CssClass="Azzera col-md-12" TabIndex="4"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <p><label id="lblTributo">Tributo</label></p>
                            <asp:DropDownList runat="server" ID="ddlTributo" CssClass="Azzera col-md-12" TabIndex="5"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <p class="lead">Dati Immobile</p>
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <div class="col-md-5">
                                    <p>
                                        <label id="lblVia">Ubicazione - Via:</label>
                                        <img class="SubmitBtn BottoneMini BottoneMapMarker" onclick="Scopri('SearchStradario');"/>
                                    </p>
                                    <asp:TextBox runat="server" ID="txtVia" CssClass="Azzera col-md-12 txtVia" CausesValidation="True" ValidationGroup="SaveDich"></asp:TextBox>
                                </div>
                                <div class="col-md-2">                        
                                    <div>
                                        <p><label id="lblCivico">Civico:</label></p>
                                        <asp:TextBox runat="server" ID="txtCivico" CssClass="Azzera col-md-12" CausesValidation="True" ValidationGroup="SaveDich" TabIndex="6"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    &nbsp;
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-3">
                                    <p><label id="lblDataInizio">Data Inizio:</label><label id="lblDataInizioORG" runat="server" class="text-italic"></label></p>
                                    <asp:TextBox runat="server" ID="txtDataInizio" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-4 text-right" TabIndex="7"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <p><label id="lblDataFine">Data Fine:</label></p>
                                    <asp:TextBox runat="server" ID="txtDataFine" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-4 text-right" TabIndex="8"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <p><label id="lblTipoDurata">Tipo Durata</label></p>
                                    <asp:DropDownList runat="server" ID="ddlTipoDurata" CssClass="Azzera col-md-6" TabIndex="9"></asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <p><label id="lblDurata">Durata</label></p>
                                    <asp:TextBox runat="server" ID="txtDurata" CssClass="Azzera col-md-4 OnlyNumber text-right" TabIndex="10"></asp:TextBox>
                                </div>
                            </div>
                        </div>      
                    </div>
                    <div class="col-md-12">
                        <p class="lead">Dati Per Calcolo</p>
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <div class="col-md-4">
                                    <p><label id="lblCat">Categoria:</label></p>
                                    <asp:DropDownList ID="ddlCat" runat="server" CssClass="Azzera col-md-10" TabIndex="11"></asp:DropDownList>
                                </div>
                                <div class="col-md-7">
                                    <p><label id="lblOccupazione">Occupazione:</label></p>
                                    <asp:DropDownList ID="ddlOccupazione" runat="server" CssClass="Azzera col-md-11" TabIndex="12"></asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <p><label id="lblAttrazione">Attrazione:</label></p>
                                    <asp:CheckBox runat="server" ID="chkAttrazione" CssClass="Azzera col-md-12" Text=""  TabIndex="13"/>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-3">
                                    <p><label id="lblMisuraCons">Misura Consistenza:</label></p>
                                    <asp:DropDownList ID="ddlMisuraCons" runat="server" CssClass="Azzera col-md-10" TabIndex="14"></asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <p><label id="lblCons">Consistenza:</label></p>
                                    <asp:TextBox ID="txtCons" runat="server" CssClass="Azzera col-md-4 OnlyNumber text-right" TabIndex="15"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <p><label id="lblPercMagg">% Maggiorazione:</label></p>
                                    <asp:TextBox runat="server" ID="txtPercMagg" CssClass="Azzera col-md-4 OnlyNumber text-right" TabIndex="16"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <p><label id="lblImpDetraz">Imp. Detrazione:</label></p>
                                    <asp:TextBox runat="server" ID="txtImpDetraz" CssClass="Azzera col-md-6 OnlyNumber text-right" TabIndex="17"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <p><label id="lblAgevolazioni">Agevolazioni:</label></p>
                                <Grd:RibesGridView ID="GrdAgevolazioni" runat="server" BorderStyle="None" 
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
                                                <asp:CheckBox ID="chkSel" runat="server" TabIndex="18"/>
                                                <asp:HiddenField ID="hfIdAgevolazione" runat="server" Value='<%# Eval("ID") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="DESCRIZIONE" HeaderText="Agevolazione">
                                            <headerstyle horizontalalign="Center"></headerstyle>
                                            <itemstyle horizontalalign="Justify"></itemstyle>
                                        </asp:BoundField>
                                    </Columns>
                                </Grd:RibesGridView>
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
                        <asp:TextBox ID="txtMotivazione" runat="server" TextMode="MultiLine" Width="100%" CssClass="Azzera" TabIndex="19"></asp:TextBox>
                    </div>
                    <div class="col-md-12">
                        <asp:CheckBox ID="chkAllegati" runat="server" CssClass="Input_Label_bold" Text="Allego Documentazione" AutoPostBack="true" OnCheckedChanged="GestAllegati" />
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
</asp:Content>
