using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class AllocateVisitor : InventoryVisitor
    {
        public Measure Quantity { get; set; }

        public override void Visit(Inventory target)
        {
            if (target.Available < this.Quantity)
            {
                throw new BusinessException(
                    $"Unable to invoice {target.Product.Name}." +
                    $"Order quantity is {Quantity.Value} {Quantity.Unit.Id} " + 
                    $"while only {target.Available.Value} {target.Available.Unit.Id} are available."
                );
            }
            
            target.Allocated += Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}
