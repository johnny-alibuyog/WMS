using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class QuantityUnit : Entity<Guid, QuantityUnit>
    {
        public virtual string Name { get; set; }

        public QuantityUnit() : this(default(Guid)) { }

        public QuantityUnit(Guid id) : base(id) { }
    }
}
