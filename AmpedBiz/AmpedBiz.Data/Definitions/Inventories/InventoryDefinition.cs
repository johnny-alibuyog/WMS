using AmpedBiz.Core.Inventories;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Inventories
{
	public class InventoryDefinition
	{
		public class Mapping : ClassMap<Inventory>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.Branch);

				References(x => x.Product);

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

				Component(x => x.IncreaseAdjustment,
					MeasureDefinition.Mapping.Map("IncreaseAdjustment_", nameof(Inventory)));

				Component(x => x.DecreaseAdjustment,
					MeasureDefinition.Mapping.Map("DecreaseAdjustment_", nameof(Inventory)));

				//HasMany(x => x.Stocks)
				//    .Cascade.AllDeleteOrphan()
				//    .Not.KeyNullable()
				//    .Not.KeyUpdate()
				//    .Inverse()
				//    .AsBag();

				HasMany(x => x.Adjustments)
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

				Define(x => x.Branch)
					.IsValid();

				Define(x => x.Product)
					.IsValid();

				Define(x => x.BadStock)
					.IsValid();

				Define(x => x.Received)
					.IsValid();

				Define(x => x.OnOrder)
					.IsValid();

				Define(x => x.OnHand)
					.IsValid();

				Define(x => x.Allocated)
					.IsValid();

				Define(x => x.Shipped)
					.IsValid();

				Define(x => x.BackOrdered)
					.IsValid();

				Define(x => x.Returned)
					.IsValid();

				Define(x => x.Available)
					.IsValid();

				Define(x => x.InitialLevel)
					.IsValid();

				Define(x => x.Shrinkage)
					.IsValid();

				Define(x => x.CurrentLevel)
					.IsValid();

				Define(x => x.TargetLevel)
					.IsValid();

				Define(x => x.BelowTargetLevel)
					.IsValid();

				Define(x => x.ReorderLevel)
					.IsValid();

				Define(x => x.ReorderQuantity)
					.IsValid();

				Define(x => x.MinimumReorderQuantity)
					.IsValid();

				Define(x => x.IncreaseAdjustment)
					.IsValid();

				Define(x => x.DecreaseAdjustment)
					.IsValid();

				//Define(x => x.Stocks);

				//this.ValidateInstance.By((instance, context) =>
				//{
				//    var valid = true;

				//    var defaultCount = instance.UnitOfMeasures.Where(x => x.IsDefault).Count();
				//    if (defaultCount > 1)
				//    {
				//        context.AddInvalid<Product, IEnumerable<ProductUnitOfMeasure>>(
				//            message: $"There should be one default UOM for {instance.Name} but has {defaultCount.ToWords()}.",
				//            property: x => x.UnitOfMeasures
				//        );
				//        valid = false;
				//    }

				//    var standardCount = instance.UnitOfMeasures.Where(x => x.IsStandard).Count();
				//    if (standardCount != 1)
				//    {
				//        context.AddInvalid<Product, IEnumerable<ProductUnitOfMeasure>>(
				//            message: $"There should be one standard UOM for {instance.Name} but has {standardCount.ToWords()}.",
				//            property: x => x.UnitOfMeasures
				//        );
				//        valid = false;
				//    }

				//    return valid;
				//});
			}
		}
	}
}