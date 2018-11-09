using AmpedBiz.Core.Products;
using System;

namespace AmpedBiz.Core.Common
{
    public class Attachment : Entity<Guid, Attachment>
    {
        public virtual string Name { get; set; }

        public virtual string Type { get; set; }

        public virtual string Description { get; set; }

        public virtual Product Product { get; set; }

        public Attachment() : base(default(Guid)) { }

        public Attachment(Guid id) : base(id) { }
    }
}