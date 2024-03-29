﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Definitions.Common;
using NHibernate;
using NHibernate.Context;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AmpedBiz.Data
{
	public static class Extentions
    {
        private static ISession WithFilters(this ISession session, IContext context = null)
        {
            if (context != null)
            {
                if (!string.IsNullOrWhiteSpace(context.TenantId))
                    session.EnableTenantFilter(context.TenantId);

                if (context.BranchId != Guid.Empty)
                    session.EnableBranchFilter(context.BranchId);
            }

            return session;
        }

        public static void DisableFilter(this ISession session, string filterName)
        {
            session.DisableFilter(filterName);
        }

        public static ISession RetrieveSharedSession(this ISessionFactory sessionFactory, IContext context = null)
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

            return sessionFactory.GetCurrentSession().WithFilters(context);
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

        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            if (key.IsNullOrDefault())
                return default(TValue);

            if (source == null)
                return default(TValue);

            if (source.ContainsKey(key) != true)
                return default(TValue);

            return source[key];
        }

		// https://github.com/nhibernate/nhibernate-core/issues/1123
		public static IFutureValue<int> GroupByCount<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector)
		{
			return source.GroupBy(keySelector)
				.Select(group => group.Count() / group.Count())
				.ToFutureValue(group => group.Sum());
		}

	}
}
