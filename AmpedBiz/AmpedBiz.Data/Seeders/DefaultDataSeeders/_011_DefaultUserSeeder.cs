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
                var user = session.Get<User>(User.SuperUser.Id);
                var branch = session.Get<Branch>(Branch.SuperBranch.Id);
                var roles = session.Query<Role>().Cacheable().ToList();

                if (user == null)
                {
                    user = User.SuperUser;
                    user.Branch = branch;
                    user.Roles = roles;

                    session.Save(user, user.Id);
                }

                transaction.Commit();
            }
        }
    }
}
