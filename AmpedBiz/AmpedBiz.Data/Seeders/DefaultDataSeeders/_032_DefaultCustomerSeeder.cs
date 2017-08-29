using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _032_DefaultCustomerSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _032_DefaultCustomerSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
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

            using (var session = this._sessionFactory.RetrieveSharedSession(_context))
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
