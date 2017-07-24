using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
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
