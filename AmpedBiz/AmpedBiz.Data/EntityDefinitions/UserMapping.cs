using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            Schema("public");

            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Username);

            Map(x => x.Password);
        }
    }
}
