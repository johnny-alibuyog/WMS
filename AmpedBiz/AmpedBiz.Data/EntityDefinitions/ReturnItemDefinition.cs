using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnItemDefinition
    {
        public class Mapping : SubclassMap<ReturnItem>
        {
            public Mapping()
            {
                References(x => x.Return);

				Component(x => x.UnitPrice,
					MoneyDefinition.Mapping.Map("UnitPrice_", nameof(ReturnItem)));
			}
		}

        public class Validation : ValidationDef<ReturnItem>
        {
            public Validation()
            {
                Define(x => x.Return)
                    .NotNullable();

				Define(x => x.UnitPrice)
					.IsValid();
			}
        }
    }
}
