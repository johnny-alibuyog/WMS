using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.Seeders
{
    public class UnitOfMeasureClassSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public UnitOfMeasureClassSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool IsDummyData
        {
            get { return false; }
        }

        public int ExecutionOrder
        {
            get { return 7; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<UnitOfMeasureClass>().ToList();

                foreach (var item in UnitOfMeasureClass.All)
                {
                    if (!entities.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
