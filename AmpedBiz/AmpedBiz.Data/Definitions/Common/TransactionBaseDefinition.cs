using AmpedBiz.Core.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
    public class TransactionBaseDefinition
    {
        public class Mapping : ClassMap<TransactionBase>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                //References(x => x.Branch);
            }
        }

        public class Validation : ValidationDef<TransactionBase>
        {
            public Validation()
            {
                Define(x => x.Id);

                //Define(x => x.Branch)
                //    .IsValid();
            }
        }
    }
}
