using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductUnitOfMeasurePriceDefinition
    {
        public class Mapping : ClassMap<ProductUnitOfMeasurePrice>
        {
            protected Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.ProductUnitOfMeasure);

                //References(x => x.Branch);

                References(x => x.Pricing);

                Component(x => x.Price,
                    MoneyDefinition.Mapping.Map("Price_", nameof(ProductUnitOfMeasurePrice)));

                ApplyFilter<BranchDefinition.Filter>();
            }
        }

        public class Validation : ValidationDef<ProductUnitOfMeasurePrice>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.ProductUnitOfMeasure)
                    .NotNullable();

                //Define(x => x.Branch)
                //    .NotNullable()
                //    .And.IsValid();

                Define(x => x.Pricing)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.Price)
                    .NotNullable()
                    .And.IsValid();
            }
        }
    }
}
