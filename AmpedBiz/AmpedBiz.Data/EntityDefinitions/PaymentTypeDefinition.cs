using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PaymentTypeDefinition
    {
        public class Mapping : ClassMap<PaymentType>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.Assigned();

                Map(x => x.Name);
            }
        }

        public class Validation : ValidationDef<PaymentType>
        {
            public Validation()
            {
                Define(x => x.Id)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(30);

                Define(x => x.Name)
                    .NotNullableAndNotEmpty()
                    .And.MaxLength(150);
            }
        }
    }
}
