using AmpedBiz.Core.SharedKernel;

namespace AmpedBiz.Core.Inventories.Services
{
	public class InventoryRecomputeVisitor : IVisitor<Inventory>
    {
        public virtual void Visit(Inventory target)
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
