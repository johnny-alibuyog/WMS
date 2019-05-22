﻿using AmpedBiz.Core.Common;
using AmpedBiz.Core.Orders;
using AmpedBiz.Data.Definitions.Common;
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

				References(x => x.PaidTo);

				Map(x => x.PaidOn);

				References(x => x.PaymentType);

				Component(x => x.Payment,
					MoneyDefinition.Mapping.Map("Payment_", nameof(OrderPayment)));

				Component(x => x.Balance,
					MoneyDefinition.Mapping.Map("Balance_", nameof(OrderPayment)));
			}
		}

		public class Validation : ValidationDef<OrderPayment>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.Order)
					.NotNullable();

				Define(x => x.PaidTo)
					.NotNullable();

				Define(x => x.PaidOn);

				Define(x => x.PaymentType)
					.NotNullable();

				Define(x => x.Payment)
					.NotNullable();

                this.ValidateInstance.By((instance, context) =>
                {
                    var valid = true;

                    if (instance.Payment.Amount <= 0)
                    {
                        context.AddInvalid<OrderPayment, Money>(
                            message: "Payment sould contain amount.",
                            property: x => x.Payment
                        );
                        valid = false;
                    }

                    return valid;
                });
            }
		}
	}
}