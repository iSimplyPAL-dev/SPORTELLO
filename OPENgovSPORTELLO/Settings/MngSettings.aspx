<%@ Page Title="Configura" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="MngSettings.aspx.cs" Inherits="OPENgovSPORTELLO.Settings.MngSettings" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container" >
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand" style="height:50px;"><a class="title" id="TitlePage"></a></li>
            </ul>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button ID="btnUpload" runat="server" Text="" CssClass="Bottone BottoneUpload" OnClick="Upload" /></li>
                <li class="bottoni_header"><asp:Button ID="btnCopyTo" runat="server" Text="" CssClass="Bottone BottoneRecycle" OnClick="CopyTo" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSave" OnClick="Save" CausesValidation="True" ValidationGroup="SaveDich" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSearch" OnClick="Search" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneBack" OnClick="Back" /></li>
            </ul>
        </div>
    </div>
</asp:Content>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
     <div class="container">
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
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <!--<script type="text/javascript">
        $(function () {
            $("[id$=txtMyEnte]").autocomplete({                
                source: function (request, response) {
                    var param = { keyword: $('#MainContent_txtMyEnte').val() };
                    console.log(param);
                    $.ajax({
                        url: '<%=ResolveUrl("MngSettings.aspx/GetEnti") %>',
                        data: JSON.stringify(param),
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        dataFilter: function (data) { return data; },
                        success: function (data) {
                            console.log(data.d);
                            response($.map(data.d, function (item) {
                                console.log('quà');
                                return {
                                    label: item.split('-')[0],
                                    val: item.split('-')[1]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $("[id$=hfIdEnte]").val(i.item.val);
                },
                minLength: 1
            });
        });   
    </script>-->
    <div class="body_page"><br />
        <div class="pageBO">
            <p class="lead_con_barra">
                <label id="lblTitle"></label>
                <div id="HelpUtilizzo">
                    <a href="" class="tooltip">
                        <img class="BottoneMini BottoneHelpMini" />
                        <span>
                            <iframe src=""></iframe>
                        </span>
                    </a>
                </div>
            </p>
            <asp:HiddenField ID="hfIdRow" runat="server" Value="-1" />
            <label id="lblErrorBO" class="col-md-12 text-danger usain"></label>
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
                                <asp:Button runat="server" ID="btnBackSearchEnti" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
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
                                <Columns>
                                    <asp:BoundField DataField="IDEnte" HeaderText="Codice">
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DESCRIZIONE" HeaderText="Ente">
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Provincia" HeaderText="Prov.">
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <itemtemplate>
                                            <asp:ImageButton id="imgAtt" runat="server" CssClass="SubmitBtn BottoneGrd BottoneAttachGrd" CausesValidation="False" CommandName="AttachRow" CommandArgument='<%# Eval("IDEnte") %>'></asp:ImageButton> 
                                        </itemtemplate>
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Center"></itemstyle>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>&nbsp;
                        </div>&nbsp;
                    </div>&nbsp;
                </div>&nbsp;
            </div>
            <div id="Enti">
                <div class="col-md-12">
                    <Grd:RibesGridView ID="GrdEnti" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdEntiRowDataBound" OnRowCommand="GrdEntiRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Codice">
                                <HeaderTemplate>
                                    <asp:label runat="server">Codice</asp:label>&ensp;
                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn Bottone BottoneNewGrd" CausesValidation="False" CommandName="NewRow" CommandArgument='-1'></asp:ImageButton> 
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center" width="100px"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCodice" runat="server" Text='<%# Eval("IdEnte") %>' width="50px" Enabled="false"></asp:TextBox>
                                    <asp:HiddenField ID="hfIdEnte" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderTemplate>
                                    <asp:Label runat="server">Comune</asp:Label>
                                    <a href="" target="_blank" class="tooltip HelpBOSettingsEnteCod">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src="" class="HelpBOSettingsEnteCod"></iframe>
                                        </span>
                                    </a>
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify" width="275px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtEnte" runat="server" Text='<%# Eval("Descrizione") %>' Width="225px" Enabled="false"></asp:TextBox>&ensp;
                                    <asp:ImageButton id="imgAtt" runat="server" CssClass="SubmitBtn BottoneMini BottoneFinderGrd" CausesValidation="False" CommandName="FindRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="">
                                <HeaderTemplate>
                                    <asp:Label runat="server">Patto/Unione</asp:Label>
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAmbiente" runat="server" Text='<%# Eval("Ambiente") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">Logo</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgLogo" runat="server" CssClass="SubmitBtn Bottone BottoneTownGrd" CausesValidation="False" CommandName="AttachRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">Password<br />via mail</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:CheckBox ID="chkSplitPWD" runat="server" Checked='<%# FncGrd.FormattaBoolGrd(Eval("SplitPWD")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">Tributi</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgObject" runat="server" CssClass="SubmitBtn Bottone BottoneObjectGrd" CausesValidation="False" CommandName="ObjectRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfTributi" runat="server" Value='<%# FncGrd.FormattaHideListGenGrd(Eval("ListTributi")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">Dati Mail</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgTown" runat="server" CssClass="SubmitBtn Bottone BottoneMailGrd" CausesValidation="False" CommandName="MailRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfBaseMail" runat="server" Value='<%# FncGrd.FormattaHideListMailGrd(Eval("Mail")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">Cartografia</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgCartografia" runat="server" CssClass="SubmitBtn Bottone BottoneMapGrd" CausesValidation="False" CommandName="CatastoRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfBaseCartografia" runat="server" Value='<%# FncGrd.FormattaHideListCartografiaGrd(Eval("SIT")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">Agg. Verticale</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgVerticale" runat="server" CssClass="SubmitBtn Bottone BottoneDataBaseGrd" CausesValidation="False" CommandName="VerticaleRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfVerticale" runat="server" Value='<%# FncGrd.FormattaHideListVerticaliGrd(Eval("DatiVerticali")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>                   
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label runat="server">PagoPA</asp:Label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgPagoPA" runat="server" CssClass="SubmitBtn Bottone BottoneShoppingCartGrd" CausesValidation="False" CommandName="PagoPARow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfPagoPA" runat="server" Value='<%# FncGrd.FormattaHideListPagoPAGrd(Eval("DatiPagoPA")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
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
                </div>
            </div>
            <div id="ConfigVSEnte">
                <div class="panel panel-primary">
                    <div class="col-md-12">                    
                        <div class="col-md-8">
                            <p><label id="lblEnte">Ente:</label></p>
                            <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-6" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList> 
                        </div>
                        <div class="col-md-4" id="divAnno">
                            <p><label id="lblAnno">Anno:</label></p>
                            <asp:TextBox runat="server" ID="txtAnno" class="col-md-4" AutoPostBack="true" OnTextChanged="ControlSelectedChanged"></asp:TextBox> 
                        </div>
                        <div id="lblInstructions" class="col-md-12 text-italic"></div>
                    </div>
                    &nbsp;   
                    <div class="col-md-12" id="divDest">
                        <div class="col-md-8">
                            <p><label id="lblEnteDest">Ente sul quale ribaltare:</label></p>
                            <asp:DropDownList runat="server" ID="ddlEnteDest" class="col-md-6"></asp:DropDownList> 
                            <asp:HiddenField ID="hfIsCopyTo" runat="server" Value="0" />
                        </div>&nbsp;    
                        <div class="col-md-4">
                            <p><label id="lblAnnoDest">Anno sul quale ribaltare:</label></p>
                            <asp:TextBox runat="server" ID="txtAnnoDest" class="col-md-4"></asp:TextBox> 
                        </div>
                    </div>     
                </div>
                <div id="GeneralCategory">
                    <Grd:RibesGridView ID="GrdConfig" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdConfigRowDataBound" OnRowCommand="GrdConfigRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Codice">
                                <HeaderTemplate>
                                    <asp:label runat="server">Codice</asp:label>
                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn Bottone BottoneNewGrd" CausesValidation="False" CommandName="NewConf" CommandArgument='-1'></asp:ImageButton> 
                                </HeaderTemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCodice" runat="server" Text='<%# Eval("Codice") %>'></asp:TextBox>
                                     <asp:HiddenField ID="hfIdSetting" runat="server" Value='<%# Eval("ID") %>' />
                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descrizione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify" Width="70%"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescrizione" runat="server" Text='<%# Eval("Descrizione") %>' style="width:100%"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Codice Banca Dati Esterna">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtIdOrg" runat="server" Text='<%# Eval("IdOrg") %>'></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <itemtemplate>
                                    <asp:ImageButton id="imgDelete" runat="server" CssClass="Bottone BottoneDeleteGrd" CausesValidation="False" CommandName="DeleteConf" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Attenzione!\nStai per eliminare una voce:\nVuoi procedere?')"></asp:ImageButton> 
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
                <div id="DocAllegati">
                    <Grd:RibesGridView ID="GrdDoc" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowCommand="GrdDocRowCommand" OnRowDataBound="GrdDocRowDataBound">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Tributo">
                                <HeaderTemplate>
                                    <asp:label runat="server">Tributo</asp:label>&ensp;
                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn Bottone BottoneNewGrd" CausesValidation="False" CommandName="NewConf" CommandArgument='-1'></asp:ImageButton> 
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTributo" runat="server" DataSource='<%# LoadPageCombo("TRIBUTO") %>' DataTextField="Descrizione" DataValueField="Codice" SelectedValue='<%# Eval("IdTributo") %>' AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList>
                                    <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo Istanza">
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlIstanze" runat="server"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Documento">
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDoc" runat="server" Text='<%# Eval("Documento") %>' Width="100%"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>                        
                            <asp:TemplateField HeaderText="Obbligatorio">
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkIsConstraint" runat="server" Checked='<%# Eval("IsObbligatorio") %>'></asp:CheckBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <itemtemplate>
                                    <asp:ImageButton id="imgDelete" runat="server" CssClass="Bottone BottoneDeleteGrd" CausesValidation="False" CommandName="DeleteRow" CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Attenzione!\nStai per eliminare una voce:\nVuoi procedere?');"></asp:ImageButton> 
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
                <div id="Tariffe">
                    <Grd:RibesGridView ID="GrdAliquote" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdAliquoteRowDataBound" OnRowCommand="GrdAliquoteRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Codice">
                                <HeaderTemplate>
                                    <label id="lblHeaderGrdAliquoteCod">Codice</label>
                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn Bottone BottoneNewGrd" CausesValidation="False" CommandName="NewRow" CommandArgument='-1'></asp:ImageButton> 
                                </HeaderTemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtCodice" runat="server" Text='<%# Eval("Codice") %>' CssClass="col-md-11" style="min-width:90px"></asp:TextBox>
                                    <asp:DropDownList ID="ddlTipologia" runat="server" CssClass="col-md-11"></asp:DropDownList>
                                    <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                               </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descrizione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify" Width="70%"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDescrizione" runat="server" Text='<%# Eval("Descrizione") %>' CssClass="col-md-11"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="% Aliquota">
                                <HeaderTemplate>
                                    <label id="lblHeaderGrdAliquoteAliq"></label>
                                </HeaderTemplate>
                                <headerstyle horizontalalign="Center" Width="100px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="100px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtValore" runat="server" Text='<%# Eval("Valore") %>' CssClass="col-md-11 text-right OnlyNumber" style="min-width:80px"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="%">
                                <HeaderTemplate>
                                    <label id="lblHeaderGrdAliquoteProp"></label>
                                </HeaderTemplate>
                                <headerstyle horizontalalign="Center" Width="100px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="100px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPercProprietario" runat="server" Text='<%# Eval("PercProprietario") %>' CssClass="col-md-11 text-right OnlyNumber PercProprietario"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="%">
                                <HeaderTemplate>
                                    <label id="lblHeaderGrdAliquoteInq"></label>
                                </HeaderTemplate>
                                <headerstyle horizontalalign="Center" Width="100px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="100px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPercInquilino" runat="server" Text='<%# Eval("PercInquilino") %>' CssClass="col-md-11 text-right OnlyNumber PercInquilino"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Codice Banca Dati Esterna">
                                <HeaderTemplate>
                                    <label id="lblHeaderGrdAliquoteCodEst"></label>
                                </HeaderTemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtIdOrg" runat="server" Text='<%# Eval("IdOrg") %>' CssClass="col-md-9" style="min-width:80px;max-width:100px"></asp:TextBox>
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
                </div>
            </div>
            <div id="Operatori">
                <div class="col-md-12">
                    <Grd:RibesGridView ID="GrdOperatori" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False"
                        ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                        OnRowDataBound="GrdOperatoriRowDataBound" onrowcommand="GrdOperatoriRowCommand">
                        <PagerSettings Position="Bottom"></PagerSettings>
                        <FooterStyle CssClass="CartListFooter"></FooterStyle>
                        <RowStyle CssClass="CartListItemEdit"></RowStyle>
                        <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                        <Columns>
                            <asp:TemplateField HeaderText="Operatore">
                                <HeaderTemplate>
                                    <asp:label runat="server">Operatore</asp:label>&ensp;
                                    <asp:ImageButton id="imgNew" runat="server" CssClass="SubmitBtn Bottone BottoneNewGrd" CausesValidation="False" CommandName="NewConf" CommandArgument='-1'></asp:ImageButton> 
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify" width="275px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtOperatore" runat="server" Text='<%# Eval("NameUser") %>' Width="100%"></asp:TextBox>
                                    <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Profilo">
                                <HeaderTemplate>
                                    <label runat="server">Profilo</label>&ensp;
                                    <a href="" target="_blank" class="tooltip HelpBOSettingsEnteOperatoreProfilo">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src="" class="HelpBOSettingsEnteOperatoreProfilo"></iframe>
                                        </span>
                                    </a>
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <ItemStyle horizontalalign="Justify" Width="200px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlProfilo" runat="server" DataSource='<%# LoadPageCombo("PROFILO") %>' DataTextField="Descrizione" DataValueField="Codice" SelectedValue='<%# Eval("IdTipoProfilo") %>' Width="100%"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:label runat="server">Password</asp:label>
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <itemtemplate>
                                    <asp:ImageButton id="imgKey" runat="server" CssClass="SubmitBtn Bottone BottoneKey" CausesValidation="False" CommandName="ChangePassword" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton>
                                    <asp:HiddenField ID="hfKey" runat="server" />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:label runat="server">Enti</asp:label>
                                </HeaderTemplate>
                                <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                <itemtemplate>
                                    <asp:ImageButton id="imgTown" runat="server" CssClass="SubmitBtn Bottone BottoneTownGrd" CausesValidation="False" CommandName="TownRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfEnti" runat="server" Value='<%# FncGrd.FormattaHideIdListGrd(Eval("Enti")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:label runat="server">Tributi</asp:label>
                                </HeaderTemplate>
                                <itemtemplate>
                                    <asp:ImageButton id="imgObject" runat="server" CssClass="SubmitBtn Bottone BottoneObjectGrd" CausesValidation="False" CommandName="ObjectRow" CommandArgument='<%# Eval("ID") %>'></asp:ImageButton> 
                                    <asp:HiddenField ID="hfTributi" runat="server" Value='<%# FncGrd.FormattaHideIdListGrd(Eval("Tributi")) %>' />
                                </itemtemplate>
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Center"></itemstyle>
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
                </div>                
                <div id="PWDOperatore" class="modal">
                    <div class="modal-dialog">
                        <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                            <div class="panel panel-primary">
                                <div class="col-md-4">
                                    <label class="lead" id="lblIntestPWDOperatore"></label>
                                </div>
                                <div class="nav navbar-nav lead-right-btn">
                                    <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneSave" OnClick="PWDOperatore" />&ensp;
                                    <asp:Button runat="server" ID="btnBackPWDOperatore" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class="col-md-12">
                                    <p><label class="col-md-11">Posta elettronica</label></p>
                                    <asp:TextBox ID="txtNameUser" runat="server" CssClass="col-md-11" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <p><label class="col-md-11">Password</label></p>
                                    <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="col-md-11" />
                                </div>
                            </div>&nbsp;
                        </div>&nbsp;
                    </div>&nbsp;
                </div>
            </div>
            <div id="UserDetail" class="modal">
                <div class="modal-dialog">
                    <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                        <div class="panel panel-primary">
                            <div class="col-md-3">
                                <label class="lead" id="lblIntestUserDetail"></label>
                            </div>
                            <div class="nav navbar-nav lead-right-btn">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneAttach" OnClick="AttachUserDetail" />&ensp;
                                <asp:Button runat="server" ID="btnBackUserDetail" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <Grd:RibesGridView ID="GrdUserDetail" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
                                ErrorStyle-Font-Bold="True" ErrorStyle-ForeColor="Red"
                                OnPageIndexChanging="GrdUserDetailPageIndexChanging">
                                <PagerSettings Position="Bottom"></PagerSettings>
                                <FooterStyle CssClass="CartListFooter"></FooterStyle>
                                <RowStyle CssClass="CartListItem"></RowStyle>
                                <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                                <Columns>
                                    <asp:BoundField DataField="DESCRIZIONE" HeaderText="Descrizione">
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Justify"></ItemStyle>
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Sel.">
                                        <HeaderStyle horizontalalign="Center"></HeaderStyle>
                                        <ItemStyle horizontalalign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSel" runat="server" Checked='<%# FncGrd.FormattaBoolGrd(Eval("IsActive")) %>'></asp:CheckBox>
                                            <asp:HiddenField ID="hfIdRow" runat="server" Value='<%# Eval("ID") %>' />
                                            <asp:HiddenField ID="hfCodice" runat="server" Value='<%# Eval("CODICE") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Grd:RibesGridView>&nbsp;
                        </div>&nbsp;
                        <asp:HiddenField ID="hfTypeUserDetail" runat="server" Value="-1" />
                    </div>&nbsp;
                </div>&nbsp;
            </div>&nbsp;
            <div id="EnteMailDetail" class="modal">
                <div class="modal-dialog">
                    <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                        <div class="panel panel-primary">
                            <label class="lead">E-Mail</label>
                            <div class="nav navbar-nav lead-right-btn">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneAttach" OnClick="AttachEnteMailDetail" />&ensp;
                                <asp:Button runat="server" ID="btnBackEnteMailDetail" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                            </div>
                        </div>
                        <div class="col-md-12"> 
							<div class="col-md-12 lead">Configurazione FrontOffice</div>
                            <div class="col-md-6">
                                <p><label>Mail di invio</label></p>
                                <asp:TextBox id="txtMailSender" runat="server" TextMode="Email" CssClass="validEmail col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <p><label>Nome Mail di invio</label></p>
                                <asp:TextBox id="txtMailSenderName" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <p><label>Server Mail</label></p>
                                <asp:TextBox id="txtMailServer" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <p><label>Password</label></p>
                                <asp:TextBox id="txtMailPassword" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <p><label>SSL</label></p>
                                <asp:RadioButton id="optYes" runat="server" Text="Si" GroupName="optSSL"></asp:RadioButton>
                                <asp:RadioButton id="optNo" runat="server" Text="No" GroupName="optSSL" Checked="true"></asp:RadioButton>
                            </div>
                            <div class="col-md-1">
                                <p><label>Porta</label></p>
                                <asp:TextBox id="txtMailServerPort" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
							<div class="col-md-12 lead">Configurazione Mail del Comune</div>
							<div class="col-md-6">
                                <p><label>Mail BackOffice</label></p>
                                <asp:TextBox id="txtMailBackOffice" runat="server" TextMode="Email" CssClass="validEmail col-md-11"></asp:TextBox>
                            </div>
							<div class="col-md-6">
                                <p><label>Mail Protocollo</label></p>
                                <asp:TextBox id="txtMailProtocollo" runat="server" TextMode="Email" CssClass="validEmail col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <p><label>Mail di archiviazione</label></p>
                                <asp:TextBox id="txtMailArchive" runat="server" TextMode="Email" CssClass="validEmail col-md-11"></asp:TextBox>
                            </div>
							<div class="col-md-6 hidden">
                                <p><label>Mail del Comune</label></p>
                                <asp:TextBox id="txtMailEnte" runat="server" TextMode="Email" CssClass="validEmail col-md-11"></asp:TextBox>
                            </div>
							<div class="col-md-12 lead">Configurazione Mail di Errore</div>
                            <div class="col-md-12">
                                <p><label>Mail di Errore</label></p>
                                <asp:TextBox id="txtMailWarningRecipient" runat="server" TextMode="Email" CssClass="validEmail col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                <p><label>Oggetto Mail di Errore</label></p>
                                <asp:TextBox id="txtMailWarningSubject" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                <p><label>Testo Mail di Errore</label></p>
                                <asp:TextBox id="txtMailWarningMessage" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-12 hidden">
                                <p><label>Testo Mail di Errore invio</label></p>
                                <asp:TextBox id="txtMailSendErrorMessage" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>                           
                        </div>&nbsp;
                    </div>&nbsp;
                </div>&nbsp;
            </div>&nbsp;
            <div id="EnteCartografiaDetail" class="modal">
                <div class="modal-dialog">
                    <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                        <div class="panel panel-primary">
                            <label class="lead">SIT</label>
                            <div class="nav navbar-nav lead-right-btn">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneAttach" OnClick="AttachEnteCartografiaDetail" />&ensp;
                                <asp:Button runat="server" ID="btnBackEnteCartografiaDetail" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <p><label>SIT</label></p>
                                <asp:RadioButton id="optCartoYes" runat="server" Text="Si" GroupName="optCartografia"></asp:RadioButton>
                                <asp:RadioButton id="optCartoNo" runat="server" Text="No" GroupName="optCartografia" Checked="true"></asp:RadioButton>
                            </div> 
                            <div class="col-md-12">
                                <p><label>Url</label></p>
                                <asp:TextBox id="txtCartoUrl" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                <p><label>Url Autorizzazione</label></p>
                                <asp:TextBox id="txtCartoUrlAuth" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-12">
                                <p><label>Token</label></p>
                                <asp:TextBox id="txtCartoToken" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>                      
                        </div>&nbsp;
                    </div>&nbsp;
                </div>&nbsp;
            </div>&nbsp;
            <div id="EnteVerticaliDetail" class="modal">
                <div class="modal-dialog">
                    <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                        <div class="panel panel-primary">
                            <label class="lead">Verticali</label>
                            <div class="nav navbar-nav lead-right-btn">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneAttach" OnClick="AttachEnteVerticaliDetail" />&ensp;
                                <asp:Button runat="server" ID="btnBackEnteVerticaliDetail" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <p><label>Tipo banca dati</label></p>
                                <asp:RadioButton id="optVertInt" runat="server" Text="Interna" GroupName="optVert"></asp:RadioButton>
                                <asp:RadioButton id="optVertEst" runat="server" Text="Esterna" GroupName="optVert" Checked="true"></asp:RadioButton>
                            </div>
                            <div class="col-md-12">
                                <p><label>Fornitore</label></p>
                                <asp:DropDownList ID="ddlFornitore" runat="server" Width="100%"></asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <p>
                                    <label>Ultimo Anno Verticale ICI</label>
                                    <a href="" target="_blank" class="tooltip HelpBOSettingsEnteAnnoICI">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src="" class="HelpBOSettingsEnteAnnoICI"></iframe>
                                        </span>
                                    </a>
                                </p>
                                <asp:TextBox ID="txtAnnoVertICI" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>                        
                            <div class="col-md-4">
                                <p>
                                    <label runat="server">Anni Uso Gratuito</label>&ensp;
                                    <a href="" target="_blank" class="tooltip HelpBOSettingsEnteUsoGrat">
                                        <img class="BottoneMini BottoneHelpMini" />
                                        <span>
                                            <iframe src=""></iframe>
                                        </span>
                                    </a>
                                </p>
                                <asp:TextBox ID="txtAnniUsoGrat" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>                        
                            <div class="col-md-4">
                                <p><label>Agg. da verticali</label></p>
                                <asp:TextBox ID="txtDataAgg" runat="server" ReadOnly="true" CssClass="col-md-11"></asp:TextBox>
                            </div>
                        </div>&nbsp;
                    </div>&nbsp;
                </div>&nbsp;
            </div>&nbsp;
            <div id="EntePagoPADetail" class="modal">
                <div class="modal-dialog">
                    <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                        <div class="panel panel-primary">
                            <label class="lead">PagoPA</label>
                            <div class="nav navbar-nav lead-right-btn">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneAttach" OnClick="AttachEntePagoPADetail" />&ensp;
                                <asp:Button runat="server" ID="btnBackEntePagoPADetail" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="col-md-6">
                                <p><label>CARTId</label></p>
                                <asp:TextBox id="txtCARTId" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <p><label>CARTSys</label></p>
                                <asp:TextBox id="txtCARTSys" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <p><label>IBAN</label></p>
                                <asp:TextBox id="txtIBAN" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div> 
                            <div class="col-md-6">
                                <p><label>Descrizione</label></p>
                                <asp:TextBox id="txtDescrIBAN" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>  
                            <div class="col-md-6">
                                <p><label>Identificativo Riscossore</label></p>
                                <asp:TextBox id="txtIdRiscossore" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>  
                            <div class="col-md-6">
                                <p><label>Descrizione</label></p>
                                <asp:TextBox id="txtDescrRiscossore" runat="server" CssClass="col-md-11"></asp:TextBox>
                            </div>                       
                        </div>&nbsp;
                    </div>&nbsp;
                </div>&nbsp;
            </div>&nbsp;
            <div id="EnteLogoDetail" class="modal">
                <div class="modal-dialog">
                    <div class="modal-content" style="padding:10px 10px 10px 10px;overflow:auto;">
                        <div class="panel panel-primary">
                            <label id="lblEnteFileAttach" class="lead">Logo</label>
                            <div class="nav navbar-nav lead-right-btn">
                                <asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottoneAttach" OnClick="AttachEnteLogoDetail" />&ensp;
                                <asp:Button runat="server" ID="btnBackEnteLogoDetail" Text="" CssClass="SubmitBtn Bottone BottoneBack" OnClick="Back" />
                            </div>
                        </div>
                        <div id="divDescrFlussiCarico">
                            <p class="col-md-12 text-11 text-italic">Il nome del flusso deve iniziare con il codice dell'ente e deve essere in formato <b>.zip</b>.<br /><br />
                                Il carico può durare qualche minuto perchè comprende l'unzip dei file e la conversione dei flussi in base al fornitore prescelto.<br />
                                Nello zip non devono essere presenti files di dimensione 0KB.</p><br />
                            <label id="lblEsitoUploadFlussi" class="usain"></label>
                        </div>
                        <div class="col-md-3">
                           <Grd:RibesGridView ID="GrdLogo" runat="server" BorderStyle="None" 
                                BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                                AutoGenerateColumns="False" AllowPaging="False" PageSize="20"
                                OnRowDataBound="GrdLogoRowDataBound" OnRowCommand="GrdLogoRowCommand">
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
                                                            <li><asp:ImageButton runat="server" CssClass="Bottone BottoneAttach" CausesValidation="False" CommandName="RowDownload" CommandArgument='<%# Eval("ID") %>'/><p id="download" class="TextCmdGrd"></p></li>
                                                        </ul>
                                                    </div>
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NAMELOGO" HeaderText="Logo">
                                        <headerstyle horizontalalign="Center"></headerstyle>
                                        <itemstyle horizontalalign="Justify"></itemstyle>
                                    </asp:BoundField>
                                </Columns>
                            </Grd:RibesGridView>
                            <br /><br />
                        </div>
                        <div class="col-md-12">
                            <div id="FileToUpload">
                                <asp:FileUpload ID="MIOfileUpload" runat="server" AllowMultiple="true" />
                                <asp:RequiredFieldValidator ID="rfvFileUpload" runat="server" ControlToValidate="MIOfileUpload" ErrorMessage="Seleziona il file da importare" ValidationGroup="UploadValidation"></asp:RequiredFieldValidator>
                            </div>
                        </div>&nbsp;
                    </div>&nbsp;
                </div>&nbsp;
            </div>&nbsp;
       </div>
    </div>
</asp:Content>
