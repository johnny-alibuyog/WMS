using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Orders
{
    public class OrderPayment : Entity<Guid, OrderPayment>
    {
        public virtual Order Order { get; protected internal set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual User PaidTo { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public virtual Money Balance { get; protected internal set; }

        public OrderPayment() : base(default(Guid)) { }

        public OrderPayment(
            DateTime? paidOn, User paidTo, 
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
