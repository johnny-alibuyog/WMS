using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Service.Dto
{
	public class PointOfSale
	{
		public Guid Id { get; set; }

		public string InvoiceNumber { get; set; }

		public Lookup<Guid> Branch { get; set; }

		public Lookup<Guid> Customer { get; set; }

		public Lookup<string> Pricing { get; set; }

		public Lookup<string> PaymentType { get; set; }

		public Lookup<Guid> TendredBy { get; set; }

		public DateTime TendredOn { get; set; }

		public decimal DiscountAmount { get; set; }

		public decimal SubTotalAmount { get; set; }

		public decimal TotalAmount { get; set; }

		public decimal PaidAmount { get; set; }

		public DateTime? CreatedOn { get; set; }

		public DateTime? ModifiedOn { get; set; }

		public Lookup<Guid> CreatedBy { get; set; }

		public Lookup<Guid> ModifiedBy { get; set; }

		public virtual ICollection<PointOfSaleItem> Items { get; set; } = new Collection<PointOfSaleItem>();

		public virtual ICollection<PointOfSalePayment> Payments { get; set; } = new Collection<PointOfSalePayment>();
	}

	public class PointOfSalePageItem
	{
		public Guid Id { get; set; }

		public string InvoiceNumber { get; set; }

		public string TenderedByName { get; set; }

		public DateTime? TenderedOn { get; set; }

		public string CustomerName { get; set; }

		public decimal? DiscountAmount { get; set; }

		public decimal? SubTotalAmount { get; set; }

		public decimal? TotalAmount { get; set; }
	}
}
