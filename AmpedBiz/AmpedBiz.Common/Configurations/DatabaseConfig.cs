using System;
using System.IO;
using Westwind.Utilities.Configuration;

namespace AmpedBiz.Common.Configurations
{
    public enum DatabaseProvider
    {
        MsSql,
        MySql,
        Postgres
    }

    public class DatabaseConfig : AppConfiguration
    {
        public static DatabaseConfig Instance = Create();

        public virtual DatabaseProvider Database { get; set; }

        public virtual int Port { get; set; }

        public virtual string HostServer { get; set; }

        public virtual string Name { get; set; }

        public virtual string Username { get; set; }

        public virtual string Password { get; set; }

        public virtual short BatchSize { get; set; }

        public virtual bool UseDummyData { get; set; }

        public virtual bool RecreateDb { get; set; }

        public DatabaseConfig()
        {
            // staging.gear.host
            //this.Database = DatabaseProvider.MySql;
            //this.Port = 3306;
            //this.HostServer = "mysql3.gear.host";
            //this.Name = "ampbizdb";
            //this.Username = "ampbizdb";
            //this.Password = "123!@#qwe";
            //this.BatchSize = 50;
            //this.UseDummyData = true;
            //this.RecreateDb = true;

            // staging.mywindowshosting
            //this.Database = DatabaseProvider.MySql;
            //this.Port = 3306;
            //this.HostServer = "mysql5005.mywindowshosting.com";
            //this.Name = "db_a123b7_ampbiz";
            //this.Username = "a123b7_ampbiz";
            //this.Password = "123!@#qwe";
            //this.BatchSize = 50;
            //this.UseDummyData = true;
            //this.RecreateDb = true;

            // local.mysql
            //this.Database = DatabaseProvider.MySql;
            //this.Port = 3306;
            //this.HostServer = "localhost";
            //this.Name = "ampedbizdb";
            //this.Username = "root";
            //this.Password = "123!@#qwe";
            //this.BatchSize = 50;
            //this.UseDummyData = true;
            //this.RecreateDb = true;

            // local.postgres
            this.Database = DatabaseProvider.Postgres;
            this.Port = 5432;
            this.HostServer = "localhost";
            this.Name = "ampedbizdb";
            this.Username = "postgres";
            this.Password = "123!@#qwe";
            this.BatchSize = 50;
            this.UseDummyData = false;
            this.RecreateDb = true;
        }

        private static DatabaseConfig Create()
        {
            var config = new DatabaseConfig();
            config.Initialize();
            config.Write();

            return config;
        }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            this.Provider = new JsonFileConfigurationProvider<DatabaseConfig>()
            {
                EncryptionKey = "seekrit123",
                PropertiesToEncrypt = "Password",
                JsonConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database.config.json"),
            };

            return this.Provider;
        }

        public string GetWorkingPath(string folder = "")
        {
            var workingPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", folder);

            if (!Directory.Exists(workingPath))
                Directory.CreateDirectory(workingPath);

            return workingPath;
        }
    }
}
