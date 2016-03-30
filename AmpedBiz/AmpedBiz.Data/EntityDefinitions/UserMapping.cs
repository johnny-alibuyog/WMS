using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Username);

            Map(x => x.Password);

            Component(x => x.Person);

            Component(x => x.Address);

            References(x => x.Branch);

            HasMany(x => x.UserRoles)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}
