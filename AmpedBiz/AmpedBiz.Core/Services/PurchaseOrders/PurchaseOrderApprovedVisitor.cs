using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderApprovedVisitor : PurchaseOrderVisitor
    {
        public virtual User ApprovedBy { get; set; }

        public virtual DateTime? ApprovedOn { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            foreach (var item in target.Items)
            {
                item.Approved();
            }

            target.ApprovedBy = this.ApprovedBy;
            target.ApprovedOn = this.ApprovedOn;
            target.Status = PurchaseOrderStatus.Approved;
        }
    }
}
