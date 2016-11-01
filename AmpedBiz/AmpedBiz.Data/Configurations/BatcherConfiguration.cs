using AmpedBiz.Common.Configurations;
using NHibernate.Cfg;

namespace AmpedBiz.Data.Configurations
{
    internal static class BatcherConfiguration
    {
        public static void Configure(Configuration config)
        {
            config.DataBaseIntegration(db => db.BatchSize = DatabaseConfig.Instance.BatchSize);
        }
    }
}
