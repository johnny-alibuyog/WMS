using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CustomerValidation : ValidationDef<Customer>
    {
        public CustomerValidation()
        {
            Define(x => x.Id);

            Define(x => x.Code)
                .MaxLength(255);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);

            Define(x => x.Pricing)
                .IsValid();

            Define(x => x.Contact)
                .IsValid();

            Define(x => x.IsActive);

            Define(x => x.CreditLimit)
                .IsValid();

            Define(x => x.OfficeAddress)
                .IsValid();

            Define(x => x.BillingAddress)
                .IsValid();
        }
    }
}