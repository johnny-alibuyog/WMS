using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnReasonDefinition
    {
        public class Mapping : ClassMap<ReturnReason>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                References(x => x.Tenant);

                Map(x => x.Name);

                ApplyFilter<TenantDefinition.Filter>();
            }
        }

        public class Validation : ValidationDef<ReturnReason>
        {
            public Validation()
            {
                Define(x => x.Id)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(15);

                Define(x => x.Tenant)
                    .IsValid();

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(50);
            }
        }
    }
}
