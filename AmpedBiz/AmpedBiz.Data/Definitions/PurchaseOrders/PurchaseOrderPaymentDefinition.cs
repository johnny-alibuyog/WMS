using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data.Definitions.Common;
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

				References(x => x.PaidBy);

				Map(x => x.PaidOn);

				References(x => x.PaymentType);

				Component(x => x.Payment,
					MoneyDefinition.Mapping.Map("Payment_", nameof(PurchaseOrderPayment)));
			}
		}

		public class Validation : ValidationDef<PurchaseOrderPayment>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.PurchaseOrder)
					.NotNullable();

				Define(x => x.PaidBy)
					.NotNullable();

				Define(x => x.PaidOn);

				Define(x => x.PaymentType)
					.NotNullable();

				Define(x => x.Payment)
					.NotNullable();
			}
		}
	}
}
