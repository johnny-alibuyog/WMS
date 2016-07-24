<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucCustomerList.ascx.cs" Inherits="AmpedBiz.Reports.ReportViewers.ucCustomerList" %>
<h5><u>Display Customer List</u></h5>
<div style="height: 20px; line-height: 20px;"></div>

<asp:ObjectDataSource ID="odsCustomer" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetCustomers" TypeName="AmpedBiz.Reports.Datasource.AmpedBizDatasetTableAdapters.dtCustomersTableAdapter"></asp:ObjectDataSource>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container" style="margin-left: 10px;">
            <div class="row vcenter">
                <div class="col-xs-2">
                    <asp:RadioButton AutoPostBack="true" runat="server" Checked="true" Text="All" ID="rdoAll" GroupName="CustomerList" CssClass="radio-inline" />
                </div>
            </div>
            <div class="row vcenter">
                <div class="col-xs-2" style="width: 200px;">
                    <asp:RadioButton AutoPostBack="true" Checked="false" runat="server" Text="By Customer" ID="rdoCustomer" GroupName="CustomerList" CssClass="radio-inline" />
                </div>
                <div class="col-xs-1">
                    <asp:DropDownList ID="ddlCustomer" runat="server" DataSourceID="odsCustomer" DataTextField="name" DataValueField="customerid" CssClass="dropdown"></asp:DropDownList>
                </div>
            </div>

            <div style="height: 20px; line-height: 20px;"></div>
            <div class="row">
                <asp:Button ID="btnView" runat="server" Text="View" CssClass=" btn-primary" OnClick="btnView_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
