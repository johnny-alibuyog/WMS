using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ShipperMapping : ClassMap<Shipper>
    {
        public ShipperMapping()
        {

            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);

            //References(x => x.Tenant);

            Component(x => x.Address);

            Component(x => x.Contact);

            HasMany(x => x.Orders)
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}
