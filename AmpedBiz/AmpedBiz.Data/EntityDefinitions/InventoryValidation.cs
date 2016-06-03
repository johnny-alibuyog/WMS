using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class GoodStockInventoryValidation : ValidationDef<GoodStockInventory>
    {
        public GoodStockInventoryValidation()
        {
            Define(x => x.Id);

            Define(x => x.ReorderLevel);

            Define(x => x.TargetLevel);

            Define(x => x.MinimumReorderQuantity);

            Define(x => x.Received);

            Define(x => x.OnOrder);

            Define(x => x.Shipped);

            Define(x => x.Allocated);

            Define(x => x.BackOrdered);

            Define(x => x.InitialLevel);

            Define(x => x.OnHand);

            Define(x => x.Available);

            Define(x => x.CurrentLevel);

            Define(x => x.BelowTargetLevel);

            Define(x => x.ReorderQuantity);

            Define(x => x.Product)
                .IsValid();

            Define(x => x.Shrinkages)
                .HasValidElements();
        }
    }

    public class BadStockInventoryValidation : ValidationDef<BadStockInventory>
    {
        public BadStockInventoryValidation()
        {
            Define(x => x.Id);

            Define(x => x.OnHand);

            Define(x => x.Product)
                .IsValid();
        }
    }
}
