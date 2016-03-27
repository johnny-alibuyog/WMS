using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderDetailsStatus : Entity<OrderDetailsStatus, Guid>
    {
        public OrderDetailsStatus()
        {
        }

        public virtual string StatusText { get; set; }
    }
}