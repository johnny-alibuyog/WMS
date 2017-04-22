using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.PurchaseOrders
{
    public class PurchaseOrderCompletedVisitor : PurchaseOrderVisitor
    {
        public virtual User CompletedBy { get; set; }

        public virtual DateTime? CompletedOn { get; set; }

        public override void Visit(PurchaseOrder target)
        {
            target.CompletedBy = this.CompletedBy;
            target.CompletedOn = this.CompletedOn;
            target.Status = PurchaseOrderStatus.Completed;


        }
        
    }
}
