using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PointOfSales
{
	public class PointOfSalePayment : Entity<Guid, PointOfSalePayment>
	{
		public virtual PointOfSale PointOfSale { get; protected internal set; }

		public virtual DateTime? PaidOn { get; protected set; }

		public virtual User PaidTo { get; protected set; }

		public virtual PaymentType PaymentType { get; protected set; }

		public virtual Money Payment { get; protected set; }

		public virtual Money Balance { get; protected internal set; }

		public PointOfSalePayment() : base(default(Guid)) { }

		public PointOfSalePayment(
			DateTime? paidOn,
			User paidTo,
			PaymentType paymentType,
			Money payment,
			Money balance = null,
			Guid? id = null
		) : base(id ?? default(Guid))
		{
			this.PaidOn = paidOn;
			this.PaidTo = paidTo;
			this.PaymentType = paymentType;
			this.Payment = payment;
			this.Balance = balance;
		}
	}
}
