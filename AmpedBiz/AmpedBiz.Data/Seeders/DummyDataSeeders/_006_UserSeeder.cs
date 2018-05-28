using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Users;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DummyDataSeeders
{
    public class _006_UserSeeder : IDummyDataSeeder
    {
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _006_UserSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => false;

        public void Seed()
        {
            var data = new List<User>();

            for (int i = 0; i < 2; i++)
            {
                data.Add(new Func<User>(() =>
                {
                    var user = new User() //new User($"user{i}")
                    {
                        Username = $"Username{i}",
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
                    };

                    user.Accept(new SetPasswordVisitor()
                    {
                        NewPassword = $"Password{i}",
                        ConfirmPassword = $"Password{i}"
                    });

                    return user;
                })());
            }

            var context = this._contextProvider.Build();

            using (var session = _sessionFactory.RetrieveSharedSession(context))
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
                        item.Accept(new SetRoleVisitor(roles));
                        item.EnsureValidity();

                        session.Save(item);
                    }
                }

                transaction.Commit();
                _sessionFactory.ReleaseSharedSession();
            }
        }
    }
}
