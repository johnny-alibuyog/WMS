﻿using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _002_ProductSeeder : IDummyDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _002_ProductSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var data = new List<Product>();

            for (int i = 0; i < 5; i++)
            {
                data.Add(new Product()
                {
                    Code = $"Code {1}",
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    Image = $"some_image_{i}.png"
                });
            }

            using (var session = _sessionFactory.RetrieveSharedSession())
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

                        item.Code = _utils.RandomString(length: 25);
                        item.Category = _utils.Random<ProductCategory>();
                        item.Supplier = _utils.Random<Supplier>();
                        item.Discontinued = _utils.RandomBoolean();
                        item.Inventory.IndividualBarcode = _utils.RandomString(length: 25);
                        item.Inventory.PackagingBarcode = _utils.RandomString(length: 25);
                        item.Inventory.UnitOfMeasure = _utils.Random<UnitOfMeasure>();
                        item.Inventory.PackagingUnitOfMeasure = _utils.Random<UnitOfMeasure>();
                        item.Inventory.PackagingSize = _utils.RandomInteger(1, 24);
                        item.Inventory.BasePrice = prices.BasePrice;
                        item.Inventory.WholesalePrice = prices.WholesalePrice;
                        item.Inventory.RetailPrice = prices.RetailPrice;
                        item.Inventory.BadStockPrice = prices.BadStockPrice;

                        item.Inventory.InitialLevel = new Measure(_utils.RandomDecimal(150M, 300M), item.Inventory.UnitOfMeasure);
                        item.Inventory.TargetLevel = new Measure(_utils.RandomDecimal(150M, 300M), item.Inventory.UnitOfMeasure);
                        item.Inventory.ReorderLevel = item.Inventory.TargetLevel - new Measure(_utils.RandomDecimal(50M, 100M), item.Inventory.UnitOfMeasure);
                        item.Inventory.MinimumReorderQuantity = item.Inventory.TargetLevel - item.Inventory.ReorderLevel;
                        item.Inventory.Compute();
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
