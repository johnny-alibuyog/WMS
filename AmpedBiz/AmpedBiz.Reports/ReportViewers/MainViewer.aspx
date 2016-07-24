<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainViewer.aspx.cs" Inherits="AmpedBiz.Reports.ReportViewers.MainViewer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Assets/css/bootstrap.css" rel="stylesheet" />
    <link href="../Assets/css/Ampedbiz.css" rel="stylesheet" />
    <script src="../Assets/js/jquery-3.1.0.min.js"></script>
</head>
<body>
    <form id="frmMainViewer" runat="server" class="form-horizontal">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <div id="rptSelector" runat="server">
            <label>Loading Report Selector</label>
        </div>
        <h6 class="page-header" style="margin:0px 0px 10px 0px;"></h6>
        <div id="rptContainer">
            <center>
                <rsweb:ReportViewer
                    ID="rptViewer"
                    runat="server"
                    SizeToReportContent="True"
                    Width="100%"
                    Font-Names="Verdana"
                    Font-Size="8pt"
                    WaitMessageFont-Names="Verdana"
                    WaitMessageFont-Size="14pt">
                </rsweb:ReportViewer>
        </center>
        </div>
    </form>
</body>
</html>
