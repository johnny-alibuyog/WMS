using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderPaymentDefinition
    {
        public class Mapping : ClassMap<OrderPayment>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.Order);

                References(x => x.PaidTo);

                Map(x => x.PaidOn);

                References(x => x.PaymentType);

                Component(x => x.Payment,
                    MoneyDefinition.Mapping.Map("Payment_", nameof(OrderPayment)));
            }
        }

        public class Validation : ValidationDef<OrderPayment>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.Order)
                    .NotNullable();

                Define(x => x.PaidTo)
                    .NotNullable();

                Define(x => x.PaidOn);

                Define(x => x.PaymentType)
                    .NotNullable();

                Define(x => x.Payment)
                    .NotNullable();
            }
        }
    }
}