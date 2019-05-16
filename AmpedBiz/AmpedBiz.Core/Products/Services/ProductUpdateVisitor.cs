using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;

namespace AmpedBiz.Core.Products.Services
{
    public class ProductUpdateVisitor : IVisitor<Product>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory Category { get; set; }

        public string Image { get; set; }

        public bool? Discontinued { get; set; }

        public Inventory Inventory { get; set; }

        public IEnumerable<Supplier> Suppliers { get; set; }

        public IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; set; }

        public virtual void Visit(Product target)
        {
            target.Code = this.Code ?? target.Code;
            target.Name = this.Name ?? target.Name;
            target.Description = this.Description ?? target.Description;
            target.Category = this.Category ?? target.Category;
            target.Image = this.Image ?? target.Image;
            target.Discontinued = this.Discontinued ?? target.Discontinued;
            target.Accept(new ProductSuppliersUpdateVisitor(this.Suppliers));
            target.Accept(new ProductUnitOfMeasuresUpdateVisitor(this.UnitOfMeasures));
        }
    }
}
