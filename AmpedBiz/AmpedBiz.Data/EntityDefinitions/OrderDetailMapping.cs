using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderDetailMapping : ClassMap<OrderDetail>
    {
        public OrderDetailMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Product);

            References(x => x.Order);

            Component(x => x.Quantity, MeasureMapping.Map("Quantity_", nameof(OrderDetail)));

            Component(x => x.Discount, MoneyMapping.Map("Discount_", nameof(OrderDetail)));

            Component(x => x.UnitPrice, MoneyMapping.Map("UnitPrice_", nameof(OrderDetail)));

            Component(x => x.ExtendedPrice, MoneyMapping.Map("ExtendedPrice_", nameof(OrderDetail)));

            Map(x => x.Status);

            Map(x => x.InsufficientInventory);
        }
    }
}
