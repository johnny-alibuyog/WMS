using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InvoiceValidation : ValidationDef<Invoice>
    {
        public InvoiceValidation()
        {
            Define(x => x.Id);

            Define(x => x.DueDate);

            Define(x => x.InvoiceDate);

            Define(x => x.Order)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Shipping)
                .IsValid();

            Define(x => x.SubTotal)
                .IsValid();

            Define(x => x.Tax)
                .IsValid();

            Define(x => x.Total)
                .IsValid();
        }
    }
}