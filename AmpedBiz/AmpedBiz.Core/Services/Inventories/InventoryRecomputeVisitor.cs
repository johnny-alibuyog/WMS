using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories
{
    public class InventoryRecomputeVisitor : InventoryVisitor
    {
        public override void Visit(Inventory target)
        {
            var calculator = new InventoryCalculator();
            target.OnHand = calculator.OnHand(target);
            target.Available = calculator.Available(target);
            target.CurrentLevel = calculator.CurrentLevel(target);
            target.BelowTargetLevel = calculator.BelowTargetLevel(target);
            target.ReorderQuantity = calculator.ReorderQuantity(target);
        }
    }
}
