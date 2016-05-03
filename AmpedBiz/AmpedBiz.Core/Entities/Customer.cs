using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Customer : Entity<string, Customer>
    {
        public virtual string Name { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Address OfficeAddress { get; set; }

        public virtual Address BillingAddress { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }

        public virtual IEnumerable<Location> Locations { get; set; }

        public Customer() : this(default(string)) { }

        public Customer(string id) : base(id)
        {
            this.Orders = new Collection<Order>();
            this.Locations = new Collection<Location>();
        }
    }
}