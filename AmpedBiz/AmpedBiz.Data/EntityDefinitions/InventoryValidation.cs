using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryValidation : ValidationDef<Inventory>
    {
        public InventoryValidation()
        {
            Define(x => x.Id);

            Define(x => x.Product)
                .IsValid();

            Define(x => x.IndividualBarcode)
                .MaxLength(255);

            Define(x => x.PackagingBarcode)
                .MaxLength(255);

            Define(x => x.UnitOfMeasure);

            Define(x => x.PackagingUnitOfMeasure);

            Define(x => x.PackagingSize);

            Define(x => x.BasePrice);

            Define(x => x.DistributorPrice);

            Define(x => x.ListPrice);

            Define(x => x.BadStockPrice);

            Define(x => x.BadStock);

            Define(x => x.Received);

            Define(x => x.OnOrder);

            Define(x => x.OnHand);

            Define(x => x.Allocated);

            Define(x => x.Shipped);

            Define(x => x.BackOrdered);

            Define(x => x.Available);

            Define(x => x.InitialLevel);

            Define(x => x.Shrinkage);

            Define(x => x.CurrentLevel);

            Define(x => x.TargetLevel);

            Define(x => x.BelowTargetLevel);

            Define(x => x.ReorderLevel);

            Define(x => x.ReorderQuantity);

            Define(x => x.MinimumReorderQuantity);

            Define(x => x.Stocks);
        }
    }

    //public class BadStockInventoryValidation : ValidationDef<BadStockInventory>
    //{
    //    public BadStockInventoryValidation()
    //    {
    //        Define(x => x.Id);

    //        Define(x => x.OnHand);

    //        Define(x => x.Product)
    //            .IsValid();

    //        Define(x => x.UnitOfMeasure);

    //        Define(x => x.UnitOfMeasureBase);
    //    }
    //}
}
