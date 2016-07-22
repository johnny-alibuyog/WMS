using System;

namespace AmpedBiz.Core.Entities
{
    public class Tenant : Entity<Guid, Tenant>
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public Tenant() : base(default(Guid)) { }

        public Tenant(Guid id) : base(id) { }
    }
}