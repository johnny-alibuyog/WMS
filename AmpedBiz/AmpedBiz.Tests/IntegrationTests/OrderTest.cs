using AmpedBiz.Common.Configurations;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Dto.Mappers;
using AmpedBiz.Service.Tests.Configurations.Database;
using AmpedBiz.Tests.Configurations;
using NHibernate;
using NHibernate.Validator.Engine;
using NUnit.Framework;
using System;
using System.Linq;

namespace AmpedBiz.Tests.IntegrationTests
{
    [TestFixture]
    public class OrderTest
    {
        private ISessionFactory _sessionFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            //new Mapper().Initialze();

            this._sessionFactory = new SessionFactoryProvider(
                    validator: new ValidatorEngine(),
                    auditProvider: new AuditProvider()
                )
                .WithBatcher(BatcherConfiguration.Configure)
                .GetSessionFactory();

            var seeders = (
                from t in AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("AmpedBiz.Data")).GetTypes()
                where t.GetInterfaces().Contains(typeof(IDefaultDataSeeder))
                orderby t.Name
                select Activator.CreateInstance(t, this._sessionFactory) as IDefaultDataSeeder
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

        //[Test]
        //public void Test1()
        //{
        //    using (var session = this._sessionFactory.OpenSession())
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
