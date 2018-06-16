using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.Inventories;
using AmpedBiz.Tests.Bootstrap;
using Autofac;
using Common.Logging;
using MediatR;
using NHibernate;
using NUnit.Framework;
using System;
using System.Data.Common;
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
