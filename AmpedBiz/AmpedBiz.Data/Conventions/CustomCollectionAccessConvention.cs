using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace AmpedBiz.Data.Conventions
{
    public class CustomCollectionAccessConvention : ICollectionConvention
    {
        public void Apply(ICollectionInstance instance)
        {
            //instance.Fetch.Join();
            //instance.Not.LazyLoad();
            //instance.Access.CamelCaseField(CamelCasePrefix.Underscore);

            var entityType = instance.EntityType;
            var camelCaseUnderscoreName = ConvertToCamelCaseUnderscore(instance.Member.Name);
            var hasBackingField = HasField(entityType, camelCaseUnderscoreName);

            // Default is to use property setter, so only modify mapping if there is a backing field
            if (hasBackingField)
                instance.Access.CamelCaseField(CamelCasePrefix.Underscore);
        }

        private string ConvertToCamelCaseUnderscore(string propertyName)
        {
            return "_" +
                propertyName[0].ToString().ToLower() +
                propertyName.Substring(1);
        }

        private bool HasField(Type type, string fieldName)
        {
            var backingField = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            return backingField != null;
        }

    }
}
