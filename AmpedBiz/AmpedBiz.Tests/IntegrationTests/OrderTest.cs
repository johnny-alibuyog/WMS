using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Orders;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.Inventories;
using AmpedBiz.Tests.Bootstrap;
using Autofac;
using Common.Logging;
using MediatR;
using NHibernate;
using NHibernate.Transform;
using NUnit.Framework;
using System;
using System.Data.Common;
using System.Linq;
using System.Transactions;

namespace AmpedBiz.Tests.IntegrationTests
{
    [TestFixture]
    public class OrderTest
    {
        private readonly ISessionFactory _sessionFactory = Ioc.Container.Resolve<ISessionFactory>();
        private readonly IMediator _mediator = Ioc.Container.Resolve<IMediator>();

        [OneTimeSetUp]
        public void Setup()
        {
            var log = LogManager.GetLogger<OrderTest>();
            log.Error("log me like you do");

            var config = DatabaseConfig.Instance.Seeder;

            Ioc.Container.Resolve<IMapper>().Initialze();
            Ioc.Container.Resolve<Runner>().Run(config);
        }

        [Test]
        public void Test()
        {
            var state = StateConfig.Instance.Value;

            var order = new Order();

            for (int i = 0; i < 100; i++)
            {
                //order.AddOrderItem(new OrderItem() { ExtendedPrice = new Money(10) });
            }
        }

        [Test]
        public void Test1()
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    using (var session = this._sessionFactory.RetrieveSharedSession())
                    using (var transaction = session.BeginTransaction())
                    {
                        if (Transaction.Current != null)
                            ((DbConnection)session.Connection).EnlistTransaction(Transaction.Current);

                        session.Save(new Pricing("XX", "XXXXX"));

                        transaction.Commit();

                        _sessionFactory.ReleaseSharedSession();
                    }

                    //using (var session = this._sessionFactory.RetrieveSharedSession())
                    //using (var transaction = session.BeginTransaction())
                    //{
                    //    var XX = session.Get<Pricing>("XX");
                    //    XX.Name = "YYYY";

                    //    transaction.Commit();
                    //}

                    scope.Dispose();
                }
            }
            catch
            {

            }
            finally
            {
                using (var session = this._sessionFactory.RetrieveSharedSession())
                using (var transaction = session.BeginTransaction())
                {
                    var XX = session.Get<Pricing>("XX");
                    //Assert.IsNull(XX);
                }
            }
        }

        [Test]
        public void Test2()
        {
            var xxx = this._mediator.Send(new GetInventoryMovementsReportPage.Request());
            Console.WriteLine(xxx);
        }

        [Test]
        public void Test3()
        {
            using (var session = this._sessionFactory.RetrieveSharedSession(DefaultContext.Instance))
            using (var transaction = session.BeginTransaction())
            {
                //var entity = session.QueryOver<Order>()
                //    .Fetch(x => x.Branch).Eager
                //    .Fetch(x => x.Customer).Eager
                //    .Fetch(x => x.Pricing).Eager
                //    .Fetch(x => x.PaymentType).Eager
                //    .Fetch(x => x.Shipper).Eager
                //    .Fetch(x => x.Tax).Eager
                //    .Fetch(x => x.ShippingFee).Eager
                //    .Fetch(x => x.Discount).Eager
                //    .Fetch(x => x.SubTotal).Eager
                //    .Fetch(x => x.Total).Eager
                //    .Fetch(x => x.CreatedBy).Eager
                //    .Fetch(x => x.OrderedBy).Eager
                //    .Fetch(x => x.RoutedBy).Eager
                //    .Fetch(x => x.StagedBy).Eager
                //    .Fetch(x => x.InvoicedBy).Eager
                //    .Fetch(x => x.PaymentBy).Eager
                //    .Fetch(x => x.RoutedBy).Eager
                //    .Fetch(x => x.CompletedBy).Eager
                //    .Fetch(x => x.CancelledBy).Eager
                //    .Fetch(x => x.Items).Eager
                //    .Fetch(x => x.Items.First().Product).Eager
                //    .Fetch(x => x.Returns).Eager
                //    .Fetch(x => x.Returns.First().Product).Eager
                //    .Fetch(x => x.Payments).Eager
                //    .Fetch(x => x.Payments.First().PaymentBy).Eager
                //    .Fetch(x => x.Payments.First().PaymentType).Eager
                //    .SingleOrDefault();


                var orders = session.QueryOver<Order>()
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
                    .Fetch(x => x.PaymentBy).Eager
                    .Fetch(x => x.RoutedBy).Eager
                    .Fetch(x => x.CompletedBy).Eager
                    .Fetch(x => x.CancelledBy).Eager
                    .Fetch(x => x.Items).Eager
                    .Fetch(x => x.Items.First().Product).Eager
                    .Fetch(x => x.Items.First().Product.Inventories).Eager
                    .Fetch(x => x.Items.First().Product.UnitOfMeasures).Eager
                    .Fetch(x => x.Items.First().Product.UnitOfMeasures.First().Prices).Eager
                    .Fetch(x => x.Payments).Eager
                    .Fetch(x => x.Payments.First().PaymentBy).Eager
                    .Fetch(x => x.Payments.First().PaymentType).Eager
                    .Fetch(x => x.Returns).Eager
                    .Fetch(x => x.Returns.First().Reason).Eager
                    .Fetch(x => x.Returns.First().ReturnedBy).Eager
                    .Fetch(x => x.Returns.First().Product).Eager
                    .Fetch(x => x.Returns.First().Product.Inventories).Eager
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .List();

                transaction.Commit();

                Console.Write(orders);
            }
        }

        //[Test]
        //public void Test1()
        //{
        //    using (var session = this._sessionFactory.RetrieveSharedSession(_context))
        //    using (var transaction = session.BeginTransaction())
        //    {
        //        //var entity = new ProductCategory("xxx");
        //        //var entity = session.Get<ProductCategory>(ProductCategory.Drinks.Id);

        //        //entity.Name = null;

        //        //var invalidValues = SessionFactoryProvider.Validator.Validate(entity);

        //        var entity = new Order();
        //        entity.EnsureValidity();

        //        session.Save(new Order());

        //        transaction.Commit();
        //    }
        //}

    }
}
