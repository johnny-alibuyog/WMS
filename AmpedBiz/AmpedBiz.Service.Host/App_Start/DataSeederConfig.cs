using System.Linq;
using System.Web.Http;
using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Seeders;
using Common.Logging;
using Autofac;

namespace AmpedBiz.Service.Host.App_Start
{
    public class DataSeederConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var log = LogManager.GetLogger<DataSeederConfig>();
            log.Error("log me like you do");

            //var seeders = config.DependencyResolver
            //    .GetServices(typeof(ISeeder)).Cast<ISeeder>()
            //    .Where(x => !DbConfig.Instance.UseDummyData ? !x.IsDummyData : true)
            //    .OrderBy(x => x.ExecutionOrder);

            //foreach (var seeder in seeders)
            //{
            //    seeder.Seed();
            //}

            var defaultDataSeeders = config.DependencyResolver
                .GetServices(typeof(IDefaultDataSeeder)).Cast<IDefaultDataSeeder>()
                .OrderBy(x => x.GetType().Name)
                .Cast<ISeeder>();

            var dummyDataSeeders = config.DependencyResolver
                .GetServices(typeof(IDummyDataSeeder)).Cast<IDummyDataSeeder>()
                .Where(x => DatabaseConfig.Instance.UseDummyData)
                .OrderBy(x => x.GetType().Name)
                .Cast<ISeeder>();

            foreach(var seeder in defaultDataSeeders.Concat(dummyDataSeeders))
            {
                seeder.Seed();
            }
        }
    }
}