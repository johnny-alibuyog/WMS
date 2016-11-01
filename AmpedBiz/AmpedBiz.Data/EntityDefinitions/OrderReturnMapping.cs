using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderReturnMapping : ClassMap<OrderReturn>
    {
        public OrderReturnMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Product);

            References(x => x.Order);

            References(x => x.ReturnedBy);

            Map(x => x.ReturnedOn);

            Component(x => x.Quantity,
                MeasureMapping.Map("Quantity_", nameof(OrderReturn)));

            Map(x => x.DiscountRate);

            Component(x => x.Discount,
                MoneyMapping.Map("Discount_", nameof(OrderReturn)));

            Component(x => x.UnitPrice,
                MoneyMapping.Map("UnitPrice_", nameof(OrderReturn)));

            Component(x => x.ExtendedPrice,
                MoneyMapping.Map("ExtendedPrice_", nameof(OrderReturn)));

            Component(x => x.TotalPrice,
                MoneyMapping.Map("TotalPrice_", nameof(OrderReturn)));
        }
    }
}
