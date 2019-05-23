using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class PurchaseOrderPayment
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Lookup<Guid> PaymentBy { get; set; }

        public DateTime? PaymentOn { get; set; }

        public decimal PaymentAmount { get; set; }

        public Lookup<string> PaymentType { get; set; }
    }

    public class PurchaseOrderPayable
    {
        public Guid PurchaseOrderId { get; set; }

        public DateTime? PaymentOn { get; set; }

        public Lookup<Guid> PaymentBy { get; set; }

        public Lookup<string> PaymentType { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal ShippingFeeAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal SubTotalAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal BalanceAmount { get; set; }

        public decimal PaidAmount { get; set; }

        public decimal PaymentAmount { get; set; }
    }
}
