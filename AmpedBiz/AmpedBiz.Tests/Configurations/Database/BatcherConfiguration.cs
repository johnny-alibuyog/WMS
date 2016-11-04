using AmpedBiz.Common.Configurations;
using NHibernate.AdoNet;
using NHibernate.Cfg;

namespace AmpedBiz.Service.Tests.Configurations.Database
{
    // https://github.com/jraristi/NHibernate.PostgresBatcher
    internal static class BatcherConfiguration
    {
        public static void Configure(Configuration config)
        {
            switch(DatabaseConfig.Instance.Database)
            {
                case DatabaseProvider.Postgres:
                    ConfigurePostgres(config);
                    break;

                case DatabaseProvider.MySql:
                    ConfigureMySql(config);
                    break;

                case DatabaseProvider.MsSql:
                    ConfigureMsSql(config);
                    break;

                default:
                    ConfigureMsSql(config);
                    break;
            }
        }

        private static void ConfigurePostgres(Configuration config)
        {
            config.DataBaseIntegration(db =>
            {
                db.BatchSize = DatabaseConfig.Instance.BatchSize;
                db.Batcher<PostgresBatcherFactory>();
            });
        }

        private static void ConfigureMySql(Configuration config)
        {
            config.DataBaseIntegration(db =>
            {
                db.BatchSize = DatabaseConfig.Instance.BatchSize;
                db.Batcher<MySqlClientBatchingBatcherFactory>();
            });
        }

        private static void ConfigureMsSql(Configuration config)
        {
            config.DataBaseIntegration(db =>
            {
                db.BatchSize = DatabaseConfig.Instance.BatchSize;
            });
        }

    }
}