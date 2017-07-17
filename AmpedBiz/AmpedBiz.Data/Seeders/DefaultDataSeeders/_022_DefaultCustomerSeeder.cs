using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _022_DefaultCustomerSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _022_DefaultCustomerSeeder(ISessionFactory sessionFactory)
        {
            this._sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var filename = Path.Combine(DatabaseConfig.Instance.GetDefaultSeederDataAbsolutePath(), @"default_customers.xlsx");

            if (!File.Exists(filename))
                return;

            var excel = new ExcelQueryFactory(filename);
            var data = excel.Worksheet()
                .Select(x => new Customer()
                {
                    Code = x["Account ID"],
                    Name = x["Account Name"],
                    IsActive = x["Customer Activity Status"] == "Active",
                    Pricing = Pricing.RetailPrice,
                    CreditLimit = new Money(0),
                    BillingAddress = new Address()
                    {
                        Barangay = x["Barangay"],
                        City = x["City"],
                        Province = x["Billing State/Province"]
                    },
                    OfficeAddress = new Address()
                    {
                        Barangay = x["Barangay"],
                        City = x["City"],
                        Province = x["Billing State/Province"]
                    },
                })
                .ToList();

            using (var session = this._sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(100);

                var exists = session.Query<Customer>().Any();

                if (!exists)
                {
                    data.ForEach(customer =>
                    {
                        customer.EnsureValidity();
                        session.Save(customer);
                    });
                }

                transaction.Commit();
            }

        }
    }
}
