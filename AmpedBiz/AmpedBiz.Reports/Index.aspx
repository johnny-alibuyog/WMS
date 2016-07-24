<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="AmpedBiz.Reports.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Assets/css/bootstrap.css" rel="stylesheet" />

    <script type="text/javascript" >

        function showReport(selector) {
            var path = 'ReportViewers/MainViewer.aspx?DFD18FD7-9D34-4692-B7F9-7E54BFB4B0EB=' + selector;
            window.open(path, 'Reports')
            return false;
        }

    </script>
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
                <li><a href="#" onclick="showReport('ucpricelistselector.ascx')">Price List</a></li>
                <li><a href="#" onclick="showReport('ucUOMDetails.ascx')">Unit of Measurements</a></li>
                <li><a href="#" onclick="showReport('ucCustomerList.ascx')">Customer List</a></li>
                
            </ul>
        </div>
    </form>
</body>
</html>
