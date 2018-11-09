using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.Inventories.Services
{
    public class InventoryAdjustVisitor : IVisitor<Inventory>
    {
        private readonly InventoryAdjustment _adjustment;

        public InventoryAdjustVisitor(
            User adjustedBy, 
            DateTime adjustedOn, 
            InventoryAdjustmentReason reason, 
            string remarks,
            Measure quantity,
            Measure standard)
        {
            this._adjustment = new InventoryAdjustment(
                adjustedBy: adjustedBy,
                adjustedOn: adjustedOn,
                reason: reason,
                remarks: remarks,
                type: reason.Type,
                quantity: quantity,
                standard: standard
            );
        }

        public void Visit(Inventory target)
        {
            this.Validate(target);
            this.Adjust(target);
        }

        private void Validate(Inventory target)
        {
            if (this._adjustment.Type == InventoryAdjustmentType.Decrease)
            {
                if (this._adjustment.QuantityStandardEquivalent > target.OnHand)
                {
                    var message = $"You cannot deduct if on hand is less than adjustment. " +
                        $"On Hand: {target.OnHand.ToStringWithSymbol()}; Adjustment: {this._adjustment.QuantityStandardEquivalent.ToStringWithSymbol()}";
                    throw new BusinessException(message);
                }
            }
        }

        private void Adjust(Inventory target)
        {
            var quantity = this._adjustment.QuantityStandardEquivalent;
            if (this._adjustment.Type == InventoryAdjustmentType.Increase)
            {
                target.CurrentLevel += quantity;
                target.OnHand += quantity;
                target.IncreaseAdjustment += quantity;
            }
            else
            {
                target.CurrentLevel -= quantity;
                target.OnHand -= quantity;
                target.DecreaseAdjustment += quantity;
            }
            this._adjustment.Inventory = target;
            target.Adjustments.Add(this._adjustment);
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}
