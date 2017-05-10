using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderItem : Entity<Guid, OrderItem>
    {
        public virtual Order Order { get; protected internal set; }

        public virtual Product Product { get; protected set; }

        public virtual decimal PackagingSize { get; protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual decimal DiscountRate { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money ExtendedPrice { get; protected set; }

        public virtual Money TotalPrice { get; protected set; }

        public OrderItem() : base(default(Guid)) { }

        public OrderItem(Product product, decimal packagingSize, Measure quantity, decimal discountRate, Money unitPrice, Guid? id = null) 
            : base(id ?? default(Guid))
        {
            this.Product = product;
            this.PackagingSize = packagingSize;
            this.Quantity = quantity;
            this.DiscountRate = discountRate;
            this.UnitPrice = unitPrice;
            
            // discount is not included in the extended price
            this.ExtendedPrice = new Money((this.Quantity.Value * this.UnitPrice.Amount), this.UnitPrice.Currency);
            this.Discount = new Money((this.ExtendedPrice.Amount * this.DiscountRate), this.UnitPrice.Currency);
            this.TotalPrice = this.ExtendedPrice - this.Discount;
        }

        //protected internal virtual void Invoiced()
        //{
        //    this.Product.Inventory.Allocate(this.Quantity);
        //}

        //protected internal virtual void Shipped()
        //{
        //    this.Product.Inventory.Ship(this.Quantity);
        //}

        //protected internal virtual void BackOrdered()
        //{
        //    this.Product.Inventory.BackOrder(this.Quantity);
        //}
    }
}