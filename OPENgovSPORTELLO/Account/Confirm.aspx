<%@ Page Title="Conferma account" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="OPENgovSPORTELLO.Account.Confirm" Async="true" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <div class="body_page">
        <h2 class="lead_con_barra"><%: Title %>.</h2>
        <div>
            <asp:PlaceHolder runat="server" ID="successPanel" ViewStateMode="Disabled" Visible="true">            
                <p>
                    <label id="lblEnte">Ente:</label>
                    <asp:DropDownList runat="server" ID="ddlEnte" Width="100%" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ControlSelectedChanged"></asp:DropDownList>
                </p>
                <p>
                    Grazie per aver confermato l'account. Fare clic <asp:HyperLink ID="login" runat="server" CssClass="LoginFO">qui</asp:HyperLink>  per accedere             
                </p>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="errorPanel" ViewStateMode="Disabled" Visible="false">
                <p class="text-danger">
                    Si è verificato un errore.
                </p>
            </asp:PlaceHolder>
        </div>
    </div>
    <asp:HiddenField ID="hfFrom" runat="server" Value="FO" />
</asp:Content>
