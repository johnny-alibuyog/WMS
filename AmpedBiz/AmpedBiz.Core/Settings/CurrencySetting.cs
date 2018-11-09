using AmpedBiz.Core.Common;

namespace AmpedBiz.Core.Settings
{
    public class CurrencySetting : SettingType
    {
        public virtual string DefaultCurrencyId { get; set; } = Currency.PHP.Id;
    }
}
