﻿using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.PurchaseOrders
{
    public class ReceiveVisitor : IVisitor<Inventory>
    {
        public virtual Measure Quantity { get; set; }

        public virtual void Visit(Inventory target)
        {
            target.OnOrder -= this.Quantity;
            target.OnHand += this.Quantity;
            target.Received += this.Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}
