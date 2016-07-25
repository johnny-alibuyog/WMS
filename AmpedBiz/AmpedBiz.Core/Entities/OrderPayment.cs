using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderPayment : Entity<Guid, OrderPayment>
    {
        public virtual Order Order { get; protected internal set; }

        public virtual DateTime? PaidOn { get; protected set; }

        public virtual User PaidBy { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Tax { get; protected set; }

        public virtual Money ShippingFee { get; protected set; }

        public virtual Money Discount { get; protected set; }

        public virtual Money SubTotal { get; protected set; }

        public virtual Money Total { get; protected set; }

        public OrderPayment() : base(default(Guid)) { }

        public OrderPayment(DateTime? paidOn, User paidBy, PaymentType paymentType, Money tax, 
            Money shippingFee, Money discount, Money subTotal, Guid? id = null) : base(id ?? default(Guid))
        {
            this.PaidOn = paidOn;
            this.PaidBy = paidBy;
            this.PaymentType = paymentType;
            this.Tax = tax;
            this.ShippingFee = shippingFee;
            this.Discount = discount;
            this.SubTotal = subTotal;
            this.Total = this.SubTotal + this.Tax + this.ShippingFee - this.Discount;
        }
    }
}
