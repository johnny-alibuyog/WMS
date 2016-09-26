using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _011_DefaultBranchSeeder : IDefaultDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public _011_DefaultBranchSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Get<Branch>("main_branch_001");
                if (entity == null)
                {
                    entity = new Branch("main_branch_001")
                    {
                        Name = "Main Branch",
                        Description = "Description",
                        Address = new Address()
                        {
                            Street = "Main Street",
                            Barangay = "Main Barangay",
                            City = "Main City",
                            Province = "Main Province",
                            Region = "Main Region",
                            Country = "Main Country",
                            ZipCode = "Main Zip Code"
                        }
                    };
                }

                session.Save(entity);
                transaction.Commit();
            }
        }
    }
}
