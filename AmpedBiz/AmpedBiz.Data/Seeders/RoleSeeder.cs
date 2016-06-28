using System;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class RoleSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public RoleSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 5; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<Role>().ToList();

                foreach (var item in Role.All)
                {
                    if (!entities.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
