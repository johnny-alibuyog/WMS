using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserRoleValidation : ValidationDef<UserRole>
    {
        public UserRoleValidation()
        {
            Define(x => x.Id);

            Define(x => x.User)
                .IsValid();

            Define(x => x.Role)
                .IsValid();
        }
    }
}
