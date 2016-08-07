using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UnitOfMeasureValidation : ValidationDef<UnitOfMeasure>
    {
        public UnitOfMeasureValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);

            Define(x => x.IsBaseUnit);

            Define(x => x.ConversionFactor);

            Define(x => x.UnitOfMeasureClass);
        }
    }
}
