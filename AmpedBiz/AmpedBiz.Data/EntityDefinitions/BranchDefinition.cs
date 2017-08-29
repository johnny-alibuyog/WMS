using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate;
using NHibernate.Validator.Cfg.Loquacious;
using System;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class BranchDefinition
    {
        public class Mapping : ClassMap<Branch>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Tenant);

                Map(x => x.Name);

                Map(x => x.Description);

                Map(x => x.TaxpayerIdentificationNumber);

                Component(x => x.Contact);

                Component(x => x.Address);

                Map(x => x.CreatedOn);

                Map(x => x.ModifiedOn);

                //OptimisticLock.Dirty();

                ApplyFilter<TenantDefinition.Filter>();
            }
        }

        public class Validation : ValidationDef<Branch>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Tenant);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);

                Define(x => x.Description)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);

                Define(x => x.Address)
                    .IsValid();

                Define(x => x.CreatedOn);

                Define(x => x.ModifiedOn);
            }
        }

        public class Filter : FilterDefinition
        {
            public static string FilterName = "BranchFilter";
            public static string ParameterName = "branchId";

            public Filter()
            {
                this.WithName(Filter.FilterName)
                    .WithCondition($"(BranchId = :{Filter.ParameterName} or BranchId is null)")
                    .AddParameter(Filter.ParameterName, NHibernateUtil.Guid);
            }
        }
    }

    public static class BranchDefinitionExtention
    {
        public static ISession ApplyBranchFilter(this ISession session, Guid branchId)
        {
            session
                .EnableFilter(BranchDefinition.Filter.FilterName)
                .SetParameter(BranchDefinition.Filter.ParameterName, branchId);

            return session;
        }
    }
}
