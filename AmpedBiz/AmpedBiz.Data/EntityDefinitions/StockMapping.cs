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

            DiscriminateSubClassesOnColumn("Movement")
                .Length(20);
        }
    }

    public class ReceivedStockMapping : SubclassMap<ReceivedStock>
    {
        public ReceivedStockMapping()
        {
            DiscriminatorValue("Received");
        }
    }

    public class ReleasedStockMapping : SubclassMap<ReleasedStock>
    {
        public ReleasedStockMapping()
        {
            DiscriminatorValue("Released");
        }
    }

    public class ShrinkedStockMapping : SubclassMap<ShrinkedStock>
    {
        public ShrinkedStockMapping()
        {
            DiscriminatorValue("Shrinked");
        }
    }
}
