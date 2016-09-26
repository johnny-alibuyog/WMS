﻿using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _003_PricingSchemeSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _003_PricingSchemeSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<PricingScheme>().Cacheable().ToList();

                foreach (var item in PricingScheme.All)
                {
                    if (!entities.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}