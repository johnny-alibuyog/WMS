using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.Seeders
{
    public class CurrencyDataSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public CurrencyDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 1; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entity = session.Query<Currency>().ToList();
                if (entity.Count == 0)
                {
                    foreach (var item in Currency.All)
                    {
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }
    }
}
