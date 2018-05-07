using AmpedBiz.Core.Entities;
using System;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class InventoryMeasureConverter
    {
        private readonly Inventory _inventory;

        private readonly MeasureConverter _converter;

        private readonly Func<Inventory, Measure> _selector;

        public InventoryMeasureConverter(Inventory inventory, Func<Inventory, Measure> selector)
        {
            this._selector = selector;
            this._inventory = inventory;
            this._converter = new MeasureConverter();
        }

        private Measure DoConvertion(Func<ProductUnitOfMeasure, bool> predicate)
        {
            var measure = this._selector(this._inventory);

            var productUnitOfMeasure = this._inventory.Product.UnitOfMeasures.FirstOrDefault(predicate);

            return this._converter.Convert(
                measure: measure,
                product: this._inventory.Product,
                toUnit: productUnitOfMeasure?.UnitOfMeasure
            );
        }

        public Measure To(UnitOfMeasure toUnit) => this.DoConvertion(x => x.UnitOfMeasure == toUnit);

        public decimal ToValue(UnitOfMeasure toUnit) => this.To(toUnit)?.Value ?? 0;
    }
}
