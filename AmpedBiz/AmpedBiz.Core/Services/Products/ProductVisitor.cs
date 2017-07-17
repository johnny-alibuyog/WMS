using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Products
{
    public abstract class ProductVisitor : IVisitor<Product>
    {
        public abstract void Visit(Product target);
    }
}
