using AmpedBiz.Data.Context;
using System;
using System.Web;

namespace AmpedBiz.Service.Host.Context
{
    public class ContextProvider : IContextProvider
    {
        public IContext Build(Func<TenantId> tenantId = null, Func<BranchId> branchId = null, Func<UserId> userId = null)
        {
            var tenant = tenantId?.Invoke() ?? this.ResolveTenantId();
            var branch = branchId?.Invoke() ?? this.ResolveBranchId();
            var user = userId?.Invoke() ?? this.ResolveUserId();

            return new RequestContext(
                userId: user?.Value ?? Guid.Empty,
                branchId: branch?.Value ?? Guid.Empty,
                tenantId: tenant.Value
            );
        }

        private T Get<T>(string key, Func<string, T> parse, T defaultValue = default(T))
        {
            if (HttpContext.Current?.Handler == null)
                return defaultValue;

            if (HttpContext.Current?.Request?.Headers == null)
                return defaultValue;

            var value = HttpContext.Current.Request.Headers[key];

            if (string.IsNullOrWhiteSpace(value))
                return defaultValue;

            return parse.Invoke(value);
        }

        private TenantId ResolveTenantId() => this.Get("TenantId", (value) => value, DefaultContext.Instance.TenantId);

        private BranchId ResolveBranchId() => this.Get("BranchId", Guid.Parse, DefaultContext.Instance.BranchId);

        private UserId ResolveUserId() => this.Get("UserId", Guid.Parse, DefaultContext.Instance.UserId);
    }
}