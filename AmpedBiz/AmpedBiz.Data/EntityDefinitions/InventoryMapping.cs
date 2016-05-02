using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class InventoryMapping : ClassMap<Inventory>
    {
        public InventoryMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Foreign("Product");

            Map(x => x.ReorderLevel);

            Map(x => x.TargetLevel);

            Map(x => x.MinimumReorderQuantity);

            Map(x => x.Received);

            Map(x => x.OnOrder);

            Map(x => x.Shipped);

            Map(x => x.Allocated);

            Map(x => x.BackOrdered);

            Map(x => x.InitialLevel);

            Map(x => x.OnHand);

            Map(x => x.Available);

            Map(x => x.CurrentLevel);

            Map(x => x.BelowTargetLevel);

            Map(x => x.ReorderQuantity);

            HasOne(x => x.Product).Constrained();

            //HasMany(x => x.Shrinkages)
            //    .Cascade.AllDeleteOrphan()
            //    .Not.KeyNullable()
            //    .Not.KeyUpdate()
            //    .Inverse()
            //    .AsBag();
        }
    }
}
