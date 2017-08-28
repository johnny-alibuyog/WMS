using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

        private IEnumerable<Product> GetProducts(Expression<Func<Product, bool>> condition = null)
        {
            // for some reason, product always hit database with a query on 
            // the same session. let us put it in an container(_product) instead.

            var session = _sessionFactory.RetrieveSharedSession();

            var query = session.Query<Product>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query
                .Fetch(x => x.Inventory)
                .FetchMany(x => x.UnitOfMeasures)
                .ThenFetchMany(x => x.Prices)
                .ToList();

            //return (condition != null)
            //    ? this._products.Where(condition)
            //    : this._products.AsEnumerable();
        }


        public Product RandomProduct(Expression<Func<Product, bool>> condition = null)
        {
            var products = GetProducts(condition);

            var randomIndex = _random.Next(0, products.Count());

            return products
                .Select((x, i) => new { Index = i, Product = x })
                .Where(x => x.Index == randomIndex)
                .Select(x => x.Product)
                .FirstOrDefault();
        }

        public IEnumerable<Product> RandomWithShippedProducts()
        {
            return this.GetProducts(x =>
                x.Inventory != null &&
                x.Inventory.Shipped != null &&
                x.Inventory.Shipped.Value > 0
            );
        }

        public IEnumerable<Product> RandomAvailableProducts()
        {
            return this.GetProducts(x =>
                x.Inventory != null &&
                x.Inventory.Available != null &&
                x.Inventory.Available.Value > 0
            );
        }

        public IEnumerable<Product> RandomUninitializedProducts()
        {
            return this.GetProducts(x =>
                x.Inventory != null &&
                (
                    x.Inventory.OnHand == null ||
                    x.Inventory.OnHand.Value <= 0
                )
            );
        }

        public int RandomInteger()
        {
            return _random.Next();
        }

        public int RandomInteger(int max)
        {
            return _random.Next(max);
        }

        public int RandomInteger(int min, int max)
        {
            return _random.Next(min, max);
        }

        public decimal RandomDecimal(decimal min, decimal max)
        {
            return _random.NextDecimal(min, max);
        }

        //public decimal RandomDecimal(decimal min, decimal max)
        //{
        //    return _random.NextDecimal(min, max);
        //}

        public bool RandomBoolean()
        {
            return _random.NextDouble() > 0.5;
        }

        public string RandomString(int? length = null)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            length = length ?? chars.Length;
            return new string(Enumerable.Repeat(chars, length.Value)
              .Select(x => x[_random.Next(x.Length)]).ToArray());
        }
    }
}
