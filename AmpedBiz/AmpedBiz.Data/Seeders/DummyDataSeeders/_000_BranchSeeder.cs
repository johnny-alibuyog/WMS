﻿using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _000_BranchSeeder : IDummyDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _000_BranchSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var data = new List<Branch>();

            for (int i = 0; i < 2; i++)
            {
                data.Add(new Branch()
                {
                    Name = $"Branch {i}",
                    Description = $"Description {i}",
                    Address = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    }
                });
            }

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entity = session.Query<Branch>().Cacheable().ToList();
                if (entity.Count == 0)
                {
                    foreach (var item in data)
                    {
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
