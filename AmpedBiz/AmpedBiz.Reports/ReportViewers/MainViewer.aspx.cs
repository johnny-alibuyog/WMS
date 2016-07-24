using System;
using System.Web.UI;
using Microsoft.Reporting.WebForms;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class MainViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.LoadSelector();
        }

        public void LoadReport(string reportPath, ReportDataSource dataSource)
        {
            this.rptViewer.ProcessingMode = ProcessingMode.Local;

            this.rptViewer.LocalReport.DataSources.Clear();
            this.rptViewer.LocalReport.DataSources.Add(dataSource);

            this.rptViewer.LocalReport.ReportPath = reportPath;
        }

        private void LoadSelector()
        {
            var selector = Request.QueryString.Get(App_Code.Reports.ReportKey.Selector);

            if (!string.IsNullOrEmpty(selector))
            {
                var priceListSelector = this.Page.LoadControl(selector) as UserControl;
                this.rptSelector.Controls.Clear();
                this.rptSelector.Controls.Add(priceListSelector);
            }
        }
    }
}