using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Customer : Entity<Customer, Guid>
    {
        public virtual string Name { get; set; }

        public virtual IEnumerable<Location> Locations { get; set; }
    }
}
