using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Settings
{
    public interface ISettingsFacade
    {
        UserSetting Users { get; }

        InvoiceReportSetting InvoiceReports { get; }

        CurrencySetting Currencies { get; }
    }
}
