<%@ Page Title="Reimposta password" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="OPENgovSPORTELLO.Account.ResetPassword" Async="true" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div id="divLeftMenu" class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="HomeFO nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="HomeFO nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="body_page">
        <h2 class="lead_con_barra"><%: Title %>.</h2>
        <div class="form-horizontal">
            <h4 id="TitleH4">Immettere la nuova password</h4>
            <hr />
            <p class="text-danger">
                <asp:Literal runat="server" ID="ErrorMessage" />
                <asp:ValidationSummary runat="server" CssClass="text-danger hidden" />
            </p>
            <div id="divMayus" style="visibility:hidden" class="text-danger">Caps Lock attivo.</div>
            <div class="col-md-9">
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label">Posta elettronica</asp:Label>
                    <div class="col-md-9">
                        <asp:TextBox ReadOnly="true" style="height:20px;" runat="server" ID="Email" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="Il campo Posta elettronica è obbligatorio." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="Password" CssClass="col-md-2 control-label">Nuova password</asp:Label>
                    <div class="col-md-9">
                        <asp:TextBox style="height:20px;" runat="server" ID="Password" TextMode="Password" CssClass="form-control" onkeypress="capLock(event)" onmouseover="mouseoverPWD();" onmouseout="mouseoutPWD();" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="Il campo Password è obbligatorio." />
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" AssociatedControlID="ConfirmPassword" CssClass="col-md-2 control-label">Conferma nuova password</asp:Label>
                    <div class="col-md-9">
                        <asp:TextBox style="height:20px;" runat="server" ID="ConfirmPassword" TextMode="Password" CssClass="form-control" onkeypress="capLock(event)" onmouseover="mouseoverPWD();" onmouseout="mouseoutPWD();"/>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="Il campo Conferma password è obbligatorio." />
                        <asp:CompareValidator runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" CssClass="text-danger" Display="Dynamic" ErrorMessage="La password e la password di conferma non corrispondono." />
                    </div>
                </div>
                <div class="form-group">
                    <div class="col-md-offset-10">
                        <asp:Button runat="server" OnClick="Reset_Click" Text="Reimposta" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <h4>La password deve:</h4>
                <div class="form-group">
                    <div class="col-md-1"></div>
                    <div id="lblPasswordRules" class="col-md-11">
                        <p id="RuleWhiteSpace" class="text-success"> non contenere spazi</p>
                        <p id="RuleLength" class="text-danger"> essere lunga almeno 8 caratteri</p>
                        <p id="RuleNumber" class="text-danger"> avere almeno un numero (0-9)</p>
                        <p id="RuleLowerCase" class="text-danger"> avere almeno una lettera minuscola (a-z)</p>
                        <p id="RuleUpperCase" class="text-danger"> avere almeno una lettera maiuscola (A-Z)</p>
                        <p id="RuleSpecialChr" class="text-danger"> avere almeno un carattere speciale<br />(@ ! . _ - & + = # $ % ^)</p>
                    </div>
                </div>
            </div>
            <hr />
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
