using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderReturnDefinition
    {
        public class Mapping : SubclassMap<OrderReturn>
        {
            public Mapping()
            {
                References(x => x.Order);

                References(x => x.ReturnedBy);

                Map(x => x.ReturnedOn);
            }
        }

        public class Validation : ValidationDef<OrderReturn>
        {
            public Validation()
            {
                Define(x => x.Order)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ReturnedBy)
                    .NotNullable()
                    .And.IsValid();

                Define(x => x.ReturnedOn);
            }
        }
    }
}
