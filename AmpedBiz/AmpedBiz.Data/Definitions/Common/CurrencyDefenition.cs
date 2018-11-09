using AmpedBiz.Core.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
	public class CurrencyDefenition
	{
		public class Mapping : ClassMap<Currency>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.Assigned();

				Map(x => x.Symbol);

				Map(x => x.Name);
			}
		}

		public class Validation : ValidationDef<Currency>
		{
			public Validation()
			{
				Define(x => x.Id)
				   .NotNullableAndNotEmpty()
				   .And.MaxLength(30);

				Define(x => x.Symbol)
					.NotNullableAndNotEmpty()
					.And.MaxLength(150);

				Define(x => x.Name)
					.NotNullableAndNotEmpty()
					.And.MaxLength(150);
			}
		}
	}
}
