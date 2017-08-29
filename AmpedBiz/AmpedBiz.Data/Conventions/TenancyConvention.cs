using AmpedBiz.Core;
using AmpedBiz.Data.EntityDefinitions;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace AmpedBiz.Data.Conventions
{
    public class TenancyConvention : IClassConvention, IClassConventionAcceptance
    {
        public void Apply(IClassInstance instance)
        {
            instance.ApplyFilter<TenantDefinition.Filter>();
        }

        public void Accept(IAcceptanceCriteria<IClassInspector> criteria)
        {
            criteria.Expect(x =>
                x.EntityType.IsAbstract != true &&
                x.EntityType.GetType() == (typeof(IHaveTenant))
            );
        }
    }
}
