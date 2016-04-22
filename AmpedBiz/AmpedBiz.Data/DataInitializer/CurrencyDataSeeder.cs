﻿using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.DataInitializer
{
    public class CurrencyDataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public CurrencyDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public int ExecutionOrder
        {
            get { return 1; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<Currency>().ToList();
                if (users.Count == 0)
                {
                    foreach (var item in Currency.All)
                    {
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}