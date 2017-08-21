using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories.Orders
{
    public class AllocateVisitor : IVisitor<Inventory>
    {
        public Measure Quantity { get; set; }

        public virtual void Visit(Inventory target)
        {
            if (target.Available < this.Quantity)
            {
                var summary = new
                {
                    Product = target.Product.Name,
                    Unit = Quantity.Unit.Id,
                    OrderValue = (Quantity.Value).ToString("0.##"),
                    TargetValue = (target.Available?.Value ?? 0M).ToString("0.##")
                };

                throw new BusinessException(
                    $"Unable to invoice {summary.Product}." +
                    $"Order quantity is {summary.OrderValue}({summary.Unit}) " + 
                    $"while only {summary.TargetValue}({summary.Unit}) are available."
                );
            }
            
            target.Allocated += Quantity;
            target.Accept(new InventoryRecomputeVisitor());
        }
    }
}
