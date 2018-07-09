using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Helpers;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _013_InventoryAdjustementSeeder : IDummyDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _013_InventoryAdjustementSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
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
                if (session.Query<InventoryAdjustment>().Any())
                    return;

                var utils = new Utils(new Random(), context, _sessionFactory);

                var currency = new SettingsFacade(session).DefaultCurrency;

                var products = utils.RandomAvailableProducts();

                if (!products.Any())
                    return;

                var inventories = session.Query<Inventory>()
                    .Where(x =>
                        x.Branch.Id == context.BranchId &&
                        x.Adjustments.Any() == false &&
                        products.Contains(x.Product)
                    )
                    .Take(utils.RandomInteger(1, 10))
                    .ToList();

                inventories.ForEach((inventory) =>
                {
                    Enumerable.Range(0, 5).ToList().ForEach(_ =>
                    {
                        var reason = utils.Random<InventoryAdjustmentReason>();

                        var quantity = new
                        {
                            Unit = inventory.OnHand.Unit,
                            ValueRange = new
                            {
                                Min = 1M,
                                Max = Math.Round(inventory.OnHand.Value / 50)
                            }
                        };

                        var standard = inventory.Product.StandardEquivalentMeasureOf(quantity.Unit);

                        if (quantity.ValueRange.Max < quantity.ValueRange.Min)
                            return;

                        inventory.Accept(new InventoryAdjustVisitor(
                            adjustedBy: utils.Random<User>(),
                            adjustedOn: DateTime.Now,
                            reason: reason,
                            remarks: $"Remarks_{utils.RandomString(200)}",
                            quantity: new Measure(
                                unit: quantity.Unit,
                                value: utils.RandomDecimal(
                                    min: quantity.ValueRange.Min,
                                    max: quantity.ValueRange.Max
                                )
                            ),
                            standard: standard
                        ));
                    });
                });

                transaction.Commit();

                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}
