using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public enum PurchaseOrderDetailStatus
    {
        Submitted = 1,
        Posted
    }

    public class PurchaseOrderDetail
    {
        public Guid Id { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public string ProductId { get; set; }

        public decimal QuantityValue { get; set; }

        public decimal UnitPriceAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public PurchaseOrderDetailStatus Status { get; set; }
    }

    public class PurchaseOrderDetailPageItem
    {
        public string PurchaseOrderId { get; set; }

        public string Id { get; set; }

        public string ProductName { get; set; }

        public string QuantityValue { get; set; }

        public string UnitPriceAmount { get; set; }

        public string TotalAmount { get; set; }

        public string DateReceived { get; set; }

        public string StatusName { get; set; }
    }
}
