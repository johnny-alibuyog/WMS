using AmpedBiz.Core.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
    public class TransactionAuditBaseDefinition
    {
        public class Mapping : ClassMap<TransactionAuditBase>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.TransactedBy);

                Map(x => x.TransactedOn);
            }
        }

        public class Validation : ValidationDef<TransactionAuditBase>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.TransactedBy)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.TransactedOn);
            }
        }
    }
}
