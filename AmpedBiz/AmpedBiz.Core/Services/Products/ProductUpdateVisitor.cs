using AmpedBiz.Core.Entities;
using System.Collections.Generic;

namespace AmpedBiz.Core.Services.Products
{
    public class ProductUpdateVisitor : ProductVisitor
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Supplier Supplier { get; set; }

        public ProductCategory Category { get; set; }

        public string Image { get; set; }

        public bool? Discontinued { get; set; }

        public Inventory Inventory { get; set; }

        public IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; set; }

        public override void Visit(Product target)
        {
            target.Code = this.Code ?? target.Code;
            target.Name = this.Name ?? target.Name;
            target.Description = this.Description ?? target.Description;
            target.Supplier = this.Supplier ?? target.Supplier;
            target.Category = this.Category ?? target.Category;
            target.Image = this.Image ?? target.Image;
            target.Discontinued = this.Discontinued ?? target.Discontinued;
            target.Accept(new UpdateProductUnitOfMeasuresVisitor(this.UnitOfMeasures));
        }
    }
}
