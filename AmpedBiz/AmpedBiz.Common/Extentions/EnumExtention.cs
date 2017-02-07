using AmpedBiz.Common.CustomTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Common.Extentions
{
    public static class EnumExtention
    {
        public static List<Lookup<TEnum>> ToLookup<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(x => new Lookup<TEnum>(x, x.ToString()))
                .ToList();
        }

        public static TEnum As<TEnum>(this object value) //where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            if (value.IsNullOrDefault())
                return default(TEnum);

            if (string.IsNullOrWhiteSpace(value.ToString()))
                return default(TEnum);

            try
            {
                var result = (TEnum)Enum.Parse(typeof(TEnum), value.ToString(), true);
                return result;
            }
            catch 
            {
                throw new Exception($"{value} is an invalid {typeof(TEnum).Name}. Please check check your status configuration.");
            }
        }
    }
}
