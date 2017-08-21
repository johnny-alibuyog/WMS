using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services.Products
{
    public class GetAvailableVisitor : IVisitor<Product, Measure>
    {
        private readonly MeasureConverter _converter;
        private readonly UnitOfMeasure _byUnitOfMeasure;

        public GetAvailableVisitor(UnitOfMeasure byUnitOfMeasure)
        {
            this._converter = new MeasureConverter();
            this._byUnitOfMeasure = byUnitOfMeasure;
        }

        public Measure Visit(Product target)
        {
            return this._converter.Convert(target, target.Inventory.Available, this._byUnitOfMeasure);
        }
    }

    public class GetAvailableValueVisitor : IVisitor<Product, decimal>
    {
        private readonly UnitOfMeasure _byUnitOfMeasure;
        private readonly MeasureConverter _converter;

        public GetAvailableValueVisitor(UnitOfMeasure byUnitOfMeasure)
        {
            this._converter = new MeasureConverter();
            this._byUnitOfMeasure = byUnitOfMeasure;
        }

        public decimal Visit(Product target)
        {
            var measure = this._converter.Convert(target, target.Inventory.Available, this._byUnitOfMeasure);

            return measure?.Value ?? 0M;
        }
    }
}
