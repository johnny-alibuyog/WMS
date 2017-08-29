﻿using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class TenantDefinition
    {
        public class Mapping : ClassMap<Tenant>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                Map(x => x.Name);

                Map(x => x.Description);
            }
        }

        public class Validation : ValidationDef<Tenant>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);

                Define(x => x.Description)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(250);
            }
        }

        public class Filter : FilterDefinition
        {
            public static string FilterName = "TenantFilter";
            public static string ParameterName = "tenantId";

            public Filter()
            {
                this.WithName(Filter.FilterName)
                    .WithCondition($"TenantId = :{Filter.ParameterName}")
                    .AddParameter(Filter.ParameterName, NHibernateUtil.String);
            }
        }
    }

    public static class TenantDefinitionExtention
    {
        public static ISession ApplyTenantFilter(this ISession session, string tenantId)
        {
            session
                .EnableFilter(TenantDefinition.Filter.FilterName)
                .SetParameter(TenantDefinition.Filter.ParameterName, tenantId);

            return session;
        }
    }
}
