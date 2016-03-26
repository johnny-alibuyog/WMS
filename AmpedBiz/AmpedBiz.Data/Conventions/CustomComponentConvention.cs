using System.Reflection;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using FluentNHibernate.MappingModel.ClassBased;

namespace AmpedBiz.Data.Conventions
{
    public class CustomComponentConvention : IComponentConvention
    {
        public void Apply(IComponentInstance instance)
        {
            var mapping = typeof(ComponentInstance)
                .GetField("mapping", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(instance) as ComponentMapping;

            foreach (var reference in mapping.References)
            {
                var referenceInstance = new ManyToOneInstance(reference);
                var columnName = mapping.HasColumnPrefix
                    ? mapping.ColumnPrefix + referenceInstance.Property.Name + "Id"
                    : referenceInstance.Property.Name + "Id";

                referenceInstance.Column(columnName);
            }
        }
    }
}
