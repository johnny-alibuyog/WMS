using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PricingSchemeValidation : ValidationDef<PricingScheme>
    {
        public PricingSchemeValidation()
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
