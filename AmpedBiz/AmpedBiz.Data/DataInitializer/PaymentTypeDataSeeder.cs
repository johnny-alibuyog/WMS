using System;
using System.Linq;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Data.DataInitializer
{
    public class PaymentTypeDataSeeder : IDataSeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public PaymentTypeDataSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public int ExecutionOrder
        {
            get { return 2; }
        }

        public void Seed()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                var paymentTypes = session.Query<PaymentType>().ToList();

                foreach(var item in PaymentType.All)
                {
                    if (!paymentTypes.Contains(item))
                        session.Save(item);
                }

                transaction.Commit();
            }
        }
    }
}
