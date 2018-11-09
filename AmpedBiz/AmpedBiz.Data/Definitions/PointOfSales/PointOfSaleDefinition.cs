using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PointOfSales
{
	public class PointOfSaleDefinition
	{
		public class Mapping : ClassMap<PointOfSale>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				Map(x => x.InvoiceNumber)
					.Index("IDX_POSInvoiceNumber");

				References(x => x.Branch);

				References(x => x.Customer);

				References(x => x.PaymentType);

				Map(x => x.TendredOn);

				References(x => x.TendredBy);

				Map(x => x.CreatedOn);

				References(x => x.CreatedBy);

				References(x => x.Pricing);

				Component(x => x.Discount,
					MoneyDefinition.Mapping.Map("Discount_", nameof(PointOfSale)));

				Component(x => x.SubTotal,
					MoneyDefinition.Mapping.Map("SubTotal_", nameof(PointOfSale)));

				Component(x => x.Total,
					MoneyDefinition.Mapping.Map("Total_", nameof(PointOfSale)));

				Component(x => x.Paid,
					MoneyDefinition.Mapping.Map("Payment_", nameof(PointOfSale)));

				HasMany(x => x.Items)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
					.AsSet();

				HasMany(x => x.Payments)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
					.AsSet();
			}
		}

		public class Validation : ValidationDef<PointOfSale>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.InvoiceNumber)
					.MaxLength(30);

				Define(x => x.Branch)
					.NotNullable();

				Define(x => x.Customer)
					.NotNullable()
					.And.IsValid();

				Define(x => x.PaymentType);

				Define(x => x.CreatedOn);

				Define(x => x.CreatedBy);

				Define(x => x.TendredOn);

				Define(x => x.TendredBy);

				Define(x => x.Pricing);

				Define(x => x.Discount);

				Define(x => x.SubTotal)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Total)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Paid);

				Define(x => x.Items)
					.NotNullableAndNotEmpty()
					.And.HasValidElements();

				Define(x => x.Payments)
					.HasValidElements();
			}
		}
	}
}
