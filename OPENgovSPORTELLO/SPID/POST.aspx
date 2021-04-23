<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="POST.aspx.cs" Inherits="OPENgovSPORTELLO.SPID.POST" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../Content/bootstrap.css" rel="stylesheet" />
    <link href="../Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div  class="navbar navbar-fixed-top">
            <div  class="navbar-header">
                <div class="divLogo" runat="server" id="Logo">
                </div>
                <div class="body_inner">  
                    <a class="CenterTitle navbar-brandTitle"><label id="lblDescrEnte" class="Ente"></label></a>
                    <asp:HiddenField ID="hdDescrEnte" runat="server" />
                </div>                   
            </div> 
            <div style="display: table-row" class="navbar-inverse">
                <div class="body_inner">
                    
                </div> 
            </div>
        </div>
        <div style="display: table;" class="body_outer">
            <div style="display: table-row;">
                <div style="width: 160px; display: table-cell; position:fixed; top:125px; left:10px;">  
                    <div id="divLeftMenu" class="container">
                        <ul class="nav navbar-nav navbar-left">
                            <li><a class="HomeFO nav navbar-nav Bottone BottoneHome" href="../DefaultFO.aspx"></a></li>
                            <li><a class="HomeFO nav navbar-nav" href="../DefaultFO.aspx">Home</a></li>
                        </ul>
                    </div>                    
                </div>
                <div style="display: table-cell" class="body_inner">
                    <div class="body_page">
                        <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="false">
                            <p class="text-danger">
                                <asp:Literal runat="server" ID="FailureText" />
                            </p>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
