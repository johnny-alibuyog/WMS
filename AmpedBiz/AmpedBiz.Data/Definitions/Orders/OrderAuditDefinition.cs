using AmpedBiz.Core.Orders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions
{
	public class OrderTransactionDefinition
	{
		public class Mapping : SubclassMap<OrderAudit>
		{
			public Mapping()
			{
				References(x => x.Order);

				Map(x => x.Status);

				Map(x => x.Type);
			}
		}

		public class Validation : ValidationDef<OrderAudit>
		{
			public Validation()
			{
				Define(x => x.Order)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Status);

				Define(x => x.Type);
			}
		}
	}
}
