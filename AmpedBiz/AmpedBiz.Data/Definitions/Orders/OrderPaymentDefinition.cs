using AmpedBiz.Core.Orders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions
{
    public class OrderPaymentDefinition
	{
		public class Mapping : SubclassMap<OrderPayment>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(OrderPayment)}_{nameof(OrderPayment.Sequence)}");

                References(x => x.Order);
			}
		}

		public class Validation : ValidationDef<OrderPayment>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.Order)
					.NotNullable();
            }
		}
	}
}