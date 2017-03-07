using AmpedBiz.Core.Entities;
using NHibernate.Caches.SysCache2;
using NHibernate.Cfg;

namespace AmpedBiz.Data.Configurations
{
    internal static class CacheConfiguration
    {
        private const string RegionName = "hourly";

        public static void Configure(Configuration config)
        {
            config
                .SetProperty(Environment.UseSecondLevelCache, "true")
                .SetProperty(Environment.UseQueryCache, "true")
                .Cache(c => c.Provider<SysCacheProvider>())
                .EntityCache<ReturnReason>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Currency>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<PaymentType>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Pricing>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<ProductCategory>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<User>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Role>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<UnitOfMeasure>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Supplier>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Product>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Shipper>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Customer>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                })
                .EntityCache<Branch>(x =>
                {
                    x.Strategy = EntityCacheUsage.ReadWrite;
                    x.RegionName = CacheConfiguration.RegionName;
                });
        }
    }
}
