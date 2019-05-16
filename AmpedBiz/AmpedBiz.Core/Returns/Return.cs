using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Core.Returns
{
    public class Return : Entity<Guid, Return>, IAccept<IVisitor<Return>>
    {
        public virtual Branch Branch { get; internal protected set; }

        public virtual Customer Customer { get; internal protected set; }

        public virtual User ReturnedBy { get; internal protected set; }

        public virtual DateTime? ReturnedOn { get; internal protected set; }

        public virtual string Remarks { get; internal protected set; }

        public virtual Money TotalReturned { get; internal protected set; }

        public virtual IEnumerable<ReturnItem> Items { get; internal protected set; } = new Collection<ReturnItem>();

        public Return() : base(default(Guid)) { }

        public Return(Guid id) : base(id) { }

        public virtual void Accept(IVisitor<Return> visitor)
        {
            visitor.Visit(this);
        }
    }
}
