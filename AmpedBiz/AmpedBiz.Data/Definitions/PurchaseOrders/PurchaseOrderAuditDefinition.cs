using AmpedBiz.Core.PurchaseOrders;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.PurchaseOrdersS
{
	public class PurchaseOrderAuditDefinition
    {
		public class Mapping : SubclassMap<PurchaseOrderAudit>
		{
			public Mapping()
			{
				References(x => x.PurchaseOrder);

				Map(x => x.Status);

				Map(x => x.Type);
			}
		}

		public class Validation : ValidationDef<PurchaseOrderAudit>
		{
			public Validation()
			{
                Define(x => x.PurchaseOrder)
					.NotNullable()
					.And.IsValid();

				Define(x => x.Status);

				Define(x => x.Type);
			}
		}
	}
}
