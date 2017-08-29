using System;

namespace AmpedBiz.Core.Entities
{
    public class ReturnItem : Entity<Guid, ReturnItem>
    {
        public virtual Product Product { get; protected set; }

        public virtual Return Return { get; protected internal set; }

        public virtual ReturnReason ReturnReason { get; internal protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Measure Standard { get; protected set; }

        public virtual Measure QuantityStandardEquivalent { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money TotalPrice { get; protected set; }

        public ReturnItem() : base(default(Guid)) { }

        public ReturnItem(
            Product product,
            ReturnReason returnReason,
            Measure quantity,
            Measure standard,
            Money unitPrice,
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Product = product;
            this.ReturnReason = returnReason;
            this.Quantity = quantity;
            this.Standard = standard;
            this.UnitPrice = unitPrice;

            // quantity convertion to standard uom
            this.QuantityStandardEquivalent = standard * quantity;

            this.TotalPrice = new Money((this.Quantity.Value * this.UnitPrice.Amount), this.UnitPrice.Currency);
        }
    }
}
