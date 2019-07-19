using AmpedBiz.Core.SharedKernel;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Common
{
    public abstract class TransactionPaymentBase : Entity<Guid, TransactionPaymentBase>
    {
        public virtual User PaymentBy { get; protected set; }

        public virtual DateTime? PaymentOn { get; protected set; }

        public virtual PaymentType PaymentType { get; protected set; }

        public virtual Money Payment { get; protected set; }

        public virtual Money Balance { get; protected internal set; }

        public TransactionPaymentBase() : base(default(Guid)) { }

        public TransactionPaymentBase(Guid id) : base(id) { }
    }
}
