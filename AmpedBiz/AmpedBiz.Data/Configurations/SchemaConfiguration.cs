﻿using System.Diagnostics;
using System.IO;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace AmpedBiz.Data.Configurations
{
    internal static class SchemaConfiguration
    {
        public static void Configure(this Configuration config)
        {
            /// WARNING: Do not use in production
            RecreateDatabase(config);                

            /// NOTE: Applies in production
            //UpdateDatabase(config);
        }

        /// <summary>
        /// WARNING: this line will recreate all your 
        ///     database object removing all the data,
        ///     not to be used in production
        /// </summary>
        /// <param name="config">Nhibernate configuration</param>
        private static void RecreateDatabase(Configuration config)
        {
            //new SchemaExport(config).Create(true, true);

            var schemaPath = Path.Combine(System.Environment.CurrentDirectory, "Schema");

            if (!Directory.Exists(schemaPath))
                Directory.CreateDirectory(schemaPath);

            new SchemaExport(config)
                .SetOutputFile(Path.Combine(schemaPath, "schema.sql"))
                .Create(x => Debug.WriteLine(x), true);
        }

        //private static void Print(string value)
        //{
        //    Console.WriteLine(value);
        //}

        /// <summary>
        /// NOTE: this line will update your database
        ///     schema based on the changes you made
        ///     on your Entities(business models) if
        ///     there is any    
        /// </summary>
        /// <param name="config">Nhibernate configuration</param>
        private static void UpdateDatabase(Configuration config)
        {
            //new SchemaUpdate(config).Execute(true, true);

            new SchemaUpdate(config).Execute(x => Debug.WriteLine(x), true);
        }
    }
}
