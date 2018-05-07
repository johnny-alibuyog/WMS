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

                References(x => x.Tenant);

                Map(x => x.Name);

                ApplyFilter<TenantDefinition.Filter>();
            }
        }

        public class Vaildation : ValidationDef<ProductCategory>
        {
            public Vaildation()
            {
                Define(x => x.Id)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(30);

                Define(x => x.Tenant)
                    .IsValid();

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);
            }
        }
    }
}
