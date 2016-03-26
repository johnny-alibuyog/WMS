using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace AmpedBiz.Data.Conventions
{
    public class CustomForeignKeyConstraintConvention : IHasManyConvention, IReferenceConvention, IHasOneConvention
    {
        private readonly PluralizationService _pluralizationService;

        public CustomForeignKeyConstraintConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }


        public void Apply(IOneToManyCollectionInstance instance)
        {
            instance.Key.ForeignKey(string.Format("FK_{0}_{1}", instance.Member.Name, instance.EntityType.Name));
        }

        public void Apply(IManyToOneInstance instance)
        {
            instance.ForeignKey(string.Format("FK_{0}_{1}", _pluralizationService.Pluralize(instance.EntityType.Name), instance.Name));
        }

        public void Apply(IOneToOneInstance instance)
        {
            instance.ForeignKey(string.Format("FK_{0}_{1}", _pluralizationService.Pluralize(instance.EntityType.Name), instance.Name));
        }
    }
}
