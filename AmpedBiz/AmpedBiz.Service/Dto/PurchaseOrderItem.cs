using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class PurchaseOrderItem
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitCostAmount { get; set; }

        public decimal TotalCostAmount { get; set; }

        public DateTime? ReceivedOn { get; set; }
    }
}
