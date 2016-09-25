using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Data.Seeders
{
    internal class Utils
    {
        private readonly Random _random = new Random();
        private readonly ISessionFactory _sessionFactory = SessionFactoryProvider.SessionFactory;

        public Utils(Random random = null, ISessionFactory sessionFactory = null)
        {
            _random = random ?? new Random();
            _sessionFactory = sessionFactory ?? SessionFactoryProvider.SessionFactory;
        }

        public T Random<T>()
        {
            var session = _sessionFactory.RetrieveSharedSession();
            var entities = session.Query<T>()
                .Cacheable()
                .ToArray();

            var randomIndex = _random.Next(0, entities.Count());
            return entities[randomIndex];
        }

        private Product[] _product;
        public Product RandomProduct()
        {
            // for some reason, product always hit database with a queary on 
            // the same session. let us put it in an container(_product) instead.
            if (_product == null)
            {
                var session = _sessionFactory.RetrieveSharedSession();
                _product = session.Query<Product>()
                    .Fetch(x => x.Inventory)
                    .ToArray();
            }

            var randomIndex = _random.Next(0, _product.Count());
            return _product[randomIndex];
        }

        public int Next()
        {
            return _random.Next();
        }

        public int Next(int max)
        {
            return _random.Next(max);
        }

        public int Next(int min, int max)
        {
            return _random.Next(min, max);
        }

        public decimal NextDecimal(decimal min, decimal max)
        {
            return _random.NextDecimal(min, max);
        }
    }
}
