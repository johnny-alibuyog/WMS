using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductDefinition
    {
        public class Mapping : ClassMap<Product>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                Map(x => x.Code);

                Map(x => x.Name);

                Map(x => x.Description);

                Map(x => x.Image);

                Map(x => x.Discontinued);

                References(x => x.Category);

                References(x => x.Supplier);

                HasOne(x => x.Inventory)
                    .Cascade.All()
                    .Fetch.Join();
            }
        }

        public class Validation : ValidationDef<Product>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Code)
                    .MaxLength(255);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(255);

                Define(x => x.Description);

                Define(x => x.Image)
                    .MaxLength(255);

                Define(x => x.Discontinued);

                Define(x => x.Category);

                Define(x => x.Supplier)
                    .NotNullable()
                    .And.IsValid();
            }
        }
    }
}