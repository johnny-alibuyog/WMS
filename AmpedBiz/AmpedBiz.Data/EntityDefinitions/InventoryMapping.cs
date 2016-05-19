﻿using System;
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

            References(x => x.UnitOfMeasure);

            References(x => x.UnitOfMeasureBase);

            Map(x => x.ConvertionFactor);

            Component(x => x.ReorderLevel, MeasureMapping.Map("ReorderLevel_", nameof(Inventory)));

            Component(x => x.TargetLevel, MeasureMapping.Map("TargetLevel_", nameof(Inventory)));

            Component(x => x.MinimumReorderQuantity, MeasureMapping.Map("MinimumReorderQuantity_", nameof(Inventory)));

            Component(x => x.Received, MeasureMapping.Map("Received_", nameof(Inventory)));

            Component(x => x.OnOrder, MeasureMapping.Map("OnOrder_", nameof(Inventory)));

            Component(x => x.Shipped, MeasureMapping.Map("Shipped_", nameof(Inventory)));

            Component(x => x.Allocated, MeasureMapping.Map("Allocated_", nameof(Inventory)));

            Component(x => x.BackOrdered, MeasureMapping.Map("BackOrdered_", nameof(Inventory)));

            Component(x => x.InitialLevel, MeasureMapping.Map("InitialLevel_", nameof(Inventory)));

            Component(x => x.OnHand, MeasureMapping.Map("OnHand_", nameof(Inventory)));

            Component(x => x.Available, MeasureMapping.Map("Available_", nameof(Inventory)));

            Component(x => x.CurrentLevel, MeasureMapping.Map("CurrentLevel_", nameof(Inventory)));

            Component(x => x.BelowTargetLevel, MeasureMapping.Map("BelowTargetLevel_", nameof(Inventory)));

            Component(x => x.ReorderQuantity, MeasureMapping.Map("ReorderQuantity_", nameof(Inventory)));

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