using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class ucPricelistSelector : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/Reports/PriceList.rdlc");

            MainViewer viewer = this.Page as MainViewer;
            viewer.LoadReport(path, this.PrepareDataSource());
        }

        private ReportDataSource PrepareDataSource()
        {
            Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
            Datasource.AmpedBizDataset.dtProductsDataTable dt = new Datasource.AmpedBizDataset.dtProductsDataTable();

            dt.Clear();

            var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtProductsTableAdapter();

            if (rdoCategory.Checked)
            {
                adapter.FillProductsByCategoryId(dt, ddlCategory.SelectedValue);
            }
            else if (rdoSupplier.Checked)
            {
                adapter.FillProductsBySupplierId(dt, ddlSupplier.SelectedValue);
            }
            else
            {
                adapter.FillProducts(dt);
            }

            ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);

            return datasource;
        }
    }
}