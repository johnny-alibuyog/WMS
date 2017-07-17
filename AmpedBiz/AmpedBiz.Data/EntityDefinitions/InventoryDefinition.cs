using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryDefinition
    {
        public class Mapping : ClassMap<Inventory>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Foreign("Product");

                HasOne(x => x.Product)
                    .Constrained();

                Map(x => x.IndividualBarcode);

                Map(x => x.PackagingBarcode);

                References(x => x.UnitOfMeasure);

                References(x => x.PackagingUnitOfMeasure);

                Map(x => x.PackagingSize);

                Component(x => x.BasePrice,
                    MoneyDefinition.Mapping.Map("BasePrice_", nameof(Inventory)));

                Component(x => x.WholesalePrice,
                    MoneyDefinition.Mapping.Map("WholesalePrice_", nameof(Inventory)));

                Component(x => x.RetailPrice,
                    MoneyDefinition.Mapping.Map("RetailPrice_", nameof(Inventory)));

                Component(x => x.BadStockPrice,
                    MoneyDefinition.Mapping.Map("BadStockPrice_", nameof(Inventory)));

                Component(x => x.BadStock,
                    MeasureDefinition.Mapping.Map("BadStock_", nameof(Inventory)));

                Component(x => x.Received,
                    MeasureDefinition.Mapping.Map("Received_", nameof(Inventory)));

                Component(x => x.OnOrder,
                    MeasureDefinition.Mapping.Map("OnOrder_", nameof(Inventory)));

                Component(x => x.OnHand,
                    MeasureDefinition.Mapping.Map("OnHand_", nameof(Inventory)));

                Component(x => x.Allocated,
                    MeasureDefinition.Mapping.Map("Allocated_", nameof(Inventory)));

                Component(x => x.Shipped,
                    MeasureDefinition.Mapping.Map("Shipped_", nameof(Inventory)));

                Component(x => x.BackOrdered,
                    MeasureDefinition.Mapping.Map("BackOrdered_", nameof(Inventory)));

                Component(x => x.Returned,
                    MeasureDefinition.Mapping.Map("Returned_", nameof(Inventory)));

                Component(x => x.Available,
                    MeasureDefinition.Mapping.Map("Available_", nameof(Inventory)));

                Component(x => x.InitialLevel,
                    MeasureDefinition.Mapping.Map("InitialLevel_", nameof(Inventory)));

                Component(x => x.Shrinkage,
                    MeasureDefinition.Mapping.Map("Shrinkage_", nameof(Inventory)));

                Component(x => x.CurrentLevel,
                    MeasureDefinition.Mapping.Map("CurrentLevel_", nameof(Inventory)));

                Component(x => x.TargetLevel,
                    MeasureDefinition.Mapping.Map("TargetLevel_", nameof(Inventory)));

                Component(x => x.BelowTargetLevel,
                    MeasureDefinition.Mapping.Map("BelowTargetLevel_", nameof(Inventory)));

                Component(x => x.ReorderLevel,
                    MeasureDefinition.Mapping.Map("ReorderLevel_", nameof(Inventory)));

                Component(x => x.ReorderQuantity,
                    MeasureDefinition.Mapping.Map("ReorderQuantity_", nameof(Inventory)));

                Component(x => x.MinimumReorderQuantity,
                    MeasureDefinition.Mapping.Map("MinimumReorderQuantity_", nameof(Inventory)));

                HasMany(x => x.Stocks)
                    .Cascade.AllDeleteOrphan()
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .AsBag();
            }
        }

        public class Validation : ValidationDef<Inventory>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Product)
                    .IsValid();

                Define(x => x.IndividualBarcode)
                    .MaxLength(255);

                Define(x => x.PackagingBarcode)
                    .MaxLength(255);

                Define(x => x.UnitOfMeasure);

                Define(x => x.PackagingUnitOfMeasure);

                Define(x => x.PackagingSize);

                Define(x => x.BasePrice);

                Define(x => x.WholesalePrice);

                Define(x => x.RetailPrice);

                Define(x => x.BadStockPrice);

                Define(x => x.BadStock);

                Define(x => x.Received);

                Define(x => x.OnOrder);

                Define(x => x.OnHand);

                Define(x => x.Allocated);

                Define(x => x.Shipped);

                Define(x => x.BackOrdered);

                Define(x => x.Returned);

                Define(x => x.Available);

                Define(x => x.InitialLevel);

                Define(x => x.Shrinkage);

                Define(x => x.CurrentLevel);

                Define(x => x.TargetLevel);

                Define(x => x.BelowTargetLevel);

                Define(x => x.ReorderLevel);

                Define(x => x.ReorderQuantity);

                Define(x => x.MinimumReorderQuantity);

                Define(x => x.Stocks);
            }
        }
    }
}