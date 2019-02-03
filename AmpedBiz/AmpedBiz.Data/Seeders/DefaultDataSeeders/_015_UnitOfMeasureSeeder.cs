using AmpedBiz.Common.Configurations;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Data.Context;
using Humanizer;
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
        private readonly IContextProvider _contextProvider;
        private readonly ISessionFactory _sessionFactory;

        public _015_UnitOfMeasureSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
        {
            this._contextProvider = contextProvider;
            this._sessionFactory = sessionFactory;
        }

        public bool IsSourceExternalFile => true;

        public void Seed()
        {
            var context = this._contextProvider.Build();

            var items = (
                    this.GetUOMsFromDefaultFile(context)
                )
                .Concat(
                    this.GetUOMsFromProductList(context)
                )
                .Distinct()
				.ToList()
				.AsReadOnly();

            if (items.IsNullOrEmpty())
                return;

            using (var session = _sessionFactory.RetrieveSharedSession(context))
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

                _sessionFactory.ReleaseSharedSession();
            }
        }

        public IReadOnlyCollection<UnitOfMeasure> GetUOMsFromDefaultFile(IContext context)
        {
            var uoms = new List<UnitOfMeasure>();

            var filename = Path.Combine(DatabaseConfig.Instance.Seeder.ExternalFilesAbsolutePath, context.TenantId, @"default_uom.xlsx");

            if (File.Exists(filename))
            {
                var raw = new ExcelQueryFactory(filename)
                    .Worksheet()
                    .Select(x => new
                    {
                        Id = x["ID"],
                        Name = x["Name"]
                    })
                    .ToList();

                uoms = raw
                    .Where(x =>
                        !x.Id.Cast<string>().IsNullOrWhiteSpace() ||
                        !x.Name.Cast<string>().IsNullOrWhiteSpace()
                    )
                    .Select(x => new UnitOfMeasure(
                        x.Id.ToString().ToLowerInvariant(), 
                        x.Name.ToString().Titleize()
                    ))
                    .ToList();
            }

            return uoms;
        }

        public IReadOnlyCollection<UnitOfMeasure> GetUOMsFromProductList(IContext context)
        {
            var uoms = context
                .ExtractRawProducts()
                .ExtractUnitOfMeasures();

            return uoms;
        }
    }
}
