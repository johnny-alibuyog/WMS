using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderItemMapping : ClassMap<OrderItem>
    {
        public OrderItemMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Order);

            References(x => x.Product);

            Map(x => x.PackagingSize);

            Component(x => x.Quantity, 
                MeasureMapping.Map("Quantity_", nameof(OrderItem)));

            Map(x => x.DiscountRate);

            Component(x => x.Discount, 
                MoneyMapping.Map("Discount_", nameof(OrderItem)));

            Component(x => x.UnitPrice, 
                MoneyMapping.Map("UnitPrice_", nameof(OrderItem)));

            Component(x => x.ExtendedPrice, 
                MoneyMapping.Map("ExtendedPrice_", nameof(OrderItem)));

            Component(x => x.TotalPrice,
                MoneyMapping.Map("TotalPrice_", nameof(OrderItem)));
        }
    }
}
