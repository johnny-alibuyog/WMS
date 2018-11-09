using AmpedBiz.Core.Orders;
using AmpedBiz.Core.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Common
{
	public class Customer : Entity<Guid, Customer>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string ContactPerson { get; set; }

        public virtual Pricing Pricing { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual Money CreditLimit { get; set; }

        public virtual Address OfficeAddress { get; set; }

        public virtual Address BillingAddress { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; } = new Collection<Order>();

        public virtual IEnumerable<Location> Locations { get; set; } = new Collection<Location>();

        public Customer() : base(default(Guid)) { }

        public Customer(Guid id) : base(id) { }
    }
}