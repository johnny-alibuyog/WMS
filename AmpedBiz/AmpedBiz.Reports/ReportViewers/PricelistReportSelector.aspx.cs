using System;
using System.Web.UI;

namespace AmpedBiz.Reports.ReportViewers
{
    public partial class PricelistReportSelector1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.rdoAll.Checked = true;
            }
        }

        protected void rdoAll_CheckedChanged(object sender, EventArgs e)
        {
            this.ddlSupplier.Enabled = false;
            this.ddlCategory.Enabled = false;
        }

        protected void rdoCategory_CheckedChanged(object sender, EventArgs e)
        {
            this.ddlSupplier.Enabled = false;
            this.ddlCategory.Enabled = true;
        }

        protected void rdoSupplier_CheckedChanged(object sender, EventArgs e)
        {
            this.ddlSupplier.Enabled = true;
            this.ddlCategory.Enabled = false;
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
        }
    }
}