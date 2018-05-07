using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _005_DefaultUserSeeder : IDefaultDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _005_DefaultUserSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            _contextProvider = contextProvider;
            _sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
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
