namespace AmpedBiz.Core.Entities
{
    public class InvoiceReportSetting : SettingType
    {
        public virtual int PageItemSize { get; set; } = 6;
    }
}
