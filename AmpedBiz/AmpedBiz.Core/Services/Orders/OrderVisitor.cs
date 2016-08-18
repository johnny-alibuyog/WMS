using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Orders
{
    public abstract class OrderVisitor : IVisitor<Order>
    {
        public abstract void Visit(Order target);
    }
}
