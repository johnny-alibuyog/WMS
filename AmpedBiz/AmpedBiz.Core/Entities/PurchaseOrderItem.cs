using AmpedBiz.Core.Services.PurchaseOrderItems;
using System;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderItemStatus
    {
        New,
        Submitted,
        Cancelled,
        Posted
    }

    public class PurchaseOrderItem : Entity<Guid, PurchaseOrderItem>
    {
        public virtual PurchaseOrder PurchaseOrder { get; protected internal set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual DateTime? DateReceived { get; protected set; }

        public virtual PurchaseOrderItemStatus Status { get; protected set; }

        public virtual State State
        {
            get { return State.GetState(this); }
        }

        public PurchaseOrderItem() : this(default(Guid)) { }

        public PurchaseOrderItem(Guid id) : base(id) { }

        protected internal virtual PurchaseOrderItem New(Product product, Money unitPrice, decimal quantity)
        {
            this.Product = product;
            this.Quantity = new Measure(quantity, product.UnitOfMeasure);
            this.UnitPrice = unitPrice;
            this.Total = new Money(
                amount: this.UnitPrice.Amount * this.Quantity.Value, 
                currency: this.UnitPrice.Currency
            );

            this.Status = PurchaseOrderItemStatus.New;

            return this;
        }

        protected internal virtual PurchaseOrderItem Submit()
        {
            this.Status = PurchaseOrderItemStatus.Submitted;

            return this;
        }

        protected internal virtual PurchaseOrderItem Cancel()
        {
            this.Status = PurchaseOrderItemStatus.Cancelled;

            //TODO: should deduct to inventory if necessary

            return this;
        }

        protected internal virtual PurchaseOrderItem Post()
        {
            this.DateReceived = DateTime.Now;
            this.Status = PurchaseOrderItemStatus.Posted;

            //TODO: should add to inventory
            return this;
        }
    }
}