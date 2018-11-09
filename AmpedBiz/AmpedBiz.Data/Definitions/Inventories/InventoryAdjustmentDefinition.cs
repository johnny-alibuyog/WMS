using AmpedBiz.Core.Inventories;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Inventories
{
	public class InventoryAdjustmentDefinition
	{
		public class Mapping : ClassMap<InventoryAdjustment>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.Inventory);

				References(x => x.AdjustedBy);

				Map(x => x.AdjustedOn);

				References(x => x.Reason);

				Map(x => x.Remarks);

				Map(x => x.Type);

				Component(x => x.Quantity,
					MeasureDefinition.Mapping.Map("Quantity_", nameof(InventoryAdjustment)));

				Component(x => x.Standard,
					MeasureDefinition.Mapping.Map("Standard_", nameof(InventoryAdjustment)));

				Component(x => x.QuantityStandardEquivalent,
					MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(InventoryAdjustment)));
			}
		}

		public class Validation : ValidationDef<InventoryAdjustment>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.Inventory)
					.NotNullable()
					.And.IsValid();

				Define(x => x.AdjustedBy)
					.NotNullable()
					.And.IsValid();

				Define(x => x.AdjustedOn);

				Define(x => x.Reason)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Remarks)
					.MaxLength(550);

				Define(x => x.Type);

				Define(x => x.Quantity)
					.NotNullable()
					.And.IsValid();
			}
		}
	}
}
