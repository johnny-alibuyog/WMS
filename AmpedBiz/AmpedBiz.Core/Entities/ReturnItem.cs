using System;

namespace AmpedBiz.Core.Entities
{
    public class ReturnItem : Entity<Guid, ReturnItem>
    {
        public virtual Product Product { get; protected set; }

        public virtual Return Return { get; protected internal set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money TotalPrice { get; protected set; }

        public ReturnItem() : base(default(Guid)) { }

        public ReturnItem(Product product, Measure quantity, Money unitPrice, Guid? id = null)
            : base(id ?? default(Guid))
        {
            this.Product = product;
            this.Quantity = quantity;
            this.UnitPrice = unitPrice;
            this.TotalPrice = new Money((this.Quantity.Value * this.UnitPrice.Amount), this.UnitPrice.Currency);
        }
    }
}
