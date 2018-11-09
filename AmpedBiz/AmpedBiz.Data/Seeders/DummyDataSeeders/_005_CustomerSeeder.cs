using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Helpers;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
	public class _005_CustomerSeeder : IDummyDataSeeder
	{
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _005_CustomerSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			this._contextProvider = contextProvider;
			this._sessionFactory = sessionFactory;
		}

		public bool IsSourceExternalFile => false;

		public void Seed()
		{
			var context = this._contextProvider.Build();

			using (var session = _sessionFactory.RetrieveSharedSession(context))
			using (var transaction = session.BeginTransaction())
			{
				session.SetBatchSize(100);

				var settings = new SettingsFacade(session);
				var currency = settings.DefaultCurrency;

				var data = Enumerable.Range(1, 2)
					.Select((x, i) => new Customer()
					{
						Code = $"Code {i}",
						Name = $"Customer {i}",
						ContactPerson = $"Contact Person {i}",
						//CreditLimit = new Money(100000.00M, currency)),
						//Pricing = session.Load<Pricing>(Pricing.RetailPrice.Id),
						CreditLimit = new Money(100000.00M, currency),
						Pricing = Pricing.RetailPrice,
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

				var entity = session.Query<Customer>().Cacheable().ToList();
				if (entity.Count == 0)
				{
					foreach (var item in data)
					{
						item.EnsureValidity();
						session.Save(item);
					}
				}

				transaction.Commit();
				_sessionFactory.ReleaseSharedSession();
			}
		}
	}
}
