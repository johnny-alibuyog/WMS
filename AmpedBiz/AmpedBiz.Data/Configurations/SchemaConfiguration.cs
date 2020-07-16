using AmpedBiz.Common.Configurations;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Diagnostics;
using System.IO;

namespace AmpedBiz.Data.Configurations
{
    internal static class SchemaConfiguration
    {
        public static void Configure(Configuration config)
        {
            if (DatabaseConfig.Instance.RecreateDb)
                RecreateDatabase(config);   /// WARNING: Do not use in production
            else if (DatabaseConfig.Instance.UpdateDb)
                UpdateDatabase(config);     /// NOTE: Applies in production
        }

        private static void RecreateDatabase(this Configuration config)
        {
            var path = Path.Combine(DatabaseConfig.Instance.GetWorkingPath("Schemas"), "schema.sql");

            var schema = new SchemaExport(config);
            schema.SetDelimiter(";");
            schema.SetOutputFile(path);
            schema.Create(x => Debug.WriteLine(x), true);
        }

        private static void UpdateDatabase(Configuration config)
        {
            var schema = new SchemaUpdate(config);
            schema.Execute(x => Debug.WriteLine(x), true);
        }
    }
}
