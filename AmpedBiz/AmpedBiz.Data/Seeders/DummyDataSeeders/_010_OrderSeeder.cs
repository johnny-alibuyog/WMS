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
using System.Reflection;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _010_OrderSeeder : IDummyDataSeeder
    {
        private readonly Random _random;
        private readonly ISessionFactory _sessionFactory;

        public _010_OrderSeeder(ISessionFactory sessionFactory)
        {
            _random = new Random();
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var maxRecord = 6;

            CreateNewOrders(_random.Next(maxRecord));
            CreateInvoiceOrders(_random.Next(maxRecord));
            CreatePaidOrders(_random.Next(maxRecord));
            CreateStageOrders(_random.Next(maxRecord));
            CreateShippedOrders(_random.Next(maxRecord));
            CreateReturnedOrders(_random.Next(maxRecord));
            CreateCompletedOrders(_random.Next(maxRecord));
            CreateCancelledOrders(_random.Next(maxRecord));
        }

        public void CreateNewOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.New);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }

        }

        public void CreateInvoiceOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Invoiced);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new InvoiceAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }

        public void CreatePaidOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Paid);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new InvoiceAction())
                    .Register(new PayAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }

        public void CreateStageOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Staged);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new InvoiceAction())
                    .Register(new PayAction())
                    .Register(new StageAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }

        public void CreateShippedOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Shipped);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new InvoiceAction())
                    .Register(new PayAction())
                    .Register(new StageAction())
                    .Register(new ShipAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }

        public void CreateReturnedOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Returned);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new InvoiceAction())
                    .Register(new PayAction())
                    .Register(new StageAction())
                    .Register(new ShipAction())
                    .Register(new ReturnAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }

        public void CreateCompletedOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Completed);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new InvoiceAction())
                    .Register(new PayAction())
                    .Register(new StageAction())
                    .Register(new ShipAction())
                    .Register(new ReturnAction())
                    .Register(new CompleteAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }

        public void CreateCancelledOrders(int count)
        {
            try
            {
                var session = this._sessionFactory.RetrieveSharedSession();
                var exists = session.Query<Order>().Any(x => x.Status == OrderStatus.Cancelled);
                if (exists)
                    return;

                var start = Stopwatch.StartNew();

                new Pipeline<IEnumerable<Order>>()
                    .Register(new NewAction(count))
                    .Register(new CancelAction())
                    .Execute(null);

                Console.WriteLine("{0}({1}): {2} seconds", MethodBase.GetCurrentMethod().Name, count, start.Elapsed.Seconds);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                this._sessionFactory.ReleaseSharedSession();
            }
        }
    }

    internal abstract class ActionPipe : Filter<IEnumerable<Order>>
    {
        protected readonly Utils _utils = new Utils(new Random(), SessionFactoryProvider.SessionFactory);
        protected readonly ISessionFactory _sessionFactory = SessionFactoryProvider.SessionFactory;

        protected abstract override IEnumerable<Order> Process(IEnumerable<Order> input);
    }

    internal class NewAction : ActionPipe
    {
        public virtual int Count { get; set; }

        public NewAction(int count)
        {
            this.Count = count;
        }

        protected override IEnumerable<Order> Process(IEnumerable<Order> input)
        {
            input = new List<Order>();

            using (var session = this._sessionFactory.RetrieveSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                var currency = session.Load<Currency>(Currency.PHP.Id);

                Enumerable.Range(0, this.Count - 1).ToList().ForEach(_ =>
                {
                    var entity = new Order(Guid.NewGuid());
                    entity.State.Process(new OrderNewlyCreatedVisitor()
                    {
                        OrderNumber = _utils.NextDecimal(10000M, 99999M).ToString(),
                        CreatedBy = _utils.Random<User>(),
                        CreatedOn = DateTime.Now,
                        OrderedBy = _utils.Random<User>(),
                        OrderedOn = DateTime.Now,
                        Branch = _utils.Random<Branch>(),
                        Customer = _utils.Random<Customer>(),
                        Shipper = _utils.Random<Shipper>(),
                        PaymentType = _utils.Random<PaymentType>(),
                        TaxRate = _utils.NextDecimal(0.01M, 0.30M),
                        Tax = null, // compute this
                        ShippingFee = new Money(_utils.NextDecimal(10M, 10000M), currency),
                        Items = Enumerable.Range(0, _utils.Next(1, 50))
                            .Select(x => _utils.RandomProduct()).Distinct()
                            .Select(x => new OrderItem(
                                product: x,
                                discountRate: _utils.NextDecimal(0.01M, 0.10M),
                                quantity: new Measure(_utils.NextDecimal(1M, 100M), x.Inventory.UnitOfMeasure),
                                discount: new Money(_utils.NextDecimal(100M, 500M), currency),
                                unitPrice: new Money(_utils.NextDecimal(1000M, 100000M), currency)
                            ))
                    });

                    session.Save(entity, entity.Id);

                    input.Add(entity);
                });

                transaction.Commit();
            }

            return input;
        }
    }

    internal class InvoiceAction : ActionPipe
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
                    session.Update(entity);
                }

                transaction.Commit();
            }

            return input;
        }
    }

    internal class PayAction : ActionPipe
    {
        protected override IEnumerable<Order> Process(IEnumerable<Order> input)
        {
            using (var session = this._sessionFactory.RetrieveSharedSession())
            using (var transaction = session.BeginTransaction())
            {
                foreach (var entity in input)
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    entity.State.Process(new OrderPaidVisitor()
                    {
                        Payments = Enumerable
                            .Range(0, _utils.Next(1, 5))
                            .Select(x => new OrderPayment(
                                paidOn: DateTime.Now,
                                paidBy: _utils.Random<User>(),
                                paymentType: _utils.Random<PaymentType>(),
                                payment: new Money(_utils.NextDecimal(1M, entity.Total.Amount), currency)
                            ))
                    });
                    session.Update(entity);
                }

                transaction.Commit();
            }

            return input;
        }
    }

    internal class StageAction : ActionPipe
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
                    session.Update(entity);
                }

                transaction.Commit();
            }

            return input;
        }
    }

    internal class ShipAction : ActionPipe
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
                    session.Update(entity);
                }

                transaction.Commit();
            }

            return input;
        }
    }

    internal class ReturnAction : ActionPipe
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

                    Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<Guid, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    entity.State.Process(new OrderReturnedVisitor()
                    {
                        Returns = entity.Items
                            .Take(_utils.Next(1, entity.Items.Count()))
                            .Select(x => new OrderReturn(
                                product: x.Product,
                                returnedOn: DateTime.Now,
                                returnedBy: _utils.Random<User>(),
                                quantity: new Measure(
                                    value: _utils.NextDecimal(1M, x.Quantity.Value),
                                    unit: x.Quantity.Unit
                                ),
                                discountRate: x.DiscountRate,
                                discount: x.Discount,
                                unitPrice: x.UnitPrice
                            ))
                    });
                }

                transaction.Commit();
            }

            return input;
        }
    }

    internal class CompleteAction : ActionPipe
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
                    session.Update(entity);
                }

                transaction.Commit();
            }

            return input;
        }
    }

    internal class CancelAction : ActionPipe
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
                    session.Update(entity);
                }

                transaction.Commit();
            }

            return input;
        }
    }
}
