using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Products.Services
{
	public class ProductUnitOfMeasuresUpdateVisitor : IVisitor<Product>
    {
        public IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; set; }

        public ProductUnitOfMeasuresUpdateVisitor(IEnumerable<ProductUnitOfMeasure> unitOfMeasures)
        {
            this.UnitOfMeasures = unitOfMeasures;
        }

        public virtual void Visit(Product target)
        {
            if (this.UnitOfMeasures.IsNullOrEmpty())
                return;

            if (this.UnitOfMeasures.Count() == 1)
            {
                var onlyUnit = this.UnitOfMeasures.First();
                onlyUnit.IsDefault = true;
                onlyUnit.IsStandard = true;
            }

            var unitOfMeasuresToInsert = this.UnitOfMeasures.Except(target.UnitOfMeasures).ToList();
            var unitOfMeasuresToUpdate = target.UnitOfMeasures.Where(x => this.UnitOfMeasures.Contains(x)).ToList();
            var unitOfMeasuresToRemove = target.UnitOfMeasures.Except(this.UnitOfMeasures).ToList();

            foreach (var unitOfMeasure in unitOfMeasuresToInsert)
            {
                unitOfMeasure.Product = target;
                unitOfMeasure.Prices.ForEach(x => x.ProductUnitOfMeasure = unitOfMeasure); // ensure relationship
                target.UnitOfMeasures.Add(unitOfMeasure);
            }

            foreach (var unitOfMeasure in unitOfMeasuresToUpdate)
            {
                var value = this.UnitOfMeasures.Single(x => x == unitOfMeasure);
                unitOfMeasure.Size = value.Size;
                unitOfMeasure.Barcode = value.Barcode;
                unitOfMeasure.UnitOfMeasure = value.UnitOfMeasure;
                unitOfMeasure.StandardEquivalentValue = value.StandardEquivalentValue;
                unitOfMeasure.IsDefault = value.IsDefault;
                unitOfMeasure.IsStandard = value.IsStandard;
                unitOfMeasure.Accept(new ProductUnitOfMeasurePricesUpdateVisitor(value.Prices));
                unitOfMeasure.Product = target;
            }

            foreach (var unitOfMeasure in unitOfMeasuresToRemove)
            {
                unitOfMeasure.Product = null;
                target.UnitOfMeasures.Remove(unitOfMeasure);
            }
        }
    }
}
