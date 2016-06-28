using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class PricingSchemeSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public PricingSchemeSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
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
                var entities = session.Query<PricingScheme>().ToList();

                foreach (var item in PricingScheme.All)
                {
                    if (!entities.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}