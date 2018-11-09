using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using MediatR;

namespace AmpedBiz.Service.Users
{
	public class UpdateUserInfo
	{
		public class Request : Dto.UserInfo, IRequest<Response> { }

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

					entity.Person = message.Person.MapTo(default(Person));

					entity.EnsureValidity();

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
