using System;
using AmpedBiz.Core.Services.OrderItems;

namespace AmpedBiz.Core.Entities
{
    public enum OrderItemStatus
    {
        Allocated,
        Invoiced,
        Shipped,
        BackOrdered
    }

    public class OrderItem : Entity<Guid, OrderItem>
    {
        public virtual Product Product { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money ExtendedPrice { get; protected set; }

        public virtual OrderItemStatus Status { get; protected set; }

        public virtual bool InsufficientInventory { get; protected set; }

        public virtual State CurrentState
        {
            get { return State.GetState(this); }
        }

        public OrderItem() : this(default(Guid)) { }

        public OrderItem(Guid id) : base(id) { }

        public OrderItem(Product product, Measure quantity, Money discount, Money unitPrice) : this()
        {
            this.Product = product;
            this.Quantity = quantity;
            this.Discount = discount ?? new Money(0.0M);
            this.UnitPrice = unitPrice;

            this.Allocate();
        }

        public virtual void Allocate()
        {
            this.Status = OrderItemStatus.Allocated;

            this.ExtendedPrice = new Money((this.UnitPrice.Amount - this.Discount.Amount) * this.Quantity.Value);
            this.Product.GoodStockInventory.Allocated += this.Quantity;
        }

        public virtual void Invoice()
        {
            this.Status = OrderItemStatus.Invoiced;

            this.Product.GoodStockInventory.OnOrder += this.Quantity;
        }

        public virtual void Ship()
        {
            this.Status = OrderItemStatus.Shipped;

            this.Product.GoodStockInventory.Shipped += this.Quantity;
        }

        public virtual void BackOrder()
        {
            this.Status = OrderItemStatus.BackOrdered;

            this.Product.GoodStockInventory.BackOrdered += this.Quantity;
        }
    }
}