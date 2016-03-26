using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductCategoryMapping : ClassMap<ProductCategory>
    {
        public ProductCategoryMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);
        }
    }
}
