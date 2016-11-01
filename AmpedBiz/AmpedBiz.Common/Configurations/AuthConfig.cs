using System;
using System.IO;
using Westwind.Utilities.Configuration;

namespace AmpedBiz.Common.Configurations
{
    public class AuthConfig : AppConfiguration
    {
        public static AuthConfig Instance = Create();

        public virtual string Secret { get; set; }

        public virtual string Issuer { get; set; }

        public virtual string Audience { get; set; }

        public AuthConfig()
        {
            this.Secret = "bOMx3lMZd84f88XV3L5R5e6gYheC47oF";
            this.Issuer = "localhost";
            this.Audience = "localhost";
        }

        private static AuthConfig Create()
        {
            var config = new AuthConfig();

            config.Initialize();

            return config;
        }

        protected override IConfigurationProvider OnCreateDefaultProvider(string sectionName, object configData)
        {
            this.Provider = new JsonFileConfigurationProvider<DatabaseConfig>()
            {
                EncryptionKey = "seekrit123",
                PropertiesToEncrypt = "Secret",
                JsonConfigurationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auth.config.json"),
            };

            return this.Provider;
        }
    }
}
