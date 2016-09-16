using System;

namespace AmpedBiz.Common.Extentions
{
    public static class NumberExtention
    {
        //private static decimal _scale = 2; //29;
        //private static decimal _whole = 7;

        public static bool HasFraction(this double value)
        {
            return Math.Floor(value) != value;

            //return value % 1 == 0;
        }

        public static bool HasFraction(this decimal value)
        {
            return Math.Floor(value) != value;

            //return value % 1 == 0;
        }

        public static int NextInt32(this Random random)
        {
            unchecked
            {
                var firstBits = random.Next(0, 1 << 4) << 28;
                var lastBits = random.Next(0, 1 << 28);
                return firstBits | lastBits;
            }
        }

        public static decimal NextDecimal(this Random random)
        {
            var scale = (byte)random.Next(29);
            var isNegative = random.Next(2) == 1;

            return new decimal(
                lo: random.NextInt32(),
                mid: random.NextInt32(),
                hi: random.NextInt32(),
                isNegative: isNegative,
                scale: scale
            );
        }

        //public static decimal NextDecimal(this Random random, decimal min, decimal max)
        //{
        //    return (random.NextDecimal() * Math.Abs(max - min)) + min;
        //}

        public static decimal NextDecimal(this Random random, decimal from, decimal to)
        {
            var fromScale = new System.Data.SqlTypes.SqlDecimal(from).Scale;
            var toScale = new System.Data.SqlTypes.SqlDecimal(to).Scale;

            var scale = (byte)(fromScale + toScale);
            if (scale > 28)
                scale = 28;

            var randomValue = new decimal(random.Next(), random.Next(), random.Next(), false, scale);
            if (Math.Sign(from) == Math.Sign(to) || from == 0 || to == 0)
                return decimal.Remainder(randomValue, to - from) + from;

            var valueFromNegativeRange = (double)from + random.NextDouble() * ((double)to - (double)from) < 0;
            return valueFromNegativeRange ? decimal.Remainder(randomValue, -from) + from : decimal.Remainder(randomValue, to);
        }

        public static decimal ZeroIfNull(this decimal? value)
        {
            return value ?? 0M;
        }
    }
}
