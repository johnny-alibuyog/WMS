﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class UpdateProductUnitOfMeasurePricesVisitor : IVisitor<ProductUnitOfMeasure>
    {
        public IEnumerable<ProductUnitOfMeasurePrice> Prices { get; private set; }

        public UpdateProductUnitOfMeasurePricesVisitor(IEnumerable<ProductUnitOfMeasurePrice> prices)
        {
            this.Prices = prices;
        }

        public void Visit(ProductUnitOfMeasure target)
        {
            this.Prices.ForEach(x => x.ProductUnitOfMeasure = target);

            var itemsToInsert = this.Prices.Except(target.Prices).ToList();
            var itemsToUpdate = target.Prices.Where(x => this.Prices.Contains(x)).ToList();
            var itemsToRemove = target.Prices.Except(this.Prices).ToList();

            foreach (var item in itemsToInsert)
            {
                item.ProductUnitOfMeasure = target;
                target.Prices.Add(item);
            }

            foreach (var item in itemsToUpdate)
            {
                var value = this.Prices.Single(x => x == item);
                item.SerializeWith(value);
                item.ProductUnitOfMeasure = target;
            }

            foreach (var item in itemsToRemove)
            {
                item.ProductUnitOfMeasure = null;
                target.Prices.Remove(item);
            }
        }
    }
}
