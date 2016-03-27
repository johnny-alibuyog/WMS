using System;

namespace AmpedBiz.Core.Entities
{
    public class Branch : Entity<Branch, Guid>
    {
        public virtual string Description { get; set; }
    }
}