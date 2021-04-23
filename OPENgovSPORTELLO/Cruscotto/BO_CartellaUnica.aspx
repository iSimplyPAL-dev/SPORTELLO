<%@ Page Title="" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="BO_CartellaUnica.aspx.cs" Inherits="OPENgovSPORTELLO.Cruscotto.BO_CartellaUnica" %>
<%@ Register Assembly="Ribes.OPENgov.WebControls" Namespace="Ribes.OPENgov.WebControls" TagPrefix="Grd" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadTitleContent" runat="server">
    <div class="container">
        <div class="navbar-collapse collapse">
            <p class="navbar-brand">Cartella Unica</p>
            <ul class="nav navbar-nav navbar-right-btn">
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="SubmitBtn Bottone BottonePrint" onclick="stampaReport" /></li>
                <li class="bottoni_header"><asp:Button runat="server" Text="" CssClass="Bottone BottoneSearch" OnClick="Search" /></li>              
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
    <div class="body_page"><br />
        <div class="col-md-12 pageBO">
            <label id="OnlyNumber_error" class="text-danger usain"></label>
            <div class="panel panel-primary col-md-12">
                <div class="col-md-12">                                        
                    <div class="col-md-2">
                        <p><label id="lblEnte">Ente:</label></p>
                        <asp:DropDownList runat="server" ID="ddlEnte" class="col-md-11 Azzera"></asp:DropDownList> 
                    </div>   
                    <div class="col-md-2">
                        <p><label id="lblNominativo">Nominativo:</label></p>
                        <asp:TextBox runat="server" ID="txtNominativo" class="col-md-11"></asp:TextBox> 
                    </div>  
                    <div class="col-md-2">
                        <p><label id="lblCFPIVA">Cod.Fiscale/P.IVA:</label></p>
                        <asp:TextBox runat="server" ID="txtCFPIVA" class="col-md-11"></asp:TextBox> 
                    </div>  
                </div>
            </div>     
            <div id="divReport" class="col-md-12">
                <div class="jumbotronAnag" style="top:188px"></div>
                <div id="div8852" class="col-md-12">
                    <div class="col-md-12 lead_Emphasized">Situazione Dichiarata IMU</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDich8852" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="UBICAZIONE" HeaderText="Ubicazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RIFCAT" HeaderText="Fg - Num - Sub">
                                    <headerstyle horizontalalign="Center" Width="150px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="150px"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Dal">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Al">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAl" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CODCATEGORIA" HeaderText="Cat.">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RENDITAVALORE" HeaderText="Rendita/Valore" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PERCPOSSESSO" HeaderText="%Pos." DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRUTILIZZO" HeaderText="Utilizzo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Dovuto IMU</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDovuto8852" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TRIBUTO" HeaderText="Codice Tributo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="IMPORTO" HeaderText="Importo" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NFAB" HeaderText="Numero Fabbricati" DataFormatString="{0:0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Versato IMU</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdPagato8852" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Versamento">
                                    <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="120px" Text='<%# FncGrd.FormattaDataGrd(Eval("data")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TIPO" HeaderText="Tipo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTALE" HeaderText="Pagato" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
               </div>
                <div id="divTASI" class="col-md-12">
                    <div class="col-md-12 lead_Emphasized">Situazione Dichiarata TASI</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDichTASI" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="UBICAZIONE" HeaderText="Ubicazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RIFCAT" HeaderText="Fg - Num - Sub">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Dal">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Al">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAl" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CODCATEGORIA" HeaderText="Cat.">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RENDITAVALORE" HeaderText="Rendita/Valore" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PERCPOSSESSO" HeaderText="%Pos." DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRUTILIZZO" HeaderText="Utilizzo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TIPODICHIARANTE" HeaderText="Tipo Dichiarante">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Dovuto TASI</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDovutoTASI" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TRIBUTO" HeaderText="Codice Tributo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="IMPORTO" HeaderText="Importo" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NFAB" HeaderText="Numero Fabbricati" DataFormatString="{0:0}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Versato TASI</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdPagatoTASI" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Versamento">
                                    <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="120px" Text='<%# FncGrd.FormattaDataGrd(Eval("data")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TIPO" HeaderText="Tipo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTALE" HeaderText="Pagato" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                </div>
                <div id="div0434" class="col-md-12">
                    <div class="col-md-12 lead_Emphasized">Situazione Dichiarata TARI</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDich0434" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="UBICAZIONE" HeaderText="Ubicazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="RIFCAT" HeaderText="Fg - Num - Sub">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Dal">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("DAL")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Al">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAl" runat="server" Width="80px" Text='<%# FncGrd.FormattaDataGrd(Eval("AL")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="CODCATEGORIA" HeaderText="Cat.">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NC" HeaderText="NC Fissa/NC Variabile">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Center"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="MQ" HeaderText="MQ" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Dovuto TARI</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDovuto0434" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>						
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="FISSA" HeaderText="Imposta Fissa" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="VARIABILE" HeaderText="Imposta Variabile" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PROVINCIALE" HeaderText="Provinciale" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTALE" HeaderText="Totale" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Versato TARI</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdPagato0434" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Versamento">
                                    <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="120px" Text='<%# FncGrd.FormattaDataGrd(Eval("data")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TOTALE" HeaderText="Pagato" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
               </div>
                <div id="div0453" class="col-md-12">
                    <div class="col-md-12 lead_Emphasized">Situazione Dichiarata OSAP</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDich0453" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="UBICAZIONE" HeaderText="Ubicazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TIPOOCCUPAZIONE" HeaderText="Occupazione">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CODCATEGORIA" HeaderText="Cat.">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DURATA" HeaderText="Durata">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="CONSISTENZA" HeaderText="Consistenza">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Dovuto OSAP</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdDovuto0453" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>						
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="TOTALE" HeaderText="Totale" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
                    <div class="col-md-12 lead_Emphasized">Situazione Versato OSAP</div>
                    <div class="col-md-12">
                        <Grd:RibesGridView ID="GrdPagato0453" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false">
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="NAVVISO" HeaderText="N.Avviso">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Data Versamento">
                                    <headerstyle horizontalalign="Center" Width="120px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="120px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtDal" runat="server" Width="120px" Text='<%# FncGrd.FormattaDataGrd(Eval("data")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="TOTALE" HeaderText="Pagato" DataFormatString="{0:N}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                            </Columns>
                        </Grd:RibesGridView>
                    </div>
               </div>
                <div id="div9999" class="col-md-12">
                    <div class="col-md-12 lead_con_barra">
                        <div class="col-md-4 lead_header">Accertamenti</div>
                    </div>
                    <div class="col-md-4 lead_Emphasized">Atti</div>
                    <div id="divDich9999" class="col-md-12">        
                        <Grd:RibesGridView ID="GrdDich9999" runat="server" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" Width="100%"
                            AutoGenerateColumns="False" AllowPaging="false" >
                            <PagerSettings Position="Bottom"></PagerSettings>
                            <FooterStyle CssClass="CartListFooter"></FooterStyle>
                            <RowStyle CssClass="CartListItem"></RowStyle>
                            <HeaderStyle CssClass="CartListHead"></HeaderStyle>
                            <AlternatingRowStyle CssClass="CartListItem"></AlternatingRowStyle>
                            <Columns>
                                <asp:BoundField DataField="ANNO" HeaderText="Anno">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DESCRIZIONE" HeaderText="Tipo">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Justify"></itemstyle>
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Notifica">
                                    <headerstyle horizontalalign="Center" Width="80px"></headerstyle>
                                    <itemstyle horizontalalign="Right" Width="80px"></itemstyle>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtAl" Width="80px" runat="server" Text='<%# FncGrd.FormattaDataGrd(Eval("DATANOTIFICA")) %>' CssClass="text-right"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ImpPieno.DiffImposta" HeaderText="Diff.Imposta" DataFormatString="{0:0.00}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ImpPieno.Sanzioni" HeaderText="Sanzioni" DataFormatString="{0:0.00}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ImpPieno.Interessi" HeaderText="Interessi" DataFormatString="{0:0.00}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="ImpPieno.SpeseNotifica" HeaderText="Spese" DataFormatString="{0:0.00}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="DOVUTO" HeaderText="Dovuto" DataFormatString="{0:0.00}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="PAGATO" HeaderText="Pagato" DataFormatString="{0:0.00}">
                                    <headerstyle horizontalalign="Center"></headerstyle>
                                    <itemstyle horizontalalign="Right"></itemstyle>
                                </asp:BoundField>
                                <asp:BoundField DataField="STATO" HeaderText="Stato">
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
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>
