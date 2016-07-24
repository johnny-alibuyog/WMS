using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class ucCustomerList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/Reports/CustomerList.rdlc");

            MainViewer viewer = this.Page as MainViewer;
            viewer.LoadReport(path, this.PrepareDataSource());
        }

        private ReportDataSource PrepareDataSource()
        {
            Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
            Datasource.AmpedBizDataset.dtCustomersDataTable dt = new Datasource.AmpedBizDataset.dtCustomersDataTable();

            dt.Clear();

            var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtCustomersTableAdapter();

            if (rdoAll.Checked)
            {
                adapter.FillCustomers(dt);
            }
            else
            {
                adapter.FillCustomersById(dt, this.ddlCustomer.SelectedValue);
            }

            ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);

            return datasource;
        }
    }
}