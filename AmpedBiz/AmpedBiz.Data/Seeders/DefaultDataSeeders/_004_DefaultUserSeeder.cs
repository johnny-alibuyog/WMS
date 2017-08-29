using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _004_DefaultUserSeeder : IDefaultDataSeeder
    {
        private readonly Utils _utils;
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _004_DefaultUserSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            _utils = new Utils(new Random(), sessionFactory);
            _context = context;
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.RetrieveSharedSession(_context))
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<User>().Cacheable().ToList();
                var usersToInsert = User.All.Except(users);

                foreach (var user in usersToInsert)
                {
                    user.EnsureValidity();
                    session.Save(user, user.Id);
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}
