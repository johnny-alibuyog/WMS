using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class UpdateProductUnitOfMeasuresVisitor : ProductVisitor
    {
        public IEnumerable<ProductUnitOfMeasure> UnitOfMeasures { get; set; }

        public UpdateProductUnitOfMeasuresVisitor(IEnumerable<ProductUnitOfMeasure> unitOfMeasures)
        {
            this.UnitOfMeasures = unitOfMeasures;
        }

        public override void Visit(Product target)
        {
            if (this.UnitOfMeasures.IsNullOrEmpty())
                return;

            var unitOfMeasuresToInsert = this.UnitOfMeasures.Except(target.UnitOfMeasures).ToList();
            var unitOfMeasuresToUpdate = target.UnitOfMeasures.Where(x => this.UnitOfMeasures.Contains(x)).ToList();
            var unitOfMeasuresToRemove = target.UnitOfMeasures.Except(this.UnitOfMeasures).ToList();

            foreach (var unitOfMeasure in unitOfMeasuresToInsert)
            {
                unitOfMeasure.Product = target;
                unitOfMeasure.Accept(new UpdateProductUnitOfMeasurePricesVisitor(unitOfMeasure.Prices));
                target.UnitOfMeasures.Add(unitOfMeasure);
            }

            foreach (var unitOfMeasure in unitOfMeasuresToUpdate)
            {
                var value = this.UnitOfMeasures.Single(x => x == unitOfMeasure);
                unitOfMeasure.Size = value.Size;
                unitOfMeasure.UnitOfMeasure = value.UnitOfMeasure;
                unitOfMeasure.StandardEquivalentValue = value.StandardEquivalentValue;
                unitOfMeasure.IsDefault = value.IsDefault;
                unitOfMeasure.IsStandard = value.IsStandard;
                unitOfMeasure.Accept(new UpdateProductUnitOfMeasurePricesVisitor(value.Prices));
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
