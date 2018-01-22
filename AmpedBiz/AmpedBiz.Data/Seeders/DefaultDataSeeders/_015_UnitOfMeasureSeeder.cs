﻿using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
    public class _015_UnitOfMeasureSeeder : IDefaultDataSeeder
    {
        private readonly IContext _context;
        private readonly ISessionFactory _sessionFactory;

        public _015_UnitOfMeasureSeeder(DefaultContext context, ISessionFactory sessionFactory)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
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
            var items = (this.GetUOMsFromDefaultFile()).Concat(this.GetUOMsFromProductList()).Distinct();

            if (items.IsNullOrEmpty())
                return;

            using (var session = _sessionFactory.RetrieveSharedSession(_context))
            using (var transaction = session.BeginTransaction())
            {
                var entities = session.Query<UnitOfMeasure>().Cacheable().ToList();

                foreach (var item in items)
                {
                    if (!entities.Contains(item))
                    {
                        item.EnsureValidity();
                        session.Save(item);
                    }
                }

                transaction.Commit();
            }
        }

        public IEnumerable<UnitOfMeasure> GetUOMsFromDefaultFile()
        {
            var uoms = new List<UnitOfMeasure>();

            var filename = Path.Combine(DatabaseConfig.Instance.GetDefaultSeederDataAbsolutePath(), @"default_uom.xlsx");

            if (File.Exists(filename))
            {
                uoms = new ExcelQueryFactory(filename)
                    .Worksheet()
                    .Select(x => x["UOM"])
                    .Where(x => x != null)
                    .Select(x => new UnitOfMeasure(x, x))
                    .ToList();

            }

            return uoms;
        }

        public IEnumerable<UnitOfMeasure> GetUOMsFromProductList()
        {
            var uoms = new List<UnitOfMeasure>();

            var filename = Path.Combine(DatabaseConfig.Instance.GetDefaultSeederDataAbsolutePath(), @"default_products.xlsx");

            if (File.Exists(filename))
            {
                var raw = new ExcelQueryFactory(filename)
                    .Worksheet()
                    .Select(x => new
                    {
                        PieceUOM = x["Piece UOM"].ToString(),
                        PackageUOM = x["Package UOM"].ToString(),
                    })
                    .ToList();

                uoms = 
                    (
                        raw
                            .Where(x => !string.IsNullOrWhiteSpace(x.PieceUOM))
                            .Select(x => new UnitOfMeasure(id: x.PieceUOM, name: x.PieceUOM))
                    )
                    .Concat
                    (
                        raw
                            .Where(x => !string.IsNullOrWhiteSpace(x.PackageUOM))
                            .Select(x => new UnitOfMeasure(x.PackageUOM, x.PackageUOM))
                    )
                    .ToList();
            }

            return uoms;
        }
    }
}
