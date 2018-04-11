using System;

namespace AmpedBiz.Data.Context
{
    public interface IContext
    {
        Guid UserId { get; }

        Guid BranchId { get; }

        string TenantId { get; }
    }

    public static class ContextExtention
    {
        public static bool HasUser(this IContext context) => context != null && context.UserId != Guid.Empty;

        public static bool HasBanch(this IContext context) => context != null && context.BranchId != Guid.Empty;

        public static bool HasTenant(this IContext context) => !string.IsNullOrWhiteSpace(context?.TenantId);
    }
}
