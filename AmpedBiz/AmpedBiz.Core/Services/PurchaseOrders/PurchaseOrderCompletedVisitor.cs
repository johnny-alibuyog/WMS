using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCompletedVisitor : IVisitor<PurchaseOrder>
    {
        public virtual User CompletedBy { get; set; }

        public virtual DateTime? CompletedOn { get; set; }

        public virtual void Visit(PurchaseOrder target)
        {
            var calculator = new PurchaseOrderCalculator();

            foreach (var item in target.Items)
            {
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = null,
                    InventoryVisitor = new RetractOrderedVisitor()
                    {
                        Status = target.Status,
                        Remaining = calculator.Remaining(
                            product: item.Product,
                            items: target.Items,
                            receipts: target.Receipts
                        )
                    }
                });
            }

            target.CompletedBy = this.CompletedBy;
            target.CompletedOn = this.CompletedOn;
            target.Status = PurchaseOrderStatus.Completed;
        }
    }
}
