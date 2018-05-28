using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryAdjustmentReasonDefinition
    {
        public class Mapping : ClassMap<InventoryAdjustmentReason>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Tenant);

                Map(x => x.Name);

                Map(x => x.Description);

                Map(x => x.Type);

                ApplyFilter<TenantDefinition.Filter>();
            }
        }

        public class Validation : ValidationDef<InventoryAdjustmentReason>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Tenant);

                Define(x => x.Name)
                    .NotNullable()
                    .And.MaxLength(35);

                Define(x => x.Description)
                    .MaxLength(550);

                Define(x => x.Type);
            }
        }
    }
}
