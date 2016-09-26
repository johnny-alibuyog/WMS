using System;

namespace AmpedBiz.Core.Entities
{
    public class Branch : Entity<Guid, Branch>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Address Address { get; set; }

        public Branch() : base(default(Guid)) { }

        public Branch(Guid id) : base(id) { }
    }
}