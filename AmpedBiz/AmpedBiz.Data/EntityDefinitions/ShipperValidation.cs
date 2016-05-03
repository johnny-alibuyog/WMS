using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ShipperValidation : ValidationDef<Shipper>
    {
        public ShipperValidation()
        {
            Define(x => x.Id);

            Define(x => x.Name)
                .NotNullableAndNotEmpty()
                .And.MaxLength(255);

            //Define(x => x.Tenant);
            //    .NotNullable()
            //    .And.IsValid();

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Contact)
                .IsValid();

            Define(x => x.Orders);
        }
    }
}
