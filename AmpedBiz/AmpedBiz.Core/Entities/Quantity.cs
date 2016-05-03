using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Quantity : Entity<Guid, Quantity>
    {
        public virtual decimal Value { get; set; }

        public virtual QuantityUnit Unit { get; set; }

        public Quantity() : this(default(Guid)) { }

        public Quantity(Guid id) : base(id) { }
    }
}
