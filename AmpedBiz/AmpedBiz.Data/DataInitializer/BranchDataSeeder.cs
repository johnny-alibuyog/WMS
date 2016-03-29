using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.DataInitializer
{
    public class BranchDataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public BranchDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public int ExecutionOrder
        {
            get { return 10; }
        }

        public void Seed()
        {
            var data = new List<Branch>();

            for (int i = 0; i < 6; i++)
            {
                data.Add(new Branch()
                {
                    Id = $"branch{i}",
                    Name = $"Branch {i}",
                    Description = $"Description {i}",
                    Address = new Address()
                    {
                        Street = $"Street {i}",
                        Barangay = $"Barangay {i}",
                        City = $"City {i}",
                        Province = $"Province {i}",
                        Region = $"Region {i}",
                        Country = $"Country {i}",
                        ZipCode = $"Zip Code {i}"
                    }
                });
            }

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var users = session.Query<Branch>().ToList();
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
