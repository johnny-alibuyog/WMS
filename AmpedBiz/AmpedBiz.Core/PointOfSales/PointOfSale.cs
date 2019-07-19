using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.PointOfSales
{
	public enum PointOfSaleStatus
	{
		UnPaid = 1,
		PartiallyPaid = 2,
		FullyPaid = 3,
	}

	public class PointOfSale : TransactionBase, IAccept<IVisitor<PointOfSale>>, IAuditable
	{
		public virtual string InvoiceNumber { get; protected internal set; }

		public virtual Branch Branch { get; protected internal set; }

		public virtual Customer Customer { get; protected internal set; }

		public virtual Pricing Pricing { get; protected internal set; }

		public virtual PaymentType PaymentType { get; protected internal set; }

		public virtual User TenderedBy { get; protected internal set; }

		public virtual DateTime TenderedOn { get; protected internal set; }

        public virtual DateTime? PaymentOn { get; protected internal set; }

        public virtual User PaymentBy { get; protected internal set; }

        public virtual decimal DiscountRate { get; protected internal set; }

		public virtual Money Discount { get; internal protected set; }

		public virtual Money SubTotal { get; internal protected set; }

		public virtual Money Total { get; internal protected set; }

		public virtual Money Received { get; internal protected set; }

		public virtual Money Change { get; internal protected set; }

		public virtual Money Paid { get; internal protected set; }

		public virtual Money Balance { get; internal protected set; }

		public virtual DateTime? CreatedOn { get; set; }

		public virtual DateTime? ModifiedOn { get; set; }

        public virtual User CreatedBy { get; set; }

		public virtual User ModifiedBy { get; set; }

        public virtual PointOfSaleStatus Status { get; internal protected set; } = PointOfSaleStatus.UnPaid;

		public virtual ICollection<PointOfSaleItem> Items { get; protected internal set; } = new Collection<PointOfSaleItem>();

		public virtual ICollection<PointOfSalePayment> Payments { get; protected internal set; } = new Collection<PointOfSalePayment>();

        public PointOfSale() : this(default(Guid)) { }

		public PointOfSale(Guid id) : base(id) { }

		public virtual void Accept(IVisitor<PointOfSale> visitor)
		{
			visitor.Visit(this);
		}
	}
}
