using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _001_SupplierSeeder : IDummyDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _001_SupplierSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return true; }
        }

        public int ExecutionOrder
        {
            get { return 6; }
        }

        public void Seed()
        {
            var data = new List<Supplier>();

            for (int i = 0; i < 36; i++)
            {
                data.Add(new Supplier($"supplier{i}")
                {
                    Name = $"Supplier {i}",
                    Address = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    },
                    Contact = new Contact()
                    {
                        Email = $"customer{i}@domain.com",
                        Landline = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Fax = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Mobile = $"{i}{i}{i}{i}-{i}{i}{i}-{i}{i}{i}{i}",
                        Web = $"customer{i}.com",
                    },
                });
            }

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entities = session.Query<Supplier>().Cacheable().ToList();
                if (entities.Count == 0)
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
