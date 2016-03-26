using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.DataInitializer
{
    class ProductCategoryDataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public ProductCategoryDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
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
