using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class GoodStockInventoryMapping : ClassMap<GoodStockInventory>
    {
        public GoodStockInventoryMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("Product");

            References(x => x.UnitOfMeasure);

            References(x => x.UnitOfMeasureBase);

            Map(x => x.ConvertionFactor);

            Component(x => x.ReorderLevel, MeasureMapping.Map("ReorderLevel_", nameof(GoodStockInventory)));

            Component(x => x.TargetLevel, MeasureMapping.Map("TargetLevel_", nameof(GoodStockInventory)));

            Component(x => x.MinimumReorderQuantity, MeasureMapping.Map("MinimumReorderQuantity_", nameof(GoodStockInventory)));

            Component(x => x.Received, MeasureMapping.Map("Received_", nameof(GoodStockInventory)));

            Component(x => x.OnOrder, MeasureMapping.Map("OnOrder_", nameof(GoodStockInventory)));

            Component(x => x.Shipped, MeasureMapping.Map("Shipped_", nameof(GoodStockInventory)));

            Component(x => x.Allocated, MeasureMapping.Map("Allocated_", nameof(GoodStockInventory)));

            Component(x => x.BackOrdered, MeasureMapping.Map("BackOrdered_", nameof(GoodStockInventory)));

            Component(x => x.InitialLevel, MeasureMapping.Map("InitialLevel_", nameof(GoodStockInventory)));

            Component(x => x.OnHand, MeasureMapping.Map("OnHand_", nameof(GoodStockInventory)));

            Component(x => x.Available, MeasureMapping.Map("Available_", nameof(GoodStockInventory)));

            Component(x => x.CurrentLevel, MeasureMapping.Map("CurrentLevel_", nameof(GoodStockInventory)));

            Component(x => x.BelowTargetLevel, MeasureMapping.Map("BelowTargetLevel_", nameof(GoodStockInventory)));

            Component(x => x.ReorderQuantity, MeasureMapping.Map("ReorderQuantity_", nameof(GoodStockInventory)));

            HasOne(x => x.Product).Constrained();

            //HasMany(x => x.Shrinkages)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();
        }
    }

    public class BadStockInventoryMapping : ClassMap<BadStockInventory>
    {
        public BadStockInventoryMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("Product");

            References(x => x.UnitOfMeasure);

            References(x => x.UnitOfMeasureBase);

            Map(x => x.ConvertionFactor);

            Component(x => x.OnHand, MeasureMapping.Map("OnHand_", nameof(BadStockInventory)));

            HasOne(x => x.Product).Constrained();

            //HasMany(x => x.Shrinkages)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();
        }
    }

    public class GoodStockInventoryReceivedMapping : ClassMap<GoodStockInventoryReceived>
    {
        public GoodStockInventoryReceivedMapping()
        {
            Id(x => x.Id)
                .GeneratedBy
                .GuidComb();

            Map(x => x.ExpiryDate);

            Map(x => x.ReceivedDate);
        }
    }

    public class BadStockInventoryReceivedMapping : ClassMap<BadStockInventoryReceived>
    {
        public BadStockInventoryReceivedMapping()
        {
            Id(x => x.Id)
                .GeneratedBy
                .GuidComb();
        }
    }
}