﻿using System.Linq;
using System.Web.Http;
using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Seeders;
using Common.Logging;

namespace AmpedBiz.Service.Host.App_Start
{
    public class DataSeederConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var log = LogManager.GetLogger<DataSeederConfig>();
            log.Error("log me like you do");

            var seeders = config.DependencyResolver
                .GetServices(typeof(ISeeder)).Cast<ISeeder>()
                .Where(x => !DbConfig.Instance.UseDummyData ? !x.IsDummyData : true)
                .OrderBy(x => x.ExecutionOrder);

            foreach (var seeder in seeders)
            {
                seeder.Seed();
            }
        }
    }
}