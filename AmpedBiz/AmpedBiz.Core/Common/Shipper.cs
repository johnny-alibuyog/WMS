using AmpedBiz.Core.Orders;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Common
{
	public class Shipper : Entity<string, Shipper>, IHasTenant
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