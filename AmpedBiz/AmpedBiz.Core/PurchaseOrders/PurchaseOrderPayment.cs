using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.PurchaseOrders
{
    public class PurchaseOrderPayment : TransactionPaymentBase
    {
        public virtual int Sequence { get; protected set; }

        public virtual PurchaseOrder PurchaseOrder { get; set; }

        public PurchaseOrderPayment() : base(default(Guid)) { }

        public PurchaseOrderPayment(
            int sequence,
            User paymentBy, 
            DateTime? paymentOn, 
            Money payment, 
            PaymentType paymentType, 
            Guid? id = null
        ) : base(id ?? default(Guid))
        {
            this.Sequence = sequence;
            this.PaymentBy = paymentBy;
            this.PaymentOn = paymentOn;
            this.Payment = payment;
            this.PaymentType = paymentType;
        }
    }
}
