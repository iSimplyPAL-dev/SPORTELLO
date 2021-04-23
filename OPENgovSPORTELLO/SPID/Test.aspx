<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="OPENgovSPORTELLO.SPID.Test" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form2" runat="server" method="POST" action="http://localhost:59706/SPID/POST">
               
        <input type="hidden" name="AuthnRequest" value="-" />
        <input type="hidden" name="Response" value="-" />
        <input type="hidden" name="AuthnReq_ID" value="-" />
        <input type="hidden" name="AuthnReq_IssueInstant" value="1382728968" />
        <input type="hidden" name="spidCode" value="123456" />
        <input type="hidden" name="FiscalNumber" value="RCPGNN79C22A883U" />
        <input type="hidden" name="ivaCode" value="0" />
        <input type="hidden" name="Data" value="2020-01-30" />
        <input type="hidden" name="Ora" value="10:27" />
        <input type="hidden" name="Mail" value="monica.tarello@isimply.it" />
               
    </form>
</body>
</html>

<script type="text/javascript">
    document.forms[0].submit();
</script>