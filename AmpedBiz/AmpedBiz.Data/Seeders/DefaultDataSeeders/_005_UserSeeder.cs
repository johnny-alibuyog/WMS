using AmpedBiz.Core.Users;
using AmpedBiz.Core.Users.Services;
using AmpedBiz.Data.Context;
using AmpedBiz.Data.Helpers;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Data.Seeders.DefaultDataSeeders
{
	public class _005_UserSeeder : IDefaultDataSeeder
	{
		private readonly IContextProvider _contextProvider;
		private readonly ISessionFactory _sessionFactory;

		public _005_UserSeeder(IContextProvider contextProvider, ISessionFactory sessionFactory)
		{
			_contextProvider = contextProvider;
			_sessionFactory = sessionFactory;
		}

		public bool IsSourceExternalFile => false;

		public void Seed()
		{
			var context = this._contextProvider.Build();

			using (var session = _sessionFactory.RetrieveSharedSession(context))
			using (var transaction = session.BeginTransaction())
			{
				var existingUsers = session.Query<User>().Cacheable().ToList();
				var settingsFacade = new SettingsFacade(session);

				var seed = User.All.Except(existingUsers);

				foreach (var user in seed)
				{
					user.Accept(new ResetPasswordVisitor(settingsFacade));
					user.EnsureValidity();
					session.Save(user, user.Id);
				}

				transaction.Commit();
				_sessionFactory.ReleaseSharedSession();
			}
		}
	}
}
