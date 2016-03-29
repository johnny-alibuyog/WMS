using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class AddressValidation : ValidationDef<Address>
    {
        public AddressValidation()
        {
            Define(x => x.Street)
                .MaxLength(150);

            Define(x => x.Barangay)
                .MaxLength(150);

            Define(x => x.City)
                .MaxLength(150);

            Define(x => x.Province)
                .MaxLength(150);

            Define(x => x.Region)
                .MaxLength(150);

            Define(x => x.Country)
                .MaxLength(150);

            Define(x => x.ZipCode)
                .MaxLength(150);
        }
    }
}
