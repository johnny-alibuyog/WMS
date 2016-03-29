using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserValidation : ValidationDef<User>
    {
        public UserValidation()
        {
            Define(x => x.Id)
                .NotNullableAndNotEmpty()
                .And.MaxLength(30);

            Define(x => x.Username)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.Password)
                .NotNullableAndNotEmpty()
                .And.MaxLength(50);

            Define(x => x.Person)
                .IsValid();

            Define(x => x.Address)
                .IsValid();

            Define(x => x.Branch)
                .IsValid();

            Define(x => x.UserRoles)
                .HasValidElements();
        }
    }
}
