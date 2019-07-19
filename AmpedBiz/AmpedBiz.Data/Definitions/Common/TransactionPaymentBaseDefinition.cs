using AmpedBiz.Core.Common;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.Definitions.Common
{
    public class TransactionPaymentBaseDefinition
    {
        public class Mapping : ClassMap<TransactionPaymentBase>
        {
            public Mapping()
            {
                Id(x => x.Id)
                    .GeneratedBy.GuidComb();

                References(x => x.PaymentBy);

                Map(x => x.PaymentOn);

                References(x => x.PaymentType);

                Component(x => x.Payment,
                    MoneyDefinition.Mapping.Map("Payment_", nameof(TransactionPaymentBase)));

                Component(x => x.Balance,
                    MoneyDefinition.Mapping.Map("Balance_", nameof(TransactionPaymentBase)));
            }
        }

        public class Validation : ValidationDef<TransactionPaymentBase>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.PaymentBy)
                    .NotNullable();

                Define(x => x.PaymentOn);

                Define(x => x.PaymentType)
                    .NotNullable();

                Define(x => x.Payment)
                    .NotNullable();

                this.ValidateInstance.By((instance, context) =>
                {
                    var valid = true;

                    if (instance.Payment.Amount <= 0)
                    {
                        context.AddInvalid<TransactionPaymentBase, Money>(
                            message: "Payment sould contain amount.",
                            property: x => x.Payment
                        );
                        valid = false;
                    }

                    return valid;
                });
            }
        }
    }
}
