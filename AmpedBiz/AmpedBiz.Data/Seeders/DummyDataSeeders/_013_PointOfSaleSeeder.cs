using AmpedBiz.Core.Common;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Core.PointOfSales.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Helpers;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _013_PointOfSaleSeeder : IDummyDataSeeder
	{
		private readonly Utils _utils;
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _013_PointOfSaleSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			this._utils = new Utils(new Random(), contextProvider.Build(), sessionFactory);
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
				var settings = new SettingsFacade(session);

				Enumerable.Range(0, 2).ToList().ForEach(_ =>
				{
					var products = _utils.RandomAvailableProducts();

					if (!products.Any())
						return;

					var productIds = products.Select(x => x.Id);

					if (!products.Any())
						return;

					var validCount = _utils.RandomInteger(1, products.Count());

					var randomProductCount = validCount > 50 ? 50 : validCount;

					var entity = new PointOfSale();

					entity.Accept(new PointOfSaleUpdateVisitor()
					{
						Branch = session.Load<Branch>(context.BranchId),
						Customer = _utils.Random<Customer>(),
						Pricing = session.Load<Pricing>(Pricing.RetailPrice.Id),
						DiscountRate = _utils.RandomDecimal(0.01M, 0.10M),
						PaymentType = _utils.Random<PaymentType>(),
						TenderedBy = _utils.Random<User>(),
						TenderedOn = DateTime.Now.AddDays(_utils.RandomInteger(-36, -1)),
						Items = products
							.Take(randomProductCount)
							.Select((product, index) => new PointOfSaleItem(
                                sequence: index,
								product: product,
								quantity: new Measure(
									value: _utils.RandomInteger(1, 4),
									unit: product.UnitOfMeasures.Standard(o => o.UnitOfMeasure)
								),
								discountRate: _utils.RandomDecimal(0.01M, 0.10M),
								standard: product.StandardEquivalentMeasureOf(product.UnitOfMeasures.Standard(o => o.UnitOfMeasure)),
								unitPrice: product.UnitOfMeasures.Standard(o => o.Prices.Retail())
							))
							.ToList()
					});

					entity.Accept(new PointOfSaleUpdateVisitor()
					{
						Payments = new Func<List<PointOfSalePayment>>(() =>
						{
                            var sequence = 0;
							var result = new List<PointOfSalePayment>();
							while (result.Sum(x => x.Payment) < entity.Total)
							{
                                result.Add(new PointOfSalePayment(
                                    sequence: ++sequence,
									paidOn: DateTime.Now,
									paidTo: _utils.Random<User>(),
									paymentType: _utils.Random<PaymentType>(),
									payment: new Money(_utils.RandomInteger(1, (int)entity.Total.Amount), settings.DefaultCurrency)
							  ));
							}
							return result;
						})()
					});

					entity.EnsureValidity();

					session.Save(entity);

				});

				transaction.Commit();

				_sessionFactory.ReleaseSharedSession();
			}
		}
	}
}
