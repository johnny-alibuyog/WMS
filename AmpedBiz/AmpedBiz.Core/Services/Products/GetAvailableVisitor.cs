﻿using AmpedBiz.Core.Entities;

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
            return this._converter.Convert(target, target.Inventories.Available, this._byUnitOfMeasure);
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
            var measure = this._converter.Convert(target, target.Inventories.Available, this._byUnitOfMeasure);

            return measure?.Value ?? 0M;
        }
    }
}
