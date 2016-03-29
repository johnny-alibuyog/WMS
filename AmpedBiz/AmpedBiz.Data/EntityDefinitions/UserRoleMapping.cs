using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserRoleMapping : ClassMap<UserRole>
    {
        public UserRoleMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.User);

            References(x => x.Role);
        }
    }
}
