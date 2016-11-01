using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnItemMapping : ClassMap<ReturnItem>
    {
        public ReturnItemMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Product);

            References(x => x.Return);

            References(x => x.ReturnReason);

            Component(x => x.Quantity,
                MeasureMapping.Map("Quantity_", nameof(ReturnItem)));

            Component(x => x.UnitPrice,
                MoneyMapping.Map("UnitPrice_", nameof(ReturnItem)));

            Component(x => x.TotalPrice,
                MoneyMapping.Map("TotalPrice_", nameof(ReturnItem)));
        }
    }
}
