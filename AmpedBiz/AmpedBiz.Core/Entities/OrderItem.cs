using AmpedBiz.Core.Services;
using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderItem : Entity<Guid, OrderItem>, IAccept<IVisitor<OrderItem>>
	{
        public virtual Order Order { get; protected internal set; }

        public virtual Product Product { get; protected set; }

        public virtual Measure Quantity { get; internal protected set; }

        public virtual Measure Standard { get; protected set; }

        public virtual Measure QuantityStandardEquivalent { get; protected set; }

        public virtual decimal DiscountRate { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money UnitPrice { get; protected set; }

        public virtual Money ExtendedPrice { get; protected set; }

        public virtual Money TotalPrice { get; protected set; }

        public OrderItem() : base(default(Guid)) { }

        public OrderItem(
            Product product,
            Measure quantity,
            Measure standard,
            decimal discountRate,
            Money unitPrice,
            Guid? id = null
        ) : base(id ?? default(Guid))
		{
			this.Product = product;
			this.Quantity = quantity;
			this.Standard = standard;
			this.DiscountRate = discountRate;
			this.UnitPrice = unitPrice;
			this.Compute();
		}

		protected internal virtual void Compute()
		{
			// quantity convertion to standard uom
			this.QuantityStandardEquivalent = this.Standard * this.Quantity;

			// discount is not included in the extended price
			this.ExtendedPrice = new Money((this.Quantity.Value * this.UnitPrice.Amount), this.UnitPrice.Currency);

			this.Discount = new Money((this.ExtendedPrice.Amount * this.DiscountRate), this.UnitPrice.Currency);

			this.TotalPrice = this.ExtendedPrice - this.Discount;
		}

		public virtual void Accept(IVisitor<OrderItem> visitor)
		{
			visitor.Visit(this);
		}
	}
}