using AmpedBiz.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Products
{
	public class Supplier : Entity<Guid, Supplier>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string ContactPerson { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual IEnumerable<Product> Products { get; protected set; }

        public Supplier() : base(default(Guid)) { }

        public Supplier(Guid id) : base(id)
        {
            this.Products = new Collection<Product>();
        }
    }
}