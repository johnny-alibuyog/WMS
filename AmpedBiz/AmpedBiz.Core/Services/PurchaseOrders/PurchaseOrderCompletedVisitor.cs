using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCompletedVisitor : PurchaseOrderVisitor
    {
        public virtual User CompletedBy { get; set; }

        public virtual DateTime? CompletedOn { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            var calculator = new PurchaseOrderCalculator();

            foreach (var item in target.Items)
            {
                item.Product.Inventory.Accept(new RetractOrderedVisitor()
                {
                    Remaining = calculator.Remaining(
                        product: item.Product,
                        items: target.Items,
                        receipts: target.Receipts
                    )
                });
            }

            target.CompletedBy = this.CompletedBy;
            target.CompletedOn = this.CompletedOn;
            target.Status = PurchaseOrderStatus.Completed;
        }
    }
}
