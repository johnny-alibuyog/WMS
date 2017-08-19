using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Service.Dto
{
    public class ProductUnitOfMeasure
    {
        public virtual Guid Id { get; set; }

        public string Size { get; set; }

        public UnitOfMeasure UnitOfMeasure { get; set; }

        public decimal? StandardEquivalentValue { get; set; }

        public bool? IsStandard { get; set; }

        public bool? IsDefault { get; set; }

        public IEnumerable<ProductUnitOfMeasurePrice> Prices { get; set; } = new Collection<ProductUnitOfMeasurePrice>();
    }
}
