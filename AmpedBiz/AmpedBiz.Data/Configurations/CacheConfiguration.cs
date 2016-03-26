using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Cfg;
using NHibernate.Caches.SysCache2;

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
