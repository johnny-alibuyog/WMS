using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Seeders;
using Common.Logging;
using System.Web.Http;

namespace AmpedBiz.Service.Host.App_Start
{
    public class DataSeederConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var log = LogManager.GetLogger<DataSeederConfig>();
            log.Error("log me like you do");

            var runner = config.DependencyResolver.GetService(typeof(Runner)) as Runner;
            var seederConfig = DatabaseConfig.Instance.Seeder;
            runner.Run(seederConfig);
        }
    }
}