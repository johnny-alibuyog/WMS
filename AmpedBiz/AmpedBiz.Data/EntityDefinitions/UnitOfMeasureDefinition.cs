using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UnitOfMeasureDefinition
    {
        public class Mapping : ClassMap<UnitOfMeasure>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                Map(x => x.Name);
            }
        }

        public class Validation : ValidationDef<UnitOfMeasure>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(255);
            }
        }
    }
}
