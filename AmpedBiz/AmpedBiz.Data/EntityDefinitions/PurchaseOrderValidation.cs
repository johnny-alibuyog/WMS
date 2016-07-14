using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderValidation : ValidationDef<PurchaseOrder>
    {
        public PurchaseOrderValidation()
        {
            Define(x => x.Id);

            Define(x => x.PaymentType);

            Define(x => x.Supplier);

            Define(x => x.Tax)
                .IsValid();

            Define(x => x.ShippingFee)
                .IsValid();

            Define(x => x.Payment)
                .IsValid();

            Define(x => x.SubTotal)
                .IsValid();

            Define(x => x.Total)
                .IsValid();

            Define(x => x.Status);

            Define(x => x.ExpectedOn);

            Define(x => x.CreatedBy);

            Define(x => x.CreatedOn);

            Define(x => x.SubmittedBy);

            Define(x => x.SubmittedOn);

            Define(x => x.ApprovedBy);

            Define(x => x.ApprovedOn);

            Define(x => x.PaidBy);

            Define(x => x.PaidOn);

            Define(x => x.CompletedBy);

            Define(x => x.CompletedOn);

            Define(x => x.CancelledBy);

            Define(x => x.CancelledOn);

            Define(x => x.CancellationReason);

            Define(x => x.Items)
                .HasValidElements();

            Define(x => x.Payments)
                .HasValidElements();
        }
    }
}
