using AmpedBiz.Core.Returns;
using AmpedBiz.Data.Definitions.Common;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions
{
	public class ReturnItemBaseDefinition
	{
		public class Mapping : ClassMap<ReturnItemBase>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.Product);

				References(x => x.Reason);

				Component(x => x.Quantity,
					MeasureDefinition.Mapping.Map("Quantity_", nameof(ReturnItemBase)));

				Component(x => x.Standard,
					MeasureDefinition.Mapping.Map("Standard_", nameof(ReturnItemBase)));

				Component(x => x.QuantityStandardEquivalent,
					MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(ReturnItemBase)));

				Component(x => x.Returned,
					MoneyDefinition.Mapping.Map("Returned_", nameof(ReturnItemBase)));
			}
		}

		public class Validation : ValidationDef<ReturnItemBase>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.Product)
					.NotNullable();

				Define(x => x.Reason)
					.NotNullable();

				Define(x => x.Quantity)
					.IsValid();

				Define(x => x.Standard)
					.IsValid();

				Define(x => x.QuantityStandardEquivalent)
					.IsValid();

				Define(x => x.Returned)
					.IsValid();
			}
		}
	}
}
