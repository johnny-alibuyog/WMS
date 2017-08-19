using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using Humanizer;
using NHibernate.Validator.Cfg.Loquacious;
using System.Collections.Generic;
using System.Linq;

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

                HasMany(x => x.UnitOfMeasures)
                    .Cascade.AllDeleteOrphan()
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .AsSet();
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

                Define(x => x.Inventory);

                Define(x => x.UnitOfMeasures)
                    .HasValidElements();


                this.ValidateInstance.By((instance, context) =>
                {
                    var valid = true;

                    var defaultCount = instance.UnitOfMeasures.Where(x => x.IsDefault).Count();
                    if (defaultCount > 1)
                    {
                        context.AddInvalid<Product, IEnumerable<ProductUnitOfMeasure>>(
                            message: $"There should be one default UOM for {instance.Name} but has {defaultCount.ToWords()}.", 
                            property: x => x.UnitOfMeasures
                        );
                        valid = false;
                    }

                    var standardCount = instance.UnitOfMeasures.Where(x => x.IsStandard).Count();
                    if (standardCount != 1)
                    {
                        context.AddInvalid<Product, IEnumerable<ProductUnitOfMeasure>>(
                            message: $"There should be one standard UOM for {instance.Name} but has {standardCount.ToWords()}.", 
                            property: x => x.UnitOfMeasures
                        );
                        valid = false;
                    }

                    return valid;
                });
            }
        }
    }
}