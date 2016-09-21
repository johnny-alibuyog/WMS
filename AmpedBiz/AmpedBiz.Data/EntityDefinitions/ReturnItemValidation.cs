using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class ReturnItemValidation : ValidationDef<ReturnItem>
    {
        public ReturnItemValidation()
        {
            Define(x => x.Id);

            Define(x => x.Product)
                .NotNullable();

            Define(x => x.Return)
                .NotNullable();

            Define(x => x.Quantity)
                .IsValid();

            Define(x => x.UnitPrice)
                .IsValid();

            Define(x => x.TotalPrice)
                .IsValid();
        }
    }
}
