using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using System;

namespace AmpedBiz.Core.PointOfSales
{
	public class PointOfSaleItem : Entity<Guid, PointOfSaleItem>, IAccept<IVisitor<PointOfSaleItem>>
	{
        public virtual int Sequence { get; protected set; }

        public virtual PointOfSale PointOfSale { get; protected internal set; }

		public virtual Product Product { get; protected set; }

		public virtual Measure Quantity { get; internal protected set; }

		public virtual Measure Standard { get; protected set; }

		public virtual Measure QuantityStandardEquivalent { get; protected set; }

		public virtual decimal DiscountRate { get; protected set; }

		public virtual Money Discount { get; protected set; }

		public virtual Money UnitPrice { get; protected set; }

		public virtual Money ExtendedPrice { get; protected set; }

		public virtual Money TotalPrice { get; protected set; }

		public PointOfSaleItem() : base(default(Guid)) { }

		public PointOfSaleItem(
            int sequence,
			Product product,
			Measure quantity,
			Measure standard,
			decimal discountRate,
			Money unitPrice,
			Guid? id = null
		) : base(id ?? default(Guid))
		{
            this.Sequence = sequence;
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

		public virtual void Accept(IVisitor<PointOfSaleItem> visitor)
		{
			visitor.Visit(this);
		}
	}
}
