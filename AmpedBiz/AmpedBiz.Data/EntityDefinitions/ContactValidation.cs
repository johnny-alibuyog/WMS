using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ContactValidation : ValidationDef<Contact>
    {
        public ContactValidation()
        {
            Define(x => x.Email)
                .MaxLength(50);
                //.And.IsEmail(); TODO: implement soon if ui validation is already setup :)

            Define(x => x.Landline)
                .MaxLength(50);

            Define(x => x.Fax)
                .MaxLength(50);

            Define(x => x.Mobile)
                .MaxLength(50);

            Define(x => x.Web)
                .MaxLength(100);
        }
    }
}
