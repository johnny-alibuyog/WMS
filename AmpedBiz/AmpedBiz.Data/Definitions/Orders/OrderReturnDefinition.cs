using AmpedBiz.Core.Orders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions
{
	public class OrderReturnDefinition
	{
		public class Mapping : SubclassMap<OrderReturn>
		{
			public Mapping()
			{
                Map(x => x.Sequence)
                    .Index($"IDX_{nameof(OrderReturn)}_{nameof(OrderReturn.Sequence)}");

                References(x => x.Order);

				References(x => x.ReturnedBy);

				Map(x => x.ReturnedOn);
			}
		}

		public class Validation : ValidationDef<OrderReturn>
		{
			public Validation()
			{
                Define(x => x.Sequence);

                Define(x => x.Order)
					.NotNullable()
					.And.IsValid();

				Define(x => x.ReturnedBy)
					.NotNullable()
					.And.IsValid();

				Define(x => x.ReturnedOn);
			}
		}
	}
}
