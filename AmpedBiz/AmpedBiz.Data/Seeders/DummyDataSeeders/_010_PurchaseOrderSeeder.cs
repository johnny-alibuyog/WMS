using AmpedBiz.Common.Extentions;
using AmpedBiz.Common.Pipes;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Core.PurchaseOrders.Services;
using AmpedBiz.Core.Users;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
	public class _010_PurchaseOrderSeeder : IDummyDataSeeder
    {
        private readonly Utils _utils;
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _010_PurchaseOrderSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), contextProvider.Build(), sessionFactory);
            _contextProvider = contextProvider;
            _sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var min = 1;
            var max = 3;

            PurchaseOrderActions.Context = this._contextProvider.Build();

            CreateNewPurchaseOrders(_utils.RandomInteger(min, max));
            CreateSubmittedPurchaseOrder(_utils.RandomInteger(min, max));
            CreateApprovedPurchaseOrder(_utils.RandomInteger(min, max));
            CreatePayPurchaseOrder(_utils.RandomInteger(min, max));
            CreateReceivePurchaseOrder(_utils.RandomInteger(min, max * 2));
            CreateCompletePurchaseOrder(_utils.RandomInteger(min, max * 3));
            CreateCancelPurchaseOrder(_utils.RandomInteger(min, max));
        }

        private bool Exists(Expression<Func<PurchaseOrder, bool>> condition)
        {
            var exists = false;
            using (var session = this._sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                exists = session.Query<PurchaseOrder>().Where(condition).Any();
                transaction.Commit();
            }
            return exists;
        }

        public IEnumerable<PurchaseOrder> CreateNewPurchaseOrders(int count)
        {
            try
            {
                if (this.Exists(x => x.Status == PurchaseOrderStatus.Created))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Execute(new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<PurchaseOrder> CreateSubmittedPurchaseOrder(int count)
        {
            try
            {
                if (Exists(x => x.Status == PurchaseOrderStatus.Submitted))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Register(new PurchaseOrderActions.SubmitAction())
                    .Execute(new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<PurchaseOrder> CreateApprovedPurchaseOrder(int count)
        {
            try
            {
                if (Exists(x => x.Status == PurchaseOrderStatus.Approved))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Register(new PurchaseOrderActions.SubmitAction())
                    .Register(new PurchaseOrderActions.ApproveAction())
                    .Execute(new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<PurchaseOrder> CreatePayPurchaseOrder(int count)
        {
            try
            {
                if (Exists(x => x.Payments.Any()))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Register(new PurchaseOrderActions.SubmitAction())
                    .Register(new PurchaseOrderActions.ApproveAction())
                    .Register(new PurchaseOrderActions.PayAction())
                    .Execute(new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<PurchaseOrder> CreateReceivePurchaseOrder(int count)
        {
            try
            {
                if (Exists(x => x.Receipts.Any()))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Register(new PurchaseOrderActions.SubmitAction())
                    .Register(new PurchaseOrderActions.ApproveAction())
                    .Register(new PurchaseOrderActions.PayAction())
                    .Register(new PurchaseOrderActions.ReceiveAction())
                    .Execute(new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<PurchaseOrder> CreateCompletePurchaseOrder(int count)
        {
            try
            {
                if (Exists(x => x.Status == PurchaseOrderStatus.Completed))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Register(new PurchaseOrderActions.SubmitAction())
                    .Register(new PurchaseOrderActions.ApproveAction())
                    .Register(new PurchaseOrderActions.PayAction())
                    .Register(new PurchaseOrderActions.ReceiveAction())
                    .Register(new PurchaseOrderActions.CompleteAction())
                    .Execute(new List<PurchaseOrder>());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public IEnumerable<PurchaseOrder> CreateCancelPurchaseOrder(int count)
        {
            try
            {
                if (Exists(x => x.Status == PurchaseOrderStatus.Cancelled))
                    return null;

                return new Pipeline<IEnumerable<PurchaseOrder>>()
                    .Register(new PurchaseOrderActions.NewAction(count))
                    .Register(new PurchaseOrderActions.SubmitAction())
                    .Register(new PurchaseOrderActions.CancelledAction())
                    .Execute(new List<PurchaseOrder>());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }

    public class PurchaseOrderActions
    {
        public static IContext Context;

        internal abstract class ActionStep : Step<IEnumerable<PurchaseOrder>>
        {
            protected readonly IContext _context = Context;
            protected readonly Utils _utils = new Utils(new Random(), Context, SessionFactoryProvider.SessionFactory);
            protected readonly ISessionFactory _sessionFactory = SessionFactoryProvider.SessionFactory;

            protected abstract override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input);

            protected IEnumerable<PurchaseOrder> ReloadFromSession(IEnumerable<PurchaseOrder> input)
            {
                var session = this._sessionFactory.RetrieveSharedSession(_context);
                var ids = input.Select(x => x.Id).ToList();
                return session.QueryOver<PurchaseOrder>()
                    .AndRestrictionOn(x => x.Id).IsIn(ids)
                    .Fetch(x => x.Tax).Eager
                    .Fetch(x => x.ShippingFee).Eager
                    .Fetch(x => x.Shipper).Eager
                    .Fetch(x => x.Supplier).Eager
                    .Fetch(x => x.PaymentType).Eager
                    .Fetch(x => x.Paid).Eager
                    .Fetch(x => x.SubTotal).Eager
                    .Fetch(x => x.Total).Eager
                    .Fetch(x => x.CreatedBy).Eager
                    .Fetch(x => x.SubmittedBy).Eager
                    .Fetch(x => x.ApprovedBy).Eager
                    .Fetch(x => x.PaidBy).Eager
                    .Fetch(x => x.ReceivedBy).Eager
                    .Fetch(x => x.CompletedBy).Eager
                    .Fetch(x => x.CancelledBy).Eager
                    .Fetch(x => x.Items).Eager
                    .Fetch(x => x.Items.First().Product).Eager
                    .Fetch(x => x.Items.First().Product.Supplier).Eager
                    .Fetch(x => x.Items.First().Product.Category).Eager
                    .Fetch(x => x.Items.First().Product.Inventories).Eager
                    .Fetch(x => x.Items.First().Product.UnitOfMeasures).Eager
                    .Fetch(x => x.Items.First().Product.UnitOfMeasures.First().Prices).Eager
                    .Fetch(x => x.Payments).Eager
                    .Fetch(x => x.Payments.First().PaidBy).Eager
                    .Fetch(x => x.Receipts).Eager
                    .Fetch(x => x.Receipts.First().Product).Eager
                    .Fetch(x => x.Receipts.First().Product.Supplier).Eager
                    .Fetch(x => x.Receipts.First().Product.Category).Eager
                    .Fetch(x => x.Receipts.First().Product.Inventories).Eager
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

            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    Enumerable.Range(0, this.Count).ToList().ForEach(_ =>
                    {
                        var products = _utils.RandomProducts();
                        if (!products.Any())
                            return;

                        var validCount = _utils.RandomInteger(1, products.Count());
                        var randomProductCount = validCount > 30 ? 30 : validCount;

                        var entity = new PurchaseOrder(Guid.NewGuid());
                        entity.Accept(new PurchaseOrderUpdateVisitor()
                        {
                            Branch = session.Load<Branch>(_context.BranchId),
                            CreatedBy = _utils.Random<User>(),
                            CreatedOn = DateTime.Now,
                            ExpectedOn = DateTime.Now.AddMonths(5),
                            PaymentType = _utils.Random<PaymentType>(),
                            Shipper = _utils.Random<Shipper>(),
                            ShippingFee = new Money(_utils.RandomInteger(10, 10000), currency),
                            Tax = null, // compute this
                            Supplier = _utils.Random<Supplier>(),
                            Items = products
                                .Take(randomProductCount)
                                .Select(x => new PurchaseOrderItem(
                                    product: x,
                                    quantity: new Measure(
                                        value: _utils.RandomInteger(1, 20),
                                        unit: x.UnitOfMeasures.Default(o => o.UnitOfMeasure)
                                    ),
                                    standard: x.StandardEquivalentMeasureOf(x.UnitOfMeasures.Default(o => o.UnitOfMeasure)),
                                    unitCost: x.UnitOfMeasures.Default(o => o.Prices.Base())
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

        internal class SubmitAction : ActionStep
        {
            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        entity.State.Process(new PurchaseOrderSubmittedVisitor()
                        {
                            SubmittedOn = DateTime.Now,
                            SubmittedBy = _utils.Random<User>()
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

        internal class ApproveAction : ActionStep
        {
            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        entity.State.Process(new PurchaseOrderApprovedVisitor()
                        {
                            Branch = session.Load<Branch>(_context.BranchId),
                            ApprovedOn = DateTime.Now,
                            ApprovedBy = _utils.Random<User>()
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
            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        var currency = session.Load<Currency>(Currency.PHP.Id);

                        entity.Accept(new PurchaseOrderUpdateVisitor()
                        {
                            Branch = session.Load<Branch>(_context.BranchId),
                            Payments = Enumerable
                                .Range(0, _utils.RandomInteger(1, 1))
                                .Select(x => new PurchaseOrderPayment(
                                    paidOn: DateTime.Now,
                                    paidBy: _utils.Random<User>(),
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

        internal class ReceiveAction : ActionStep
        {
            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        entity.Accept(new PurchaseOrderUpdateVisitor()
                        {
                            Branch = session.Load<Branch>(_context.BranchId),
                            Receipts = entity.Items
                                .Select(x => new PurchaseOrderReceipt(
                                    batchNumber: this._utils.RandomString(255),
                                    receivedBy: this._utils.Random<User>(),
                                    receivedOn: DateTime.Now,
                                    expiresOn: DateTime.Now.AddYears(3),
                                    product: x.Product,
                                    quantity: new Measure(
                                        value: this._utils.RandomInteger(
                                            min: (int)x.Quantity.Value - 2,
                                            max: (int)x.Quantity.Value
                                        ),
                                        unit: x.Quantity.Unit
                                    ),
                                    standard: x.Standard
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

        internal class CompleteAction : ActionStep
        {
            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        entity.State.Process(new PurchaseOrderCompletedVisitor()
                        {
                            Branch = session.Load<Branch>(_context.BranchId),
                            CompletedOn = DateTime.Now,
                            CompletedBy = _utils.Random<User>()
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

        internal class CancelledAction : ActionStep
        {
            protected override IEnumerable<PurchaseOrder> Process(IEnumerable<PurchaseOrder> input)
            {
                using (var session = this._sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    input = this.ReloadFromSession(input);
                    foreach (var entity in input)
                    {
                        entity.State.Process(new PurchaseOrderCancelledVisitor()
                        {
                            Branch = session.Load<Branch>(_context.BranchId),
                            CancelledOn = DateTime.Now,
                            CancelledBy = _utils.Random<User>()
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