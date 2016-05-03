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
    public class OrderSeeder : ISeeder
    {
        private readonly ISessionFactory _sessionFactory;

        public OrderSeeder(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public bool DummyData
        {
            get { return true; }
        }

        public int ExecutionOrder
        {
            get { return 25; }
        }

        public void Seed()
        {
            var data = new List<Order>();

            for (int i = 0; i < 153; i++)
            {
                data.Add(new Order(
                ));
            }

            //using (var session = _sessionFactory.OpenSession())
            //using (var transaction = session.BeginTransaction())
            //{
            //    //session.SetBatchSize(100);

            //    var entity = session.Query<Order>().ToList();
            //    if (entity.Count == 0)
            //    {
            //        foreach (var item in data)
            //        {
            //            session.Save(item);
            //        }
            //    }

            //    transaction.Commit();
            //}
        }
    }
}
