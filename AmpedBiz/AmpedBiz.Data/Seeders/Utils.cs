﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Data.Context;
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
        private readonly IContext _context;
        private readonly Random _random = new Random();
        private readonly ISessionFactory _sessionFactory = SessionFactoryProvider.SessionFactory;

        public Utils(Random random = null, IContext context = null, ISessionFactory sessionFactory = null)
        {
            _context = context ?? DefaultContext.Instance;
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

            query
                .FetchMany(x => x.Inventories)
                .ToFuture();

            var future = query
                .FetchMany(x => x.UnitOfMeasures)
                .ThenFetchMany(x => x.Prices)
                .ToFuture();

            return future.ToList();

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

        public IEnumerable<Product> RandomProducts()
        {
            return this.GetProducts();
        }

        public IEnumerable<Product> RandomShippedProducts()
        {
            return this.GetProducts(x =>
                //x.Inventories != null &&
                x.Inventories.Any(o => 
                    o.Branch.Id == this._context.BranchId &&
                    o.Shipped != null &&
                    o.Shipped.Value > 0
                )
            );
        }

        public IEnumerable<Product> RandomAvailableProducts()
        {
            return this.GetProducts(x =>
                //x.Inventories != null &&
                x.Inventories.Any(o =>
                    o.Branch.Id == this._context.BranchId &&
                    o.Available != null &&
                    o.Available.Value > 0
                )
            );
        }

        public IEnumerable<Product> RandomUninitializedProducts()
        {
            return this.GetProducts(x =>
                //x.Inventories != null &&
                x.Inventories.Any(o =>
                    o.Branch.Id == this._context.BranchId &&
                    o.OnHand == null ||
                    o.OnHand.Value <= 0
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
