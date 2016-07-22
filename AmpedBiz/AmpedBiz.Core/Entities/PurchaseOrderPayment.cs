using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderPayment : Entity<Guid, PurchaseOrderPayment>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual User PaidBy { get; protected set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public PurchaseOrderPayment() : base(default(Guid)) { }

        public PurchaseOrderPayment(User paidBy, DateTime? paidOn, Money payment, PaymentType paymentType, Guid? id = null) 
            : base(id ?? default(Guid))
        {
            this.PaidBy = paidBy;
            this.PaidOn = paidOn;
            this.Payment = payment;
            this.PaymentType = paymentType;
        }
    }
}
