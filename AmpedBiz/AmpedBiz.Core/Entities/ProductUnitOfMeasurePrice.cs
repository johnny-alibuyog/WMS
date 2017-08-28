using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Entities
{
    public class ProductUnitOfMeasurePrice : Entity<Guid, ProductUnitOfMeasurePrice>
    {
        public virtual ProductUnitOfMeasure ProductUnitOfMeasure { get; protected internal set; }

        public virtual Pricing Pricing { get; protected set; }

        public virtual Money Price { get; protected set; }

        public ProductUnitOfMeasurePrice() : base(default(Guid)) { }

        public ProductUnitOfMeasurePrice(Pricing pricing, Money price, Guid? id = null) : base(id ?? default(Guid))
        {
            this.Pricing = pricing;
            this.Price = price;
        }
    }

    public static class ProductUnitOfMeasurePriceExtention
    {
        public static Money Base(this IEnumerable<ProductUnitOfMeasurePrice> prices) => prices.FirstOrDefault(x => x.Pricing == Pricing.BasePrice)?.Price;
        public static Money BadStock(this IEnumerable<ProductUnitOfMeasurePrice> prices) => prices.FirstOrDefault(x => x.Pricing == Pricing.BadStockPrice)?.Price;
        public static Money Wholesale(this IEnumerable<ProductUnitOfMeasurePrice> prices) => prices.FirstOrDefault(x => x.Pricing == Pricing.WholesalePrice)?.Price;
        public static Money Retail(this IEnumerable<ProductUnitOfMeasurePrice> prices) => prices.FirstOrDefault(x => x.Pricing == Pricing.RetailPrice)?.Price;
        public static Money SuggestedRetail(this IEnumerable<ProductUnitOfMeasurePrice> prices) => prices.FirstOrDefault(x => x.Pricing == Pricing.SuggestedRetailPrice)?.Price;
    }
}
