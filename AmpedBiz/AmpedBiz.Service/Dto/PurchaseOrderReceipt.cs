using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class PurchaseOrderReceipt
    {
        public Guid PurchaseOrderId { get; set; }

        public string BatchNumber { get; set; }

        public Lookup<Guid> ReceivedBy { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public Lookup<string> Product { get; set; }

        public decimal QuantityValue { get; set; }
    }
}
