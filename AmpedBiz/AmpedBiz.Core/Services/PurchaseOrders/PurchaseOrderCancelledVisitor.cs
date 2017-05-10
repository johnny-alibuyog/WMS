using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCancelledVisitor : PurchaseOrderVisitor
    {
        public virtual User CancelledBy { get; set; }

        public virtual DateTime? CancelledOn { get; set; }

        public virtual string CancellationReason { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            var calculator = new PurchaseOrderCalculator();

            foreach (var item in target.Items)
            {
                item.Product.Inventory.Accept(new RetractOrderedVisitor()
                {
                    Status = target.Status,
                    Remaining = calculator.Remaining(
                        product: item.Product,
                        items: target.Items,
                        receipts: target.Receipts
                    )
                });
            }

            target.CancelledBy = this.CancelledBy;
            target.CancelledOn = this.CancelledOn;
            target.CancellationReason = this.CancellationReason;
            target.Status = PurchaseOrderStatus.Cancelled;
        }
    }
}
