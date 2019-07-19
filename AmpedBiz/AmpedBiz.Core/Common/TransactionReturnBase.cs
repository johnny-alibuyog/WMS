using AmpedBiz.Core.Products;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Common
{
    public abstract class TransactionReturnBase : Entity<Guid, TransactionReturnBase>
    {
        public virtual Product Product { get; protected set; }

        public virtual ReturnReason Reason { get; internal protected set; }

        public virtual Measure Quantity { get; protected set; }

        public virtual Measure Standard { get; protected set; }

        public virtual Measure QuantityStandardEquivalent { get; protected set; }

        public virtual Money Returned { get; protected set; }

        public TransactionReturnBase() : base(default(Guid)) { }

        public TransactionReturnBase(Guid id) : base(id) { }
    }
}
