using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Users;
using AmpedBiz.Core.Users.Services;
using AmpedBiz.Data;
using MediatR;

namespace AmpedBiz.Service.Users
{
	public class Override
	{
		public class Request : Dto.User, IRequest<Response> { }

		public class Response : Dto.User { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var user = session.QueryOver<User>()
						.Where(x => x.Username == message.Username)
						.Fetch(x => x.Branch).Eager
						.Fetch(x => x.Roles).Eager
						.SingleOrDefault();


					user.Ensure(
						that: instance =>
						{
							if (instance == null)
								return false;

							var verfied = default(bool);

							instance.Accept(new VerifyPasswordVisitor()
							{
								Password = message.Password,
								ResultCallback = (result) => verfied = result
							});

							return verfied;
						},
						message: "Invalid user or password!"
					);

					user.IsManager().Assert("You do not have overriding rights.");
					user.MapTo(response);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
