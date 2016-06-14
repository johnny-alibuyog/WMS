using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderDetailValidation : ValidationDef<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailValidation()
        {
            Define(x => x.Id);

            Define(x => x.PurchaseOrder)
                .NotNullable();

            Define(x => x.Product)
                .NotNullable();

            Define(x => x.Quantity);

            Define(x => x.UnitPrice)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Total)
                .NotNullable()
                .And.IsValid();

            Define(x => x.DateReceived);

            Define(x => x.Status);
        }
    }
}