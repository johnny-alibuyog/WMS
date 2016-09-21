using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _004_CustomerSeeder : IDummyDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _004_CustomerSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var data = Enumerable.Range(1, 153)
                    .Select((x, i) => new Customer($"customer{i}")
                    {
                        Name = $"Customer {i}",
                        //CreditLimit = new Money(100000.00M, session.Load<Currency>(Currency.PHP.Id)),
                        //PricingScheme = session.Load<PricingScheme>(PricingScheme.WholesalePrice.Id),
                        CreditLimit = new Money(100000.00M, Currency.PHP),
                        PricingScheme = PricingScheme.WholesalePrice,
                        Tenant = new Tenant(),
                        Contact = new Contact()
                        {
                            Email = $"customer{i}@domain.com",
                            Landline = $"{i}{i}{i}-{i}{i}{i}{i}",
                            Fax = $"{i}{i}{i}-{i}{i}{i}{i}",
                            Mobile = $"{i}{i}{i}{i}-{i}{i}{i}-{i}{i}{i}{i}",
                            Web = $"customer{i}.com",
                        },
                        OfficeAddress = new Address()
                        {
                            Street = $"Street {i}",
                            Barangay = $"Barangay {i}",
                            City = $"City {i}",
                            Province = $"Province {i}",
                            Region = $"Region {i}",
                            Country = $"Country {i}",
                            ZipCode = $"Zip Code {i}"
                        },
                        BillingAddress = new Address()
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

                session.SetBatchSize(100);

                var entity = session.Query<Customer>().Cacheable().ToList();
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
