using AmpedBiz.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Products
{
	// TODO: Pricing should be per branch

	public class ProductUnitOfMeasurePrice : Entity<Guid, ProductUnitOfMeasurePrice>
    {
        public virtual ProductUnitOfMeasure ProductUnitOfMeasure { get; protected internal set; }

        //public virtual Branch Branch { get; protected set; }

        public virtual Pricing Pricing { get; protected set; }

        public virtual Money Price { get; protected set; }

        public ProductUnitOfMeasurePrice() : base(default(Guid)) { }

        public ProductUnitOfMeasurePrice(
            //Branch branch, 
            Pricing pricing, 
            Money price, 
            Guid? id = null) : base(id ?? default(Guid))
        {
            //this.Branch = branch;
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
