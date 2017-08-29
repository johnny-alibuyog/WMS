using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using AmpedBiz.Data.Context;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _003_ShipperSeeder : IDummyDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _003_ShipperSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var data = new List<Shipper>();

            for (int i = 0; i < 2; i++)
            {
                data.Add(new Shipper($"shipper{i}")
                {
                    Name = $"Shipper {i}",
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
                        Email = $"shipper{i}@domain.com",
                        Landline = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Fax = $"{i}{i}{i}-{i}{i}{i}{i}",
                        Mobile = $"{i}{i}{i}{i}-{i}{i}{i}-{i}{i}{i}{i}",
                        Web = $"shipper{i}.com",
                    },
                });
            }

            using (var session = _sessionFactory.RetrieveSharedSession(_context))
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entities = session.Query<Shipper>().Cacheable().ToList();
                if (entities.Count == 0)
                {
                    foreach (var item in data)
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
