using AmpedBiz.Data;

namespace AmpedBiz.Service.Test.Integration.Configurations
{
    internal class AuditProvider : IAuditProvider
    {
        public object GetCurrentUserId()
        {
            return "Test User";
        }
    }
}