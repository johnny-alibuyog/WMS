using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.DataInitializer
{
    public class UserSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public UserSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool DummyData
        {
            get { return true; }
        }

        public int ExecutionOrder
        {
            get { return 20; }
        }

        public void Seed()
        {
            var data = new List<User>();

            for (int i = 0; i < 8; i++)
            {
                data.Add(new User()
                {
                    Id = $"user{i}",
                    Username = $"Username{i}",
                    Password = $"Password{i}",
                    Person = new Person()
                    {
                        FirstName = $"FirstName {i}",
                        MiddleName = $"MiddleName {i}",
                        LastName = $"LastName {i}",
                        BirthDate = DateTime.UtcNow
                    },
                    Address = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    },
                });
            }

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<User>().ToList();
                var roles = session.Query<Role>().ToList();
                var branch = session.Query<Branch>().FirstOrDefault();
                if (users.Count == 0)
                {
                    foreach (var item in data)
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
                }

                transaction.Commit();
            }
        }
    }
}
