using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AmpedBiz.Service.Dto
{
    public class ProductUnitOfMeasure
    {
        public virtual Guid Id { get; set; }

        public virtual Product Product { get; set; }

        public virtual UnitOfMeasure UnitOfMeasure { get; set; }

        public virtual decimal? StandardEquivalentValue { get; set; }

        public virtual bool? IsStandard { get; set; }

        public virtual bool? IsDefault { get; set; }

        public virtual IEnumerable<ProductUnitOfMeasurePrice> Prices { get; set; } = new Collection<ProductUnitOfMeasurePrice>();
    }
}
