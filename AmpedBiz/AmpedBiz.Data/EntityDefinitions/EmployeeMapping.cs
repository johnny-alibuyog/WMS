using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class EmployeeMapping : SubclassMap<Employee>
    {
        public EmployeeMapping()
        {
            Component(x => x.Contact);

            References(x => x.EmployeeType);

            //DiscriminatorValue("Employee");

            //HasMany(x => x.Orders)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();

            //HasMany(x => x.PurchaseOrders)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();
        }
    }
}