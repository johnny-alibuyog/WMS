using System;
using System.Data;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class PriceList : Page
    {
        private const string ReportTitle = "Product Pricelist";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Datasource.AmpedBizDataset ds = new Datasource.AmpedBizDataset();
                Datasource.AmpedBizDataset.dtProductsDataTable dt = new Datasource.AmpedBizDataset.dtProductsDataTable();

                dt.Clear();

                var adapter = new Datasource.AmpedBizDatasetTableAdapters.dtProductsTableAdapter();

                var queryString = Request.QueryString;

                var criteria = queryString["criteria"].ToLower();
                var value = queryString["value"];

                switch (criteria)
                {
                    case "supplier":
                        adapter.FillProductsBySupplierId(dt, value);
                        this.title.InnerText = ReportTitle + " By Supplier";
                        break;
                    case "category":
                        adapter.FillProductsByCategoryId(dt, value);
                        this.title.InnerText = ReportTitle + " By Category";
                        break;
                    default:
                        adapter.FillProducts(dt);
                        this.title.InnerText = ReportTitle;
                        break;
                }

                ReportDataSource datasource = new ReportDataSource("AmpedBizDataset", dt as DataTable);

                this.ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.DataSources.Add(datasource);
                this.ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}