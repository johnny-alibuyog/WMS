using AmpedBiz.Data.Configurations;

namespace AmpedBiz.Service.Host.Providers
{
    public class AuditProvider : IAuditProvider
    {
        public object GetCurrentUserId()
        {
            return "admin1";
        }
    }
}