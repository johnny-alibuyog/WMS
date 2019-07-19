using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Common
{
    public abstract class TransactionBase : Entity<Guid, TransactionBase>//, IHasBranch
    {
        //public Branch Branch { get; set; }

        public TransactionBase() : base(default(Guid)) { }

        public TransactionBase(Guid id) : base(id) { }
    }

    //public abstract class SalesTransactionBase : TransactionBase
    //{
    //    public SalesTransactionBase() : base(default(Guid)) { }

    //    public SalesTransactionBase(Guid id) : base(id) { }
    //}

    //public abstract class PurchaseTransactionBase : TransactionBase
    //{
    //    public PurchaseTransactionBase() : base(default(Guid)) { }

    //    public PurchaseTransactionBase(Guid id) : base(id) { }
    //}

    //public abstract class ReturnTransactionBase : TransactionBase
    //{
    //    public ReturnTransactionBase() : base(default(Guid)) { }

    //    public ReturnTransactionBase(Guid id) : base(id) { }
    //}
}
