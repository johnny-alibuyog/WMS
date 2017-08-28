using AmpedBiz.Common.Extentions;
using AmpedBiz.Common.Pipes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using AmpedBiz.Core.Services.Products;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            var max = 3;

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

            protected IEnumerable<Order> ReloadFromSession(IEnumerable<Order> input)
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var ids = input.Select(x => x.Id).ToList();
                return session.QueryOver<Order>()
                    .AndRestrictionOn(x => x.Id).IsIn(ids)
                    .Fetch(x => x.Branch).Eager
                    .Fetch(x => x.Customer).Eager
                    .Fetch(x => x.Pricing).Eager
                    .Fetch(x => x.PaymentType).Eager
                    .Fetch(x => x.Shipper).Eager
                    .Fetch(x => x.Tax).Eager
                    .Fetch(x => x.ShippingFee).Eager
                    .Fetch(x => x.Discount).Eager
                    .Fetch(x => x.SubTotal).Eager
                    .Fetch(x => x.Total).Eager
                    .Fetch(x => x.CreatedBy).Eager
                    .Fetch(x => x.OrderedBy).Eager
                    .Fetch(x => x.RoutedBy).Eager
                    .Fetch(x => x.StagedBy).Eager
                    .Fetch(x => x.InvoicedBy).Eager
                    .Fetch(x => x.PaidTo).Eager
                    .Fetch(x => x.RoutedBy).Eager
                    .Fetch(x => x.CompletedBy).Eager
                    .Fetch(x => x.CancelledBy).Eager
                    .Fetch(x => x.Items).Eager
                    .Fetch(x => x.Items.First().Product).Eager
                    .Fetch(x => x.Items.First().Product.Inventory).Eager
                    .Fetch(x => x.Items.First().Product.UnitOfMeasures).Eager
                    .Fetch(x => x.Items.First().Product.UnitOfMeasures.First().Prices).Eager
                    .Fetch(x => x.Payments).Eager
                    .Fetch(x => x.Payments.First().PaidTo).Eager
                    .Fetch(x => x.Payments.First().PaymentType).Eager
                    .Fetch(x => x.Returns).Eager
                    .Fetch(x => x.Returns.First().Reason).Eager
                    .Fetch(x => x.Returns.First().ReturnedBy).Eager
                    .Fetch(x => x.Returns.First().Product).Eager
                    .Fetch(x => x.Returns.First().Product.Inventory).Eager
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .List();
            }
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
                        var products = _utils.RandomAvailableProducts();
                        if (!products.Any())
                            return;

                        var validCount = _utils.RandomInteger(1, products.Count());
                        var randomProductCount = validCount > 50 ? 50 : validCount;

                        var entity = new Order(Guid.NewGuid());
                        entity.Accept(new OrderUpdateVisitor()
                        {
                            OrderNumber = _utils.RandomInteger(10000, 99999).ToString(),
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
                            ShippingFee = new Money(_utils.RandomInteger(10, 10000), currency),
                            Items = products
                                .Take(randomProductCount)
                                .Select(x => new OrderItem(
                                        product: x,
                                        discountRate: _utils.RandomDecimal(0.01M, 0.10M),
                                        quantity: new Measure(
                                            value: _utils.RandomInteger(1, 15),
                                            unit: x.UnitOfMeasures.Standard(o => o.UnitOfMeasure)
                                        ),
                                        standard: new Measure(
                                            unit: x.UnitOfMeasures.Standard(o => o.UnitOfMeasure),
                                            value: x.UnitOfMeasures.Standard(o => o.StandardEquivalentValue)
                                        ),
                                        unitPrice: x.UnitOfMeasures.Standard(o => o.Prices.Wholesale())
                                    )
                                )
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
                    input = this.ReloadFromSession(input);
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
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        var currency = session.Load<Currency>(Currency.PHP.Id);
                        entity.Accept(new OrderUpdateVisitor()
                        {
                            Payments = Enumerable
                                .Range(0, _utils.RandomInteger(1, 1))
                                .Select(x => new OrderPayment(
                                    paidOn: DateTime.Now,
                                    paidTo: _utils.Random<User>(),
                                    paymentType: _utils.Random<PaymentType>(),
                                    payment: new Money(_utils.RandomInteger(1, (int)entity.Total.Amount), currency)
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
                    input = this.ReloadFromSession(input);
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
                    input = this.ReloadFromSession(input);
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
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        var products = entity.Items
                          .Select(x => x.Product)
                          .ToList();

                        var GetProduct = new Func<Guid, Product>(id => products.First(x => x.Id == id));
                        var GetUnitOfMeasure = new Func<Guid, UnitOfMeasure>(id => products.First(x => x.Id == id).Inventory.UnitOfMeasure);

                        var currency = session.Load<Currency>(Currency.PHP.Id);

                        entity.Accept(new OrderUpdateVisitor()
                        {
                            Returns = entity.Items
                                .Take(_utils.RandomInteger(1, entity.Items.Count()))
                                .Select(x => new OrderReturn(
                                    product: x.Product,
                                    reason: _utils.Random<ReturnReason>(),
                                    returnedOn: DateTime.Now,
                                    returnedBy: _utils.Random<User>(),
                                    quantity: new Measure(
                                        value: _utils.RandomInteger(1, (int)x.Quantity.Value),
                                        unit: x.Quantity.Unit
                                    ),
                                    returned: new Money(
                                        amount: _utils.RandomInteger(1, (int)x.TotalPrice.Amount),
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
                    input = this.ReloadFromSession(input);
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
                    input = this.ReloadFromSession(input);
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