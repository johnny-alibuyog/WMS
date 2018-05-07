using AmpedBiz.Core.Entities;
using AmpedBiz.Data.CustomTypes;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CurrencySettingDefinition
    {
        public class Mapping : SubclassMap<Setting<CurrencySetting>>
        {
            public Mapping()
            {
                Map(x => x.Value)
                    .CustomType<JsonType<CurrencySetting>>();

                DiscriminatorValue(nameof(CurrencySetting));
            }
        }

        public class Validation : ValidationDef<Setting<CurrencySetting>>
        {
            public Validation()
            {
                Define(x => x.Value);
            }
        }
    }
}
