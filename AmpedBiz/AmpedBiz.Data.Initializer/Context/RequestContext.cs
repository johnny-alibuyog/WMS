using AmpedBiz.Data.Context;
using System;

namespace AmpedBiz.Data.Initializer.Context
{
    public class RequestContext : IContext
    {
        public Guid UserId { get; private set; }

        public Guid BranchId { get; private set; }

        public string TenantId { get; private set; }

        public RequestContext(Guid userId, Guid branchId, string tenantId)
        {
            this.UserId = userId;
            this.BranchId = branchId;
            this.TenantId = tenantId;
        }
    }
}
