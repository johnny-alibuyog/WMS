using System;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.Seeders
{
    class ProductCategorySeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public ProductCategorySeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool DummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 3; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var paymentTypes = session.Query<ProductCategory>().ToList();

                foreach (var item in ProductCategory.All)
                {
                    if (!paymentTypes.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
