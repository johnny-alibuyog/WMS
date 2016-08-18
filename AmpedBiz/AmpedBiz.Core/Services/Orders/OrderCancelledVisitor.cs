using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderCancelledVisitor : OrderVisitor
    {
        public virtual DateTime? CancelledOn { get; set; }

        public virtual User CancelledBy { get; set; }

        public virtual string CancellationReason { get; set; }

        public override void Visit(Order target)
        {
            target.CancelledBy = this.CancelledBy ?? target.CancelledBy;
            target.CancelledOn = this.CancelledOn ?? target.CancelledOn;
            target.CancellationReason = this.CancellationReason ?? target.CancellationReason;
            target.Status = OrderStatus.Cancelled;
        }
    }
}
