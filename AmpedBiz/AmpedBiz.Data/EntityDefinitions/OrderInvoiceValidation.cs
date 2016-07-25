using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderInvoiceValidation : ValidationDef<OrderPayment>
    {
        public OrderInvoiceValidation()
        {
            Define(x => x.Id);

            Define(x => x.PaidOn);

            Define(x => x.Order)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Tax)
                .IsValid();

            Define(x => x.ShippingFee)
                .IsValid();

            Define(x => x.Discount)
                .IsValid();

            Define(x => x.SubTotal)
                .IsValid();

            Define(x => x.Total)
                .IsValid();
        }
    }
}