using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnMapping : ClassMap<Return>
    {
        public ReturnMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Branch);

            References(x => x.Customer);

            References(x => x.ReturnedBy);

            Map(x => x.ReturnedOn);

            Map(x => x.Remarks);

            Component(x => x.Total, 
                MoneyMapping.Map("Total_", nameof(Return)));

            HasMany(x => x.Items)
                .Cascade.AllDeleteOrphan()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Inverse()
                .AsSet();
        }
    }
}
