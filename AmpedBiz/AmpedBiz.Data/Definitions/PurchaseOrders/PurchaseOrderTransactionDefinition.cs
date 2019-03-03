using AmpedBiz.Core.PurchaseOrders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PurchaseOrdersS
{
	public class PurchaseOrderTransactionDefinition
	{
		public class Mapping : ClassMap<PurchaseOrderTransaction>
		{
			public Mapping()
			{
				Id(x => x.Id)
					.GeneratedBy.GuidComb();

				References(x => x.PurchaseOrder);

				References(x => x.TransactedBy);

				Map(x => x.TransactedOn);

				Map(x => x.Status);

				Map(x => x.Type);
			}
		}

		public class Validation : ValidationDef<PurchaseOrderTransaction>
		{
			public Validation()
			{
				Define(x => x.Id);

                Define(x => x.PurchaseOrder)
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
