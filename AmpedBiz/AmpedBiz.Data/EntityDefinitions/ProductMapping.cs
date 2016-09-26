using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductMapping : ClassMap<Product>
    {
        public ProductMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);

            Map(x => x.Description);

            Map(x => x.Image);

            Map(x => x.Discontinued);

            References(x => x.Category);

            References(x => x.Supplier);

            HasOne(x => x.Inventory)
                .Cascade.All()
                .Fetch.Join();
        }
    }
}