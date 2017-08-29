using System;

namespace AmpedBiz.Data.Context
{
    public interface IContext
    {
        Guid UserId { get; }

        Guid BranchId { get; }

        string TenantId { get; }
    }
}
