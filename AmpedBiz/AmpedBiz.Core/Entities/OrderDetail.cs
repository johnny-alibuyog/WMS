using System;

namespace AmpedBiz.Core.Entities
{
    public enum OrderDetailStatus
    {
        Allocated,
        Invoiced,
        Shipped,
        BackOrdered
    }

    public class OrderDetail : Entity<Guid, OrderDetail>
    {
        public virtual Product Product { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money ExtendedPrice { get; protected set; }

        public virtual OrderDetailStatus Status { get; protected set; }

        public virtual bool InsufficientInventory { get; protected set; }

        public OrderDetail() : this(default(Guid)) { }

        public OrderDetail(Guid id) : base(id) { }

        public OrderDetail(Product product, Measure quantity, Money discount, Money unitPrice) : this()
        {
            this.Allocate(product, quantity, discount, unitPrice);
        }

        public virtual void Allocate(Product product, Measure quantity, Money discount, Money unitPrice)
        {
            this.Status = OrderDetailStatus.Allocated;
            this.Product = product;
            this.Quantity = quantity;
            this.Discount = discount;
            this.UnitPrice = unitPrice;
            this.ExtendedPrice = unitPrice - discount;

            this.Product.Inventory.Allocated += this.Quantity;
        }

        public virtual void Invoice()
        {
            this.Status = OrderDetailStatus.Invoiced;

            this.Product.Inventory.OnOrder += this.Quantity;
        }

        public virtual void Ship()
        {
            this.Status = OrderDetailStatus.Shipped;

            this.Product.Inventory.Shipped += this.Quantity;
        }

        public virtual void BackOrder()
        {
            this.Status = OrderDetailStatus.BackOrdered;

            this.Product.Inventory.BackOrdered += this.Quantity;
        }
    }
}