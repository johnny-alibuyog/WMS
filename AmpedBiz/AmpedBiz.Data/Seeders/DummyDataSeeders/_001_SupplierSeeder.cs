using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _001_SupplierSeeder : IDummyDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _001_SupplierSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var data = new List<Supplier>();

            for (int i = 0; i < 2; i++)
            {
                data.Add(new Supplier()
                {
                    Code = $"Code {i}",
                    Name = $"Supplier {i}",
                    ContactPerson = $"Contact Person {i}",
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

            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entities = session.Query<Supplier>().Cacheable().ToList();
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
