using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class ucOrderListSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/Reports/OrderList.rdlc");

            MainViewer viewer = this.Page as MainViewer;
            viewer.LoadReport(path, this.PrepareDataSource());
        }

        private ReportDataSource PrepareDataSource()
        {
            Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
            Datasource.AmpedBizDataset.dtOrdersDataTable dt = new Datasource.AmpedBizDataset.dtOrdersDataTable();

            dt.Clear();

            var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtOrdersTableAdapter();

            if (rdoAll.Checked)
            {
                adapter.FillOrders(dt);
            }
            else
            {
                adapter.FillOrdersByStatus(dt, this.ddlOrderStatus.SelectedValue);
            }

            ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);

            return datasource;
        }
    }
}