using System;

namespace AmpedBiz.Core.SharedKernel
{
    public class Event : Entity<Guid, Event>
    {
        public Event() : base(default(Guid)) { }

        public Event(Guid id) : base(id) { }
    }
}
