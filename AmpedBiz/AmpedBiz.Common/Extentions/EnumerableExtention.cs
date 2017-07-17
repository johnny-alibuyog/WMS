using System;
using System.Collections.Generic;
using System.Linq;

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

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action.Invoke(item);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> items)
        {
            if (items == null)
                return true;

            return !items.Any();
        }
            
        public static T RandomElement<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.RandomElementUsing(new Random());
        }

        public static T RandomElementUsing<T>(this IEnumerable<T> enumerable, Random rand)
        {
            int index = rand.Next(0, enumerable.Count());
            return enumerable.ElementAt(index);
        }
    }
}
