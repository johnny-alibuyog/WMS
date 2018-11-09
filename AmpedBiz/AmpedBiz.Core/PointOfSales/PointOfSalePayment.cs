using AmpedBiz.Core.Common;
using System;

namespace AmpedBiz.Core.PointOfSales
{
	public class PointOfSalePayment : Entity<Guid, PointOfSalePayment>
	{
		public virtual PointOfSale PointOfSale { get; protected internal set; }

		public virtual PaymentType PaymentType { get; protected set; }

		public virtual Money Payment { get; protected set; }

		public virtual Money Balance { get; protected internal set; }

		public PointOfSalePayment() : base(default(Guid)) { }

		public PointOfSalePayment(
			PaymentType paymentType,
			Money payment,
			Money balance = null,
			Guid? id = null
		) : base(id ?? default(Guid))
		{
			this.PaymentType = paymentType;
			this.Payment = payment;
			this.Balance = balance;
		}
	}
}
