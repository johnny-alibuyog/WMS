using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderCompletedVisitor : OrderVisitor
    {
        public virtual DateTime? CompletedOn { get; set; }

        public virtual User CompletedBy { get; set; }

        public override void Visit(Order target)
        {
            target.CompletedBy = this.CompletedBy;
            target.CompletedOn = this.CompletedOn;
            target.Status = OrderStatus.Completed;
        }
    }
}
