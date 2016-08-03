using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class ucSupplierList : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/Reports/SupplierList.rdlc");

            MainViewer viewer = this.Page as MainViewer;
            viewer.LoadReport(path, this.PrepareDataSource());
        }

        private ReportDataSource PrepareDataSource()
        {
            Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
            Datasource.AmpedBizDataset.dtSuppliersDataTable dt = new Datasource.AmpedBizDataset.dtSuppliersDataTable();

            dt.Clear();

            var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtSuppliersTableAdapter();

            if (rdoAll.Checked)
            {
                adapter.FillSuppliers(dt);
            }
            else
            {
                adapter.FillSuppliersById(dt, this.ddlSupplier.SelectedValue);
            }

            ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);

            return datasource;
        }
    }
}