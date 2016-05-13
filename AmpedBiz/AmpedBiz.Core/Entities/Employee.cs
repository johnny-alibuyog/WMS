using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Employee : Entity<string, Employee>
    {
        public virtual User User { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual EmployeeType EmployeeType { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }

        public virtual IEnumerable<PurchaseOrder> PurchaseOrders { get; set; }

        public Employee() : this(default(string)) { }

        public Employee(string id) : base(id)
        {
            this.Orders = new Collection<Order>();
            this.PurchaseOrders = new Collection<PurchaseOrder>();
        }
    }
}