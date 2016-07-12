using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Supplier : Entity<string, Supplier>
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual IEnumerable<Product> Products { get; protected set; }

        public Supplier() : this(default(string)) { }

        public Supplier(string id) : base(id)
        {
            this.Products = new Collection<Product>();
        }
    }
}