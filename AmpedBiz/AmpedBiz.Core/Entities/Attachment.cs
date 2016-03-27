using System;

namespace AmpedBiz.Core.Entities
{
    public class Attachment : Entity<Attachment, Guid>
    {
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual string Description { get; set; }

        public virtual Product Product { get; set; }
    }
}