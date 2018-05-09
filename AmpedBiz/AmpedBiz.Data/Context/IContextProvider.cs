using System;

namespace AmpedBiz.Data.Context
{
    public interface IContextProvider
    {
        IContext Build(
            Func<TenantId> tenantId = null,
            Func<BranchId> branchId = null,
            Func<UserId> userId = null
        );
    }


    /// Reference: https://lucax88x.github.io/2016/11/21/2016-11-21-primitive-obsession/
    public class PrimitiveHolder<T>
    {
        public T Value { get; private set; }

        public PrimitiveHolder(T value) => this.Value = value;
    }

    public class TenantId : PrimitiveHolder<string>
    {
        public TenantId(string value) : base(value) { }

        public static implicit operator string(TenantId holder) => holder.Value;

        public static implicit operator TenantId(string value) => new TenantId(value);
    }

    public class UserId : PrimitiveHolder<Guid>
    {
        public UserId(Guid value) : base(value) { }

        public static implicit operator Guid(UserId holder) => holder.Value;

        public static implicit operator UserId(Guid value) => new UserId(value);
    }

    public class BranchId : PrimitiveHolder<Guid>
    {
        public BranchId(Guid value) : base(value) { }

        public static implicit operator Guid(BranchId holder) => holder.Value;

        public static implicit operator BranchId(Guid value) => new BranchId(value);
    }


}