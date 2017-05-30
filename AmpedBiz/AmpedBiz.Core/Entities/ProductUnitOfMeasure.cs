using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    // https://docs.oracle.com/cd/E39583_01/fscm92pbr0/eng/fscm/fsit/task_UsingItemQuantityUOM-9f1965.html
    /*
        ProductUnitOfMeasure[]
	        UnitOfMeasure
            StandardEquivalent
            IsStandard
	        IsDefault
	        ProductPricingItems[]
		        Pricing
		        Amount
     */

    public class ProductUnitOfMeasure : Entity<Guid, ProductUnitOfMeasure>
    {
        public virtual Product Product { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual UnitOfMeasure StandardEquivalent { get; set; }

        public virtual bool IsStandard { get; set; }

        public virtual bool IsDefault { get; set; }

        public virtual IEnumerable<ProductUnitOfMeasurePrice> Prices { get; set; } = new Collection<ProductUnitOfMeasurePrice>();

        public ProductUnitOfMeasure() : base(default(Guid)) { }

        public ProductUnitOfMeasure(Product product, Guid id = default(Guid)) : base(id)
        {
            this.Product = product;
        }
    }
}
