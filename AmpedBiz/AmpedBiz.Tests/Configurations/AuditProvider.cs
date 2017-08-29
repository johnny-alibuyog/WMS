using AmpedBiz.Data;
using System;

namespace AmpedBiz.Tests.Configurations
{
    internal class AuditProvider : IAuditProvider
    {
        public object GetCurrentUserId()
        {
            return Guid.NewGuid();
        }
    }
}