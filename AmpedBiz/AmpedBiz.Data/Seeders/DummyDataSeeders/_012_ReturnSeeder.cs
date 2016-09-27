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
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var currency = session.Load<Currency>(Currency.PHP.Id);

                Enumerable.Range(0, 13).ToList().ForEach(_ =>
                {
                    var entity = new Return();

                    entity.Accept(new ReturnCreateVisitor()
                    {
                        Branch = _utils.Random<Branch>(),
                        Customer = _utils.Random<Customer>(),
                        ReturnedBy = _utils.Random<User>(),
                        ReturnedOn = DateTime.Now.AddDays(_utils.RandomInt(-36, -1)),
                        Remarks = "Some Remarks",
                        Items = Enumerable.Range(0, _utils.RandomInt(1, 50))
                            .Select(x => _utils.RandomProduct()).Distinct()
                            .Select(x => new ReturnItem(
                                product: x,
                                returnReason: _utils.Random<ReturnReason>(),
                                quantity: new Measure(
                                    value: _utils.RandomDecimal(1M, 100M), 
                                    unit: x.Inventory.UnitOfMeasure
                                ),
                                unitPrice: new Money(
                                    amount: _utils.RandomDecimal(1000M, 100000M), 
                                    currency: currency
                                )
                            ))
                            .ToList()
                    });

                    session.Save(entity);

                });

                transaction.Commit();
            }
        }
    }
}
