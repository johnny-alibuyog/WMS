using System;
using System.Linq;

namespace AmpedBiz.Common.Extentions
{
    public static class TypeExtentions
    {
        public static string ParseSchema(this Type type)
        {
            return type.Namespace.Split('.').Last();
        }

        public static bool IsNullOrDefault<T>(this T argument)
        {
            // deal with normal scenarios
            if (argument == null)
                return true;

            if (object.Equals(argument, default(T)))
                return true;

            // deal with non-null nullables
            var nullableType = typeof(T);
            if (Nullable.GetUnderlyingType(nullableType) != null)
                return false;

            // deal with boxed value types
            var valueType = argument.GetType();
            if (valueType.IsValueType && valueType != nullableType)
            {
                object obj = Activator.CreateInstance(argument.GetType());
                return obj.Equals(argument);
            }

            return false;
        }

        public static T Coalesce<T>(params T[] vaules)
        {
            var defaultValue = vaules
                .Where(x => !x.IsNullOrDefault())
                .FirstOrDefault();

            return defaultValue;
        }
    }
}
