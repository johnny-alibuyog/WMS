using AmpedBiz.Core.Orders;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data.Definitions.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.Definitions.PointOfSales
{
	public class PointOfSalePaymentDefinition
	{
		public class Mapping : ClassMap<PointOfSalePayment>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.PointOfSale);

				References(x => x.PaymentType);

				Component(x => x.Payment,
					MoneyDefinition.Mapping.Map("Payment_", nameof(PointOfSalePayment)));

				Component(x => x.Balance,
					MoneyDefinition.Mapping.Map("Balance_", nameof(PointOfSalePayment)));
			}
		}

		public class Validation : ValidationDef<PointOfSalePayment>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.PointOfSale)
					.NotNullable();

				Define(x => x.PaymentType)
					.NotNullable();

				Define(x => x.Payment)
					.NotNullable();
			}
		}
	}
}
