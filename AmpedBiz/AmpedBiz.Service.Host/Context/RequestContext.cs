using AmpedBiz.Data.Context;
using System;
using System.Web;

namespace AmpedBiz.Service.Host.Context
{
    public class RequestContext : IContext
    {
        public Guid BranchId { get; private set; }

        public string TenantId { get; private set; }

        public Guid UserId { get; private set; }

        public RequestContext()
        {
            if (HttpContext.Current?.Handler == null || HttpContext.Current?.Request?.Headers == null)
            {
                this.UserId = DefaultContext.Instance.UserId;
                this.BranchId = DefaultContext.Instance.BranchId;
                this.TenantId = DefaultContext.Instance.TenantId;
            }
            else // TODO: this info is critical. Should use JWT soon instead.
            {
                this.UserId = !string.IsNullOrWhiteSpace(HttpContext.Current.Request.Headers["UserId"])
                    ? Guid.Parse(HttpContext.Current.Request.Headers["UserId"]) : Guid.Empty;

                this.BranchId = !string.IsNullOrWhiteSpace(HttpContext.Current.Request.Headers["BranchId"])
                    ? Guid.Parse(HttpContext.Current.Request.Headers["BranchId"]) : Guid.Empty;

                this.TenantId = !string.IsNullOrWhiteSpace(HttpContext.Current.Request.Headers["TenantId"])
                    ? this.TenantId = HttpContext.Current.Request.Headers["TenantId"] : null;
            }
        }
    }
}