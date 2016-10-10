using AmpedBiz.Common.Configurations;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Diagnostics;
using System.IO;

namespace AmpedBiz.Data.Configurations
{
    internal static class SchemaConfiguration
    {
        public static void Configure(this Configuration config)
        {
            if (DbConfig.Instance.RecreateDb)
                RecreateDatabase(config);   /// WARNING: Do not use in production
            else
                UpdateDatabase(config);     /// NOTE: Applies in production
        }

        private static void RecreateDatabase(Configuration config)
        {
            //new SchemaExport(config).Create(true, true);

            var schemaPath = Path.Combine(DbConfig.Instance.GetWorkingPath("Schemas"), "schema.sql");

            new SchemaExport(config)
                .SetOutputFile(schemaPath)
                .Create(x => Debug.WriteLine(x), true);
                //.Create(true, true);
        }

        private static void UpdateDatabase(Configuration config)
        {
            //new SchemaUpdate(config).Execute(true, true);

            new SchemaUpdate(config).Execute(x => Debug.WriteLine(x), true);
        }
    }
}
