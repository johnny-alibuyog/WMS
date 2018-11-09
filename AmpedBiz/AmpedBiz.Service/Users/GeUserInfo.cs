using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using MediatR;
using System;

namespace AmpedBiz.Service.Users
{
	public class GetUserInfo
	{
		public class Request : IRequest<Response>
		{
			public Guid Id { get; set; }
		}

		public class Response : Dto.UserInfo { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entity = session.Get<User>(message.Id);

					entity.MapTo(response);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
