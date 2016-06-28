using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ProductMapping : ClassMap<Product>
    {
        public ProductMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);

            Map(x => x.Description);

            Map(x => x.Image);

            Map(x => x.Discontinued);

            References(x => x.Category);

            References(x => x.Supplier);

            Component(x => x.BasePrice, MoneyMapping.Map("BasePrice_", nameof(Product)));

            Component(x => x.RetailPrice, MoneyMapping.Map("RetailPrice_", nameof(Product)));

            Component(x => x.WholeSalePrice, MoneyMapping.Map("WholeSalePrice_", nameof(Product)));

            HasOne(x => x.GoodStockInventory).Cascade.All();
        }
    }
}