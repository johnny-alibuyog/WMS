using AmpedBiz.Core.Orders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions
{
	public class OrderTransactionDefinition
	{
		public class Mapping : ClassMap<OrderTransaction>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.Order);

				References(x => x.TransactedBy);

				Map(x => x.TransactedOn);

				Map(x => x.Status);

				Map(x => x.Type);
			}
		}

		public class Validation : ValidationDef<OrderTransaction>
		{
			public Validation()
			{
				Define(x => x.Id);

				Define(x => x.Order)
					.NotNullable()
					.And.IsValid();

				Define(x => x.TransactedBy)
					.NotNullable()
					.And.IsValid();

				Define(x => x.TransactedOn);

				Define(x => x.Status);

				Define(x => x.Type);
			}
		}
	}
}
