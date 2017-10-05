using AmpedBiz.Data.Inteceptors;
using NHibernate.Cfg;

namespace AmpedBiz.Data.Configurations
{
    public static class InterceptorConfiguration
    {
        public static void Configure(Configuration configuration)
        {
            if (SessionFactoryProvider.GetContext == null)
                return;

            configuration.SetInterceptor(new CompositeInterceptor(
                new AuditInterceptor(SessionFactoryProvider.GetContext),
                new TenancyInterceptor(SessionFactoryProvider.GetContext)
            ));
        }
    }
}
