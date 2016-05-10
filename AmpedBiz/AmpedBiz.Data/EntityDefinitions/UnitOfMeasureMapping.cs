using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class UnitOfMeasureMapping : ClassMap<UnitOfMeasure>
    {
        public UnitOfMeasureMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);

            Map(x => x.IsBaseUnit);

            Map(x => x.ConvertionFactor);

            References(x => x.UnitOfMeasureClass);
        }
    }
}
