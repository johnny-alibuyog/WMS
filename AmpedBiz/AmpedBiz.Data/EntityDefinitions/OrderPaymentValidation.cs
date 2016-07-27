using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderPaymentValidation : ValidationDef<OrderPayment>
    {
        public OrderPaymentValidation()
        {
            Define(x => x.Id);

            Define(x => x.Order)
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