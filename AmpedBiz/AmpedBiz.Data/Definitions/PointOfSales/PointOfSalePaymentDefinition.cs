using AmpedBiz.Core.PointOfSales;
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
			}
		}

		public class Validation : ValidationDef<PointOfSalePayment>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.PointOfSale)
					.NotNullable();
			}
		}
	}
}
