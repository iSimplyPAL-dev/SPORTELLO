<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Dich.aspx.cs" Inherits="OPENgovSPORTELLO.Dichiarazioni.ICP.Dich" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p id="PageTitle" class="navbar-brand col-md-6"><a class="Tributi text-white">Tributi</a>&ensp;-&ensp;<a class="ICP text-white" id="TitlePage">Consultazione ICP</a>&ensp;-&ensp;Immobile</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" ID="CmdSave" Text="" CssClass="Bottone BottoneSave" OnClientClick="return FieldValidatorICP();" OnClick="Save"/></li>
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
                </div>  
                <div id="divDetUI" class="col-md-12">
                    <div class="col-md-12">
                        <div class="col-md-6">
                            <p><label id="lblDataDic">Data Dichiarazione:</label></p>
                            <asp:TextBox runat="server" ID="txtDataAtto" onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="Azzera col-md-3 text-right" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="col-md-6">
                            <p><label id="lblNumDic">N. Dichiarazione:</label></p>
                            <asp:TextBox runat="server" ID="txtNumAtto" CssClass="Azzera col-md-11" TabIndex="2"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-md-12">
                        <p class="lead">Dati Per Calcolo</p>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdUI" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnRowCommand="GrdUIRowCommand" OnRowDataBound="GrdUIRowDataBound">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItemEdit"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:label runat="server" ID="lblVia">Ubicazione - Via</asp:label>
                                            <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn BottoneGrd BottoneNewGrd" CausesValidation="False" CommandName="NewRow" CommandArgument='-1'></asp:ImageButton> 
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify" width="175px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtVia" runat="server" Text='<%# Eval("VIA") %>' CssClass="txtVia" Width="125px"></asp:TextBox>&ensp;
                                            <asp:ImageButton id="imgAtt" runat="server" CssClass="SubmitBtn BottoneGrd BottoneMapMarker" CausesValidation="False" CommandName="FindRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                                            <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                            <asp:HiddenField ID="hfIdVia" runat="server" Value="-1" /> 
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:label runat="server" Width="50px">Civico</asp:label>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right" Width="50px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCivico" runat="server" Text='<%# Eval("civico") %>' CssClass="text-right" Width="50px" TabIndex="3"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:label runat="server">Tipologia</asp:label>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTipologia" runat="server" TabIndex="4"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:label runat="server">Caratteristica</asp:label>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlCaratteristica" runat="server" Width="100%" TabIndex="5"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:label runat="server">Tipo Durata </asp:label>
                                        </HeaderTemplate>
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlTipoDurata" runat="server" Width="100%" TabIndex="6"></asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>                  
                                    <asp:TemplateField HeaderText="Mezzo">
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtMezzo" runat="server" Text='<%# Eval("Mezzo") %>' Width="100%" TabIndex="7"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>                
                                    <asp:TemplateField HeaderText="Dal">
                                        <HeaderStyle horizontalalign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right" Width="70px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDataInizio" Width="70px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("datainizio")) %>' onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="text-right" TabIndex="8"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>                
                                    <asp:TemplateField HeaderText="Al">
                                        <HeaderStyle horizontalalign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right" Width="70px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDataFine" Width="70px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("datafine")) %>' onblur="txtDateLostfocus(this);VerificaData(this);" onfocus="txtDateGotfocus(this);" CssClass="text-right" TabIndex="9"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>                
                                    <asp:TemplateField HeaderText="Qta">
                                        <HeaderStyle horizontalalign="Center" Width="50px"></HeaderStyle>
                                        <ItemStyle horizontalalign="Right" Width="50px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtQta" runat="server" Text='<%# Eval("qta") %>' Width="100%" CssClass="OnlyNumber text-right" onblur="LoadQta();" TabIndex="10">0</asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <itemtemplate>
                                            <asp:ImageButton id="imgDelete" runat="server" CssClass="Bottone BottoneDeleteGrd" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Attenzione!\nStai per eliminare una voce:\nVuoi procedere?')"></asp:ImageButton> 
                                        </itemtemplate>
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Center"></itemstyle>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>
                            <div class="col-md-12 text-right">
                                <p><label id="lblTotQta" class="usain"></label></p>
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
                        <asp:TextBox ID="txtMotivazione" runat="server" TextMode="MultiLine" Width="100%" CssClass="Azzera" TabIndex="11"></asp:TextBox>
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
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
    <asp:HiddenField ID="hfIdRow" runat="server" Value="-1" />
</asp:Content>
