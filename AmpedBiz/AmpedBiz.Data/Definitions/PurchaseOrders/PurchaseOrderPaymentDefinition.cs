using AmpedBiz.Core.PurchaseOrders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PurchaseOrders
{
    public class PurchaseOrderPaymentDefinition
	{
		public class Mapping : SubclassMap<PurchaseOrderPayment>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(PurchaseOrderPayment)}_{nameof(PurchaseOrderPayment.Sequence)}");

                References(x => x.PurchaseOrder);
			}
		}

		public class Validation : ValidationDef<PurchaseOrderPayment>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.PurchaseOrder)
					.NotNullable();
			}
		}
	}
}
