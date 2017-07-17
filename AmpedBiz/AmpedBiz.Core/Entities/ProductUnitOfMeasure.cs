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
        public virtual Product Product { get; protected internal set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; protected internal set; }

        public virtual decimal StandardEquivalentValue { get; protected internal set; }

        public virtual bool IsStandard { get; protected internal set; }

        public virtual bool IsDefault { get; protected internal set; }

        public virtual IEnumerable<ProductUnitOfMeasurePrice> Prices { get; protected internal set; } = new Collection<ProductUnitOfMeasurePrice>();

        public virtual Measure GetStandardEquivalent() => new Measure(this.StandardEquivalentValue, this.UnitOfMeasure);

        public ProductUnitOfMeasure() : base(default(Guid)) { }

        public ProductUnitOfMeasure(
            bool isDefault,
            bool isStandard,
            UnitOfMeasure unitOfMeasure,
            decimal standardEquivalentValue,
            IEnumerable<ProductUnitOfMeasurePrice> prices,
            Guid id = default(Guid)
        ) : base(id)
        {
            this.UnitOfMeasure = unitOfMeasure;
            this.StandardEquivalentValue = standardEquivalentValue;
            this.IsStandard = isStandard;
            this.IsDefault = isDefault;
        }
    }
}
