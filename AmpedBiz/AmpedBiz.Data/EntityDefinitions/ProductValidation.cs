using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductValidation : ValidationDef<Product>
    {
        public ProductValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty();

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);
        }
    }
}