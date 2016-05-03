using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class ProductSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public ProductSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool DummyData
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

                        item.Category = RotateCategory();
                        item.Supplier = RotateSupplier();
                        item.Discontinued = RotateDiscontinued();
                        item.BasePrice = prices.BasePrice;
                        item.RetailPrice = prices.RetailPrice;
                        item.WholesalePrice = prices.WholeSalePrice;

                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
