using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PointOfSales
{
    public class PointOfSalePayment : TransactionPaymentBase
	{
        public virtual int Sequence { get; protected set; }

        public virtual PointOfSale PointOfSale { get; protected internal set; }

		public PointOfSalePayment() : base(default(Guid)) { }

		public PointOfSalePayment(
            int sequence,
            User paymentBy,
            DateTime? paymentOn,
			PaymentType paymentType,
			Money payment,
			Money balance = null,
			Guid? id = null
		) : base(id ?? default(Guid))
		{
            this.Sequence = sequence;
			this.PaymentOn = paymentOn;
			this.PaymentBy = paymentBy;
			this.PaymentType = paymentType;
			this.Payment = payment;
			this.Balance = balance;
		}
	}
}
