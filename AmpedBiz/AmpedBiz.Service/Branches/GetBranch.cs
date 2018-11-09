using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Data;
using MediatR;
using System;

namespace AmpedBiz.Service.Branches
{
	public class GetBranch
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

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
                    entity.MapTo(response);

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
