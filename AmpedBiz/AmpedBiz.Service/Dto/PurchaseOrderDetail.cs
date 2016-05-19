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
        public string Id { get; set; }
        public string ProductId { get; set; }

        public decimal Quantity { get; set; }

        public decimal UnitCostAmount { get; set; }

        public decimal ExtendedPriceAmount { get; set; }

        public DateTime? DateReceived { get; protected set; }

        public PurchaseOrderDetailStatus Status { get; set; }
    }

    public class PurchaseOrderDetailPageItem
    {
        public string Id { get; set; }
        public string ProductName { get; set; }

        public string Quantity { get; set; }

        public string UnitCostAmount { get; set; }

        public string ExtendedPriceAmount { get; set; }

        public string DateReceived { get; protected set; }

        public string StatusName { get; set; }
    }
}
