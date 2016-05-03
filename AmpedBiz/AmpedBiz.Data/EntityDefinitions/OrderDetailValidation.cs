using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderDetailValidation : ValidationDef<OrderDetail>
    {
        public OrderDetailValidation()
        {
            Define(x => x.Id);

            Define(x => x.Product)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Order)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Quantity);

            Define(x => x.Discount)
                .IsValid();

            Define(x => x.UnitPrice)
                .IsValid();

            Define(x => x.ExtendedPrice)
                .IsValid();

            Define(x => x.Status);

            Define(x => x.InsufficientInventory);
        }
    }
}
