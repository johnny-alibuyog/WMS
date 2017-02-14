using AmpedBiz.Common.CustomTypes;
using System;
using System.Linq;

namespace AmpedBiz.Service.Dto
{
    public class PurchaseOrderReceipt
    {
        public Guid Id { get; set; }
        
        public Guid PurchaseOrderId { get; set; }

        public string BatchNumber { get; set; }

        public Lookup<Guid> ReceivedBy { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public Lookup<Guid> Product { get; set; }

        public decimal QuantityValue { get; set; }
    }

    public class PurchaseOrderReceivable
    {
        public Guid PurchaseOrderId { get; set; }

        public Lookup<Guid> Product { get; set; }

        public DateTime? ExpiresOn { get; set; }

        public decimal OrderedQuantity { get; set; }

        public decimal ReceivedQuantity { get; set; }

        public decimal ReceivableQuantity { get; set; }

        public decimal ReceivingQuantity { get; set; }

        public DateTime? ReceivingDoneOn { get; set; }

        public Lookup<Guid> ReceivingDoneBy { get; set; }

        public string ReceivingBatchNumber { get; set; }

        public static PurchaseOrderReceivable[] Evaluate(Core.Entities.PurchaseOrder purchaseOrder)
        {
            return purchaseOrder.Items
                .GroupJoin(purchaseOrder.Receipts,
                    (x) => x.Product,
                    (x) => x.Product,
                    (item, receipts) => new Dto.PurchaseOrderReceivable()
                    {
                        PurchaseOrderId = purchaseOrder.Id,
                        Product = new Lookup<Guid>(
                            item.Product.Id,
                            item.Product.Name
                        ),
                        OrderedQuantity = item.Quantity.Value,
                        ReceivedQuantity = receipts.Sum(x => x.Quantity.Value),
                        ReceivableQuantity = item.Quantity.Value - receipts.Sum(x => x.Quantity.Value),
                        ReceivingQuantity = item.Quantity.Value - receipts.Sum(x => x.Quantity.Value),
                    }
                )
                .ToArray();
        }
    }
}
