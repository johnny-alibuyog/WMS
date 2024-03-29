﻿using AmpedBiz.Common.Extentions;
using System.Linq;

namespace AmpedBiz.Core.Products.Services
{
	public class MeasureConverter
    {
        public Measure Convert(Product product, Measure measure, UnitOfMeasure toUnit)
        {
            if (toUnit == null)
                return null;

            if (measure == null)
                return new Measure(0M, toUnit);

            product.Ensure(
                that: instance => !instance.IsNullOrDefault(),
                message: $"{nameof(product)} should not be null."
            );

            measure.Ensure(
                that: instance => !instance.Unit.IsNullOrDefault(),
                message: $"Product(({nameof(product.Name)})) convertion with value {measure.Value} should have a unit"
            );

            var fromStandardEquivalentValue = product.UnitOfMeasures
                .Where(x => x.UnitOfMeasure == measure.Unit)
                .Select(x => x.StandardEquivalentValue)
                .FirstOrDefault();

            var toStandardEquivalentValue = product.UnitOfMeasures
                .Where(x => x.UnitOfMeasure == toUnit)
                .Select(x => x.StandardEquivalentValue)
                .FirstOrDefault();

            var toValue = measure.Value * fromStandardEquivalentValue / toStandardEquivalentValue;

            return new Measure(value: toValue, unit: toUnit);
        }
    }

    public static class MeasureConverterExtention
    {
        public static Measure Convert(this Product product, Measure measure, UnitOfMeasure toUnit)
        {
            var converter = new MeasureConverter();
            return converter.Convert(product, measure, toUnit);
        }

        public static Measure ConvertToDefault(this Product product, Measure measure)
        {
            var uom = product.UnitOfMeasures.Default(x => x.UnitOfMeasure);
            return product.Convert(measure, uom);
        }

        public static Measure ConvertToStandard(this Product product, Measure measure)
        {
            var uom = product.UnitOfMeasures.Standard(x => x.UnitOfMeasure);
            return product.Convert(measure, uom);
        }

        public static decimal ConvertValue(this Product product, Measure measure, UnitOfMeasure toUnit)
        {
            var converter = new MeasureConverter();
            var result = converter.Convert(product, measure, toUnit);
            return result.Value;
        }

        public static decimal ConvertToDefaultValue(this Product product, Measure measure)
        {
            var uom = product.UnitOfMeasures.Default(x => x.UnitOfMeasure);
            return product.ConvertValue(measure, uom);
        }

        public static decimal ConvertToStandardValue(this Product product, Measure measure)
        {
            var uom = product.UnitOfMeasures.Standard(x => x.UnitOfMeasure);
            return product.ConvertValue(measure, uom);
        }
    }
}
