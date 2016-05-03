using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Location : Entity<Guid, Location>
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }

        public Location() : this(default(Guid)) { }

        public Location(Guid id) : base(id) { }
    }
}
