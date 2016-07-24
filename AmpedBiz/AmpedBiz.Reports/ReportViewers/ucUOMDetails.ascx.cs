using System;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class ucUOMDetails : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            var path = Server.MapPath("~/Reports/UOMDetails.rdlc");

            MainViewer viewer = this.Page as MainViewer;
            viewer.LoadReport(path, this.PrepareDataSource());
        }

        private ReportDataSource PrepareDataSource()
        {
            Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
            Datasource.AmpedBizDataset.dtUOMDataTable dt = new Datasource.AmpedBizDataset.dtUOMDataTable();
            dt.Clear();

            var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtUOMTableAdapter();
            adapter.FillUOM(dt);

            ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);

            return datasource;
        }
    }
}