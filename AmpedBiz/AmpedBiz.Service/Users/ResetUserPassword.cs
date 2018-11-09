using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Users;
using AmpedBiz.Core.Users.Services;
using AmpedBiz.Data;
using AmpedBiz.Data.Helpers;
using MediatR;

namespace AmpedBiz.Service.Users
{
	public class ResetUserPassword
	{
		public class Request : Dto.UserResetPassword, IRequest<Response> { }

		public class Response { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entity = session.Get<User>(message.Id);

					entity.EnsureExistence($"User with id {message.Id} does not exists.");

					entity.Accept(new ResetPasswordVisitor(new SettingsFacade(session)));

					entity.EnsureValidity();

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
