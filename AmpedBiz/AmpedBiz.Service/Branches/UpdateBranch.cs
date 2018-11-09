using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Data;
using MediatR;

namespace AmpedBiz.Service.Branches
{
	public class UpdateBranch
    {
        public class Request : Dto.Branch, IRequest<Response> { }

        public class Response : Dto.Branch { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Branch>(message.Id);
                    entity.EnsureExistence($"Branch with id {message.Id} does not exists.");
                    entity.MapFrom(message);
                    entity.EnsureValidity();

                    transaction.Commit();

                    entity.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
