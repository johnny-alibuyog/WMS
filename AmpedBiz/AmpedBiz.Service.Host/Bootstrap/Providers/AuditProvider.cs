using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using System;

namespace AmpedBiz.Service.Host.Bootstrap.Providers
{
    public class AuditProvider : IAuditProvider
    {
        private readonly IContext _context;

        public AuditProvider(IContext context)
        {
            this._context = context;
        }

        public object GetCurrentUserId()
        {
            return this._context.UserId;
        }
    }
}