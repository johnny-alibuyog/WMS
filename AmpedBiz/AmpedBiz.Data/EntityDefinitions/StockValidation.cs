using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class StockValidation : ValidationDef<Stock>
    {
        public StockValidation()
        {
            Define(x => x.Id);

            Define(x => x.CreatedOn);

            Define(x => x.CreatedBy)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ModifiedOn);

            Define(x => x.ModifiedBy);

            Define(x => x.Inventory)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Quantity)
                .NotNullable()
                .And.IsValid();

            Define(x => x.ExpiresOn);

            Define(x => x.Bad);
        }
    }

    public class ReleasedStockValidation : ValidationDef<ReleasedStock> { }

    public class ReceivedStockValidation : ValidationDef<ReceivedStock> { }

    public class ShrinkedStockValidation : ValidationDef<ShrinkedStock>
    {
        public ShrinkedStockValidation()
        {
            Define(x => x.Cause)
                .NotNullable()
                .And.IsValid();

            Define(x => x.Remarks)
                .MaxLength(700);
        }
    }
}
