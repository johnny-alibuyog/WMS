using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderStagedVisitor : IVisitor<Order>
    {
        public virtual DateTime? StagedOn { get; set; }

        public virtual User StagedBy { get; set; }

        public virtual void Visit(Order target)
        {
            target.StagedBy = this.StagedBy ?? target.StagedBy;
            target.StagedOn = this.StagedOn ?? target.StagedOn;
            target.Status = OrderStatus.Staged;
        }
    }
}
