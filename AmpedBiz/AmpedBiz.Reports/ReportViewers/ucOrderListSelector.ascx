<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOrderListSelector.ascx.cs" Inherits="AmpedBiz.Reports.ReportViewers.ucOrderListSelector" %>
<h5><u>Display Order List</u></h5>
<div style="height: 20px; line-height: 20px;"></div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="container" style="margin-left: 10px;">
            <div class="row vcenter">
                <div class="col-xs-2">
                    <asp:RadioButton AutoPostBack="true" runat="server" Checked="true" Text="All" ID="rdoAll" GroupName="OrderList" CssClass="radio-inline" />
                </div>
            </div>
            <div class="row vcenter">
                <div class="col-xs-2" style="width: 200px;">
                    <asp:RadioButton AutoPostBack="true" Checked="false" runat="server" Text="By Status" ID="rdoOrder" GroupName="OrderList" CssClass="radio-inline" />
                </div>
                <div class="col-xs-1">
                    <asp:DropDownList ID="ddlOrderStatus" runat="server" CssClass="dropdown">
                        <asp:ListItem Selected="True" Value="New">New</asp:ListItem>
                        <asp:ListItem>Staged</asp:ListItem>
                        <asp:ListItem>Routed</asp:ListItem>
                        <asp:ListItem>Invoiced</asp:ListItem>
                        <asp:ListItem>Paid</asp:ListItem>
                        <asp:ListItem>Shipped</asp:ListItem>
                        <asp:ListItem>Completed</asp:ListItem>
                        <asp:ListItem>Cancelled</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div style="height: 20px; line-height: 20px;"></div>
            <div class="row">
                <asp:Button ID="btnView" runat="server" Text="View" CssClass=" btn-primary" OnClick="btnView_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
