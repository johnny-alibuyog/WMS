using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using AmpedBiz.Common.Extentions;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;

namespace AmpedBiz.Data.Conventions
{
    public class CustomTableNameConvention : IClassConvention, IJoinedSubclassConvention
    {
        private readonly PluralizationService _pluralizationService;

        private string GenerateTableName(Type type) => _pluralizationService.Pluralize(type.Name);

        private string GenerateSchemaName(Type type) => type.ParseSchema();

        public CustomTableNameConvention()
        {
            _pluralizationService = PluralizationService.CreateService(new CultureInfo("en-US"));
        }

        public void Apply(IClassInstance instance)
        {
            //instance.Schema(GenerateSchemaName(instance.EntityType));
            instance.Table(GenerateTableName(instance.EntityType));
        }

        public void Apply(IJoinedSubclassInstance instance)
        {
            //instance.Schema(GenerateSchemaName(instance.EntityType));
            instance.Table(GenerateTableName(instance.EntityType));
        }
    }
}
