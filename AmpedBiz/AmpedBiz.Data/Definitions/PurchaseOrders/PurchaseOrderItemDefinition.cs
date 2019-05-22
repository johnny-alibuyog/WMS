using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data.Definitions.Common;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PurchaseOrders
{
	public class PurchaseOrderItemDefinition
	{
		public class Mapping : SubclassMap<PurchaseOrderItem>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(PurchaseOrderItem)}_{nameof(PurchaseOrderItem.Sequence)}");

                References(x => x.PurchaseOrder);

				References(x => x.Product);

				Component(x => x.Quantity,
					MeasureDefinition.Mapping.Map("Quantity_", nameof(PurchaseOrderItem)));

				Component(x => x.Standard,
					MeasureDefinition.Mapping.Map("Standard_", nameof(PurchaseOrderItem)));

				Component(x => x.QuantityStandardEquivalent,
					MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(PurchaseOrderItem)));

				Component(x => x.UnitCost,
					MoneyDefinition.Mapping.Map("UnitCost_", nameof(PurchaseOrderItem)));

				Component(x => x.TotalCost,
					MoneyDefinition.Mapping.Map("TotalCost_", nameof(PurchaseOrderItem)));
			}
		}

		public class Validation : ValidationDef<PurchaseOrderItem>
		{
			public Validation()
			{
                Define(x => x.Sequence);

				Define(x => x.PurchaseOrder)
					.NotNullable();

				Define(x => x.Product)
					.NotNullable();

				Define(x => x.Quantity)
					.NotNullable();

				Define(x => x.Standard)
					.NotNullable();

				Define(x => x.QuantityStandardEquivalent)
					.NotNullable();

				Define(x => x.UnitCost)
					.NotNullable()
					.And.IsValid();

				Define(x => x.TotalCost)
					.NotNullable()
					.And.IsValid();
			}
		}
	}
}