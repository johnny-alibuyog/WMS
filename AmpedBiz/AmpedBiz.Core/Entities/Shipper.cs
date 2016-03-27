using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Shipper : Entity<Shipper, Guid>
    {
        public Shipper()
        {
            this.Orders = new HashSet<Order>();
        }

        public virtual string Name { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }
    }
}