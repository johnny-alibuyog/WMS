using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data.Definitions.Common;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PointOfSales
{
    public class PointOfSaleItemDefinition
	{
		public class Mapping : SubclassMap<PointOfSaleItem>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(PointOfSaleItem)}_{nameof(PointOfSaleItem.Sequence)}");

                References(x => x.PointOfSale);

				References(x => x.Product);

				Component(x => x.Quantity,
					MeasureDefinition.Mapping.Map("Quantity_", nameof(PointOfSaleItem)));

				Component(x => x.Standard,
					MeasureDefinition.Mapping.Map("Standard_", nameof(PointOfSaleItem)));

				Component(x => x.QuantityStandardEquivalent,
					MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(PointOfSaleItem)));

				Map(x => x.DiscountRate);

				Component(x => x.Discount,
					MoneyDefinition.Mapping.Map("Discount_", nameof(PointOfSaleItem)));

				Component(x => x.UnitPrice,
					MoneyDefinition.Mapping.Map("UnitPrice_", nameof(PointOfSaleItem)));

				Component(x => x.ExtendedPrice,
					MoneyDefinition.Mapping.Map("ExtendedPrice_", nameof(PointOfSaleItem)));

				Component(x => x.TotalPrice,
					MoneyDefinition.Mapping.Map("TotalPrice_", nameof(PointOfSaleItem)));
			}
		}

		public class Validation : ValidationDef<PointOfSaleItem>
		{
			public Validation()
			{
                Define(x => x.Sequence);

				Define(x => x.PointOfSale)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Product)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Quantity)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Standard)
					.NotNullable()
					.And.IsValid();

				Define(x => x.QuantityStandardEquivalent)
					.NotNullable()
					.And.IsValid();

				Define(x => x.DiscountRate);

				Define(x => x.Discount)
					.IsValid();

				Define(x => x.UnitPrice)
					.IsValid();

				Define(x => x.ExtendedPrice)
					.IsValid();

				Define(x => x.TotalPrice)
					.IsValid();
			}
		}
	}
}
