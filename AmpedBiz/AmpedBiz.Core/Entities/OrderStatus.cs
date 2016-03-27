using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderStatus : Entity<OrderStatus, Guid>
    {
        public OrderStatus()
        {
        }

        public virtual string StatusText { get; set; }
    }
}