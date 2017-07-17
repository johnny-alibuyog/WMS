using AmpedBiz.Data;

namespace AmpedBiz.Tests.Configurations
{
    internal class AuditProvider : IAuditProvider
    {
        public object GetCurrentUserId()
        {
            return "Test User";
        }
    }
}