using System;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.DataInitializer
{
    public class RoleDataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public RoleDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
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
                var paymentTypes = session.Query<Role>().ToList();

                foreach (var item in Role.All)
                {
                    if (!paymentTypes.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
