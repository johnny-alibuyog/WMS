using System;

namespace AmpedBiz.Core.Entities
{
    public class Tenant : Entity<Tenant, Guid>
    {
        public virtual string Name { get; set; }

        public virtual string Code { get; set; }

        public virtual string Description { get; set; }

        public Tenant()
        {
        }
    }
}