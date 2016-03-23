using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Domain.Entities
{
    public class QuantityUnit : Entity<QuantityUnit, Guid>
    {
        public virtual string Name { get; set; }
    }
}
