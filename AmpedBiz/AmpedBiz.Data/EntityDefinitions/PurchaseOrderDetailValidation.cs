using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderDetailValidation : ValidationDef<PurchaseOrderDetail>
    {
        public PurchaseOrderDetailValidation()
        {
            Define(x => x.Id);

            Define(x => x.Product)
                .NotNullable()
                .And.IsValid();

            Define(x => x.DateReceived);

            Define(x => x.ExtendedPrice)
                .IsValid();

            Define(x => x.PurchaseOrder)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Quantity);

            Define(x => x.Status);

            Define(x => x.UnitCost)
                .IsValid();
        }
    }
}