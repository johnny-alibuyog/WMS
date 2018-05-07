using AmpedBiz.Data.Context;
using System;

namespace AmpedBiz.Tests.Context
{
    public class ContextProvider : IContextProvider
    {
        private TenantId _tenantId;

        private BranchId _branchId;

        private UserId _userId;

        public ContextProvider(TenantId tenantId = null, BranchId branchId = null, UserId userId = null)
        {
            this._tenantId = tenantId;
            this._branchId = branchId;
            this._userId = userId;
        }

        public IContext Build(Func<TenantId> tenantId = null, Func<BranchId> branchId = null, Func<UserId> userId = null)
        {
            this._tenantId = tenantId?.Invoke() ?? this._tenantId;
            this._branchId = branchId?.Invoke() ?? this._branchId;
            this._userId = userId?.Invoke() ?? this._userId;

            return new RequestContext(
                userId: this._userId?.Value ?? Guid.Empty,
                branchId: this._branchId?.Value ?? Guid.Empty,
                tenantId: this._tenantId?.Value
            );
        }
    }
}
