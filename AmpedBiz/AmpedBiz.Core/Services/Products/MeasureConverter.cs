using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class MeasureConverter
    {
        public Measure Convert(Product product, Measure measure, UnitOfMeasure toUnit)
        {
            if (toUnit == null)
                return null;

            if (measure == null)
                return new Measure(0M, toUnit);

            Ensure.That(() => !product.IsNullOrDefault(), $"{nameof(product)} should not be null.");

            Ensure.That(() => !measure.Unit.IsNullOrDefault(), $"Product(({nameof(product.Name)})) convertion with value {measure.Value} should have a unit");


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

        public static decimal ConvertValue(this Product product, Measure measure, UnitOfMeasure toUnit)
        {
            var converter = new MeasureConverter();
            var result = converter.Convert(product, measure, toUnit);
            return result.Value;
        }
    }
}
