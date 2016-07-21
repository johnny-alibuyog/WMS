<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AmpedBiz.Reports.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Assets/css/bootstrap.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <h3>
            Reports
        </h3>
        <div>
            <ul>
                <li><a onclick="window.open('ReportViewers/UOMDetails.aspx','UOM');return false;" href="ReportViewers/UOMDetails.aspx">Unit of Measurements</a></li>
                <li><a href="ReportViewers/PriceListReportSelector.aspx">Price List of Products</a></li>
            </ul>
        </div>
    </form>
</body>
</html>
