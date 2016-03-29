using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CustomerMapping : ClassMap<Customer>
    {
        public CustomerMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Address2);

            Component(x => x.Address);
            Component(x => x.Contact);
        }
    }
}