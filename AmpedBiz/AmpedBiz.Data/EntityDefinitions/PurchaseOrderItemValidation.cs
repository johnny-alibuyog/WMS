using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderItemValidation : ValidationDef<PurchaseOrderItem>
    {
        public PurchaseOrderItemValidation()
        {
            Define(x => x.Id);

            Define(x => x.PurchaseOrder)
                .NotNullable();

            Define(x => x.Product)
                .NotNullable();

            Define(x => x.Quantity);

            Define(x => x.UnitCost)
                .NotNullable()
                .And.IsValid();

            Define(x => x.TotalCost)
                .NotNullable()
                .And.IsValid();
        }
    }
}