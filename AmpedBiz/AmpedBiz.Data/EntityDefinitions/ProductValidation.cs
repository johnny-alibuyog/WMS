using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductValidation : ValidationDef<Product>
    {
        public ProductValidation()
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

            Define(x => x.Supplier);
        }
    }
}