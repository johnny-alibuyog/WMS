using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Mapping;

namespace AmpedBiz.Data.Configurations
{
    internal static class IndexForeignKeyConfiguration
    {
        private static readonly PropertyInfo TableMappingsProperty = typeof(Configuration)
                 .GetProperty("TableMappings", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public static void Configure(Configuration config)
        {
            var tables = (ICollection<Table>)TableMappingsProperty.GetValue(config, null);
            foreach (var table in tables)
            {
                foreach (var foreignKey in table.ForeignKeyIterator)
                {
                    var index = new Index();
                    index.AddColumns(foreignKey.ColumnIterator);
                    index.Name = foreignKey.Name.Replace("FK_", "IDX_");
                    index.Table = table;
                    table.AddIndex(index);
                }
            }
        }
    }
}
