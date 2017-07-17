using System;

namespace AmpedBiz.Core.Entities
{
    public class ProductUnitOfMeasurePrice : Entity<Guid, ProductUnitOfMeasurePrice>
    {
        public virtual ProductUnitOfMeasure ProductUnitOfMeasure { get; protected internal set; }

        public virtual Pricing Pricing { get; protected set; }

        public virtual Money Amount { get; protected set; }

        public ProductUnitOfMeasurePrice() : base(default(Guid)) { }

        public ProductUnitOfMeasurePrice(Pricing pricing, Money amount, Guid? id = null) : base(id ?? default(Guid))
        {
            this.Pricing = pricing;
            this.Amount = amount;
        }
    }
}
