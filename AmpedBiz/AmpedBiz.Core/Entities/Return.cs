using AmpedBiz.Core.Services;
using AmpedBiz.Core.Services.Returns;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Entities
{
    public class Return : Entity<Guid, Return>, IAccept<ReturnVisitor>
    {
        public virtual Branch Branch { get; internal protected set; }

        public virtual Customer Customer { get; internal protected set; }

        public virtual User ReturnedBy { get; internal protected set; }

        public virtual DateTime? ReturnedOn { get; internal protected set; }

        public virtual string Remarks { get; internal protected set; }

        public virtual Money Total { get; internal protected set; }

        public virtual IEnumerable<ReturnItem> Items { get; internal protected set; } = new Collection<ReturnItem>();

        public Return() : base(default(Guid)) { }

        public Return(Guid id) : base(id) { }

        public virtual void Accept(ReturnVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
