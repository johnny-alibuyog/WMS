using System;
using System.IO;
using System.Reflection;
using Westwind.Utilities.Configuration;

namespace AmpedBiz.Common.Configurations
{
    public enum DatabaseProvider
    {
        MsSql,
        MySql,
        Postgres,
        InMemory
    }

    public class DatabaseConfig : AppConfiguration
    {
        public static DatabaseConfig Instance = Create();

        public DatabaseProvider Database { get; set; }

        public int Port { get; set; }

        public string HostServer { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public short BatchSize { get; set; }

        public bool RecreateDb { get; set; }

        public bool UpdateDb { get; set; }

        public SeederConfig Seeder { get; set; } = new SeederConfig();

        public DatabaseConfig()
        {
            //// staging.gear.host
            //this.Database = DatabaseProvider.MySql;
            //this.Port = 3306;
            //this.HostServer = "mysql4.gear.host";
            //this.Name = "ampbizdb";
            //this.Username = "ampbizdb";
            //this.Password = "123!@#qwe";
            //this.BatchSize = 50;
            //this.RecreateDb = false;
            //this.UpdateDb = true;
            //this.Seeder = new SeederConfig()
            //{
            //    Enabled = true,
            //    UseDummyData = true,
            //    UseExternalFiles = true,
            //    ExternalFilesPath = ".\\Data\\Default"
            //};


            // staging.mywindowshosting
            //this.Database = DatabaseProvider.MySql;
            //this.Port = 3306;
            //this.HostServer = "mysql5005.mywindowshosting.com";
            //this.Name = "db_a123b7_ampbiz";
            //this.Username = "a123b7_ampbiz";
            //this.Password = "123!@#qwe";
            //this.BatchSize = 50;
            //this.RecreateDb = true;
            //this.UpdateDb = true;
            //this.Seeder = new SeederConfig()
            //{
            //    Enabled = true,
            //    UseDummyData = true,
            //    UseExternalFiles = true,
            //    ExternalFilesPath = ".\\Data\\Default"
            //};

            //// local.mssql
            //this.Database = DatabaseProvider.MsSql;
            //this.Port = 3306;
            //this.HostServer = "PHMANJALIBUYO03\\LAWSONINSTANCE";
            //this.Name = "ampedbizdb";
            //this.Username = "lawsql";
            //this.Password = "L@wsql";
            //this.BatchSize = 50;
            //this.RecreateDb = true;
            //this.UpdateDb = true;
            //this.Seeder = new SeederConfig()
            //{
            //    Enabled = true,
            //    UseDummyData = true,
            //    UseExternalFiles = true,
            //    ExternalFilesPath = ".\\Data\\Default"
            //};

            // local.mysql
            this.Database = DatabaseProvider.MySql;
            this.Port = 3306;
            this.HostServer = "localhost";
            this.Name = "ampedbizdb";
            this.Username = "root";
            this.Password = "123!@#qwe";
            this.BatchSize = 50;
            this.RecreateDb = true;
            this.UpdateDb = true;
            this.Seeder = new SeederConfig()
            {
                Enabled = true,
                UseDummyData = true,
                UseExternalFiles = true,
                ExternalFilesPath = ".\\Data\\Default"
            };

            // local.postgres
            //this.Database = DatabaseProvider.Postgres;
            //this.Port = 5432;
            //this.HostServer = "localhost";
            //this.Name = "ampedbizdb";
            //this.Username = "postgres";
            //this.Password = "123!@#qwe";
            //this.BatchSize = 50;
            //this.RecreateDb = true;
            //this.UpdateDb = true;
            //this.Seeder = new SeederConfig()
            //{
            //    Enabled = true,
            //    UseDummyData = true,
            //    UseExternalFiles = true,
            //    ExternalFilesPath = ".\\Data\\Default"
            //};
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

        public class SeederConfig
        {
            public virtual bool Enabled { get; set; }

            public virtual bool UseDummyData { get; set; }

            public virtual bool UseExternalFiles { get; set; }

            public virtual string ExternalFilesPath { get; set; }

            public virtual string ExternalFilesAbsolutePath => Path.Combine(this.AssemblyPath(), this.ExternalFilesPath);

            private string AssemblyPath()
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
