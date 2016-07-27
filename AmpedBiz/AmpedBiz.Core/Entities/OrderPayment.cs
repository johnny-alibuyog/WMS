using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderPayment : Entity<Guid, OrderPayment>
    {
        public virtual Order Order { get; protected internal set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual User PaidBy { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public OrderPayment() : base(default(Guid)) { }

        public OrderPayment(DateTime? paidOn, User paidBy, PaymentType paymentType, 
            Money payment, Guid? id = null) : base(id ?? default(Guid))
        {
            this.PaidOn = paidOn;
            this.PaidBy = paidBy;
            this.PaymentType = paymentType;
            this.Payment = payment;
        }
    }
}
