using AmpedBiz.Core.Common;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PointOfSales
{
    public class PointOfSaleDefinition
	{
		public class Mapping : SubclassMap<PointOfSale>
		{
			public Mapping()
			{
				Map(x => x.InvoiceNumber)
					.Index("IDX_POSInvoiceNumber");

				References(x => x.Branch);

				References(x => x.Customer);

				References(x => x.PaymentType);

				Map(x => x.TenderedOn);

				References(x => x.TenderedBy);

				Map(x => x.CreatedOn);

				References(x => x.CreatedBy);

				References(x => x.Pricing);

				Map(x => x.DiscountRate);

				Component(x => x.Discount,
					MoneyDefinition.Mapping.Map("Discount_", nameof(PointOfSale)));

				Component(x => x.SubTotal,
					MoneyDefinition.Mapping.Map("SubTotal_", nameof(PointOfSale)));

				Component(x => x.Total,
					MoneyDefinition.Mapping.Map("Total_", nameof(PointOfSale)));

				Component(x => x.Received,
					MoneyDefinition.Mapping.Map("Received_", nameof(PointOfSale)));

				Component(x => x.Change,
					MoneyDefinition.Mapping.Map("Change_", nameof(PointOfSale)));

				Component(x => x.Paid,
					MoneyDefinition.Mapping.Map("Payment_", nameof(PointOfSale)));

				Component(x => x.Balance,
					MoneyDefinition.Mapping.Map("Balance_", nameof(PointOfSale)));

				Map(x => x.Status);

				HasMany(x => x.Items)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
                    .OrderBy(nameof(PointOfSaleItem.Sequence))
					.AsSet();

				HasMany(x => x.Payments)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
                    .OrderBy(nameof(PointOfSalePayment.Sequence))
                    .AsSet();
			}
		}

		public class Validation : ValidationDef<PointOfSale>
		{
			public Validation()
			{
				Define(x => x.InvoiceNumber)
					.MaxLength(30);

				Define(x => x.Branch)
					.NotNullable();

				Define(x => x.Customer);

				Define(x => x.PaymentType);

				Define(x => x.CreatedOn);

				Define(x => x.CreatedBy);

				Define(x => x.TenderedOn);

				Define(x => x.TenderedBy);

				Define(x => x.Pricing);

				Define(x => x.DiscountRate);

				Define(x => x.Discount);

				Define(x => x.SubTotal)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Total)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Received)
					.IsValid();

				Define(x => x.Change)
					.IsValid();

				Define(x => x.Paid)
					.IsValid();

				Define(x => x.Balance)
					.IsValid();

				Define(x => x.Status)
					.NotNullable();

				Define(x => x.Items)
					.NotNullableAndNotEmpty()
					.And.HasValidElements();

				Define(x => x.Payments)
					.HasValidElements();

				this.ValidateInstance.By((instance, context) =>
				{
					var valid = true;

					/* allow debit if customer field is supplied  */
					if (instance.Customer != null)
					{
						return valid;
					}

					if (instance.Paid < instance.Total)
					{
						context.AddInvalid<PointOfSale, Money>(
							message: 
                                "Debit is not allowed for non member customers. " + 
                                "Please select customer otherwise fullfill payment.",//$"Payment amount of {instance.Paid} is less than total amount of {instance.Total}!",
							property: x => x.Paid
						);
						valid = false;
					}
	
					return valid;
				});

			}
		}
	}
}
