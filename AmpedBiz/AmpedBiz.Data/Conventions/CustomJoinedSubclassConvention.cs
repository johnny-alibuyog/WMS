using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace AmpedBiz.Data.Conventions
{
    public class CustomJoinedSubclassConvention : IJoinedSubclassConvention
    {
        private readonly PluralizationService _pluralizationService;

        public CustomJoinedSubclassConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            var basetype = instance.Extends;
            while (basetype.IsAbstract)
            {
                basetype = basetype.BaseType;
            }

            instance.Table(_pluralizationService.Pluralize(instance.EntityType.Name));
            instance.Key.Column(instance.EntityType.Name + "Id");

            instance.Key.Column(basetype.Name + "Id");
        }
    }
}
