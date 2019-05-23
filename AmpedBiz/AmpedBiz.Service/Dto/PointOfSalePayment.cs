using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
	public class PointOfSalePayment
	{
		public virtual Guid Id { get; set; }

		public virtual Guid PointOfSaleId { get; set; }

		public virtual DateTime? PaymentOn { get; set; }

		public virtual Lookup<Guid> PaymentBy { get; set; }

		public virtual Lookup<string> PaymentType { get; set; }

		public virtual decimal PaymentAmount { get; set; }

		public virtual decimal BalanceAmount { get; set; }
	}

	public class PointOfSalePayable
	{
		public virtual Guid Id { get; set; }

		public virtual DateTime? PaymentOn { get; set; }

		public virtual Lookup<Guid> PaymentBy { get; set; }

		public virtual Lookup<string> PaymentType { get; set; }

		public virtual decimal DiscountAmount { get; set; }

		public virtual decimal SubTotalAmount { get; set; }

		public virtual decimal TotalAmount { get; set; }

		public virtual decimal BalanceAmount { get; set; }

		public virtual decimal PaidAmount { get; set; }

		public virtual decimal PaymentAmount { get; set; }
	}
}