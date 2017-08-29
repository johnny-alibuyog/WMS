using AmpedBiz.Data;
using System;

namespace AmpedBiz.Service.Host.Plugins.Providers
{
    public class AuditProvider : IAuditProvider
    {
        public object GetCurrentUserId()
        {
            return Guid.NewGuid();
        }
    }
}