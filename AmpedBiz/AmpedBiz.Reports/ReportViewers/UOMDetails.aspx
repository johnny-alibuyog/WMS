<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UOMDetails.aspx.cs" Inherits="AmpedBiz.Reports.ReportViewers.UOMDetails" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <center>
        <h1>Unit of Measurements</h1>
        <rsweb:ReportViewer
            ID="ReportViewer1"
            runat="server"
            SizeToReportContent="True"
            Width="100%" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
            <LocalReport ReportPath="Reports\UOMDetails.rdlc" DisplayName="UOM Report">
            </LocalReport>
        </rsweb:ReportViewer>
    </center>
    </form>
</body>
</html>
