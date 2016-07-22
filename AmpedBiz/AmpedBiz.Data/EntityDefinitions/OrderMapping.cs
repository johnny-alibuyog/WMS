using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderMapping : ClassMap<Order>
    {
        public OrderMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Branch);

            Map(x => x.OrderedOn);

            Map(x => x.StagedOn);

            Map(x => x.RoutedOn);

            Map(x => x.InvoicedOn);

            Map(x => x.ShippedOn);

            Map(x => x.PaidOn);

            Map(x => x.CompletedOn);

            Map(x => x.CancelledOn);

            Map(x => x.CancellationReason);

            References(x => x.PaymentType);

            References(x => x.Shipper);

            Map(x => x.TaxRate);

            Component(x => x.Tax, 
                MoneyMapping.Map("Tax_", nameof(Order)));

            Component(x => x.ShippingFee, 
                MoneyMapping.Map("ShippingFee_", nameof(Order)));

            Component(x => x.Discount, 
                MoneyMapping.Map("Discount_", nameof(Order)));

            Component(x => x.SubTotal, 
                MoneyMapping.Map("SubTotal_", nameof(Order)));

            Component(x => x.Total, 
                MoneyMapping.Map("Total_", nameof(Order)));

            Map(x => x.Status);

            Map(x => x.IsActive);

            References(x => x.Customer);

            References(x => x.CreatedBy);

            References(x => x.StagedBy);

            References(x => x.RoutedBy);

            References(x => x.InvoicedBy);

            References(x => x.PaidTo);

            References(x => x.CompletedBy);

            References(x => x.CancelledBy);

            HasMany(x => x.Items)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();

            HasMany(x => x.Invoices)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}
