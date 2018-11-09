using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
	public class _023_DefaultCustomerSeeder : IDefaultDataSeeder
	{
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _023_DefaultCustomerSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			this._contextProvider = contextProvider;
			this._sessionFactory = sessionFactory;
		}

		public bool IsSourceExternalFile => true;

		public void Seed()
		{
			var context = this._contextProvider.Build();

			var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"default_customers.xlsx");

			if (!File.Exists(filename))
				return;

			var excel = new ExcelQueryFactory(filename);
			var data = excel.Worksheet()
				.Select(x => new Customer()
				{
					Code = x["Account ID"],
					Name = x["Account Name"],
					ContactPerson = x["Contact Person"],
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

			using (var session = this._sessionFactory.RetrieveSharedSession(context))
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
				_sessionFactory.ReleaseSharedSession();
			}

		}
	}
}
