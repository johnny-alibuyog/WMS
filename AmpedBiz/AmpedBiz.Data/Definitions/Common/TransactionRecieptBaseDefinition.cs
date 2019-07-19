using AmpedBiz.Core.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
    public class TransactionRecieptBaseDefinition
    {
        public class Mapping : ClassMap<TransactionRecieptBase>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();
            }
        }

        public class Validation : ValidationDef<TransactionRecieptBase>
        {
            public Validation()
            {
                Define(x => x.Id);
            }
        }
    }
}
