using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductCategoryDefinition
    {
        public class Mapping : ClassMap<ProductCategory>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                Map(x => x.Name);
            }
        }

        public class Vaildation : ValidationDef<ProductCategory>
        {
            public Vaildation()
            {
                Define(x => x.Id)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(30);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);
            }
        }
    }
}
