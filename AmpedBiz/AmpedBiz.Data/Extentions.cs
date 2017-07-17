using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Data.Configurations;
using NHibernate;
using NHibernate.Context;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data
{
    public static class Extentions
    {
        public static ISession RetrieveSharedSession(this ISessionFactory sessionFactory)
        {
            if (!CurrentSessionContext.HasBind(sessionFactory))
            {
                CurrentSessionContext.Bind(sessionFactory.OpenSession());
            }

            if (!sessionFactory.GetCurrentSession().IsConnected ||
                !sessionFactory.GetCurrentSession().IsOpen)
            {
                CurrentSessionContext.Unbind(sessionFactory);
                CurrentSessionContext.Bind(sessionFactory.OpenSession());
            }

            return sessionFactory.GetCurrentSession();
        }

        public static ISession ReleaseSharedSession(this ISessionFactory sessionFactory)
        {
            return CurrentSessionContext.Unbind(sessionFactory);
        }

        // this was created since NotNull is validated by nhibernate before on pre insert/update is triggered. 
        // this is just a work around. if NotNull will be captured by event listeners, this will be removed soon
        public static void EnsureValidity(this object entity)
        {
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
