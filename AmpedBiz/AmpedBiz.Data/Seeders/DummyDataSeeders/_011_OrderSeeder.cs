using AmpedBiz.Common.Extentions;
using AmpedBiz.Common.Pipes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _011_OrderSeeder : IDummyDataSeeder
    {
        private readonly Utils _utils;
        private readonly ISessionFactory _sessionFactory;

        public _011_OrderSeeder(ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), sessionFactory);
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var min = 1;
            var max = 10;

            CreateNewOrders(_utils.RandomInteger(min, max));
            CreateInvoicedOrders(_utils.RandomInteger(min, max));
            CreatePaidOrders(_utils.RandomInteger(min, max));
            CreateStageOrders(_utils.RandomInteger(min, max));
            CreateShippedOrders(_utils.RandomInteger(min, max));
            CreateReturnedOrders(_utils.RandomInteger(min, max));
            CreateCompletedOrders(_utils.RandomInteger(min, max));
            CreateCancelledOrders(_utils.RandomInteger(min, max));
        }


        private bool Exists(Expression<Func<Order, bool>> condition)
        {
            var exists = false;
            using (var session = this._sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                exists = session.Query<Order>().Where(condition).Any();
                transaction.Commit();
            }
            return exists;
        }

        public IEnumerable<Order> CreateNewOrders(int count)
        {
            try
            {
                if (Exists(x => x.Status == OrderStatus.New))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Execute(new List<Order>());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreateInvoicedOrders(int count)
        {
            try
            {
                if (Exists(x => x.Status == OrderStatus.Invoiced))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.InvoiceAction())
                    .Execute(new List<Order>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreatePaidOrders(int count)
        {
            try
            {
                if (Exists(x => x.Payments.Any()))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.InvoiceAction())
                    .Register(new OrderActions.PayAction())
                    .Execute(new List<Order>());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreateStageOrders(int count)
        {
            try
            {
                if (Exists(x => x.Status == OrderStatus.Staged))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.InvoiceAction())
                    .Register(new OrderActions.PayAction())
                    .Register(new OrderActions.StageAction())
                    .Execute(new List<Order>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreateShippedOrders(int count)
        {
            try
            {
                if (Exists(x => x.Status == OrderStatus.Shipped))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.InvoiceAction())
                    .Register(new OrderActions.PayAction())
                    .Register(new OrderActions.StageAction())
                    .Register(new OrderActions.ShipAction())
                    .Execute(new List<Order>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreateReturnedOrders(int count)
        {
            try
            {
                if (Exists(x => x.Returns.Any()))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.InvoiceAction())
                    .Register(new OrderActions.PayAction())
                    .Register(new OrderActions.StageAction())
                    .Register(new OrderActions.ShipAction())
                    .Register(new OrderActions.ReturnAction())
                    .Execute(new List<Order>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreateCompletedOrders(int count)
        {
            try
            {
                if (Exists(x => x.Status == OrderStatus.Completed))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.InvoiceAction())
                    .Register(new OrderActions.PayAction())
                    .Register(new OrderActions.StageAction())
                    .Register(new OrderActions.ShipAction())
                    .Register(new OrderActions.ReturnAction())
                    .Register(new OrderActions.CompleteAction())
                    .Execute(new List<Order>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<Order> CreateCancelledOrders(int count)
        {
            try
            {
                if (Exists(x => x.Status == OrderStatus.Cancelled))
                    return null;

                return new Pipeline<IEnumerable<Order>>()
                    .Register(new OrderActions.NewAction(count))
                    .Register(new OrderActions.CancelAction())
                    .Execute(new List<Order>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }

    public class OrderActions
    {
        internal abstract class ActionStep : Step<IEnumerable<Order>>
        {
            protected readonly Utils _utils = new Utils(new Random(), SessionFactoryProvider.SessionFactory);
            protected readonly ISessionFactory _sessionFactory = SessionFactoryProvider.SessionFactory;

            protected abstract override IEnumerable<Order> Process(IEnumerable<Order> input);
        }

        internal class NewAction : ActionStep
        {
            public virtual int Count { get; set; }

            public NewAction(int count)
            {
                this.Count = count;
            }

            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    Enumerable.Range(0, this.Count).ToList().ForEach(_ =>
                    {
                        var entity = new Order(Guid.NewGuid());
                        entity.Accept(new OrderSaveVisitor()
                        {
                            OrderNumber = _utils.RandomDecimal(10000M, 99999M).ToString(),
                            CreatedBy = _utils.Random<User>(),
                            CreatedOn = DateTime.Now,
                            OrderedBy = _utils.Random<User>(),
                            OrderedOn = DateTime.Now,
                            Branch = _utils.Random<Branch>(),
                            Customer = _utils.Random<Customer>(),
                            Shipper = _utils.Random<Shipper>(),
                            ShippingAddress = null,
                            Pricing = _utils.Random<Pricing>(),
                            PaymentType = _utils.Random<PaymentType>(),
                            TaxRate = _utils.RandomDecimal(0.01M, 0.30M),
                            Tax = null, // compute this
                            ShippingFee = new Money(_utils.RandomDecimal(10M, 10000M), currency),
                            Items = Enumerable.Range(0, _utils.RandomInteger(1, 50))
                                .Select(x => _utils.RandomProduct()).Distinct()
                                .Select(x => new OrderItem(
                                    product: x,
                                    packagingSize: x.Inventory.PackagingSize,
                                    discountRate: _utils.RandomDecimal(0.01M, 0.10M),
                                    quantity: new Measure(_utils.RandomDecimal(1M, 100M), x.Inventory.UnitOfMeasure),
                                    unitPrice: x.Inventory.WholesalePrice
                                ))
                        });
                        entity.EnsureValidity();

                        session.Save(entity, entity.Id);

                        input.Add(entity);
                    });

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class InvoiceAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        entity.State.Process(new OrderInvoicedVisitor()
                        {
                            InvoicedOn = DateTime.Now,
                            InvoicedBy = _utils.Random<User>()
                        });
                        entity.EnsureValidity();
                        session.Update(entity);
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class PayAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        var currency = session.Load<Currency>(Currency.PHP.Id);
                        entity.Accept(new OrderSaveVisitor()
                        {
                            Payments = Enumerable
                                .Range(0, _utils.RandomInteger(1, 1))
                                .Select(x => new OrderPayment(
                                    paidOn: DateTime.Now,
                                    paidTo: _utils.Random<User>(),
                                    paymentType: _utils.Random<PaymentType>(),
                                    payment: new Money(_utils.RandomDecimal(1M, entity.Total.Amount), currency)
                                ))
                        });
                        entity.EnsureValidity();
                        session.Update(entity);
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class StageAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        entity.State.Process(new OrderStagedVisitor()
                        {
                            StagedOn = DateTime.Today,
                            StagedBy = _utils.Random<User>()
                        });
                        entity.EnsureValidity();
                        session.Update(entity);
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class ShipAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        entity.State.Process(new OrderShippedVisitor()
                        {
                            ShippedOn = DateTime.Now,
                            ShippedBy = _utils.Random<User>()
                        });
                        entity.EnsureValidity();
                        session.Update(entity);
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class ReturnAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        var products = entity.Items
                          .Select(x => x.Product)
                          .ToList();

                        var GetProduct = new Func<Guid, Product>(id => products.First(x => x.Id == id));
                        var GetUnitOfMeasure = new Func<Guid, UnitOfMeasure>(id => products.First(x => x.Id == id).Inventory.UnitOfMeasure);

                        var currency = session.Load<Currency>(Currency.PHP.Id);

                        entity.Accept(new OrderSaveVisitor()
                        {
                            Returns = entity.Items
                                .Take(_utils.RandomInteger(1, entity.Items.Count()))
                                .Select(x => new OrderReturn(
                                    product: x.Product,
                                    reason: _utils.Random<ReturnReason>(),
                                    returnedOn: DateTime.Now,
                                    returnedBy: _utils.Random<User>(),
                                    quantity: new Measure(
                                        value: _utils.RandomDecimal(1M, x.Quantity.Value),
                                        unit: x.Quantity.Unit
                                    ),
                                    returned: new Money(
                                        amount: _utils.RandomDecimal(1M, x.TotalPrice.Amount),
                                        currency: x.TotalPrice.Currency
                                    )
                                ))
                        });
                        entity.EnsureValidity();
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class CompleteAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        var currency = session.Load<Currency>(Currency.PHP.Id);
                        entity.State.Process(new OrderCompletedVisitor()
                        {
                            CompletedBy = _utils.Random<User>(),
                            CompletedOn = DateTime.Now
                        });
                        entity.EnsureValidity();
                        session.Update(entity);
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }

        internal class CancelAction : ActionStep
        {
            protected override IEnumerable<Order> Process(IEnumerable<Order> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    foreach (var entity in input)
                    {
                        var currency = session.Load<Currency>(Currency.PHP.Id);
                        entity.State.Process(new OrderCancelledVisitor()
                        {
                            CancelledBy = _utils.Random<User>(),
                            CancelledOn = DateTime.Now,
                            CancellationReason = "Cancellation Reason"
                        });
                        entity.EnsureValidity();
                        session.Update(entity);
                    }

                    transaction.Commit();

                    this._sessionFactory.ReleaseSharedSession();
                }

                return input;
            }
        }
    }
}