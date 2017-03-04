using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderReturnValidation : ValidationDef<OrderReturn>
    {
        public OrderReturnValidation()
        {
            Define(x => x.Id);

            Define(x => x.Product)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Order)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Reason)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ReturnedBy)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ReturnedOn);

            Define(x => x.Quantity);

            Define(x => x.Returned)
                .IsValid();
        }
    }
}
