using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.EntityDefinitions;
using NHibernate;
using NHibernate.Context;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data
{
    public static class Extentions
    {
        private static ISession WithFilters(this ISession session, IContext context = null)
        {
            if (context != null)
            {
                if (!string.IsNullOrWhiteSpace(context.TenantId))
                    session.ApplyTenantFilter(context.TenantId);

                if (context.BranchId != Guid.Empty)
                    session.ApplyBranchFilter(context.BranchId);
            }

            return session;
        }

        public static ISession RetrieveSharedSession(this ISessionFactory sessionFactory, IContext context = null)
        {
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                CurrentSessionContext.Bind(sessionFactory.OpenSession().WithFilters(context));
            }

            if (!sessionFactory.GetCurrentSession().IsConnected ||
                !sessionFactory.GetCurrentSession().IsOpen)
            {
                CurrentSessionContext.Unbind(sessionFactory);
                CurrentSessionContext.Bind(sessionFactory.OpenSession().WithFilters(context));
            }

            return sessionFactory.GetCurrentSession();
        }

        public static ISession ReleaseSharedSession(this ISessionFactory sessionFactory)
        {
            return CurrentSessionContext.Unbind(sessionFactory);
        }

        public static void EnsureValidity(this object entity)
        {
            // this was created since NotNull is validated by nhibernate before on pre insert/update is triggered. 
            // this is just a work around. if NotNull will be captured by event listeners, this will be removed soon
            new ValidationEventListener().PerformValidation(entity);
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

        public static TValue GetValueSafely<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (key.IsNullOrDefault())
                return default(TValue);

            if (source == null)
                return default(TValue);

            if (source.ContainsKey(key) != true)
                return default(TValue);

            return source[key];
        }

        public static void EnsureExistence<T>(this T entity, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = $"{typeof(T).Name} does not exists";
            }

            if (entity == null)
            {
                throw new ResourceNotFoundException(message);
            }
        }

        public static void EnsureExistence<T>(this IEnumerable<T> entities, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                message = $"{typeof(T).Name} does not exists";
            }

            if (entities == null || !entities.Any())
            {
                throw new ResourceNotFoundException(message);
            }
        }
    }
}
