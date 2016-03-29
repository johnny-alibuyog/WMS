using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PersonValidation : ValidationDef<Person>
    {
        public PersonValidation()
        {
            Define(x => x.FirstName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(75);

            Define(x => x.MiddleName)
                .MaxLength(75);

            Define(x => x.LastName)
                .NotNullableAndNotEmpty()
                .And.MaxLength(75);

            Define(x => x.BirthDate);
        }
    }
}
