<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucPricelistSelector.ascx.cs" Inherits="AmpedBiz.Reports.ReportViewers.ucPricelistSelector" %>
<h6 class="page-header" style="margin: 0px 0px 10px 0px;"></h6>
<asp:ObjectDataSource ID="odsSupplier" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetSuppliers" TypeName="AmpedBiz.Reports.Datasource.AmpedBizDatasetTableAdapters.dtSuppliersTableAdapter"></asp:ObjectDataSource>

<asp:ObjectDataSource ID="odsCategory" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetCategories" TypeName="AmpedBiz.Reports.Datasource.AmpedBizDatasetTableAdapters.dtProductCategoriesTableAdapter"></asp:ObjectDataSource>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div id="divPriceListSelector" class="container" style="margin-left: 10px;">
            <div class="row vcenter">
                <div class="col-xs-2">
                    <asp:RadioButton AutoPostBack="true" runat="server" Checked="true" Text="All" ID="rdoAll" GroupName="PriceList" CssClass="radio-inline" />
                </div>
            </div>
            <div class="row vcenter">
                <div class="col-xs-2" style="width: 200px;">
                    <asp:RadioButton AutoPostBack="true" runat="server" Checked="false" Text="By Product Category" ID="rdoCategory" GroupName="PriceList" CssClass="radio-inline" />
                </div>
                <div class="col-xs-1">
                    <asp:DropDownList ID="ddlCategory" runat="server" DataSourceID="odsCategory" DataTextField="name" DataValueField="productcategoryid" CssClass="dropdown"></asp:DropDownList>
                </div>
            </div>

            <div class="row vcenter">
                <div class="col-xs-2" style="width: 200px;">
                    <asp:RadioButton AutoPostBack="true" Checked="false" runat="server" Text="By Supplier" ID="rdoSupplier" GroupName="PriceList" CssClass="radio-inline" />
                </div>
                <div class="col-xs-1">
                    <asp:DropDownList ID="ddlSupplier" runat="server" DataSourceID="odsSupplier" DataTextField="name" DataValueField="supplierid" CssClass="dropdown"></asp:DropDownList>
                </div>
            </div>

            <div style="height: 20px; line-height: 20px;"></div>
            <div class="row">
                <asp:Button ID="btnView" runat="server" Text="View" CssClass=" btn-primary" OnClick="btnView_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
