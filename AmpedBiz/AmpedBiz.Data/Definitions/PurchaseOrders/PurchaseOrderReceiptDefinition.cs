using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data.Definitions.Products;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PurchaseOrders
{
	public class PurchaseOrderReceiptDefinition
	{
		public class Mapping : ClassMap<PurchaseOrderReceipt>
		{
			public Mapping()
			{
				Id(x => x.Id)
				   .GeneratedBy.GuidComb();

				Map(x => x.BatchNumber);

				References(x => x.PurchaseOrder);

				References(x => x.ReceivedBy);

				Map(x => x.ReceivedOn);

				Map(x => x.ExpiresOn);

				References(x => x.Product);

				Component(x => x.Quantity,
					MeasureDefinition.Mapping.Map("Quantity_", nameof(PurchaseOrderReceipt)));

				Component(x => x.Standard,
					MeasureDefinition.Mapping.Map("Standard_", nameof(PurchaseOrderReceipt)));

				Component(x => x.QuantityStandardEquivalent,
					MeasureDefinition.Mapping.Map("QuantityStandardEquivalent_", nameof(PurchaseOrderReceipt)));
			}
		}

		public class Validation : ValidationDef<PurchaseOrderReceipt>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.BatchNumber)
					.MaxLength(255);

				Define(x => x.PurchaseOrder)
					.NotNullable();

				Define(x => x.ReceivedBy)
					.NotNullable();

				Define(x => x.ReceivedOn);

				Define(x => x.ExpiresOn)
					.IsInTheFuture();

				Define(x => x.Product)
					.NotNullable();

				Define(x => x.Quantity)
					.NotNullable();

				Define(x => x.Standard)
					.NotNullable();

				Define(x => x.QuantityStandardEquivalent)
					.NotNullable();
			}
		}
	}
}
