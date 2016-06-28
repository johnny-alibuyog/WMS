using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderValidation : ValidationDef<Order>
    {
        public OrderValidation()
        {
            Define(x => x.Id);

            Define(x => x.Branch);

            Define(x => x.OrderDate);

            Define(x => x.ShippedDate);

            Define(x => x.PaymentDate);

            Define(x => x.CompletedDate);

            Define(x => x.CancelDate);

            Define(x => x.CancelReason);

            Define(x => x.PaymentType)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Shipper)
                .IsValid();

            Define(x => x.TaxRate);

            Define(x => x.Tax);

            Define(x => x.ShippingFee);

            Define(x => x.SubTotal);

            Define(x => x.Total);

            Define(x => x.Status);

            Define(x => x.IsActive);

            Define(x => x.Customer)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Invoices)
                .HasValidElements();

            //Define(x => x.OrderDetails)
            //    .NotNullableAndNotEmpty()
            //    .And.HasValidElements();
        }
    }
}
