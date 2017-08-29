using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Returns;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _012_ReturnSeeder : IDummyDataSeeder
    {
        private readonly Utils _utils;
        private readonly ISessionFactory _sessionFactory;

        public _012_ReturnSeeder(ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), sessionFactory);
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.RetrieveSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var currency = session.Load<Currency>(Currency.PHP.Id);

                Enumerable.Range(0, 2).ToList().ForEach(_ =>
                {
                    var products = _utils.RandomShippedProducts();
                    if (!products.Any())
                        return;

                    var validCount = _utils.RandomInteger(1, products.Count());
                    var randomProductCount = validCount > 50 ? 50 : validCount;

                    var entity = new Return();
                    entity.Accept(new ReturnSaveVisitor()
                    {
                        Branch = _utils.Random<Branch>(),
                        Customer = _utils.Random<Customer>(),
                        ReturnedBy = _utils.Random<User>(),
                        ReturnedOn = DateTime.Now.AddDays(_utils.RandomInteger(-36, -1)),
                        Remarks = "Some Remarks",
                        Items = products
                            .Take(randomProductCount)
                            .Select(x => new ReturnItem(
                                product: x,
                                returnReason: _utils.Random<ReturnReason>(),
                                quantity: new Measure(
                                    value: _utils.RandomInteger(1, (int)x.Inventory.Shipped.Value),
                                    unit: x.UnitOfMeasures.Default(o => o.UnitOfMeasure)
                                ),
                                standard: x.StandardEquivalentMeasureOf(x.UnitOfMeasures.Default(o => o.UnitOfMeasure)),
                                unitPrice: x.UnitOfMeasures.Default(o => o.Prices.Retail())
                            ))
                            .ToList()
                    });
                    entity.EnsureValidity();

                    session.Save(entity);

                });

                transaction.Commit();

                this._sessionFactory.ReleaseSharedSession();
            }
        }
    }
}
