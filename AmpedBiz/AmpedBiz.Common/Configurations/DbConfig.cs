using System;
using System.IO;
using Westwind.Utilities.Configuration;

namespace AmpedBiz.Common.Configurations
{
    public class DbConfig : AppConfiguration
    {
        public static DbConfig Instance = Create();

        public virtual int Port { get; set; }
        public virtual string Host { get; set; }
        public virtual string Name { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual bool UseDummyData { get; set; }
        public virtual bool RecreateDb { get; set; }

        public DbConfig()
        {
            this.Port = 5432;
            this.Host = "localhost";
            this.Name = "ampedbizdb";
            this.Username = "postgres";
            this.Password = "postgres";
            this.UseDummyData = true;
            this.RecreateDb = true;
        }

        private static DbConfig Create()
        {
            var config = new DbConfig();
            config.Initialize();

            return config;
        }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            this.Provider = new JsonFileConfigurationProvider<DbConfig>()
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
