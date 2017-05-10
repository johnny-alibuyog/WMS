using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Inventories
{
    public abstract class ProductVisitor : IVisitor<Product>
    {
        public abstract void Visit(Product target);
    }
}
