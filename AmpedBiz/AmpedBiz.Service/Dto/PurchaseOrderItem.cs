using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public enum PurchaseOrderItemStatus
    {
        Submitted = 1,
        Posted
    }

    public class PurchaseOrderItem
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Lookup<string> Product { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public PurchaseOrderItemStatus Status { get; set; }
    }
}
