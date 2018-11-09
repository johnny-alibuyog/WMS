using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Returns;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.ReturnReasons
{
	public class CreateReturnReason
	{
		public class Request : Dto.ReturnReason, IRequest<Response> { }

		public class Response : Dto.ReturnReason { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var exists = session.Query<ReturnReason>().Any(x => x.Id == message.Id);
					exists.Assert($"Return Reason with id {message.Id} already exists.");

					var entity = message.MapTo(new ReturnReason(message.Id));
					entity.EnsureValidity();

					session.Save(entity);
					transaction.Commit();

					entity.MapTo(response);

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
