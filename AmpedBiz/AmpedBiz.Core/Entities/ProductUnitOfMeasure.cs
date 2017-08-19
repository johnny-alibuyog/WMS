using AmpedBiz.Core.Services;
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

    public class ProductUnitOfMeasure : Entity<Guid, ProductUnitOfMeasure>, IAccept<IVisitor<ProductUnitOfMeasure>>
    {
        public virtual Product Product { get; protected internal set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; protected internal set; }

        public virtual string Size { get; protected internal set; }

        public virtual bool IsStandard { get; protected internal set; }

        public virtual bool IsDefault { get; protected internal set; }

        public virtual decimal StandardEquivalentValue { get; protected internal set; }

        public virtual IEnumerable<ProductUnitOfMeasurePrice> Prices { get; protected internal set; } = new Collection<ProductUnitOfMeasurePrice>();

        public virtual Measure GetStandardMeasure() => new Measure(this.StandardEquivalentValue, this.UnitOfMeasure);

        public ProductUnitOfMeasure() : base(default(Guid)) { }

        public ProductUnitOfMeasure(
            UnitOfMeasure unitOfMeasure,
            string size,
            bool isDefault,
            bool isStandard,
            decimal standardEquivalentValue,
            IEnumerable<ProductUnitOfMeasurePrice> prices,
            Guid id = default(Guid)
        ) : base(id)
        {
            this.UnitOfMeasure = unitOfMeasure;
            this.Size = size;
            this.IsDefault = isDefault;
            this.IsStandard = isStandard;
            this.StandardEquivalentValue = standardEquivalentValue;
            this.Prices = prices;
        }

        public virtual void Accept(IVisitor<ProductUnitOfMeasure> visitor)
        {
            visitor.Visit(this);
        }
    }
}
