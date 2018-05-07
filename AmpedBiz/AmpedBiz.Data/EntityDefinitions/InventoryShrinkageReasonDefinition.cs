using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryShrinkageReasonDefinition
    {
        public class Mapping : ClassMap<InventoryShrinkageReason>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                References(x => x.Tenant);

                Map(x => x.Name);
            }
        }

        public class Validation : ValidationDef<InventoryShrinkageReason>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Tenant);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(50);
            }
        }
    }
}
