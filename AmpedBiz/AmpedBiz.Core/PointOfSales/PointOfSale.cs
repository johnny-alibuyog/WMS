using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.PointOfSales
{
	public class PointOfSale : Entity<Guid, PointOfSale>, IAccept<IVisitor<PointOfSale>>, IAuditable
	{
		public virtual string InvoiceNumber { get; protected internal set; }

		public virtual Branch Branch { get; protected internal set; }

		public virtual Customer Customer { get; protected internal set; }

		public virtual Pricing Pricing { get; protected internal set; }

		public virtual PaymentType PaymentType { get; protected internal set; }

		public virtual User TendredBy { get; protected internal set; }

		public virtual DateTime TendredOn { get; protected internal set; }

		public virtual Money Discount { get; internal protected set; }

		public virtual Money SubTotal { get; internal protected set; }

		public virtual Money Total { get; internal protected set; }

		public virtual Money Paid { get; internal protected set; }

		public virtual DateTime? CreatedOn { get; set; }

		public virtual DateTime? ModifiedOn { get; set; }

		public virtual User CreatedBy { get; set; }

		public virtual User ModifiedBy { get; set; }

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
