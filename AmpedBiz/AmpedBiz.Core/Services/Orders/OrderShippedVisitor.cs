using AmpedBiz.Core.Entities;
using System;

namespace AmpedBiz.Core.Services.Orders
{
    public class OrderShippedVisitor : OrderVisitor
    {
        public virtual DateTime? ShippedOn { get; set; }

        public virtual User ShippedBy { get; set; }

        public override void Visit(Order target)
        {
            foreach (var item in target.Items)
            {
                item.Shiped();
            }

            target.ShippedOn = this.ShippedOn;
            target.ShippedBy = this.ShippedBy;
            target.Status = OrderStatus.Shipped;
        }
    }
}
