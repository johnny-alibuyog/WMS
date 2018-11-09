using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AmpedBiz.Core.Products
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

        public virtual string Barcode { get; protected internal set; }

        public virtual bool IsStandard { get; protected internal set; }

        public virtual bool IsDefault { get; protected internal set; }

        public virtual decimal StandardEquivalentValue { get; protected internal set; }

        public virtual IEnumerable<ProductUnitOfMeasurePrice> Prices { get; protected internal set; } = new Collection<ProductUnitOfMeasurePrice>();

        public ProductUnitOfMeasure() : base(default(Guid)) { }

        public ProductUnitOfMeasure(
            UnitOfMeasure unitOfMeasure,
            string size,
            string barcode,
            bool isDefault,
            bool isStandard,
            decimal standardEquivalentValue,
            IEnumerable<ProductUnitOfMeasurePrice> prices,
            Guid id = default(Guid)
        ) : base(id)
        {
            this.UnitOfMeasure = unitOfMeasure;
            this.Size = size;
            this.Barcode = barcode;
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


    public static class ProductUnitOfMeasureExtention
    {
        public static T Standard<T>(this IEnumerable<ProductUnitOfMeasure> productUnitOfMeasures, Func<ProductUnitOfMeasure, T> selector)
        {
            var instance = productUnitOfMeasures.FirstOrDefault(x => x.IsStandard);
            if (instance == null)
            {
                return default(T);
            }

            return selector(instance);
        }

        public static T Default<T>(this IEnumerable<ProductUnitOfMeasure> productUnitOfMeasures, Func<ProductUnitOfMeasure, T> selector)
        {
            var instance = productUnitOfMeasures.FirstOrDefault(x => x.IsDefault);
            if (instance == null)
            {
                return default(T);
            }

            return selector(instance);
        }
    }
}
