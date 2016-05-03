using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UnitOfMeasureClassMapping : ClassMap<UnitOfMeasureClass>
    {
        public UnitOfMeasureClassMapping()
        {
            Id(x => x.Id)
              .GeneratedBy.Assigned();

            Map(x => x.Name);

            HasMany(x => x.Units)
                .Cascade.AllDeleteOrphan()                
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsBag();
        }
    }
}
