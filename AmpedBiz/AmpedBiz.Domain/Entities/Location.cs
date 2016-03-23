using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Domain.Entities
{
    public class Location : Entity<Location, Guid>
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }
    }
}
