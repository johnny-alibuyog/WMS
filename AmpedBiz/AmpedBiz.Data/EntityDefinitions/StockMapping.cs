using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class StockMapping : ClassMap<Stock>
    {
        public StockMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.CreatedOn);

            References(x => x.CreatedBy);

            Map(x => x.ModifiedOn);

            References(x => x.ModifiedBy);

            References(x => x.Inventory);

            Component(x => x.Quantity,
                MeasureMapping.Map("Quantity_", nameof(Stock)));

            Map(x => x.ExpiresOn);

            Map(x => x.Bad);
        }
    }
}
