using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _005_UserSeeder : IDummyDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _005_UserSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            var data = new List<User>();

            for (int i = 0; i < 8; i++)
            {
                data.Add(new User() //new User($"user{i}")
                {
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
                var entities = session.Query<User>().Cacheable().ToList();
                var roles = session.Query<Role>().Cacheable().ToList();
                var branch = session.Query<Branch>().Cacheable().ToList().FirstOrDefault();
                if (entities.Count == 0)
                {
                    foreach (var item in data)
                    {
                        item.Branch = branch;
                        item.Roles = roles;

                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
