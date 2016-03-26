using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AmpedBiz.Data.DataInitializer;

namespace AmpedBiz.Service.Host.App_Start
{
    public class DataSeederConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var seeders = config.DependencyResolver.GetServices(typeof(IDataSeeder)).Cast<IDataSeeder>();
            foreach(var seeder in seeders)
            {
                seeder.Seed();
            }
        }
    }
}