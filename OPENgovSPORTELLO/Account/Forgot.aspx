<%@ Page Title="Password dimenticata" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Forgot.aspx.cs" Inherits="OPENgovSPORTELLO.Account.ForgotPassword" Async="true" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div id="divLeftMenu" class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeFO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeFO nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="NewLogin nav navbar-nav Bottone BottoneAccount"></a></li>
            <li><a class="NewLogin nav navbar-nav">Esegui Registrazione</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="body_page">
        <h2 id="hTitle" class="lead_con_barra"><%: Title %></h2>
        <div class="col-md-12">
            <asp:PlaceHolder id="loginForm" runat="server">
                <div class="form-horizontal">
                    <p>Inserisci la mail e ti sarà inviata una nuova Password.</p>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="form-group">
                        <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Posta elettronica</asp:Label>
                        <div class="col-md-10">
                            <asp:TextBox style="height:20px;" runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                CssClass="text-danger" ErrorMessage="Il campo Posta elettronica è obbligatorio." />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <asp:Button runat="server" OnClick="Forgot" Text="Inoltra la richiesta" CssClass="btn btn-default" />
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="DisplayEmail" Visible="false">
                <p class="text-info">
                    Controllare la posta elettronica per reimpostare la password.
                </p>
            </asp:PlaceHolder>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
