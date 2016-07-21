<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PricelistReportSelector.aspx.cs" Inherits="AmpedBiz.Reports.ReportViewers.PricelistReportSelector1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Assets/css/bootstrap.css" rel="stylesheet" />
    <link href="../Assets/css/Ampedbiz.css" rel="stylesheet" />
    <script src="../Assets/js/jquery-3.1.0.min.js"></script>

    <script type="text/javascript">
        function openReport() {
            
            var selectedCriteria = $(".container input[type='radio']:checked").val().toLowerCase();
            var filterName = "";
            var filterValue = "";

            switch (selectedCriteria) {
                case 'rdocategory':
                    filterName = 'category';
                    filterValue = $('#ddlCategory').val();
                    break;
                case 'rdosupplier':
                    filterName = 'supplier';
                    filterValue = $('#ddlSupplier').val();
                    break;
                default:
                    filterName = 'all';
                    break;
            }
            
            window.open('pricelist.aspx?criteria=' + filterName + '&value=' + filterValue, 'pricelist');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <h3><u>Display Product Pricelist</u></h3>
        <div style="height: 20px; line-height: 20px;"></div>

        <asp:ObjectDataSource ID="odsSupplier" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSuppliers" TypeName="AmpedBiz.Reports.Datasource.AmpedBizDatasetTableAdapters.suppliersTableAdapter"></asp:ObjectDataSource>

        <asp:ObjectDataSource ID="odsCategory" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetCategories" TypeName="AmpedBiz.Reports.Datasource.AmpedBizDatasetTableAdapters.productcategoriesTableAdapter"></asp:ObjectDataSource>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div class="container" style="margin-left: 10px;">
                    <div class="row vcenter">
                        <div class="col-xs-2">
                            <asp:RadioButton AutoPostBack="true" runat="server" Checked="true" OnCheckedChanged="rdoAll_CheckedChanged" Text="All" ID="rdoAll" GroupName="PriceList" CssClass="radio-inline" />
                        </div>
                    </div>
                    <div class="row vcenter">
                        <div class="col-xs-2" style="width: 200px;">
                            <asp:RadioButton AutoPostBack="true" runat="server" Checked="false" OnCheckedChanged="rdoCategory_CheckedChanged" Text="By Product Category" ID="rdoCategory" GroupName="PriceList" CssClass="radio-inline" />
                        </div>
                        <div class="col-xs-1">
                            <asp:DropDownList ID="ddlCategory" Enabled="false" runat="server" DataSourceID="odsCategory" DataTextField="name" DataValueField="productcategoryid" CssClass="dropdown"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row vcenter">
                        <div class="col-xs-2" style="width: 200px;">
                            <asp:RadioButton AutoPostBack="true" Checked="false" runat="server" OnCheckedChanged="rdoSupplier_CheckedChanged" Text="By Supplier" ID="rdoSupplier" GroupName="PriceList" CssClass="radio-inline" />
                        </div>
                        <div class="col-xs-1">
                            <asp:DropDownList ID="ddlSupplier" Enabled="false" runat="server" DataSourceID="odsSupplier" DataTextField="name" DataValueField="supplierid" CssClass="dropdown"></asp:DropDownList>
                        </div>
                    </div>

                    <div style="height: 20px; line-height: 20px;"></div>
                    <div class="row">
                        <asp:Button ID="btnView" runat="server" Text="View" CssClass=" btn-primary" OnClick="btnView_Click" OnClientClick="openReport();" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
