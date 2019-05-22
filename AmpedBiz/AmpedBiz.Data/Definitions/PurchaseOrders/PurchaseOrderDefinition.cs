using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PurchaseOrders
{
	public class PurchaseOrderDefinition
	{
		public class Mapping : SubclassMap<PurchaseOrder>
		{
			public Mapping()
			{
				Map(x => x.PurchaseOrderNumber);

				Map(x => x.ReferenceNumber)
					.Index("IDX_ReferenceNumber");

				Map(x => x.VoucherNumber)
					.Index("IDX_VoucherNumber");

				References(x => x.Branch);

				References(x => x.PaymentType);

				References(x => x.Supplier);

				References(x => x.Shipper);

				Component(x => x.Tax,
					MoneyDefinition.Mapping.Map("Tax_", nameof(PurchaseOrder)));

				Component(x => x.ShippingFee,
					MoneyDefinition.Mapping.Map("ShippingFee_", nameof(PurchaseOrder)));

				Component(x => x.Discount,
					MoneyDefinition.Mapping.Map("Discount_", nameof(PurchaseOrder)));

				Component(x => x.SubTotal,
					MoneyDefinition.Mapping.Map("SubTotal_", nameof(PurchaseOrder)));

				Component(x => x.Total,
					MoneyDefinition.Mapping.Map("Total_", nameof(PurchaseOrder)));

				Component(x => x.Paid,
					MoneyDefinition.Mapping.Map("Paid_", nameof(PurchaseOrder)));

				Component(x => x.Balance,
					MoneyDefinition.Mapping.Map("Balance_", nameof(PurchaseOrder)));

				Map(x => x.Status);

				Map(x => x.ExpectedOn);

				References(x => x.CreatedBy);

				Map(x => x.CreatedOn);

				References(x => x.SubmittedBy);

				Map(x => x.SubmittedOn);

				References(x => x.ApprovedBy);

				Map(x => x.ApprovedOn);

				References(x => x.PaidBy);

				Map(x => x.PaidOn);

				References(x => x.ReceivedBy);

				Map(x => x.ReceivedOn);

				References(x => x.CompletedBy);

				Map(x => x.CompletedOn);

				References(x => x.CancelledBy);

				Map(x => x.CancelledOn);

				Map(x => x.CancellationReason);

				HasMany(x => x.Items)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
                    .OrderBy(nameof(PurchaseOrderItem.Sequence))
                    .AsSet();

				HasMany(x => x.Payments)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
                    .OrderBy(nameof(PurchaseOrderPayment.Sequence))
                    .AsSet();

				HasMany(x => x.Receipts)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
                    .OrderBy(nameof(PurchaseOrderReceipt.Sequence))
                    .AsSet();

				HasMany(x => x.Transactions)
					.Cascade.AllDeleteOrphan()
					.Not.KeyNullable()
					.Not.KeyUpdate()
					.Inverse()
					.AsSet();
			}
		}

		public class Validation : ValidationDef<PurchaseOrder>
		{
			public Validation()
			{
				Define(x => x.PurchaseOrderNumber)
					.MaxLength(30);

				Define(x => x.ReferenceNumber)
					.MaxLength(30);

				Define(x => x.VoucherNumber)
					.MaxLength(30);

				Define(x => x.Branch)
					.NotNullable();

				Define(x => x.PaymentType);

				Define(x => x.Supplier);

				Define(x => x.Shipper);

				Define(x => x.Tax)
					.IsValid();

				Define(x => x.ShippingFee)
					.IsValid();

				Define(x => x.Discount)
					.IsValid();

				Define(x => x.SubTotal)
					.IsValid();

				Define(x => x.Total)
					.IsValid();

				Define(x => x.Paid)
					.IsValid();

				Define(x => x.Balance)
					.IsValid();

				Define(x => x.Status);

				Define(x => x.ExpectedOn);

				Define(x => x.CreatedBy);

				Define(x => x.CreatedOn);

				Define(x => x.SubmittedBy);

				Define(x => x.SubmittedOn);

				Define(x => x.ApprovedBy);

				Define(x => x.ApprovedOn);

				Define(x => x.PaidBy);

				Define(x => x.PaidOn);

				Define(x => x.CompletedBy);

				Define(x => x.CompletedOn);

				Define(x => x.CancelledBy);

				Define(x => x.CancelledOn);

				Define(x => x.CancellationReason);

				Define(x => x.Items)
					.HasValidElements();

				Define(x => x.Payments)
					.HasValidElements();

				Define(x => x.Receipts)
					.HasValidElements();
			}
		}
	}
}