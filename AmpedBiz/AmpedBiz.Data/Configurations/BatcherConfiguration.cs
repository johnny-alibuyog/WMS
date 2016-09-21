using AmpedBiz.Common.Configurations;
using NHibernate.Cfg;

namespace AmpedBiz.Data.Configurations
{
    // https://github.com/jraristi/NHibernate.PostgresBatcher
    internal static class BatcherConfiguration
    {
        public static void Configure(Configuration config)
        {
            config.DataBaseIntegration(db =>
            {
                db.BatchSize = DbConfig.Instance.BatchSize; //this batch size is an example, set as needed
                db.Batcher<PostgresBatcherFactory>();
            });
        }
    }
}
