using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderItem : Entity<Guid, OrderItem>
    {
        public virtual Product Product { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money ExtendedPrice { get; protected set; }

        public OrderItem() : base(default(Guid)) { }

        public OrderItem(Product product, Measure quantity, Money discount, Money unitPrice, Guid? id = null) 
            : base(id ?? default(Guid))
        {
            this.Product = product;
            this.Quantity = quantity;
            this.Discount = discount ?? new Money(0.0M);
            this.UnitPrice = unitPrice;

            // discount is not included in the extended price
            this.ExtendedPrice = new Money((this.Quantity.Value * this.UnitPrice.Amount), this.UnitPrice.Currency); 

            //this.Allocate();
        }

        public virtual void Allocate()
        {
            this.ExtendedPrice = new Money((this.UnitPrice.Amount - this.Discount.Amount) * this.Quantity.Value);
            this.Product.Inventory.Allocated += this.Quantity;
        }

        public virtual void Invoice()
        {
            //this.Status = OrderItemStatus.Invoiced;

            this.Product.Inventory.OnOrder += this.Quantity;
        }

        public virtual void Ship()
        {
            //this.Status = OrderItemStatus.Shipped;

            this.Product.Inventory.Shipped += this.Quantity;
        }

        public virtual void BackOrder()
        {
            //this.Status = OrderItemStatus.BackOrdered;

            this.Product.Inventory.BackOrdered += this.Quantity;
        }
    }
}