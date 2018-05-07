using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using System.Collections.Generic;
using System.Linq;
using static AmpedBiz.Common.Configurations.DatabaseConfig;

namespace AmpedBiz.Data.Seeders
{
    public class Runner
    {
        private readonly IEnumerable<ISeeder> _seeders;

        public Runner(IEnumerable<ISeeder> seeders, DatabaseConfig dbConfig = null)
        {
            this._seeders = seeders;
        }

        public void Run(SeederConfig config)
        {
            if (config == null)
                config = DatabaseConfig.Instance.Seeder;

            if (!config.Enabled)
                return;

            var seeders = this._seeders
                .OrderBy(x => x.GetType().AssemblyQualifiedName)
                .Where(x =>
                    (!config.UseDummyData ? !(x is IDummyDataSeeder) : true) &&
                    (!config.UseExternalFiles ? !x.IsSourceExternalFile : true)
                );

            seeders.ForEach(x => x.Seed());
        }
    }
}
