using AmpedBiz.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Common.Extentions
{
    //public class Ensure1<T>
    //{
    //    private T _instance;

    //    private Func<T, bool> _predicate;

    //    internal Ensure1(T instance)
    //    {
    //        this._instance = instance;
    //    }

    //    public Ensure1<T> That(Func<T, bool> predicate)
    //    {
    //        this._predicate = predicate;
    //        return this;
    //    }

    //    public void ElseThrow(string message)
    //    {
    //        if (!this._predicate(this._instance))
    //            throw new ArgumentException(message);
    //    }

    //    public void ElseThrow(Func<T, string> messageBuilder)
    //    {
    //        if (!this._predicate(this._instance))
    //            throw new ArgumentException(messageBuilder(this._instance));
    //    }

    //    public void ElseThrow(Exception exception)
    //    {
    //        if (!this._predicate(this._instance))
    //            throw exception;
    //    }
    //}

    //public static class Ensure1Extention
    //{
    //    public static Ensure1<T> Ensure<T>(this T instance)
    //    {
    //        return new Ensure1<T>(instance);
    //    }
    //}

    //public static class Ensure
    //{
    //    public static void That(Func<bool> predicate, string message)
    //    {
    //        if (!predicate())
    //            throw new ArgumentException(message);
    //    }

    //    public static void That(Func<bool> predicate, Exception exception)
    //    {
    //        if (!predicate())
    //            throw exception;
    //    }
    //}

    public static class EnsureExtention
    {
        public static T Ensure<T>(this T instance, Predicate<T> that, string message)
        {
            if (!that(instance))
                throw new BusinessException(message);

            return instance;
        }

        public static T Ensure<T>(this T instance, Predicate<T> that, Func<T, string> messageBuilder)
        {
            if (!that(instance))
                throw new BusinessException(messageBuilder(instance));

            return instance;
        }

        public static T Ensure<T>(this T instance, Predicate<T> that, Exception exception)
        {
            if (!that(instance))
                throw exception;

            return instance;
        }

        public static void Assert(this bool exists, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = "Already exists.";
            }

            if (exists)
            {
                throw new ResourceNotFoundException(message);
            }
        }


        public static T EnsureExistence<T>(this T entity, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = $"{typeof(T).Name} does not exists";
            }

            entity.Ensure(that: instance => !instance.IsNullOrDefault(), exception: new ResourceNotFoundException(message));

            return entity;
        }

        public static IEnumerable<T> EnsureExistence<T>(this IEnumerable<T> entities, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = $"{typeof(T).Name} does not exists";
            }

            entities.Ensure(that: instance => !instance.IsNullOrEmpty(), exception: new ResourceNotFoundException(message));

            return entities;
        }
    }
}
