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

                Map(x => x.Size);

                Map(x => x.Barcode);

                Map(x => x.StandardEquivalentValue);

                Map(x => x.IsStandard);

                Map(x => x.IsDefault);

                HasMany(x => x.Prices)
                    .Cascade.AllDeleteOrphan()
                    .Not.KeyNullable()
                    .Not.KeyUpdate()
                    .Inverse()
                    .AsSet();
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

                Define(x => x.Barcode)
                    .MaxLength(255);

                Define(x => x.Size)
                    .MaxLength(255);

                Define(x => x.StandardEquivalentValue);

                Define(x => x.IsStandard);

                Define(x => x.IsDefault);

                Define(x => x.Prices)
                    .NotNullableAndNotEmpty()
                    .And.HasValidElements();

                this.ValidateInstance.By((instance, context) =>
                {
                    var valid = true;

                    if (instance.IsStandard && instance.StandardEquivalentValue != 1M)
                    {
                        context.AddInvalid<ProductUnitOfMeasure, decimal>(
                            message: $"Standard equivalent value for {instance.Product.Name} of standard unit {instance.UnitOfMeasure.Name} should be equat to one (1).",
                            property: x => x.StandardEquivalentValue
                        );
                        valid = false;
                    }
                    else if (instance.StandardEquivalentValue <= 0M)
                    {
                        context.AddInvalid<ProductUnitOfMeasure, decimal>(
                            message: $"Standard equivalent value for {instance.Product.Name} of unit {instance.UnitOfMeasure.Name} should be greater than zero(0).",
                            property: x => x.StandardEquivalentValue
                        );
                        valid = false;
                    }

                    return valid;
                });

            }
        }
    }
}
