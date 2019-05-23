using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders
{
    public class OrderPayment : TransactionPaymentBase
    {
        public virtual int Sequence { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public OrderPayment() : base(default(Guid)) { }

        public OrderPayment(
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
            this.PaymentBy = paymentBy;
            this.PaymentOn = paymentOn;
            this.PaymentType = paymentType;
            this.Payment = payment;
            this.Balance = balance;
        }
    }
}
