<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_IstanzeDet.aspx.cs" Inherits="OPENgovSPORTELLO.Istanze.BO_IstanzeDet" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container" >
        <div class="navbar-collapse collapse">
            <ul class="nav navbar-nav">
                <li class="navbar-brand" style="height:50px;"><a class="title" id="TitlePage"></a></li>
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
    <div class="body_page"><br />
        <div class="col-md-12 pageBO">
            <label id="OnlyNumber_error" class="text-danger usain"></label>
            <div id="divDetIstanza" class="col-md-12">
                <div class="col-md-11" id="divDetUI"></div>
                <ul class="nav navbar-nav lead-right-btn">
                    <li>
                        <div class="BottoneDiv">
                            <asp:Button runat="server" Text="" CssClass="SubmitBtn BottoneGrd BottoneNewGrd" OnClick="NewDichiarazione" />
                            <p id="NuovaUI" class="TextCmdBlack"></p>
                        </div>
                    </li>
                </ul>
           </div>
            <div id="divDatiRifCat" class="col-md-6">
                <div class="col-md-12 lead_con_barra">
                    <a id="lblHeadRifCat" class="col-md-12" onclick="ShowHideDiv(this,'divDetRifCat','Dati Istanza')" href="#">Rif.Cat.</a>
                </div>
                <div id="divDetRifCat" class="col-md-12">
                    <Grd:RibesGridView ID="GrdRifCat" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False" PageSize="20"
                        OnRowDataBound="GrdRifCatRowDataBound" OnRowCommand="GrdRifCatRowCommand">
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneClose" CausesValidation="False" CommandName="RowClose" CommandArgument='<%# Eval("ID") %>'/><p id="close" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneFolderAdd" CausesValidation="False" CommandName="RowCloseAdd" CommandArgument='<%# Eval("ID") %>'/><p id="folderadd" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Nominativo" HeaderText="Nominativo">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Dal">
                                <headerstyle horizontalalign="Center" Width="70px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="70px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDal" runat="server" Width="70px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Al">
                                <headerstyle horizontalalign="Center" Width="70px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="70px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAl" runat="server" Width="70px" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DescrCategoria" HeaderText="Cat.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Quota" HeaderText="Quota" DataFormatString="{0:N}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
            </div>
            <div id="divDatiContrib" class="col-md-6">
                <div class="col-md-12 lead_con_barra">
                    <a id="lblHeadContrib" class="col-md-12" onclick="ShowHideDiv(this,'divDetContrib','Dati Istanza')" href="#">Nominativo</a>
                </div>
                <div id="divDetContrib" class="col-md-12">
                    <Grd:RibesGridView ID="GrdContrib" runat="server" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                        AutoGenerateColumns="False" AllowPaging="False" PageSize="20"
                        OnRowDataBound="GrdContribRowDataBound" OnRowCommand="GrdContribRowCommand">
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
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneOpen" CausesValidation="False" CommandName="RowOpen" CommandArgument='<%# Eval("ID") %>'/><p id="apri" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneClose" CausesValidation="False" CommandName="RowClose" CommandArgument='<%# Eval("ID") %>'/><p id="close" class="TextCmdGrd"></p></li>
                                                    <li><asp:ImageButton runat="server" CssClass="SubmitBtn Bottone BottoneFolderAdd" CausesValidation="False" CommandName="RowCloseAdd" CommandArgument='<%# Eval("ID") %>'/><p id="folderadd" class="TextCmdGrd"></p></li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dal">
                                <headerstyle horizontalalign="Center" Width="70px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="70px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDal" runat="server" Width="70px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Al">
                                <headerstyle horizontalalign="Center" Width="70px"></headerstyle>
                                <itemstyle horizontalalign="Right" Width="70px"></itemstyle>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtAl" runat="server" Width="70px" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="RifCat" HeaderText="Ric.Cat.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Ubicazione" HeaderText="Ubicazione">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="DescrCategoria" HeaderText="Cat.">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Justify"></itemstyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Quota" HeaderText="Quota" DataFormatString="{0:N}">
                                <headerstyle horizontalalign="Center"></headerstyle>
                                <itemstyle horizontalalign="Right"></itemstyle>
                            </asp:BoundField>
                        </Columns>
                    </Grd:RibesGridView>
                </div>
            </div>
        </div>      
    </div>
</asp:Content>
