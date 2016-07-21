using System;
using System.Data;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class UOMDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
                Datasource.AmpedBizDataset.dtUOMDataTable dt = new Datasource.AmpedBizDataset.dtUOMDataTable();
                dt.Clear();

                var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtUOMTableAdapter();
                adapter.FillUOM(dt);

                ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(datasource);
            }
        }
    }
}