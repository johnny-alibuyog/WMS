using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
using AmpedBiz.Core.Services.Products;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderApprovedVisitor : IVisitor<PurchaseOrder>
    {
        public virtual User ApprovedBy { get; set; }

        public virtual DateTime? ApprovedOn { get; set; }

        public virtual void Visit(PurchaseOrder target)
        {
            foreach (var item in target.Items)
            {
                item.Product.Accept(new SearchAndApplyVisitor()
                {
                    Branch = null,
                    InventoryVisitor = new OrderVisitor()
                    {
                        QuantityStandardEquivalent = item.QuantityStandardEquivalent
                    }
                });
            }

            target.ApprovedBy = this.ApprovedBy;
            target.ApprovedOn = this.ApprovedOn;
            target.Status = PurchaseOrderStatus.Approved;
        }
    }
}
