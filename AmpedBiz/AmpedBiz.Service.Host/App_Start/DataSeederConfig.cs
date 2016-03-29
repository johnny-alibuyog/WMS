using System.Linq;
using System.Web.Http;
using AmpedBiz.Data.DataInitializer;
using Common.Logging;

namespace AmpedBiz.Service.Host.App_Start
{
    public class DataSeederConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var log = LogManager.GetLogger(typeof(DataSeederConfig));
            log.Error("log me like you do");


            var seeders = config.DependencyResolver.GetServices(typeof(IDataSeeder)).Cast<IDataSeeder>();
            foreach(var seeder in seeders.OrderBy(x => x.ExecutionOrder))
            {
                seeder.Seed();
            }
        }
    }
}