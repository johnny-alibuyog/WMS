using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.DataInitializer
{
    public class UserDataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public UserDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var data = new List<User>()
            {
                new User() { Id = "admin1", Username = "admin1", Password = "admin1" },
                new User() { Id = "admin2", Username = "admin2", Password = "admin2" }
            };

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<User>().ToList();
                if (users.Count == 0)
                {
                    foreach (var item in data)
                    {
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
