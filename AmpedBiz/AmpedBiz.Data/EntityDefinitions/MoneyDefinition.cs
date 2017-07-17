using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;
using System;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class MoneyDefinition
    {
        public class Mapping : ComponentMap<Money>
        {
            public Mapping()
            {
                Map(x => x.Amount);

                References(x => x.Currency);
            }

            internal static Action<ComponentPart<Money>> Map(string prefix = "", string parent = "")
            {
                return mapping =>
                {
                    mapping.Map(x => x.Amount, $"{prefix}Amount");

                    mapping.References(x => x.Currency, $"{prefix}CurrencyId")
                        .ForeignKey($"FK_{parent}_{prefix}Currency");
                };
            }
        }

        public class Validation : ValidationDef<Money>
        {
            public Validation()
            {
                Define(x => x.Amount);

                Define(x => x.Currency)
                    .IsValid();
            }
        }
    }
}
