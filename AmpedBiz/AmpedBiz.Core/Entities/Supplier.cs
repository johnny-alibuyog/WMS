using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Supplier : Entity<Supplier, Guid>
    {
        public Supplier()
        {
            this.Products = new HashSet<Product>();
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public virtual string Address2 { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }

        public virtual IEnumerable<PurchaseOrder> PurchaseOrders { get; set; }
    }
}