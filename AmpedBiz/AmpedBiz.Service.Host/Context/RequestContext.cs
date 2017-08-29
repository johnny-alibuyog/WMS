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
            Console.WriteLine(HttpContext.Current);
        }
    }
}