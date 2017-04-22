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

            Map(x => x.OrderNumber);

            References(x => x.Branch);

            References(x => x.Customer);

            References(x => x.PaymentType);

            References(x => x.Shipper);

            Map(x => x.TaxRate);

            Component(x => x.Tax, 
                MoneyMapping.Map("Tax_", nameof(Order)));

            Component(x => x.ShippingFee, 
                MoneyMapping.Map("ShippingFee_", nameof(Order)));

            Component(x => x.Discount, 
                MoneyMapping.Map("Discount_", nameof(Order)));

            Component(x => x.Returned,
                MoneyMapping.Map("Returned_", nameof(Order)));

            Component(x => x.SubTotal, 
                MoneyMapping.Map("SubTotal_", nameof(Order)));

            Component(x => x.Total, 
                MoneyMapping.Map("Total_", nameof(Order)));

            Component(x => x.Paid,
                MoneyMapping.Map("Payment_", nameof(Order)));

            Map(x => x.Status);

            Map(x => x.DueOn);

            Map(x => x.OrderedOn);

            References(x => x.OrderedBy);

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
                .AsSet();

            HasMany(x => x.Returns)
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
}
