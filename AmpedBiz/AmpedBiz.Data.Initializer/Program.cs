using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Initializer.Bootstrap;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Dto.Mappers;
using Autofac;

namespace AmpedBiz.Data.Initializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = DatabaseConfig.Instance.Seeder;
            config.Enabled = true;          // enforce seeding
            config.UseExternalFiles = true; // enforce seeding

            Ioc.Container.Resolve<IMapper>().Initialze();
            Ioc.Container.Resolve<Runner>().Run(config);
        }
    }
}