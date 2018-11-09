namespace AmpedBiz.Core.Settings.Services
{
	public interface ISettingsFacade
    {
        UserSetting Users { get; }

        InvoiceReportSetting InvoiceReports { get; }

        CurrencySetting Currencies { get; }
    }
}
