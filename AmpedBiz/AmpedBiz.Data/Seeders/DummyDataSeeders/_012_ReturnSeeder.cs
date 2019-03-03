using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.Returns.Services;
using AmpedBiz.Core.Users;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Helpers;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
	public class _012_ReturnSeeder : IDummyDataSeeder
    {
        private readonly Utils _utils;
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _012_ReturnSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), contextProvider.Build(), sessionFactory);
            _contextProvider = contextProvider;
            _sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
            using (var transaction = session.BeginTransaction())
            {
                var settings = new SettingsFacade(session);
                var currency = settings.DefaultCurrency;

                Enumerable.Range(0, 2).ToList().ForEach(_ =>
                {
                    var products = _utils.RandomShippedProducts();
                    var productIds = products.Select(x => x.Id);
                    var inventories = session.Query<Inventory>()
                        .Where(x =>
                            x.Branch.Id == context.BranchId &&
                            productIds.Contains(x.Product.Id)
                        )
                        .ToList()
                        .ToDictionary(x => x.Product);


                    if (!products.Any())
                        return;

                    var validCount = _utils.RandomInteger(1, products.Count());
                    var randomProductCount = validCount > 50 ? 50 : validCount;

                    var entity = new Return();
                    entity.Accept(new ReturnSaveVisitor()
                    {
                        Branch = session.Load<Branch>(context.BranchId),
                        Customer = _utils.Random<Customer>(),
                        ReturnedBy = _utils.Random<User>(),
                        ReturnedOn = DateTime.Now.AddDays(_utils.RandomInteger(-36, -1)),
                        Remarks = "Some Remarks",
                        Items = products
                            .Take(randomProductCount)
                            .Select((x, i) => new ReturnItem(
                                sequence: i,
                                product: x,
                                reason: _utils.Random<ReturnReason>(),
                                quantity: new Measure(
                                    value: _utils.RandomInteger(1, (int)inventories[x].Shipped.Value),
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
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}
