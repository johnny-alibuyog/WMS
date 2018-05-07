namespace AmpedBiz.Core.Entities
{
    public class CurrencySetting : SettingType
    {
        public virtual string DefaultCurrencyId { get; set; } = Currency.PHP.Id;
    }
}
