using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
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

            var data = Enumerable.Range(1, 2)
                .Select((x, i) => new Customer()
                {
                    Code = $"Code {i}",
                    Name = $"Customer {i}",
                    //CreditLimit = new Money(100000.00M, session.Load<Currency>(Currency.PHP.Id)),
                    //Pricing = session.Load<Pricing>(Pricing.ListPrice.Id),
                    CreditLimit = new Money(100000.00M, Currency.PHP),
                    Pricing = Pricing.ListPrice,
                    IsActive = true,
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

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
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
