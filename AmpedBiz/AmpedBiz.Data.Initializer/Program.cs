using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Common;
using AmpedBiz.Data.Initializer.Bootstrap;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Dto.Mappers;
using Autofac;
using System.Linq;

namespace AmpedBiz.Data.Initializer
{
	class Program
	{
		static void Main(string[] args)
		{
			var config = DatabaseConfig.Instance.Seeder;
			config.Enabled = true;          // enforce seeding
			config.UseExternalFiles = true; // enforce seeding

			var container = Ioc.BuildContainer(args.Any() ? args[0] : Tenant.Default.Id);
			container.Resolve<IMapper>().Initialze();
			container.Resolve<Runner>().Run(config);
		}
	}
}