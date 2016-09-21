using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
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

            for (int i = 0; i < 580; i++)
            {
                data.Add(new Product($"product{i}")
                {
                    Name = $"Product {i}",
                    Description = $"Description {i}",
                    Image = $"some_image_{i}.png"
                });
            }

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                //session.SetBatchSize(100);

                var entity = session.Query<Product>().Cacheable().ToList();
                if (entity.Count == 0)
                {
                    var unitOfMeasureIndex = 0;
                    var unitOfMeasures = session.Query<UnitOfMeasure>().Cacheable().ToList();

                    Func<UnitOfMeasure> RotateUnitOfMeasure = () =>
                    {
                        var result = unitOfMeasures[unitOfMeasureIndex];

                        if (unitOfMeasureIndex < unitOfMeasures.Count - 1)
                            unitOfMeasureIndex++;
                        else
                            unitOfMeasureIndex = 0;

                        return result;
                    };

                    var supplierIndex = 0;
                    var suppliers = session.Query<Supplier>().Cacheable().ToList();

                    Func<Supplier> RotateSupplier = () =>
                    {
                        var result = suppliers[supplierIndex];

                        if (supplierIndex < suppliers.Count - 1)
                            supplierIndex++;
                        else
                            supplierIndex = 0;

                        return result;
                    };

                    var categoryIndex = 0;
                    var categories = session.Query<ProductCategory>().Cacheable().ToList();

                    Func<ProductCategory> RotateCategory = () =>
                    {
                        var result = categories[categoryIndex];

                        if (categoryIndex < categories.Count - 1)
                            categoryIndex++;
                        else
                            categoryIndex = 0;

                        return result;
                    };

                    var discontinued = default(bool);

                    Func<bool> RotateDiscontinued = () =>
                    {
                        var result = !discontinued;
                        discontinued = result;
                        return result;
                    };

                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    var random = new Random();

                    foreach (var item in data)
                    {
                        dynamic prices = new ExpandoObject();
                        prices.BasePrice = new Money(random.NextDecimal(1.00M, 20000.00M), currency);
                        prices.RetailPrice = new Money(random.NextDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.BasePrice.Amount + 5000M), currency);
                        prices.WholeSalePrice = new Money(random.NextDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.RetailPrice.Amount), currency);
                        prices.BadStockPrice = new Money(prices.BasePrice.Amount * 0.10M, currency);

                        item.Category = RotateCategory();
                        item.Supplier = RotateSupplier();
                        item.Discontinued = RotateDiscontinued();
                        item.Inventory.UnitOfMeasure = RotateUnitOfMeasure(); // UnitOfMeasureClass.Quantity.Units.RandomElement();
                        item.Inventory.UnitOfMeasureBase = RotateUnitOfMeasure();
                        item.Inventory.ConversionFactor = random.Next(1, 24);
                        item.Inventory.BasePrice = prices.BasePrice;
                        item.Inventory.RetailPrice = prices.RetailPrice;
                        item.Inventory.WholeSalePrice = prices.WholeSalePrice;
                        item.Inventory.BadStockPrice = prices.BadStockPrice;

                        item.Inventory.InitialLevel = new Measure(random.NextDecimal(150M, 300M), item.Inventory.UnitOfMeasure);
                        item.Inventory.TargetLevel = new Measure(random.NextDecimal(150M, 300M), item.Inventory.UnitOfMeasure);
                        item.Inventory.ReorderLevel = item.Inventory.TargetLevel - new Measure(random.NextDecimal(50M, 100M), item.Inventory.UnitOfMeasure);
                        item.Inventory.MinimumReorderQuantity = item.Inventory.TargetLevel - item.Inventory.ReorderLevel;
                        item.Inventory.Compute();

                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
