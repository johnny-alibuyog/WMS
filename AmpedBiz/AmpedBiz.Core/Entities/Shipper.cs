using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Shipper : Entity<string, Shipper>
    {
        public virtual string Name { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; } = new Collection<Order>();

        public Shipper() : base(default(string)) { }

        public Shipper(string id) : base(id) { }
    }
}