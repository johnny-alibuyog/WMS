using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductUnitOfMeasureDefinition
    {
        public class Mapping : ClassMap<ProductUnitOfMeasure>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Product);

                References(x => x.UnitOfMeasure);

                Map(x => x.StandardEquivalentValue);

                Map(x => x.IsStandard);

                Map(x => x.IsDefault);

                //HasMany(x => x.Prices)
                //    .Cascade.AllDeleteOrphan()
                //    .Not.KeyNullable()
                //    .Not.KeyUpdate()
                //    .Inverse()
                //    .AsSet();
            }
        }

        public class Validation : ValidationDef<ProductUnitOfMeasure>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Product)
                    .NotNullable();

                Define(x => x.UnitOfMeasure)
                    .NotNullable();

                Define(x => x.StandardEquivalentValue);

                Define(x => x.IsStandard);

                Define(x => x.IsDefault);

                Define(x => x.Prices)
                    .NotNullableAndNotEmpty()
                    .And.HasValidElements();
            }
        }
    }
}
