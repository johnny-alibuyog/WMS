using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderItem : Entity<Guid, PurchaseOrderItem>
    {
        public virtual PurchaseOrder PurchaseOrder { get; protected internal set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money UnitCost { get; protected set; }

        public virtual Money ExtendedCost { get; protected set; }

        public PurchaseOrderItem() : base(default(Guid)) { }

        public PurchaseOrderItem(Product product, Money unitCost, Measure quantity, Guid? id = null) : base(id ?? default(Guid))
        {
            this.Product = product;
            this.Quantity = quantity;
            this.UnitCost = unitCost;
            this.ExtendedCost = new Money(
                amount: this.UnitCost.Amount * this.Quantity.Value, 
                currency: this.UnitCost.Currency
            );
        }

        protected internal virtual void Approved()
        {
            this.Product.Inventory.Order(this.Quantity);
        }
    }
}