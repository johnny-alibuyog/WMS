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

        protected internal virtual PurchaseOrderItem Submit()
        {
            //this.Status = PurchaseOrderItemStatus.Submitted;

            return this;
        }

        protected internal virtual PurchaseOrderItem Cancel()
        {
            //this.Status = PurchaseOrderItemStatus.Cancelled;

            //TODO: should deduct to inventory if necessary

            return this;
        }

        protected internal virtual PurchaseOrderItem Post()
        {
            //this.DateReceived = DateTime.Now;
            //this.Status = PurchaseOrderItemStatus.Posted;

            //TODO: should add to inventory
            return this;
        }
    }
}