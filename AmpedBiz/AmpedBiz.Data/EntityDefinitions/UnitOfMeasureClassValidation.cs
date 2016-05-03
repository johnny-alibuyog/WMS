using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UnitOfMeasureClassValidation : ValidationDef<UnitOfMeasureClass>
    {
        public UnitOfMeasureClassValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);

            Define(x => x.Units)
                .HasValidElements();
        }
    }
}
