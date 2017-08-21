using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class MeasureConverter
    {
        public Measure Convert(Product product, Measure measure, UnitOfMeasure toUnit)
        {
            Ensure.That(() => product.IsNullOrDefault(), $"{nameof(product)} should not be null");
            Ensure.That(() => product.UnitOfMeasures.IsNullOrEmpty(), $"{nameof(product.UnitOfMeasures)} of {product.Name} should not be null or empty.");
            Ensure.That(() => measure.IsNullOrDefault(), $"{nameof(measure)} should not be null");
            Ensure.That(() => measure.Unit.IsNullOrDefault(),$"{nameof(measure.Unit)} should not be null");
            Ensure.That(() => toUnit.IsNullOrDefault(), $"{nameof(toUnit)} should not be null");

            var fromSev = product.UnitOfMeasures
                .Where(x => x.UnitOfMeasure == measure.Unit)
                .Select(x => x.StandardEquivalentValue)
                .FirstOrDefault();

            var toSev = product.UnitOfMeasures
                .Where(x => x.UnitOfMeasure == toUnit)
                .Select(x => x.StandardEquivalentValue)
                .FirstOrDefault();

            var toValue = measure.Value * fromSev / toSev;

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
