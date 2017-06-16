using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _011_DefaultUserSeeder : IDefaultDataSeeder
    {
        private readonly Utils _utils;
        private readonly ISessionFactory _sessionFactory;

        public _011_DefaultUserSeeder(ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), sessionFactory);
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<User>().Cacheable().ToList();
                var usersToInsert = User.All.Except(users);

                foreach (var user in usersToInsert)
                {
                    user.EnsureValidity();
                    session.Save(user);
                }

                transaction.Commit();
            }
        }
    }
}
