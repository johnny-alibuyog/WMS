using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class ProductUpdateUnitOfMeasuresVisitor : ProductVisitor
    {
        public IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; set; }

        public ProductUpdateUnitOfMeasuresVisitor(IEnumerable<ProductUnitOfMeasure> unitOfMeasures)
        {
            this.UnitOfMeasures = unitOfMeasures;
        }

        public override void Visit(Product target)
        {
            if (this.UnitOfMeasures.IsNullOrEmpty())
                return;

            var itemsToInsert = this.UnitOfMeasures.Except(target.UnitOfMeasures).ToList();
            var itemsToUpdate = target.UnitOfMeasures.Where(x => this.UnitOfMeasures.Contains(x)).ToList();
            var itemsToRemove = target.UnitOfMeasures.Except(this.UnitOfMeasures).ToList();

            foreach (var item in itemsToInsert)
            {
                item.Product = target;
                target.UnitOfMeasures.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.UnitOfMeasures.Single(x => x == item);
                item.SerializeWith(value);
                item.Product = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.Product = null;
                target.UnitOfMeasures.Remove(item);
            }
        }
    }
}
