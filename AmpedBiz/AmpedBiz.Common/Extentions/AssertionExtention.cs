using System;

namespace AmpedBiz.Common.Extentions
{
    //public static class AssertionExtention
    //{
    //    public static void Assert<T>(this T instance, Predicate<T> predicate, string message)
    //    {
    //        if (!predicate(instance))
    //            throw new ArgumentException(message);
    //    }

    //    public static void Assert<T>(this T instance, Predicate<T> predicate, Func<T, string> messageBuilder)
    //    {
    //        if (!predicate(instance))
    //            throw new ArgumentException(messageBuilder(instance));
    //    }

    //    public static void Assert<T>(this T instance, Predicate<T> predicate, Exception exception)
    //    {
    //        if (!predicate(instance))
    //            throw exception;
    //    }
    //}

    public static class Ensure
    {
        public static void That(Func<bool> predicate, string message)
        {
            if (!predicate())
                throw new ArgumentException(message);
        }

        public static void That(Func<bool> predicate, Exception exception)
        {
            if (!predicate())
                throw exception;
        }
    }
}
