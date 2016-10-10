using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryMapping : ClassMap<Inventory>
    {
        public InventoryMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("Product");

            HasOne(x => x.Product)
                .Constrained();

            References(x => x.UnitOfMeasure);

            References(x => x.UnitOfMeasureBase);

            Map(x => x.ConversionFactor);

            Component(x => x.BasePrice,
                MoneyMapping.Map("BasePrice_", nameof(Product)));

            Component(x => x.RetailPrice,
                MoneyMapping.Map("RetailPrice_", nameof(Product)));

            Component(x => x.WholesalePrice,
                MoneyMapping.Map("WholeSalePrice_", nameof(Product)));

            Component(x => x.BadStockPrice,
                MoneyMapping.Map("BadStockPrice_", nameof(Product)));

            Component(x => x.BadStock,
                MeasureMapping.Map("BadStock_", nameof(Inventory)));

            Component(x => x.Received,
                MeasureMapping.Map("Received_", nameof(Inventory)));

            Component(x => x.OnOrder,
                MeasureMapping.Map("OnOrder_", nameof(Inventory)));

            Component(x => x.OnHand,
                MeasureMapping.Map("OnHand_", nameof(Inventory)));

            Component(x => x.Allocated,
                MeasureMapping.Map("Allocated_", nameof(Inventory)));

            Component(x => x.Shipped,
                MeasureMapping.Map("Shipped_", nameof(Inventory)));

            Component(x => x.BackOrdered,
                MeasureMapping.Map("BackOrdered_", nameof(Inventory)));

            Component(x => x.Available,
                MeasureMapping.Map("Available_", nameof(Inventory)));

            Component(x => x.InitialLevel,
                MeasureMapping.Map("InitialLevel_", nameof(Inventory)));

            Component(x => x.Shrinkage,
                MeasureMapping.Map("Shrinkage_", nameof(Inventory)));

            Component(x => x.CurrentLevel,
                MeasureMapping.Map("CurrentLevel_", nameof(Inventory)));

            Component(x => x.TargetLevel,
                MeasureMapping.Map("TargetLevel_", nameof(Inventory)));

            Component(x => x.BelowTargetLevel,
                MeasureMapping.Map("BelowTargetLevel_", nameof(Inventory)));

            Component(x => x.ReorderLevel,
                MeasureMapping.Map("ReorderLevel_", nameof(Inventory)));

            Component(x => x.ReorderQuantity,
                MeasureMapping.Map("ReorderQuantity_", nameof(Inventory)));

            Component(x => x.MinimumReorderQuantity,
                MeasureMapping.Map("MinimumReorderQuantity_", nameof(Inventory)));

            HasMany(x => x.Stocks)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}