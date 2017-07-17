using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnDefinition
    {
        public class Mapping : ClassMap<Return>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Branch);

                References(x => x.Customer);

                References(x => x.ReturnedBy);

                Map(x => x.ReturnedOn);

                Map(x => x.Remarks);

                Component(x => x.Total,
                    MoneyDefinition.Mapping.Map("Total_", nameof(Return)));

                HasMany(x => x.Items)
                    .Cascade.AllDeleteOrphan()
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .AsSet();
            }
        }

        public class Validation : ValidationDef<Return>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Branch)
                    .NotNullable();

                Define(x => x.Customer)
                    .NotNullable();

                Define(x => x.ReturnedBy)
                    .NotNullable();

                Define(x => x.ReturnedOn);

                Define(x => x.Remarks)
                    .MaxLength(500);

                Define(x => x.Items)
                    .NotNullableAndNotEmpty()
                    .And.HasValidElements();
            }
        }
    }
}
