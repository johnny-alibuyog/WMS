using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Employee : Entity<Employee, Guid>
    {
        public Employee()
        {
            this.Orders = new HashSet<Order>();
            this.PurchaseOrders = new HashSet<PurchaseOrder>();
        }

        public virtual User User { get; set; }

        public virtual Tenant Tenant { get; set; }

        public virtual Person Person { get; set; }

        public virtual Address Address { get; set; }

        public virtual Contact Contact { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual EmployeeType EmployeeType { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }

        public virtual IEnumerable<PurchaseOrder> PurchaseOrders { get; set; }
    }
}