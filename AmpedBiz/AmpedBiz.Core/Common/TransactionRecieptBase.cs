using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Common
{
    public abstract class TransactionRecieptBase : Entity<Guid, TransactionRecieptBase>
    {
        public TransactionRecieptBase() : base(default(Guid)) { }

        public TransactionRecieptBase(Guid id) : base(id) { }
    }
}
