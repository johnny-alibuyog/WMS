using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class DefaultUserSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public DefaultUserSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 11; }
        }

        public void Seed()
        {
            var item = new User(new Guid("{AD6C8DCF-BB0B-4923-A53D-725AEA8871CE}"))
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
                var entities = session.Query<User>().ToList();
                var roles = session.Query<Role>().ToList();
                var branch = session.Query<Branch>().FirstOrDefault();

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
