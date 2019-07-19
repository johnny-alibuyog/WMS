using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Common
{
    public abstract class TransactionItemBase : Entity<Guid, TransactionItemBase>
    {
        public TransactionItemBase() : base(default(Guid)) { }

        public TransactionItemBase(Guid id) : base(id) { }
    }
}
