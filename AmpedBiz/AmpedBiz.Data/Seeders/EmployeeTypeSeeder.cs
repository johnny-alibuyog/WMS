using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class EmployeeTypeSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public EmployeeTypeSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool DummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 26; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<EmployeeType>().ToList();

                foreach (var item in EmployeeType.All)
                {
                    if (!entities.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
