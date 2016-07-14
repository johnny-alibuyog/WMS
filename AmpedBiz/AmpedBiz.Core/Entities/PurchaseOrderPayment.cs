using System;

namespace AmpedBiz.Core.Entities
{
    public class PurchaseOrderPayment : Entity<Guid, PurchaseOrderPayment>
    {
        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public virtual User PaidBy { get; protected set; }

        public virtual DateTime PaidOn { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public PurchaseOrderPayment() : this(default(Guid)) { }

        public PurchaseOrderPayment(Guid id) : base(id) { }

        public PurchaseOrderPayment(Guid? id = null, User paidBy = null, DateTime? paidOn = null, Money payment = null, PaymentType paymentType = null) : this(default(Guid))
        {
            this.Id = id ?? this.Id;
            this.PaidBy = paidBy ?? this.PaidBy;
            this.PaidOn = paidOn ?? this.PaidOn;
            this.Payment = payment ?? this.Payment;
            this.PaymentType = paymentType ?? this.PaymentType;
        }
    }
}
