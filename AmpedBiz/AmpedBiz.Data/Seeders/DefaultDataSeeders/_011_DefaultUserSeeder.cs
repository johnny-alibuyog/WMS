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
                var user = session.Get<User>(User.SupperUser.Id);
                var branch = session.Get<Branch>(Branch.SupperBranch.Id);
                var roles = session.Query<Role>().Cacheable().ToList();

                if (user == null)
                {
                    user.Branch = branch;
                    user.UserRoles = roles
                        .Select(x => new UserRole()
                        {
                            Role = x,
                            User = user
                        })
                        .ToList();

                    session.Save(user);
                }

                transaction.Commit();
            }
        }
    }
}
