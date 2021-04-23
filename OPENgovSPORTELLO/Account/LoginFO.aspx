<%@ Page Title="Accedi" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="LoginFO.aspx.cs" Inherits="OPENgovSPORTELLO.Account.Login" Async="true" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="NewLogin nav navbar-nav Bottone BottoneAccount"></a></li>
            <li><a class="NewLogin nav navbar-nav">Esegui registrazione</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link href="../Content/spid-sp-access-button.min.css" rel="stylesheet" />
    <div class="body_page">
        <h2 id="hTitle" class="lead_con_barra">
            <span class="glyphicon glyphicon-user login-span" aria-hidden="true"></span>
            <span class="login-span">&nbsp;<%: Title %></span>
        </h2>
        <div class="col-md-12">
            <blockquote class="browsersupportati">
                <p>Utilizza lo Sportello Contribuente con 
                    <strong>
                        <a href="https://support.google.com/chrome/answer/95346?co=GENIE.Platform%3DDesktop&amp;hl=it&amp;oco=1" target="_blank">
                            Google Chrome
                            <img src="https://www.google.it/chrome/static/images/chrome-logo.svg" class="fr-fic fr-dii" style="width: 32px;" />
                        </a>
                    </strong>                
                    che garantisce le migliori prestazioni nella <strong>fruizione&nbsp;</strong>di tutti i servizi.
                </p>
            </blockquote>
        </div>
        <div id="divConfirmAccount divDescr" class="col-md-12">
            <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true"> 
                <h1 class="login-nav-pills-ul">Grazie per aver confermato l’account.</h1>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
                <p class="text-danger">Si è verificato un errore.</p>
            </asp:PlaceHolder>
        </div>
        <div class="col-md-12 form-group lead hidden" style="padding-bottom: 30px;">
            <label class="col-md-4" style="margin-top:10px;">Sportello Contribuente per il comune di </label>
            <asp:DropDownList runat="server" ID="ddlEnte" CssClass="subtitle col-md-4" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList>
        </div>
        <div class="login-nav-pills-ul">
            <ul class="nav nav-pills loginNav">
                <li><a class="tablinks" href="#">Password</a></li>
                <li><a class="tablinks" target="_self" href="https://spid.grandcombin.vda.it">SPID</a></li>
            </ul>          
        </div>
        <div id="byAccount" class="col-md-12 tabcontent">
            <section id="loginForm">
                <div class="form-horizontal">
                    <p>Per poter accedere ai servizi dello sportello on-line, è necessario effettuare la login (devi identificarti).
					Se non sei ancora registrato, procedi con la registrazione mediante selezione &lsquo;esegui registrazione&rsquo;.</p>
                    <hr />
                    <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                        <p class="text-danger">
                            <asp:Literal runat="server" ID="FailureText" />
                        </p>
                    </asp:PlaceHolder>
                    <div class="col-md-7 login">
                        <div class="col-md-12">
                            <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-3 control-label">Posta elettronica</asp:Label>
                            <div class="col-md-9">
                                <asp:TextBox runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                                    CssClass="text-danger" ErrorMessage="Il campo Posta elettronica è obbligatorio." />
                            </div>
                        </div>
                        <div class="col-md-12">
                            <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-3 control-label">Password</asp:Label>
                            <div class="col-md-9">
                                <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="form-control" onkeypress="capLock(event)" onmouseover="mouseoverPWD();" onmouseout="mouseoutPWD();" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Il campo Password è obbligatorio." />
                            </div>
                            <div id="divMayus" style="visibility:hidden" class="text-danger destra">Caps Lock attivo.</div> 
                        </div>
                        <div class="col-md-8 hidden">
                            <div class="col-md-offset-2 col-md-10">
                                <div class="checkbox">
                                    <asp:CheckBox runat="server" ID="RememberMe" />
                                    <asp:Label runat="server" AssociatedControlID="RememberMe">Memorizza account</asp:Label>
                                </div>
                            </div>
                        </div>
                        <div id="PrivacyPolicy" class="col-md-12 hidden">
                            <div class="col-md-05"><asp:CheckBox ID="chkAccept" runat="server" Text="" /></div>
                            <div id="Policy" class="col-md-10 text-justified text-italic"></div>
                        </div>
                    </div>
                    <div class="col-md-5">
                        <div class="col-md-7">
                            <div class="col-md-offset-2 col-md-10">
                                <asp:Button runat="server" OnClick="LogIn" Text="Accedi" CssClass="btn btn-default" />
                            </div>
                        </div>
                       <!--<p>
                            <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Esegui registrazione come nuovo utente</asp:HyperLink>
                        </p>-->
                        <div class="col-md-4">
                            <div class="btn btn-default pull-right">
                                <a class="ForgotPWD">Password dimenticata?</a>
                                <p>
                                    <%-- Abilitare questa opzione dopo aver abilitato la conferma dell'account per la funzionalità di reimpostazione della password
                                    <asp:HyperLink runat="server" CssClass="lead destra" ID="ForgotPasswordHyperLink" ViewStateMode="Disabled">Vecchio Password dimenticata?</asp:HyperLink>--%>
                                </p>
                            </div>
                        </div>
                    </div>
                 </div>
            </section>
			<div class="col-md-12">
				<div id="BodyHomePage" class="col-md-10 divDescr"></div>
				<div id="HomePagePrivacy" class="col-md-2 text-justified text-italic" style="padding-left: 20px;"></div>
			</div>
        </div>
        <%--<div class="col-md-4">
            <section id="socialLoginForm">
                <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
            </section>
        </div>--%>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
