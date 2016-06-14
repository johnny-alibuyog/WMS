﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Common.Extentions
{
    public static class EnumExtention
    {
        public static List<KeyValuePair<int, string>> ToLookup<TEnum>() where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>()
                .Select(x => new KeyValuePair<int, string>(
                    key: Convert.ToInt32(x), 
                    value: x.ToString()
                ))
                .ToList();
        }

        public static TEnum Parse<TEnum>(this object value) //where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("TEnum must be an enumerated type");

            if (value.IsNullOrDefault())
                return default(TEnum);

            if (string.IsNullOrWhiteSpace(value.ToString()))
                return default(TEnum);

            var result = (TEnum)Enum.Parse(typeof(TEnum), value.ToString(), true);

            return result;
        }
    }
}
