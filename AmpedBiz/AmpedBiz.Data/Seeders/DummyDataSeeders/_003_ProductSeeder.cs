using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Inventories.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Core.Settings;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
	public class _003_ProductSeeder : IDummyDataSeeder
	{
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _003_ProductSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			this._contextProvider = contextProvider;
			this._sessionFactory = sessionFactory;
		}

		public bool IsSourceExternalFile => false;

		public void Seed()
		{
			var data = new List<Product>();

			for (int i = 0; i < 5; i++)
			{
				data.Add(new Func<Product>(() =>
				{
					var product = new Product();
					product.Accept(new ProductUpdateVisitor()
					{
						Code = $"Code {1}",
						Name = $"Product {i}",
						Description = $"Description {i}",
						Image = $"some_image_{i}.png",
					});
					return product;
				})());
			}

			var context = this._contextProvider.Build();

			using (var session = _sessionFactory.RetrieveSharedSession(context))
			using (var transaction = session.BeginTransaction())
			{
				session.SetBatchSize(100);

				var _utils = new Utils(new Random(), context, _sessionFactory);
				var exists = session.Query<Product>().Any();
				if (!exists)
				{
					var currencySettings = session.Query<Setting<CurrencySetting>>().Cacheable().First();
					var currency = session.Load<Currency>(currencySettings.Value.DefaultCurrencyId);
                    var suppliers = session.Query<Supplier>().Cacheable().ToList();

					foreach (var product in data)
					{
						dynamic prices = new ExpandoObject();
						prices.BasePrice = new Money(_utils.RandomDecimal(1.00M, 20000.00M), currency);
						prices.WholesalePrice = new Money(_utils.RandomDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.BasePrice.Amount + 5000M), currency);
						prices.RetailPrice = new Money(_utils.RandomDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.WholesalePrice.Amount), currency);
						prices.BadStockPrice = new Money(prices.BasePrice.Amount * 0.10M, currency);

						product.Accept(new ProductUpdateVisitor()
						{
							Code = _utils.RandomString(length: 25),
							Category = _utils.Random<ProductCategory>(),
							Discontinued = _utils.RandomBoolean(),
                            Suppliers = suppliers,
							UnitOfMeasures = new Collection<ProductUnitOfMeasure>()
							{
								new ProductUnitOfMeasure(
									size: string.Empty,
									barcode: string.Empty,
									isDefault: true,
									isStandard: true,
									standardEquivalentValue: 1,
									unitOfMeasure: _utils.Random<UnitOfMeasure>(),
									prices: new Collection<ProductUnitOfMeasurePrice>()
									{
										new ProductUnitOfMeasurePrice(
											pricing: Pricing.BasePrice,
											price: new Money(100M, Currency.PHP)
										),
										new ProductUnitOfMeasurePrice(
											pricing: Pricing.WholesalePrice,
											price: new Money(100M, Currency.PHP)
										),
										new ProductUnitOfMeasurePrice(
											pricing: Pricing.RetailPrice,
											price: new Money(100M, Currency.PHP)
										),
										new ProductUnitOfMeasurePrice(
											pricing: Pricing.SuggestedRetailPrice,
											price: new Money(100M, Currency.PHP)
										),
										new ProductUnitOfMeasurePrice(
											pricing: Pricing.BadStockPrice,
											price: new Money(100M, Currency.PHP)
										),
									}
								)
							}
						});

						var @default = product.UnitOfMeasures.FirstOrDefault(o => o.IsDefault);
						var branch = session.Load<Branch>(context.BranchId);
						var inventory = new Inventory(branch, product);

						inventory.Accept(new InventoryUpdateVisitor()
						{
							Branch = branch,
							Product = product,
							InitialLevel = new Measure(_utils.RandomDecimal(150M, 300M), @default.UnitOfMeasure),
							TargetLevel = new Measure(_utils.RandomDecimal(150M, 300M), @default.UnitOfMeasure),
							ReorderLevel = inventory.TargetLevel - new Measure(_utils.RandomDecimal(50M, 100M), @default.UnitOfMeasure),
							MinimumReorderQuantity = inventory.TargetLevel - inventory.ReorderLevel,
						});

						product.EnsureValidity();
						inventory.EnsureValidity();

						session.Save(inventory);
						session.Save(product);
					}
				}

				transaction.Commit();

				_sessionFactory.ReleaseSharedSession();
			}
		}
	}
}
