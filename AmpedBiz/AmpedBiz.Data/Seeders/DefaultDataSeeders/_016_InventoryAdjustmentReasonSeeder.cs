using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Seeders.DataProviders;
using LinqToExcel;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
	public class _016_InventoryAdjustmentReasonSeeder : IDefaultDataSeeder
	{
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _016_InventoryAdjustmentReasonSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			this._contextProvider = contextProvider;
			this._sessionFactory = sessionFactory;
		}

		public bool IsSourceExternalFile => true;

		public void Seed()
		{
			var context = this._contextProvider.Build();

			using (var session = this._sessionFactory.RetrieveSharedSession(context))
			using (var transaction = session.BeginTransaction())
			{
				var seed = InventoryAdjustmentReasonData.Get(context, session);

				if (!session.Query<InventoryAdjustmentReason>().Any())
				{
					seed.ForEach(item =>
					{
						item.EnsureValidity();
						session.Save(item);
					});
				}

				transaction.Commit();
				_sessionFactory.ReleaseSharedSession();
			}
		}
	}

	internal class InventoryAdjustmentReasonData : DataProvider<InventoryAdjustmentReason>
	{
		public static IEnumerable<InventoryAdjustmentReason> Get(IContext context, ISession session) => new InventoryAdjustmentReasonData(context, session).Get();

		public InventoryAdjustmentReasonData(IContext context, ISession session) : base(@"inventory_adjustment_reason.xlsx", context, session) { }

		public override InventoryAdjustmentReason Map(Row row)
		{
			return new InventoryAdjustmentReason(
				name: row[nameof(InventoryAdjustmentReason.Name)],
				description: row[nameof(InventoryAdjustmentReason.Description)],
				type: row[nameof(InventoryAdjustmentReason.Type)].As<InventoryAdjustmentType>()
			);
		}
	}
}
