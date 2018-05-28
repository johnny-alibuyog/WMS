using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Core.Services.Products
{
    public class MeasureBreaker
    {
        public IEnumerable<Measure> BreakDown(Product product, Measure measure)
        {
            var brokenDownMeasures = new List<Measure>();

            var unit = new
            {
                Standard = product.UnitOfMeasures.Standard(x => x),
                Default = product.UnitOfMeasures.Default(x => x),
            };

            if (measure == null)
            {
                measure = new Measure(0, unit.Standard.UnitOfMeasure);
            }

            if (product.UnitOfMeasures.Count() == 1)
            {
                brokenDownMeasures.Add(product.Convert(measure, unit.Standard.UnitOfMeasure));
            }
            else
            {
                var defaultMeasure = product.ConvertToDefault(measure);

                var defaultParts = new
                {
                    Integral = new Measure(Math.Truncate(defaultMeasure.Value), defaultMeasure.Unit),
                    Fraction = new Measure(defaultMeasure.Value - Math.Truncate(defaultMeasure.Value), defaultMeasure.Unit)
                };

                var breakDownParts = new
                {
                    Default = product.Convert(defaultParts.Integral, unit.Default.UnitOfMeasure),
                    Standard = product.Convert(defaultParts.Fraction, unit.Standard.UnitOfMeasure)
                };

                brokenDownMeasures.AddRange(new[] { breakDownParts.Default, breakDownParts.Standard });
            }

            return brokenDownMeasures;
        }
    }

    public static class MeasureBreakerExtention
    {
        public static IEnumerable<Measure> BreakDown(this Measure measure, Product product)
        {
            return new MeasureBreaker().BreakDown(product, measure);
        }

        public static Measure TakePart(this Measure measure, Product product, UnitOfMeasure part)
        {
            return new MeasureBreaker().BreakDown(product, measure).First(x => x.Unit == part);
        }

        public static decimal? TakePartValue(this Measure measure, Product product, UnitOfMeasure part)
        {
            return TakePart(measure, product, part)?.Value;
        }

        public static string InterpretAsString(this IEnumerable<Measure> measures)
        {
            return string.Join(" & ", measures.Select(x => x.ToStringIntegral()));
        }
    }
}
