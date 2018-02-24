using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetInitialUser
    {
        public class Request : IRequest<Response> { }

        public class Response : Dto.User { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var roles = session.Query<Role>().Cacheable().ToList();

                    response.Roles = roles
                        .Select(x => new Dto.Role()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Assigned = false,
                        })
                        .ToList();

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
