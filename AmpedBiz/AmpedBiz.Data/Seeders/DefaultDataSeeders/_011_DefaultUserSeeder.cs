using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _011_DefaultUserSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _011_DefaultUserSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var item = new User(new Guid("{CA42947A-0BA3-4FC8-86E0-A635014B6B11}"))
            {
                Username = "supper_user",
                Password = "123!@#qweASD",
                Person = new Person()
                {
                    FirstName = "Supper",
                    MiddleName = "Power",
                    LastName = "User",
                    BirthDate = new DateTime(1999, 1, 1)
                },
                Address = new Address()
                {
                    Street = "Ocean Street",
                    Barangay = "Virginia Summer Ville, Mayamot",
                    City = "Antipolo City",
                    Province = "Rizal",
                    Region = "NCR",
                    Country = "Philippines",
                    ZipCode = "1870"
                },
            };

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<User>().Cacheable().ToList();
                var roles = session.Query<Role>().Cacheable().ToList();
                var branch = session.Query<Branch>().Cacheable().ToList().FirstOrDefault();

                if (!entities.Contains(item))
                {
                    item.Branch = branch;
                    item.UserRoles = roles
                        .Select(x => new UserRole()
                        {
                            Role = x,
                            User = item
                        })
                        .ToList();

                    session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
