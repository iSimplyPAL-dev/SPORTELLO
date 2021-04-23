<%@ Page Title="Accedi" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="LoginBO.aspx.cs" Inherits="OPENgovSPORTELLO.Account.LoginBO" Async="true"%>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <table>
        <tr>
            <td><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></td>
            <td><a class="FAQFO nav navbar-nav">F.A.Q.</a></td>
        </tr>
    </table>
</asp:Content>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2 id="hTitle" class="lead_con_barra"><%: Title %></h2>
    <div class="body_page">
        <div class="col-md-12">
            <section id="loginForm">
                <div class="form-horizontal">
                    <h4>Per poter accedere ai servizi dello sportello on-line, è necessario effettuare la login (devi identificarti).</h4>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="col-md-12 login">
                        <div class="form-group col-md-8">
                            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Posta elettronica</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox  style="height:20px;" runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                    CssClass="text-danger" ErrorMessage="Il campo Posta elettronica è obbligatorio." />
                            </div>
                        </div>
                        <div class="form-group col-md-8">
                            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Password</asp:Label>
                            <div class="col-md-10">
                                <asp:TextBox style="height:20px;" runat="server" ID="Password" TextMode="Password" CssClass="form-control" onkeypress="capLock(event)" onmouseover="mouseoverPWD();" onmouseout="mouseoutPWD();" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Il campo Password è obbligatorio." />
                            </div>
                            <div id="divMayus" style="visibility:hidden" class="text-danger">Caps Lock attivo.</div> 
                        </div>
                        <div class="form-group col-md-8 hidden">
                            <div class="col-md-offset-2 col-md-10">
                                <div class="checkbox">
                                    <asp:CheckBox runat="server" ID="RememberMe" />
                                    <asp:Label runat="server" AssociatedControlID="RememberMe">Memorizza account</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md-8">
                            <div class="col-md-offset-2 col-md-10">
                                <asp:Button runat="server" OnClick="LogIn" Text="Accedi" CssClass="btn btn-default" />
                            </div>
                        </div>
                      </div>
                  </div>
            </section>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="BO" />
</asp:Content>
