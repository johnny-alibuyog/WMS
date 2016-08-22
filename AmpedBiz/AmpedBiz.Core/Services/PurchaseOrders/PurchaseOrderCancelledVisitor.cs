using AmpedBiz.Core.Entities;
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
            target.CancelledBy = this.CancelledBy;
            target.CancelledOn = this.CancelledOn;
            target.CancellationReason = this.CancellationReason;
            target.Status = PurchaseOrderStatus.Cancelled;

            // TODO: Inventory adjustment if necessary
        }
    }
}
