﻿using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _006_UnitOfMeasureSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _006_UnitOfMeasureSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 7; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<UnitOfMeasure>().Cacheable().ToList();

                foreach (var item in UnitOfMeasure.All)
                {
                    if (!entities.Contains(item))
                    {
                        item.EnsureValidity();
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}