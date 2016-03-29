using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class BranchMapping : ClassMap<Branch>
    {
        public BranchMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);

            Map(x => x.Description);

            Component(x => x.Address);
        }
    }
}
