using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Customer : Entity<string, Customer>
    {
        public virtual string Name { get; set; }

        public virtual Money CreditLimit { get; set; }

        public virtual PricingScheme PricingScheme { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Address OfficeAddress { get; set; }

        public virtual Address BillingAddress { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; } = new Collection<Order>();

        public virtual IEnumerable<Location> Locations { get; set; } = new Collection<Location>();

        public Customer() : base(default(string)) { }

        public Customer(string id) : base(id) { }
    }
}