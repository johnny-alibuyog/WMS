using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Quantity : Entity<Quantity, Guid>
    {
        public virtual decimal Value { get; set; }

        public virtual QuantityUnit Unit { get; set; }
    }
}
