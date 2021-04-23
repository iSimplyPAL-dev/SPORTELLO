<%@ Page Title="About" Language="C#" MasterPageFile="~/OPENgovSPORTELLO.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="OPENgovSPORTELLO.About" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="HeadContent" runat="server">
    <script>
        $(document).ready(function(){
            var TipoUtenza = getUrlParameter('TipoUtenza');
            if (TipoUtenza == 'BO')
            {
                $("#AboutBO").show();
                $("#AboutFO").hide();
            }
            else { 
                $("#AboutFO").show();
                $("#AboutBO").hide();
            }
        });
    </script>
    <div class="container">
        <div class="navbar-header">
            <ul class="nav navbar-nav">
                <li><a id="hHome" href="~/DefaultBO.aspx" class="SubmitBtn Bottone BottoneHome" title="Home"></a></li>
            </ul>
        </div>
        <div class="navbar-collapse collapse">
            <a class="navbar-brand" id="hSportello" href="~/DefaultBO.aspx">Sportello Contribuente</a>
        </div>
    </div>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div id="AboutBO">
        <h2><%: Title %>.</h2>
        <h3>Your application description page.</h3>
        <p>Use this area to provide additional information.</p>
        <p>Lorem Ipsum è un testo segnaposto utilizzato nel settore della tipografia e della stampa. Lorem Ipsum è considerato il testo segnaposto standard sin dal sedicesimo secolo, quando un anonimo tipografo prese una cassetta di caratteri e li assemblò per preparare un testo campione. È sopravvissuto non solo a più di cinque secoli, ma anche al passaggio alla videoimpaginazione, pervenendoci sostanzialmente inalterato. Fu reso popolare, negli anni ’60, con la diffusione dei fogli di caratteri trasferibili “Letraset”, che contenevano passaggi del Lorem Ipsum, e più recentemente da software di impaginazione come Aldus PageMaker, che includeva versioni del Lorem Ipsum.</p>
    </div>
    <div id="AboutFO">
        <h2><%: Title %>.</h2>
        <h3>Your application description page.</h3>
        <p>Use this area to provide additional information.</p>
        <p>Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
        <p>Lorem ipsum dolor sit amet, consectetur adipisci elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</p>
   </div>
</asp:Content>
