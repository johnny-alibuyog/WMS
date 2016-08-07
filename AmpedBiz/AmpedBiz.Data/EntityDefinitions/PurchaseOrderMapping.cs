using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderMapping : ClassMap<PurchaseOrder>
    {
        public PurchaseOrderMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.PurchaseOrderNumber);

            References(x => x.PaymentType);

            References(x => x.Supplier);

            References(x => x.Shipper);

            Component(x => x.Tax,
                MoneyMapping.Map("Tax_", nameof(PurchaseOrder)));

            Component(x => x.ShippingFee,
                MoneyMapping.Map("ShippingFee_", nameof(PurchaseOrder)));

            Component(x => x.Discount,
                MoneyMapping.Map("Discount_", nameof(PurchaseOrder)));

            Component(x => x.SubTotal,
                MoneyMapping.Map("SubTotal_", nameof(PurchaseOrder)));

            Component(x => x.Total,
                MoneyMapping.Map("Total_", nameof(PurchaseOrder)));

            Component(x => x.Payment,
                MoneyMapping.Map("Payment_", nameof(PurchaseOrder)));

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
                .AsBag();

            HasMany(x => x.Payments)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();

            HasMany(x => x.Receipts)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}