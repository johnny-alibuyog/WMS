using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class PurchaseOrderPayment
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Lookup<Guid> PaidBy { get; set; }

        public DateTime? PaidOn { get; set; }

        public decimal PaymentAmount { get; set; }

        public Lookup<string> PaymentType { get; set; }
    }
}
