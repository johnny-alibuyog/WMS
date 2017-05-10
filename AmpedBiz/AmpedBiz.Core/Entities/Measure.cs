using System;
using System.Collections.Generic;

namespace AmpedBiz.Core.Entities
{
    public class Measure : ValueObject<Measure>, IComparable<Measure>
    {
        public virtual decimal Value { get; set; }

        public virtual UnitOfMeasure Unit { get; set; }

        public Measure() : this(0M, null) { }

        public Measure(decimal value, UnitOfMeasure unit)
        {
            this.Value = value;
            this.Unit = unit;
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }

        public virtual string ToStringWithSymbol()
        {
            return $"{this.Value.ToString("0.##")} {this.Unit}";
        }

        public int CompareTo(Measure that)
        {
            if (that == null)
                return 1;

            if (this.Unit != null && that.Unit != null && this.Unit != that.Unit)
                throw new InvalidOperationException($"You cannot compare measure of unit {this.Unit.Name} to {that.Unit.Name}");

            return this.Value.CompareTo(that.Value);
        }

        public static bool operator >(Measure operand1, Measure operand2)
        {
            return operand1.CompareTo(operand2) == 1;
        }

        public static bool operator <(Measure operand1, Measure operand2)
        {
            return operand1.CompareTo(operand2) == -1;
        }

        public static bool operator >=(Measure operand1, Measure operand2)
        {
            return operand1.CompareTo(operand2) >= 0;
        }

        public static bool operator <=(Measure operand1, Measure operand2)
        {
            return operand1.CompareTo(operand2) <= 0;
        }

        public static Measure operator +(Measure value1, Measure value2)
        {
            if (value1 == null && value2 == null)
                return null;

            var unit = value1?.Unit ?? value2?.Unit;
            if (value1 == null)
                value1 = new Measure(0M, unit);

            if (value2 == null)
                value2 = new Measure(0M, unit);

            return new Measure(value1.Value + value2.Value, unit);
        }

        public static Measure operator -(Measure value1, Measure value2)
        {
            if (value1 == null && value2 == null)
                return null;

            var unit = value1?.Unit ?? value2?.Unit;
            if (value1 == null)
                value1 = new Measure(0M, unit);

            if (value2 == null)
                value2 = new Measure(0M, unit);

            return new Measure(value1.Value - value2.Value, unit);
        }

        //public static Measure operator <()

        //public static Measure operator *(Measure value1, Measure value2)
        //{
        //    if (value1 == null && value2 == null)
        //        return null;

        //    var unit = value1?.Unit ?? value2?.Unit;
        //    if (value1 == null)
        //        value1 = new Measure(0M, unit);

        //    if (value2 == null)
        //        value2 = new Measure(0M, unit);

        //    return new Measure(value1.Value * value2.Value, unit);
        //}


        //public static Measure operator /(Measure value1, Measure value2)
        //{
        //    if (value1 == null && value2 == null)
        //        return null;

        //    var unit = value1?.Unit ?? value2?.Unit;
        //    if (value1 == null)
        //        value1 = new Measure(0M, unit);

        //    if (value2 == null)
        //        value2 = new Measure(0M, unit);

        //    return new Measure(value1.Value / value2.Value, unit);
        //}
    }

    public static class MeasureExtention
    {
        public static decimal Value(this Measure measure, decimal? defaultValue = 0M)
        {
            if (measure == null)
            {
                return defaultValue ?? 0M;
            }

            return measure.Value;
        }

        public static Measure Sum<T>(this IEnumerable<T> items, Func<T, Measure> selector)
        {
            var sum = default(Measure);
            foreach (var item in items)
            {
                sum += selector(item);
            }
            return sum;
        }
    }
}
