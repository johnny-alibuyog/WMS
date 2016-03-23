using System.Collections.Generic;

namespace AmpedBiz.Common.Extentions
{
    public static class EnumerableExtention
    {
        public static ICollection<T> AsCollection<T>(this IEnumerable<T> items)
        {
            return items as ICollection<T>;
        }

        public static void Add<T>(this IEnumerable<T> items, T value)
        {
            ((ICollection<T>)items).Add(value);
        }

        public static void Remove<T>(this IEnumerable<T> items, T value)
        {
            ((ICollection<T>)items).Remove(value);
        }
    }
}
