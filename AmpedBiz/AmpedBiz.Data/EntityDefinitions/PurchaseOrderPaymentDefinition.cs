using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderPaymentDefinition
    {
        public class Mapping : ClassMap<PurchaseOrderPayment>
        {
            public Mapping()
            {
                Id(x => x.Id)
                   .GeneratedBy.GuidComb();

                References(x => x.PurchaseOrder);

                References(x => x.PaidBy);

                Map(x => x.PaidOn);

                References(x => x.PaymentType);

                Component(x => x.Payment,
                    MoneyDefinition.Mapping.Map("Payment_", nameof(PurchaseOrderPayment)));
            }
        }

        public class Validation : ValidationDef<PurchaseOrderPayment>
        {
            public Validation()
            {
                Define(x => x.Id);

                Define(x => x.PurchaseOrder)
                    .NotNullable();

                Define(x => x.PaidBy)
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
