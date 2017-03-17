using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class SupplierValidation : ValidationDef<Supplier>
    {
        public SupplierValidation()
        {
            Define(x => x.Id);

            Define(x => x.Code)
                .MaxLength(150);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(150);

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Contact)
                .IsValid();

            Define(x => x.Products)
                .HasValidElements();
        }
    }
}
