using AmpedBiz.Core.Services.PurchaseOrderDetails;
using System;

namespace AmpedBiz.Core.Entities
{
    public enum PurchaseOrderDetailStatus
    {
        New,
        Submitted,
        Cancelled,
        Posted
    }

    public class PurchaseOrderDetail : Entity<Guid, PurchaseOrderDetail>
    {
        public virtual PurchaseOrder PurchaseOrder { get; protected internal set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money Total { get; protected set; }

        public virtual DateTime? DateReceived { get; protected set; }

        public virtual PurchaseOrderDetailStatus Status { get; protected set; }

        public virtual State CurrentState
        {
            get { return State.GetState(this); }
        }

        public PurchaseOrderDetail() : this(default(Guid)) { }

        public PurchaseOrderDetail(Guid id) : base(id) { }

        protected internal virtual void New(Product product, Money unitPrice, decimal quantity)
        {
            this.Product = product;
            this.Quantity = new Measure(quantity, product.UnitOfMeasure);
            this.UnitPrice = unitPrice;
            this.Total = new Money(
                amount: this.UnitPrice.Amount * this.Quantity.Value, 
                currency: this.UnitPrice.Currency
            );

            this.Status = PurchaseOrderDetailStatus.New;
        }

        protected internal virtual void Submit()
        {
            this.Status = PurchaseOrderDetailStatus.Submitted;
        }

        protected internal virtual void Cancel()
        {
            this.Status = PurchaseOrderDetailStatus.Cancelled;

            //TODO: should deduct to inventory if necessary
        }

        public virtual void Post()
        {
            this.DateReceived = DateTime.Now;
            this.Status = PurchaseOrderDetailStatus.Posted;

            //TODO: should add to inventory
        }
    }
}