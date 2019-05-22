using AmpedBiz.Core.Orders;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Orders
{
	public class OrderDefinition
	{
		public class Mapping : SubclassMap<Order>
		{
			public Mapping()
			{
				Map(x => x.OrderNumber);

				Map(x => x.InvoiceNumber)
					.Index("IDX_InvoiceNumber");

				References(x => x.Branch);

				References(x => x.Customer);

				References(x => x.PaymentType);

				References(x => x.Shipper);

				Map(x => x.TaxRate);

				Component(x => x.Tax,
					MoneyDefinition.Mapping.Map("Tax_", nameof(Order)));

				Component(x => x.ShippingFee,
					MoneyDefinition.Mapping.Map("ShippingFee_", nameof(Order)));

				Component(x => x.Discount,
					MoneyDefinition.Mapping.Map("Discount_", nameof(Order)));

				Component(x => x.Returned,
					MoneyDefinition.Mapping.Map("Returned_", nameof(Order)));

				Component(x => x.SubTotal,
					MoneyDefinition.Mapping.Map("SubTotal_", nameof(Order)));

				Component(x => x.Total,
					MoneyDefinition.Mapping.Map("Total_", nameof(Order)));

				Component(x => x.Paid,
					MoneyDefinition.Mapping.Map("Payment_", nameof(Order)));

				Component(x => x.Balance,
					MoneyDefinition.Mapping.Map("Balance_", nameof(Order)));

				Map(x => x.Status);

				Map(x => x.DueOn);

				Map(x => x.OrderedOn);

				References(x => x.OrderedBy);

				Map(x => x.RecreatedOn);

				References(x => x.RecreatedBy);

				Map(x => x.CreatedOn);

				References(x => x.CreatedBy);

				Map(x => x.StagedOn);

				References(x => x.StagedBy);

				Map(x => x.ShippedOn);

				References(x => x.ShippedBy);

				Map(x => x.RoutedOn);

				References(x => x.RoutedBy);

				Map(x => x.InvoicedOn);

				References(x => x.InvoicedBy);

				Map(x => x.PaidOn);

				References(x => x.PaidTo);

				Map(x => x.CompletedOn);

				References(x => x.CompletedBy);

				Map(x => x.ReturnedOn);

				References(x => x.ReturnedBy);

				Map(x => x.CancelledOn);

				References(x => x.CancelledBy);

				Map(x => x.CancellationReason);

				References(x => x.Pricing);

				HasMany(x => x.Items)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
                    .Inverse()
                    .OrderBy(nameof(OrderItem.Sequence)) 
                    // TODO: since this mapping (OrderBy) doesn't work properly on MSSQL (alias table is not included in the query), 
                    //    find a way hydrate releation based on sequence. Thiis will be posponed. Upgrade to new NHibernate version could be the fix
                    .AsSet();

                HasMany(x => x.Returns)
                    .Cascade.AllDeleteOrphan()
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .OrderBy(nameof(OrderReturn.Sequence))
                    .AsSet();

                HasMany(x => x.Payments)
                    .Cascade.AllDeleteOrphan()
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .OrderBy(nameof(OrderPayment.Sequence))
                    .AsSet();

                HasMany(x => x.Transactions)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
                    .AsSet();
			}
		}

		public class Validation : ValidationDef<Order>
		{
			public Validation()
			{
				Define(x => x.OrderNumber)
					.MaxLength(30);

				Define(x => x.InvoiceNumber)
					.MaxLength(30);

				Define(x => x.Branch)
					.NotNullable();

				Define(x => x.Customer)
					.NotNullable()
					.And.IsValid();

				Define(x => x.PaymentType);

				Define(x => x.Shipper);

				Define(x => x.TaxRate);

				Define(x => x.Tax);

				Define(x => x.ShippingFee);

				Define(x => x.Discount);

				Define(x => x.Returned);

				Define(x => x.SubTotal)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Total)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Paid);

				Define(x => x.Balance);

				Define(x => x.Status)
					.NotNullable();

				Define(x => x.DueOn);

				Define(x => x.RecreatedOn);

				Define(x => x.RecreatedBy);

				Define(x => x.OrderedOn);

				Define(x => x.OrderedBy);

				Define(x => x.CreatedOn);

				Define(x => x.CreatedBy);

				Define(x => x.StagedOn);

				Define(x => x.StagedBy);

				Define(x => x.ShippedOn);

				Define(x => x.ShippedBy);

				Define(x => x.RoutedOn);

				Define(x => x.RoutedBy);

				Define(x => x.InvoicedOn);

				Define(x => x.InvoicedBy);

				Define(x => x.PaidOn);

				Define(x => x.PaidTo);

				Define(x => x.CompletedOn);

				Define(x => x.CompletedBy);

				Define(x => x.CancelledOn);

				Define(x => x.CancelledBy);

				Define(x => x.CancellationReason);

				Define(x => x.Pricing);

				Define(x => x.Payments)
					.HasValidElements();

				Define(x => x.Items)
					.NotNullableAndNotEmpty()
					.And.HasValidElements();
			}
		}
	}
}
