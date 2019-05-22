using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Conventions;
using AmpedBiz.Data.Definitions.Users;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;

namespace AmpedBiz.Data.Configurations
{
	internal static class MappingDefinitionConfiguration
	{
		public static void Configure(MappingConfiguration config)
		{
			config
				.FluentMappings.AddFromAssemblyOf<UserDefinition.Mapping>()
				.Conventions.AddFromAssemblyOf<CustomTableNameConvention>()
				.Conventions.Setup(o => o.Add(AutoImport.Never()))
				.ExportTo(DatabaseConfig.Instance.GetWorkingPath("Mappings"));
		}
	}
}
