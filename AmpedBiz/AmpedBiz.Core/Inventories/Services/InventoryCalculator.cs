using AmpedBiz.Core.Products;

namespace AmpedBiz.Core.Inventories.Services
{
    public class InventoryCalculator
    {
        private Measure EnsureNonNegative(Measure target)
        {
            if (target == null)
                return null;

            if (target.Value < 0M)
                target.Value = 0M;

            return target;
        }

        public Measure OnHand(Inventory target)
        {
            var onHand = (target.OnHand == null)
                ? target.InitialLevel
                : target.OnHand;

            return onHand;
        }

        public Measure Available(Inventory target)
        {
            var available = target.OnHand - target.Allocated;
            return available;
        }

        public Measure CurrentLevel(Inventory target)
        {
            var currentLevel = target.Available + target.OnOrder - target.BackOrdered;
            return currentLevel;
        }

        public Measure BelowTargetLevel(Inventory target)
        {
            var belowTargetLevel = target.TargetLevel - target.CurrentLevel;
            return this.EnsureNonNegative(belowTargetLevel);
        }

        public Measure ReorderQuantity(Inventory target)
        {
            if (target.ReorderLevel < target.CurrentLevel)
                return null;

            var reorderQuantity = (target.BelowTargetLevel < target.MinimumReorderQuantity)
                ? target.MinimumReorderQuantity
                : target.BelowTargetLevel;

            return reorderQuantity;
        }
    }
}
