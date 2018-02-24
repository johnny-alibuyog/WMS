using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
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
    public class _002_ProductSeeder : IDummyDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _002_ProductSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }

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

            using (var session = _sessionFactory.RetrieveSharedSession(_context))
            using (var transaction = session.BeginTransaction())
            {
                session.SetBatchSize(100);

                var _utils = new Utils(new Random(), _sessionFactory);
                var exists = session.Query<Product>().Any();
                if (!exists)
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    foreach (var item in data)
                    {
                        dynamic prices = new ExpandoObject();
                        prices.BasePrice = new Money(_utils.RandomDecimal(1.00M, 20000.00M), currency);
                        prices.WholesalePrice = new Money(_utils.RandomDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.BasePrice.Amount + 5000M), currency);
                        prices.RetailPrice = new Money(_utils.RandomDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.WholesalePrice.Amount), currency);
                        prices.BadStockPrice = new Money(prices.BasePrice.Amount * 0.10M, currency);

                        item.Accept(new ProductUpdateVisitor()
                        {
                            Code = _utils.RandomString(length: 25),
                            Category = _utils.Random<ProductCategory>(),
                            Supplier = _utils.Random<Supplier>(),
                            Discontinued = _utils.RandomBoolean(),
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

                        var @default = item.UnitOfMeasures.FirstOrDefault(o => o.IsDefault);

                        item.Inventory.Accept(new InventoryUpdateVisitor()
                        {
                            Product = item,
                            InitialLevel = new Measure(_utils.RandomDecimal(150M, 300M), @default.UnitOfMeasure),
                            TargetLevel = new Measure(_utils.RandomDecimal(150M, 300M), @default.UnitOfMeasure),
                            ReorderLevel = item.Inventory.TargetLevel - new Measure(_utils.RandomDecimal(50M, 100M), @default.UnitOfMeasure),
                            MinimumReorderQuantity = item.Inventory.TargetLevel - item.Inventory.ReorderLevel,
                        });

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
