using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;

namespace AmpedBiz.Data.Configurations
{
    internal static class CacheConfiguration
    {
        public static void Configure(this NHibernate.Cfg.Configuration config)
        {
            config
                .SetProperty(NHibernate.Cfg.Environment.UseSecondLevelCache, "true")
                .SetProperty(NHibernate.Cfg.Environment.UseQueryCache, "true")
                .Cache(c => c.Provider<SysCacheProvider>());
        }
    }
}
