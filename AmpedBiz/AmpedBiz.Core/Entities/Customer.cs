using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Customer : Entity<Customer, Guid>
    {
        public virtual string Name { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Address Address { get; set; }

        public virtual string Address2 { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }

        public virtual IEnumerable<Location> Locations { get; set; }

        public Customer()
        {
            this.Orders = new HashSet<Order>();
            this.Locations = new HashSet<Location>();
        }
    }
}