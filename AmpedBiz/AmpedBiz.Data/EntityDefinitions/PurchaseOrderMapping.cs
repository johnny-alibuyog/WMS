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
            
            Map(x => x.CreationDate);
            References(x => x.CreatedBy);

            Map(x => x.SubmittedDate);
            References(x => x.SubmittedBy);

            Map(x => x.RejectedDate);
            References(x => x.RejectedBy);

            Map(x => x.ApprovedDate);
            References(x => x.ApprovedBy);

            Map(x => x.ClosedDate);
            References(x => x.CompletedBy);

            Map(x => x.Reason);

            Component(x => x.SubTotal, MoneyMapping.Map("SubTotal_", nameof(PurchaseOrder)));

            References(x => x.Supplier);

            Component(x => x.Tax, MoneyMapping.Map("Tax_", nameof(PurchaseOrder)));

            Component(x => x.Total, MoneyMapping.Map("Total_", nameof(PurchaseOrder)));
        }
    }
}