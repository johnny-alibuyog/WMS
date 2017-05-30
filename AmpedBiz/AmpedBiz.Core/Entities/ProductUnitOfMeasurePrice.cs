using System;

namespace AmpedBiz.Core.Entities
{
    public class ProductUnitOfMeasurePrice : Entity<Guid, ProductUnitOfMeasurePrice>
    {
        public virtual ProductUnitOfMeasure ProductUnitOfMeasure { get; set; }

        public virtual Pricing Pricing { get; set; }

        public virtual Money Amount { get; set; }

        public ProductUnitOfMeasurePrice() : base(default(Guid)) { }
    }
}
