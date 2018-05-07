using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services.Products
{
    public class InventoryMeasureBreaker
    {
        private readonly Inventory _inventory;

        private readonly MeasureBreaker _breaker;

        private readonly Func<Inventory, Measure> _selector;

        public InventoryMeasureBreaker(Inventory inventory, Func<Inventory, Measure> selector)
        {
            this._selector = selector;
            this._inventory = inventory;
            this._breaker = new MeasureBreaker();
        }

        public IEnumerable<Measure> BreakDown()
        {
            var measure = this._selector(this._inventory);

            return this._breaker.BreakDown(this._inventory.Product, measure);
        }
    }
}
