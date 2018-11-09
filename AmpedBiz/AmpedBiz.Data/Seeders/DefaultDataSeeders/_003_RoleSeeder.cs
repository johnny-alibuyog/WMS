using AmpedBiz.Core.Users;
using AmpedBiz.Data.Context;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
	public class _003_RoleSeeder : IDefaultDataSeeder
	{
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _003_RoleSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			this._contextProvider = contextProvider;
			this._sessionFactory = sessionFactory;
		}

		public bool IsSourceExternalFile => false;

		public void Seed()
		{
			var context = this._contextProvider.Build();

			using (var session = _sessionFactory.RetrieveSharedSession(context))
			using (var transaction = session.BeginTransaction())
			{
				var entities = session.Query<Role>().Cacheable().ToList();

				foreach (var item in Role.All)
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
	}
}
