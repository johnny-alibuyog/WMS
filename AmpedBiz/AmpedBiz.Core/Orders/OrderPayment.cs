using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders
{
    public class OrderPayment : TransactionPaymentBase
    {
        public virtual int Sequence { get; protected set; }

        public virtual Order Order { get; protected internal set; }

        public virtual User PaidTo { get; protected set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public virtual Money Balance { get; protected internal set; }

        public OrderPayment() : base(default(Guid)) { }

        public OrderPayment(
            int sequence,
            User paidTo,
            DateTime? paidOn, 
            PaymentType paymentType, 
            Money payment,
            Money balance = null,
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Sequence = sequence;
            this.PaidTo = paidTo;
            this.PaidOn = paidOn;
            this.PaymentType = paymentType;
            this.Payment = payment;
            this.Balance = balance;
        }
    }
}
