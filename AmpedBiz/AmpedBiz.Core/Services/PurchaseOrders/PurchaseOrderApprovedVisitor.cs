using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories.PurchaseOrders;
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
                item.Product.Inventory.Accept(new OrderVisitor() 
                {
                    Quantity = item.Quantity
                });
            }

            target.ApprovedBy = this.ApprovedBy;
            target.ApprovedOn = this.ApprovedOn;
            target.Status = PurchaseOrderStatus.Approved;
        }
    }
}
