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

            References(x => x.Reason);

            References(x => x.ReturnedBy);

            Map(x => x.ReturnedOn);

            Component(x => x.Quantity,
                MeasureMapping.Map("Quantity_", nameof(OrderReturn)));

            Component(x => x.Returned,
                MoneyMapping.Map("Returned_", nameof(OrderReturn)));
        }
    }
}
