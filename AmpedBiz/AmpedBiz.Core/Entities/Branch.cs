using System;

namespace AmpedBiz.Core.Entities
{
    public class Branch : Entity<string, Branch>
    {
        public virtual Tenant Tenant { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual Address Address { get; set; }

        public Branch() : this(default(string)) { }

        public Branch(string id) : base(id) { }
    }
}