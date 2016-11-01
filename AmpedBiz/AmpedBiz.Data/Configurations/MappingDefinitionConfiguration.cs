using AmpedBiz.Common.Configurations;
using AmpedBiz.Data.Conventions;
using AmpedBiz.Data.EntityDefinitions;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;

namespace AmpedBiz.Data.Configurations
{
    internal static class MappingDefinitionConfiguration
    {
        public static void Configure(MappingConfiguration config)
        {
            config
                .FluentMappings.AddFromAssemblyOf<UserMapping>()
                .Conventions.AddFromAssemblyOf<CustomJoinedSubclassConvention>()
                .Conventions.Setup(o => o.Add(AutoImport.Never()))
                .ExportTo(DatabaseConfig.Instance.GetWorkingPath("Mappings"));
        }
    }
}
