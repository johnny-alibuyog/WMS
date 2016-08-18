using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderStagedVisitor : OrderVisitor
    {
        public virtual DateTime? StagedOn { get; set; }

        public virtual User StagedBy { get; set; }

        public override void Visit(Order target)
        {
            target.StagedBy = this.StagedBy ?? target.StagedBy;
            target.StagedOn = this.StagedOn ?? target.StagedOn;
            target.Status = OrderStatus.Staged;
        }
    }
}
