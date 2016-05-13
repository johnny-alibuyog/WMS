using System.Collections.Generic;
using System.Linq;
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
                .NotNullableAndNotEmpty()
                .And.HasValidElements();

            ValidateInstance.By((instance, context) =>
            {
                var baseUnitCount = instance.Units.Where(x => x.IsBaseUnit).Count();
                if (baseUnitCount != 1)
                {
                    context.AddInvalid<UnitOfMeasureClass, IEnumerable<UnitOfMeasure>>("One unit should be base.", x => x.Units);
                    return false;
                }

                return true;
            });
        }
    }
}
