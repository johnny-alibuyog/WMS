using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Branches
{
	public class CreateBranch
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
                    var exists = session.Query<Branch>().Any(x => x.Id == message.Id);
                    exists.Assert($"Branch with id {message.Id} already exists.");

                    var entity = message.MapTo(new Branch(message.Id));
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
