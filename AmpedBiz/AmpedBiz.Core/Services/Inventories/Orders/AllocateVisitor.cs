﻿using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class AllocateVisitor : InventoryVisitor
    {
        public Measure Quantity { get; set; }

        public override void Visit(Inventory target)
        {
            target.Allocated += Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}