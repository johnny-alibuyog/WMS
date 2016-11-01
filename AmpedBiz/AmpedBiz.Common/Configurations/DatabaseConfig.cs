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
            this.Database = DatabaseProvider.MySql;
            this.Port = 3306;
            this.HostServer = "localhost";
            this.Name = "ampedbizdb";
            this.Username = "root";
            this.Password = "mysql";
            this.BatchSize = 50;
            this.UseDummyData = true;
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
                PropertiesToEncrypt = "Name,Username,Password",
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
