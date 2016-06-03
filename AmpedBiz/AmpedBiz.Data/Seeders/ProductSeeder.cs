using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class ProductSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public ProductSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return true; }
        }

        public int ExecutionOrder
        {
            get { return 11; }
        }

        public void Seed()
        {
            var data = new List<Product>();

            for (int i = 0; i < 36; i++)
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

                var entity = session.Query<Product>().ToList();
                if (entity.Count == 0)
                {
                    var unitOfMeasureIndex = 0;
                    var unitOfMeasures = session.Query<UnitOfMeasure>().ToList();

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
                    var suppliers = session.Query<Supplier>().ToList();

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
                    var categories = session.Query<ProductCategory>().ToList();

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

                    var random = new Random();

                    foreach (var item in data)
                    {
                        dynamic prices = new ExpandoObject();
                        prices.BasePrice = new Money(random.NextDecimal(1.00M, 20000.00M));
                        prices.RetailPrice = new Money(random.NextDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.BasePrice.Amount + 5000M));
                        prices.WholeSalePrice = new Money(random.NextDecimal((decimal)prices.BasePrice.Amount, (decimal)prices.RetailPrice.Amount));

                        item.GoodStockInventory.UnitOfMeasure = UnitOfMeasureClass.Quantity.Units.RandomElement();
                        item.GoodStockInventory.UnitOfMeasureBase = RotateUnitOfMeasure();
                        item.GoodStockInventory.ConvertionFactor = random.Next(1, 24);
                        item.Category = RotateCategory();
                        item.Supplier = RotateSupplier();
                        item.Discontinued = RotateDiscontinued();
                        item.BasePrice = prices.BasePrice;
                        item.RetailPrice = prices.RetailPrice;
                        item.WholesalePrice = prices.WholeSalePrice;

                        item.GoodStockInventory.TargetLevel = new Measure(random.NextDecimal(150M, 300M), item.GoodStockInventory.UnitOfMeasure);
                        item.GoodStockInventory.ReorderLevel = item.GoodStockInventory.TargetLevel - new Measure(random.NextDecimal(50M, 100M), item.GoodStockInventory.UnitOfMeasure);
                        item.GoodStockInventory.MinimumReorderQuantity = item.GoodStockInventory.TargetLevel - item.GoodStockInventory.ReorderLevel;

                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
