﻿using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Tests.Bootstrap;
using Autofac;
using MediatR;
using NHibernate;
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
            var seeders = (
                from t in AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("AmpedBiz.Data")).GetTypes()
                where t.GetInterfaces().Contains(typeof(IDefaultDataSeeder))
                orderby t.Name
                select Activator.CreateInstance(t, DefaultContext.Instance, this._sessionFactory) as IDefaultDataSeeder
            );

            seeders.ToList().ForEach(x => x.Seed());
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
