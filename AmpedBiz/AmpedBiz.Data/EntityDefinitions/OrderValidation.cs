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

            Define(x => x.OrderedOn);

            Define(x => x.StagedOn);

            Define(x => x.RoutedOn);

            Define(x => x.InvoicedOn);

            Define(x => x.ShippedOn);

            Define(x => x.PaidOn);

            Define(x => x.CompletedOn);

            Define(x => x.CancelledOn);

            Define(x => x.CancellationReason);

            Define(x => x.CreatedBy);

            Define(x => x.StagedBy);

            Define(x => x.RoutedBy);

            Define(x => x.InvoicedBy);

            Define(x => x.PaidTo);

            Define(x => x.CompletedBy);

            Define(x => x.CancelledBy);

            Define(x => x.PaymentType)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Shipper)
                .IsValid();

            Define(x => x.TaxRate);

            Define(x => x.Tax);

            Define(x => x.ShippingFee);

            Define(x => x.Discount);

            Define(x => x.SubTotal);

            Define(x => x.Total);

            Define(x => x.Status);

            Define(x => x.IsActive);

            Define(x => x.Customer)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Invoices)
                .HasValidElements();

            Define(x => x.Items)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();
        }
    }
}
