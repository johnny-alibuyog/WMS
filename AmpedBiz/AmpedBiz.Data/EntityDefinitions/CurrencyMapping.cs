using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CurrencyMapping : ClassMap<Currency>
    {
        public CurrencyMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Symbol);

            Map(x => x.Name);
        }
    }
}
