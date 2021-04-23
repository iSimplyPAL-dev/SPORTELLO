<%@ Page Title="Registrazione" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="RegisterFO.aspx.cs" Inherits="OPENgovSPORTELLO.Account.RegisterFO" %>
<asp:Content ID="MenuContent" ContentPlaceHolderID="LeftMenuContent" runat="server">
    <div class="container">
        <ul class="nav navbar-nav navbar-left">
            <li><a class="Login nav navbar-nav Bottone BottoneHome"></a></li>
            <li><a class="Login nav navbar-nav">Home</a></li>
        </ul>
        <ul class="nav navbar-nav navbar-left">
            <li><a class="FAQFO nav navbar-nav Bottone BottoneFAQ"></a></li>
            <li><a class="FAQFO nav navbar-nav">F.A.Q.</a></li>
        </ul>
    </div>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="body_page">
        <h2 id="hTitle" class="lead_con_barra">
            <span class="glyphicon glyphicon-user login-span" aria-hidden="true"></span>
            <span class="login-span">&nbsp;Crea un nuovo account</span>
        </h2>
        <p class="text-primary usain">
            <asp:Literal runat="server" ID="infoSPID" />
        </p>
        <div class="col-md-12 form-group lead">
            <label class="col-md-4" style="margin-top:10px;">Sportello Contribuente per il comune di </label>
            <asp:DropDownList runat="server" ID="ddlEnte" CssClass="subtitle col-md-4" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList>
        </div>
        <div class="col-md-12">
        <p class="text-danger usain">
            <asp:Literal runat="server" ID="ErrorMessage" />
            <label id="OnlyNumber_error" class="text-danger usain"></label>
        </p>
        <br />
        <div class="form-horizontal">
            <asp:ValidationSummary runat="server" CssClass="text-danger" />
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="Email" CssClass="col-md-2 control-label usain">Posta elettronica</asp:Label>
                <div class="col-md-8">
                    <asp:TextBox style="height:20px;" runat="server" ID="Email" CssClass="form-control validEmail" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="Email" CssClass="text-danger" ErrorMessage="Il campo Posta elettronica è obbligatorio." />
                    <p class="ac" id="MessageEmail"></p>
                </div>
            </div>
            <%--codice fiscale e partita iva--%>
            <div class="form-group">
                <asp:Label runat="server" CssClass="col-md-2 control-label usain">Codice Fiscale/P.IVA</asp:Label>
                <div class="col-md-8">
                    <asp:TextBox style="height:20px;" ID="TextBoxCodiceFiscale" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RegularExpressionValidator CssClass="text-danger" Display = "Dynamic" ControlToValidate="TextBoxCodiceFiscale" id="ControlloCodiceFiscale" ValidationExpression = "^[\s\S]{11,16}$" runat="server" ErrorMessage="Il campo Codice Fiscale/Partita IVA deve contenere almeno 11 caratteri e fino ad un massimo di 16."></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-6">
                    <div class="col-md-05"><asp:CheckBox ID="chkAccept" runat="server" Text="" /></div>
                    <div id="Policy" class="col-md-10 text-justified text-italic"></div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-md-offset-8 col-md-3">
                    <asp:Button  runat="server" OnClientClick="return FieldValidatorRegister()" OnClick="CreateUser_Click" Text="Esegui registrazione" CssClass="btn btn-default" />
                </div>
            </div>
        </div>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
