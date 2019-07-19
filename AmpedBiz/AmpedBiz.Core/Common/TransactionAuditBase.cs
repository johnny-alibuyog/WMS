using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Common
{
    public abstract class TransactionAuditBase : Entity<Guid, TransactionAuditBase>
    {
        public virtual User TransactedBy { get; protected internal set; }

        public virtual DateTime TransactedOn { get; protected internal set; }

        public TransactionAuditBase() : base(default(Guid)) { }

        public TransactionAuditBase(Guid id) : base(id) { }
    }
}
