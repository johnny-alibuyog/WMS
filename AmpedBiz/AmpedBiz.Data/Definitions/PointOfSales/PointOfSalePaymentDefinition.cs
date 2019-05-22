using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PointOfSales
{
    public class PointOfSalePaymentDefinition
	{
		public class Mapping : SubclassMap<PointOfSalePayment>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(PointOfSalePayment)}_{nameof(PointOfSalePayment.Sequence)}");

                References(x => x.PointOfSale);

				Map(x => x.PaidOn);

				References(x => x.PaidTo);

				References(x => x.PaymentType);

				Component(x => x.Payment,
					MoneyDefinition.Mapping.Map("Payment_", nameof(PointOfSalePayment)));
			}
		}

		public class Validation : ValidationDef<PointOfSalePayment>
		{
			public Validation()
			{
                Define(x => x.Sequence);

				Define(x => x.PointOfSale)
					.NotNullable();

				Define(x => x.PaidOn);

				Define(x => x.PaidTo)
					.NotNullable();

				Define(x => x.PaymentType)
					.NotNullable();

				Define(x => x.Payment)
					.NotNullable();
			}
		}
	}
}
