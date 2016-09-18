using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderValidation : ValidationDef<Order>
    {
        public OrderValidation()
        {
            Define(x => x.Id);

            Define(x => x.OrderNumber)
                .MaxLength(30);

            Define(x => x.Branch)
                .IsValid();

            Define(x => x.Customer)
                .NotNullable()
                .And.IsValid();

            Define(x => x.PaymentType);

            Define(x => x.Shipper);

            Define(x => x.TaxRate);

            Define(x => x.Tax);

            Define(x => x.ShippingFee);

            Define(x => x.Discount);

            Define(x => x.Returned);

            Define(x => x.SubTotal)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Total)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Status)
                .NotNullable();

            Define(x => x.DueOn);

            Define(x => x.OrderedOn);

            Define(x => x.OrderedBy);

            Define(x => x.CreatedOn);

            Define(x => x.CreatedBy);

            Define(x => x.StagedOn);

            Define(x => x.StagedBy);

            Define(x => x.ShippedOn);

            Define(x => x.ShippedBy);

            Define(x => x.RoutedOn);

            Define(x => x.RoutedBy);

            Define(x => x.InvoicedOn);

            Define(x => x.InvoicedBy);

            Define(x => x.PaidOn);

            Define(x => x.PaidTo);

            Define(x => x.CompletedOn);

            Define(x => x.CompletedBy);

            Define(x => x.CancelledOn);

            Define(x => x.CancelledBy);

            Define(x => x.CancellationReason);

            Define(x => x.PricingScheme);

            Define(x => x.Payments)
                .HasValidElements();

            Define(x => x.Items)
                .NotNullableAndNotEmpty()
                .And.HasValidElements();
        }
    }
}
