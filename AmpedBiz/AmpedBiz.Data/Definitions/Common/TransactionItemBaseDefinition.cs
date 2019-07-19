using AmpedBiz.Core.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
    public class TransactionItemBaseDefinition
    {
        public class Mapping : ClassMap<TransactionItemBase>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();
            }
        }

        public class Validation : ValidationDef<TransactionItemBase>
        {
            public Validation()
            {
                Define(x => x.Id);
            }
        }
    }
}
