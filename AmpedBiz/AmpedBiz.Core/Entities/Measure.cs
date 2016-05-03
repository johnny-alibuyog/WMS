using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Entities
{
    public class Measure : ValueObject<Measure>
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
}
