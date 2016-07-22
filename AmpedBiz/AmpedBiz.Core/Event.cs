using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core
{
    public class Event : Entity<Guid, Event>
    {
        public Event() : base(default(Guid)) { }

        public Event(Guid id) : base(id) { }
    }
}
