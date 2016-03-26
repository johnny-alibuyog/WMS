using System;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;

namespace AmpedBiz.Data.Conventions
{
    // Note: underscore was included to make this class on top of the naming order as it affects
    //       the ussage sequence
    public class _CustomJoinedSubclassConvention : IJoinedSubclassConvention
    {
        private readonly PluralizationService _pluralizationService;

        public _CustomJoinedSubclassConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            instance.Table(_pluralizationService.Pluralize(instance.EntityType.Name));
            instance.Key.Column(instance.EntityType.Name + "Id");
        }
    }
}
