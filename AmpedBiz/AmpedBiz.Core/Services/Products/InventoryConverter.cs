using AmpedBiz.Core.Entities;
using System;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class InventoryConverter
    {
        private readonly Inventory _inventory;

        private readonly MeasureConverter _converter;

        private readonly Func<Inventory, Measure> _selector;

        public InventoryConverter(Inventory inventory, Func<Inventory, Measure> selector)
        {
            this._selector = selector;
            this._inventory = inventory;
            this._converter = new MeasureConverter();
        }

        public Measure ToStandard()
        {
            var measure = this._selector(this._inventory);

            var standard = this._inventory.Product.UnitOfMeasures.FirstOrDefault(x => x.IsStandard);

            return this._converter.Convert(
                measure: measure,
                product: this._inventory.Product,
                toUnit: standard?.UnitOfMeasure
            );
        }

        public Measure ToDefault()
        {
            var measure = this._selector(this._inventory);

            var @default = _inventory.Product.UnitOfMeasures.FirstOrDefault(x => x.IsDefault);

            return this._converter.Convert(
                measure: measure,
                product: this._inventory.Product,
                toUnit: @default?.UnitOfMeasure
            );
        }

        public Measure To(UnitOfMeasure toUnit)
        {
            var measure = this._selector(this._inventory);

            var selectedUnit = _inventory.Product.UnitOfMeasures.FirstOrDefault(x => x.UnitOfMeasure == toUnit);

            return this._converter.Convert(
                measure: measure,
                product: this._inventory.Product,
                toUnit: selectedUnit?.UnitOfMeasure
            );
        }
    }
}
