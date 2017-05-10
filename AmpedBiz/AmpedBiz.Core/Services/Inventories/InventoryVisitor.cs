using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories
{
    public abstract class InventoryVisitor : IVisitor<Inventory>
    {
        public abstract void Visit(Inventory target);
    }
}
