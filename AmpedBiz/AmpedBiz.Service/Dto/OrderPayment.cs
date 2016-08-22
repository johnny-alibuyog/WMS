using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class OrderPayment
    {
        public Guid OrderId { get; set; }

        public virtual DateTime? PaidOn { get; set; }

        public virtual Lookup<Guid> PaidBy { get; set; }

        public virtual Lookup<string> PaymentType { get; set; }

        public virtual decimal PaymentAmount { get; set; }
    }

    public class OrderPayable
    {
        public Guid OrderId { get; set; }

        public virtual DateTime? PaidOn { get; set; }

        public virtual Lookup<Guid> PaidBy { get; set; }

        public virtual Lookup<string> PaymentType { get; set; }

        public virtual decimal TaxAmount { get; set; }

        public virtual decimal ShippingFeeAmount { get; set; }

        public virtual decimal DiscountAmount { get; set; }

        public virtual decimal SubTotalAmount { get; set; }

        public virtual decimal TotalAmount { get; set; }

        public virtual decimal BalanceAmount { get { return this.TotalAmount - this.PaidAmount; } }

        public virtual decimal PaidAmount { get; set; }

        public virtual decimal PaymentAmount { get; set; }
    }
}
