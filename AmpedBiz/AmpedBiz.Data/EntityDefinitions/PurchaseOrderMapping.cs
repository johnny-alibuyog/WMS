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

            References(x => x.CompletedBy);

            Map(x => x.ClosedDate);

            References(x => x.CreatedBy);

            Map(x => x.CreationDate);

            Map(x => x.ExpectedDate);

            Map(x => x.OrderDate);

            Component(x => x.Payment, MoneyMapping.Map("PaymentAmount_", nameof(PurchaseOrder)));

            Map(x => x.PaymentDate);

            References(x => x.PaymentType);

            HasMany(x => x.PurchaseOrderDetails)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();

            Component(x => x.ShippingFee, MoneyMapping.Map("ShippingFee_", nameof(PurchaseOrder)));

            Map(x => x.Status);

            References(x => x.SubmittedBy);

            Map(x => x.SubmittedDate);

            Component(x => x.SubTotal, MoneyMapping.Map("SubTotal_", nameof(PurchaseOrder)));

            References(x => x.Supplier);

            Component(x => x.Tax, MoneyMapping.Map("Tax_", nameof(PurchaseOrder)));

            Component(x => x.Total, MoneyMapping.Map("Total_", nameof(PurchaseOrder)));
        }
    }
}